
ARG BUILDCONFIG=RELEASE

FROM mcr.microsoft.com/dotnet/sdk:6.0.110 AS build
WORKDIR /build

RUN curl -sL https://deb.nodesource.com/setup_10.x |  bash -
RUN apt-get install -y nodejs

COPY ./*.csproj .
RUN dotnet restore

COPY . .
WORKDIR /build
RUN dotnet publish -c $BUILDCONFIG -o publish --no-cache

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /build/publish ./
ENTRYPOINT ["dotnet", "LinuxCourses.dll"]