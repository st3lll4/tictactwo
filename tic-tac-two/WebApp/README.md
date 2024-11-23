dotnet aspnet-codegenerator razorpage -m Game -dc AppDbContext -udl -outDir Pages/Games --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m User -dc AppDbContext -udl -outDir Pages/Users --referenceScriptLibraries
dotnet aspnet-codegenerator razorpage -m Configuration -dc AppDbContext -udl -outDir Pages/Configs --referenceScriptLibraries

