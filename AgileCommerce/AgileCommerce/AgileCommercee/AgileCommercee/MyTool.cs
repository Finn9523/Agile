namespace AgileCommercee
{
    public class MyTool
    {
        public static string UploadImageToFolder(IFormFile myfile, string folder)
        {
            try
            {
                // Ensure the folder path exists
                var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Hinh", folder);
                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder); // Create folder if it doesn't exist
                }

                // Generate unique file name to avoid overwriting
                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(myfile.FileName);

                // Combine the folder path with the unique file name
                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                // Save the file
                using (var newFile = new FileStream(filePath, FileMode.Create))
                {
                    myfile.CopyTo(newFile);
                }

                return uniqueFileName; // Return the generated unique file name
            }
            catch (Exception ex)
            {
                // Log the error message (this can be integrated with a proper logging framework)
                Console.WriteLine($"Error uploading file: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
