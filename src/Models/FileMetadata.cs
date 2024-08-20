using System;

namespace MediaWatcher.Models;

public class FileMetadata
{
    public string Name { get; set; }
    public string Path { get; set; }
    public string Extension {get; set;}
    public long Size { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsDirectory { get; set; }
}
