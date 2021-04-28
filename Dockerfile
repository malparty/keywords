# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:5.0
COPY . /keywordsApp
WORKDIR /keywordsApp
RUN dotnet tool install --global dotnet-ef
RUN ["dotnet", "restore"]
RUN ["dotnet", "build"]
EXPOSE 80/tcp
EXPOSE 5000
ENV ASPNETCORE_URLS http://*:5000
ENV ASPNETCORE_ENVIRONMENT Test

RUN chmod +x ./entrypoint.sh
CMD /bin/bash ./entrypoint.sh