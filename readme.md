# Lentyna Backend API

Knygų vertinimo sistemos REST API, sukurta su ASP.NET Core. Sistema leidžia naudotojams dalintis nuomone apie knygas, rašyti atsiliepimus, kurti asmeninius knygų sąrašus ir dalyvauti forumo diskusijose.

## Technologijos

- **.NET 8.0** - framework
- **ASP.NET Core Web API** - REST API
- **Entity Framework Core 8** - ORM
- **MySQL** (Pomelo.EntityFrameworkCore.MySql) - duomenų bazė
- **JWT Bearer Authentication** - autentifikacija
- **AutoMapper** - objektų mapinimas
- **BCrypt.Net** - slaptažodžių šifravimas
- **Swashbuckle** - Swagger/OpenAPI dokumentacija

## Projekto Struktūra

```
lentynaBackEnd/
├── Controllers/              # API endpoint'ai
│   ├── AuthController.cs
│   ├── KnygosController.cs
│   ├── AutoriaiController.cs
│   ├── KomentaraiController.cs
│   ├── IrasaiController.cs
│   ├── SekimaiController.cs
│   ├── TemosController.cs
│   ├── BalsavimaiController.cs
│   ├── ZanraiController.cs
│   ├── NuotaikosController.cs
│   └── CitatosController.cs
├── Services/                 # Verslo logika
│   ├── Interfaces/
│   └── Implementations/
├── Repositories/             # Duomenų prieiga
│   ├── Interfaces/
│   └── Implementations/
├── Models/
│   ├── Entities/             # Duomenų bazės esybės
│   │   ├── Naudotojas.cs
│   │   ├── Knyga.cs
│   │   ├── Autorius.cs
│   │   ├── Komentaras.cs
│   │   ├── Irasas.cs
│   │   ├── Tema.cs
│   │   ├── Balsavimas.cs
│   │   └── ...
│   └── Enums/
│       ├── Roles.cs
│       └── BookshelfTypes.cs
├── DTOs/                     # Data Transfer Objects
│   ├── Auth/
│   ├── Knygos/
│   ├── Autoriai/
│   ├── Komentarai/
│   ├── Irasai/
│   ├── Temos/
│   ├── Balsavimai/
│   ├── Sekimai/
│   ├── Citatos/
│   └── Common/
├── Data/
│   └── ApplicationDbContext.cs
├── Helpers/
│   ├── JwtHelper.cs
│   └── PasswordHelper.cs
├── Middleware/
│   └── ExceptionHandlingMiddleware.cs
├── Common/
│   ├── Result.cs
│   └── Constants.cs
├── Program.cs
├── appsettings.json
└── appsettings.Development.json
```

## Instaliacija ir Paleidimas

### Reikalavimai

- .NET 8.0 SDK
- MySQL Server

### Žingsniai

1. **Klonuoti projektą**
```bash
git clone <repository-url>
cd lentynaBackEnd
```

2. **Sukonfigūruoti duomenų bazę**

