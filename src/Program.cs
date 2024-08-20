using System.Net;
using System.Text.Json;
using MediaWatcher;
using MediaWatcher.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<FileIndexService>();
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

if (!File.Exists(configFile)){
    Dictionary<string, object> newConfig = new(){
        {"LibraryPath", Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "/"}
    };
    File.WriteAllText(configFile, JsonSerializer.Serialize(newConfig));
} 

var configFileContent = File.ReadAllText(configFile);
var config = JsonSerializer.Deserialize<Dictionary<string, object>>(configFileContent);

FileIndexService.LibraryPath = config!["LibraryPath"].ToString()! + "/";

app.Urls.Add(Data.address);
var localIP = "127.0.0.1";
        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                var IpValue = ip.ToString();

                if(IpValue == "127.0.0.1" || IpValue == "127.0.1.1") continue;
                localIP = ip.ToString();
                break;
            }
            Data.address = $"http://{localIP}:5000";
            app.Urls.Add(Data.address);
        }
        catch { }

app.Run();
