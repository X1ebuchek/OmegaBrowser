namespace Browser.Networking;

public class Resource
{
    public enum ResourceType
    {
        Html, Js, Css, Img
    }
    public string host { get; set; }
    public string path { get; set; }
    public string? localPath { get; set; }
    public ResourceType type { get; init; }

    public Resource(string path, ResourceType type)
    {
        this.path = path;
        this.type = type;
    }

    public override string ToString()
    {
        return $"{type}, {path}" + (string.IsNullOrEmpty(localPath) ? "" : $", {localPath}");
    }
}