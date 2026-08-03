using SocialNetwork.Data.Enums;
using SocialNetwork.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static SocialNetwork.ValidationConstraints.Constraints;


namespace SocialNetwork.DataProcessor.ImportDTOs
{
    [XmlType("Message")]
    public class MessageXmlDtoImport
    {


        [Required]
        [XmlAttribute("SentAt")]

        public string SentAt { get; set; } = null!;

        [Required]
        [MaxLength(MessageContentMaxLength)]
        [MinLength(MessageContentMinLength)]
        public string Content { get; set; } = null!;


        [Required]
        public string Status { get; set; } = null!;

        [Required]
        public int ConversationId { get; set; }


        [Required]
        public int SenderId { get; set; }



    }
}
