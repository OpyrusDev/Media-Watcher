using System;
using MediaWatcher.Models;
using MediaWatcher.Services;

namespace MediaWatcher;

public class FileIndexRepository
{
    private readonly List<FileMetadata> _indexedFiles = new List<FileMetadata>();

    private readonly FileIndexService _service;

    public FileIndexRepository(FileIndexService service){
        _service = service;
    }


    public void IndexFiles(string directoryPath)
    {
        _indexedFiles.Clear();
        _indexedFiles.AddRange(_service.IndexFiles(directoryPath));
    }

    public List<FileMetadata> GetIndexedFiles()
    {
        return _indexedFiles;
    }
}
