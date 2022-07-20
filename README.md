# Plea [![GitHub](https://img.shields.io/github/license/kokhans/plea?style=flat-square)](LICENSE)

Plea is a [.NET 6](https://docs.microsoft.com/en-us/dotnet/core/whats-new/dotnet-6) implementation of [JSend](https://github.com/omniti-labs/jsend) specification for a simple, no-frills, JSON-based format for application-level communication.

## Status

### Pre-Alpha

The software is still under active development and not feature complete or ready for consumption by anyone other than software developers. There may be milestones during the pre-alpha which deliver specific sets of functionality, and nightly builds for other developers or users who are comfortable living on the absolute bleeding edge.

## Specification

|Status Type|HTTP Status Code|Description|Properties|
|-|-|-|-|
|success|2xx|The request was successfully received, understood, and accepted|status, metadata, data|
|failure|4xx|The request contains bad syntax or cannot be fulfilled|status, metadata, reason|
|error|5xx|The server failed to fulfil an apparently valid request|status, metadata, message, code|

### Success

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

## License

This project is licensed under the [MIT license](LICENSE).