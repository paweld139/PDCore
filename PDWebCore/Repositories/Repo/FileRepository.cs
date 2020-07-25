using PDCore.Interfaces;
using PDCore.Repositories.Repo;
using PDCoreNew.Models;
using PDCoreNew.Repositories.Repo;
using PDWebCore.Context.IContext;
using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace PDWebCore.Repositories.Repo
{
    public sealed class FileRepository : SqlRepositoryEntityFrameworkAsync<FileModel>
    {
        public readonly IMainWebDbContext _db;
        public FileRepository(IMainWebDbContext db, ILogger logger) : base(db, logger)
        {
            _db = db;
        }

        public async Task WriteAllBytesAsync(string path, byte[] data)
        {
            using (FileStream SourceStream = File.Open(path, FileMode.OpenOrCreate))
            {
                SourceStream.Seek(0, SeekOrigin.End);

                await SourceStream.WriteAsync(data, 0, data.Length);
            }
        }

        public async Task<byte[]> ReadAllBytesAsync(string path)
        {
            byte[] result;

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                result = new byte[stream.Length];

                await stream.ReadAsync(result, 0, (int)stream.Length);
            }

            return result;
        }

        public Task RemoveFileAsync(string path)
        {
            return Task.Run(() => { File.Delete(path); });
        }

        public async Task AddFile(string name, int objId, ObjType objType, byte[] file)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            FileModel filee = new FileModel
            {
                Name = name,
                Extension = Path.GetExtension(name).Replace(".", ""),
                RefId = objId,
                RefGid = objType
            };

            Add(filee);

            await CommitAsync();

            await WriteAllBytesAsync(Path.Combine(targetFolder, filee.ALLFId.ToString()), file);
        }

        public string GetExtension(string fileName)
        {
            string ext = Path.GetExtension(fileName).Replace(".", "");

            return ext;
        }

        public async Task AddFileFromObject(FileModel file)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            Add(file);

            await CommitAsync();

            await WriteAllBytesAsync(Path.Combine(targetFolder, file.ALLFId.ToString()), file.Data);
        }

        public async Task AdFileFromObjectsList(List<FileModel> File)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            AddRange(File);

            await CommitAsync();

            int index = 0;

            var tasks = new List<Task>();

            foreach (var item in File)
            {
                tasks.Add(WriteAllBytesAsync(Path.Combine(targetFolder, item.ALLFId.ToString()), File[index].Data));

                index++;
            }

            await Task.WhenAll(tasks);
        }

        public async Task AddFile(List<Tuple<string, int, ObjType, byte[]>> File)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            List<FileModel> files = new List<FileModel>();

            foreach (var item in File)
            {
                FileModel filee = new FileModel
                {
                    Name = item.Item1,

                    Extension = Path.GetExtension(item.Item1).Replace(".", ""),
                    RefId = item.Item2,
                    RefGid = item.Item3
                };

                files.Add(filee);

                Add(filee);              
            }

            await CommitAsync();

            int index = 0;

            var tasks = new List<Task>();

            foreach (var item in files)
            {
                tasks.Add(WriteAllBytesAsync(Path.Combine(targetFolder, item.ALLFId.ToString()), File[index].Item4));

                index++;
            }

            await Task.WhenAll(tasks);
        }

        public async Task<string> GetFilePath(int objId, ObjType objType)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");
            FileModel file = await FindAll().Where(f => f.RefId == objId && f.RefGid == objType).OrderByDescending(f => f.ALLFId).FirstOrDefaultAsync();
            string targetPath = Path.Combine(targetFolder, file.ALLFId.ToString());

            return targetPath;
        }

        public string GetFilePath(int imgId)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");
            string targetPath = Path.Combine(targetFolder, imgId.ToString());

            return targetPath;
        }

        public async Task<string> GetFileName(int imgId)
        {
            FileModel file = await FindAll().FirstOrDefaultAsync(f => f.ALLFId == imgId);

            string fileName = file.Name;

            return fileName;
        }

        public async Task<string> GetFileName(int objId, ObjType objType)
        {
            FileModel file = await FindAll().Where(f => f.RefId == objId && f.RefGid == objType).OrderByDescending(f => f.ALLFId).FirstOrDefaultAsync();

            string fileName = file.Name;

            return fileName;
        }

        public async Task Download(int imgId, string path)
        {
            var file = await FindAll().FirstOrDefaultAsync(f => f.ALLFId == imgId);

            byte[] data = await GetFile(file.RefId, file.RefGid, false);

            await WriteAllBytesAsync(path, data);
        }

        public async Task<List<Tuple<int, int, byte[]>>> GetFile(ObjType objType)
        {
            try
            {
                List<Tuple<int, int, byte[]>> datas = new List<Tuple<int, int, byte[]>>();

                List<Tuple<int, int, string>> paths = await GetFileNames2(objType);

                List<Task<byte[]>> tasks = paths.Select(x => ReadAllBytesAsync(x.Item3)).ToList();

                await Task.WhenAll(tasks);

                int i = 0;

                foreach (var item in paths)
                {
                    if (File.Exists(item.Item3))
                    {
                        datas.Add(new Tuple<int, int, byte[]>(item.Item1, item.Item2, await tasks[i]));
                    }

                    i++;
                }

                return datas;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Dictionary<int, byte[]>> GetFile(List<int> objId, ObjType objType)
        {
            try
            {
                Dictionary<int, byte[]> datas = new Dictionary<int, byte[]>();

                Dictionary<int, string> paths = await GetFileNames(objId, objType);

                List<Task<byte[]>> tasks = paths.Select(x => ReadAllBytesAsync(x.Value)).ToList();

                await Task.WhenAll(tasks);

                int i = 0;

                foreach (var item in paths)
                {
                    datas.Add(item.Key, await tasks[i]);

                    i++;
                }

                try
                {
                    return datas;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<List<Tuple<int, string, byte[]>>> GetFileWithNames(List<int> objId, ObjType objType)
        {
            try
            {
                List<Tuple<int, string, byte[]>> datas = new List<Tuple<int, string, byte[]>>();

                List<Tuple<int, string, string>> paths = await GetFileNamesWithNames(objId, objType);

                List<Task<byte[]>> tasks = paths.Select(x => ReadAllBytesAsync(x.Item2)).ToList();

                await Task.WhenAll(tasks);

                int i = 0;

                foreach (var item in paths)
                {
                    datas.Add(new Tuple<int, string, byte[]>(item.Item1, item.Item3, await tasks[i]));

                    i++;
                }

                try
                {
                    return datas;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<dynamic> GetFile(int objId, ObjType objType, bool FileList = false)
        {
            try
            {
                if (!FileList)
                {
                    string fileName = await GetFileName(objId, objType);

                    return await ReadAllBytesAsync(fileName);
                }
                else
                {
                    return await GetFile(objId, objType);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<dynamic> GetObject(int objId, ObjType objType, bool FileList = false)
        {
            try
            {
                if (!FileList)
                    return await GetFileObject(objId, objType);
                else
                {
                    return await GetFileObjects(objId, objType);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Task<byte[]> GetFile(int imgId)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            string targetPath = Path.Combine(targetFolder, imgId.ToString());

            return ReadAllBytesAsync(targetPath);
        }

        public async Task<List<byte[]>> GetFile(int objId, ObjType objType)
        {
            List<byte[]> datas = new List<byte[]>();

            List<string> paths = await GetFileNames(objId, objType);


            List<Task<byte[]>> tasks = paths.Select(ReadAllBytesAsync).ToList();

            await Task.WhenAll(tasks);

            int i = 0;

            foreach (var item in paths)
            {
                datas.Add(await tasks[i]);

                i++;
            }

            try
            {
                return datas;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task RemoveFile(int imgId)
        {
            FileModel file = new FileModel { ALLFId = imgId };

            Attach(file);

            Delete(file);

            await CommitAsync();

            await RemoveFileAsync(GetFilePath(imgId));
        }

        public Task RemoveFilelocal(int imgId)
        {
            return RemoveFileAsync(GetFilePath(imgId));
        }

        public Task AddFilelocal(int imgId, byte[] data)
        {
            return WriteAllBytesAsync(GetFilePath(imgId), data);
        }

        public async Task RemoveFile(List<int> imgIds)
        {
            List<Task> tasks = imgIds.Select(x => RemoveFileAsync(GetFilePath(x))).ToList();

            foreach (int item in imgIds)
            {
                FileModel al = new FileModel { ALLFId = item };

                Attach(al);

                Delete(al);
            }

            await CommitAsync();

            await Task.WhenAll(tasks);
        }

        public async Task<List<string>> GetFileNames(int objId, ObjType objType)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            List<FileModel> File = await FindAll().Where(f => f.RefId == objId && f.RefGid == objType).ToListAsync();

            List<string> targetPaths = new List<string>();

            foreach (var item in File)
            {
                targetPaths.Add(Path.Combine(targetFolder, item.ALLFId.ToString()));
            }

            return targetPaths;
        }

        public async Task<Dictionary<int, string>> GetFileNames(ObjType objType)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");
            List<FileModel> File = await FindAll().Where(f => f.RefGid == objType).GroupBy(x => x.RefId).Select(x => x.OrderByDescending(a => a.ALLFId).FirstOrDefault()).ToListAsync();

            Dictionary<int, string> targetPaths = new Dictionary<int, string>();

            foreach (var item in File)
            {
                targetPaths.Add(item.RefId, Path.Combine(targetFolder, item.ALLFId.ToString()));
            }

            return targetPaths;
        }

        public async Task<List<Tuple<int, int, string>>> GetFileNames2(ObjType objType)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            List<FileModel> File = await FindAll().Where(f => f.RefGid == objType).GroupBy(x => x.RefId).Select(x => x.OrderByDescending(a => a.ALLFId).FirstOrDefault()).ToListAsync();

            List<Tuple<int, int, string>> targetPaths = new List<Tuple<int, int, string>>();

            foreach (var item in File)
            {
                targetPaths.Add(new Tuple<int, int, string>(item.ALLFId, item.RefId, Path.Combine(targetFolder, item.ALLFId.ToString())));
            }

            return targetPaths;
        }

        public async Task<Dictionary<int, string>> GetFileNames(List<int> ids, ObjType objType)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            List<FileModel> File = await FindAll().Where(f => f.RefGid == objType && ids.Contains(f.RefId)).GroupBy(x => x.RefId).Select(x => x.OrderByDescending(a => a.ALLFId).FirstOrDefault()).ToListAsync();

            Dictionary<int, string> targetPaths = new Dictionary<int, string>();

            foreach (var item in File)
            {
                targetPaths.Add(item.RefId, Path.Combine(targetFolder, item.ALLFId.ToString()));
            }

            return targetPaths;
        }

        public async Task<List<FileModel>> GetFileObjects(int refId, ObjType objType)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            List<FileModel> File = await FindAll().OrderByDescending(x => x.ALLFId).Where(f => f.RefGid == objType && f.RefId == refId).ToListAsync();

            foreach (var item in File)
            {
                item.Data = await ReadAllBytesAsync(Path.Combine(targetFolder, item.ALLFId.ToString()));
            }

            return File;
        }

        public async Task<FileModel> GetFileObject(int refId, ObjType objType)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            FileModel file = await FindAll().OrderByDescending(x => x.ALLFId).FirstOrDefaultAsync(f => f.RefGid == objType && f.RefId == refId);

            file.Data = await ReadAllBytesAsync(Path.Combine(targetFolder, file.ALLFId.ToString()));

            return file;
        }

        public async Task<List<Tuple<int, string, string>>> GetFileNamesWithNames(List<int> ids, ObjType objType)
        {
            string targetFolder = HostingEnvironment.MapPath("~/Uploads");

            List<FileModel> File = await FindAll().Where(f => f.RefGid == objType && ids.Contains(f.RefId)).GroupBy(x => x.RefId).Select(x => x.OrderByDescending(a => a.ALLFId).FirstOrDefault()).ToListAsync();

            List<Tuple<int, string, string>> targetPaths = new List<Tuple<int, string, string>>();

            foreach (var item in File)
            {
                targetPaths.Add(new Tuple<int, string, string>(item.RefId, Path.Combine(targetFolder, item.ALLFId.ToString()), item.Name));
            }

            return targetPaths;
        }
    }
}
