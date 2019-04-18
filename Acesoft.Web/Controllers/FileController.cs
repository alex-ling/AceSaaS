using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Acesoft.Rbac;
using Acesoft.Web.Mvc;
using Acesoft.Util;

namespace Acesoft.Web.Controllers
{
	[ApiExplorerSettings(GroupName = "PLAT")]
	[Route("api/[controller]/[action]")]
	public class FileController : ApiControllerBase
	{
		private class NameSorter : IComparer
		{
			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				FileInfo fileInfo = new FileInfo(x.ToString());
				FileInfo fileInfo2 = new FileInfo(y.ToString());
				return fileInfo.FullName.CompareTo(fileInfo2.FullName);
			}
		}

		private class SizeSorter : IComparer
		{
			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				FileInfo fileInfo = new FileInfo(x.ToString());
				FileInfo fileInfo2 = new FileInfo(y.ToString());
				return fileInfo.Length.CompareTo(fileInfo2.Length);
			}
		}

		private class TypeSorter : IComparer
		{
			public int Compare(object x, object y)
			{
				if (x == null && y == null)
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				FileInfo fileInfo = new FileInfo(x.ToString());
				FileInfo fileInfo2 = new FileInfo(y.ToString());
				return fileInfo.Extension.CompareTo(fileInfo2.Extension);
			}
		}

		[HttpGet, MultiAuthorize, Action("获取图片")]
		public IActionResult GetPhoto(string folder)
		{
            Check.Require(folder.HasValue(), "未指定图片路径！");

            folder = AppCtx.AC.Replace(folder);
			var path = App.GetLocalPath(folder);
			if (Directory.Exists(path))
			{
				DirectoryInfo di = new DirectoryInfo(path);
				var value = from fi in new string[5]
				{
					"*.gif",
					"*.jpg",
					"*.jpeg",
					"*.png",
					"*.bmp"
				}.SelectMany((string fileType) => di.GetFiles(fileType))
				select new
				{
					file_size = fi.Length,
					file_type = fi.Extension,
					file_name = fi.Name,
					file_time = fi.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss")
				};
				return Ok(value);
			}
			return Ok(new string[0]);
		}

		[HttpPost, MultiAuthorize, Action("上传文件")]
		public IActionResult Upload(IFormCollection form)
		{
			var folder = App.GetQuery("folder", "uploads");
			folder = AppCtx.AC.Replace(folder);
			var path = App.GetLocalPath(folder, true);

			IFormFile formFile = form.Files["file"];
			if (formFile == null)
			{
				formFile = form.Files[0];
			}
			var fileName = DateTime.Now.ToDHMSF() + "_" + formFile.FileName;
            FileHelper.Write(path + fileName, formFile.OpenReadStream());

			return Ok(folder + fileName);
		}

		[HttpGet, Action("加载文件")]
		public IActionResult GetKind(string folder)
		{
			var fileTypes = "gif,jpg,jpeg,png,bmp";
			var rootPath = "/wwwroot/uploads/";
			var rootUrl = "/uploads/";
            var curPath2 = "";
            var curUrl2 = "";
            var curDirPath2 = "";
            var upDirPath2 = "";

            if (folder.HasValue())
            {
                folder = AppCtx.AC.Replace(folder);
                rootPath = rootPath + folder + "/";
                rootUrl = rootUrl + folder + "/";
            }

            var path = App.GetQuery("path", "");
            if (!path.HasValue())
            {
                curPath2 = App.GetLocalPath(rootPath);
                curUrl2 = rootUrl;
                curDirPath2 = "";
                upDirPath2 = "";
            }
            else
            {
                curPath2 = App.GetLocalPath(rootPath) + path;
                curUrl2 = rootUrl + path;
                curDirPath2 = path;
                upDirPath2 = Regex.Replace(curDirPath2, "(.*?)[^\\/]+\\/$", "$1");
            }

            var order = App.GetQuery("order", "");
            if (Regex.IsMatch(path, "\\.\\."))
            {
                return Json("禁止访问上层目录！");
            }
            if (path != "" && !path.EndsWith("/"))
            {
                return Json("路径不存在或路径错误！");
            }
            if (!Directory.Exists(curPath2))
            {
                return Json(new string[0]);
            }

            var dirList = Directory.GetDirectories(curPath2);
            var fileList = Directory.GetFiles(curPath2);
            if (!(order == "size"))
            {
                if (order == "type")
                {
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new TypeSorter());
                }
                else
                {
                    Array.Sort(dirList, new NameSorter());
                    Array.Sort(fileList, new NameSorter());
                }
            }
            else
            {
                Array.Sort(dirList, new NameSorter());
                Array.Sort(fileList, new SizeSorter());
            }

            var result = new Hashtable();
            result["moveup_dir_path"] = upDirPath2;
            result["current_dir_path"] = curDirPath2;
            result["current_url"] = curUrl2;
            result["total_count"] = dirList.Length + fileList.Length;
            var dirFileList = (List<Hashtable>)(result["file_list"] = new List<Hashtable>());
            for (int j = 0; j < dirList.Length; j++)
            {
                var dir = new DirectoryInfo(dirList[j]);
                var hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dir.GetFileSystemInfos().Length != 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dir.Name;
                hash["datetime"] = dir.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash);
            }
            for (int i = 0; i < fileList.Length; i++)
            {
                var file = new FileInfo(fileList[i]);
                var hash2 = new Hashtable();
                hash2["is_dir"] = false;
                hash2["has_file"] = false;
                hash2["filesize"] = file.Length;
                hash2["is_photo"] = (Array.IndexOf(fileTypes.Split(','), file.Extension.Substring(1).ToLower()) >= 0);
                hash2["filetype"] = file.Extension.Substring(1);
                hash2["filename"] = file.Name;
                hash2["datetime"] = file.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
                dirFileList.Add(hash2);
            }
            return Json(result);
        }

		[HttpGet, MultiAuthorize, Action("创建文件")]
		public IActionResult CreateFile(string file, string temp)
		{
			var fileInfo = new FileInfo(App.GetLocalPath(file));
            Check.Assert(fileInfo.Exists, $"目标目录下已存在 {fileInfo.Name} 文件");

			new FileInfo(App.GetLocalPath(temp)).CopyTo(fileInfo.FullName);

			return Ok(null);
		}

		[HttpPut, MultiAuthorize, Action("命名文件")]
		public IActionResult RenameFile(string file, string fileName)
		{
			var fileInfo = new FileInfo(App.GetLocalPath(file));
			fileInfo.MoveTo(Path.Combine(fileInfo.DirectoryName, fileName));

			return Ok(null);
		}

		[HttpPut, MultiAuthorize, Action("更新文件")]
		public IActionResult PutFile(string file, [FromBody]JObject data)
		{
			var content = data.GetValue("content", "");
			new FileInfo(App.GetLocalPath(file)).Write(content);
			return Ok(null);
		}

		[HttpDelete, MultiAuthorize, Action("删除文件")]
		public IActionResult DeleteFile(string file)
		{
			var fileInfo = new FileInfo(App.GetLocalPath(file));
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}

			return Ok(null);
		}
	}
}
