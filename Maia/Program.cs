using Maia.Data.Interface;
using Maia.Data.Persistence;
using Maia.Data.Repository.NoSQL;
using Maia.Services;
using Microsoft.EntityFrameworkCore;

DotNetEnv.Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<Maia.Data.DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Add services to the container.
builder.Services.AddControllers();

// 🔥 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IKidsViewAllCards, KidsViewAllCardsService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// MongoDB
builder.Services.AddSingleton<MongoDbContext>();

// Redis
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING")
        ?? builder.Configuration["Redis:ConnectionString"];
});

// MemoryCache (për product cache në repository)
builder.Services.AddMemoryCache();

// Repositories — NoSQL
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors("FrontendPolicy");

app.UseAuthorization();

app.MapControllers();

// Serve the reset password HTML page at GET /reset?token=...
app.MapGet("/reset", (string? token) =>
{
    var html = $$"""
<!DOCTYPE html>
<html>
<head>
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1">
  <title>Reset Password — MAIA</title>
  <style>
    * { box-sizing: border-box; margin: 0; padding: 0; }
    body { font-family: Georgia, serif; background: #fff; display: flex; align-items: center; justify-content: center; min-height: 100vh; }
    .card { width: 100%; max-width: 400px; padding: 48px 40px; }
    h1 { font-size: 42px; font-style: italic; font-weight: 400; letter-spacing: 3px; color: #1C0A06; text-align: center; margin-bottom: 8px; }
    .sub { font-size: 11px; letter-spacing: 3px; color: #9E8E88; text-align: center; margin-bottom: 36px; }
    label { font-size: 10px; letter-spacing: 1.5px; color: #9E8E88; display: block; margin-bottom: 4px; }
    input { width: 100%; border: none; border-bottom: 1px solid #CCC0BB; padding: 8px 0; font-size: 13px; color: #1C0A06; outline: none; background: transparent; margin-bottom: 24px; }
    input:focus { border-bottom-color: #1C0A06; }
    button { width: 100%; background: #1C0A06; color: #fff; border: none; padding: 14px; font-size: 11px; letter-spacing: 2px; cursor: pointer; margin-bottom: 16px; }
    button:hover { background: #3a1a10; }
    .msg { font-size: 12px; text-align: center; margin-top: 8px; }
    .ok { color: #2E7D32; } .err { color: #C62828; }
  </style>
</head>
<body>
<div class="card">
  <h1>MAIA</h1>
  <p class="sub">RESET PASSWORD</p>
  <form id="f">
    <label>NEW PASSWORD</label>
    <input type="password" id="pw" placeholder="Enter new password" required minlength="6">
    <label>CONFIRM PASSWORD</label>
    <input type="password" id="pw2" placeholder="Confirm new password" required minlength="6">
    <button type="submit">SET NEW PASSWORD</button>
  </form>
  <p class="msg" id="msg"></p>
</div>
<script>
  document.getElementById('f').onsubmit = async e => {
    e.preventDefault();
    const pw = document.getElementById('pw').value;
    const pw2 = document.getElementById('pw2').value;
    const msg = document.getElementById('msg');
    if (pw !== pw2) { msg.className='msg err'; msg.textContent='Passwords do not match.'; return; }
    const r = await fetch('/api/auth/reset-password', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ token: '{{token}}', newPassword: pw })
    });
    const d = await r.json();
    if (r.ok) { msg.className='msg ok'; msg.textContent='Password reset! You can now log in.'; document.getElementById('f').style.display='none'; }
    else { msg.className='msg err'; msg.textContent = d.message || 'Something went wrong.'; }
  };
</script>
</body>
</html>
""";
    return Results.Content(html, "text/html");
});

app.Run();