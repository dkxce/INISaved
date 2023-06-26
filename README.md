# INISaved

**INI Serializer**

Object <--> INI File    

based on XMLSerializer, xml serialized object as xml converts to ini and back.

**Usage**:
```C#
using  System.Xml;

...
    public class Config
    {
        public string name;
        public int intval;
        public byte[] data;
        public bool boolval;
        public SubConfig sub;
    }

    public class SubConfig
    {
        [XmlAttribute]
        public string key;
        [XmlText]
        public string value;
    }
...

        Config cfg_save = new Config();
        IniSaved<Config>.Save(Path.Combine(IniSaved<int>.CurrentDirectory(), "Config.ini"), cfg_save);
        Config cfg_load = IniSaved<Config>.Load(Path.Combine(IniSaved<int>.CurrentDirectory(), "Config.ini"),);

...
```

**Sample**:
```ini
;
;IniSaved File UTF-8
;[section]
;@attr|param=value
;@ -> \u0040, ; -> \u003B, # -> \u0023, \r -> \u000D, \n -> \u000A
;

[Config]
name=Config public string name
intval=10128
data=Af8=
boolval=false

[Config.sub]
@key=SubConfig public string key
@=SubConfig public string value
```
