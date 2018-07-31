using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ProductShop.App.DTOs
{
    [XmlType("product")]
    public class ProductDto
    {
        //[XmlElement("name")]
        //[MinLength(3)]
        [XmlAttribute("name")] // for query 4
        public string Name { get; set; }

        //[XmlElement("price")]
        [XmlAttribute("price")] // for query 4
        public decimal Price { get; set; }
    }
}
