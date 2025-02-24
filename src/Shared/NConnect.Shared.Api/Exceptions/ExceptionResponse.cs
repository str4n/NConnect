using System.Net;

namespace NConnect.Shared.Api.Exceptions;

internal sealed record ExceptionResponse(HttpStatusCode StatusCode, Error Error);