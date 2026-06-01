using Maia.Data.Interface;
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

builder.Services.AddScoped<ICardsWomenService, CardsWomenService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IOrderService, OrderService>();
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