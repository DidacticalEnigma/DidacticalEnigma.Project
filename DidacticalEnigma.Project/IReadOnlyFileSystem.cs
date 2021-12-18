using System.Collections.Generic;
using System.IO;

namespace DidacticalEnigma.Project
{
    // Filesystem abstraction
    public interface IReadOnlyFileSystem
    {
        // open a file relative to root of this object
        Stream FileOpen(string path);

        // list all the files and directories directly contained by directory at given path
        // can be empty, which means listing files and directories at root 
        // the listed paths are relative to the root of this object
        // so they can be passed directly to the FileOpen function
        // directories are indicated by the trailing slash character
        IEnumerable<string> List(string path);
    }
}