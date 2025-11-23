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

3. **Sukonfigūruoti aplinkos kintamuosius**

Sukurti `.env` failą (arba nukopijuoti iš `.env.example`):
```
OPENAI_API_KEY=your-openai-api-key-here
OPENAI_MODEL=gpt-4o-mini

# Brevo Email Service (neprivaloma)
BREVO_API_KEY=your-brevo-api-key-here
BREVO_SENDER_EMAIL=noreply@lentyna.lt
BREVO_SENDER_NAME=Lentyna
```

4. **Pritaikyti migracijas**
```bash
dotnet ef database update
```

5. **Paleisti projektą**
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

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| POST | `/registruotis` | ❌ | - | Registracija |
| POST | `/prisijungti` | ❌ | - | Prisijungimas |
| GET | `/profilis` | ✅ | - | Gauti profilį |
| PUT | `/profilis` | ✅ | - | Atnaujinti profilį |
| DELETE | `/profilis` | ✅ | - | Ištrinti paskyrą |
| PUT | `/naudotojai/{id}/role` | ✅ | admin | Keisti rolę |

**POST `/registruotis`** - Registracija
```json
// Request body
{
  "slapyvardis": "jonas",           // privalomas, max 100 simbolių
  "el_pastas": "jonas@mail.com",    // privalomas, validus email
  "slaptazodis": "Password123!"     // privalomas, min 6 simboliai
}

// Response
{
  "token": "jwt...",
  "naudotojas": {
    "Id": "guid",
    "slapyvardis": "jonas",
    "el_pastas": "jonas@mail.com",
    "role": "naudotojas",
    "sukurimo_data": "2025-01-15T10:00:00Z",
    "profilio_nuotrauka": null
  }
}
```

**POST `/prisijungti`** - Prisijungimas
```json
// Request body
{
  "el_pastas": "jonas@mail.com",    // privalomas
  "slaptazodis": "Password123!"     // privalomas
}

// Response - tas pats kaip registracijoje
```

**PUT `/profilis`** - Atnaujinti profilį
```json
// Request body
{
  "slapyvardis": "naujas_vardas",        // neprivalomas, max 100 simbolių
  "profilio_nuotrauka": "https://..."    // neprivalomas, max 500 simbolių
}
```

**PUT `/naudotojai/{id}/role`** - Keisti rolę
```json
// Request body
{
  "role": "redaktorius"   // privalomas, enum: naudotojas, redaktorius, moderatorius, admin
}
```

---

### Knygos (`/api/knygos`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/` | ❌ | - | Knygų sąrašas |
| GET | `/{id}` | ❌ | - | Knygos detalės |
| GET | `/{id}/komentarai` | ❌ | - | Knygos komentarai |
| POST | `/` | ✅ | redaktorius, admin | Sukurti knygą |
| PUT | `/{id}` | ✅ | redaktorius, admin | Atnaujinti knygą |
| DELETE | `/{id}` | ✅ | redaktorius, admin | Ištrinti knygą |
| POST | `/isplestine-paieska` | ❌ | - | AI paieška |

**GET `/`** - Knygų sąrašas

Query parametrai:
- `page`, `pageSize` - puslapiavimas
- `paieska` - paieška pagal pavadinimą
- `zanrasId` - filtras pagal žanrą
- `autoriusId` - filtras pagal autorių
- `bestseleris` - tik bestseleriui (true/false)
- `sortBy` - rikiavimas (pavadinimas, leidimo_metai)
- `descending` - mažėjančia tvarka (true/false)

**POST `/`** - Sukurti knygą
```json
// Request body
{
  "knygos_pavadinimas": "Knyga",    // privalomas, max 255 simboliai
  "leidimo_metai": "2025-01-01",    // neprivalomas, DateTime
  "aprasymas": "Aprašymas...",      // neprivalomas
  "psl_skaicius": 320,              // neprivalomas, int
  "ISBN": "9785417012345",          // neprivalomas, max 20 simbolių
  "virselio_nuotrauka": "https://...", // neprivalomas, max 500 simbolių
  "kalba": "Lietuvių",              // neprivalomas, max 50 simbolių
  "bestseleris": false,             // neprivalomas, default: false
  "AutoriusId": "guid",             // privalomas
  "ZanrasId": "guid"                // privalomas
}
```

**PUT `/{id}`** - Atnaujinti knygą
```json
// Request body - visi laukai neprivalomi
{
  "knygos_pavadinimas": "Naujas pavadinimas",
  "leidimo_metai": "2025-01-01",
  "aprasymas": "Naujas aprašymas...",
  "psl_skaicius": 350,
  "ISBN": "9785417012346",
  "virselio_nuotrauka": "https://...",
  "kalba": "Anglų",
  "bestseleris": true,
  "AutoriusId": "guid",
  "ZanrasId": "guid"
}
```

