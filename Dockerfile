FROM mcr.microsoft.com/dotnet/sdk:8.0-jammy AS build

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    tzdata \
    libicu-dev \
    && rm -rf /var/lib/apt/lists/* \
    && ln -sf /usr/share/zoneinfo/UTC /etc/localtime \
    && echo "UTC" > /etc/timezone

WORKDIR /app
COPY . .

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore

ARG TARGETARCH
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    if [ "$TARGETARCH" = "amd64" ]; then ARCH="x64"; else ARCH="$TARGETARCH"; fi \
    && dotnet publish -c Release -a $ARCH --use-current-runtime --self-contained false -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-jammy AS final
WORKDIR /app

RUN apt-get update \
    && apt-get install -y --no-install-recommends \
    tzdata \
    libicu70 \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /out .

ENTRYPOINT ["dotnet", "phosAnalyticsApi.dll"]