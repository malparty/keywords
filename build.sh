export ASPNETCORE_ENVIRONMENT=production
cd ./KeywordsApp/
rm -rf ./wwwroot/dist/
cd ./ClientApp/
npm run build
cd ..
dotnet run --launch-profile test
read -p 'press enter to continue...'