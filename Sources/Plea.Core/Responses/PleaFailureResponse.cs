using System.Collections.ObjectModel;
using Carcass.Core;
using Plea.Core.Responses.Abstracts;

namespace Plea.Core.Responses;

public sealed class PleaFailureResponse : IPleaFailureResponse
{
    public PleaFailureResponse(ReadOnlyDictionary<string, object?> metadata, string? reason = default)
    {
        ArgumentVerifier.NotNull(metadata, nameof(metadata));

        Metadata = metadata;
        Reason = reason;
    }

    public PleaStatus Status => PleaStatus.Failure;
    public ReadOnlyDictionary<string, object?> Metadata { get; }
    public string? Reason { get; }
}