using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Application.Common
{
    /// <summary>
    /// Extension methods for IdentityResult to convert it to a Response object.
    /// </summary>
    public static class IdentityResultExtension
    {
        /// <summary>
        /// Converts an IdentityResult to a Response object.
        /// </summary>
        public static Response<T> ToResponse<T>(this IdentityResult result, T data)
        {
            if (result.Succeeded)
            {
                return new Response<T>
                {
                    IsSuccess = true,
                    Data = data
                };
            }

            var primaryError = result.Errors.First();
            return new Response<T>
            {
                IsSuccess = false,
                Message = primaryError.Description,
                ErrorType = IdentityErrorMapper.MapToErrorType(primaryError.Code)
            };
        }
    }
}
