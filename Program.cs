using Electric_Prices.Interfaces;
using Electric_Prices.Services;
using Electric_Prices.Models;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<Electric_Prices.Models.Electric_Prices_Context>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("Electric_Prices_Db")));

builder.Services.AddScoped<IRetrieveData, RetrieveData>();
builder.Services.AddTransient<RetrieveData>();

var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<Electric_Prices.Models.Electric_Prices_Context>();
    context.Database.EnsureCreated();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseAuthorization();

app.MapRazorPages();

app.Run();




