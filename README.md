---
page_type: sample
languages:
- csharp
products:
- dotnet
description: SCIM provisioning reference code  
urlFragment: "update-this-to-unique-url-stub"
---

# SCIM Reference Code

<!-- 
Guidelines on README format: https://review.docs.microsoft.com/help/onboard/admin/samples/concepts/readme-template?branch=master

Guidance on onboarding samples to docs.microsoft.com/samples: https://review.docs.microsoft.com/help/onboard/admin/samples/process/onboarding?branch=master

Taxonomies for products and languages: https://review.docs.microsoft.com/new-hope/information-architecture/metadata/taxonomies?branch=master
-->

Use this reference code to get started building a [SCIM](https://docs.microsoft.com/azure/active-directory/manage-apps/use-scim-to-provision-users-and-groups) endpoint. It contains all the basic requirements for CRUD operatons on a user and group object. In addition, it contains many useful optional features such as filtering and pagination. This code is intended to help you get started building your SCIM endpoint and is provided "AS IS." It is intended as a reference to get started and there is no gaurantee of actively maintaining or supporting it. 

## Capabilities 

|Endpoint|Description|
|---|---|
|/User|Perform CRUD operations on a user object. <br/> Create <br/> Update (PUT and PATCH) <br/> Delete <br/> Get <br/> List <br/> Filter <br/> Sort <br/> Patch <br/> Paginate|
|/Group|Perform CRUD operations on a group object. <br/> Create <br/> Update <br/> Delete <br/> Get <br/> List <br/> Filter <br/> Sort <br/> Patch <br/> Paginate|
|/Schemas|The set of attributes supported by each client and service provider can vary. While one service provider may include “name”, “title”, and “emails” another service provider may use “name”, “title”, and “phoneNumbers”. The schemas endpoint allows for discovery of the attributes supported. Currently available in the reference code.|
|/ResourceTypes|Specifies metadata about each resource. Currently available in reference code (returns a file with the supported resource endpoints - schema, name, and id for users and groups)|
|/ServiceProviderConfig|Provides details about the features of the SCIM standard that are supported. For example it would indicate what resources are supported and the authentication method. Currently available in reference code (lists whether things such as PATCH are supported)|
|/Bulk|Bulk operations allow you to perform operations on a large collection of resource objects in a single operation (e.g. update memberships for a large group). Currently unavailable in reference code.|
## Prerequisites

* [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) (required)
* [.NET core 2.2 or above](https://dotnet.microsoft.com/download/dotnet-core/2.2) (required)
* IIS (recommended)
* Testing platform such as [Postman](https://www.getpostman.com/downloads/) or [Jmeter](https://jmeter.apache.org/download_jmeter.cgi) (recommended)
    * [Java](https://www.java.com/en/download/) is required if using Jmeter

## Clone or fork the repo and build your SCIM endpoint

The solution is in the ScimReferenceApi folder and can be built and run from VisualStudio locally or hosted in the cloud.

#### Steps to run the solution locally
1. Click the clone or download link near the top of the page and copy the link. OR. Click fork near the top of the page to make a copy of the repo for personal use.
2. Open Visual Studio and choose clone or checkout. 
3. Use the clone link from Github int the repository link feild and click clone to make a local copy of all files. The solution should open.
4. Switch views to the SCIMReference.sln
5. Click IIS Express to execute (the project will build and you will be redirected to a web page with the local host URL)

#### Steps to host the solution in the Azure
1. Open Visual Studio and sign into the account that has access to your hosting resources. 
2. Click the solution file in the solution explorer and select publish from the list on the left of the window. Alternatively use the Search (ctrl + q) at the top of the page to search for "publish."
    1. If the solution has not yet been published this will open a dialog in a new window.
4. Click create profile.
    1. Make sure app service is selected and the radio button for "create new" is selected. 
5. Walk through the options in the dialog. 
6. Remove the date time numbers from the app name. The name field is used for both the app name and the SCIM URL.
7. Select the resource group and plan you would like to use and hit publish.

All the endpoints are are at the host /api/ directory and can be interacted with the standard HTTP requests. eg {host}/api/Users can take a GET reequest that will return all users. The /api/ route can be changed at the top of each controller.

## Test your SCIM endpoint
This project provides test cases that you can use to ensure your application is SCIM compliant. The test cases have been authored for both Jmeter and Postman.

#### Postman testing instructions
1. Download the [Postman collection](https://aka.ms/ProvisioningPostman)
2. Download the [Postman client](https://www.getpostman.com/downloads/)
3. Import the postman collection as raw text. **Import** > **Paste Raw Text**
4. Specify the URL for your SCIM endpoint
    1. When running the project locally, the format is typically (replace 44355 with the port found in the URL that opens up when you execute the project): https://localhost:44355/api/users 
    2. When hosting the endpoint in Azure, the URL is typically similar to: https://scimreferenceapi19.azurewebsites.net/api/users)
5. Turn off SSL Cert verification. **File** > **Settings** > **SSL certificate verification**
6. Ensure that you are authorized to make requests to the endpoint
    1. Option 1: Turn off authorization for your endpoint (this is fine for testing purposes, but there must be some form of authorization for apps being used by customers in production.
    2. Option 2: POST to key endpoint to retrieve a token
7. Run your tests

#### Jmeter instructions
The repository contains a .jmx file that can be used with Jmeter or other simialr tools for testing purposes. The http request default is set to send requests to https://scimreferenceapi19.azurewebsites.net/ but will need to be changed to your specific URI.


1. Download [Java](https://www.java.com/en/download/) 
2. Download [Jmeter](https://jmeter.apache.org/download_jmeter.cgi) (Apache open source testing framework built with java available) 
3. Add Java to PATH
3. Unzip the download there and execute the ApacheJMeter.jar file
4. Once it runs file -> open ~.jmx will load the tests
5. Under thread group the target URI can be enabled/disabled or changed with the user defined varible options 
6. While the sample is running, click "ctrl + r" or the "start" button
7. Review the results in the results tree.

#### Tests executed

* CRUD operations on a user
* Filtering
* Attribute filtering

## Navigating the reference code

This reference code was developed as a .Net core MVC web API for SCIM provisioning. The three main folders are Schemas, Controllers, and Protocol. 
* The **Schemas** folder includes the models for the User and Group resources along with some abstract classes like Schematized for shared functionality. Schemas also contains an Attributes folder which contains the class definitions for complex attributes of Users and Groups such as address
* The **Controllers** folder contains the controllers for the various SCIM endpoints. Here again the Users and Groups are the most important aspect to consider as SCIM was designed for resource provisioning. Both resource controllers include 7 main functions for the HTTP verbs pertaining to CRUD for the resource. 
* The **Protocol** folder contains the code for actions relating to the way resources are returned according to the SCIM RFC. Such as returning multiple resources as a list or returning only specific resources based on a filter.
Here there is also the most of the logic. For example FilterExpression contians the logic for turning a querry into a list of linked lists of single filters. There is also the logic for turning a patch request into an operation with attributes 
pertating to the value path and the type of operation that can then be used to apply changes to resource objects.
Currently the user extension enterpriseUser is handled at the user endpoint as well by checking the schemas included with a post to determine the type of user. Therefore to add another extension one would have to include another schema with it and check for its existance in the post as well.

## Common scenarios
|Scenario|How-to|
|---|---|
|Enable / disable authorization|Navigate to the UsersController.cs file and comment out the authorize command|
|Add additional filterable attributes|Navigate to the FilterUsers.cs or FilterGroups.cs file and update the method to take a filter expression|
|Extend the user schema to support additional attributes||

## Contents


| File/folder       | Description                                |
|-------------------|--------------------------------------------|
| `ScimRefrenceAPI` | Sample source code.                        |
| `.gitignore`      | Define what to ignore at commit time.      |
| `CHANGELOG.md`    | List of changes to the sample.             |
| `CONTRIBUTING.md` | Guidelines for contributing to the sample. |
| `README.md`       | This README file.                          |
| `LICENSE`         | The license for the sample.                |

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
