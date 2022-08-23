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
using Carcass.Logging.Core.Adapters;
using Carcass.Logging.Core.Adapters.Abstracts;
using Carcass.Metadata.Accessors.AdHoc.Abstracts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Plea.AspNetCore.Results.Errors;

namespace Plea.AspNetCore.FilterAttributes;

public sealed class PleaExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override async Task OnExceptionAsync(ExceptionContext context)
    {
        ArgumentVerifier.NotNull(context, nameof(context));

        ILoggerAdapterFactory loggerAdapterFactory =
            context.HttpContext.RequestServices.GetRequiredService<ILoggerAdapterFactory>();
        LoggerAdapter<PleaExceptionFilterAttribute> loggerAdapter =
            loggerAdapterFactory.CreateLoggerAdapter<PleaExceptionFilterAttribute>();
        IAdHocMetadataAccessor adHocMetadataAccessor =
            context.HttpContext.RequestServices.GetRequiredService<IAdHocMetadataAccessor>();

        context.ExceptionHandled = true;
        context.Result = new PleaInternalServerErrorResult(
            await adHocMetadataAccessor.GetMetadataAsync(),
            context.Exception.Message
        );

        loggerAdapter.LogError(context.Exception);
    }
}