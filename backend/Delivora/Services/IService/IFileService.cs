namespace Delivora.Services.IService;

public interface IFileService
{
    Task<string> UploadFileAsync(IFormFile file, string folderPath);

    Task DeleteFileAsync(string filePath);
}
