using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Pratico.Business.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Pratico.Business.Utils
{
    public static class BlobService
    {
        private const string AzureSectionName = "ContainerAzure";
        private static readonly Lazy<AzureStorageAccount> Settings = new Lazy<AzureStorageAccount>(LoadSettings);

        public static async Task UploadAsync(Stream stream, string blobName, string contentType = null, bool overwrite = true)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(blobName)) throw new ArgumentException("Blob name is required.", nameof(blobName));

            var blob = CreateBlobClient(blobName);

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            await blob.UploadAsync(stream, overwrite);

            if (!string.IsNullOrWhiteSpace(contentType))
            {
                await blob.SetHttpHeadersAsync(new BlobHttpHeaders
                {
                    ContentType = contentType
                });
            }
        }

        public static async Task<bool> DeleteIfExistsAsync(string blobName)
        {
            if (string.IsNullOrWhiteSpace(blobName)) throw new ArgumentException("Blob name is required.", nameof(blobName));

            var blob = CreateBlobClient(blobName);
            var result = await blob.DeleteIfExistsAsync();
            return result.Value;
        }

        public static async Task<bool> ExistsAsync(string blobName)
        {
            if (string.IsNullOrWhiteSpace(blobName)) throw new ArgumentException("Blob name is required.", nameof(blobName));

            var blob = CreateBlobClient(blobName);
            var result = await blob.ExistsAsync();
            return result.Value;
        }

        public static async Task<BlobDownloadResult> DownloadAsync(string blobName)
        {
            if (string.IsNullOrWhiteSpace(blobName)) throw new ArgumentException("Blob name is required.", nameof(blobName));

            var blob = CreateBlobClient(blobName);
            var result = await blob.DownloadContentAsync();
            return result.Value;
        }

        public static Uri GetBlobUri(string blobName)
        {
            if (string.IsNullOrWhiteSpace(blobName)) throw new ArgumentException("Blob name is required.", nameof(blobName));

            return CreateBlobClient(blobName).Uri;
        }

        private static BlobClient CreateBlobClient(string blobName)
        {
            var settings = Settings.Value;
            return new BlobClient(settings.ConectionString, settings.NomeContainer, blobName);
        }

        private static AzureStorageAccount LoadSettings()
        {
            var builder = new ConfigurationBuilder();
            var currentDirectory = Directory.GetCurrentDirectory();
            var baseDirectory = AppContext.BaseDirectory;

            builder.AddJsonFile(Path.Combine(baseDirectory, "appsettings.json"), optional: true, reloadOnChange: false);
            builder.AddJsonFile(Path.Combine(baseDirectory, $"appsettings.{GetEnvironmentName()}.json"), optional: true, reloadOnChange: false);
            builder.AddJsonFile(Path.Combine(currentDirectory, "appsettings.json"), optional: true, reloadOnChange: false);
            builder.AddJsonFile(Path.Combine(currentDirectory, $"appsettings.{GetEnvironmentName()}.json"), optional: true, reloadOnChange: false);
            builder.AddEnvironmentVariables();

            var configuration = builder.Build();
            var settings = configuration.GetSection(AzureSectionName).Get<AzureStorageAccount>();

            if (settings == null || string.IsNullOrWhiteSpace(settings.ConectionString) || string.IsNullOrWhiteSpace(settings.NomeContainer))
            {
                throw new InvalidOperationException($"Missing or invalid configuration section '{AzureSectionName}' in appsettings.");
            }

            return settings;
        }

        private static string GetEnvironmentName()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                   ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                   ?? "Production";
        }
    }
}
