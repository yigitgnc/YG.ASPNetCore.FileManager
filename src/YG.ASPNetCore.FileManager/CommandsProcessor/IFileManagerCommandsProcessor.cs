using YG.ASPNetCore.FileManager.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace YG.ASPNetCore.FileManager.CommandsProcessor
{
    public interface IFileManagerCommandsProcessor
    {
        Task<IActionResult> ProcessCommandAsync(string id, Command command, string parameters, IFormFile file);
    }
}