Sukurti `appsettings.json` failą su MySQL prisijungimo duomenimis:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=lentyna;User=root;Password=your_password;"
  },
  "Jwt": {
    "SecretKey": "your-secret-key-min-32-characters",
    "Issuer": "LentynaAPI",
    "Audience": "LentynaClient"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

3. **Pritaikyti migracijas**
```bash
dotnet ef database update
```

4. **Paleisti projektą**
```bash
dotnet run
```

API bus pasiekiama adresu: `https://localhost:7153`

Swagger dokumentacija: `https://localhost:7153/swagger`

## Testinės Paskyros

Paleidus programą, automatiškai sukuriamos testinės paskyros. Visų paskyrų slaptažodis: `Password123!`

- **admin@lentyna.lt** - Administratorius (pilna prieiga)
- **moderatorius@lentyna.lt** - Moderatorius (forumo priežiūra)
- **redaktorius@lentyna.lt** - Redaktorius (knygų/autorių valdymas)
- **skaitytojas@lentyna.lt** - Skaitytojas (paprastas naudotojas)
- **jonas@gmail.com** - Skaitytojas (paprastas naudotojas)

## API Endpoint'ai

### Pagination (Puslapiavimas)

Sąrašų endpoint'ai grąžina puslapiuotus rezultatus:

```json
{
  "items": [...],
  "page": 1,
  "pageSize": 10,
  "totalCount": 45,
  "totalPages": 5,
  "hasNextPage": true,
  "hasPreviousPage": false
}
```

**Query parametrai:**
- `page` - puslapio numeris (default: 1)
- `pageSize` - įrašų skaičius puslapyje (default: 10, max: 100)

---

### Autentifikacija (`/api/auth`)

**POST `/registruotis`** - Registracija
```json
// Request
{ "slapyvardis": "jonas", "el_pastas": "jonas@mail.com", "slaptazodis": "Password123!" }
// Response
{ "token": "jwt...", "naudotojas": { "Id", "slapyvardis", "el_pastas", "role", "sukurimo_data", "profilio_nuotrauka" } }
```

**POST `/prisijungti`** - Prisijungimas
```json
// Request
{ "el_pastas": "jonas@mail.com", "slaptazodis": "Password123!" }
// Response
{ "token": "jwt...", "naudotojas": { "Id", "slapyvardis", "el_pastas", "role", "sukurimo_data", "profilio_nuotrauka" } }
```

**GET `/profilis`** - Gauti profilį (reikia JWT)

**PUT `/profilis`** - Atnaujinti profilį (reikia JWT)

**DELETE `/profilis`** - Ištrinti paskyrą (reikia JWT)

**PUT `/naudotojai/{id}/role`** - Keisti rolę (tik Admin)

---

### Knygos (`/api/knygos`)

**GET `/`** - Knygų sąrašas (su pagination)

Query parametrai:
- `page`, `pageSize` - puslapiavimas
- `paieska` - paieška pagal pavadinimą
- `zanrasId` - filtras pagal žanrą
- `nuotaikaId` - filtras pagal nuotaiką
- `autoriusId` - filtras pagal autorių
- `bestseleris` - tik bestseleriui (true/false)
- `sortBy` - rikiavimas (pavadinimas, leidimo_metai, vertinimas)
- `descending` - mažėjančia tvarka (true/false)

```json
// Response item
{
  "Id", "knygos_pavadinimas", "leidimo_metai", "virselio_nuotrauka",
  "bestseleris", "autorius_vardas", "vidutinis_vertinimas",
  "komentaru_skaicius", "zanrai": ["..."], "nuotaikos": ["..."]
}
```

**GET `/{id}`** - Knygos detalės
```json
{
  "Id", "knygos_pavadinimas", "leidimo_metai", "aprasymas",
  "psl_skaicius", "ISBN", "virselio_nuotrauka", "raisos", "bestseleris",
  "AutoriusId", "Autorius": { "Id", "vardas", "pavarde", "nuotrauka" },
  "vidutinis_vertinimas", "komentaru_skaicius",
  "zanrai": [{ "Id", "pavadinimas" }],
  "nuotaikos": [{ "Id", "pavadinimas" }],
  "di_komentaras": { "Id", "sugeneravimo_data", "tekstas", "modelis" }
}
```

**GET `/{id}/komentarai`** - Knygos komentarai

**POST `/`** - Sukurti knygą (Redaktorius/Admin)

**PUT `/{id}`** - Atnaujinti knygą (Redaktorius/Admin)

**DELETE `/{id}`** - Ištrinti knygą (Redaktorius/Admin)

---

### Autoriai (`/api/autoriai`)

**GET `/`** - Autorių sąrašas (su pagination)
```json
// Response item
{ "Id", "vardas", "pavarde", "nuotrauka", "knygu_skaicius" }
```

**GET `/{id}`** - Autoriaus detalės
```json
{
  "Id", "vardas", "pavarde", "gimimo_metai", "mirties_data",
  "curiculum_vitae", "nuotrauka", "laidybe", "knygu_skaicius",
  "knygos": [KnygaListDto...],
  "citatos": [{ "Id", "citatos_tekstas", "citatos_data", "citatos_saltinis" }],
  "sekejuSkaicius"
}
```

**GET `/{id}/knygos`** - Autoriaus knygos

**GET `/{id}/citatos`** - Autoriaus citatos

**POST `/`** - Sukurti autorių (Redaktorius/Admin)

**PUT `/{id}`** - Atnaujinti autorių (Redaktorius/Admin)

**DELETE `/{id}`** - Ištrinti autorių (Redaktorius/Admin)

---

### Komentarai (`/api/komentarai`)

**GET `/knyga/{knygaId}`** - Knygos komentarai
```json
// Response item
{
  "Id", "komentaro_tekstas", "komentaro_data", "vertinimas",
  "redagavimo_data", "NaudotojasId", "naudotojo_slapyvardis",
  "naudotojo_nuotrauka", "KnygaId", "TemaId"
}
```

**POST `/`** - Sukurti komentarą (reikia JWT)
```json
// Request
{ "komentaro_tekstas": "...", "vertinimas": 5, "KnygaId": "guid" }
```

**PUT `/{id}`** - Atnaujinti komentarą

**DELETE `/{id}`** - Ištrinti komentarą

---

### Įrašai / Bookshelf (`/api/irasai`)

**GET `/`** - Naudotojo knygų sąrašas (reikia JWT)
```json
// Response item
{
  "Id", "tipas": "skaityta|skaitoma|norima_skaityti",
  "sukurimo_data", "redagavimo_data",
  "Knyga": { KnygaListDto }
}
```

**POST `/`** - Pridėti knygą į sąrašą
```json
// Request
{ "KnygaId": "guid", "tipas": "skaityta" }
```

**PUT `/{id}`** - Atnaujinti įrašo statusą

**DELETE `/{id}`** - Pašalinti knygą iš sąrašo

---

### Temos / Forumas (`/api/temos`)

**GET `/`** - Temų sąrašas (su pagination)
```json
// Response item
{
  "Id", "pavadinimas", "sukurimo_data", "prikabinta",
  "autorius_slapyvardis", "komentaru_skaicius"
}
```

**GET `/{id}`** - Temos detalės (su komentarais)

**POST `/`** - Sukurti temą (reikia JWT)

**PUT `/{id}`** - Atnaujinti temą

**DELETE `/{id}`** - Ištrinti temą

---

### Balsavimai (`/api/balsavimai`)

**GET `/current`** - Dabartinis aktyvus balsavimas

**POST `/vote`** - Balsuoti už knygą (reikia JWT)
```json
// Request
{ "KnygaId": "guid" }
```

---

### Sekimai (`/api/sekimai`)

**GET `/check/{autoriusId}`** - Ar naudotojas seka autorių (reikia JWT)
```json
{ "spiking": true }
```

**POST `/`** - Sekti autorių (reikia JWT)
```json
// Request
{ "AutoriusId": "guid" }
```

**DELETE `/{autoriusId}`** - Nebesekti autoriaus (reikia JWT)

---

### Žanrai (`/api/zanrai`)

**GET `/`** - Žanrų sąrašas
```json
[{ "Id", "pavadinimas" }]
```

---

### Nuotaikos (`/api/nuotaikos`)

**GET `/`** - Nuotaikų sąrašas
```json
[{ "Id", "pavadinimas" }]
```

---

### Citatos (`/api/citatos`)

**GET `/autorius/{autoriusId}`** - Autoriaus citatos

**POST `/`** - Sukurti citatą (Redaktorius/Admin)

**DELETE `/{id}`** - Ištrinti citatą (Redaktorius/Admin)

## Autorizacijos Politikos

- **AdminOnly** - tik administratoriai
- **ModeratorOrAdmin** - moderatoriai ir administratoriai
- **EditorOrAdmin** - redaktoriai ir administratoriai
- **Authenticated** - visi prisijungę naudotojai

## Naudotojų Rolės

| Rolė | Aprašymas |
|------|-----------|
| `admin` | Pilna prieiga prie sistemos |
| `moderatorius` | Forumo priežiūra, turinio moderavimas |
| `redaktorius` | Knygų ir autorių CRUD operacijos |
| `skaitytojas` | Komentarai, bookshelf, forumas |

## Duomenų Bazės Struktūra

### Naudotojas
- `Id` (Guid, PK)
- `slapyvardis` (string, max 100, unikalus)
- `el_pastas` (string, max 255, unikalus)
- `slaptazodis` (string, BCrypt hash)
- `role` (enum: naudotojas, redaktorius, moderatorius, admin)
- `sukurimo_data` (DateTime)
- `profilio_nuotrauka` (string?, max 500)

### Autorius
- `Id` (Guid, PK)
- `vardas` (string, max 100)
- `pavarde` (string, max 100)
- `gimimo_metai` (DateTime?)
- `mirties_data` (DateTime?)
- `curiculum_vitae` (string?)
- `nuotrauka` (string?, max 500)
- `laidybe` (string?, max 255)
- `knygu_skaicius` (int)

### Knyga
- `Id` (Guid, PK)
- `knygos_pavadinimas` (string, max 255)
- `leidimo_metai` (DateTime?)
- `aprasymas` (string?)
- `psl_skaicius` (int?)
- `ISBN` (string?, max 20, unikalus)
- `virselio_nuotrauka` (string?, max 500)
- `raisos` (string?, max 100)
- `bestseleris` (bool)
- `AutoriusId` (Guid, FK -> Autorius)

### Zanras
- `Id` (Guid, PK)
- `pavadinimas` (string, max 100, unikalus)

### Nuotaika
- `Id` (Guid, PK)
- `pavadinimas` (string, max 50)

### KnygaZanras (Many-to-Many)
- `KnygaId` (Guid, PK, FK -> Knyga)
- `ZanrasId` (Guid, PK, FK -> Zanras)

### KnygaNuotaika (Many-to-Many)
- `KnygaId` (Guid, PK, FK -> Knyga)
- `NuotaikaId` (Guid, PK, FK -> Nuotaika)

### Komentaras
- `Id` (Guid, PK)
- `komentaro_tekstas` (string)
- `komentaro_data` (DateTime)
- `vertinimas` (int, 1-5)
- `redagavimo_data` (DateTime?)
- `NaudotojasId` (Guid, FK -> Naudotojas)
- `KnygaId` (Guid?, FK -> Knyga)
- `TemaId` (Guid?, FK -> Tema)

### Dirbtinio_intelekto_komentaras
- `Id` (Guid, PK)
- `sugeneravimo_data` (DateTime)
- `tekstas` (string)
- `modelis` (string, max 100)
- `KnygaId` (Guid, FK -> Knyga)

### Citata
- `Id` (Guid, PK)
- `citatos_tekstas` (string)
- `citatos_data` (DateTime?)
- `citatos_saltinis` (string?, max 255)
- `AutoriusId` (Guid, FK -> Autorius)

### Irasas (Bookshelf)
- `Id` (Guid, PK)
- `tipas` (enum: skaityta, norima_skaityti, skaitoma)
- `sukurimo_data` (DateTime)
- `redagavimo_data` (DateTime?)
- `NaudotojasId` (Guid, FK -> Naudotojas)
- `KnygaId` (Guid, FK -> Knyga)
- `KnygosRekomendacijaId` (Guid?, FK -> Knygos_rekomendacija)

### Knygos_rekomendacija
- `Id` (Guid, PK)
- `rekomendacijos_pradzia` (DateTime)
- `rekomendacijos_pabaiga` (DateTime?)
- `NaudotojasId` (Guid, FK -> Naudotojas)

### Autoriaus_sekimas (Many-to-Many)
- `NaudotojasId` (Guid, PK, FK -> Naudotojas)
- `AutoriusId` (Guid, PK, FK -> Autorius)
- `sekimo_pradzia` (DateTime)

### Tema
- `Id` (Guid, PK)
- `pavadinimas` (string, max 255)
- `tekstas` (string)
- `sukurimo_data` (DateTime)
- `redagavimo_data` (DateTime?)
- `istrynimo_data` (DateTime?)
- `prikabinta` (bool)
- `NaudotojasId` (Guid, FK -> Naudotojas)

### Balsavimas
- `Id` (Guid, PK)
- `balsavimo_pradzia` (DateTime)
- `balsavimo_pabaiga` (DateTime)
- `isrinkta_knyga_id` (Guid?, FK -> Knyga)
- `uzbaigtas` (bool)
- `susitikimo_data` (DateTime?)
- `oro_prognoze` (string?)

### Balsas
- `Id` (Guid, PK)
- `pateikimo_data` (DateTime)
- `NaudotojasId` (Guid, FK -> Naudotojas)
- `BalsavimasId` (Guid, FK -> Balsavimas)
- `KnygaId` (Guid, FK -> Knyga)

## CORS

Konfigūruotas frontend prieigai iš:
- `http://localhost:5173`
- `https://localhost:5173`

## Licencija

© 2025 Lentyna - Visos teisės saugomos
