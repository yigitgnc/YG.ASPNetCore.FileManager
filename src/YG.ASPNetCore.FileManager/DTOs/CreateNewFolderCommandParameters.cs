namespace YG.ASPNetCore.FileManager.DTOs;

internal class CreateNewFolderCommandParameters
{
    public string Path { get; set; } = string.Empty;
    public string FolderName { get; set; } = string.Empty;
}