namespace Browser.Networking;

public class ResourceManager
{
    private readonly string _dataPath;

    public ResourceManager(string dataPath)
    {
        this._dataPath = dataPath;
    }


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

    public bool GetResource(ref Resource resource)
    {
        Uri myUri;
        if (resource.path.StartsWith("/"))
        {
            myUri = new Uri(resource.host + resource.path);
        }
        else
        {
            myUri = new Uri(resource.path);
        }

        var host = myUri.Host;
        var path = myUri.AbsoluteUri;

        var fileName = resource.localPath;

        if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName)) return true;

        fileName = Path.Combine(_dataPath, $"{host}__{ResourceUtil.ComputeHash(path)}");
        if (DownloadResource(path, fileName))
        {
            resource.localPath = fileName;
            return true;
        }
        else
        {
            resource.localPath = null;
            return false;
        }
    }

    public void ClearCacheByTab(List<Resource> resources)
    {
        foreach (var resource in resources)
        {
            if (resource.localPath != null && File.Exists(resource.localPath))
            {
                File.Delete(resource.localPath);
            }
        }
    }
}