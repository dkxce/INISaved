//
// C# (cross-platform)
// IniSaved
// v 0.1, 26.06.2023
// https://github.com/dkxce/INISaved
// en,ru,1251,utf-8
//

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;

namespace System.Xml
{
    /// <summary>
    ///     Ini Serializer (XML <--> INI)
    ///     Supports ; & # (using XMLSerializer)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class IniSaved<T>
    {
        public static bool presaveFileHeader = true;
        public static string customHeaderLine = null;

        #region SAVE

        public static void SaveHere(string file, T obj, string section = null)
        {
            Save(Path.Combine(CurrentDirectory(), file), obj, section);
        }

        public static void Save(string file, Type type, object obj, string section = null)
        {
            using (StreamWriter sw = new StreamWriter(file, false, Encoding.UTF8))
            {
                string chl = string.IsNullOrEmpty(customHeaderLine) ? "" : $";{customHeaderLine}\r\n";
                if (presaveFileHeader) sw.WriteLine($";\r\n;IniSaved File UTF-8\r\n{chl};[section]\r\n;@attr|param=value\r\n;@ -> \\u0040, ; -> \\u003B, # -> \\u0023, \\r -> \\u000D, \\n -> \\u000A\r\n;\r\n");
                sw.Write(Save(type, obj, section));
            };
        }

        public static void Save(string file, T obj, string section = null)
        {
            Save(file, typeof(T), obj, section);
        }

        public static void Save(StreamWriter file, T obj, string section = null)
        {
            file.Write(Save(typeof(T), obj, section));
        }

        public static string Save(T obj, string section = null)
        {
            return Save(typeof(T), obj, section);
        }

        public static string Save(Type type, object obj, string section = null)
        {
            if (string.IsNullOrEmpty(section))
            {
                section = typeof(T).Name;
                IniNameAttribute ina = typeof(T).GetCustomAttribute<IniNameAttribute>();
                if (ina != null && !String.IsNullOrEmpty(ina.name)) section = ina.name;
            };

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); ns.Add("", "");
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            StreamWriter writer = new StreamWriter(ms);
            xs.Serialize(writer, obj, ns);
            writer.Flush();
            ms.Position = 0;
            byte[] bb = new byte[ms.Length];
            ms.Read(bb, 0, bb.Length);
            writer.Close();
            string xml = Encoding.UTF8.GetString(bb);

            // XML -> INI
            XmlDocument xd = new XmlDocument();
            xd.LoadXml(xml);
            Dictionary<string, int> sections = new Dictionary<string, int>();
            string res = GetIni(xd.DocumentElement, section, sections);
            return res;
        }

        private static string GetIni(XmlNode xn, string section, Dictionary<string, int> sections)
        {                        
            bool hasRoot = false;
            bool hasBody = false;

            section = NormalizeValue(section);
            if (sections.ContainsKey(section)) section = $"{section}.{sections[section]++}";
            else sections.Add(section, 1);

            StringBuilder sb = new StringBuilder();

            if (xn.Attributes != null)
                foreach (XmlAttribute a in xn.Attributes)
                {
                    AddRoot(ref hasRoot, sb, section);                    
                    sb.Append($"@{NormalizeValue(a.Name, true)}={NormalizeValue(a.Value)}\r\n");
                    hasBody = true;
                };

            List<XmlNode> simpleNodes = new List<XmlNode>();
            List<XmlNode> detailNodes = new List<XmlNode>();

            if(xn.HasChildNodes)
                foreach (XmlNode n in xn.ChildNodes)
                {
                    if (n.NodeType == XmlNodeType.Text)
                    {
                        AddRoot(ref hasRoot, sb, section);                        
                        sb.Append($"@={NormalizeValue(n.Value)}\r\n");
                        hasBody = true;
                    }
                    else if (n.HasChildNodes && n.ChildNodes.Count == 1 && n.ChildNodes[0].NodeType == XmlNodeType.Text && (n.Attributes == null || n.Attributes.Count == 0))
                        simpleNodes.Add(n);
                    else
                        detailNodes.Add(n);                    
                };

            foreach(XmlNode n in simpleNodes)
            {
                AddRoot(ref hasRoot, sb, section);                
                sb.Append($"{NormalizeValue(n.Name, true)}={NormalizeValue(n.ChildNodes[0].Value)}\r\n");
                hasBody = true;
            };

            if(simpleNodes.Count > 0) sb.Append("\r\n");

            foreach (XmlNode n in detailNodes)
            {
                string sn = $"{section}.{NormalizeValue(n.Name)}";
                string dnt = GetIni(n, sn, sections);
                sb.Append(dnt);
                hasBody = true;
            };

            if(hasBody) sb.Append("\r\n");
            return sb.ToString();
        }

