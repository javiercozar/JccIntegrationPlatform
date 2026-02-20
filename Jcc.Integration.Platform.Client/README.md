# Jcc.Integration.Platform.Client

Cliente de consola para probar el streaming gRPC del servicio `SearchAvailability`.

## Ejecutar

```bash
dotnet run --project Jcc.Integration.Platform.Client -- https://localhost:5001 --allow-untrusted
```

Notas:
- Usa `--allow-untrusted` si el servidor usa el certificado de desarrollo.
- Si el servidor expone HTTP sin TLS, usa `http://localhost:5001` y asegúrate de que soporte HTTP/2 sin TLS.

