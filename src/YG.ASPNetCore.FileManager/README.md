# YG.ASPNetCore.FileManager (Free & Open Source File Explorer for ASP.Net Core 6+)

#### YG.ASPNetCore.FileManager is an "enhanced" fork of HGO.ASPNetCore.FileManager free, open source, feature rich and easy to use file explorer/manager component for ASP.Net Core 6 and above with MIT license!

[![NuGet version (YG.ASPNetCore.FileManager)](https://img.shields.io/nuget/v/YG.ASPNetCore.FileManager)](https://www.nuget.org/packages/YG.ASPNetCore.FileManager/)  
![NuGet Downloads](https://img.shields.io/nuget/dt/YG.ASPNetCore.FileManager?style=flat&color=%23238636)

# **[Online Demo](https://filemanager.yigitgenc.com/)**

![<img alt="Deployed with FTP Deploy Action" src="https://img.shields.io/badge/Deployed With-FTP DEPLOY ACTION-%3CCOLOR%3E?style=for-the-badge&color=0077b6">](https://github.com/SamKirkland/FTP-Deploy-Action)[](url)

![YG.ASPNetCore.FileManager](https://raw.githubusercontent.com/yigitgnc/YG.ASPNetCore.FileManager/master/YG.ASPNetCore.FileManager.png "YG.ASPNetCore.FileManager")

## Features:
-  File Encryption (The primary goal of encryption is to protect your files in the event of a data breach, whether caused by a vulnerability in the hosting provider or by direct file access attempts.)
-  Multiple Language Support with fully customizable language option
-  Enum Member Access for type safety
-  Manage server's files from client side
-  **IMPROVED** Copy & cut & paste functionality
-  Compress & extract archive files (Rar, Zip, Tar, Tar.GZip, Tar.BZip2, Tar.LZip, Tar.XZ, GZip, 7Zip)
-  Download & upload
-  Rename & delete files/folders
-  Edit text based files with full feature code editor ([CodeMirror](https://codemirror.net/)) (e.g.: .css, .html, .js, ...)
-  Create new file/folder
-  Cross platform (Compatible with Windows & Linux & macOS file system)
-  Search
-  Ability to enable/disable features
-  Ability to control disk space usage
-  and more ...

![YG.ASPNetCore.FileManager Light Mode](https://raw.githubusercontent.com/yigitgnc/YG.ASPNetCore.FileManager/master/Light-min.png "YG.ASPNetCore.FileManager Light Mode")
![YG.ASPNetCore.FileManager Dark Mode](https://raw.githubusercontent.com/yigitgnc/YG.ASPNetCore.FileManager/master/Dark-min.png"YG.ASPNetCore.FileManager Dark Mode")

## How to Install:
At first you should install  [YG.ASPNetCore.FileManager from NuGet](https://www.nuget.org/packages/YG.ASPNetCore.FileManager/):
```
Install-Package YG.ASPNetCore.FileManager
```
Or via the .NET Core command line interface:

```cs
dotnet add package YG.ASPNetCore.FileManager
```
Either commands, from Package Manager Console or .NET Core CLI, will download and install YG.ASPNetCore.FileManager and all required dependencies.
Now you need to add YG.ASPNetCore.FileManager to the ASP.NET Core services container. Open `Program.cs` and insert the marked lines into `Program.cs` file:
```cs
using YG.ASPNetCore.FileManager;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// YG.AspNetCore.FileManager -------
builder.Services.AddYGFileManager();
//-----------------------------------

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// YG.AspNetCore.FileManager -------
app.UseYGFileManager();
//-----------------------------------

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```
Now you need to create an `Action Method` to handle server side operations, so open (or create) a `Controller` class and add the following action method:
```cs
[HttpPost, HttpGet]
public async Task<IActionResult> YgApi(string id, Command command, string parameters, IFormFile file)
{
    return await _processor.ProcessCommandAsync(id, command, parameters, file);
}
```
Also you need to inject `IFileManagerCommandsProcessor` to you controller contractor method with IoC, so edit your controller contractor method like below:
```cs
private readonly IFileManagerCommandsProcessor _processor;

public HomeController(IFileManagerCommandsProcessor processor)
{
    _processor = processor;
}
```
Now you can add `YG.ASPNetCore.FileManager` component view to any razor page or view you want:
```cs
<div style="height: 550px; margin-bottom:20px">
        @await Component.InvokeAsync("FileManagerComponent", new FileManagerModel(){
            Id = "FM1",
            RootFolder = "files",
            ApiEndPoint = Url.Action("YgApi"),
            Config = new FileManagerConfig(){
                UseEncryption = true,
                UseRecycleBin = true,
                EncryptionKey = "<Your Super Secret Encryption Key>",
                DisabledFunctions = { Command.Delete, Command.Rename },
                Language = new TurkishLanguage(),                
            }
        })
</div>
```
Also you need to reference YG.ASPNetCore.FileManager JavaScript and CSS files to your view:
```cshtml
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    
    @*YG.AspNetCore.FileManager Styles*@
    @this.RenderYGFileManagerCss(true)
    @*---------------------------------*@
</head>
<body>
    <div class="container">
        <main role="main" class="pb-3">
        @await Component.InvokeAsync("FileManagerComponent", new FileManagerModel(){
            Id = "FM1",
            RootFolder = "files",
            ApiEndPoint = Url.Action("YgApi"),
            Config = new FileManagerConfig(){
                UseEncryption = true,
                UseRecycleBin = true,
                EncryptionKey = "<Your Super Secret Encryption Key>",
                DisabledFunctions = { Command.Delete, Command.Rename },
                Language = new TurkishLanguage(),                
            }
        })
        </main>
    </div>

    @*YG.AspNetCore.FileManager depends on jQuery, so you need to add jQuery reference before (If you don't it will automatically add jquery in newer versions) *@
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    
    @*----------------------------------*@

    @*YG.AspNetCore.FileManager Scripts*@
    @this.RenderYGFileManagerJavaScripts()
    @*----------------------------------*@
</body>
</html>
```
For more information please check the following sample projects:
- [ASP.Net Core MVC](https://github.com/yigitgnc/YG.ASPNetCore.FileManager/tree/master/test/YG.ASPNetCore.FileManager.Test)
- [ASP.Net Core Razor Pages](https://github.com/yigitgnc/YG.ASPNetCore.FileManager/tree/master/test/YG.ASPNetCore.FileManager.RazorPages.Test)

### Note:
YG.ASPNetCore.FileManager depends on jQuery library, so you need reference jQuery library before calling `RenderYGFileManagerJavaScripts()` (If you don't it will automatically add jquery in newer versions)

### Third-party JS libraries which I used in this project:
- [context-js](https://github.com/heapoverride/context-js)
- [dropzone](https://github.com/dropzone/dropzone)
- [Font Awesome](https://github.com/FortAwesome/Font-Awesome)
- [js-cookie](https://github.com/js-cookie/js-cookie)
- [jsTree](https://github.com/vakata/jstree)
- [Lozad.js](https://github.com/ApoorvSaxena/lozad.js)
- [Split.js](https://github.com/nathancahill/split)
- [toastify-js](https://github.com/apvarun/toastify-js)
- [viSelection](https://github.com/simonwep/selection)

Thanks to all!

