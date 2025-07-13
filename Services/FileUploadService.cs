namespace EmployeeManagement.Web.Services;

public class FileUploadService
{
    private string _uploadPath;

    public FileUploadService(IConfiguration config)
    {
        _uploadPath = Path.Combine(
            Directory.GetCurrentDirectory(),
            config["FileUploadPath"] ?? "wwwroot/uploads/aadhar"
        );

        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }
    private static string[] _allowedExtensions = { ".pdf", ".jpg", ".png" };
    public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
        {
            throw new InvalidOperationException("Invalid file type");
        }
        var FileName = Path.GetFileName(fileName);
        var filePath = Path.Combine(_uploadPath, FileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.CopyToAsync(stream);
        }

        return $"/uploads/aadhar/{FileName}";
    }

}