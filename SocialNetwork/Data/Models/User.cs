using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SocialNetwork.ValidationConstraints.Constraints;

namespace SocialNetwork.Data.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [MaxLength(UserUSernameMaxLength)]
        public string Username { get; set; } = null!;

        [MaxLength(UserEmailMaxLength)]
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;

        public virtual ICollection<Post> Posts { get; set; } = new HashSet<Post>();
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
        public virtual ICollection<UserConversation> UsersConversations  { get; set; } = new HashSet<UserConversation>();




    }
}
