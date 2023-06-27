
# Experimental Reference Application (Library)

Experimental app built for finding scope & style for 3rd semester programming & Systems development @ EASV.

Get a postgres plan at ElephantSQL: https://www.elephantsql.com 
and add the connectionstring from ElephantSQL as environment variable (with name pgconn). Also add a environment variable secret for jwt with name dotnetsecret. (If you want to run locally)

Key decisions:

- Postgres as DB due to its open source nature, rich features, community and abundance of cloud vendors
- Dapper as MicroORM
- Controller-based setup as opposed to Minimal API, since many attributes for ActionFilters (any much documentation & resources in general follow controllers)
- Data validation (of client requests) using Data Annotations
- Layered architecture as opposed to hexagonal/onion
- API tests with Postman (collections can be executed with newman CLI)
- Only explicit middleware added is CORS, Exception handling, and route checking (and of course controller mapping)
- Also a static file server for Angular (only one deployment needed)
- This sample also features authentication, although this is outside the scope of the programming course


