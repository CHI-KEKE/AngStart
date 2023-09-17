using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    public static class SaveUploadedFile
    {
        public static string SaveUploadedFileMethod(IFormFile file)
        {
            // Generate a unique file name or use a predefined file naming convention
            var fileName = Guid.NewGuid().ToString() + "_" + file.FileName;

            // Specify the directory where you want to save the file
            var filePath = Path.Combine("wwwroot", "uploads", fileName);

            // Save the file to the specified path
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            // Return the file path
            return $"http://localhost:5000/uploads/{fileName}";
        }
    }
}