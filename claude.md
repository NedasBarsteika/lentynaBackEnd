# Lentyna - Knyg? Vertinimo Sistema

## Sistemos Aprašymas

Kuriama sistema - knyg? vertinimui ir nuomoni? dalinimuisi skirta svetain?. Svetain? skirta asmenims, norintiems dalintis nuomone apie knygas, rašyti atsiliepimus, ir atrasti bendramin?ius.

## Naudotoj? Tipai

Sistemoje egzistuoja keturi naudotoj? tipai su skirtingomis prieigos teis?mis:

### 1. Administratoriai
- Prieiga prie vis? sistemos funkcij?
- Gali tvarkyti visus naudotojus ir j? roles
- Gali trinti bet kok? turin?

### 2. Moderatoriai
- Priži?ri forumo tvark?
- Gali trinti netinkamas temas ir komentarus
- Priži?ri išreikšt? nuomoni? etiškum?

### 3. Redaktoriai
- Kuria, redaguoja, trina knyg? ?rašus
- Tvarko autori? profilius ir biografijas
- Priži?ri informacijos apie knygas ir autorius tikslum?

### 4. Skaitytojai (registruoti naudotojai)
- Rašo atsiliepimus apie knygas
- Kuria savo knyg? s?rašus (bookshelf)
- Dalyvauja forume
- Gali pažym?ti m?gstamus autorius
- Gauna rekomendacijas

### Neregistruoti lankytojai
- Gali perži?r?ti viešai prieinam? informacij?
- Negali rašyti atsiliepim? ar dalyvauti forume

## Registracija ir Autentifikacija

- Registracijai reikalingas el. paštas ir slapyvardis
- Naujiems naudotojams automatiškai priskiriamas "Skaitytojo" statusas
- Statusas gali b?ti pakeistas administratori? ? Moderatori?, Redaktori? ar Administratori?
- Kiekvienas naudotojas gali tvarkyti ir ištrinti savo profil?

## Posistem?s

### 1. Knyg? Valdymas

**Funkcionalumas:**
- Knyg? ?raš? k?rimas (redaktoriai)
- Informacija: pavadinimas, aprašymas, autorius, leidimo metai, žanrai
- Redagavimas, perži?ra, trynimas
- Viešas perži?ra visiems lankytojams

**Išpl?stin? Paieška:**
- Paieška pagal scenarijaus aprašym?
- Filtravimas pagal žanrus
- Paieška pagal knygos nuotaik?:
  - Li?dna (gali pravirkdyti, nuli?dinti)
  - Džiugi (pakelia nuotaik?, sukelia geras emocijas)
  - Neutrali

**Teis?s:**
- Redaktoriai: CRUD operacijos
- Skaitytojai ir lankytojai: tik perži?ra

### 2. Autori? Valdymas

**Autoriaus profilis apima:**
- Biografij?
- Parašyt? knyg? s?raš?
- Populiariausias k?rini? citatas
- Autoriaus nuotrauk?/avatar?

**Funkcionalumas:**
- Profili? k?rimas ir redagavimas (redaktoriai)
- Viešas perži?ra visiems
- M?gstam? autori? pažym?jimas (registruoti naudotojai)
- Pranešimai el. paštu apie naujus autoriaus k?rinius

**Teis?s:**
- Redaktoriai ir administratoriai: CRUD operacijos
- Visi naudotojai: perži?ra
- Registruoti naudotojai: gali pažym?ti m?gstamus autorius

### 3. Knyg? Atsiliepim? Valdymas

**Funkcionalumas:**
- Atsiliepim? rašymas po kiekviena knyga (registruoti naudotojai)
- Atsiliepim? redagavimas ir trynimas (autorius ir administratoriai)
- Viešas atsiliepim? perži?ra

**Dirbtinio Intelekto Atsiliepimas:**
- Automatiškai generuojamas apibendrintas atsiliepimas
- Atspindi daugumos pateikt? nuomon?
- Padeda naudotojams grei?iau apsispr?sti

