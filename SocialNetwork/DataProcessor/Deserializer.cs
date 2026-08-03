using Newtonsoft.Json;
using SocialNetwork.Data;
using SocialNetwork.Data.Enums;
using SocialNetwork.Data.Models;
using SocialNetwork.DataProcessor.ImportDTOs;
using SocialNetwork.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text;
using System.Xml.Serialization;
namespace SocialNetwork.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format.";
        private const string DuplicatedDataMessage = "Duplicated data.";
        private const string SuccessfullyImportedMessageEntity = "Successfully imported message (Sent at: {0}, Status: {1})";
        private const string SuccessfullyImportedPostEntity = "Successfully imported post (Creator {0}, Created at: {1})";

        public static string ImportMessages(SocialNetworkDbContext dbContext, string xmlString)
        {

            XmlRootAttribute root = new XmlRootAttribute("Messages");
            StringBuilder builder = new StringBuilder();

            List<MessageXmlDtoImport> messegaesForValidate = XmlSerialization.DeserializeToObject<List<MessageXmlDtoImport>>(xmlString, root);
            var currentMessegesInContext = dbContext.Messages.ToList();
            var currentSendersIdsInContext = dbContext.Users.Select(x => x.Id).ToList();
            var currentConversationsIdsIdsInContext = dbContext.Conversations.Select(x => x.Id).ToList();
            List<Message> messegaesForContext = new();

            foreach(var message in messegaesForValidate)
            {
                bool isValidDate = DateTime.TryParseExact(message.SentAt, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture,DateTimeStyles.None, out DateTime date);
                bool isEnumCorrect = Enum.GetNames(typeof(MessageStatus)).Contains(message.Status);

                if (IsValid(message) && isValidDate && isEnumCorrect)
                {
                    bool isDuplicate = currentMessegesInContext.Any(x =>
           x.Content == message.Content &&
           x.SentAt == date &&
           x.Status == Enum.Parse<MessageStatus>(message.Status) &&
           x.SenderId == message.SenderId &&
           x.ConversationId == message.ConversationId);
                    if(isDuplicate)
                    {

                        builder.AppendLine(DuplicatedDataMessage);
                        continue;

                    }

                    if (currentConversationsIdsIdsInContext.Any(x=>x==message.ConversationId)==false|| currentSendersIdsInContext.Any(x => x == message.SenderId) == false)
                    {
                        builder.AppendLine(ErrorMessage);
                        continue;
                    }

                    Message messageForDb = new Message { SentAt = date, Content = message.Content, Status = Enum.Parse<MessageStatus>(message.Status), ConversationId = message.ConversationId, SenderId = message.SenderId };

                    messegaesForContext.Add(messageForDb);
                    currentMessegesInContext.Add(messageForDb);
                    builder.AppendLine(String.Format(SuccessfullyImportedMessageEntity, date.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture), message.Status));

                }

                else
                {

                    builder.AppendLine(ErrorMessage);


                }

               

            }
            dbContext.AddRange(messegaesForContext);
            dbContext.SaveChanges();
            return builder.ToString().TrimEnd();
        }

        public static string ImportPosts(SocialNetworkDbContext dbContext, string jsonString)
        {
            StringBuilder builder = new StringBuilder();

            List<PostDtoJsonImport> postsToValidate = JsonConvert.DeserializeObject<List<PostDtoJsonImport>>(jsonString);

            var currentCreatorIdsinDb = dbContext.Users.Select(x => x.Id).ToList();
            var creators = dbContext.Users.ToList();

            var currentPostsInDb = dbContext.Posts.ToList();

            var postsForContext = new List<Post>();

            foreach(var post in postsToValidate)
            {
                bool isValidDate = DateTime.TryParseExact(post.CreatedAt, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
                bool isIdForCreatorExistsInDb = currentCreatorIdsinDb.Contains(post.CreatorId);


                if (IsValid(post) && isValidDate && isIdForCreatorExistsInDb)
                {

                    bool isDublicate = currentPostsInDb.Any(x => x.Content == post.Content && x.CreatedAt == date && x.CreatorId == post.CreatorId);


                    if (isDublicate)
                    {
                        builder.AppendLine(DuplicatedDataMessage);
                        continue;
                    }

                    else
                    {
                        string creatorUserName = creators.Where(x => x.Id == post.CreatorId).Select(x => x.Username).First();

                        postsForContext.Add(new Post { Content = post.Content, CreatedAt = date, CreatorId = post.CreatorId });
                        currentPostsInDb.Add(new Post { Content = post.Content, CreatedAt = date, CreatorId = post.CreatorId });
                        builder.AppendLine(String.Format(SuccessfullyImportedPostEntity, creatorUserName, date.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)));
                    }


                }

                else
                {
                    builder.AppendLine(ErrorMessage);

                }


            }

            dbContext.Posts.AddRange(postsForContext);
            dbContext.SaveChanges();
            return builder.ToString().TrimEnd();

        }

        public static bool IsValid(object dto)
        {
            ValidationContext validationContext = new ValidationContext(dto);
            List<ValidationResult> validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validationContext, validationResults, true);

            foreach (ValidationResult validationResult in validationResults)
            {
                if (validationResult.ErrorMessage != null)
                {
                    string currentMessage = validationResult.ErrorMessage;
                }
            }

            return isValid;
        }
    }
}
