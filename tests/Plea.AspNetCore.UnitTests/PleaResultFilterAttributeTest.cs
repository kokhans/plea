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
using Carcass.Metadata.Stores;
using Carcass.Metadata.Stores.Abstracts;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Plea.AspNetCore.FilterAttributes;
using Plea.AspNetCore.Results.Failures;
using Plea.Core.Responses;
using Plea.Core.Responses.Abstracts;
using Xunit;

namespace Plea.AspNetCore.UnitTests;

public sealed class PleaResultFilterAttributeTest
{
    [Theory]
    [MemberData(nameof(BadRequestData))]
    public void GivenBadRequestData_WhenOnResultExecuting_ThenShouldBeHandledAsPleaBadRequestResult(
        IActionResult actionResult
    )
    {
        // Arrange
        IServiceProvider serviceProviderMock = Substitute.For<IServiceProvider>();
        serviceProviderMock
            .GetService(typeof(ILogger<ResultFilterAttribute>))
            .Returns(Substitute.For<ILogger<ResultFilterAttribute>>());
        serviceProviderMock
            .GetService<IMetadataStore>()
            .Returns(new InMemoryMetadataStore());

        DefaultHttpContext defaultHttpContext = new()
        {
            RequestServices = serviceProviderMock
        };
        ActionContext actionContext = new(defaultHttpContext, new RouteData(), new ActionDescriptor());
        ResultExecutingContext resultExecutingContext = new(
            actionContext,
            filters: new List<IFilterMetadata>(),
            result: actionResult,
            controller: null!
        );
        PleaResultFilterAttribute pleaResultFilterAttribute = new();

        // Act
        pleaResultFilterAttribute.OnResultExecuting(resultExecutingContext);

        // Assert
        PleaBadRequestResult pleaBadRequestResult = resultExecutingContext.Result
            .Should()
            .BeAssignableTo<PleaBadRequestResult>()
            .Subject;

        pleaBadRequestResult.StatusCode.Should().Be((int) HttpStatusCode.BadRequest);

        PleaFailureResponse pleaFailureResponse = pleaBadRequestResult.Value
            .Should()
            .BeAssignableTo<IPleaResponse>()
            .And
            .BeAssignableTo<PleaFailureResponse>()
            .Subject;

        pleaFailureResponse.Status.Should().Be(PleaStatus.Failure);
        pleaFailureResponse.Metadata.Should().NotBeNull();
        pleaFailureResponse.Reason.Should().BeNullOrWhiteSpace();
    }

    public static IEnumerable<object[]> BadRequestData()
    {
        yield return new object[] { new ObjectResult(null) };
        yield return new object[] { new JsonResult(null) };
    }
}