﻿using System.IO;
using System.Threading.Tasks;

namespace SharpAbp.Abp.FileStoring
{
    public abstract class FileProviderBase : IFileProvider
    {
        public abstract string Provider { get; }

        public abstract Task<string> SaveAsync(FileProviderSaveArgs args);

        public abstract Task<bool> DeleteAsync(FileProviderDeleteArgs args);

        public abstract Task<bool> ExistsAsync(FileProviderExistsArgs args);

        public abstract Task<bool> DownloadAsync(FileProviderDownloadArgs args);

        public abstract Task<Stream> GetOrNullAsync(FileProviderGetArgs args);

        public abstract Task<string> GetAccessUrlAsync(FileProviderAccessArgs args);
    }
}