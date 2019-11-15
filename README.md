---
page_type: sample
languages:
- csharp
products:
- dotnet
description: SCIM reference code  
urlFragment: "update-this-to-unique-url-stub"
---

# Official Microsoft Sample

This code is intended to be a useful reference for those hoping to create their own SCIM endpoint. All the basic requirements for CRUD with two resources and an extension exist in the reference. Many of the useful optional features, such as filtering and pagination, are also provided.

## Contents

| File/folder       | Description                                |
|-------------------|--------------------------------------------|
| `ScimRefrenceAPI` | Sample source code.                        |
| `.gitignore`      | Define what to ignore at commit time.      |
| `CHANGELOG.md`    | List of changes to the sample.             |
| `CONTRIBUTING.md` | Guidelines for contributing to the sample. |
| `README.md`       | This README file.                          |
| `LICENSE`         | The license for the sample.                |

## Prerequisites

Visual Studio with .NET core, IIS is recomended for running the sample.
To use the test files with extension .jmx Jmeter, blazemeter, or equivelent is required.

## Setup

1. Open Visual Studio and choose checkout. 
2. Use the clone link from Github to make a local copy of all files.

## Runnning the sample

The solution is in the ScimReferenceApi folder and can be built and run from VisualStudio. Either locally for testing purposes with IIS Express or it can be published to Azure as a web app.
All the enpoints are are at the host /api/ directory and can be interacted with the standard HTTP requests. eg {host}/api/Users can take a GET reequest that will return all users. The /api/ route can be changed at the top of each controller.
Also included in the repository is a .jmx file that can be used with Jmeter or other simialr tools for validation purposes. Currently the http request defaults is set to send requests to https://scimreferenceapi19.azurewebsites.net/ but will need to be changed to your specific URI.
All the tests pass with the code in the repo and check CRUD for users and groups along with pagination, filtering, and attribute filtering. Any changes to the source code should at least pass all the sample tests.

## Key concepts

This reference code was developed as a .Net core MVC web API for SCIM provisioning. There are three main folders for the logic of implementing SCIM: Schemas, Controllers, and Protocol. 
The Schemas folder includes the models for the resources User and Group along with some abstract classes like Schematized for shared functionality. Schemas also contains an Attributes folder which contains the class definitions for complex attributes of Users and Groups. eg address
The Controllers folder contains the controllers for the various SCIM endpoints. Here again the Users and Groups are the most important aspect to consider as SCIM was designed for resource provisioning. Both resource controllers include 7 main functions for the HTTP verbs pertaining to CRUD for the resource. 
The Protocol folder contains the code for actions relating to the way resources are returned according to the SCIM RFC. Such as returning multiple resources as a list or returning only specific resources based on a filter.
Here there is also the most of the logic. For example FilterExpression contians the logic for turning a querry into a list of linked lists of single filters. There is also the logic for turning a patch request into an operation with attributes 
pertating to the value path and the type of operation that can then be used to apply changes to resource objects.
Currently the user extension enterpriseUser is handled at the user endpoint as well by checking the schemas included with a post to determine the type of user. Therefore to add another extension one would have to include another schema with it and check for its existance in the post as well.


## Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
