using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace SchoolRegister.Web.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetErrorsString(this IEnumerable<IdentityError> errors)
        {
            var resultString = string.Empty;
            errors.ToList().ForEach(e =>
            {
                resultString += " Error code:" + e.Code + "; " + e.Description + "\n";
            });
            return resultString;
        }
    }
}