using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Repositories.IRepo
{
    public interface IFileRepository : IEFRepo<FileModel>
    {
        Task AddFile(string name, int objId, ObjType objType, byte[] file);

        string GetExtension(string fileName);

        Task AddFileFromObject(FileModel file);

        Task AdFileFromObjectsList(List<FileModel> File);

        Task AddFile(List<Tuple<string, int, ObjType, byte[]>> File);

        Task<string> GetFilePath(int objId, ObjType objType);

        string GetFilePath(int imgId);

        Task<string> GetFileName(int imgId);

        Task<string> GetFileName(int objId, ObjType objType);

        Task Download(int imgId, string path);

        Task<List<Tuple<int, int, byte[]>>> GetFile(ObjType objType);

        Task<Dictionary<int, byte[]>> GetFile(List<int> objId, ObjType objType);

        Task<List<Tuple<int, string, byte[]>>> GetFileWithNames(List<int> objId, ObjType objType);

        Task<dynamic> GetFile(int objId, ObjType objType, bool FileList = false);

        Task<dynamic> GetObject(int objId, ObjType objType, bool FileList = false);

        Task<byte[]> GetFile(int imgId);

        Task<List<byte[]>> GetFile(int objId, ObjType objType);

        Task RemoveFile(int imgId);

        Task RemoveFilelocal(int imgId);

        Task AddFilelocal(int imgId, byte[] data);

        Task RemoveFile(List<int> imgIds);

        Task<List<string>> GetFileNames(int objId, ObjType objType);

        Task<Dictionary<int, string>> GetFileNames(ObjType objType);

        Task<List<Tuple<int, int, string>>> GetFileNames2(ObjType objType);

        Task<Dictionary<int, string>> GetFileNames(List<int> ids, ObjType objType);

        Task<List<FileModel>> GetFileObjects(int refId, ObjType objType);

        Task<FileModel> GetFileObject(int refId, ObjType objType);

        Task<List<Tuple<int, string, string>>> GetFileNamesWithNames(List<int> ids, ObjType objType);
    }
}
