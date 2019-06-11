FROM chamindu/dotnet-core-sdk-sonarqube:2.2 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
# Run Tests
Run cd ./Tests/ && dotnet restore && dotnet test
RUN dotnet sonarscanner begin /k:sprintcrowd-backend /d:sonar.host.url=https://sonarqube.z-acceleration.net /d:sonar.login=97ee00357e0bf3f04ee7bbbda21bd6bdbb7b9843
RUN dotnet publish -c Release -o out
RUN dotnet sonarscanner end /d:sonar.login=97ee00357e0bf3f04ee7bbbda21bd6bdbb7b9843
# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
EXPOSE 5000
ENTRYPOINT ["dotnet", "SprintCrowdBackEnd.dll"]