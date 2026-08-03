using SocialNetwork.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using static SocialNetwork.ValidationConstraints.Constraints;

namespace SocialNetwork.DataProcessor.ImportDTOs
{
    public class PostDtoJsonImport
    {

      

        [Required]
        [MaxLength(PostContentMaxLength)]
        [MinLength(PostContentMinLength)]
        public string Content { get; set; } = null!;
        [Required]
        public string CreatedAt { get; set; }

        [Required]
        public int CreatorId { get; set; }


    }
}
