using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ImportShopOffers
{
    [Serializable]
    public class OfferCategories
    {
        public List<OfferCategory> Categories = new List<OfferCategory>();

        public void Load(XElement categories)
        {
            Categories.Clear();
            var count = categories.Elements().Count();
            // читаем корневые категории
            foreach (var categoryElement in categories.Elements().Where(item => 
                     item.Attribute("parentId") == null || string.IsNullOrWhiteSpace(item.Attribute("parentId").Value)))
            {
                var offcat = new OfferCategory()
                                {
                                    Id = categoryElement.Attribute("id").Value,
                                    Name = categoryElement.Value
                                };
                Categories.Add(offcat);
                count--;
            }
            while (count > 0)
            {
                // читаем дочерние категории
                foreach (var categoryElement in categories.Elements().Where(item => 
                         item.Attribute("parentId") != null && !string.IsNullOrWhiteSpace(item.Attribute("parentId").Value)))
                {
                    var parentId = categoryElement.Attribute("parentId").Value;
                    var parent = Categories.FirstOrDefault(item => item.Id == parentId);
                    if (parent != null)
                    {
                        var offcat = new OfferCategory()
                        {
                            Id = categoryElement.Attribute("id").Value,
                            ParentId = parentId,
                            Name = categoryElement.Value
                        };
                        parent.Childs.Add(offcat);
                        count--;
                    }
                }
            }
        }

        public void LinkOffers(AllOffers offers)
        {
            foreach (var category in Categories)
            {
                category.Offers.Clear();
                foreach (var offer in offers.Offers.Where(offer => offer.CategoryId == category.Id))
                    category.Offers.Add(offer);
                LinkChilds(category, offers);
            }
        }

        private void LinkChilds(OfferCategory category, AllOffers offers)
        {
            foreach (var child in category.Childs)
            {
                child.Offers.Clear();
                foreach (var offer in offers.Offers.Where(offer => offer.CategoryId == child.Id))
                    child.Offers.Add(offer);
                LinkChilds(child, offers);
            }
        }
    }

    [Serializable]
    public class OfferCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string ParentId { get; set; }

        public List<OfferCategory> Childs { get; set; }

        public List<Offer> Offers { get; set; }

        public OfferCategory()
        {
            ParentId = string.Empty;
            Childs = new List<OfferCategory>();
            Offers = new List<Offer>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
