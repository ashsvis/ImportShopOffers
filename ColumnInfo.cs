using System;
using System.Collections.Generic;

namespace ImportShopOffers
{
    [Serializable]
    public class PattertInfo
    {
        public List<ColumnInfo> Columns { get; set; }
        public bool CleanHtmlFromDescription { get; set; }
        public bool AddParamAfterDescription { get; set; }
        public bool AddExtraFieldsAfterDescription { get; set; }
        public bool AddArticleBeforeDescription { get; set; }
        public Dictionary<string, string> FieldNames { get; set; }

        public PattertInfo()
        {
            Columns = new List<ColumnInfo>();
            FieldNames = new Dictionary<string, string>();
        }
    }

    public enum ColumnType
    {
        Value,
        Collection,
        Text,
        Flag,
        Number,
    }

    [Serializable]
    public class ColumnInfo
    {
        public int Index { get; set; }
        public int Subindex { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
        public string Text { get; set; }
        public ColumnType ColumnType { get; set; }
        public int Size { get; set; }
        public int Offset { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}