        private static void AddRoot(ref bool hasRoot, StringBuilder sb, string section)
        {
            if (!hasRoot)
            {
                sb.Append($"[{section}]\r\n");
                hasRoot = true;
            };
        }

        #endregion SAVE

        #region Load
        public static void Load(StreamReader sr, ref T obj, string section = null)
        {
            object v = obj;
            Load(sr, typeof(T), ref v, section);
            obj = (T)v;
        }

        public static T LoadHere(string file, string section = null)
        {
            return Load(Path.Combine(CurrentDirectory(), file, section));
        }

        public static T LoadFromText(string text, string section = null)
        {
            Type t = typeof(T);
            T obj;
            if (IsSimple(t))
                obj = t == typeof(string) ? (T)Convert.ChangeType(null, typeof(T)) : (T)Activator.CreateInstance(typeof(T));
            else if (t.IsArray)
                obj = (T)Activator.CreateInstance(typeof(T), 0);
            else
            {
                System.Reflection.ConstructorInfo c = t.GetConstructor(new Type[0]);
                obj = (T)c.Invoke(null);
            };
            using (MemoryStream fs = new MemoryStream(Encoding.UTF8.GetBytes(text))) Load(new StreamReader(fs), ref obj, section);
            return obj;
        }

        public static T Load(string file, string section = null)
        {
            Type t = typeof(T);
            T obj;
            if (IsSimple(t))
                obj = t == typeof(string) ? (T)Convert.ChangeType(null, typeof(T)) : (T)Activator.CreateInstance(typeof(T));
            else if (t.IsArray)
                obj = (T)Activator.CreateInstance(typeof(T), 0);
            else
            {
                System.Reflection.ConstructorInfo c = t.GetConstructor(new Type[0]);
                obj = (T)c.Invoke(null);
            };
            using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read)) Load(new StreamReader(fs), ref obj, section);
            return obj;
        }

        public static void Load(StreamReader sr, Type type, ref object obj, string section = null)
        {
            string xmlbase = "";
            if(!string.IsNullOrEmpty(section))
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces(); ns.Add("", "");
                XmlSerializer xxs = new XmlSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                StreamWriter writer = new StreamWriter(ms);
                xxs.Serialize(writer, obj, ns);
                writer.Flush();
                ms.Position = 0;
                byte[] bb = new byte[ms.Length];
                ms.Read(bb, 0, bb.Length);
                writer.Close();
                XmlDocument xxd = new XmlDocument();
                xxd.LoadXml(Encoding.UTF8.GetString(bb));
                xmlbase = xxd.DocumentElement.Name;
            };

            if (string.IsNullOrEmpty(section))
            {
                section = typeof(T).Name;
                IniNameAttribute ina = typeof(T).GetCustomAttribute<IniNameAttribute>();
                if (ina != null && !String.IsNullOrEmpty(ina.name)) section = ina.name;
            };

            // READ INI SECTIONS
            List<KeyValuePair<string, List<string>>> secSorted = null;
            {                
                string currsect = "";
                Dictionary<string, List<string>> sections = new Dictionary<string, List<string>>();
                while (!sr.EndOfStream)
                {
                    string ln = sr.ReadLine().Trim();
                    if (string.IsNullOrEmpty(ln) || ln.StartsWith("#") || ln.StartsWith(";")) continue;
                    if (ln.StartsWith("[")) { 
                        currsect = ReversizeValue(ln.Split('#', ';')[0].Trim('[', ']')); 
                        sections.Add(currsect, new List<string>()); continue; 
                    };
                    if (string.IsNullOrEmpty(currsect)) continue;
                    if (ln.IndexOf("=") < 0) continue;
                    ln = ln.Split(new char[] { ';', '#' })[0];
                    sections[currsect].Add(ln);
                };
                // SORT
                secSorted = new List<KeyValuePair<string, List<string>>>(sections);
                secSorted.Sort((KeyValuePair<string, List<string>> a, KeyValuePair<string, List<string>> b) => a.Key.CompareTo(b.Key));
                // REPLACE ROOT
                if(secSorted.Count > 0 && !string.IsNullOrEmpty(section) && !string.IsNullOrEmpty(xmlbase))
                {
                    List<KeyValuePair<string, List<string>>> tmps = new List<KeyValuePair<string, List<string>>>();
                    foreach (KeyValuePair<string, List<string>> kvp in secSorted)
                    {
                        if (kvp.Key == section || kvp.Key.StartsWith($"{section}."))
                        {
                            string kName = kvp.Key == section ? xmlbase : xmlbase + "." + kvp.Key.Substring(section.Length + 1);
                            tmps.Add(new KeyValuePair<string, List<string>>(kName, kvp.Value));
                        }
                        else tmps.Add(kvp);
                    };
                    secSorted = tmps;
                };
            };

            // INI -> XML
            XmlDocument xd = new XmlDocument();
            XmlDeclaration xml_declaration = xd.CreateXmlDeclaration("1.0", "utf-8", null);
            xd.InsertBefore(xml_declaration, xd.DocumentElement);

            foreach(KeyValuePair<string, List<string>> s in secSorted)
            {
                string fullKey = s.Key;
                string shrtKey = s.Key;

                // Normalize Key (strip .\d$)
                {
                    int iof = fullKey.LastIndexOf(".");
                    if (iof > 0 && int.TryParse(fullKey.Substring(iof + 1), out _)) 
                        fullKey = fullKey.Substring(0, iof);
                };

                // Find/Create Path
                XmlNode thisRoot = xd;
                if (fullKey.Contains("."))
                {                    
                    string[] paths = fullKey.Split('.');
                    for (int i = 0; i < paths.Length - 1; i++)
                    {
                        XmlNode sn = thisRoot.SelectSingleNode(paths[i]+"[last()]");
                        if (sn == null) sn = thisRoot.AppendChild(xd.CreateElement(paths[i]));
                        thisRoot = sn;
                    };
                    shrtKey = paths[paths.Length - 1];
                };

                // Create Node
                XmlNode xn = xd.CreateNode(XmlNodeType.Element, null, shrtKey, null);
                thisRoot = thisRoot.AppendChild(xn);
                
                // Fill Node
                List<KeyValuePair<string,string>> attrs = new List<KeyValuePair<string, string>>();
                List<KeyValuePair<string, string>> vars = new List<KeyValuePair<string, string>>();
                foreach (string line in s.Value)
                {
                    string[] name_value = line.Split(new char[] { '=' }, 2);
                    string name = ReversizeValue(name_value[0].Trim(), true);
                    string value = ReversizeValue(name_value[1].Trim());

                    if (name == "@")
                    {
                        XmlText t = xd.CreateTextNode(value);
                        xn.AppendChild(t);
                    }
                    else if (name.StartsWith("@"))
                    {
                        (xn as XmlElement).SetAttribute(name.Substring(1), value);
                    }
                    else
                    {
                        XmlText t = xd.CreateTextNode(value);
                        XmlElement e = xd.CreateElement(name);
                        e.AppendChild(t);
                        xn.AppendChild(e);
                    };
                };
            };
            
            string res = xd.OuterXml;

            XmlSerializer xs = new XmlSerializer(typeof(T));
            StreamReader reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(res)));
            T c = (T)xs.Deserialize(reader);
            reader.Close();
            obj = c;
        }

        #endregion LOAD         

        public static string CurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
            // return Application.StartupPath;
            // return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            // return Directory.GetCurrentDirectory();
            // return Environment.CurrentDirectory;
            // return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            // return Path.GetDirectory(Application.ExecutablePath);
        }

        public static bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple((type.GetGenericArguments()[0]).GetTypeInfo());
            }
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        public static string NormalizeValue(string value, bool name = false)
        {
            if(name) value = value.Replace("@", "\\u0040");
            value = value.Replace(";", "\\u003B");
            value = value.Replace("#", "\\u0023");
            value = value.Replace("\\r", "\\u000D");
            value = value.Replace("\\n", "\\u000A");
            return value;
        }

        public static string ReversizeValue(string value, bool name = false)
        {
            if (name) value = value.Replace("\\u0040", "@");
            value = value.Replace("\\u003B", ";");
            value = value.Replace("\\u0023", "#");
            value = value.Replace("\\u000D", "\\r");
            value = value.Replace("\\u000A", "\\n");
            return value;
        }
    }    

    public class IniNameAttribute : System.Attribute
    {
        public string name { set; get; }
        public IniNameAttribute(string name) { this.name = name; }
    }    
}
