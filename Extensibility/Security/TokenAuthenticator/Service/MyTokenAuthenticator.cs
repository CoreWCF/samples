// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Collections.ObjectModel;
using System.Security.Principal;
using System.Text.RegularExpressions;
using CoreWCF.IdentityModel.Claims;
using CoreWCF.IdentityModel.Policy;
using CoreWCF.IdentityModel.Selectors;
using CoreWCF.IdentityModel.Tokens;

namespace CoreWcf.Samples.TokenAuthenticator
{
    internal class MyTokenAuthenticator : UserNameSecurityTokenAuthenticator
    {
        private static bool IsRogueDomain(string domain)
        {
            return false;
        }

        private static bool IsEmail(string inputEmail)
        {

            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(inputEmail))
                return (true);
            else
                return (false);
        }

        private static bool ValidateUserNameFormat(string UserName)
        {
            if (!IsEmail(UserName))
            {
                Console.WriteLine("Not a valid email");
                return false;
            }
            string[] emailAddress = UserName.Split('@');
            string user = emailAddress[0];
            string domain = emailAddress[1];
            if (IsRogueDomain(domain))
                return false;
            return true;
        }
        protected override ValueTask<ReadOnlyCollection<IAuthorizationPolicy>> ValidateUserNamePasswordCoreAsync(string userName, string password)
        {
            if (!ValidateUserNameFormat(userName))
                throw new SecurityTokenValidationException("Incorrect UserName format");

            ClaimSet claimSet = new DefaultClaimSet(ClaimSet.System, new Claim(ClaimTypes.Name, userName, Rights.PossessProperty));
            List<IIdentity> identities = new List<IIdentity>(1);
            identities.Add(new GenericIdentity(userName));
            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>(1);
            policies.Add(new UnconditionalPolicy(ClaimSet.System, claimSet, DateTime.MaxValue.ToUniversalTime(), identities));
            return ValueTask.FromResult(policies.AsReadOnly());
        }
    }

    internal class UnconditionalPolicy : IAuthorizationPolicy
    {
        private readonly ClaimSet _issuance;
        private readonly IList<IIdentity> _identities;

        public UnconditionalPolicy(ClaimSet issuer, ClaimSet issuance, DateTime expirationTime, IList<IIdentity> identities)
        {
            if (issuer == null)
                throw new ArgumentNullException(nameof(issuer));

            if (issuance == null)
                throw new ArgumentNullException(nameof(issuance));

            Issuer = issuer;
            _issuance = issuance;
            _identities = identities;
            ExpirationTime = expirationTime;
        }

        public string Id { get; } = Guid.NewGuid().ToString();

        public ClaimSet Issuer { get; }

        public DateTime ExpirationTime { get; }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            evaluationContext.AddClaimSet(this, _issuance);

            if (_identities != null)
            {
                object value;
                IList<IIdentity> contextIdentities;
                if (!evaluationContext.Properties.TryGetValue("Identities", out value))
                {
                    contextIdentities = new List<IIdentity>(_identities.Count);
                    evaluationContext.Properties.Add("Identities", contextIdentities);
                }
                else
                {
                    contextIdentities = value as IList<IIdentity>;
                }
                foreach (IIdentity identity in _identities)
                {
                    contextIdentities.Add(identity);
                }
            }

            evaluationContext.RecordExpirationTime(ExpirationTime);
            return true;
        }
    }
}
