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

        try
        {
            var response = client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                using var fs = new FileStream(path, FileMode.OpenOrCreate);
                response.Content.CopyToAsync(fs).Wait();
                return true;
            }
            else
            {
                Console.Error.WriteLine($"Bad Url: {url}"); //todo include in logger
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception while downloading {url}: {ex.Message}");
            return false;
        }
        
    }

    public bool GetResource(string url, out string fileName)
    {
        var myUri = new Uri(url);
        var host = myUri.Host;
        var path = myUri.AbsoluteUri;

        if (!_data.ContainsKey(host))
        {
            _data.Add(host, new Dictionary<string, string>());
        }

        _data[host].TryGetValue(path, out fileName);

        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName)) return true;
        
        fileName = Path.Combine(_dataPath, $"{host}__{Util.ComputeHash(path)}");
        if (DownloadResource(url, fileName))
        {
            if (!_data[host].ContainsKey(path))
            {
                _data[host].Add(path, fileName);
            }
            else
            {
                _data[host][path] = fileName;
            }
            return true;
        }
        else
        {
            fileName = null;
            return false;
        }
        
    }

    public void ClearCacheByUrl(string url)
    {
        var myUri = new Uri(url);
        var host = myUri.Host;
        _data[host] = new Dictionary<string, string>();
    }
}
