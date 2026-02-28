# ServiceLogAgent

Service çaðrýlarý için request/response seviyesinde log toplayan bir .NET 8 Web API örneði.

## Teknoloji
- .NET 8 Web API
- EF Core (SQLite)
- Serilog (Console + rolling file)
- xUnit

## Çalýþtýrma
```bash
dotnet restore
dotnet build
dotnet run --project src/ServiceLogAgent.Api
```

Uygulama development ortamýnda açýlýrken migration otomatik uygulanýr ve `servicelog.db` oluþturulur.

## Test
```bash
dotnet test
```

## Endpointler
- `GET /api/ping` -> `{ "status": "ok" }`
- `POST /api/echo` -> request body aynen döner
- `GET /api/error` -> test amaçlý exception
- `GET /api/logs?fromUtc=&toUtc=&statusCode=&contains=&page=&pageSize=` -> filtreli liste
- `GET /api/logs/{id}` -> tek kayýt

## Neler Loglanýr?
- CorrelationId / TraceId
- Süre, method, path, query
- Request/response headers (Authorization, Cookie, Set-Cookie maskeli)
- Request/response body (64 KB üstü `(TRUNCATED)` ile kýrpýlýr)
- Status code, remote IP, user id (claim varsa), application key header
- Hata mesajý ve stack trace

## Loglarý Görme
- Console loglarý: uygulama çýktýsý
- Dosya loglarý: `src/ServiceLogAgent.Api/logs/servicelog-*.log`
- DB loglarý: `ServiceLogs` tablosu (`servicelog.db`)
