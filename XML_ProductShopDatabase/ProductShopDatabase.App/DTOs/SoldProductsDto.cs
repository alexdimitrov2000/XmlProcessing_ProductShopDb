using System.Xml.Serialization;

namespace ProductShop.App.DTOs
{
    [XmlType("sold-products")]
    public class SoldProductsDto
    {
        [XmlAttribute("count")]
        public int Count { get; set; }
        
        [XmlElement("product")]
        public ProductDto[] Products { get; set; }
    }
}
