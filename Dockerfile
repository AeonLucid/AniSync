FROM lsiobase/ubuntu:xenial

# Metadata.
ARG BUILD_DATE
ARG VERSION
LABEL build_version="AniSync v${VERSION} build on ${BUILD_DATE}"
LABEL maintainer="AeonLucid"

# Environment.
ENV HOME="/config"
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://*:5322

# App files.
COPY /_build /opt/anisync

# Image.
RUN \
    echo "**** Installing dependencies ****" && \
    apt-get update && \
    apt-get install -y \
        libicu55 \
        libunwind8 && \
    echo "**** Installing AniSync ****" && \
    chmod +x /opt/anisync/AniSync && \
    echo "**** Cleaning up ****" && \
    rm -rf \
        /tmp/* \
        /var/lib/apt/lists/* \
        /var/tmp/*

# Local files.
COPY /docker /

# Ports and volumes.
EXPOSE 5322
VOLUME /config