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

using Bogus;
using Carcass.Metadata.Accessors.AdHoc.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Plea.AspNetCore.Controllers.Abstracts;

namespace Plea.Sample.WebApi.Controllers;

[Route("api/posts")]
public sealed class PostController : PleaController
{
    public PostController(IAdHocMetadataAccessor adHocMetadataAccessor) : base(adHocMetadataAccessor)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Post>>> GetPostsAsync(
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();

        Random random = new();

        IEnumerable<Post> posts = new Faker<Post>()
            .StrictMode(true)
            .RuleFor(p => p.Id, f => ++f.IndexFaker)
            .RuleFor(p => p.Title, f => $"Post {f.IndexFaker}")
            .RuleFor(p => p.Body, f => f.Lorem.Lines(1))
            .Generate(random.Next(15))
            .ToArray();

        AdHocMetadataAccessor.WithAdHocMetadata("cid", Guid.NewGuid().ToString());

        return await PleaOkAsync(posts);
    }
}