**Teis?s:**
- Registruoti naudotojai: kurti, redaguoti, trinti savo atsiliepimus
- Administratoriai: gali trinti bet kokius atsiliepimus
- Visi: perži?ra

### 4. Knyg? S?rašo (Bookshelf) Valdymas

**Knyg? kategorijos:**
- Perskaitytos knygos
- Skaitomos knygos
- Norimos perskaityti knygos

**Funkcionalumas:**
- Asmeninio knyg? s?rašo k?rimas
- Knyg? prid?jimas ? skirtingas kategorijas
- S?rašo redagavimas ir ?raš? trynimas
- Knyg? rekomendacijos pagal:
  - Perskaityt? knyg? žanrus
  - M?gstamus autorius
  - Siužeto pob?d?

**Teis?s:**
- Tik registruoti naudotojai
- Kiekvienas naudotojas mato tik savo s?raš?

### 5. Nuomoni? Forumo Valdymas

**Funkcionalumas:**
- Nauj? tem? k?rimas (registruoti naudotojai)
- Tem? redagavimas/trynimas (tem? autoriai)
- Komentar? rašymas temose
- Moderavimas (moderatoriai gali trinti bet kokias temas)

**Knyg? Klubo Tema (Nuolatin?):**
- Visada prikabinta viršuje
- Kas savait? skelbiamos 5 populiariausios knygos
- Naudotoj? balsavimas už norim? aptarti knyg?
- Gyvo susitikimo data
- Oro s?lyg? prognoz? KTU miesteliui susitikimo datai
- Padeda nuspr?sti ar susitikti lauke ar viduje

**Teis?s:**
- Registruoti naudotojai: kurti temas, rašyti komentarus
- Moderatoriai: gali trinti bet kokias temas ir komentarus
- Administratoriai: visos teis?s
- Lankytojai: tik perži?ra

## Navigacija

Pagrindiniame meniu:
- **Knygos** - knyg? katalogas ir paieška
- **Autoriai** - autori? s?rašas ir profiliai
- **Knyg? s?rašas** - naudotojo asmeninis s?rašas
- **Forumas** - diskusijos ir knyg? klubas

Papildoma navigacija:
- Atsiliepimus galima pasiekti per knygos detali? puslap?
- Autoriaus knygas galima pasiekti per autoriaus profil?
- Knyg? galima prid?ti ? s?raš? iš knygos detali? puslapio

## Technin? Informacija

**Frontend:**
- React 19 su TypeScript
- Vite build tool
- Tailwind CSS stiliai
- React Router navigacijai
- Framer Motion animacijoms
- Axios HTTP užklausoms

**Autentifikacija:**
- JWT tokenai
- localStorage token saugojimas
- Roli? sistema

**Backend API:**
- Base URL: https://localhost:7296/

## Duomen? Strukt?ros

### Knyga
- ID
- Pavadinimas
- Aprašymas
- Autorius (ID)
- Leidimo metai
- Žanrai (array)
- Nuotaika (enum: džiugi/li?dna/neutrali)
- Viršelio nuotrauka

### Autorius
- ID
- Vardas
- Pavard?
- Biografija
- Nuotrauka
- Knyg? s?rašas

### Atsiliepimas
- ID
- Knygos ID
- Naudotojo ID
- Tekstas
- ?vertinimas (1-5 žvaigždut?s)
- Data
- DI generuotas (boolean)

### Knyg? S?rašo ?rašas
- ID
- Naudotojo ID
- Knygos ID
- Statusas (enum: perskaitytos/skaitomos/norimos)
- Prid?jimo data

### Forumo Tema
- ID
- Pavadinimas
- Aprašymas
- Autorius (naudotojo ID)
- Suk?rimo data
- Prikabinta (boolean)
- Komentar? skai?ius

### Komentaras
- ID
- Temos ID
- Autorius (naudotojo ID)
- Tekstas
- Data

### Knyg? Klubo Balsavimas
- ID
- Savait?s ID
- Knygos ID (5 knygos)
- Bals? skai?ius kiekvienai knygai
- Susitikimo data
- Oro prognoz?
