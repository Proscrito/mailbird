using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Limilabs.Client.IMAP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using Limilabs.Mail.MIME;

namespace MailTest
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var imap = new Imap())
            {
                imap.ConnectSSL("imap.gmail.com");    // use ConnectSSL for SSL
                imap.UseBestLogin("mbtest.task@gmail.com", "p@33w0rd");
                imap.SelectInbox();

                var uids = imap.Search(Flag.All);

                foreach (var uid in uids)
                {
                    var eml = imap.GetMessageByUID(uid);
                    var email = new MailBuilder().CreateFromEml(eml);

                    // Subject
                    Console.WriteLine(email.Subject);

                    // From
                    foreach (var m in email.From)
                    {
                        Console.WriteLine(m.Address);
                        Console.WriteLine(m.Name);
                    }

                    // Date
                    Console.WriteLine(email.Date);

                    // Text body of the message
                    Console.WriteLine(email.Text);

                    // Html body of the message
                    Console.WriteLine(email.Html);

                    // Custom header
                    Console.WriteLine(email.Document.Root.Headers["x-spam"]);

                    // Save all attachments to disk
                    //foreach (var mime in email.Attachments)
                    //{
                    //    mime.Save(@"c:\" + mime.SafeFileName);
                    //}
                }

                imap.Close();
            }

        }
    }
}
