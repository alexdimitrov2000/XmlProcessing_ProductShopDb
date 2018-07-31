using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ProductShop.App.DTOs
{
    [XmlType("category")]
    public class CategoryNameDto
    {
        [XmlElement("name")]
        [MinLength(3)]
        [MaxLength(15)]
        public string Name { get; set; }
    }
}
