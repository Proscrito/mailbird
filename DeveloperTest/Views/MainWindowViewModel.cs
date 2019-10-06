using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using DeveloperTest.Commands;
using DeveloperTest.Enums;
using DeveloperTest.Models;
using DeveloperTest.Services;

namespace DeveloperTest.Views
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IMailService _mailService;
        //test gmail account data, no need to protect
        public string ServerURL { get; set; } = "imap.gmail.com";
        public string User { get; set; } = "mbtest.task@gmail.com";
        public string Password { get; set; } = "p@33w0rd";
        public ServerType ServerType { get; set; }
        public EncryptionType EncryptionType { get; set; }
        public string MailBody { get; set; } = "This should show the message body HTML/Text";

        public MailModel SelectedMailModel { get; set; }
        public ObservableCollection<MailModel> MailModels { get; set; }

        public bool Loading { get; set; }
        public IAsyncCommand StartCommand { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel(IMailService mailService)
        {
            _mailService = mailService;
            StartCommand = new AsyncCommand(StartClick, () => !Loading);
        }

        public async Task StartClick()
        {
            try
            {
                var serverInfo = new ServerInfoModel
                {
                    Server = ServerURL,
                    User = User,
                    Password = Password
                };

                var emails = await _mailService.GetMessagesImapAsync(serverInfo);
                MailModels = new ObservableCollection<MailModel>(emails);
            }
            catch (Exception e)
            {
                //log error
                MailBody = e.GetBaseException().Message;
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
