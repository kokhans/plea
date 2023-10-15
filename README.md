# Plea [![GitHub](https://img.shields.io/github/license/kokhans/plea?style=flat-square)](LICENSE)

Plea is a .NET 7 implementation of [JSend](https://github.com/omniti-labs/jsend) specification for a simple, no-frills, JSON-based format for application-level communication.

# Getting Started

## Prerequisites

- [.NET 7 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

## Installation

To install `Plea.AspNetCore` and its dependencies via .NET Core CLI, execute the following command.

```powershell
dotnet add package Plea.AspNetCore
```

To install `Plea.AspNetCore` and its dependencies via NuGet, execute the following command.

```powershell
Install-Package Plea.AspNetCore
```

## Registration

To register `Plea.AspNetCore` and its dependencies, use the following code.

```csharp
using Carcass.Metadata.Stores.Abstracts;

services
    .AddCarcassLoggerAdapterFactory()
    .AddCarcassInMemoryMetadataStore()
    .AddCarcassAdHocMetadataAccessor()
    .AddPlea();
```

## Basic Usage

To extend the controller with Plea functionality, inherit it from `PleaController`.

```csharp
[Route("api/posts")]
public sealed class PostController : PleaController
{
    public PostController(IAdHocMetadataAccessor adHocMetadataAccessor) : base(adHocMetadataAccessor) { }
}
```

To return the successful Plea response from the method, use the following code.

```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<Post>>> GetPostsAsync(
	CancellationToken cancellationToken = default
)
{
	cancellationToken.ThrowIfCancellationRequested();

	var posts = /*Get posts*/

	return await PleaOkAsync(posts);
}
```

# Specification

|Status Type|HTTP Status Code|Description|Properties|
|-|-|-|-|
|`success`|2xx|The request was successfully received, understood, and accepted|`status`, `metadata`, `data`|
|`failure`|4xx|The request contains bad syntax or cannot be fulfilled|`status`, `metadata`, `reason`|
|`error`|5xx|The server failed to fulfil an apparently valid request|`status`, `metadata`, `message`, `code`|

## Success

When an API call is successful, the Plea object is used as a simple envelope for the results, using the data key, as in the following.

GET /posts

```json
{
	"status": "success",
	"metadata": {
		"cid": "e1f1f580-eaad-4de4-a354-bf017b0f7617",
		"language": "en-US"
	},
	"data": [
		{
			"id": 1,
			"title": "Post 1",
			"body": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
		},
		{
			"id": 2,
			"title": "Post 2",
			"body": "Id cursus metus aliquam eleifend mi in nulla."
		},
		{
			"id": 3,
			"title": "Post 3",
			"body": "Elit ut aliquam purus sit amet luctus venenatis."
		}
	]
}
```

GET /posts/1

```json
{
	"status": "success",
	"metadata": {
		"cid": "f9fed10a-a889-49e0-92c5-11fc68009217",
		"language": "en-US"
	},
	"data": {
		"id": 1,
		"title": "Post 1",
		"body": "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua."
	}
}
```

## Failure

When an API call is rejected due to invalid data or call conditions, the Plea object's data key contains an object explaining what went wrong.

POST /posts

```json
{
    "status": "failure",
    "metadata": {
        "cid": "8e97389c-02dc-4333-a73d-5dde58ce89c9",
        "language": "en-US"
    },
    "reason": "The Title field is required."
}
```

## Error

When an API call fails due to an error on the server.

GET /posts

```json
{
    "status": "error",
    "metadata": {
        "cid": "b3c5d7e9-f1g2-34h5-j6k7-l8m9n0o1p2q3",
        "language": "en-US"
    },
    "message": "Internal server error",
    "code": 500
}
```

# Metadata

Metadata offers contextual information about API responses, allowing users to understand auxiliary details surrounding the main content. In Plea, the handling of such metadata is made seamless through `AdHocMetadataAccessor`.

## Setting Default Metadata

Before diving into ad-hoc metadata, it's essential to set default metadata values. In your configuration.

```csharp
IMetadataStore metadataStore = app.Services.GetRequiredService<IMetadataStore>();
metadataStore.AddOrUpdateMetadataAsync("language", "en-US");
```

## Injecting Metadata Accessor

Within your controller, ensure IAdHocMetadataAccessor is available.

```csharp
public PostController(IAdHocMetadataAccessor adHocMetadataAccessor) : base(adHocMetadataAccessor) { }
```

## Adding Ad-Hoc Metadata

Introduce unique metadata to your responses when needed.

```csharp
AdHocMetadataAccessor.WithAdHocMetadata("cid", Guid.NewGuid().ToString());
```

## Sending the Response

Once you've set your metadata—whether default or ad-hoc—it's crucial to ensure the data is correctly packaged and sent in your API response. In the Plea framework, methods like `PleaOkAsync` handle this seamlessly. When executed, it not only returns the main data but also combines it with the metadata you've prepared, delivering a comprehensive and well-structured API response for the end-users.

```csharp
return await PleaOkAsync(posts);
```

# License

This project is licensed under the [MIT license](LICENSE).