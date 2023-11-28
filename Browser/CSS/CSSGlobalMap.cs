namespace Browser.CSS;

public class CSSGlobalMap
{
    Dictionary<string, CSSAttrMap> map = new Dictionary<string, CSSAttrMap>();
    
    public Dictionary<string, CSSAttrMap> getMap()
    {
        return map;
    }
}