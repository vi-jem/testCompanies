# myrestful
This is the sample API you've requested.

## information
I developed this on Ubuntu, not sure whether that was a smart decision, wanted to test the process.

For simplicity I choose **PostgreSQL** database and **Entity Framework**, even though NHibernate and Fluent were mentioned. I'm not familiar with those ORMs, and there were already too many moving parts (VSCode, extensions, and .NET core 2.2).

I haven't included SQL script, but this should be covered by EF Migrations added to the project.

I'll gladly include NHibernate repository as well at some point - if sending updated code in few days is agreeable with you.

## enpoints
POST: /company/create

POST: /company/search

PUT: /company/update/{id}

DELETE: /company/delete/{id}

## endpoints not covered by specification
I've decided to leave them in project to help with manual testing

GET /company/{id}