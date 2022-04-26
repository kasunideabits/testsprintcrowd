
FROM mcr.microsoft.com/dotnet/sdk:2.2 AS build-env
WORKDIR /app
#RUN mkdir -p app/fcm_keystore

# Copy csproj and restore as distinct layers
COPY src/*.csproj ./
#COPY config/firebase-token.json app/fcm_keystore/firebase-token.json


RUN dotnet restore

# Copy everything else and build
COPY . ./
# Run Tests
Run cd ./test/ && dotnet restore && dotnet test
# RUN dotnet sonarscanner begin /k:sprintcrowd-backend /d:sonar.host.url=https://sonarqube.z-acceleration.net /d:sonar.login=97ee00357e0bf3f04ee7bbbda21bd6bdbb7b9843
RUN cd src && dotnet publish -c Release -o out
# RUN dotnet sonarscanner end /d:sonar.login=97ee00357e0bf3f04ee7bbbda21bd6bdbb7b9843
# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:2.2
WORKDIR /app
COPY --from=build-env /app/src/out .
x
RUN mkdir -p /app/src/out/fcm_keystore
COPY /config/firebase-token.json /app/src/out/fcm_keystore/firebase-token.json


# RUN echo $(ls -1 /app/src/out/fcm_keystore)


EXPOSE 5002
ENTRYPOINT ["dotnet", "SprintCrowdBackEnd.dll"]
