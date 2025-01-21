#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5007
ENV ASPNETCORE_URLS=http://+:5007

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BlogService.csproj", "./"]
RUN dotnet restore "BlogService.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "BlogService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BlogService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BlogService.dll"]