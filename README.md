# Relay

Relay is a software emulator for the Silicondust HD HomeRun series of network tuners.
It has been designed to work out of the box with Plex DVR, and Channels DVR. Other services
which are HDHR capable will likely work too - however these are untested.

> OK cool. How does this differ to tvhProxy / Antennas / etc?

Relay is a modular full application built using the ASP.NET Core 2.2 Framework, with Entity Framework
(EF) utilised for persistence. Channel lineups are persisted to a SQLite database, with support for
channel favorites just like a real HD HomeRun.

A basic frontend is in place in addition to the regular REST framework. This frontend allows for
browsing and marking channels as favourites, and manually invoking a guide update.

Relay has been designed to be completely modular - with the notion of supporting multiple provider
backends. Right now there is currently support for Tvheadend - however within the project scope
is to extend this to other platforms, such as IPTV playlists.

Finally, this application fully supports the UDP discovery protocol. Several DVR systems such 
as Channels DVR make use of this protocol to discover devices on the network. Without this in
place, these apps cannot make use of the tuner.

## Configuration

Configuration is set using environment variables:

|Parameter|Default|Description|
|---------|-------|-----------|
|relay_address|127.0.0.1|The address used in the BaseURL given out to all REST/UDP calls. Must be a valid address.|
|relay_tunercount|2|The number of tuners to broadcast in discovery calls.|
|relay_tunerdeviceid|1337|The device ID provided to discovery calls. |
|relay_updateintervalseconds|3600|The interval in seconds between guide updates.|
|relay_databasepath|.|The path to where the database should be stored. |

### Provider Configuration

Tvheadend has separate configuration.

*NB: It is recommended to configure a user with streaming credentials and provide these 
to Relay here. Channels DVR can run into difficulty using the Tvheadend provider if credentials
are not set, as the default noauth implementation uses a token based system that is not
compatible with this DVR.*


|Parameter|Default|Description|
|---------|-------|-----------|
|relay_tvheadend__url|http://127.0.0.1:9981|The location of where Tvheadend is running.|
|relay_tvheadend__username|-|Username for login|
|relay_tvheadend__password|-|Password for login|

## Running 

Relay can be run directly on any Windows / Mac / Linux machine, but requires .NET Core 2.2 to be installed prior.

```
cd relay
dotnet run
```

### Docker

Docker is the preferred method of deployment.
Docker images are hosted on the [Cloudsmith](https://cloudsmith.com) platform.

```
docker run --rm -it \
    -e relay_address=192.168.1.2 \
    -e relay_tvheadend__url=http://192.168.1.1:9981 \
    -e relay_tvheadend__username=foobar \
    -e relay_tvheadend_password=wibble \
    -e relay_tunercount=4 \
    -e relay_databasepath=/config \
    -v /some/path/to/config:/config \
    docker.cloudsmith.io/mrtam/relay/relay:latest
```

When using Docker, it is important that the `relay_databasepath` parameter is set to `/config`.
The Dockerfile creates this folder, and is a safe place for mounting locally to persist the
database following container destruction.

## Finally ...

This is very much a WIP project, and as such I welcome pull requests and comments. I created this
project mostly out of need and dissatisfaction with the existing solutions, and welcome contributions
to make Relay the best it can be.

Please fork, update, and PR!!