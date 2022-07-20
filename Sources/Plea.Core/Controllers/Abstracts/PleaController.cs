// MIT License
//
// Copyright (c) 2022 Serhii Kokhan
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using Carcass.Core;
using Carcass.Metadata.Accessors.AdHoc.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Plea.Core.FilterAttributes;
using Plea.Core.Results.Failures;
using Plea.Core.Results.Successes;

// ReSharper disable UnusedMember.Global

namespace Plea.Core.Controllers.Abstracts;

[ApiController]
[ServiceFilter(typeof(PleaExceptionFilterAttribute))]
[PleaResultFilter]
public abstract class PleaController : ControllerBase
{
    protected PleaController(IAdHocMetadataAccessor adHocMetadataAccessor)
    {
        ArgumentVerifier.NotNull(adHocMetadataAccessor, nameof(adHocMetadataAccessor));

        AdHocMetadataAccessor = adHocMetadataAccessor;
    }

    protected IAdHocMetadataAccessor AdHocMetadataAccessor { get; }

    protected async Task<PleaOkResult<TData>> PleaOkAsync<TData>(TData? data = default) => new(await AdHocMetadataAccessor.GetMetadataAsync(), data);
    protected async Task<PleaCreatedResult<TData>> PleaCreatedAsync<TData>(TData? data = default) => new(await AdHocMetadataAccessor.GetMetadataAsync(), data);
    protected async Task<PleaBadRequestResult> PleaBadRequestAsync(string? reason = default) => new(await AdHocMetadataAccessor.GetMetadataAsync(), reason);
    protected async Task<PleaUnauthorizedResult> PleaUnauthorizedAsync(string? reason = default) => new(await AdHocMetadataAccessor.GetMetadataAsync(), reason);
    protected async Task<PleaNotFoundResult> PleaNotFoundAsync(string? reason = default) => new(await AdHocMetadataAccessor.GetMetadataAsync(), reason);
}