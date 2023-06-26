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

**Sample**:
```ini
﻿;
;IniSaved File UTF-8
;[section]
;@attr|param=value
;@ -> \u0040, ; -> \u003B, # -> \u0023, \r -> \u000D, \n -> \u000A
;

[Configuration]
WebUrl=http://localhost:8081/index.html
WebPort=8081
FormAllowMaximize=true
FormAllowMinimize=true
FormKeepWinPos=true
FormShowStatus=false
FormEnableLog=true
FormShowWaitBox=true
FormShowProgress=false
FormTaskProgress=true
FormMode=Dark
ShowMaximized=true
ServerBaseURL=https://localhost/api/
HighDPIMode=true
CustomCaption=1
```
