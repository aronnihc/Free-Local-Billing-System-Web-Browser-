dotnet new gitignore
dotnet new sln -n BillingSystem
dotnet new web -n BillingSystem -o src/BillingSystem
dotnet sln add src/BillingSystem/BillingSystem.csproj
dotnet add src/BillingSystem package Microsoft.Data.Sqlite
rm src/BillingSystem/Properties/launchSettings.json
mkdir -p src/BillingSystem/Data src/BillingSystem/wwwroot/js