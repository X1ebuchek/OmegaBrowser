using System.Security.Cryptography;
using System.Text;
using Browser.Management;

namespace Browser.Networking;

public class ResourceManager
{
    private readonly string _dataPath;

    public ResourceManager(string dataPath)
    {
        this._dataPath = dataPath;
    }
    
    private Dictionary<string, Dictionary<string, string>> _data = new();

    

    private static bool DownloadResource(string url, string path)
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = client.Send(request);

        if (response.IsSuccessStatusCode)
        {
            using var fs = new FileStream(path, FileMode.OpenOrCreate);
            response.Content.CopyToAsync(fs);
            return true;
        }
        else
        {
            //Console.Error.WriteLine($"Bad Url: {url}"); //todo include in logger
            return false;
        }
        
    }

    public bool GetResource(string url, out string fileName)
    {
        var myUri = new Uri(url);
        var host = myUri.Host;
        var path = myUri.AbsolutePath;

        if (!_data.ContainsKey(host))
        {
            _data.Add(host, new Dictionary<string, string>());
        }

        _data[host].TryGetValue(path, out fileName);

        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName)) return true;
        
        fileName = Path.Combine(_dataPath, $"{host}__{Util.ComputeHash(path)}");
        if (DownloadResource(url, fileName))
        {
            _data[host].Add(path, fileName);
            return true;
        }
        else
        {
            fileName = null;
            return false;
        }
        
    }

    public void ClearCacheByDomain(string domain)
    {
        _data[domain] = new Dictionary<string, string>();
    }
}
