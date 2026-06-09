using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

[ApiController]
[Route("api/chat")]
[Authorize]
public class SendMessageDto
{
    public string Content { get; set; } = string.Empty;
    public string? SenderName { get; set; }
}

public class ChatController : ControllerBase
{
    private readonly NotificationDbContext _db;
    private readonly IHubContext<ChatHub> _hub;
    private readonly IConfiguration _config;

    public ChatController(NotificationDbContext db, IHubContext<ChatHub> hub, IConfiguration config)
    {
        _db = db;
        _hub = hub;
        _config = config;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";
    private string UserName => User.FindFirstValue(ClaimTypes.Name)
                               ?? User.FindFirstValue("name") ?? "User";
    private string UserEmail => User.FindFirstValue(ClaimTypes.Email) ?? "";
    private string Role => User.FindFirstValue(ClaimTypes.Role) ?? "";
    private bool IsStaff => Role is "Admin" or "SalesManager" or "WomenManager" or "MenManager" or "KidsManager";

    [HttpPost("conversations")]
    [AllowAnonymous]
    public async Task<IActionResult> StartConversation()
    {
        var uid   = UserId.Length > 0 ? UserId : $"guest-{Guid.NewGuid():N}";
        var uname = UserName.Length > 0 && UserName != "User" ? UserName : "Guest";

        var existing = await _db.ChatConversations
            .FirstOrDefaultAsync(c => c.UserId == uid && c.Status == "Open");

        if (existing != null)
            return Ok(existing);

        var conv = new ChatConversation
        {
            UserId    = uid,
            UserName  = uname,
            UserEmail = UserEmail,
        };
        _db.ChatConversations.Add(conv);
        await _db.SaveChangesAsync();
        return Ok(conv);
    }

    [HttpGet("conversations/{id:int}/messages")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMessages(int id)
    {
        var msgs = await _db.ChatMessages
            .Where(m => m.ConversationId == id)
            .OrderBy(m => m.SentAt)
            .ToListAsync();
        return Ok(msgs);
    }

    [HttpPost("conversations/{id:int}/messages")]
    [AllowAnonymous]
    public async Task<IActionResult> SendMessage(int id, [FromBody] SendMessageDto dto)
    {
        var conv = await _db.ChatConversations.FindAsync(id);
        if (conv == null) return NotFound();

        var uid   = UserId.Length > 0 ? UserId : $"guest-{id}";
        var uname = UserName.Length > 0 && UserName != "User" ? UserName : dto.SenderName ?? "Guest";

        // Save customer message
        var userMsg = new ChatMessage
        {
            ConversationId = id,
            SenderId       = uid,
            SenderName     = uname,
            IsStaff        = false,
            Content        = dto.Content,
            SentAt         = DateTime.UtcNow,
        };
        _db.ChatMessages.Add(userMsg);
        conv.LastMessageAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        // Get chat history for context
        var history = await _db.ChatMessages
            .Where(m => m.ConversationId == id)
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        // Call Claude AI for response
        var aiReply = await GetAiResponse(history, dto.Content);

        // Save AI response
        var aiMsg = new ChatMessage
        {
            ConversationId = id,
            SenderId       = "ai",
            SenderName     = "Maia Assistant",
            IsStaff        = true,
            Content        = aiReply,
            SentAt         = DateTime.UtcNow,
        };
        _db.ChatMessages.Add(aiMsg);
        await _db.SaveChangesAsync();

        return Ok(new { userMessage = userMsg, aiMessage = aiMsg });
    }

    [HttpGet("conversations")]
    public async Task<IActionResult> GetAll()
    {
        if (!IsStaff) return Forbid();
        var convs = await _db.ChatConversations
            .OrderByDescending(c => c.LastMessageAt)
            .ToListAsync();
        return Ok(convs);
    }

    [HttpPatch("conversations/{id:int}/close")]
    public async Task<IActionResult> Close(int id)
    {
        if (!IsStaff) return Forbid();
        var conv = await _db.ChatConversations.FindAsync(id);
        if (conv == null) return NotFound();
        conv.Status = "Closed";
        await _db.SaveChangesAsync();
        return Ok();
    }

    private async Task<string> GetAiResponse(List<ChatMessage> history, string latestMessage)
    {
        var apiKey = _config["Groq:ApiKey"] ?? "";
        if (!string.IsNullOrEmpty(apiKey) && apiKey != "YOUR_GROQ_API_KEY")
        {
            try
            {
                var messages = new List<object>
                {
                    new { role = "system", content = "You are a customer support assistant for Maia, a fashion e-commerce store with Women, Men, and Kids sections. Keep answers SHORT (2-3 sentences max). NEVER invent specific products, prices, or stock. If asked about specific products or prices, tell the user to browse the shop. Help with: general questions about the store, how to navigate the site, orders, shipping (€4.99, 3-5 days), returns (30 days), and payment (cash or card). Be friendly and direct." }
                };
                foreach (var m in history.Where(m => m.SenderId != "ai"))
                    messages.Add(new { role = m.IsStaff ? "assistant" : "user", content = m.Content });

                var body = JsonSerializer.Serialize(new { model = "llama-3.1-8b-instant", messages, max_tokens = 300 });
                using var http = new HttpClient();
                http.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
                var res  = await http.PostAsync("https://api.groq.com/openai/v1/chat/completions", new StringContent(body, Encoding.UTF8, "application/json"));
                var json = await res.Content.ReadAsStringAsync();
                using var doc = JsonDocument.Parse(json);
                return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString() ?? "How can I help you?";
            }
            catch { }
        }

        // Built-in smart fallback — no API key needed
        return SmartReply(latestMessage.ToLower());
    }

    private static string SmartReply(string msg)
    {
        if (msg.Contains("kids") || msg.Contains("children") || msg.Contains("baby"))
            return "We have a great Kids section with Boys, Girls, and Baby categories! You can find dresses, tops, trousers, shoes and more. Check it out in the Shop section! 👶";
        if (msg.Contains("women") || msg.Contains("woman") || msg.Contains("dress") || msg.Contains("female"))
            return "Our Women's section has a beautiful collection of dresses, tops, skirts and more from top brands. Visit the Women section in our shop! 👗";
        if (msg.Contains("men") || msg.Contains("man") || msg.Contains("male"))
            return "Our Men's section features stylish shirts, trousers, jackets and more. Check out the Men section in our shop! 👔";
        if (msg.Contains("order") || msg.Contains("track") || msg.Contains("status"))
            return "You can track your orders in the Profile → My Purchases section. If you need more help, please provide your order number! 📦";
        if (msg.Contains("return") || msg.Contains("refund") || msg.Contains("exchange"))
            return "We accept returns within 30 days of purchase. Please contact us with your order number and reason for return and we'll sort it out! 🔄";
        if (msg.Contains("shipping") || msg.Contains("delivery") || msg.Contains("ship"))
            return "We offer standard shipping for €4.99. Orders are typically delivered within 3-5 business days. 🚚";
        if (msg.Contains("size") || msg.Contains("fit") || msg.Contains("measurement"))
            return "Each product page has a size guide. If you're between sizes, we recommend sizing up. Feel free to ask about a specific item! 📏";
        if (msg.Contains("price") || msg.Contains("discount") || msg.Contains("sale") || msg.Contains("offer"))
            return "We regularly have sales and discounts! Check our Shop page for items with sale badges. You can also filter by 'On Sale' to see all discounted items! 🏷️";
        if (msg.Contains("payment") || msg.Contains("pay") || msg.Contains("card") || msg.Contains("cash"))
            return "We accept Cash on Delivery and Credit/Debit Cards (Visa, Mastercard). All card payments are secured via Stripe. 💳";
        if (msg.Contains("hello") || msg.Contains("hi") || msg.Contains("hey") || msg.Contains("helo"))
            return "Hello! Welcome to Maia Fashion Support! 👋 How can I help you today? You can ask me about our products, orders, shipping, returns or anything else!";
        if (msg.Contains("thank") || msg.Contains("thanks") || msg.Contains("bye"))
            return "You're welcome! If you need anything else, don't hesitate to ask. Happy shopping at Maia! 🛍️";
        if (msg.Contains("contact") || msg.Contains("store") || msg.Contains("location"))
            return "You can find our store locations in the Stores page. For direct contact, you can reach us through this chat! 📍";

        return "Thank you for your question! I'm here to help with anything related to our products, orders, shipping, or returns. Could you give me more details? 😊";
    }
}
