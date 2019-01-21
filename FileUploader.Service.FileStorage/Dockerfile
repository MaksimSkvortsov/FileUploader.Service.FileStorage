FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["FileUploader.Service.FileStorage/FileUploader.Service.FileStorage.csproj", "FileUploader.Service.FileStorage/"]
RUN dotnet restore "FileUploader.Service.FileStorage/FileUploader.Service.FileStorage.csproj"
COPY . .
WORKDIR "/src/FileUploader.Service.FileStorage"
RUN dotnet build "FileUploader.Service.FileStorage.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "FileUploader.Service.FileStorage.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "FileUploader.Service.FileStorage.dll"]