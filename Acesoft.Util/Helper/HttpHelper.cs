using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Acesoft.Util
{
    public class HttpHelper
    {
        public const string ContentTypeText = "text/xml";
        public const string ContentTypeJson = "application/json";
        public const string ContentTypeForm = "application/x-www-form-urlencoded";
        public const string ContentTypeFile = "multipart/form-data";

        #region get
        public static T HttpGetJson<T>(string url, Dictionary<string, string> headers = null, int timeout = 0, Encoding encoding = null)
        {
            var json = HttpGet(url, headers, timeout, encoding);
            return SerializeHelper.FromJson<T>(json);
        }

        public static async Task<T> HttpGetJsonAsync<T>(string url, Dictionary<string, string> headers = null, int timeout = 0, Encoding encoding = null)
        {
            var json = await HttpGetAsync(url, headers, timeout, encoding);
            return SerializeHelper.FromJson<T>(json);
        }

        public static string HttpGet(string url, Dictionary<string, string> headers = null, int timeout = 0, Encoding encoding = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers, timeout);

                var resultBytes = client.GetByteArrayAsync(url).Result;
                return (encoding ?? Encoding.UTF8).GetString(resultBytes);
            }
        }

        public static async Task<string> HttpGetAsync(string url, Dictionary<string, string> headers = null, int timeout = 0, Encoding encoding = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers, timeout);

                var resultBytes = await client.GetByteArrayAsync(url);
                return (encoding ?? Encoding.UTF8).GetString(resultBytes);
            }
        }
        #endregion

        #region post
        public static string HttpPost<T>(string url, T postData, Dictionary<string, string> headers = null, string contentType = null, int timeout = 0, Encoding encoding = null)
        {
            var json = SerializeHelper.ToJson(postData);
            return HttpPost(url, json, headers, contentType, timeout, encoding);
        }

        public static Task<string> HttpPostAsync<T>(string url, T postData, Dictionary<string, string> headers = null, string contentType = null, int timeout = 0, Encoding encoding = null)
        {
            var json = SerializeHelper.ToJson(postData);
            return HttpPostAsync(url, json, headers, contentType, timeout, encoding);
        }

        public static string HttpPost(string url, string postData, Dictionary<string, string> headers = null, string contentType = null, int timeout = 0, Encoding encoding = null)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHttpClient(client, headers, timeout);

                using (var content = new StringContent(postData ?? "", encoding ?? Encoding.UTF8))
                {
                    if (contentType != null)
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                        content.Headers.ContentType.CharSet = encoding == null ? "UTF-8" : encoding.EncodingName;
                    }
                    using (var response = client.PostAsync(url, content).Result)
                    {
                        var resultBytes = response.Content.ReadAsByteArrayAsync().Result;
                        return (encoding ?? Encoding.UTF8).GetString(resultBytes);
                    }
                }
            }
        }

        public static async Task<string> HttpPostAsync(string url, string postData, Dictionary<string, string> headers = null, string contentType = "application/x-www-form-urlencoded", int timeout = 0, Encoding encoding = null)
        {
            using (HttpClient client = new HttpClient())
            {
                SetHttpClient(client, headers, timeout);

                using (var content = new StringContent(postData ?? "", encoding ?? Encoding.UTF8))
                {
                    if (contentType != null)
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                        content.Headers.ContentType.CharSet = encoding == null ? "UTF-8" : encoding.EncodingName;
                    }
                    using (var response = await client.PostAsync(url, content))
                    {
                        var resultBytes = await response.Content.ReadAsByteArrayAsync();
                        return (encoding ?? Encoding.UTF8).GetString(resultBytes);
                    }
                }
            }
        }
        #endregion

        #region download
        public static void HttpDownloadFile(string url, string saveFile, Dictionary<string, string> headers = null)
        {
            var bytes = HttpDownload(url, headers);
            FileHelper.Write(saveFile, bytes);
        }

        public static async Task HttpDownloadFileAsync(string url, string saveFile, Dictionary<string, string> headers = null)
        {
            var bytes = await HttpDownloadAsync(url, headers);
            FileHelper.Write(saveFile, bytes);
        }

        public static void HttpDownload(string url, Stream stream, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers);

                var data = client.GetByteArrayAsync(url).Result;
                stream.Write(data, 0, data.Length);
            }
        }

        public static Task HttpDownloadAsync(string url, Stream stream, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers);

                return client.GetStreamAsync(url);
            }
        }

        public static byte[] HttpDownload(string url, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers);

                return client.GetByteArrayAsync(url).Result;
            }
        }

        public static Task<byte[]> HttpDownloadAsync(string url, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers);

                return client.GetByteArrayAsync(url);
            }
        }
        #endregion

        #region upload
        public static string HttpUpload(string url, byte[] fileBytes, string fileName, string postName = "file", Dictionary<string, string> headers = null, string contentType = null, int timeout = 0, Encoding encoding = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers, timeout);

                var boundary = "----" + DateTime.Now.Ticks.ToString("x");
                using (var content = new MultipartFormDataContent(boundary))
                {
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = $"\"{postName}\"",
                        FileName = $"\"{fileName}\"",
                        Size = fileBytes.Length
                    };
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    content.Add(fileContent);

                    // 微信里boundary前后不能带""
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse($"multipart/form-data; boundary={boundary}");

                    using (var response = client.PostAsync(url, content).Result)
                    {
                        var resultBytes = response.Content.ReadAsByteArrayAsync().Result;
                        return (encoding ?? Encoding.UTF8).GetString(resultBytes);
                    }
                }
            }
        }

        public static async Task<string> HttpUploadAsync(string url, byte[] fileBytes, string fileName, string postName = "file", Dictionary<string, string> headers = null, string contentType = null, int timeout = 0, Encoding encoding = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers, timeout);

                var boundary = "----" + DateTime.Now.Ticks.ToString("x");
                using (var content = new MultipartFormDataContent(boundary))
                {
                    var fileContent = new ByteArrayContent(fileBytes);
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = $"\"{postName}\"",
                        FileName = $"\"{fileName}\"",
                        Size = fileBytes.Length
                    };
                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    content.Add(fileContent);

                    // 微信里boundary前后不能带""
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse($"multipart/form-data; boundary={boundary}");

                    using (var response = await client.PostAsync(url, content))
                    {
                        var resultBytes = await response.Content.ReadAsByteArrayAsync();
                        return (encoding ?? Encoding.UTF8).GetString(resultBytes);
                    }
                }
            }
        }

        public static string HttpUpload(string url, Stream stream, string fileName, string postName = "file", Dictionary<string, string> headers = null, string contentType = null, int timeout = 0, Encoding encoding = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers, timeout);

                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new StreamContent(stream);
                    if (contentType != null)
                    {
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    }
                    content.Add(fileContent, postName, fileName);

                    using (var response = client.PostAsync(url, content).Result)
                    {
                        var resultBytes = response.Content.ReadAsByteArrayAsync().Result;
                        return (encoding ?? Encoding.UTF8).GetString(resultBytes);
                    }
                }
            }
        }

        public static async Task<string> HttpUploadAsync(string url, Stream stream, string fileName, string postName = "file", Dictionary<string, string> headers = null, string contentType = null, int timeout = 0, Encoding encoding = null)
        {
            using (var client = new HttpClient())
            {
                SetHttpClient(client, headers, timeout);

                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new StreamContent(stream);
                    if (contentType != null)
                    {
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    }
                    content.Add(fileContent, postName, fileName);

                    using (var response = await client.PostAsync(url, content))
                    {
                        var resultBytes = await response.Content.ReadAsByteArrayAsync();
                        return (encoding ?? Encoding.UTF8).GetString(resultBytes);
                    }
                }
            }
        }

        public static string HttpUploadFile(string url, string file, string postName = "file", Dictionary<string, string> headers = null, string contentType = null, int timeout = 0, Encoding encoding = null)
        {
            return HttpUpload(url, File.OpenRead(file), Path.GetFileName(file), postName, headers, contentType, timeout, encoding);
        }

        public static Task<string> HttpUploadFileAsync(string url, string file, string postName = "file", Dictionary<string, string> headers = null, string contentType = null, int timeout = 0, Encoding encoding = null)
        {
            return HttpUploadAsync(url, File.OpenRead(file), Path.GetFileName(file), postName, headers, contentType, timeout, encoding);
        }
        #endregion

        #region client
        private static void SetHttpClient(HttpClient client, Dictionary<string, string> headers = null, int timeout = 0)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
            
            if (!client.DefaultRequestHeaders.Contains("Accept"))
            {
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            }
            if (!client.DefaultRequestHeaders.Contains("User-Agent"))
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/31.0.1650.57 Safari/537.36");
            }

            if (timeout > 0)
            {
                client.Timeout = new TimeSpan(0, 0, timeout);
            }
        }
        #endregion
    }
}
