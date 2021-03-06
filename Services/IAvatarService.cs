﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace MVCFinApp.Services
{
    public interface IAvatarService //Don't forget to make this public!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        public Task<byte[]> ConvertFileToByteArrayAsync(IFormFile file);

        public string ConvertByteArrayToFile(byte[] fileData, string extension);

        public string GetFileIcon(string file);

        public string FormatFileSize(long bytes);

        public string ShortenFileName(string name, int length);

        public string GetDefaultAvatarFileName();

        public Task<byte[]> GetDefaultAvatarFileBytesAsync();
    }
}
