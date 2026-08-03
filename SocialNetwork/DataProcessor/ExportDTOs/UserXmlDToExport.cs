using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SocialNetwork.DataProcessor.ExportDTOs
{
    public class UserXmlDToExport
    {

        [XmlAttribute("Friendships")]
        public int Friendships { get; set; }

        [XmlElement("Username")]
        public string Username { get; set; } = null!;

        [XmlArray("Posts")]
        [XmlArrayItem("Post")]
        public List<PostDtoExportXml> Posts { get; set; } = new();

    }
}