**POST `/isplestine-paieska`** - Išplėstinė AI paieška
```json
// Request body - bent vienas laukas rekomenduojamas
{
  "ScenarijausAprasymas": "Ieškau knygos apie totalitarinę visuomenę",  // neprivalomas
  "ZanruIds": ["guid1", "guid2"],    // neprivalomas, kelių žanrų filtras
  "NuotaikuIds": ["guid1", "guid2"]  // neprivalomas, kelių nuotaikų filtras
}

// Response
[
  {
    "Id": "guid",
    "knygos_pavadinimas": "1984",
    "leidimo_metai": "1949-06-08",
    "virselio_nuotrauka": "https://...",
    "bestseleris": true,
    "autorius_vardas": "George Orwell",
    "vidutinis_vertinimas": 4.8,
    "komentaru_skaicius": 15,
    "zanras": "Mokslinė fantastika"
  }
]
```

**Filtravimo logika:**
- Jei nurodytas tik 1 filtras - filtruojama tik pagal jį
- Jei nurodyti 2 ar 3 filtrai - knygos turi atitikti VISUS kriterijus (AND logika)
- `ScenarijausAprasymas` - OpenAI analizuoja ir grąžina tinkamiausias knygas
- `ZanruIds` / `NuotaikuIds` - paprastas filtravimas

---

### Autoriai (`/api/autoriai`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/` | ❌ | - | Autorių sąrašas |
| GET | `/{id}` | ❌ | - | Autoriaus detalės |
| GET | `/{id}/knygos` | ❌ | - | Autoriaus knygos |
| GET | `/{id}/citatos` | ❌ | - | Autoriaus citatos |
| POST | `/` | ✅ | redaktorius, admin | Sukurti autorių |
| PUT | `/{id}` | ✅ | redaktorius, admin | Atnaujinti autorių |
| DELETE | `/{id}` | ✅ | redaktorius, admin | Ištrinti autorių |

**POST `/`** - Sukurti autorių
```json
// Request body
{
  "vardas": "Jonas",              // privalomas, max 100 simbolių
  "pavarde": "Biliūnas",          // privalomas, max 100 simbolių
  "gimimo_metai": "1879-04-11",   // neprivalomas, DateTime
  "mirties_data": "1907-12-08",   // neprivalomas, DateTime
  "curiculum_vitae": "Biografija...", // neprivalomas
  "nuotrauka": "https://...",     // neprivalomas, max 500 simbolių
  "tautybe": "Lietuvis"           // neprivalomas, max 100 simbolių
}
```

**PUT `/{id}`** - Atnaujinti autorių
```json
// Request body - visi laukai neprivalomi
{
  "vardas": "Jonas",
  "pavarde": "Biliūnas",
  "gimimo_metai": "1879-04-11",
  "mirties_data": "1907-12-08",
  "curiculum_vitae": "Atnaujinta biografija...",
  "nuotrauka": "https://...",
  "tautybe": "Lietuvis"           // max 100 simbolių
}
```

---

### Komentarai (`/api/komentarai`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/knyga/{knygaId}` | ❌ | - | Knygos komentarai |
| POST | `/` | ✅ | - | Sukurti komentarą |
| PUT | `/{id}` | ✅ | - | Atnaujinti komentarą (tik savą) |
| DELETE | `/{id}` | ✅ | - | Ištrinti komentarą (savą arba admin/moderatorius) |

**POST `/`** - Sukurti komentarą
```json
// Request body
{
  "komentaro_tekstas": "Puiki knyga!",  // privalomas
  "vertinimas": 5,                       // privalomas, 1-5
  "KnygaId": "guid",                     // neprivalomas (jei komentuojama knyga)
  "TemaId": "guid"                       // neprivalomas (jei komentuojama tema)
}
```

**PUT `/{id}`** - Atnaujinti komentarą
```json
// Request body - visi laukai neprivalomi
{
  "komentaro_tekstas": "Atnaujintas komentaras",
  "vertinimas": 4                        // 1-5
}
```

---

### Įrašai / Bookshelf (`/api/irasai`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/` | ✅ | - | Naudotojo knygų sąrašas |
| GET | `/rekomendacijos` | ✅ | - | Gauti rekomendacijas |
| POST | `/` | ✅ | - | Pridėti knygą į sąrašą |
| PUT | `/{id}` | ✅ | - | Atnaujinti įrašo statusą |
| DELETE | `/{id}` | ✅ | - | Pašalinti knygą iš sąrašo |

**GET `/`** - Naudotojo knygų sąrašas

Query parametrai:
- `tipas` - filtras pagal tipą: `skaityta`, `skaitoma`, `norima_skaityti`

