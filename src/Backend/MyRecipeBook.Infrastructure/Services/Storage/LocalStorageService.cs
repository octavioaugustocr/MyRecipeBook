using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Extensions;
using MyRecipeBook.Domain.Services.Storage;

namespace MyRecipeBook.Infrastructure.Services.Storage
{
    public class LocalStorageService : IBlobStorageService
    {
        private readonly string _basePath;

        public LocalStorageService(string basePath)
        {
            _basePath = basePath;
        }

        public async Task Upload(User user, Stream file, string fileName)
        {
            var userFolder = Path.Combine(_basePath, user.UserIdentifier.ToString());

            if (Directory.Exists(userFolder).IsFalse())
                Directory.CreateDirectory(userFolder);

            var filePath = Path.Combine(userFolder, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);
        }

        public async Task<string> GetFileUrl(User user, string fileName)
        {
            var userFolder = Path.Combine(_basePath, user.UserIdentifier.ToString());

            if (Directory.Exists(userFolder).IsFalse())
                return string.Empty;

            var filePath = Path.Combine(userFolder, fileName);

            if (File.Exists(filePath).IsFalse())
                return string.Empty;

            return $"/images/{user.UserIdentifier}/{fileName}";
        }

        public async Task Delete(User user, string fileName)
        {
            var userFolder = Path.Combine(_basePath, user.UserIdentifier.ToString());

            if (Directory.Exists(userFolder).IsFalse())
                return;

            var filePath = Path.Combine(userFolder, fileName);

            if (File.Exists(filePath).IsFalse())
                return;

            await Task.Run(() => File.Delete(filePath));

            if (Directory.EnumerateFiles(userFolder).Any().IsFalse())
                await Task.Run(() => Directory.Delete(userFolder));
        }
    }
}
