namespace Plea.Core.Responses.Abstracts;

public interface IPleaSuccessResponse<out TData> : IPleaResponse
{
    TData? Data { get; }
}