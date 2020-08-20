#FROM chamindu/dotnet-core-sdk-sonarqube:2.2 AS build-env
FROM microsoft/dotnet:2.2-sdk AS build-env
WORKDIR /app
#RUN mkdir -p app/fcm_keystore

# Copy csproj and restore as distinct layers
COPY src/*.csproj ./
COPY config/firebase-token.json app/fcm_keystore/firebase-token.json


RUN mkdir -p /app/fcm_keystore && touch /app/fcm_keystore/file1 /app/fcm_keystore/file2


RUN echo $(ls -1 /app/fcm_keystore)

RUN dotnet restore

# Copy everything else and build
COPY . ./
# Run Tests
Run cd ./test/ && dotnet restore && dotnet test
# RUN dotnet sonarscanner begin /k:sprintcrowd-backend /d:sonar.host.url=https://sonarqube.z-acceleration.net /d:sonar.login=97ee00357e0bf3f04ee7bbbda21bd6bdbb7b9843
RUN cd src && dotnet publish -c Release -o out
# RUN dotnet sonarscanner end /d:sonar.login=97ee00357e0bf3f04ee7bbbda21bd6bdbb7b9843
# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/src/out .
EXPOSE 5002
ENTRYPOINT ["dotnet", "SprintCrowdBackEnd.dll"]
