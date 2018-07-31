using System.Xml.Serialization;

namespace ProductShop.App.DTOs
{
    [XmlRoot("users")]
    public class UsersDto
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("users")]
        public UserProductDto[] Users { get; set; }
    }
}
