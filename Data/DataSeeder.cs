using lentynaBackEnd.Helpers;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace lentynaBackEnd.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Seed Naudotojai (Users) with real BCrypt passwords
            if (!await context.Naudotojai.AnyAsync())
            {
                var password = PasswordHelper.HashPassword("Password123!");

                var naudotojai = new List<Naudotojas>
                {
                    new Naudotojas
                    {
                        Id = Guid.Parse("a0000000-0000-0000-0000-000000000001"),
                        slapyvardis = "admin",
                        el_pastas = "admin@lentyna.lt",
                        slaptazodis = password,
                        role = Roles.admin,
                        sukurimo_data = DateTime.UtcNow
                    },
                    new Naudotojas
                    {
                        Id = Guid.Parse("a0000000-0000-0000-0000-000000000002"),
                        slapyvardis = "moderatorius",
                        el_pastas = "moderatorius@lentyna.lt",
                        slaptazodis = password,
                        role = Roles.moderatorius,
                        sukurimo_data = DateTime.UtcNow
                    },
                    new Naudotojas
                    {
                        Id = Guid.Parse("a0000000-0000-0000-0000-000000000003"),
                        slapyvardis = "redaktorius",
                        el_pastas = "redaktorius@lentyna.lt",
                        slaptazodis = password,
                        role = Roles.redaktorius,
                        sukurimo_data = DateTime.UtcNow
                    },
                    new Naudotojas
                    {
                        Id = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        slapyvardis = "skaitytojas",
                        el_pastas = "skaitytojas@lentyna.lt",
                        slaptazodis = password,
                        role = Roles.naudotojas,
                        sukurimo_data = DateTime.UtcNow
                    },
                    new Naudotojas
                    {
                        Id = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        slapyvardis = "jonas123",
                        el_pastas = "jonas@gmail.com",
                        slaptazodis = password,
                        role = Roles.naudotojas,
                        sukurimo_data = DateTime.UtcNow
                    }
                };

                await context.Naudotojai.AddRangeAsync(naudotojai);
                await context.SaveChangesAsync();
            }

            // Seed Citatos (Quotes)
            if (!await context.Citatos.AnyAsync())
            {
                var citatos = new List<Citata>
                {
                    new Citata
                    {
                        Id = Guid.NewGuid(),
                        citatos_tekstas = "Žmogus be knygų - lyg paukštis be sparnų.",
                        citatos_saltinis = "Liūdna pasaka",
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000001")
                    },
                    new Citata
                    {
                        Id = Guid.NewGuid(),
                        citatos_tekstas = "Kas neskaito knygų, tas neturi ateities.",
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000001")
                    },
                    new Citata
                    {
                        Id = Guid.NewGuid(),
                        citatos_tekstas = "Tėvynė yra ten, kur gera, o gera ten, kur tėvynė.",
                        citatos_saltinis = "Skirgaila",
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000002")
                    },
                    new Citata
                    {
                        Id = Guid.NewGuid(),
                        citatos_tekstas = "Dievai miške gyvena, bet žmonės juos ten randa.",
                        citatos_saltinis = "Dievų miškas",
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000003")
                    },
                    new Citata
                    {
                        Id = Guid.NewGuid(),
                        citatos_tekstas = "Kas kontroliuoja praeitį, tas kontroliuoja ateitį. Kas kontroliuoja dabartį, tas kontroliuoja praeitį.",
                        citatos_saltinis = "1984",
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000005")
                    },
                    new Citata
                    {
                        Id = Guid.NewGuid(),
                        citatos_tekstas = "Visi gyvūnai yra lygūs, bet kai kurie gyvūnai yra lygesni už kitus.",
                        citatos_saltinis = "Gyvulių ūkis",
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000005")
                    },
                    new Citata
                    {
                        Id = Guid.NewGuid(),
                        citatos_tekstas = "Karas yra taika. Laisvė yra vergija. Nežinojimas yra jėga.",
                        citatos_saltinis = "1984",
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000005")
                    }
                };

                await context.Citatos.AddRangeAsync(citatos);
                await context.SaveChangesAsync();
            }

            // Seed Temos (Forum Topics)
            if (!await context.Temos.AnyAsync())
            {
                var temos = new List<Tema>
                {
                    new Tema
                    {
                        Id = Guid.Parse("d0000000-0000-0000-0000-000000000001"),
                        pavadinimas = "Knygų klubo savaitinė tema",
                        tekstas = "Sveiki! Čia yra mūsų savaitinė knygų klubo diskusija. Šią savaitę aptariame geriausias 2024 metų knygas. Dalinkitės savo rekomendacijomis!",
                        sukurimo_data = DateTime.UtcNow.AddDays(-7),
                        prikabinta = true,
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000001")
                    },
                    new Tema
                    {
                        Id = Guid.Parse("d0000000-0000-0000-0000-000000000002"),
                        pavadinimas = "Lietuvių klasika - ką verta perskaityti?",
                        tekstas = "Sveiki, norėčiau išgirsti jūsų nuomones apie lietuvių klasikinę literatūrą. Kokias knygas rekomenduotumėte pradedantiesiems?",
                        sukurimo_data = DateTime.UtcNow.AddDays(-5),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004")
                    },
                    new Tema
                    {
                        Id = Guid.Parse("d0000000-0000-0000-0000-000000000003"),
                        pavadinimas = "George Orwell ir šiuolaikinis pasaulis",
                        tekstas = "Ar pastebėjote, kaip Orwell'o \"1984\" temos vis dar aktualios šiandien? Aptarkime paraleles tarp knygos ir dabartinės visuomenės.",
                        sukurimo_data = DateTime.UtcNow.AddDays(-3),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005")
                    },
                    new Tema
                    {
                        Id = Guid.Parse("d0000000-0000-0000-0000-000000000004"),
                        pavadinimas = "Silva Rerum serija - diskusija",
                        tekstas = "Ką manote apie K. Sabaliauskaitės Silva Rerum seriją? Ar verta skaityti visas knygas iš eilės?",
                        sukurimo_data = DateTime.UtcNow.AddDays(-1),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004")
                    }
                };

                await context.Temos.AddRangeAsync(temos);
                await context.SaveChangesAsync();
            }

            // Seed Komentarai (Comments on books)
            if (!await context.Komentarai.AnyAsync())
            {
                var komentarai = new List<Komentaras>
                {
                    // Komentarai knygoms
                    new Komentaras
                    {
                        Id = Guid.NewGuid(),
                        komentaro_tekstas = "Nuostabi knyga! Labai jaudinanti istorija, kuri priverčia susimąstyti apie gyvenimo prasmę.",
                        komentaro_data = DateTime.UtcNow.AddDays(-10),
                        vertinimas = 5,
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000005") // Dievų miškas
                    },
                    new Komentaras
                    {
                        Id = Guid.NewGuid(),
                        komentaro_tekstas = "Klasika, kurią turėtų perskaityti kiekvienas. Orwell'o įžvalgos stulbinančiai tikslios.",
                        komentaro_data = DateTime.UtcNow.AddDays(-8),
                        vertinimas = 5,
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000008") // 1984
                    },
                    new Komentaras
                    {
                        Id = Guid.NewGuid(),
                        komentaro_tekstas = "Puiki alegorija apie valdžią ir korupciją. Trumpa, bet labai talpi.",
                        komentaro_data = DateTime.UtcNow.AddDays(-6),
                        vertinimas = 4,
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000009") // Gyvulių ūkis
                    },
                    new Komentaras
                    {
                        Id = Guid.NewGuid(),
                        komentaro_tekstas = "Silva Rerum - tikras šedevras! Puikiai perteikta XVII amžiaus Lietuvos atmosfera.",
                        komentaro_data = DateTime.UtcNow.AddDays(-4),
                        vertinimas = 5,
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000006") // Silva Rerum
                    },
                    new Komentaras
                    {
                        Id = Guid.NewGuid(),
                        komentaro_tekstas = "Labai liūdna istorija, bet parašyta meistriškai. Biliūnas - tikras talentas.",
                        komentaro_data = DateTime.UtcNow.AddDays(-2),
                        vertinimas = 4,
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000001") // Liūdna pasaka
                    },
                    // Komentarai forumo temoms
                    new Komentaras
                    {
                        Id = Guid.NewGuid(),
                        komentaro_tekstas = "Rekomenduoju pradėti nuo Biliūno novelių - trumpos ir labai jaudinančios!",
                        komentaro_data = DateTime.UtcNow.AddDays(-4),
                        vertinimas = 0,
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        TemaId = Guid.Parse("d0000000-0000-0000-0000-000000000002")
                    },
                    new Komentaras
                    {
                        Id = Guid.NewGuid(),
                        komentaro_tekstas = "Sutinku! Taip pat verta paminėti Krėvės \"Skirgailą\" - puiki istorinė drama.",
                        komentaro_data = DateTime.UtcNow.AddDays(-3),
                        vertinimas = 0,
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        TemaId = Guid.Parse("d0000000-0000-0000-0000-000000000002")
                    },
                    new Komentaras
                    {
                        Id = Guid.NewGuid(),
                        komentaro_tekstas = "Šiandien \"1984\" aktualesnė nei bet kada. Ypač kalbant apie privatumo ir stebėjimo temas.",
                        komentaro_data = DateTime.UtcNow.AddDays(-2),
                        vertinimas = 0,
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        TemaId = Guid.Parse("d0000000-0000-0000-0000-000000000003")
                    }
                };

                await context.Komentarai.AddRangeAsync(komentarai);
                await context.SaveChangesAsync();
            }

            // Seed Irasai (Bookshelf entries)
            if (!await context.Irasai.AnyAsync())
            {
                var irasai = new List<Irasas>
                {
                    // Skaitytojas bookshelf
                    new Irasas
                    {
                        Id = Guid.NewGuid(),
                        tipas = BookshelfTypes.skaityta,
                        sukurimo_data = DateTime.UtcNow.AddDays(-30),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000005") // Dievų miškas
                    },
                    new Irasas
                    {
                        Id = Guid.NewGuid(),
                        tipas = BookshelfTypes.skaityta,
                        sukurimo_data = DateTime.UtcNow.AddDays(-20),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000001") // Liūdna pasaka
                    },
                    new Irasas
                    {
                        Id = Guid.NewGuid(),
                        tipas = BookshelfTypes.skaitoma,
                        sukurimo_data = DateTime.UtcNow.AddDays(-5),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000006") // Silva Rerum
                    },
                    new Irasas
                    {
                        Id = Guid.NewGuid(),
                        tipas = BookshelfTypes.norima_skaityti,
                        sukurimo_data = DateTime.UtcNow.AddDays(-2),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000008") // 1984
                    },
                    // Jonas123 bookshelf
                    new Irasas
                    {
                        Id = Guid.NewGuid(),
                        tipas = BookshelfTypes.skaityta,
                        sukurimo_data = DateTime.UtcNow.AddDays(-15),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000008") // 1984
                    },
                    new Irasas
                    {
                        Id = Guid.NewGuid(),
                        tipas = BookshelfTypes.skaityta,
                        sukurimo_data = DateTime.UtcNow.AddDays(-10),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000009") // Gyvulių ūkis
                    },
                    new Irasas
                    {
                        Id = Guid.NewGuid(),
                        tipas = BookshelfTypes.skaitoma,
                        sukurimo_data = DateTime.UtcNow.AddDays(-3),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000006") // Silva Rerum
                    },
                    new Irasas
                    {
                        Id = Guid.NewGuid(),
                        tipas = BookshelfTypes.norima_skaityti,
                        sukurimo_data = DateTime.UtcNow.AddDays(-1),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000005") // Dievų miškas
                    }
                };

                await context.Irasai.AddRangeAsync(irasai);
                await context.SaveChangesAsync();
            }

            // Seed Autoriaus_sekimai (Author follows)
            if (!await context.Autoriaus_sekimai.AnyAsync())
            {
                var sekimai = new List<Autoriaus_sekimas>
                {
                    new Autoriaus_sekimas
                    {
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000001") // Jonas Biliūnas
                    },
                    new Autoriaus_sekimas
                    {
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000004") // Kristina Sabaliauskaitė
                    },
                    new Autoriaus_sekimas
                    {
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000005") // George Orwell
                    },
                    new Autoriaus_sekimas
                    {
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000004") // Kristina Sabaliauskaitė
                    }
                };

                await context.Autoriaus_sekimai.AddRangeAsync(sekimai);
                await context.SaveChangesAsync();
            }

            // Seed Balsavimai (Voting sessions)
            if (!await context.Balsavimai.AnyAsync())
            {
                var balsavimai = new List<Balsavimas>
                {
                    new Balsavimas
                    {
                        Id = Guid.Parse("e0000000-0000-0000-0000-000000000001"),
                        balsavimo_pradzia = DateTime.UtcNow.AddDays(-14),
                        balsavimo_pabaiga = DateTime.UtcNow.AddDays(-7),
                        isrinkta_knyga_id = Guid.Parse("c0000000-0000-0000-0000-000000000005"), // Dievų miškas
                        uzbaigtas = true,
                        susitikimo_data = DateTime.UtcNow.AddDays(-5),
                        oro_prognoze = "Debesuota, 15°C"
                    },
                    new Balsavimas
                    {
                        Id = Guid.Parse("e0000000-0000-0000-0000-000000000002"),
                        balsavimo_pradzia = DateTime.UtcNow.AddDays(-3),
                        balsavimo_pabaiga = DateTime.UtcNow.AddDays(4),
                        uzbaigtas = false,
                        susitikimo_data = DateTime.UtcNow.AddDays(7)
                    }
                };

                await context.Balsavimai.AddRangeAsync(balsavimai);
                await context.SaveChangesAsync();
            }

            // Seed Balsai (Votes)
            if (!await context.Balsai.AnyAsync())
            {
                var balsai = new List<Balsas>
                {
                    // Balsai praėjusiam balsavimui
                    new Balsas
                    {
                        Id = Guid.NewGuid(),
                        pateikimo_data = DateTime.UtcNow.AddDays(-10),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        BalsavimasId = Guid.Parse("e0000000-0000-0000-0000-000000000001"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000005") // Dievų miškas
                    },
                    new Balsas
                    {
                        Id = Guid.NewGuid(),
                        pateikimo_data = DateTime.UtcNow.AddDays(-9),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        BalsavimasId = Guid.Parse("e0000000-0000-0000-0000-000000000001"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000005") // Dievų miškas
                    },
                    // Balsai dabartiniam balsavimui
                    new Balsas
                    {
                        Id = Guid.NewGuid(),
                        pateikimo_data = DateTime.UtcNow.AddDays(-2),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000004"),
                        BalsavimasId = Guid.Parse("e0000000-0000-0000-0000-000000000002"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000008") // 1984
                    },
                    new Balsas
                    {
                        Id = Guid.NewGuid(),
                        pateikimo_data = DateTime.UtcNow.AddDays(-1),
                        NaudotojasId = Guid.Parse("a0000000-0000-0000-0000-000000000005"),
                        BalsavimasId = Guid.Parse("e0000000-0000-0000-0000-000000000002"),
                        KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000006") // Silva Rerum
                    }
                };

                await context.Balsai.AddRangeAsync(balsai);
                await context.SaveChangesAsync();
            }
        }
    }
}
