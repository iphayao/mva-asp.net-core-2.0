using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApplication
{
    internal class CanadianRequirement : AuthorizationHandler<CanadianRequirement>, IAuthorizationRequirement
    {
        public CanadianRequirement()
        {

        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanadianRequirement requirement)
        {
            if(context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
            }

            if(context.User.HasClaim(claim => claim.ValueType == ClaimTypes.Country && claim.Value == "Canada"))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
