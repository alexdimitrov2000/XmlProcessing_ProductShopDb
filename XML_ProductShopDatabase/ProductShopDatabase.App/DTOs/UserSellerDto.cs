using System.Xml.Serialization;

namespace ProductShop.App.DTOs
{
    [XmlType("user")]
    public class UserSellerDto
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlArray("sold-products")]
        [XmlArrayItem("product")]
        public ProductDto[] SoldProducts { get; set; }
    }
}
