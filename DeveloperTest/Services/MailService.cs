using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperTest.Extensions;
using DeveloperTest.Models;
using Limilabs.Client.IMAP;
using Limilabs.Mail;

namespace DeveloperTest.Services
{
    public class MailService : IMailService
    {
        public async Task<IList<MailModel>> GetMessagesImapAsync(ServerInfoModel serverInfoModel)
        {
           return await Task.Run(() => GetMessagesImap(serverInfoModel));
        }

        private IList<MailModel> GetMessagesImap(ServerInfoModel serverInfoModel)
        {
            using (var imap = new Imap())
            {
                imap.ConnectSSL(serverInfoModel.Server);
                imap.UseBestLogin(serverInfoModel.User, serverInfoModel.Password);
                imap.SelectInbox();

                return GetMessagesImap(imap).ToList();
            }
        }

        private IEnumerable<MailModel> GetMessagesImap(Imap imap)
        {
            var uids = imap.Search(Flag.All);

            foreach (var email in uids.Select(imap.GetMessageByUID).Select(eml => new MailBuilder().CreateFromEml(eml)))
            {
                yield return new MailModel
                {
                    From = email.From.FormatFromField(),
                    To = string.Join(";", email.To),
                    Date = email.Date?.ToString("G"),
                    Subject = email.Subject,
                    Body = email.Text
                };
            }
        }
    }
}