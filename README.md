# DGHA-Backend
This is the backend API for the DGHA project which was the final and largest project for my Software Development Diploma.

The other sections of the project are:
- [Mobile App](https://github.com/leechuyem/dgha)
- [Admin Application](https://github.com/jamtowers/DGHA-Admin)

This project was made with the help of these fine folks:
- [Joseph Khai](https://github.com/josephkhaipi)
- [Lee Chu Yem](https://github.com/leechuyem)
- [James Towers](https://github.com/jamtowers) (Hey that's me!)
- [Matthew Thorne](https://github.com/Thornie)

## Special Requirements
If you are planning on deploying this backend there are a few thing you need to do to get it running properly:
1. The ModelLibrary contains the `ApplicationDbContext`, this runs off Entity Framework, details on how to use/deploy/update the database can be found [here](https://docs.microsoft.com/en-gb/ef/ef6/)
2. The connection string is storied in the `ApplicatoinDbContext`, be sure to update this to your databases connection string
3. If you are deploying to a database from starch be sure to add the `Administrator` role and an administrator account to the database, there is currently no automatic way to do this implemented here so you'll need to do it manually.
4. The Identity server and API both run on ASP.NET Core 3.0, details on how to deploy them can be found [here](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/?view=aspnetcore-3.0)

Also note that the system has been built to expect in a development environment it is running locally.

## Identity Server
Identity server is the authorization server for the project to get it working you need to make sure:
1. Update the `Config.cs` file so the redirect Urls for the `AdminPortal` client match the Admin Application Urls.
2. In a production environment update the clients in `Config.cs` so the secrets aren’t just 'secret', ensure you update the Admin and Flutter Clients to match.
3. Update the `Startup.cs` file so the `apiUrl` and `adminUrl` variables in the `ConfigureServices` function match the API and Admin Portal urls, this is to set the CORS policies.
4. In a production environment replace `builder.AddDeveloperSigningCredential()` with actual key material, for more information look [here](http://docs.identityserver.io/en/latest/topics/startup.html#key-material)

If you need more help you can find the Identity Server Documentation [here](http://docs.identityserver.io/en/latest/)

## API
To Ensure the API endpoints and authentication are working be sure to:
1. Update `Startup.cs` file so the `adminUrl` variable in the `ConfigureServices` function is the same as the deployed Admin Applicaion and `baseUrl` is the same as the identity server url
2. Update `Startup.cs` file so the `Authority` option for the `AddAuthentication` function is the same as the Admin Application url

At the base url of the API you can find the Swagger API Documentation.

~~To see a deployed example of this go [here](https://dgha-api.azurewebsites.net/index.html)~~ <- That doesn't exist anymore, sorry.
