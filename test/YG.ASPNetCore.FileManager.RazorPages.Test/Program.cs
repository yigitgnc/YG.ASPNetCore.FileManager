using YG.ASPNetCore.FileManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

// YG.AspNetCore.FileManager -------
builder.Services.AddYGfilemanager();
//-----------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// YG.AspNetCore.FileManager -------
app.UseYGfilemanager();
//-----------------------------------

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

// to support controllers -----------
app.MapControllerRoute("default", "api/{controller=Home}/{action=Index}/{id?}");
app.MapControllers();
//-----------------------------------

app.Run();
