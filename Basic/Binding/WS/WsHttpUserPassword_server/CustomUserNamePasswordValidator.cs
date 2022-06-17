using CoreWCF;
using System.Threading.Tasks;

namespace NetCoreServer
{
    internal class CustomUserNamePasswordValidator : CoreWCF.IdentityModel.Selectors.UserNamePasswordValidator
    {
        public override ValueTask ValidateAsync(string userName, string password)
        {
            bool valid = userName.ToLowerInvariant().EndsWith("valid")
                && password.ToLowerInvariant().EndsWith("valid");
            if (!valid)
            {
                throw new FaultException("Unknown Username or Incorrect Password");
            }
            return new ValueTask();
        }

        public static void AddToHost(ServiceHostBase host)
        {
            var srvCredentials = new CoreWCF.Description.ServiceCredentials();
            srvCredentials.UserNameAuthentication.UserNamePasswordValidationMode = CoreWCF.Security.UserNamePasswordValidationMode.Custom;
            srvCredentials.UserNameAuthentication.CustomUserNamePasswordValidator = new CustomUserNamePasswordValidator();
            host.Description.Behaviors.Add(srvCredentials);
        }
    }

}
