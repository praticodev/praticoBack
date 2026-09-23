using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Pratico.Business.Utils
{
    public static class S3Service
    {
        private const string AwsSectionName = "AWS";

        public static S3FolderScope InFolder(string folderName)
        {
            return new S3FolderScope(folderName);
        }

        public static async Task<string> UploadAsync(Stream stream, string objectKey, string contentType = null, bool overwrite = true, string folderName = null)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            if (string.IsNullOrWhiteSpace(objectKey)) throw new ArgumentException("Object key is required.", nameof(objectKey));

            var settings = LoadSettings();
            var resolvedObjectKey = BuildObjectKey(objectKey, folderName);

            if (!overwrite && await ExistsAsync(objectKey, folderName))
            {
                throw new InvalidOperationException($"The object '{resolvedObjectKey}' already exists in bucket '{settings.BucketName}'.");
            }

            if (stream.CanSeek)
            {
                stream.Position = 0;
            }

            using (var client = CreateClient(settings))
            {
                try
                {
                    var request = new PutObjectRequest
                    {
                        BucketName = settings.BucketName,
                        Key = resolvedObjectKey,
                        InputStream = stream,
                        AutoCloseStream = false,
                        ContentType = contentType
                    };

                    await client.PutObjectAsync(request);
                }
                catch (AmazonS3Exception ex)
                {
                    throw CreateDetailedException("upload", settings, resolvedObjectKey, ex);
                }
            }
            return objectKey;
        }

        public static async Task<bool> DeleteIfExistsAsync(string objectKey, string folderName = null)
        {
            if (string.IsNullOrWhiteSpace(objectKey)) throw new ArgumentException("Object key is required.", nameof(objectKey));

            var settings = LoadSettings();
            var resolvedObjectKey = BuildObjectKey(objectKey, folderName);

            if (!await ExistsAsync(objectKey, folderName))
            {
                return false;
            }

            using (var client = CreateClient(settings))
            {
                try
                {
                    await client.DeleteObjectAsync(settings.BucketName, resolvedObjectKey);
                }
                catch (AmazonS3Exception ex)
                {
                    throw CreateDetailedException("delete", settings, resolvedObjectKey, ex);
                }
            }

            return true;
        }

        public static async Task<bool> ExistsAsync(string objectKey, string folderName = null)
        {
            if (string.IsNullOrWhiteSpace(objectKey)) throw new ArgumentException("Object key is required.", nameof(objectKey));

            var settings = LoadSettings();
            var resolvedObjectKey = BuildObjectKey(objectKey, folderName);

            using (var client = CreateClient(settings))
            {
                try
                {
                    await client.GetObjectMetadataAsync(settings.BucketName, resolvedObjectKey);
                    return true;
                }
                catch (AmazonS3Exception ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                {
                    return false;
                }
                catch (AmazonS3Exception ex)
                {
                    throw CreateDetailedException("metadata", settings, resolvedObjectKey, ex);
                }
            }
        }

        public static async Task<GetObjectResponse> DownloadAsync(string objectKey, string folderName = null)
        {
            if (string.IsNullOrWhiteSpace(objectKey)) throw new ArgumentException("Object key is required.", nameof(objectKey));

            var settings = LoadSettings();
            var resolvedObjectKey = BuildObjectKey(objectKey, folderName);

            using (var client = CreateClient(settings))
            {
                try
                {
                    //var response = await client.GetObjectAsync(settings.BucketName, objectKey);
                    //return File(
                    //    response.ResponseStream,
                    //    response.Headers.ContentType
                    //);
                    return await client.GetObjectAsync(settings.BucketName, resolvedObjectKey);
                }
                catch (AmazonS3Exception ex)
                {
                    throw CreateDetailedException("download", settings, objectKey, ex);
                }
            }
        }

        public static Uri GetObjectUri(string objectKey, string folderName = null)
        {
            if (string.IsNullOrWhiteSpace(objectKey)) throw new ArgumentException("Object key is required.", nameof(objectKey));

            var settings = LoadSettings();
            var resolvedObjectKey = BuildObjectKey(objectKey, folderName);
            var escapedKey = string.Join("/", resolvedObjectKey.Split('/').Select(Uri.EscapeDataString));
            return new Uri($"https://{settings.BucketName}.s3.{settings.Region}.amazonaws.com/{escapedKey}");
        }
        //s3://praticostorage-471112991977-us-east-1-an/veiculos/c875b140-9131-4e09-b1f4-61e5017d4919_GHD-2925.jpeg
        //https://praticostorage-471112991977-us-east-1-an.s3.us-east-1.amazonaws.com/veiculos/c875b140-9131-4e09-b1f4-61e5017d4919_GHD-2925.jpeg

        private static string BuildObjectKey(string objectKey, string folderName)
        {
            var normalizedObjectKey = NormalizePathPart(objectKey);

            if (string.IsNullOrWhiteSpace(folderName))
            {
                return normalizedObjectKey;
            }

            var normalizedFolder = NormalizePathPart(folderName);

            return string.IsNullOrWhiteSpace(normalizedFolder)
                ? normalizedObjectKey
                : $"{normalizedFolder}/{normalizedObjectKey}";
        }

        private static string NormalizePathPart(string pathPart)
        {
            return pathPart
                .Trim()
                .Replace('\\', '/')
                .Trim('/');
        }

        private static IAmazonS3 CreateClient(S3Settings settings)
        {
            var credentials = ResolveCredentials(settings);
            var regionEndpoint = RegionEndpoint.GetBySystemName(settings.Region);
            return new AmazonS3Client(credentials, regionEndpoint);
        }

        private static S3Settings LoadSettings()
        {
            var environmentName = GetEnvironmentName();
            var builder = new ConfigurationBuilder();
            var loadedFiles = new System.Collections.Generic.List<string>();

            foreach (var filePath in GetConfigurationFilePaths(environmentName))
            {
                if (!File.Exists(filePath))
                {
                    continue;
                }

                builder.AddJsonFile(filePath, optional: false, reloadOnChange: false);
                loadedFiles.Add(filePath);
            }

            builder.AddEnvironmentVariables();

            var configuration = builder.Build();
            var awsSection = configuration.GetSection(AwsSectionName);
            var region = FirstNonEmpty(
                awsSection["Region"],
                configuration["AWS_REGION"],
                configuration["AWS_DEFAULT_REGION"]);

            if (loadedFiles.Count == 0)
            {
                throw new InvalidOperationException(
                    $"No appsettings files were found for S3 configuration. " +
                    $"Environment='{environmentName}', CurrentDirectory='{Directory.GetCurrentDirectory()}', BaseDirectory='{AppContext.BaseDirectory}'.");
            }

            var accessKey = FirstNonEmpty(
                awsSection["AccessKey"],
                awsSection["AccessKeyId"],
                configuration["AWS_ACCESS_KEY_ID"]);

            var secretKey = FirstNonEmpty(
                awsSection["SecretKey"],
                configuration["AWS_SECRET_ACCESS_KEY"]);

            var bucketName = FirstNonEmpty(
                awsSection["Bucket"],
                awsSection["BucketName"],
                configuration["AWS_BUCKET"]);

            var profileName = FirstNonEmpty(
                awsSection["Profile"],
                configuration["AWS_PROFILE"]);

            var normalizedRegion = string.IsNullOrWhiteSpace(region)
                ? RegionEndpoint.USEast1.SystemName
                : region.Trim();

            var hasExplicitCredentials = !string.IsNullOrWhiteSpace(accessKey) && !string.IsNullOrWhiteSpace(secretKey);
            var hasProfile = !string.IsNullOrWhiteSpace(profileName);

            if ((!hasExplicitCredentials && !hasProfile) || string.IsNullOrWhiteSpace(bucketName))
            {
                throw new InvalidOperationException(
                    $"Missing or invalid S3 configuration. " +
                    $"Environment='{environmentName}', LoadedFiles='{string.Join(" | ", loadedFiles)}'.");
            }

            return new S3Settings
            {
                AccessKey = accessKey?.Trim(),
                SecretKey = secretKey?.Trim(),
                BucketName = bucketName.Trim(),
                Region = normalizedRegion,
                ProfileName = profileName?.Trim()
            };
        }

        private static AWSCredentials ResolveCredentials(S3Settings settings)
        {
            if (!string.IsNullOrWhiteSpace(settings.ProfileName))
            {
                var profileCredentials = TryGetProfileCredentials(settings.ProfileName);
                if (profileCredentials != null)
                {
                    settings.CredentialSource = $"profile '{settings.ProfileName}'";
                    return profileCredentials;
                }
            }

            if (LooksLikeAwsAccessKey(settings.AccessKey) && !string.IsNullOrWhiteSpace(settings.SecretKey))
            {
                settings.CredentialSource = "appsettings";
                return new BasicAWSCredentials(settings.AccessKey, settings.SecretKey);
            }

            var defaultProfileCredentials = TryGetProfileCredentials("default");
            if (defaultProfileCredentials != null)
            {
                settings.CredentialSource = "profile 'default'";
                return defaultProfileCredentials;
            }

            try
            {
                var fallbackCredentials = FallbackCredentialsFactory.GetCredentials();
                settings.CredentialSource = "AWS fallback credentials chain";
                return fallbackCredentials;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Unable to resolve AWS credentials. " +
                    $"ConfiguredProfile='{settings.ProfileName ?? "n/a"}', " +
                    $"HasAccessKeyInSettings={!string.IsNullOrWhiteSpace(settings.AccessKey)}.",
                    ex);
            }
        }

        private static AWSCredentials TryGetProfileCredentials(string profileName)
        {
            var chain = new CredentialProfileStoreChain();
            return chain.TryGetAWSCredentials(profileName, out var credentials)
                ? credentials
                : null;
        }

        private static bool LooksLikeAwsAccessKey(string accessKey)
        {
            if (string.IsNullOrWhiteSpace(accessKey))
            {
                return false;
            }

            var normalized = accessKey.Trim();

            if (normalized.Length < 16 || normalized.Length > 32)
            {
                return false;
            }

            return normalized.All(ch => ch >= 'A' && ch <= 'Z' || ch >= '0' && ch <= '9');
        }

        private static System.Collections.Generic.IEnumerable<string> GetConfigurationFilePaths(string environmentName)
        {
            var directories = new System.Collections.Generic.List<string>();
            var seenDirectories = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            AddCandidateDirectories(AppContext.BaseDirectory, directories, seenDirectories);
            AddCandidateDirectories(Directory.GetCurrentDirectory(), directories, seenDirectories);

            foreach (var directory in directories)
            {
                yield return Path.Combine(directory, "appsettings.json");
                yield return Path.Combine(directory, $"appsettings.{environmentName}.json");
            }
        }

        private static void AddCandidateDirectories(
            string startDirectory,
            System.Collections.Generic.ICollection<string> directories,
            System.Collections.Generic.ISet<string> seenDirectories)
        {
            if (string.IsNullOrWhiteSpace(startDirectory) || !Directory.Exists(startDirectory))
            {
                return;
            }

            var current = new DirectoryInfo(Path.GetFullPath(startDirectory));

            while (current != null)
            {
                AddDirectory(current.FullName, directories, seenDirectories);

                var apiDirectory = Path.Combine(current.FullName, "Pratico.Api");
                if (Directory.Exists(apiDirectory))
                {
                    AddDirectory(apiDirectory, directories, seenDirectories);
                }

                current = current.Parent;
            }
        }

        private static void AddDirectory(
            string directory,
            System.Collections.Generic.ICollection<string> directories,
            System.Collections.Generic.ISet<string> seenDirectories)
        {
            var normalizedDirectory = Path.GetFullPath(directory);
            if (seenDirectories.Add(normalizedDirectory))
            {
                directories.Add(normalizedDirectory);
            }
        }

        private static string FirstNonEmpty(params string[] values)
        {
            foreach (var value in values)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            return null;
        }

        private static InvalidOperationException CreateDetailedException(
            string operation,
            S3Settings settings,
            string objectKey,
            AmazonS3Exception ex)
        {
            var message =
                $"AWS S3 {operation} failed. " +
                $"StatusCode={(int)ex.StatusCode} ({ex.StatusCode}), " +
                $"ErrorCode={ex.ErrorCode ?? "n/a"}, " +
                $"RequestId={ex.RequestId ?? "n/a"}, " +
                $"Bucket={settings.BucketName}, " +
                $"Key={objectKey}, " +
                $"Region={settings.Region}, " +
                $"CredentialSource={settings.CredentialSource ?? "unknown"}. " +
                $"Message={ex.Message}";

            return new InvalidOperationException(message, ex);
        }

        private static string GetEnvironmentName()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                   ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
                   ?? "Production";
        }

        private sealed class S3Settings
        {
            public string AccessKey { get; set; }
            public string SecretKey { get; set; }
            public string BucketName { get; set; }
            public string Region { get; set; }
            public string ProfileName { get; set; }
            public string CredentialSource { get; set; }
        }

        public sealed class S3FolderScope
        {
            private readonly string _folderName;

            internal S3FolderScope(string folderName)
            {
                if (string.IsNullOrWhiteSpace(folderName))
                {
                    throw new ArgumentException("Folder name is required.", nameof(folderName));
                }

                _folderName = folderName;
            }

            public Task<string> UploadAsync(Stream stream, string objectKey, string contentType = null, bool overwrite = true)
            {
                return S3Service.UploadAsync(stream, objectKey, contentType, overwrite, _folderName);
            }

            public Task<bool> DeleteIfExistsAsync(string objectKey)
            {
                return S3Service.DeleteIfExistsAsync(objectKey, _folderName);
            }

            public Task<bool> ExistsAsync(string objectKey)
            {
                return S3Service.ExistsAsync(objectKey, _folderName);
            }

            public Task<GetObjectResponse> DownloadAsync(string objectKey)
            {
                return S3Service.DownloadAsync(objectKey, _folderName);
            }

            public Uri GetObjectUri(string objectKey)
            {
                return S3Service.GetObjectUri(objectKey, _folderName);
            }
        }
    }
}
