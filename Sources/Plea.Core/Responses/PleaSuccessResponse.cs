using System.Collections.ObjectModel;
using Carcass.Core;
using Plea.Core.Responses.Abstracts;

namespace Plea.Core.Responses;

public sealed class PleaSuccessResponse<TData> : IPleaSuccessResponse<TData>
{
    public PleaSuccessResponse(ReadOnlyDictionary<string, object?> metadata, TData? data = default)
    {
        ArgumentVerifier.NotNull(metadata, nameof(metadata));

        Metadata = metadata;
        Data = data;
    }

    public PleaStatus Status => PleaStatus.Success;
    public ReadOnlyDictionary<string, object?> Metadata { get; }
    public TData? Data { get; }
}