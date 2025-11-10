# Linkek

- https://github.com/breadking99/Webshop
- https://teambreadking.postman.co/workspace/My-Workspace~72af1235-dd9e-49da-80d3-7fffd7ee43d8/collection/19214792-857e55e2-40bf-4496-8acf-711148a0c511?action=share&creator=19214792

# Használat

- SQLite-ot használtam (nincs szükség extra configurációra)
- Api & Blazor futhatható egyszerre Visual Studio seqítségével (Solution -> Properties -> Multiple Startup project)
- Angular-nál a Webshop\Web.Angular könyvtárban az alábbi parancsokat futattom
  + Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope Process
  + npm.cmd run build
  + ng build
  + ng serve
  + o
- Api-hoz van Swagger

# Projectek
- Shared: Domain osztályok és interfészek
- Api: controllerek és endpontok (DataBaseContex)
- Blazor és Angular Web App: Api servic-ek, UI Componensek és Oldalak
- Blazor-hoz jobban értek így először abban programoztam le