using System.Security.Cryptography;
using System.Text;
using HtmlAgilityPack;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace Browser.Networking;

public class ResourceUtil
{
    public static string ComputeHash(string str)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(str));
        return Convert.ToBase64String(hash)
            .TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }
    
    public static List<Resource> GetResources(HtmlDocument document)
        {
            var root = document.DocumentNode;

            var result = new List<Resource>();

            var scriptNodes = root.SelectNodes("//script");
            if (scriptNodes != null)
            {
                result.AddRange(
                    (
                        from script in scriptNodes
                        from attribute in script.Attributes
                        where attribute.Name == "src"
                        select attribute.Value
                    ).Select(
                        resource => new Resource(resource, Resource.ResourceType.Js)
                    )
                );
            }

            var cssNodes = root.SelectNodes("//link[@rel='stylesheet']");
            if (cssNodes != null)
            {
                result.AddRange(
                    (
                        from style in cssNodes
                        from styleAttribute in style.Attributes
                        where styleAttribute.Name == "href"
                        select styleAttribute.Value
                    ).Select(
                        resource => new Resource(resource, Resource.ResourceType.Css)
                    )
                );
            }

            var imgNodes = root.SelectNodes("//img");
            if (imgNodes != null)
            {
                result.AddRange(
                    (
                        from img in imgNodes
                        from imgAttribute in img.Attributes
                        where imgAttribute.Name == "src"
                        select imgAttribute.Value
                    ).Select(
                        resource => new Resource(resource, Resource.ResourceType.Img)
                    )
                );
            }


            return result;
        }

        public static List<Resource> FillResourcesWithLocation(List<Resource> res, string location)
        {
            //todo process relative path too

            var uri = new Uri(location);
            string host;

            if (string.IsNullOrEmpty(uri.UserInfo))
            {
                host = $"{uri.Scheme}://{uri.Host}";
            }
            else
            {
                host = $"{uri.Scheme}://{uri.UserInfo}@{uri.Host}";
            }

            var result = new List<Resource>();
            foreach (var resource in res)
            {
                resource.host = host;
                result.Add(resource);
            }

            return result;
        }
}