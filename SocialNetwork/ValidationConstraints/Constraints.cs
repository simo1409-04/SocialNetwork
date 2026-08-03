using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.ValidationConstraints
{
    public class Constraints
    {

        public const int UserUSernameMaxLength = 20;
        public const int UserUSernameMinLength = 4;
        public const int UserEmailMaxLength = 60;
        public const int UserEmailMinLength = 8;
        public const int UserPasswordMinLength = 6;



        public const int ConversationTitleMaxLength = 30;
        public const int ConversationTitleMinLength = 2;



        public const int PostContentMaxLength = 300;
        public const int PostContentMinLength = 5;


        public const int MessageContentMinLength = 1;
        public const int MessageContentMaxLength = 200;





    }
}
