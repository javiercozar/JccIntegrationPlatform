# gRPC Services Configuration - Commons Platform

## Overview

This project implements gRPC services based on protobuf definitions to provide hotel availability search and other related operations.

## Implemented Services

### SearchAvailability
- **Proto File**: `Infraestructure/Contracts/proto/SearchAvailability.proto`
- **Implementation**: `Endpoints/Grpc/SearchAvailabilityService.cs`
- **RPC Method**: `Search(SearchRequest) -> stream SearchResponse`

## Installed NuGet Packages

```xml
<PackageReference Include="Grpc.AspNetCore" Version="2.63.0" />
<PackageReference Include="Grpc.Tools" Version="2.63.0" />
```

### Package Description:
- **Grpc.AspNetCore**: Provides support for gRPC services in ASP.NET Core
- **Grpc.Tools**: Tools to compile `.proto` files and generate C# code

## Configuration in appsettings.json

The gRPC server is configured to run on two endpoints:
- **gRPC (HTTP/2)**: `http://localhost:5000`
- **HTTP (HTTP/1 and HTTP/2)**: `http://localhost:5001`

## How to Use

### 1. Build the Project

```bash
dotnet build
```

The `.proto` files will be automatically compiled generating the necessary classes.

### 2. Run the Server

```bash
dotnet run
```

### 3. Test with grpcurl

First, install `grpcurl`:

```bash
# macOS
brew install grpcurl

# Linux
go install github.com/fullstorydev/grpcurl/cmd/grpcurl@latest
```

Then, list the available services:

```bash
grpcurl -plaintext localhost:5000 list
```

To execute the search:

```bash
grpcurl -plaintext \
  -d '{"credential_id":"demo","hotel_ids":["HOTEL_001"],"stay":{"check_in":"2024-02-20","check_out":"2024-02-22"},"occupancies":[{"ages":[25,30]}],"timeout_milliseconds":5000}' \
  localhost:5000 HotelBooking.Common.SearchAvailability/Search
```

### 4. Create a gRPC Client

Example in C#:

```csharp
using Grpc.Net.Client;
using HotelBooking.Grpc.SearchAvailability;

var channel = GrpcChannel.ForAddress("http://localhost:5000");
var client = new SearchAvailability.SearchAvailabilityClient(channel);

var request = new SearchRequest
{
    CredentialId = "demo",
    HotelIds = { "HOTEL_001" },
    Stay = new HotelBooking.Common.StayPeriod
    {
        CheckIn = "2024-02-20",
        CheckOut = "2024-02-22"
    },
    Occupancies = { new HotelBooking.Common.Occupancy { Ages = { 25, 30 } } },
    TimeoutMilliseconds = 5000
};

using var call = client.Search(request);
await foreach (var response in call.ResponseStream.ReadAllAsync())
{
    Console.WriteLine($"Results: {response}");
}
```

## Directory Structure

```
Jcc.Commons.Platform/
├── Infraestructure/
│   └── Contracts/
│       └── proto/
│           ├── Commons.proto
│           └── SearchAvailability.proto
├── Endpoints/
│   └── Grpc/
│       └── SearchAvailabilityService.cs
├── Configuration/
│   ├── DependencyInjectionConfig.cs
│   └── GrpcServiceExtensions.cs
├── Program.cs
├── appsettings.json
└── Jcc.Commons.Platform.csproj
```

## Future Extensions

1. **Add more gRPC services**: Create new `.proto` files and their corresponding implementations
2. **Authentication**: Implement gRPC authentication with certificates or JWT tokens
3. **Interceptors**: Add interceptors for logging, error handling, etc.
4. **Reflection**: Enable gRPC Server Reflection for better tooling

## Troubleshooting

### Error: "Proto file not found"
- Verify that the `.proto` files are in `Infraestructure/Contracts/proto/`
- Ensure the `.csproj` has the correct `<Protobuf>` configuration

### Error: "Service not found"
- Verify that the service is registered in `GrpcServiceExtensions.cs`
- Ensure that `MapGrpcServices()` is called in `Program.cs`

### Port already in use
- Change the ports in `appsettings.json` or use an environment variable
