namespace Plea.Core.Responses.Abstracts;

public interface IPleaErrorResponse : IPleaResponse
{
    string? Message { get; }
}