# Blog Application Api - "rimdasilva"


- Migration commands:
	- Open Developer PowerShell
	- Enter `cd src\Services\Post`
	- Add Migration: `dotnet ef migrations add "InitDatabase" -p Post.Infrastructure --startup-project Post.Api -o Migrations`
	- Remove Migration: `dotnet ef migrations remove --project Post.Infrastructure --startup-project Post.Api`
	- Update Database: `dotnet ef database update -p Post.Infrastructure --startup-project Post.Api`