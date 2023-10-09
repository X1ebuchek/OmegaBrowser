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

    

    private static void DownloadResource(string url, string path)
    {
        using var client = new HttpClient();
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        using var response = client.Send(request);

        if (response.IsSuccessStatusCode)
        {
            using var fs = new FileStream(path, FileMode.OpenOrCreate);
            response.Content.CopyToAsync(fs);
        }
        else
        {
            throw new Exception("Bad Url");
        }
        
    }

    public string GetResource(string url)
    {
        var myUri = new Uri(url);
        var host = myUri.Host;
        var path = myUri.AbsolutePath;

        if (!_data.ContainsKey(host))
        {
            _data.Add(host, new Dictionary<string, string>());
        }

        _data[host].TryGetValue(path, out var fileName);

        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName)) return fileName;
        
        fileName = Path.Combine(_dataPath, $"{host}__{Util.ComputeHash(path)}");
        DownloadResource(url, fileName);
        _data[host].Add(path, fileName);
        
        return fileName;
    }

    public void ClearCacheByDomain(string domain)
    {
        _data[domain] = new Dictionary<string, string>();
    }
}
