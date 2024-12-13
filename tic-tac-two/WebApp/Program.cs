using DAL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

connectionString = connectionString.Replace("<%location%>", FileHelper.BasePath);


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));

//builder.Services
//.AddTransient<>(); - create new one every time
//.AddSingleton<>(); - create new one on first try, all the next requests get existing
//.AddScoped<>(); - create new one for every web request

//builder.Services.AddScoped<IGameRepository, GameRepositoryJson>();
//builder.Services.AddScoped<IConfigRepository, ConfigRepositoryJson>();
builder.Services.AddScoped<IConfigRepository, ConfigRepositoryDb>();
builder.Services.AddScoped<IGameRepository, GameRepositoryDb>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
} else {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapStaticAssets(); 
app.UseStaticFiles(); 
app.UseRouting();
app.UseAuthorization();
app.MapRazorPages().WithStaticAssets();
app.Run();
