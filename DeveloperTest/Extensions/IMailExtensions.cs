using System.Collections.Generic;
using System.Linq;
using Limilabs.Mail.Headers;

namespace DeveloperTest.Extensions
{
    public static class MailExtensions
    {
        public static string FormatFromField(this IList<MailBox> mailBoxes)
        {
            return mailBoxes.Aggregate("", (current, mailBox) => current + $"<{mailBox.Name}> {mailBox.Address};");
        }
    }
}
