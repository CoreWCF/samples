// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.IdentityModel.Claims;
using CoreWCF.IdentityModel.Selectors;
using CoreWCF.IdentityModel.Tokens;

namespace CoreWcf.Samples.AuthorizationPolicy
{
    public class CustomServiceAuthorizationManager : ServiceAuthorizationManager
    {
        protected override ValueTask<bool> CheckAccessCoreAsync(OperationContext operationContext)
        {
            // Extract the action URI from the OperationContext. We will use this to match against the claims
            // in the AuthorizationContext
            string action = operationContext.RequestContext.RequestMessage.Headers.Action;
            Console.WriteLine("action: {0}", action);

            // Iterate through the various claimsets in the authorizationcontext
            foreach (ClaimSet cs in operationContext.ServiceSecurityContext.AuthorizationContext.ClaimSets)
            {
                // Only look at claimsets issued by System.
                if (cs.Issuer == ClaimSet.System)
                {
                    // Iterate through claims of type "http://example.org/claims/allowedoperation"
                    foreach (Claim c in cs.FindClaims("http://example.org/claims/allowedoperation", Rights.PossessProperty))
                    {
                        // Dump the Claim resource to the console.
                        Console.WriteLine("resource: {0}", c.Resource.ToString());

                        // If the Claim resource matches the action URI then return true to allow access
                        if (action == c.Resource.ToString())
                            return ValueTask.FromResult(true);
                    }
                }
            }

            // If we get here, return false, denying access.
            return ValueTask.FromResult(false);
        }
    }

    public class CustomUserNameValidator : UserNamePasswordValidator
    {
        // This method validates users. It allows in two users, test1 and test2 
        // with passwords 1test and 2test respectively.
        // This code is for illustration purposes only and 
        // MUST NOT be used in a production environment because it is NOT secure.
        public override ValueTask ValidateAsync(string userName, string password)
        {
            if (null == userName || null == password)
            {
                throw new ArgumentNullException();
            }

            if (!(userName == "test1" && password == "1test") && !(userName == "test2" && password == "2test"))
            {
                throw new SecurityTokenException("Unknown Username or Password");
            }

            return default;
        }
    }
}
