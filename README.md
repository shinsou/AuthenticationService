# Overview

## NOTE! 
#### :godmode:  Project is currently under construction and is not ready for use.



Sample project how one could implement authentication service (Identity service).

The main target of this project is to produce web based authentication service with administrative UI.

Note!
This is a monolith on purpose.

# Technical overview

## Architecture structure

The solution applies Clean Architecture where it decouples layers into projects.
For more of Clean Architecture, I find [this blog]:https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html of it most helpfull to get the basic understanding of it: 



#### .WebApi

This project is the top main layer. It's also the main running process that binds everything together.

#### Application



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
    it will run in configured port (`8080`) and if the previous process is still running. It will block the new SPA service from being ran!
- Self-Signed-Certificates may not work correctly due to new security changes (only locally).
  - This is found in VueJS runner. This is possible to fix, by creating your own node-server-proxy and configure it to use certificates.
- `docker-compose build` event ignores environment variables, but `docker-compose up` will read them from the `docker-compose.override.yml` if defined
  - This is by design. Just you know when you try to run this from CLI with these commands.
    (if you do: change service configurations in manner where you expect your platform to be either development or production; i.e. do not expect env.vars. to do this for you)
  - When running from Visual Studio, this is not a problem. (not sure why)
- 