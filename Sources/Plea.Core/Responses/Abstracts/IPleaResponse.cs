using System.Collections.ObjectModel;

namespace Plea.Core.Responses.Abstracts;

public interface IPleaResponse
{
    PleaStatus Status { get; }
    ReadOnlyDictionary<string, object?> Metadata { get; }
}