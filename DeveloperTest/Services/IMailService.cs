using System.Collections.Generic;
using System.Threading.Tasks;
using DeveloperTest.Models;

namespace DeveloperTest.Services
{
    public interface IMailService
    {
        Task<IList<MailModel>> GetMessagesImapAsync(ServerInfoModel serverInfoModel);
    }
}
