﻿using YG.ASPNetCore.FileManager.Enums;
using YG.ASPNetCore.FileManager.ViewComponentsModel.LanguageModels;
using YG.ASPNetCore.FileManager.ViewComponentsModel.LanguageModels.BuiltIn;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YG.ASPNetCore.FileManager.ViewComponentsModel
{
    public class FileManagerConfig
    {
        private byte _compressionLevel = 6;

        /// <summary>
        /// The maximum storage space (in megabytes) that the user can use
        /// </summary>
        public long StorageMaxSizeMByte { get; set; } = 1024;

        /// <summary>
        /// Gets or sets the maximum allowed size to compress (per action). If not set 0 (zero), there will be no size limit for compression.
        /// </summary>
        public long CompressionMaxSizeMByte { get; set; } = 256;

        /// <summary>
        /// The maximum filesize (in megabytes) that is allowed to be uploaded.
        /// </summary>
        public long MaxFileSizeToUploadMByte { get; set; } = 256;

        /// <summary>
        /// Whether you want files to be uploaded in chunks to your server. 
        /// </summary>
        public bool Chunking { get; set; } = true;

        /// <summary>
        /// If chunking is true, then this defines the chunk size in bytes.
        /// </summary>
        public long ChunkSizeByte { get; set; } = 10000000;

        /// <summary>
        /// Whether a chunk should be retried if it fails.
        /// </summary>
        public bool RetryChunks { get; set; } = true;

        /// <summary>
        /// If retryChunks is true, how many times should it be retried.
        /// </summary>
        public int RetryChunksLimit { get; set; } = 3;

        /// <summary>
        /// How many file uploads to process in parallel.
        /// </summary>
        public int ParallelUploads { get; set; } = 1;

        /// <summary>
        /// Allowed file extensions to upload (comma separated). If not set, there will be no file format limit for upload.
        /// e.g.: ".pdf,.png"
        /// </summary>
        public string AcceptedFiles { get; set; } = "";

        /// <summary>
        /// Disabled functions list:
        /// "Search", "CreateNewFolder", "CreateNewFile", "Delete", "Rename", "Zip",
        /// "Unzip", "Copy", "Cut", "EditFile", "Download", "GetFileContent", "Upload",
        /// "ToggleView", "Browse", "Reload", "Breadcrumb", "FoldersTree", "MenuBar",
        /// "ContextMenu", "FilePreview", "View"
        /// </summary>
        public HashSet<Command> DisabledFunctions { get; set; } = new HashSet<Command>();

        /// <summary>
        /// Compression Level: from 0 (fastest) to 9 (best compression)
        /// </summary>
        public byte CompressionLevel
        {
            get => _compressionLevel;
            set
            {
                if (value < 0) { _compressionLevel = 0; }
                else if (value > 9) { _compressionLevel = 9; }
                else { _compressionLevel = value; }
            }
        }

        /// <summary>
        /// Language For UI Default is (Built-In) new EnglishLanguage();
        /// Built-In alternative: TurkishLanguage();
        /// or CustomLanguage() for setting custom properties...
        /// </summary>
        public ILanguage Language { get; set; } = new EnglishLanguage();

        /// <summary>
        /// Encryption key (EncryptionKey must be provided if UseEncryption is true.)
        /// <para><b>Warning:</b> It is highly recommended not to hardcode this encryption key directly into your code.</para>
        /// <para><b>Security Risk:</b> Since both this project and the files reside on the same server, hardcoding the key compromises the entire purpose of encryption.</para>
        /// <para>The primary goal of encryption is to protect your files in the event of a data breach, whether caused by a vulnerability in the hosting provider or by direct file access attempts.</para>
        /// <para><b>Reminder:</b> Consider using a secure key management system, environment variables, or a secrets manager to store your encryption key securely.</para>
        /// </summary>
        /// <remarks>
        /// <b>Note:</b> Do not store sensitive information like encryption keys in plain text within your source code to prevent unauthorized access.
        /// </remarks>
        [Required(ErrorMessage = "EncryptionKey must be provided if UseEncryption is true.")]
        [JsonIgnore] // To prevent JSON serialization of sensitive information
        public string EncryptionKey { get; set; } = "your_encryption_key_here_for_encryption_purposes";

        /// <summary>
        /// Use encryption (EncryptionKey must be provided if UseEncryption is true.)
        /// <para>The primary goal of encryption is to protect your files in the event of a data breach, whether caused by a vulnerability in the hosting provider or by direct file access attempts.</para>
        /// <para><b>Reminder:</b> Consider using a secure key management system, environment variables, or a secrets manager to store your encryption key securely.</para>
        /// </summary>
        public bool UseEncryption { get; set; } = false;

        /// <summary>
        /// Use Recycle Bin (default is true)
        /// <para>When a file is deleted, it is moved to the Recycle Bin instead of being permanently deleted.</para>
        /// <para>Recycle bin acts like a normnal folder in the file manager.</para>
        /// <para>It can be used to restore deleted files.</para>
        /// <para>Recycle Bin is enabled by default.</para>
        /// <para>It can be disabled by setting UseRecycleBin to false.</para>
        /// <para>When UseRecycleBin is disabled, deleted files are permanently deleted.</para>
        /// <para>When UseRecycleBin is enabled, deleted files are moved to the Recycle Bin.</para>
        /// </summary>
        public bool UseRecycleBin { get; set; } = true;
    }
}
