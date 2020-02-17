# Overview

#### TL;DR

Sample project how one could implement authentication service using IdentityServer4 middleware.

Using following techs

- IdP (IdentityServer4 NetCore 3.1 + CarterApi)
- AdminUI (ReactJs): Manage Users, Clients, Resources, Sessions, etc.
- Docker/Docmer-compose (3.4)
- Azure on the background with DevOps pipeline (private)


### Description

Project consists of .NET Core as API service in the background and ReactJS+Webpack as UI for managing (AdminUI).
As for bridge between these two I've implemented separated middleware, or sort of a proxy. Purpose for this is 
to challenge the idea of using server side rendered (e.g. Razor, or alike) pages. There are several reasons 
why one should use server rendered pages (the main one is security...).

This is my first attempt on this approach and most likely others will come later. I've chosen ReactJs as to me it
was the most commonly used by developers I've been working with. There's no actual difference between other javascript
frameworks, as they could also be used.

One new tech that I've also been interested in to get more familiar with is Blazor... but as it's not the most commonly
used tech (yet), I'll most likely have a look at it later.

###### Background:

I've been working as software architect/fullstack developer, front-end development was never my strengths and I'd be more
than glad if someone would like to contribute in this part; other ideas and thoughts are also welcome.

## NOTE! 
#### :godmode:  Project is currently under construction and is not ready for use.

- AspNetCore 3+ & (SpaServices, NodeServices)  => Obsolete https://github.com/dotnet/AspNetCore/issues/12890
- MetisMenu supports only HashRouter => Refactored implementation manually to use BrowserRouter
- Using Webpack => Upgraded from 3.x to 4.x (required somewhat refactoring in UI; cleanup not complete, but runs)

# Technical overview

I've chosen to use the latest version of .NET Core at the moment (3.1) for couple of reasons. First .NET Core 2.2 has most of the
fixes that earlier versions had with HTTP messaging ([#1](https://github.com/dotnet/runtime/issues/26397#issuecomment-395489603), 
[#2](https://github.com/dotnet/runtime/issues/21440), and the list goes on...).
 As for the second, .NET Core 2.2 been stated to be EOL and I'm looking for 
framework with Long Term Support (LST) with stable release; this also the reason for "why not 3.0" (EOS: March of 2020). The third
reason for this version is because of its edge features that I'd like very much to use, also to keep my knowldege up to date.
There's also 3rd party changes that must be taken in account; [more from here](https://github.com/aspnet/Announcements/issues/390).
Beside these reasons, there's no reaso for not using earlier versions (so if you are required to use one, please find out the challenges
that may be ahead of you and resolve, how to do workarounds for them).

## Architecture structure

The solution applies Clean Architecture where it decouples layers into projects.
For more of Clean Architecture, I find [this blog](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html) 
most helpfull to get the basic understanding on it.



#### .WebApi

This project is the top main layer. It's also the main running process that binds everything together.



#### .Application

Defines used adapters/interfaces and runs business logic.



#### .Infrastructure

Implementations for adapters and serves as gateways for used services.

###### LdapGateway

Implements gateway for AD integration. #TODO/TBA...

###### RoleGateway

Overrides `RoleManager` class to enable custom user model to be used.

###### SignInGateway

Overrides `SignInManager` class to enable custom sign in process.

###### UserGateway

Overrides `UserManager` class to support `SignInGateway` implementations in authentication process,
as also to support custom user model to be used with data interactions.



#### .Persistence

Implements local data access or layer for local data storage; that consists of contexts, repositories, etc..

To create snapshots for migrations

`ApplicationDb`
```batch
dotnet ef migrations add {snapshot_name : e.g. InitAppContext} --context ApplicationDbContext --project ./AuthenticationService.Persistence --startup-project ./AuthenticationService.WebApi -o Migrations/ApplicationDb --verbose
```

`ConfigurationDb -- Identity server`
```batch
dotnet ef migrations add {snapshot_name : e.g. InitConfContext} --context CustomConfigurationDbContext --project ./AuthenticationService.Persistence --startup-project ./AuthenticationService.WebApi -o Migrations/ConfigurationsDb --verbose
```

Use `--verbose` property only when you want to see all logs from migration snapshot process



#### .Domain

I hope the name speaks of its self...



#### .Core

Interfaces mainly for business logic contracts. Works as root-blueprint for whole project.



## Architecture



### Deployment

Project deploy is designed to support Microservice architecture, even the main solution is a monolith.
By this I mean, that one can deploy this solution/service as instance of it and run it in parralel or what ever.
Base design is only depended on single cache and single storage also if prod or self-signed certificates are used, a store/location for these certificates is required.
You can change this in code if one wants...

## Running

This project is being build with Visual Studio 2019 CE, in windows platform .NET Core 3.1 framework. 
Solution service should be runnable in docker containers (linux) and it should be scalable (horizontally, as many v-instances as your platform can handle).
Service is served from CE based subscription in Azure (AKS) cloud. AKS (Azure Kubernetes server) was chosen for couple of reasons. As it can be manually/fully customized and it's more familiar platform for me.

The dotnet runner will also run a background task, where it runs NodeJS (please verify version from webapi->dockerfile) for the admin UI (as a SPA). This is handled within the .NET code (do not touch unless you know what you're doing).
Note! When running solution/service, it will start this background task and in this task it will run npm build task. This task may take awhile and seems to developer as unresponsive or blocked process scenario. It is not! Be patient it'll work! :angel:

If this bothers someone, one could override the .NET method at startup : `UseReactDevelopmentServer` and implement your own with logging. :smirk:


The `docker-compose` is build in a way where it holds all the basic stuff the service requires (cache, database, volume-share for persisted data).


## How to run

##### Prerequirements
- Docker
- NodeJS
- Dotnet 3.1 or later
- PostgreSql (implemented; can be changed)
  - either configure networking or use vm-host IP for database address
- Redis (implemented; can be changed)


##### Visual Studio (Simplest)
Start visual studio 2019 and select profile you wish to use and press play. If you want to run it as docker-compose for some reason, please change target default project to docker-compose.

##### CLI
Change your directory path to solution root path.
Run

`dotnet run --project ./AuthenticationService/AuthenticationService.csproj`

##### Container

###### Docker-compose

Change your directory path to solution root path.
Run
`docker-compose up`
(please note docker-compose comments in "known issues" -section)

# Known issues

- Stopping debug-browser, will not stop Node-task from the background. Thus leaving the main debug process running.
  - Note that this left ignored will not prevent a new debug instance to be ran. There's a problem tho' -> when running the background taks for SPA UI,
    it will run in configured port (`3000`) and if the previous process is still running. It will block the new SPA service from being ran!
- Self-Signed-Certificates may not work correctly due to new security changes (only locally).
  - This is found in VueJS runner. This is possible to fix, by creating your own node-server-proxy and configure it to use certificates.
- `docker-compose build` event ignores environment variables, but `docker-compose up` will read them from the `docker-compose.override.yml` if defined
  - This is by design. Just you know when you try to run this from CLI with these commands.
    (if you do: change service configurations in manner where you expect your platform to be either development or production; i.e. do not expect env.vars. to do this for you)
  - When running from Visual Studio, this is not a problem. (not sure why)
- 
