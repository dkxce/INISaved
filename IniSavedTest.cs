using System;
using System.Xml;

namespace Test
{
    internal class Program
    {
        static void Main()
        {
            // Write
            {
                IniSaved<System.Xml.IniSavedSample.SampleConfig>.presaveFileHeader = true;
                IniSaved<System.Xml.IniSavedSample.SampleConfig>.presaveXmlSerialization = false;
                IniSaved<System.Xml.IniSavedSample.SampleConfig>.customHeaderLine = "Original at https://github.com/dkxce/INISaved";
                System.Xml.IniSavedSample.SampleConfig CONFIG = new System.Xml.IniSavedSample.SampleConfig();
                string iniData = IniSaved<System.Xml.IniSavedSample.SampleConfig>.Save(CONFIG);
                IniSaved<System.Xml.IniSavedSample.SampleConfig>.Save("sample.ini", CONFIG);
                Console.WriteLine(iniData);
            };

            // Read CONFIG
            {
                System.Xml.IniSavedSampleBlank.SampleConfig CONFIG = new System.Xml.IniSavedSampleBlank.SampleConfig();
                CONFIG = IniSaved<System.Xml.IniSavedSampleBlank.SampleConfig>.Load("sample.ini");
            };
        }
    }
}