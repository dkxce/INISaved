# INISaved

**INI Serializer**

Object <--> INI File    

based on XMLSerializer, xml serialized object as xml converts to ini and back.

**Usage**:
```C#
using  System.Xml;

...

Config cfg_save = new Config();
IniSaved<Config>.Save(Path.Combine(IniSaved<int>.CurrentDirectory(), "Config.ini"), cfg_save);
Config cfg_load = IniSaved<Config>.Load(Path.Combine(IniSaved<int>.CurrentDirectory(), "Config.ini"),);

...
```
