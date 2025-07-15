using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Auth.Application.Common;

/// <summary>
/// Error types for API responses.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ErrorType
{
    [EnumMember(Value = "invalid_request_error")]
    InvalidRequestError,

    [EnumMember(Value = "conflict_error")] ConflictError,

    [EnumMember(Value = "authentication_error")]
    AuthenticationError,

    [EnumMember(Value = "locked_error")] LockedError,

    [EnumMember(Value = "permission_error")]
    PermissionError,

    [EnumMember(Value = "api_error")] ApiError
}