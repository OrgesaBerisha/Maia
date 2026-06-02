using Maia.Data.Interface;
using Maia.Data.Repository;
using Maia.Data.Repository.Interface;
using Maia.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// DbContext (FIXED - ignore PendingModelChangesWarning)
builder.Services.AddDbContext<Maia.Data.DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .ConfigureWarnings(w =>
               w.Ignore(RelationalEventId.PendingModelChangesWarning)
           )
);

builder.Services.AddControllers();

<<<<<<< Updated upstream
builder.Services.AddScoped<ICardsWomenService, CardsWomenService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IOrderService, OrderService>();
=======
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173",
                "http://localhost:5174",
                "http://localhost:5182"
              )
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Repositories
builder.Services.AddScoped<ICardsWomenRepository, CardsWomenRepository>();
builder.Services.AddScoped<ICartRepository,        CartRepository>();
builder.Services.AddScoped<IWishlistRepository,    WishlistRepository>();
builder.Services.AddScoped<IOrderRepository,       OrderRepository>();

// Services
builder.Services.AddScoped<ICardsWomenService, CardsWomenService>();
builder.Services.AddScoped<ICartService,       CartService>();
builder.Services.AddScoped<IWishlistService,   WishlistService>();
builder.Services.AddScoped<IOrderService,      OrderService>();

>>>>>>> Stashed changes
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();