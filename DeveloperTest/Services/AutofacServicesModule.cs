using Autofac;

namespace DeveloperTest.Services
{
    public class AutofacServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new MailService()).As<IMailService>();
        }
    }
}
