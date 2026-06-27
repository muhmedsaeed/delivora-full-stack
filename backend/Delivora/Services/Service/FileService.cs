namespace Delivora.Services.Service;


public class FileService : IFileService
{
    public async Task<string> UploadFileAsync(IFormFile file, string folderPath)
    {
        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}"; // Generate a unique file name to avoid conflicts

        var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderPath);
        Directory.CreateDirectory(directory);

        var path = Path.Combine(directory, fileName);

        using (var stream = new FileStream(path, FileMode.Create)) // Create a new file stream to write the uploaded file
        {
            await file.CopyToAsync(stream); // Copy the contents of the uploaded file to the new file stream
        }

        return $"/{folderPath}/{fileName}";
    }


    public Task DeleteFileAsync(string filePath)
    {
        var relativePath = filePath.TrimStart('/', '\\')
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }
}
