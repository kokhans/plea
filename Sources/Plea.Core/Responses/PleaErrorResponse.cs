using System.Collections.ObjectModel;
using Carcass.Core;
using Plea.Core.Responses.Abstracts;

namespace Plea.Core.Responses;

public sealed class PleaErrorResponse : IPleaErrorResponse
{
    public PleaErrorResponse(
        ReadOnlyDictionary<string, object?> metadata,
        string? message = default
    )
    {
        ArgumentVerifier.NotNull(metadata, nameof(metadata));

        Metadata = metadata;
        Message = message;
    }

    public PleaStatus Status => PleaStatus.Error;
    public ReadOnlyDictionary<string, object?> Metadata { get; }
    public string? Message { get; }
}