---
page_type: sample
languages:
- csharp
products:
- dotnet
description: SCIM provisioning reference code  
urlFragment: "update-this-to-unique-url-stub"
---

# Official Microsoft Sample

<!-- 
Guidelines on README format: https://review.docs.microsoft.com/help/onboard/admin/samples/concepts/readme-template?branch=master

Guidance on onboarding samples to docs.microsoft.com/samples: https://review.docs.microsoft.com/help/onboard/admin/samples/process/onboarding?branch=master

Taxonomies for products and languages: https://review.docs.microsoft.com/new-hope/information-architecture/metadata/taxonomies?branch=master
-->

This code is intended to be a useful reference for those building their own [SCIM](https://docs.microsoft.com/azure/active-directory/manage-apps/use-scim-to-provision-users-and-groups) endpoint. All the basic requirements for CRUD with two resources and an extension exist in the reference. Many of the useful optional features, such as filtering and pagination, are also provided. This code is intended to help you get started building your SCIM endpoint and is provided "AS IS." It is intended as a reference to get started. There is no gaurantee of actively maintaining or supporting the reference code.  

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

* [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) (required)
* [.NET core 2.2 or above](https://dotnet.microsoft.com/download/dotnet-core/2.2) (required)
* IIS (recommended)
* Testing platform such as [Postman](https://www.getpostman.com/downloads/) or [Jmeter](https://jmeter.apache.org/download_jmeter.cgi) (recommended)

## Setup

1. Click the clone or download link near the top of the page and copy the link. OR. Click fork near the top of the page to make a copy of the repo for personal use.
2. Open Visual Studio and choose clone or checkout. 
3. Use the clone link from Github int the repository link feild and click clone to make a local copy of all files. The solution should open.

Jmeter is an apache open source testing framework built with java available for download at https://jmeter.apache.org/download_jmeter.cgi
Java is required to run Jmeter and should be add to PATH
After expanding the download there is a .jar file ApacheJMeter file that can be executed.
Once it runs file -> open ~.jmx will load the tests
Under thread group the target URI can be enabled/disabled or changed with the user defined varible options 
Making sure the sample is running the test can be run (ctrl + r), or clicking the start button
The results of the tests apear in the results tree.

## Runnning the sample

The solution is in the ScimReferenceApi folder and can be built and run from VisualStudio. Either locally for testing purposes with IIS Express or it can be published to Azure as a web app.
All the enpoints are are at the host /api/ directory and can be interacted with the standard HTTP requests. eg {host}/api/Users can take a GET reequest that will return all users. The /api/ route can be changed at the top of each controller.
Also included in the repository is a .jmx file that can be used with Jmeter or other simialr tools for validation purposes. Currently the http request defaults is set to send requests to https://scimreferenceapi19.azurewebsites.net/ but will need to be changed to your specific URI.
All the tests pass with the code in the repo and check CRUD for users and groups along with pagination, filtering, and attribute filtering. Any changes to the source code should at least pass all the sample tests.

To publish the solution to Azure first make sure you are signed into visual studio with the account that has acces to your hosting resources. Then click the solution file in the solution explorer and select publish from the list on the left of the window. Alternatively use the Search (ctrl + q) at the top of the page to search for "publish"
If the solution has not yet been published this will open a dialog in a new window.
Make sure app service is selected and the radio button for create new is pressed and click create profile.
This will advance the dialog to have several options. 
The name feild is for the name of the app but is also the first part of the default URL so removeing the date time numbers from the end is a good idea.
Select the resource group and plan you would like to use then hit publish.


## Testing your SCIM endpoint
This project provides test cases that you can use to ensure your application is SCIM compliant. The test cases have been authored for both Jmeter and Postman.

#### Postman testing instructions
1. [Download Postman collection](https:aka.ms/ProvisioningPostman)
2. [Download the Postman client]()
3. Import the postman collection as raw text. **Import** > **Paste Raw Text**
4. Point the URL to your endpoint (for example: https://localhost:<input>/api/users or https://scimreferenceapi19.azurewebsites.net/api/users)
5. Turn off SSL Cert verification. **File** > **Settings** > **SSL certificate verification**
6. Ensure that you are authorized to make requests to the endpoint
    1. Option 1: Turn off authorization for your endpoint (this is fine for testing purposes, but there must be some form of authorization for apps being used by customers in production.
    2. Option 2: POST to key endpoint to retrieve a token
7. Run your tests


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
