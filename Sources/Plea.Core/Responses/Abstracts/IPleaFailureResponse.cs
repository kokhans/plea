namespace Plea.Core.Responses.Abstracts;

public interface IPleaFailureResponse : IPleaResponse
{
    string? Reason { get; }
}