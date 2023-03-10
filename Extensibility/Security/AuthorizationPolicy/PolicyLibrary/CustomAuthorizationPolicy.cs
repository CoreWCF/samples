﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using CoreWCF.IdentityModel.Claims;
using CoreWCF.IdentityModel.Policy;

namespace CoreWcf.Samples.AuthorizationPolicy
{
    public class CustomAuthorizationPolicy : IAuthorizationPolicy
    {
        string id;

        public CustomAuthorizationPolicy()
        {
            id = Guid.NewGuid().ToString();
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            bool bRet = false;
            CustomAuthState customstate = null;

            // If state is null, then we've not been called before so we need
            // to set up our custom state
            if (state == null)
            {
                customstate = new CustomAuthState();
                state = customstate;
            }
            else
                customstate = (CustomAuthState)state;

            Console.WriteLine("Inside MyAuthorizationPolicy::Evaluate");

            // If we've not added claims yet...
            if (!customstate.ClaimsAdded)
            {
                // Create an empty list of Claims
                IList<Claim> claims = new List<Claim>();

                // Iterate through each of the claimsets in the evaluation context
                foreach (ClaimSet cs in evaluationContext.ClaimSets)
                    // Look for Name claims in the current claimset...
                    foreach (Claim c in cs.FindClaims(ClaimTypes.Name, Rights.PossessProperty))
                        // Get the list of operations the given username is allowed to call...
                        foreach (string s in GetAllowedOpList(c.Resource.ToString()))
                        {
                            // Check numbers aren't too large


                            // Add claims to the list
                            claims.Add(new Claim("http://example.org/claims/allowedoperation", s, Rights.PossessProperty));
                            Console.WriteLine("Claim added {0}", s);
                        }

                // Add claims to the evaluation context    
                evaluationContext.AddClaimSet(this, new DefaultClaimSet(this.Issuer, claims));

                // record that we've added claims
                customstate.ClaimsAdded = true;

                // return true, indicating we do not need to be called again.
                bRet = true;
            }
            else
            {
                // Should never get here, but just in case...
                bRet = true;
            }


            return bRet;
        }

        public ClaimSet Issuer
        {
            get { return ClaimSet.System; }
        }

        public string Id
        {
            get { return id; }
        }

        // This method returns a collection of action strings that indicate the 
        // operations the specified username is allowed to call.
        private static IEnumerable<string> GetAllowedOpList(string username)
        {
            IList<string> ret = new List<string>();

            if (username == "test1")
            {
                ret.Add("http://CoreWcf.Samples.AuthorizationPolicy/ICalculatorService/Add");
                ret.Add("http://CoreWcf.Samples.AuthorizationPolicy/ICalculatorService/Multiply");
                ret.Add("http://CoreWcf.Samples.AuthorizationPolicy/ICalculatorService/Subtract");
            }
            else if (username == "test2")
            {
                ret.Add("http://CoreWcf.Samples.AuthorizationPolicy/ICalculatorService/Add");
                ret.Add("http://CoreWcf.Samples.AuthorizationPolicy/ICalculatorService/Subtract");
            }
            return ret;
        }

        // internal class for state
        class CustomAuthState
        {
            bool bClaimsAdded;

            public CustomAuthState()
            {
            }

            public bool ClaimsAdded
            {
                get { return bClaimsAdded; }
                set { bClaimsAdded = value; }
            }
        }
    }

}

