using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ImportShopOffers
{
    [Serializable]
    public class AllOffers
    {
        public List<Offer> Offers { get; set; }

        public AllOffers()
        {
            Offers = new List<Offer>();
        }

        public void Load(XElement offers)
        {
            Offers.Clear();
            // читаем предложения товаров
            foreach (var offerElement in offers.Elements())
            {
                var offer = new Offer()
                    {
                        Id = int.Parse(offerElement.Attribute("id").Value),
                        Available = bool.Parse(offerElement.Attribute("available").Value),
                    };
                offer.SetContent(offerElement);
                Offers.Add(offer);
            }

        }

    }

    [Serializable]
    public class Offer
    {
        private XElement _content;

        public int Id { get; set; }
        public bool Available { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }


        public List<string> ProperyNames { get; set; }

        public Offer()
        {
            ProperyNames = new List<string>();
        }

        public XElement GetContent()
        {
            return _content;
        }

        public void SetContent(XElement value)
        {
            _content = value;
            ProperyNames.Clear();
            foreach (var item in value.Elements())
            {
                var name = item.Name.ToString();
                ProperyNames.Add(name);
                switch (name)
                {
                    case "categoryId":
                        CategoryId = int.Parse(item.Value);
                        break;
                    case "name":
                        Name = Helper.CleanFromHtml(item.Value);
                        break;
                    case "description":
                        Description = item.Value;
                        break;
                }
            }
        }

        public string GetDescriptorText(PattertInfo pattern)
        {
            var content = this.GetContent();
            var value = content.Element("description").Value;
            var prop = pattern.Columns.First(item => item.Name == "description");
            var desc = TrimByLength(pattern.CleanHtmlFromDescription ? Helper.CleanFromHtml(value) : value, prop.Size) + Environment.NewLine;
            if (pattern.AddArticleBeforeDescription)
                desc = "Артикул: " + this.Id + Environment.NewLine + Environment.NewLine + desc;
            if (pattern.AddParamAfterDescription && content.Elements("param").Count() > 0)
            {
                desc += Environment.NewLine + "Характеристики:" + Environment.NewLine;
                foreach (var param in content.Elements("param"))
                {
                    desc += Environment.NewLine + Helper.CleanMoreSpaces(param.Attribute("name").Value) + ": " + 
                        Helper.CleanMoreSpaces(param.Value) + Environment.NewLine;
                }
            }
            if (pattern.AddExtraFieldsAfterDescription)
            {
                var names = new List<string>();
                foreach (var item in content.Elements()
                                        .Select(x => x.Name.ToString())
                                        .Distinct()
                                        .OrderBy(name => name))
                    names.Add(item);
                foreach (var column in pattern.Columns)
                    names.Remove(column.Name);
                names.Remove("param");
                if (names.Count > 0)
                {
                    desc += Environment.NewLine + "Дополнительные поля:" + Environment.NewLine;
                    foreach (var name in names)
                    {
                        var fieldvalue = content.Element(name).Value;
                        if (string.IsNullOrWhiteSpace(fieldvalue.Trim())) continue;
                        desc +=  Environment.NewLine + name + ": " + Helper.CleanMoreSpaces(fieldvalue) +Environment.NewLine;
                    }
                }

            }
            return desc;
        }

        private string TrimByLength(string value, int size)
        {
            if (size > 0)
            {
                var values = value.Split(new[] { ' ' });
                var words = new List<string>(values);
                var sb = new StringBuilder();
                var result = new List<string>();
                while (sb.Length < size && words.Count > 0)
                {
                    result.Add(words[0]);
                    sb.AppendFormat(" {0}", words[0]);
                    words.RemoveAt(0);
                }
                return string.Join(" ", result);
            }
            return value;
        }

    }
}
