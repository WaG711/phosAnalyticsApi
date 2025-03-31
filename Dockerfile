FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

RUN apk add --no-cache \
    icu-libs \
    tzdata \
    libstdc++ \
    && ln -sf /usr/share/zoneinfo/UTC /etc/localtime \
    && echo "UTC" > /etc/timezone

WORKDIR /app
COPY . .

ARG TARGETARCH
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -c Release -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

RUN apk add --no-cache \
    icu-libs \
    tzdata \
    libstdc++

COPY --from=build /out .

ENTRYPOINT ["dotnet", "phosAnalyticsApi.dll"]