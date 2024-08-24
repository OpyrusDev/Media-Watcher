using System.Net;
using System.Text.Json;
using MediaWatcher;
using MediaWatcher.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<VideoIndexService>();
builder.Services.AddSingleton<MusicIndexService>();
builder.Services.AddSingleton<FileIndexRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

var configFile = Path.Combine(Environment.CurrentDirectory, "config.json");

if (!File.Exists(configFile))
{
    
   VideoIndexService.VideoLibraryPath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
    MusicIndexService.MusicLibraryPath =  Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);

    Dictionary<string, object> newConfig =
        new()
        {
            { "VideoLibrary",  VideoIndexService.VideoLibraryPath},
            { "MusicLibrary", MusicIndexService.MusicLibraryPath},
        };
    File.WriteAllText(configFile, JsonSerializer.Serialize(newConfig));
}else{

    var configFileContent = File.ReadAllText(configFile);
    var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configFileContent);

    try
    {
        VideoIndexService.VideoLibraryPath = config!["VideoLibrary"].ToString()!;
        MusicIndexService.MusicLibraryPath = config!["MusicLibrary"].ToString()!;
    }
    catch
    {
        throw new Exception("Wrong Config file!");
    }
}


app.Urls.Add("http://localhost:5000");

try
{
    var host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ip in host.AddressList)
    {
        var IpValue = ip.ToString();

        if (
            IpValue == "127.0.0.1"
            || IpValue == "127.0.1.1" 
            || ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6
        )
            continue;
        app.Urls.Add($"http://{IpValue}:5000");
    }
}
catch { }
app.Run();
