using static NuGet.Packaging.PackagingConstants;

namespace Store.Helpers
{
    public static class DocumentSettings
    {
        public static string UplaodFile(IFormFile file,string folderName)
        {
            // 1. Get Located Folder Path
            string FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);

            // 2. Get File Name and make it Unique
            string FileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3.Get Path[Folder Path + FileName]
            string FilePath = Path.Combine(FolderPath, FileName);

            // 4. Save File as Stream
            using var Fs = new FileStream(FilePath, FileMode.Create);
            file.CopyTo(Fs);

            // 5. Return File Name
            return FileName;
        }
    }
}
