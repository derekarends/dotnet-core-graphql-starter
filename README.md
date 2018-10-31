# DotNet Core GraphQL Application
Is a demo application to show how an architecture could have logging, statistics, caching, and connecting to mongo

## Prerequisites
* Mongo installed and running

## Adding Redis Cache(not required)
* Redis docker
* docker run -d --name redis -p 6379:6379 --restart unless-stopped -v /tmp/redis/:/data redis redis-server /data
* Uncomment UserCache in UserDeps.cs
* Uncomment Redis cache in Startup.cs
* Swap the commented values for IIpPolicyStore and IRateLimitCounterStore in Startup.cs

## To Run
* Open the DotNetCoreGraphQL.sln
* Run Gateway project
* Go to localhost:5000

## Example Queries
```
mutation {
  createUser(user: {name: "username", email:"user@mail.com", password: "asdf"} ) {
    id
  }
}

query {
  first: user(id:"[idOfUserCreated]") {
      id
      name
  }
}
```
