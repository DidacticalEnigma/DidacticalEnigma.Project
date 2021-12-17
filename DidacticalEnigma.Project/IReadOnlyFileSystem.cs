using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DidacticalEnigma.Project
{
    public interface IReadOnlyFileSystem
    {
        Stream FileOpen(string path);

        IEnumerable<string> List(string path);
    }

    public class ReadOnlyFileSystem : IReadOnlyFileSystem
    {
        private readonly string rootPath;

        public ReadOnlyFileSystem(string rootPath)
        {
            this.rootPath = rootPath;
        }
        
        public Stream FileOpen(string path)
        {
            return File.OpenRead(Path.Combine(this.rootPath, path));
        }

        public IEnumerable<string> List(string path)
        {
            return new DirectoryInfo(Path.Combine(rootPath, path))
                .EnumerateFileSystemInfos()
                .Select(info => (info.Attributes & FileAttributes.Directory) != 0 ? info.Name + "/" : info.Name)
                .Select(p => Path.Combine(path, p));
        }
    }
}