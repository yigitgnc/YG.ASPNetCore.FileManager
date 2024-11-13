using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using YG.ASPNetCore.FileManager.CommandsProcessor;
using YG.ASPNetCore.FileManager.ViewComponents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace YG.ASPNetCore.FileManager
{
    public static class Extensions
    {
        public static void AddYGfilemanager(this IServiceCollection services)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation(c => c.FileProviders.Add(new EmbeddedFileProvider(typeof(FileManagerComponent)
                .GetTypeInfo().Assembly, "YG.ASPNetCore.FileManager")));
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IFileManagerCommandsProcessor), typeof(FileManagerCommandsProcessor));
            services.AddSingleton(HtmlEncoder.Create(allowedRanges: new[] { UnicodeRanges.BasicLatin, UnicodeRanges.Latin1Supplement, UnicodeRanges.LatinExtendedA }));
        }

        public static void UseYGfilemanager(this IApplicationBuilder builder)
        {
            var embeddedProvider = new EmbeddedFileProvider(typeof(FileManagerComponent)
                .GetTypeInfo().Assembly, "YG.ASPNetCore.FileManager.ygfilemanager");

            builder.UseSession();
            builder.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = embeddedProvider,
                RequestPath = new PathString("/ygfilemanager")
            });
        }

        public static HtmlString RenderYGfilemanagerJavaScripts(this RazorPageBase razorPage, string[]? exclude = null)
        {
            var excludelist = new List<string>();
            if (exclude is { Length: > 0 })
            {
                excludelist.AddRange(exclude.Select(e => e.ToLower())); // Normalize to lowercase for easier comparison
            }

            // Convert the exclusion list to a JavaScript array
            var excludeArrayJs = string.Join(", ", excludelist.Select(e => $"'{e}'"));

            var scripts = $@"
    <script>
        window.scriptsLoaded = false;
        var excludeList = [{excludeArrayJs}]; // Exclusion list passed from C#

        if (typeof jQuery === 'undefined') {{var script = document.createElement('script');
            script.src = '/ygfilemanager/jquery/dist/jquery.min.js';
            script.onload = function() {{loadLibraries();
            }};
            document.head.appendChild(script);
        }} else {{loadLibraries();
        }}

        function loadLibraries() {{
            var librariesToLoad = [
                {{ src: '/ygfilemanager/js.cookie/js.cookie.js', callback: onJsCookieLoaded }},
                {{ src: '/ygfilemanager/fontawesome/js/fontawesome.min.js', callback: onFontAwesomeLoaded }},
                {{ src: '/ygfilemanager/lozad.js/lozad.min.js', callback: onLozadLoaded }},
                {{ src: '/ygfilemanager/jstree/jstree.min.js', callback: onJstreeLoaded }},
                {{ src: '/ygfilemanager/viselect/viselect.js', callback: onViselectLoaded }},
                {{ src: '/ygfilemanager/split.js/split.js', callback: onSplitLoaded }},
                {{ src: '/ygfilemanager/dropzone/dropzone.js', callback: onDropzoneLoaded }},
                {{ src: '/ygfilemanager/context-js/context/context.min.js', callback: onContextJsLoaded }},
                {{ src: '/ygfilemanager/toastify/toastify.js', callback: onToastifyLoaded }}
            ];

            loadNextLibrary(librariesToLoad, 0);
        }}

        function loadNextLibrary(libraries, index) {{
            if (index < libraries.length) {{
                var library = libraries[index];
                // Check if the library is excluded
                if (!excludeList.includes(library.src.split('/').pop().toLowerCase())) {{
                    var script = document.createElement('script');
                    script.src = library.src;
                    script.onload = function() {{
                        if (library.callback) {{
                            library.callback();
                        }}
                        loadNextLibrary(libraries, index + 1); // Load the next library
                    }};
                    document.head.appendChild(script);
                }} else {{
                    loadNextLibrary(libraries, index + 1); // Skip this library and load the next
                }}
            }} else {{
                window.scriptsLoaded = true; // All libraries loaded
                onAllLibrariesLoaded(); // You can call a function here that needs all libraries
            }}
        }}

        // Callbacks for individual libraries
        function onJsCookieLoaded() {{
            console.log('js-cookie loaded');
        }}

        function onFontAwesomeLoaded() {{
            console.log('Font Awesome loaded');
        }}

        function onLozadLoaded() {{
            console.log('Lozad.js loaded');
        }}

        function onJstreeLoaded() {{
            console.log('jsTree loaded');
        }}

        function onViselectLoaded() {{
            console.log('viSelect loaded');
        }}

        function onSplitLoaded() {{
            console.log('split.js loaded');
        }}

        function onDropzoneLoaded() {{
            console.log('Dropzone.js loaded');
        }}

        function onContextJsLoaded() {{
            console.log('context-js loaded');
        }}

        function onToastifyLoaded() {{
            console.log('Toastify.js loaded');
        }}

        function onAllLibrariesLoaded() {{
            console.log('All libraries loaded');
            // This is where you can execute any code that depends on all libraries being loaded
        }}
    </script>";

            return new HtmlString(scripts);
        }


        public static HtmlString RenderYGfilemanagerCss(this RazorPageBase razorPage, bool darkMode = false, string[]? exclude = null)
        {
            var excludelist = new List<string>();
            if (exclude is { Length: > 0 })
            {
                excludelist.AddRange(exclude);
            }

            var styles = "";
            if (!excludelist.Any(p => p.ToLower() == "jstree"))
            {
                styles +=
                    "<link rel='stylesheet' href='/ygfilemanager/jstree/themes/default/style.min.css' type='text/css' />\r\n";
            }
            if (!excludelist.Any(p => p.ToLower() == "dropzone"))
            {
                styles +=
                    "<link rel='stylesheet' href='/ygfilemanager/dropzone/dropzone.css' type='text/css' />\r\n";
            }
            if (!excludelist.Any(p => p.ToLower() == "fontawesome"))
            {
                styles +=
                    "<link rel='stylesheet' href='/ygfilemanager/fontawesome/css/all.min.css' type='text/css' />\r\n";
            }
            if (!excludelist.Any(p => p.ToLower() == "context-js"))
            {
                if (darkMode)
                {
                    styles +=
                        "<link rel='stylesheet' href='/ygfilemanager/context-js/context/skins/kali_dark.css' type='text/css' />\r\n";
                }
                else
                {
                    styles +=
                        "<link rel='stylesheet' href='/ygfilemanager/context-js/context/skins/kali_light.css' type='text/css' />\r\n";
                }
            }
            if (!excludelist.Any(p => p.ToLower() == "toastify"))
            {
                styles +=
                    "<link rel='stylesheet' href='/ygfilemanager/toastify/toastify.css' type='text/css' />\r\n";
            }

            styles += "<link rel='stylesheet' href='/ygfilemanager/ygfilemanager.css' type='text/css' />\r\n";

            if (darkMode)
            {
                styles += "<link rel='stylesheet' href='/ygfilemanager/ygfilemanager-dark.css' type='text/css' />\r\n";
                razorPage.ViewContext.HttpContext.Session.SetString("ygfilemanagerTheme", "Dark");
            }

            return new HtmlString(styles);
        }
    }
}
