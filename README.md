# Modular Monolith

This is a quick and dirty demonstration of the concept of a (very simple) Modular Monolith in action.

### What is a Modular Monolith?

According to ChatGPT: 

> A modular monolith is an architectural approach that combines the benefits of modularity with the simplicity of a monolithic architecture. In a modular monolith, the application is built as a single, cohesive unit, but it is organized into well-defined modules or components that encapsulate specific functionalities or domains.

### How has this approach been implemented in this project?

There are currently three 'modules' in this project: `OrderModule`, `ProductModule` and `UserModule`. Each of these modules look after a distinct part of the system. They don't share code by default (almost all their classes are internal) and also store their data independently of each other (not unlike a microservice!).

Of the three, the `OrderModule` is the most interesting as it needs to reach out to the other two modules to get its job done (i.e. it takes a dependency on those other two modules).

To facilitate communication between the API project (the main point of entry) and the modules, as well as between the modules themselves, the mediator pattern via the [MediatR](https://github.com/jbogard/MediatR) library is used for in-process messaging. There is a `Contracts` project which contains all the shared code in the solution used to communicate between modules.