# How to contribute

The easiest way to contribute is to open an issue and start a discussion. The community is welcome to submit additional samples to this repo.

Samples should be:
* Targeted - Each sample should be small and demonstrate a single feature or concept
* Self contained - you should be able to copy the folder for the sample and not have to go hunting for contract definitions or helper classes
* Simple to follow - the simpler the code is, the easier it is for other developers to be able to follow, and to be able to copy snippets from for use in their projects 
* Minimal dependencies - don't reference other nuget packages or frameworks if they are not critical to the functionality of the sample
* Paired - samples should include code for client and server, or supply any special instructions for creating a client using svcutil.
* Targeting the latest released version of CoreWCF - Please use a draft PR until the corresponding feature is complete and included in a release
* Async for the client

Also read this first: [Being a good open source citizen](https://hackernoon.com/being-a-good-open-source-citizen-9060d0ab9732#.x3hocgw85)

## General feedback and discussions

Please use the [CoreWCF issues](https://github.com/CoreWCF/CoreWCF/issues) for anything not directly related to samples.

## Platform

Core WCF is built targeting .NET Standard 2.0. Unless the sample is dependent on features not present, the samples should work on .NET Standard. Where possible samples should not take OS/hardware dependencies and so work on Linux and Windows on x86, x64 and Arm64.

## Building

Run `dotnet build` from the command line. This builds all the samples. The build scripts should be recursive and find all project files.

## Contributing code and content

You will need to sign a [Contributor License Agreement](https://cla.dotnetfoundation.org/) before submitting your pull request.

Make sure you can build the code. Familiarize yourself with the project workflow and our coding conventions. If you don't know what a pull request is read this article: https://help.github.com/articles/using-pull-requests.
