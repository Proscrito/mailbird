﻿using System.Collections.Generic;
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
        public async Task<IList<MailModel>> GetMessagesImapAsync(ServerInfoModel serverInfoModel)
        {
           return await Task.Run(() => GetMessagesImap(serverInfoModel));
        }

        public async Task<IList<MailModel>> GetMessagesPop3Async(ServerInfoModel serverInfoModel)
        {
            return await Task.Run(() => GetMessagesPop3(serverInfoModel));
        }

        private IList<MailModel> GetMessagesPop3(ServerInfoModel serverInfoModel)
        {
            using (var pop3 = new Pop3())
            {
                pop3.ConnectSSL(serverInfoModel.Server);
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
                imap.ConnectSSL(serverInfoModel.Server);
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
    }
}