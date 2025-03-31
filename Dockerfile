FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build

RUN apk add --no-cache \
    icu-libs \
    tzdata \
    libstdc++ \
    && ln -sf /usr/share/zoneinfo/UTC /etc/localtime \
    && echo "UTC" > /etc/timezone

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY . /source
WORKDIR /source/phosAnalyticsApi

ARG TARGETARCH
RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish -a ${TARGETARCH/amd64/x64} --use-current-runtime --self-contained false -o /app

FROM build AS migrator
WORKDIR /source/phosAnalyticsApi
RUN dotnet ef migrations bundle --self-contained -r linux-musl-x64 -o /app/efbundle

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

RUN apk add --no-cache \
    icu-libs \
    tzdata \
    libstdc++ \
    && ln -sf /usr/share/zoneinfo/UTC /etc/localtime \
    && echo "UTC" > /etc/timezone

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LC_ALL=en_US.UTF-8
ENV LANG=en_US.UTF-8

COPY --from=build /app .
COPY --from=migrator /app/efbundle .

ENTRYPOINT ["dotnet", "phosAnalyticsApi.dll"]
