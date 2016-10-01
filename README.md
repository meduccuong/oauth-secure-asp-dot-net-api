# oauth-secure-asp-dot-net-api
Using OAuth to Secure Your ASP.NET API

## OAuth
https://www.scottbrady91.com/Identity-Server/Getting-Started-with-IdentityServer-4
1. Install Identity Server
```
nuget: IdentityServer4
```
2. Update ConfigureServices 
```
services.AddIdentityServer()
    .AddInMemoryStores()
    .AddInMemoryClients(new List<Client>())
    .AddInMemoryScopes(new List<Scope>())
    .AddInMemoryUsers(new List<InMemoryUser>())
    .SetTemporarySigningCredential();
```
3. Update Configure 
```
app.UseIdentityServer();
```
4. Check out the OpenID Connect Discovery Document 
```
    /.well-known/openid-configuration
```
5. OAuth Functionality
```
    POST /connect/token
    Headers:
    Content-Type: application/x-www-form-urlencoded
    Body:
    grant_type=client_credentials&scope=customAPI&client_id=oauthClient&client_secret=superSecretPassword
```