**POST `/`** - Pridėti knygą į sąrašą
```json
// Request body
{
  "KnygaId": "guid",                     // privalomas
  "tipas": "skaityta"                    // privalomas, galimos: "skaityta", "skaitoma", "norima_skaityti"
}
```

**PUT `/{id}`** - Atnaujinti įrašo statusą
```json
// Request body
{
  "tipas": "skaitoma"                    // privalomas, galimos: "skaityta", "skaitoma", "norima_skaityti"
}
```

---

### Temos / Forumas (`/api/temos`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/` | ❌ | - | Temų sąrašas |
| GET | `/{id}` | ❌ | - | Temos detalės |
| GET | `/{id}/komentarai` | ❌ | - | Temos komentarai |
| POST | `/` | ✅ | - | Sukurti temą |
| PUT | `/{id}` | ✅ | - | Atnaujinti temą (tik savą) |
| DELETE | `/{id}` | ✅ | - | Ištrinti temą (savą arba moderatorius/admin) |
| PUT | `/{id}/prikabinti` | ✅ | moderatorius, admin | Prikabinti/atkabinti temą |
| POST | `/{id}/komentarai` | ✅ | - | Pridėti komentarą į temą |

**POST `/`** - Sukurti temą
```json
// Request body
{
  "pavadinimas": "Diskusija apie...",    // privalomas, max 255 simboliai
  "tekstas": "Temos turinys..."          // privalomas
}
```

**PUT `/{id}`** - Atnaujinti temą
```json
// Request body - visi laukai neprivalomi
{
  "pavadinimas": "Atnaujintas pavadinimas",
  "tekstas": "Atnaujintas turinys..."
}
```

**POST `/{id}/komentarai`** - Pridėti komentarą į temą
```json
// Request body
{
  "komentaro_tekstas": "Komentaro tekstas...",  // privalomas
  "vertinimas": 5                               // privalomas, 1-5
}
```

---

### Balsavimai (`/api/balsavimai`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/dabartinis` | ❌ | - | Dabartinis aktyvus balsavimas |
| GET | `/{id}` | ❌ | - | Balsavimo detalės |
| POST | `/` | ✅ | admin | Sukurti balsavimą |
| GET | `/{id}/oro-prognoze` | ❌ | - | Gauti orų prognozę |

**POST `/`** - Sukurti balsavimą
```json
// Request body
{
  "balsavimo_pradzia": "2025-01-15T00:00:00Z",  // privalomas
  "balsavimo_pabaiga": "2025-01-22T23:59:59Z",  // privalomas
  "susitikimo_data": "2025-01-25T18:00:00Z",    // neprivalomas
  "nominuotos_knygos": ["guid1", "guid2", "guid3"]  // privalomas, bent 1 knyga
}
```

### Balsai (`/api/balsai`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| POST | `/` | ✅ | - | Balsuoti už knygą |
| DELETE | `/{id}` | ✅ | - | Pašalinti balsą |

**POST `/`** - Balsuoti už knygą
```json
// Request body
{
  "BalsavimasId": "guid",                // privalomas
  "KnygaId": "guid"                      // privalomas
}
```

---

### Sekimai (`/api/sekimai`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/` | ✅ | - | Visi naudotojo sekimai |
| GET | `/tikrinti/{autoriusId}` | ✅ | - | Ar seka autorių |
| POST | `/` | ✅ | - | Sekti autorių |
| DELETE | `/{autoriusId}` | ✅ | - | Nebesekti autoriaus |

**POST `/`** - Sekti autorių
```json
// Request body
{
  "AutoriusId": "guid"                   // privalomas
}

// Response
{
  "isFollowing": true
}
```

---

### Žanrai (`/api/zanrai`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/` | ❌ | - | Žanrų sąrašas |
| POST | `/` | ✅ | redaktorius, admin | Sukurti žanrą |
| DELETE | `/{id}` | ✅ | redaktorius, admin | Ištrinti žanrą |

**POST `/`** - Sukurti žanrą
```json
// Request body
{
  "pavadinimas": "Fantastika"            // privalomas
}
```

---

### Nuotaikos (`/api/nuotaikos`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/` | ❌ | - | Nuotaikų sąrašas |
| POST | `/` | ✅ | redaktorius, admin | Sukurti nuotaiką |
| DELETE | `/{id}` | ✅ | redaktorius, admin | Ištrinti nuotaiką |

**POST `/`** - Sukurti nuotaiką
```json
// Request body
{
  "pavadinimas": "Džiugi"                // privalomas
}
```

---

### Citatos (`/api/citatos`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| GET | `/autorius/{autoriusId}` | ❌ | - | Autoriaus citatos |
| POST | `/` | ✅ | redaktorius, admin | Sukurti citatą |
| DELETE | `/{id}` | ✅ | redaktorius, admin | Ištrinti citatą |

