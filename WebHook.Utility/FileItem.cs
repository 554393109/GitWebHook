﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebHook.Utility.Extension;
using WebHook.Utility.SignUtils;

namespace WebHook.Utility
{
    public class FileItem
    {
        private string fileName;
        private string fileFullName;
        private string mimeType;
        private byte[] content;
        private FileInfo fileInfo;
        public bool Exists { get { return this.fileInfo.Exists; } }

        /// <summary>
        /// 基于本地文件的构造器。
        /// </summary>
        /// <param name="fileInfo">本地文件</param>
        public FileItem(FileInfo fileInfo)
        {
            if (fileInfo == null || !fileInfo.Exists)
            {
                throw new ArgumentException("fileInfo is null or not exists!");
            }
            this.fileInfo = fileInfo;
        }

        /// <summary>
        /// 基于本地文件全路径的构造器。
        /// </summary>
        /// <param name="filePath">本地文件全路径</param>
        public FileItem(string filePath)
            : this(new FileInfo(filePath))
        { }

        /// <summary>
        /// 基于文件名和字节流的构造器。
        /// </summary>
        /// <param name="fileName">文件名称（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        public FileItem(string fileName, byte[] content)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");
            if (content == null || content.Length == 0)
                throw new ArgumentNullException("content");

            this.fileName = fileName;
            this.content = content;
        }

        /// <summary>
        /// 基于文件名、字节流和媒体类型的构造器。
        /// </summary>
        /// <param name="fileName">文件名（服务端持久化字节流到磁盘时的文件名）</param>
        /// <param name="content">文件字节流</param>
        /// <param name="mimeType">媒体类型</param>
        public FileItem(String fileName, byte[] content, String mimeType)
            : this(fileName, content)
        {
            if (string.IsNullOrEmpty(mimeType))
                throw new ArgumentNullException("mimeType");
            this.mimeType = mimeType;
        }

        public string GetFileName()
        {
            if (this.fileName == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                this.fileName = this.fileInfo.Name;
            }
            return this.fileName;
        }

        public string GetFileFullName()
        {
            if (this.fileFullName == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                this.fileFullName = this.fileInfo.FullName;
            }
            return this.fileFullName;
        }

        public long GetFileSize()
        {
            if (this.fileInfo != null && this.fileInfo.Exists)
                return fileInfo.Length;
            else
                throw new Exception("GetFileSize fail,error:文件不存在");
        }

        public string GetMimeType()
        {
            if (this.mimeType == null)
            {
                this.mimeType = this.GetMimeType(GetContent());
            }
            return this.mimeType;
        }

        public byte[] GetContent()
        {
            if (this.content == null && this.fileInfo != null && this.fileInfo.Exists)
            {
                using (Stream fileStream = this.fileInfo.OpenRead())
                {
                    this.content = new byte[fileStream.Length];
                    fileStream.Read(content, 0, content.Length);
                }
            }

            return this.content;
        }

        public FileStream GetStream()
        {
            var fileStream = default(FileStream);
            if (/*this.content != null && */this.fileInfo != null && this.fileInfo.Exists)
                fileStream = this.fileInfo.OpenRead();

            return fileStream;
        }

        /// <summary>
        /// 获取文件签名
        /// </summary>
        /// <param name="type">默认：MD5</param>
        /// <param name="format">默认：UPPER</param>
        /// <returns></returns>
        public string GetCheckSum(string type = "MD5", FromatSign format = FromatSign.UPPER)
        {
            var CheckSum = string.Empty;

            if (!this.Exists)
                return CheckSum;

            if (type.IsNullOrWhiteSpace() || "MD5".Equals(type, StringComparison.OrdinalIgnoreCase))
            {
                using (var provider = new System.Security.Cryptography.MD5CryptoServiceProvider())
                {
                    try
                    {
                        var byte_sign = provider.ComputeHash(this.GetContent());
                        var result = new StringBuilder();
                        var _format = format == FromatSign.UPPER
                                    ? "X2"              /*大写*/
                                    : "x2";             /*小写*/

                        //for (int i = 0; i < byte_sign.Length; i++)
                        //{
                        //    var hex = byte_sign[i].ToString("X");
                        //    if (hex.Length == 1)
                        //    {
                        //        result.Append("0");
                        //    }
                        //    result.Append(hex);
                        //}
                        for (int i = 0; i < byte_sign.Length; i++)
                            result.Append(byte_sign[i].ToString(format: _format));

                        CheckSum = result.ToString();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("GetCheckSum fail,error:" + ex.Message);
                    }
                }
            }

            return CheckSum;
        }

        public FileInfo GetFileInfo()
        {
            if (this.fileInfo != null && this.fileInfo.Exists)
                return this.fileInfo;
            else
                return null;
        }

        /// <summary>
        /// 获取文件的真实媒体类型。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>媒体类型</returns>
        public string GetMimeType(byte[] fileData)
        {
            string suffix = GetFileSuffix(fileData);
            string mimeType;

            switch (suffix)
            {
                case "JPG":
                mimeType = "image/jpeg";
                break;
                case "GIF":
                mimeType = "image/gif";
                break;
                case "PNG":
                mimeType = "image/png";
                break;
                case "BMP":
                mimeType = "image/bmp";
                break;
                default:
                mimeType = "application/octet-stream";
                break;
            }

            return mimeType;
        }

        /// <summary>
        /// 获取文件的真实后缀名。目前只支持JPG, GIF, PNG, BMP四种图片文件。
        /// </summary>
        /// <param name="fileData">文件字节流</param>
        /// <returns>JPG, GIF, PNG or null</returns>
        public string GetFileSuffix(byte[] fileData)
        {
            if (fileData == null || fileData.Length < 10)
            {
                return null;
            }

            if (fileData[0] == 'G' && fileData[1] == 'I' && fileData[2] == 'F')
            {
                return "GIF";
            }
            else if (fileData[1] == 'P' && fileData[2] == 'N' && fileData[3] == 'G')
            {
                return "PNG";
            }
            else if (fileData[6] == 'J' && fileData[7] == 'F' && fileData[8] == 'I' && fileData[9] == 'F')
            {
                return "JPG";
            }
            else if (fileData[0] == 'B' && fileData[1] == 'M')
            {
                return "BMP";
            }
            else
            {
                return null;
            }
        }

    }
}
