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

using System.Net;
using Carcass.Core;
using Carcass.Core.Extensions;
using Carcass.Metadata.Stores.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Plea.AspNetCore.Results.Abstracts;
using Plea.AspNetCore.Results.Errors;
using Plea.AspNetCore.Results.Failures;
using Plea.AspNetCore.Results.Successes;
using Plea.Core.Responses.Abstracts;

namespace Plea.AspNetCore.FilterAttributes;

public sealed class PleaResultFilterAttribute : ResultFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        ArgumentVerifier.NotNull(context, nameof(context));

        IMetadataStore metadataStore = context.HttpContext.RequestServices.GetRequiredService<IMetadataStore>();

        switch (context.Result)
        {
            case IPleaResult<IPleaResponse>:
                return;
            case JsonResult jsonResult:
                context.Result = GetResult(GetStatusCode(jsonResult), jsonResult.Value, metadataStore);
                break;
            case ObjectResult objectResult:
                context.Result = GetResult(GetStatusCode(objectResult), objectResult.Value, metadataStore);
                break;

                static IActionResult GetResult(
                    HttpStatusCode httpStatusCode,
                    object? value,
                    IMetadataStore metadataStore
                )
                {
                    ArgumentVerifier.NotNull(metadataStore, nameof(metadataStore));

                    if (httpStatusCode.IsSuccessHttpStatusCode())
                        return new PleaOkResult<object>(metadataStore.GetMetadataAsync().Result, value);

                    if (httpStatusCode.IsClientErrorsHttpStatusCode())
                        return new PleaBadRequestResult(metadataStore.GetMetadataAsync().Result, value?.ToString());

                    if (httpStatusCode.IsServerErrorsHttpStatusCode())
                        return new PleaInternalServerErrorResult(metadataStore.GetMetadataAsync().Result,
                            value?.ToString());

                    throw new ArgumentOutOfRangeException(nameof(httpStatusCode), httpStatusCode.ToString());
                }

                static HttpStatusCode GetStatusCode(IStatusCodeActionResult? statusCodeActionResult)
                {
                    return statusCodeActionResult?.StatusCode is not null
                        ? (HttpStatusCode) statusCodeActionResult.StatusCode.Value
                        : HttpStatusCode.BadRequest;
                }
        }
    }
}