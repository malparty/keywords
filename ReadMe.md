[![Build Status](https://github.com/malparty/keywords/workflows/Tests/badge.svg?branch=main)](https://github.com/malparty/keywords)

# Introduction

Codding challenge proposed by a nice company ~

# Project Setup

## Prerequisites

- Install [dotnet 5.0.5 sdk](https://dotnet.microsoft.com/download/dotnet/5.0)

- Set up Environment variables (for email sending)

```javascript
 "SendGridUser": "XXX",
 "SendGridKey": "XXX"
```

> Use your own SendGrid API keys

## Docker

_Docker is only used for PosgreSQL db._

- Install Docker [for Mac](https://docs.docker.com/docker-for-mac/install/), [for Windows](https://docs.docker.com/docker-for-windows/install/)

- Setup the db container:

  ```sh
  docker-compose up
  ```

## Run the app

- Enter the project folder

  ```sh
  cd .\KeywordsApp\
  ```

- Generate/Update the database

  _You need dotnet-ef tool globally setup:_

  ```sh
  dotnet tool install --global dotnet-ef
  ```

  _Use this tool to update the database:_

  ```sh
  dotnet-ef database update
  ```

## Run the app

### Manually:

- Build css/js

  ```sh
  cd ./KeywordsApp/ClientApp
  ```

  ```sh
  webpack start
  ```

- Run the dotnet app (back in the KeywordsApp project folder)

  ```sh
  cd ../
  ```

  ```sh
  dotnet watch run
  ```

### Via script:

- From the repository root folder, run dev.sh

  ```sh
  .\dev.sh
  ```

## Build

> **Warning:** Be sure to change DB username/passwords and include your connection string in a safe place (ENV variables or Secret API) before deploying this app!

- Build minified js/css:

  ```sh
  cd ./KeywordsApp/ClientApp
  ```

  ```sh
  webpack build
  ```

- Build dotnet project

  ```sh
  cd ../
  ```

  ```sh
  dotnet build --configuration Release
  ```

## Tests

- Tests can be run with this command (from root folder)

```sh
dotnet test
```

## Github actions

Tests will be run automatically at each push from any branches.
