# ServiceLogAgent

Service cagrilari icin request/response seviyesinde log toplayan bir .NET 8 Web API ornegi.

## Teknoloji
- .NET 8 Web API
- EF Core (SQLite)
- Serilog (Console + rolling file)
- xUnit

## Mimari Notlar
- Controller katmani business logic icermez, sadece servis cagirir.
- Is kurallari `Application` katmanindaki servislerde tutulur.
- Tum endpoint donusleri `GenericResponse<T>` formatindadir.
- Veri erisimi icin `IGenericRepository<T>` + `GenericRepository<T>` kullanilir.

## Calistirma
```bash
dotnet restore
dotnet build
dotnet run --project src/ServiceLogAgent.Api
```

Uygulama development ortaminda acilirken migration otomatik uygulanir ve `servicelog.db` olusturulur.

## Test
```bash
dotnet test
```

## Endpointler
- `GET /api/ping`
- `POST /api/echo`
- `GET /api/error`
- `GET /api/logs?fromUtc=&toUtc=&statusCode=&contains=&page=&pageSize=`
- `GET /api/logs/{id}`

## Neler Loglanir?
- CorrelationId / TraceId
- Sure, method, path, query
- Request/response headers (Authorization, Cookie, Set-Cookie maskeli)
- Request/response body (64 KB ustu `(TRUNCATED)` ile kirpilir)
- Status code, remote IP, user id (claim varsa), application key header
- Hata mesaji ve stack trace

## Loglari Gorme
- Console loglari: uygulama ciktisi
- Dosya loglari: `src/ServiceLogAgent.Api/logs/servicelog-*.log`
- DB loglari: `ServiceLogs` tablosu (`servicelog.db`)
