using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeveloperTest.Extensions;
using DeveloperTest.Models;
using Limilabs.Client.IMAP;
using Limilabs.Client.POP3;
using Limilabs.Mail;

namespace DeveloperTest.Services
{
    public class MailService : IMailService
    {
        private int BaseTimeoutSec = 30;
        public async Task<IList<MailModel>> GetMessagesImapAsync(ServerInfoModel serverInfoModel)
        {
            var task = Task.Run(() => GetMessagesImap(serverInfoModel));
            return await TimeoutAfter(task);
        }

        public async Task<IList<MailModel>> GetMessagesPop3Async(ServerInfoModel serverInfoModel)
        {
            var task = Task.Run(() => GetMessagesPop3(serverInfoModel));
            return await TimeoutAfter(task);
        }

        private IList<MailModel> GetMessagesPop3(ServerInfoModel serverInfoModel)
        {
            using (var pop3 = new Pop3())
            {
                pop3.ConnectSSL(serverInfoModel.Server, serverInfoModel.Port);
                pop3.Login(serverInfoModel.User, serverInfoModel.Password);

                return GetMessagesPop3(pop3).ToList();
            }
        }

        private IEnumerable<MailModel> GetMessagesPop3(Pop3 pop3)
        {
            var builder = new MailBuilder();
            var uids = pop3.GetAll();

            foreach (var email in uids.Select(pop3.GetMessageByUID).Select(x => builder.CreateFromEml(x)))
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

        private IList<MailModel> GetMessagesImap(ServerInfoModel serverInfoModel)
        {
            using (var imap = new Imap())
            {
                //imap connection with wrong port will stuck forever, this is handled by setting timeout
                //probably need more intelligent handling
                imap.ConnectSSL(serverInfoModel.Server, serverInfoModel.Port);
                imap.UseBestLogin(serverInfoModel.User, serverInfoModel.Password);
                imap.SelectInbox();

                return GetMessagesImap(imap).ToList();
            }
        }

        private IEnumerable<MailModel> GetMessagesImap(Imap imap)
        {
            var builder = new MailBuilder();
            var uids = imap.Search(Flag.All);

            foreach (var email in uids.Select(imap.GetMessageByUID).Select(x => builder.CreateFromEml(x)))
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

        //probably better to add extension method, but let's get lazy whenever it's possible... 
        private async Task<TResult> TimeoutAfter<TResult>(Task<TResult> task)
        {
            if (await Task.WhenAny(task, Task.Delay(TimeSpan.FromSeconds(BaseTimeoutSec))) == task)
            {
                //re-await for exception handling
                return await task;
            }

            //timeout
            throw new TimeoutException($"Operation exceeded timeout ({BaseTimeoutSec} sec)");
        }
    }
}