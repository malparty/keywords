[![Build Status](https://github.com/malparty/keywords/workflows/Tests/badge.svg?branch=main)](https://github.com/malparty/keywords)

# Introduction

> Codding challenge proposed by a nice company ~

# Project Setup

## Prerequisites

- dotnet 5.0.5 sdk

## Docker

- Install Docker [for Mac](https://docs.docker.com/docker-for-mac/install/), [for Windows](https://docs.docker.com/docker-for-windows/install/)

- Setup and boot the Docker containers:

```sh
docker-compose build
```

```sh
docker-compose up
```

## Run the app

- Just run it

  ```sh
  dotnet run
  ```

  > If the DB/Models does not exists in your Postgres container, the app will generate it.

## Development

- Watch available:

  ```sh
  dotnet watch run
  ```

## Tests

> TODO

## Github actions

> TODO
