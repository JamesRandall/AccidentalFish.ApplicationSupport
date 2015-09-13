---
title: Step 1 - Hello World
layout: article
---

### Step 1 - Hello World
_Last updated for v2.1.0_

This tutorial will lead you through the process of posting to a queue, reading from a queue, and outputting to the logger. I'm assuming you're using C# and Visual Studio 2015 and we're going to use Azure PaaS for the queueing and Unity as a Inversion of Control container.

As a prerequisite for the below you'll need an Azure Storage Account and the connection string to go along with it.

As with many tutorials the code samples focus on simplicity and clarity as opposed to being of production quality.

#### Setting Up The Project

Open Visual Studio and create a new console application based on .Net 4.5.1 or higher. After the solution has been created you should have the usual empty template for a console application:

~~~
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
~~~

Now we need to add the required AccidentalFish.ApplicationSupport packages from NuGet. In the package manager console (or the GUI) add the packages as follows:

~~~
Install-Package AccidentalFish.ApplicationSupport.Unity
Install-Package AccidentalFish.ApplicationSupport.Core
Install-Package AccidentalFish.ApplicationSupport.Azure
~~~

In order to support a high level of testability the features of the AccidentalFish.ApplicationSupport framework are accessed by interfaces with implementations provided by a IoC container. In order to support multiple IoC containers (currently support is available for Unity, Autofac and Ninject) the framework uses an abstraction over container represented by the _IDependencyResolver_ interface. We need to construct a concrete implementation of this for Unity and then register the framework features we want to use - add the following lines inside the _Main_ method.

~~~
IUnityContainer unityContainer = new UnityContainer();
IDependencyResolver dependencyResolver = new UnityApplicationFrameworkDependencyResolver(unityContainer);
dependencyResolver
    .UseCore(loggerType: Bootstrapper.LoggerTypeEnum.Console)
    .UseAzure();
~~~

And add the following to your using statements at the top of the Main.cs file:

~~~
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using AccidentalFish.ApplicationSupport.Unity;
using AccidentalFish.ApplicationSupport.Azure;
using Microsoft.Practices.Unity;
using LoggerTypeEnum = AccidentalFish.ApplicationSupport.Core.Bootstrapper.LoggerTypeEnum;
~~~

This should all compile but at the moment doesn't really do very much.

#### Obtaining a Queue Reference

Before we can obtain a reference to a queue we need to define a type for the queue items. We're going to simply post user messages on to a queue and read them off so add the following new class to the project:

~~~
class QueuedMessage
{
    public string Message { get; set; }
}
~~~

Now that's added return to main.cs and let's get hold of a queue by adding the below to the bottom of the Main method:

~~~
IQueueFactory queueFactory = dependencyResolver.Resolve<IQueueFactory>();
IAzureResourceManager resourceManager = dependencyResolver.Resolve<IAzureResourceManager>();
IQueue <QueuedMessage> queue = queueFactory.CreateQueue<QueuedMessage>(
    "DefaultEndpointsProtocol=https;AccountName=accountname;AccountKey=accountkey",
    "myqueue");
resourceManager.CreateIfNotExists(queue);
~~~

You should replace the dummy storage account connection string above with a real storage account connection string. There are other ways to create resources with the framework which we'll explore in later walk throughs and it's also worth noting that the framework is predominantly an async/await framework however there is also support for synchronous methods and as we're coding in the Main method I'm using these here for clarity.

Again at this point things should compile and as long as you've supplied a valid account name and key you should find the queue _myqueue_ created in your storage account.

#### Enqueuing an Item

At the botton of the Main method add a line as follows:

~~~
EnqueueMessages(queue);
~~~

And then within the Program class add the following function:

~~~
private static void EnqueueMessages(IQueue<QueuedMessage> queue)
{
    bool shouldContinue = true;
    while (shouldContinue)
    {
        Console.Write("Message: ");
        string message = Console.ReadLine();
        if (String.IsNullOrWhiteSpace(message))
        {
            shouldContinue = false;
        }
        else
        {
            queue.Enqueue(new QueuedMessage
            {
                Message = message
            });
        }
    }
}
~~~

In this method we're simply reading text from the console and you can towards the bottom constructing our queue payload and posting that on the queue. You can quickly check that items are posted on the queue by running the console app, typing a message, and hitting Enter. Then in Visual Studio open the Server Explorer and navigate through the Azure section, into Storage accounts, and then on to your queue. Double click it and you should see the messages you've posted there waiting for you.

*Note:* If you find you get an exception when you post on the queue then check that the NewtonSoft.Json package is 7.0.0.0 or later.

## Dequeuing Items

We're going to dequeue items in a background task and output the message to the console. At the botton of the Main method add a line before EnqueueMessages so that the bottom of Main reads:

~~~
Task.Run(() => DequeueMessages(queue));
EnqueueMessages(queue);
~~~

And add the DequeueMessages method to the Program class:

~~~
private static void DequeueMessages(IQueue<QueuedMessage> queue)
{
    while (true)
    {
        bool shouldPause = true;
        queue.Dequeue(msg =>
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(msg.Item.Message);
                Console.ForegroundColor = ConsoleColor.White;
                shouldPause = false;
                return true;
            });
        if (shouldPause)
        {
            Thread.Sleep(1000);
        }
    }
}
~~~

Now when you run the console application you should see the messages on the queue posted to the console and as you add messages they appear.

The synchronous Dequeue message accepts a Func as a parameter that allows you to process an item if one was recieved. The message passed in is a [wrapper for the deserialized queue payload](http://jamesrandall.github.io/docs/accidentalfish.applicationsupport/html/T_AccidentalFish_ApplicationSupport_Core_Queues_IQueueItem_1.htm) along with the payload. From this function you should return true if you want to remove the item from the queue or false if it should be returned to the queue.

#### Next Steps

In the next part we're going to extend this sample and look at how we can use another feature of the framework, the component host, to take care of nuts and bolts of processing queues in a more realistic scenario.












