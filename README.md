AccidentalFish.ApplicationSupport
=================================

Note: There is currently a major new release in the v1.0.0 branch and in the pre-release area of NuGet. I've had the oppurtunity to use this fairly extensively now and it's working fine. If you're new to this project I strongly recommend starting there as it fixes a bunch of what I consider to be fundamental issues with the code as it stands in master and includes a number of breaking changes. It is likely to become the mainline version in early May.

As I've worked on a variety of Azure projects over the last 18 months there is a bunch of plumbing I've found to be common for example wanting dependency injectable patterns for resource access, configuring components across multiple projects and servers, deployment, separation of concerns, fault diagnosis and a management dashboard, to name just a few.

The AccidentalFish.ApplicationSupport framework is my attempt to bring solutions to these common requirements into a reusable package in order to bootstrap my own, and hopefully others, work.

Different parts of the framework are at different states of maturity but moving quite quickly as since deciding to collect this code together into a consist package I'm using them in this form in two personal projects (both planned to be open source).

Documentation is currently scant but that is a priority for me, the framework itself continues to evolve as I continue to learn more about Azure and as Azure itself moves forward. .

I'm publishing early basically so some friends can make use of it and so that I can pull it out of my applications to manage separately, but if you have any feedback let me know. Bug fixes greatly welcomed!

It's covered by the permissive MIT License so is free to use in open source and commercial applications.

Going forwards I expect to make reference to this framework from my [Azure From The Trenches blog](http://www.azurefromthetrenches.com) and it forms part of the companion project I am readying for that site.

