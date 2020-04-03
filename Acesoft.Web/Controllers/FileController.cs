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
using Acesoft.Data;

namespace Acesoft.Web.Controllers
{
	[ApiExplorerSettings(GroupName = "PLAT")]
	[Route("api/[controller]/[action]")]
	public class FileController : ApiControllerBase
	{
        #region kind
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

		[HttpGet, Action("加载文件")]
		public IActionResult GetKind(string dir)
		{
			var fileTypes = "gif,jpg,jpeg,png,bmp";
			var rootPath = "/uploads/";
			var rootUrl = "/uploads/";
            var curPath2 = "";
            var curUrl2 = "";
            var curDirPath2 = "";
            var upDirPath2 = "";

            if (dir.HasValue())
            {
                dir = AppCtx.AC.Replace(dir);
                rootPath = rootPath + dir + "/";
                rootUrl = rootUrl + dir + "/";
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
                var dirInfo = new DirectoryInfo(dirList[j]);
                var hash = new Hashtable();
                hash["is_dir"] = true;
                hash["has_file"] = (dirInfo.GetFileSystemInfos().Length != 0);
                hash["filesize"] = 0;
                hash["is_photo"] = false;
                hash["filetype"] = "";
                hash["filename"] = dirInfo.Name;
                hash["datetime"] = dirInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss");
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
        #endregion

        #region upload
        [HttpPost, MultiAuthorize, Action("上传文件")]
        public IActionResult Upload(IFormCollection form)
        {
            var folder = App.GetQuery("folder", "/uploads/");
            var dir = App.GetQuery("dir", "");
            if (dir.HasValue())
            {
                folder += $"{dir.TrimEnd('/')}/";
            }

            folder = AppCtx.AC.Replace(folder);
            var path = App.GetLocalPath(folder, true);

            IFormFile formFile = form.Files["file"];
            if (formFile == null)
            {
                formFile = form.Files[0];
            }
            var fileName = DateTime.Now.ToDHMSF() + "_" + Path.GetFileName(formFile.FileName);
            FileHelper.Write(path + fileName, formFile.OpenReadStream());

            if (dir.HasValue())
            {
                return Json(new
                {
                    error = 0,
                    url = folder + fileName
                });
            }
            else
            {
                return Ok(folder + fileName);
            }
        }
        #endregion

        #region folder
        [HttpPost, MultiAuthorize, Action("创建目录")]
        public IActionResult PostFolder([FromBody]JObject data)
        {
            var newpath = data.GetValue("newpath", "");
            var dir = App.GetLocalPath(newpath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            else
            {
                throw new AceException($"目录{newpath}已存在！");
            }

            return Ok(null);
        }

        [HttpPost, MultiAuthorize, Action("拷贝目录")]
        public IActionResult CopyFolder([FromBody]JObject data)
        {
            var path = data.GetValue("path", "");
            var newPath = data.GetValue("newpath", "");
            var dir = new DirectoryInfo(App.GetLocalPath(path));
            if (dir.Exists)
            {
                var newDir = new DirectoryInfo(App.GetLocalPath(newPath));
                Check.Assert(newDir.Exists, $"目录{newPath}已存在！");

                dir.CopyTo(newDir.FullName);
            }
            else
            {
                throw new AceException($"目录{path}不存在！");
            }

            return Ok(null);
        }

        [HttpPut, MultiAuthorize, Action("命名目录")]
        public IActionResult PutFolder([FromBody]JObject data)
        {
            var path = data.GetValue("path", "");
            var newPath = data.GetValue("newpath", "");
            var dirInfo = new DirectoryInfo(App.GetLocalPath(path));
            if (dirInfo.Exists)
            {
                var newDir = App.GetLocalPath(newPath);
                Check.Assert(Directory.Exists(newDir), $"目录{newPath}已存在！");

                dirInfo.MoveTo(newDir);
            }
            else
            {
                throw new AceException($"目录{path}不存在！");
            }

            return Ok(null);
        }

        [HttpDelete, MultiAuthorize, Action("删除目录")]
        public IActionResult DeleteFolder(string id)
        {
            Check.Require(id.HasValue(), $"请传入要删除的ID参数！");

            var dir = App.GetLocalPath(id);
            if (Directory.Exists(dir))
            {
                Directory.Delete(dir);
            }
            else
            {
                throw new AceException($"目录{id}不存在！");
            }

            return Ok(null);
        }
        #endregion

        #region file
        [HttpPost, MultiAuthorize, Action("创建文件")]
		public IActionResult PostFile([FromBody]JObject data)
        {
            var path = data.GetValue("path", "");
            var name = data.GetValue("name", "");
            var content = data.GetValue("content", "");
            var file = App.GetLocalPath(Path.Combine(path, name));
			var fileInfo = new FileInfo(file);
            Check.Assert(fileInfo.Exists, $"目录下已存在{fileInfo.Name}文件");

            FileHelper.Write(fileInfo, content);
			return Ok(null);
		}

        [HttpPost, MultiAuthorize, Action("拷贝文件")]
        public IActionResult CopyFile([FromBody]JObject data)
        {
            var path = data.GetValue("path", "");
            var newPath = data.GetValue("newpath", "");
            var fileInfo = new FileInfo(App.GetLocalPath(path));
            Check.Require(fileInfo.Exists, $"文件{fileInfo.Name}不存在！");
            var file = new FileInfo(App.GetLocalPath(newPath));
            Check.Assert(file.Exists, $"文件{file.Name}已存在！");

            fileInfo.CopyTo(file.FullName);
            return Ok(null);
        }

        [HttpPut, MultiAuthorize, Action("命名文件")]
		public IActionResult MoveFile([FromBody]JObject data)
        {
            var path = data.GetValue("path", "");
            var newPath = data.GetValue("newpath", "");
            var fileInfo = new FileInfo(App.GetLocalPath(path));
			fileInfo.MoveTo(App.GetLocalPath(newPath));

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
		public IActionResult DeleteFile(string id)
		{
            Check.Require(id.HasValue(), $"请传入要删除的ID参数！");

            var fileInfo = new FileInfo(App.GetLocalPath(id));
			if (fileInfo.Exists)
			{
				fileInfo.Delete();
			}

			return Ok(null);
		}
        #endregion

        #region temp
        [HttpGet, MultiAuthorize, Action("获取目录")]
        public IActionResult GetFolders(string path, string search)
        {
            var dir = new DirectoryInfo(App.GetLocalPath(path));
            var root = new TreeNode();
            root.Id = App.GetQuery("rootid", path);
            root.Text = App.GetQuery("rootname", dir.Name);
            root.IconCls = App.GetQuery("rooticon", "");

            LoadTreeNode(root, search.HasValue() ? dir.GetDirectories(search) : dir.GetDirectories());
            return Json(root);
        }

        [HttpGet, MultiAuthorize, Action("获取文件")]
        public IActionResult GetFiles(string path, string search)
        {
            var dir = new DirectoryInfo(App.GetLocalPath(path));
            var files = dir.GetFiles();
            var grid = new
            {
                total = files.Length,
                rows = files.Select(file => new
                {
                    id = App.GetLocalBasePath(file.FullName),
                    name = file.Name,
                    filesize = file.Length,
                    filetype = file.Extension,
                    modified = file.LastWriteTime,
                    created = file.CreationTime,
                    action = "del_remove=删除"
                })
            };

            return Json(grid);
        }

        private void LoadTreeNode(TreeNode node, DirectoryInfo[] dirs)
        {
            foreach (var subDir in dirs)
            {
                var child = new TreeNode
                {
                    Id = $"{node.Id}/{subDir.Name}",
                    Text = subDir.Name
                };
                node.Children.Add(child);
                LoadTreeNode(child, subDir.GetDirectories());
            }
        }
        #endregion
    }
}
