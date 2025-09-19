# install dotnet-ef globally
dotnet tool install --global dotnet-ef

# create initial migration
dotnet ef migrations add InitialCreate

# create initial migration specifying startup and project
dotnet ef migrations add InitialCreate --startup-project DotnetTemplateApp.Api --project DotnetTemplateApp.Persistence

# update database
dotnet ef database update