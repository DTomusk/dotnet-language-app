# dotnet-language-app

## Creating migrations 
`dotnet ef migrations add <migration name> --project server/Infrastructure --startup-project server/Api --output-dir Data/Migrations` from project root

## Running migrations 
To update the database with the connection string in app settings, run this from the root: 
`dotnet ef database update --project server\Infrastructure\Infrastructure.csproj --startup-project server\api\Api.csproj --context AppDbContext`