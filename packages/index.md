---
title: NuGet Packages
layout: article
---
### NuGet Packages

The framework is comprised of multiple NuGet packages in order to allow for a pick and choose approach to the functionality you want to use and minimise the number of dependencies dragged into your project.

At a minimum you are likely to want to bring in the Core, Abstract Dependency Resolver and Unity Dependency Resolver in order to make basic use of the framework. Beyond that what else you bring in depends on your requirements - for example if you want to use Azure storage (blobs, tables, queues) and service bus then you're likely to want to include the Azure PaaS package. On the other hand if you want to use a SQL database then you are likely to want to include the Entity Framework Repository package that will provide an implementation of the unit of work and repository pattern.

Package|Description
-------|-----------
[Azure PaaS](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.Azure/)|Azure queue, table storage, topic and subscription implementations
[Core](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.Core/)|The core library - defines many of the frameworks interfaces and some of the dependency free implementation
[Abstract Dependency Resolver](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.DependencyResolver/)|Buffers the framework from a specific dependency injector
[Amazon SES Email Provider](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.Email.Amazon/)|Amazon SES email implementation for use with the email templating and dispatch system
[SendGrid Email Provider](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.Email.SendGrid/)|SendGrid email implementation for use with the email templating and dispatch system
[Application Insights Logger](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.Logging.ApplicationInsights/)|Extends the built in logger to push warnings and errors to Azure Application Insights
[Powershell Cmdlets](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.Powershell/)|Powershell cmdlets for supporting configuration and a continual delivery / automated deployment system
[Background Processes](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.Processes/)|Email dispatch and log queue processing
[Entity Framework Repository](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.Repository.EntityFramework/)|Entity Framework implementation of the unit of work and repository pattern defined in the core package
[Unity Dependency Resolver](https://www.nuget.org/packages/AccidentalFish.ApplicationSupport.Unity/)|Unity implementation of the dependency resolver


