# INISaved

**INI Serializer**

Object <--> INI File    

based on XMLSerializer, xml serialized object as xml converts to ini and back.

**Usage**:
```C#
using  System.Xml;

...
    // Write CONFIG
    {
        IniSaved<System.Xml.IniSavedSample.SampleConfig>.presaveFileHeader = true;
        IniSaved<System.Xml.IniSavedSample.SampleConfig>.presaveXmlSerialization = false;                
        IniSaved<System.Xml.IniSavedSample.SampleConfig>.customHeaderLine = "Original at https://github.com/dkxce/INISaved";                
        System.Xml.IniSavedSample.SampleConfig CONFIG = new System.Xml.IniSavedSample.SampleConfig();
        string iniData = IniSaved<System.Xml.IniSavedSample.SampleConfig>.Save(CONFIG);
        IniSaved<System.Xml.IniSavedSample.SampleConfig>.Save("sample.ini", CONFIG);
    };

    // Read CONFIG
    {
        System.Xml.IniSavedSampleBlank.SampleConfig CONFIG = new System.Xml.IniSavedSampleBlank.SampleConfig();
        CONFIG = IniSaved<System.Xml.IniSavedSampleBlank.SampleConfig>.Load("sample.ini");
    };
...
```

**Short Sample**:
```ini
;
;IniSaved File UTF-8
;Original at https://github.com/dkxce/INISaved
;[section]
;@attr|param=value
;@ -> \u0040, ; -> \u003B, # -> \u0023, \r -> \u000D, \n -> \u000A
;

[DEFAULT]
Name=SampleConfiguration
Age=18
BirthDay=2023-06-27T00:00:00+03:00
Weight=101
Length=777
Man=true
aBytes=ARD/Cg3NOyM=
```
**Full Sample**:    
- [CONFIG](IniSavedSample.cs)
- [INIFile](sample.ini)
- [INIFile+XML](sample_with_xml.ini)
- [XML](sample.xml)
