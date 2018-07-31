using System.Xml.Serialization;

namespace ProductShop.App.DTOs
{
    [XmlType("product")]
    public class ProductsInRangeDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }

        [XmlAttribute("buyer")]
        public string BuyerName { get; set; }
    }
}
