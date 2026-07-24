# Migrations 
`dotnet ef migrations add <migration_name> --context AppDbContext --project Infrastructure --startup-project Api --output-dir Data/Migrations`

`dotnet ef database update --context AppDbContext --project Infrastructure --startup-project Api`

# Python 
`uvicorn app.main:app --port 8000`