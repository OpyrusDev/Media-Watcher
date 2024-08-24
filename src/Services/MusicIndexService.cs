using System;
using System.Net.Http.Headers;
using MediaWatcher.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.StaticFiles;

namespace MediaWatcher.Services;

public class MusicIndexService
{
    public static string MusicLibraryPath {get; set;} = string.Empty;

    readonly string[] authorizedExtensions =  [".mp3", ".ogg"];
    public List<FileMetadata> IndexFiles(string directoryPath = "")
    {
        var files = new List<FileMetadata>();
        var path = Path.Combine(MusicLibraryPath, directoryPath);
        var directoryInfo = new DirectoryInfo(path);

        foreach (var dir in directoryInfo.GetDirectories())
        {
            files.Add(SetMetaData(dir));
        }

        foreach (var file in directoryInfo.GetFiles())
        {
            var hasNoExtension = string.IsNullOrEmpty(file.Extension);
            var isAuthorized = authorizedExtensions.Contains(file.Extension);

            if(!isAuthorized || hasNoExtension || file == null) continue;

            files.Add(SetMetaData(file));
        }

        return files;
    }

    public List<FileMetadata> GetFiles(string directoryPath){

        var files = new List<FileMetadata>();
        var path = Path.Combine(MusicLibraryPath, directoryPath);
        var directoryInfo = new DirectoryInfo(path);
        foreach (var file in directoryInfo.GetFiles())
        {
            var hasNoExtension = string.IsNullOrEmpty(file.Extension);
            var isAuthorized = authorizedExtensions.Contains(file.Extension);

            if(!isAuthorized || hasNoExtension || file == null) continue;

            files.Add(SetMetaData(file));
        }

        return files;
    }

    public List<FileMetadata> GetFolders(string category){
        var files = new List<FileMetadata>();
        var path = Path.Combine(MusicLibraryPath, category);
        var directoryInfo = new DirectoryInfo(path);

        foreach (var dir in directoryInfo.GetDirectories())
        {
            files.Add(SetMetaData(dir));// Recursively index subfolders
        }

        return files;
    }

    public List<FileMetadata> GetCategories(){
        var files = new List<FileMetadata>();
        var directoryInfo = new DirectoryInfo(MusicLibraryPath);

        foreach (var dir in directoryInfo.GetDirectories())
        {
            files.Add(SetMetaData(dir));// Recursively index subfolders
        }

        return files;
    }


    FileMetadata SetMetaData(FileInfo file){
        var fullname = file.Name;

        var splitted = fullname.Split(".").ToList();

        splitted.RemoveAt(splitted.Count - 1);

        var name = string.Concat(splitted);


        return new FileMetadata
            {
                Name = name,
                Path = file.FullName.Replace(MusicLibraryPath, ""),
                Size = file.Length,
                LastModified = file.LastWriteTime,
                Extension = file.Extension.Remove(0),
                IsDirectory = false
            };
    }

    FileMetadata SetMetaData(DirectoryInfo dir){
        return new FileMetadata
            {
                Name = dir.Name,
                Path = dir.FullName.Replace(MusicLibraryPath, ""),
                LastModified = dir.LastWriteTime,
                Extension = string.Empty,
                Size = 0,
                IsDirectory = true
            };
    }
}
