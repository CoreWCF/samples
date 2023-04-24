// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.IdentityModel.Selectors;

namespace CoreWcf.Samples.TokenAuthenticator
{
    public class MySecurityTokenManager : ServiceCredentialsSecurityTokenManager
    {
        private MyUserNameCredential myUserNameCredential;
        private readonly string _userNameTokenType = "http://schemas.microsoft.com/ws/2006/05/identitymodel/tokens/UserName";

        public MySecurityTokenManager(MyUserNameCredential myUserNameCredential)
            : base(myUserNameCredential)
        {
            this.myUserNameCredential = myUserNameCredential;
        }

        public override SecurityTokenAuthenticator CreateSecurityTokenAuthenticator(SecurityTokenRequirement tokenRequirement, out SecurityTokenResolver outOfBandTokenResolver)
        {
            if (tokenRequirement.TokenType == _userNameTokenType)
            {
                outOfBandTokenResolver = null;
                return new MyTokenAuthenticator();
            }
            else
            {
                return base.CreateSecurityTokenAuthenticator(tokenRequirement, out outOfBandTokenResolver);
            }
        }
    }
}

