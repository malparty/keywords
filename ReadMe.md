[![Build Status](https://github.com/malparty/keywords/workflows/test/badge.svg?branch=main)](https://github.com/malparty/keywords)

# Introduction

> Codding challenge proposed by a nice company ~

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

_We use Docker for our PosgreSQL db only._

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

- Run the app

  ```sh
  dotnet run
  ```

## Development

- Watch available:

  ```sh
  cd .\KeywordsApp\
  ```

  ```sh
  dotnet watch run
  ```

## Tests

- unit tests can be run with this command (from root folder)

```sh
dotnet test
```

## Github actions

Unit tests will be run automatically at each push (any branch).
