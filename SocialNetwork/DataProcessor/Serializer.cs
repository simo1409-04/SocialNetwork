using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SocialNetwork.Data;
using SocialNetwork.Data.Models;
using SocialNetwork.DataProcessor.ExportDTOs;
using SocialNetwork.Serialization;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace SocialNetwork.DataProcessor
{
    public class Serializer
    {



        public static string ExportUsersWithFriendShipsCountAndTheirPosts(SocialNetworkDbContext dbContext)
        {
            var rawUsers = dbContext.Users
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    FriendshipsCount = dbContext.Friendships
                        .Count(f => f.UserOneId == u.Id || f.UserTwoId == u.Id),
                    Posts = u.Posts
                        .Select(p => new
                        {
                            p.Id,
                            p.Content,
                            p.CreatedAt
                        })
                        .ToList()
                })
                .AsNoTracking()
                .ToList();

            List<UserXmlDToExport> usersForExport = rawUsers
                .OrderBy(u => u.Username)
                .Select(u => new UserXmlDToExport
                {
                    Username = u.Username,
                    Friendships = u.FriendshipsCount,
                    Posts = u.Posts
                        .OrderBy(p => p.Id)
                        .Select(p => new PostDtoExportXml
                        {
                            Content = p.Content,
                            CreatedAt = p.CreatedAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture)
                        })
                        .ToList()
                })
                .ToList();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<UserXmlDToExport>), new XmlRootAttribute("Users"));

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            StringBuilder sb = new StringBuilder();
            using (StringWriter writer = new StringWriter(sb))
            {
                xmlSerializer.Serialize(writer, usersForExport, namespaces);
            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportConversationsWithMessagesChronologically(SocialNetworkDbContext dbContext)
        {

            List<Conversation> conversations = dbContext.Conversations.Include(x => x.Messages).ThenInclude(x=>x.Sender).OrderBy(x=>x.StartedAt).AsNoTracking().ToList();


            var conversationsForExport = conversations.Select(x => new
            {
                Id = x.Id,
                Title = x.Title,
                StartedAt = x.StartedAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                Messages =
                x.Messages.OrderBy(m => m.SentAt).Select(m => new
                {
                    Content = m.Content,
                    SentAt = m.SentAt.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                    Status = (int)m.Status,
                    SenderUsername = m.Sender.Username

                }).ToList()


            }).ToList();



            JsonSerializerSettings settings = new JsonSerializerSettings
            {

                Formatting = Formatting.Indented

            };


            return JsonConvert.SerializeObject(conversationsForExport, settings);

        }
    }
}
