export ASPNETCORE_ENVIRONMENT=production
cd ./KeywordsApp/
cd ./ClientApp/
npm run build
cd ..
dotnet build --configuration Release
read -p 'press enter to continue...'