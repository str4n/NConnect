namespace NConnect.Shared.Api.Exceptions.Mappers;

internal interface IExceptionToResponseMapper
{
    ExceptionResponse Map(Exception exception);
}