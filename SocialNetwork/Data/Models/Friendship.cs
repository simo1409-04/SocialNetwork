using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Data.Models
{
    public class Friendship
    {
        [Required]
        [ForeignKey(nameof(UserOne))]
        public int UserOneId { get; set; }

        public virtual User UserOne { get; set; } = null!;

        [ForeignKey(nameof(UserTwo))]
        [Required]

        public int UserTwoId  { get; set; }


        public virtual User UserTwo  { get; set; } = null!;

    }
}
