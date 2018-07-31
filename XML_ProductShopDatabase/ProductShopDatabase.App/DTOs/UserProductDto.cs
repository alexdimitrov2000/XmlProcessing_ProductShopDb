using System.Xml.Serialization;

namespace ProductShop.App.DTOs
{
    [XmlType("user")]
    public class UserProductDto
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlAttribute("age")]
        public string Age { get; set; }

        [XmlElement("sold-products")]
        public SoldProductsDto SoldProducts { get; set; }
    }
}
