FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /app

COPY . .

RUN dotnet restore

ARG TARGETARCH
RUN if [ "$TARGETARCH" = "amd64" ]; then ARCH="x64"; else ARCH="$TARGETARCH"; fi && \
    dotnet publish -c Release -a $ARCH --use-current-runtime --self-contained false -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /out .

RUN apt-get update && \
    apt-get install -y libgomp1 libnuma1

ENTRYPOINT ["dotnet", "phosAnalyticsApi.dll"]