#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8002
EXPOSE 8003

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Registration/RegistrationWebApi/RegistrationWebApi.csproj", "RegistrationWebApi/"]
COPY ["Registration/Rabbit/Rabbit.csproj", "Rabbit/"]
COPY ["Registration/Messages/Messages.csproj", "Messages/"]
COPY ["Registration/Helps/Helps.csproj", "Helps/"]
COPY ["Registration/EntityFramework/EntityFramework.csproj", "EntityFramework/"]
COPY ["Registration/Entities.Registration/Entities.Registration.csproj", "Entities.Registration/"]
COPY ["Registration/Entities.Base/Entities.Base.csproj", "Entities.Base/"]
COPY ["Registration/Dto.Registration/Dto.Registration.csproj", "Dto.Registration/"]
COPY ["Registration/RabbitMQ/RabbitMQ.csproj", "RabbitMQ/"]
COPY ["Registration/Options/Options.csproj", "Options/"]
COPY ["Registration/Module/Modules.csproj", "Module/"]
COPY ["Registration/Extensions/Extensions.csproj", "Extensions/"]
COPY ["Registration/Workers/Workers.csproj", "Workers/"]
COPY ["Registration/Mapping.Registration/Mapping.Registration.csproj", "Mapping.Registration/"]
COPY ["Registration/Commands/Commands.csproj", "Commands/"]
RUN dotnet restore "RegistrationWebApi/RegistrationWebApi.csproj"
COPY . .
WORKDIR "/src/Registration/RegistrationWebApi"
RUN dotnet build "RegistrationWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RegistrationWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RegistrationWebApi.dll"]