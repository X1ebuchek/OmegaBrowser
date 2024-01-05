using System.Text;

namespace Browser.CSS;

public class CssParser
{
    public static CSSGlobalMap ParseString(string cssString)
    {
        byte[] byteArray = Encoding.UTF8.GetBytes(cssString.Trim());
        MemoryStream stream = new MemoryStream(byteArray);
        StreamReader sr = new StreamReader(stream);
        return Parse(sr);
    }
    
    public static CSSGlobalMap ParseFile(string path)
    {
        StreamReader sr = File.OpenText(path);
        return Parse(sr);
    }

    public static CSSAttrMap ParseInline(string cssString)
    {

        var map = new CSSAttrMap(); 
        
        foreach (var attr in cssString.Split(";"))
        {
            var kv = attr.Split(":");
            try
            {
                map.getMap()[kv[0].Trim()] = kv[1].Trim();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        return map;
    }
    
    private static CSSGlobalMap Parse(StreamReader sr)
    {
        CSSGlobalMap globalMap = new CSSGlobalMap();
        
        r:while (sr.Peek() != -1)
        {
            char c;
            string key = "";
            while ((char)sr.Peek() != '{' && sr.Peek() != -1)
            {
                c = (char)sr.Read();
                if (c == '@')
                {
                    bool inMediaBlock = true;
                    int countStart = 0;
                    int countEnd = 0;
                    while ((char)sr.Peek() != '{')
                    {
                        c = (char)sr.Read();
                    }

                    while (inMediaBlock)
                    {
                        c = (char)sr.Read();
                        if (c == '{')
                        {
                            countStart++;
                        }

                        if (c == '}')
                        {
                            countEnd++;
                        }

                        if (countStart == countEnd)
                        {
                            inMediaBlock = false;
                        }
                    }

                    goto r;
                    //continue;
                }

                if (c == '/' && (char)sr.Peek() == '*')
                {
                    while (c != '*' || (char)sr.Peek() != '/')
                    {
                        c = (char)sr.Read();
                    }

                    c = (char)sr.Read();
                }
                else
                {
                    key += c;
                }
            }

            if (sr.Peek() == -1)
            {
                return globalMap;
            }

            //Console.WriteLine("key: " + key);
            c = (char)sr.Read();
            CSSAttrMap cssAttrMap = new CSSAttrMap();
            while (sr.Peek() != '}')
            {
                string keyAttr = "";
                string valueAttr = "";
                while ((char)sr.Peek() != '{' && (char)sr.Peek() != ':' && (char)sr.Peek() != '}')
                {
                    c = (char)sr.Read();
                    if (c == '/' && (char)sr.Peek() == '*')
                    {
                        while (c != '*' || (char)sr.Peek() != '/')
                        {
                            c = (char)sr.Read();
                        }

                        c = (char)sr.Read();
                    }
                    else
                    {
                        keyAttr += c;
                    }
                }

                c = (char)sr.Read();

                if (c == '}')
                {
                    goto cancelLoop;
                }

                if (c == '{')
                {
                    bool inBlock = true;
                    int countStart = 2;
                    int countEnd = 0;
                    while (inBlock)
                    {
                        c = (char)sr.Read();
                        if (c == '{')
                        {
                            countStart++;
                        }

                        if (c == '}')
                        {
                            countEnd++;
                        }

                        if (countStart == countEnd)
                        {
                            inBlock = false;
                        }
                    }

                    goto q;
                }

                if (c == ':')
                {
                    while ((char)sr.Peek() != '{' && (char)sr.Peek() != ';' && (char)sr.Peek() != '}')
                    {
                        c = (char)sr.Read();
                        if (c == '/' && (char)sr.Peek() == '*')
                        {
                            while (c != '*' || (char)sr.Peek() != '/')
                            {
                                c = (char)sr.Read();
                            }

                            c = (char)sr.Read();
                        }
                        else
                        {
                            valueAttr += c;
                        }
                    }
                }

                c = (char)sr.Read();
                if (c == '}')
                {
                    cssAttrMap.getMap()[keyAttr.Trim()] = valueAttr.Trim();
                    goto cancelLoop;
                }

                if (c == '{')
                {
                    bool inBlock = true;
                    int countStart = 2;
                    int countEnd = 0;
                    while (inBlock)
                    {
                        c = (char)sr.Read();
                        if (c == '{')
                        {
                            countStart++;
                        }

                        if (c == '}')
                        {
                            countEnd++;
                        }

                        if (countStart == countEnd)
                        {
                            inBlock = false;
                        }
                    }

                    goto q;
                }
                
                cssAttrMap.getMap()[keyAttr.Trim()] = valueAttr.Trim();
                //Console.WriteLine(keyAttr + " " + valueAttr);
            }

            cancelLoop:
            //c = (char)sr.Read();
            if (globalMap.getMap().ContainsKey(key.Trim()))
            {
                foreach (var kvp in cssAttrMap.getMap())
                {
                    globalMap.getMap()[key.Trim()].getMap()[kvp.Key] = kvp.Value;
                }
            }
            else
            {
                globalMap.getMap().Add(key.Trim(), cssAttrMap);
            }
            
            q:
            Console.Write("");
        }


        // foreach (var global in globalMap.getMap())
        // {
        //     Console.WriteLine(global.Key + ": ");
        //     foreach (var map in global.Value.getMap())
        //     {
        //         Console.WriteLine("\t" + map.Key + " : " + map.Value);
        //     }
        // }

        return globalMap;
    }
}