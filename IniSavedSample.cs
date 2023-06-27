//
// C# (cross-platform)
// IniSaved
// v 0.2, 27.06.2023
// https://github.com/dkxce/INISaved
// en,ru,1251,utf-8
//

using System.Collections.Generic;
using System.Xml.Serialization;

namespace System.Xml.IniSavedSample
{
    [IniSection("DEFAULT")] // OPTIONAL
    public class SampleConfig: IniSaved<SampleConfig>
    {
        #region Simple
        public string Name = "SampleConfiguration";
        public int Age = 18;
        [XmlElement("BirthDay")]
        public DateTime Birth = DateTime.Today;
        public float Weight = 101f;
        public long Length = 777;
        public bool Man = true;
        #endregion Simple

        #region Arrays
        public byte[] aBytes = new byte[] { 0x01, 0x10, 0xFF, 0x0A, 0x0D, 0xCD, 0x3B, 0x23 };
        public double[] aDoubles = new double[] { 11.0, 22.0, 33.0, 44.0, 55.0, 66.0, 77.0 };
        public int[] aInts = new int[] { 101, 202, 303, 404, 505, 606, 707 };
        public SubSampleConfig[] aSubSampleConfigs = new SubSampleConfig[] {
            new SubSampleConfig() { comment = "com", key = "kname", description = "vval" },
            new SubSampleConfig() { comment = "fav", key = "movek", description = "loop" },
            new SubSampleConfig() { comment = "moc", key = "emank", description = "lavv" }
        };
        #endregion Arrays

        #region Lists
        public List<string> lStrings = new List<string>() { "s1", "s2", "s3", "s4" };
        public List<int> lInts = new List<int>() { byte.MaxValue, short.MaxValue, ushort.MaxValue };
        public List<SubSampleRecord> lSubSampleRecords = new List<SubSampleRecord>() { new SubSampleRecord() { person = "Unknown", mail = "noMail", web = "http://" } };
        #endregion Lists

        #region Dictionaries

        [XmlIgnore]
        public Dictionary<string, int> IntDict = new Dictionary<string, int>() { { "IntDict_A", -101 }, { "IntDict_B", -2022 } };

        [XmlArray("IntDict")]
        public DictionaryEntry[] IntDictAsList
        {
            get { return DictionaryEntry.Get(IntDict); }
            set { DictionaryEntry.Set(IntDict = new Dictionary<string, int>(), value); }
        }

        [XmlIgnore]
        public Dictionary<string, string> StringDict = new Dictionary<string, string>() { { "StringDict_C", "s1088" }, { "StringDict_D", "s2088" } };

        [XmlArray("StringDict")]
        public DictionaryEntry[] StringDictAsList
        {
            get { return DictionaryEntry.Get(StringDict); }
            set { DictionaryEntry.Set(StringDict = new Dictionary<string, string>(), value); }
        }

        #endregion Dictionaries      
    }

    public class SubSampleRecord
    {
        [XmlAttribute]
        public string person;

        [XmlAttribute]
        public string mail;

        [XmlAttribute]
        public string web;
    }

    public class SubSampleConfig
    {
        [XmlAttribute]
        public string key;

        [XmlAttribute]
        public string comment;

        [XmlText]
        public string description;

        [XmlIgnore]
        public Dictionary<string, float> FloatDict = new Dictionary<string, float>() { { "FloatDictSictA", 11f }, { "FloatDictSictB", 22f }, { "FloatDictSictC", 33f } };

        [XmlArray("FloatDict")]
        public DictionaryEntry[] FloatDictAsList
        {
            get { return DictionaryEntry.Get(FloatDict); }
            set { DictionaryEntry.Set(FloatDict = new Dictionary<string, float>(), value); }
        }
    }
}

namespace System.Xml.IniSavedSampleBlank
{
    [IniSection("DEFAULT")] // OPTIONAL
    public class SampleConfig : IniSaved<SampleConfig>
    {
        #region Simple
        public string Name;
        public int Age;
        public DateTime Birth;
        public float Weight;
        public long Length;
        public bool Man;
        #endregion Simple

        #region Arrays
        public byte[] aBytes;
        public double[] aDoubles;
        public int[] aInts;
        public SubSampleConfig[] aSubSampleConfigs;
        #endregion Arrays

        #region Lists
        public List<string> lStrings = new List<string>();
        public List<int> lInts = new List<int>();
        public List<SubSampleRecord> lSubSampleRecords = new List<SubSampleRecord>();
        #endregion Lists

        #region Dictionaries

        [XmlIgnore]
        public Dictionary<string, int> IntDict = new Dictionary<string, int>();

        [XmlArray("IntDict")]
        public DictionaryEntry[] IntDictAsList
        {
            get { return DictionaryEntry.Get(IntDict); }
            set { DictionaryEntry.Set(IntDict = new Dictionary<string, int>(), value); }
        }

        [XmlIgnore]
        public Dictionary<string, string> StringDict = new Dictionary<string, string>();

        [XmlArray("StringDict")]
        public DictionaryEntry[] StringDictAsList
        {
            get { return DictionaryEntry.Get(StringDict); }
            set { DictionaryEntry.Set(StringDict = new Dictionary<string, string>(), value); }
        }

        #endregion Dictionaries      
    }

    public class SubSampleRecord
    {
        [XmlAttribute]
        public string person;

        [XmlAttribute]
        public string mail;

        [XmlAttribute]
        public string web;
    }

    public class SubSampleConfig
    {
        [XmlAttribute]
        public string key;

        [XmlAttribute]
        public string comment;

        [XmlText]
        public string description;

        [XmlIgnore]
        public Dictionary<string, float> FloatDict = new Dictionary<string, float>();

        [XmlArray("FloatDict")]
        public DictionaryEntry[] FloatDictAsList
        {
            get { return DictionaryEntry.Get(FloatDict); }
            set { DictionaryEntry.Set(FloatDict = new Dictionary<string, float>(), value); }
        }
    }
}