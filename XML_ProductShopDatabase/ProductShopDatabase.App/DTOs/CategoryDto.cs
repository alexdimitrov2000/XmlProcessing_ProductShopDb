using System.Xml.Serialization;

namespace ProductShop.App.DTOs
{
    [XmlType("category")]
    public class CategoryDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("products-count")]
        public int NumberOfProducts { get; set; }

        [XmlElement("average-price")]
        public decimal AverageProductPrice { get; set; }

        [XmlElement("total-revenue")]
        public decimal TotalRevenue { get; set; }
    }
}
