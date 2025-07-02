# Imagen base oficial de .NET para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar los archivos del proyecto
COPY . ./

# Restaurar paquetes y compilar
RUN dotnet restore
RUN dotnet publish -c Release -o out

# Imagen base para runtime (más liviana)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa de build
COPY --from=build /app/out .

# Exponer el puerto por defecto
EXPOSE 80

# Comando de inicio
ENTRYPOINT ["dotnet", "backend.dll"]
