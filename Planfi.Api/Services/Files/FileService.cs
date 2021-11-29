using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PlanfiApi.Helpers;

namespace PlanfiApi.Services.Files
{
  public class FileService : IFileService
  {
    private readonly IWebHostEnvironment _environment;
    private readonly string _movieBucketName = "planfi-movies";
    private readonly string _filesBucketName = "planfi-files";
    private readonly string[] _movieExtensions = { "video/mp4", "video/mov", "video/avi", "video/quicktime" };
    private readonly string[] _imageExtensions = { "image/jpeg", "image/jpg", "image/png", "image/gif" };
  

  public FileService(DataContext context, IWebHostEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        
        public static void Compress(FileInfo fi)
        {
            // Get the stream of the source file.
            using var inFile = fi.OpenRead();
            // Prevent compressing hidden and 
            // already compressed files.
            if ((File.GetAttributes(fi.FullName)
                 & FileAttributes.Hidden)
                != FileAttributes.Hidden & fi.Extension != ".gz")
            {
                // Create the compressed file.
                using var outFile =
                    File.Create(fi.FullName + ".gz");
                using var Compress =
                    new GZipStream(outFile,
                        CompressionMode.Compress);
                // Copy the source file into 
                // the compression stream.
                inFile.CopyTo(Compress);

                Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                    fi.Name, fi.Length.ToString(), outFile.Length.ToString());
            }
        }
        
    
        public async Task<List<string>?> ProcessFileExercise(List<IFormFile> files, string fileName)
        {
          var filesUrl = new List<string>();
            
            foreach (var (formFile, iterator) in files.Select((c, i) => (c, i)))
            {
              if (_movieExtensions.Contains(formFile.ContentType)) 
              {
                var ext = Path.GetExtension(formFile.FileName);
                await using var memoryStream = new MemoryStream();
                await formFile.CopyToAsync(memoryStream);
                var fileNameWithExtensionAndNumber = fileName+1+ext;
                
                //var path = await SaveFileToDirectory(formFile, fileNameWithExtensionAndNumber, FileType.Movie);
                await SaveFileToGoogleStorage(fileNameWithExtensionAndNumber, memoryStream, FileType.Movie);
                
                var nameWithoutSpaces = System.Web.HttpUtility.UrlPathEncode(fileNameWithExtensionAndNumber);
                // generate url for movie
                var url = "https://storage.cloud.google.com/" + _movieBucketName + "/" +nameWithoutSpaces + "?authuser=1";

                filesUrl.Add(url);
              }
              else
              {
                var ext = Path.GetExtension(formFile.FileName);
                await using var memoryStream = new MemoryStream();
                await formFile.CopyToAsync(memoryStream);
                
                var fileNameWithExtensionAndNumber = fileName+iterator+ext;
                //var path = await SaveFileToDirectory(formFile, fileNameWithExtensionAndNumber, FileType.Image);
                await SaveFileToGoogleStorage(fileNameWithExtensionAndNumber, memoryStream, FileType.Image);
                
                var nameWithoutSpaces = System.Web.HttpUtility.UrlPathEncode(fileNameWithExtensionAndNumber);
                // generate url for image
                var url = "https://storage.cloud.google.com/" + _filesBucketName + "/" + nameWithoutSpaces + "?authuser=1";
                
                filesUrl.Add(url);
              }
            }

            return filesUrl;
        }

        public enum FileType
        {
          Movie,
          Image
        }
        
        public async Task<string> SaveFileToDirectory(IFormFile formFile, string name, FileType type)
        {
          string path = type switch
          {
            FileType.Image => Path.Combine(_environment.WebRootPath, "Images"),
            FileType.Movie => Path.Combine(_environment.WebRootPath, "Movies"),
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
          };

          if (!Directory.Exists(path))
          {
            Directory.CreateDirectory(path);
          }
          var fileName = Path.GetFileName(name);
          await using var stream = new FileStream(Path.Combine(path, fileName), FileMode.Create);
          await formFile.CopyToAsync(stream);
          return Path.Combine(path, fileName);
        }

        private GZipStream Compress (string path, string fileName)
        {
            using var sourceFile = File.OpenRead(path);
            using var destinationFile = File.Create(fileName);
            using var output = new GZipStream(destinationFile, CompressionMode.Compress);
            sourceFile.CopyTo(output);
            return output;
        }

        public async Task SaveFileToGoogleStorage(string fileName, MemoryStream stream, FileType type)
        {
            var gcsStorage = await StorageClient.CreateAsync();
            //var stream = new FileStream(path, FileMode.Open);
            var isExist = IsObjectExist(fileName, type);
            
            if (!isExist)
            {
              var uploadObjectAsync = type switch
              {
                FileType.Image => await gcsStorage.UploadObjectAsync(_filesBucketName, fileName, null, stream),
                FileType.Movie => await gcsStorage.UploadObjectAsync(_movieBucketName, fileName, null, stream),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
              };
            }
        }
        
        public async Task DeleteFileFromGoogleStorage(string fileName, FileType type)
        {
            var storage = await StorageClient.CreateAsync();

            var isExist = IsObjectExist(fileName, type);
            if (isExist)
            {
              switch(type)
              {
                case FileType.Movie:
                  await storage.DeleteObjectAsync(_movieBucketName, fileName);
                  break;
                case FileType.Image:
                  await storage.DeleteObjectAsync(_filesBucketName, fileName);
                  break;
                default:
                  throw new ArgumentOutOfRangeException(nameof(type), type, null);
              }
            }
        }

        public async Task DeleteFilesFromExercise(string exerciseName, List<byte[]> filesToDelete)
        {
            for (var i = 0; i < filesToDelete.Count; i++)
            {
                var result = Encoding.UTF8.GetString(filesToDelete[i]);
                if (_movieExtensions.Contains(result)) 
                {
                  await DeleteFileFromGoogleStorage(exerciseName + 1 + result, FileType.Movie);
                }
                else
                {
                  await DeleteFileFromGoogleStorage(exerciseName + i + result, FileType.Image);
                }
            }
        }

        private bool IsObjectExist(string objectName, FileType type)
        {
            try
            {
                var client = StorageClient.Create();
                var isExist = type switch
                {
                  FileType.Image => client.GetObject(_filesBucketName, objectName),
                  FileType.Movie => client.GetObject(_movieBucketName, objectName),
                  _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
                };

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    public interface IFileService
    {
        Task<List<string>?> ProcessFileExercise(List<IFormFile> files, string fileName);
        Task<string> SaveFileToDirectory(IFormFile formFile, string name, FileService.FileType type);
        Task SaveFileToGoogleStorage(string fileName, MemoryStream stream, FileService.FileType type);
        Task DeleteFileFromGoogleStorage(string fileName, FileService.FileType type);
        Task DeleteFilesFromExercise(string exerciseName, List<byte[]> filesToDelete);
    }
}
