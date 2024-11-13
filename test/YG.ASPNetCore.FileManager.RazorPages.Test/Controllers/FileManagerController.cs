using YG.ASPNetCore.FileManager.CommandsProcessor;
using YG.ASPNetCore.FileManager.Enums;
using Microsoft.AspNetCore.Mvc;

namespace YG.ASPNetCore.FileManager.RazorPages.Test.Controllers
{
    public class FileManagerController : Controller
    {
        private readonly IFileManagerCommandsProcessor _processor;

        public FileManagerController(IFileManagerCommandsProcessor processor)
        {
            _processor = processor;
        }

        [HttpPost, HttpGet]
        public async Task<IActionResult> Index(string id, Command command, string parameters, IFormFile file)
        {
            return await _processor.ProcessCommandAsync(id, command, parameters, file);
        }
    }
}