**POST `/`** - Sukurti citatą
```json
// Request body
{
  "citatos_tekstas": "Citatos tekstas...",  // privalomas
  "citatos_data": "1900-01-01",             // neprivalomas
  "citatos_saltinis": "Knyga, psl. 42",     // neprivalomas, max 255 simboliai
  "AutoriusId": "guid"                      // privalomas
}
```

---

### Nuotraukų įkėlimas (`/api/uploads`)

| Metodas | Endpoint | JWT | Rolės | Aprašymas |
|---------|----------|-----|-------|-----------|
| POST | `/image` | ✅ | - | Įkelti bendrą nuotrauką |
| POST | `/knygos/virselis` | ✅ | redaktorius, admin | Įkelti knygos viršelį |
| POST | `/autoriai/nuotrauka` | ✅ | redaktorius, admin | Įkelti autoriaus nuotrauką |
| POST | `/profilis/nuotrauka` | ✅ | - | Įkelti profilio nuotrauką |
| DELETE | `/` | ✅ | redaktorius, admin | Ištrinti nuotrauką |

**POST `/image`** - Įkelti nuotrauką
```
// Request: multipart/form-data
file: <binary image data>

// Response
{
  "url": "https://localhost:7296/images/abc123.jpg"
}
```

**Limtai:**
- Maksimalus failo dydis: 5 MB
- Leidžiami formatai: `.jpg`, `.jpeg`, `.png`, `.gif`, `.webp`

**Naudojimo pavyzdys:**
1. Įkelti nuotrauką: `POST /api/uploads/knygos/virselis`
2. Gauti URL: `https://localhost:7296/images/knygos/abc123.jpg`
3. Naudoti URL kuriant knygą: `POST /api/knygos { "virselio_nuotrauka": "https://..." }`

**DELETE `/?url=...`** - Ištrinti nuotrauką
```
// Query parameter
url: https://localhost:7296/images/abc123.jpg

// Response: 204 No Content
```

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
| `naudotojas` | Komentarai, bookshelf, forumas (default rolė registruojantis) |

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
- `tautybe` (string?, max 100)
- `knygu_skaicius` (int)

### Knyga
- `Id` (Guid, PK)
- `knygos_pavadinimas` (string, max 255)
- `leidimo_metai` (DateTime?)
- `aprasymas` (string?)
- `psl_skaicius` (int?)
- `ISBN` (string?, max 20, unikalus)
- `virselio_nuotrauka` (string?, max 500)
- `kalba` (string?, max 50)
- `bestseleris` (bool)
- `AutoriusId` (Guid, FK -> Autorius)
- `ZanrasId` (Guid, FK -> Zanras)

### Zanras
- `Id` (Guid, PK)
- `pavadinimas` (string, max 100, unikalus)

### Nuotaika
- `Id` (Guid, PK)
- `pavadinimas` (string, max 50)
- `ZanrasId` (Guid, FK -> Zanras)

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

### Balsas
- `Id` (Guid, PK)
- `pateikimo_data` (DateTime)
- `NaudotojasId` (Guid, FK -> Naudotojas)
- `BalsavimasId` (Guid, FK -> Balsavimas)
- `KnygaId` (Guid, FK -> Knyga)

## Išorinės API

### OpenAI (GPT-4o mini)
Naudojama išplėstinei knygų paieškai pagal scenarijaus aprašymą.
- Reikia API rakto `.env` faile

### api.meteo.lt
Naudojama realiai orų prognozei knygų klubo susitikimams KTU miestelyje.
- Nemokama, nereikia API rakto
- Endpoint: `https://api.meteo.lt/v1/places/kaunas/forecasts/long-term`

### Brevo (Email pranešimai)
Naudojama el. pašto pranešimams siųsti autoriaus sekėjams, kai sukuriama nauja knyga.
- Nemokama paskyra (nereikia kortelės): https://www.brevo.com/
- Reikia API rakto `.env` faile (`BREVO_API_KEY`)
- Pranešimai siunčiami automatiškai kuriant naują knygą
- Jei API raktas nekonfigūruotas, el. laiškai tiesiog nesiunčiami (be klaidų)

**Funkcionalumas:**
- Kai redaktorius/admin sukuria naują knygą, visi to autoriaus sekėjai gauna el. pašto pranešimą
- Pranešime nurodomas knygos pavadinimas ir autoriaus vardas
- Sekėjai pridedami per `/api/sekimai` endpoint

## CORS

Konfigūruotas frontend prieigai iš:
- `http://localhost:5173`
- `https://localhost:5173`

## Licencija

© 2025 Lentyna - Visos teisės saugomos
