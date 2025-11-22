using Microsoft.EntityFrameworkCore;
using lentynaBackEnd.Models.Entities;
using lentynaBackEnd.Models.Enums;

namespace lentynaBackEnd.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Naudotojas> Naudotojai { get; set; }
        public DbSet<Autorius> Autoriai { get; set; }
        public DbSet<Knyga> Knygos { get; set; }
        public DbSet<Zanras> Zanrai { get; set; }
        public DbSet<Nuotaika> Nuotaikos { get; set; }
        public DbSet<KnygaZanras> KnygaZanrai { get; set; }
        public DbSet<KnygaNuotaika> KnygaNuotaikos { get; set; }
        public DbSet<Komentaras> Komentarai { get; set; }
        public DbSet<Dirbtinio_intelekto_komentaras> DI_Komentarai { get; set; }
        public DbSet<Citata> Citatos { get; set; }
        public DbSet<Irasas> Irasai { get; set; }
        public DbSet<Knygos_rekomendacija> Knygos_rekomendacijos { get; set; }
        public DbSet<Autoriaus_sekimas> Autoriaus_sekimai { get; set; }
        public DbSet<Tema> Temos { get; set; }
        public DbSet<Balsavimas> Balsavimai { get; set; }
        public DbSet<Balsas> Balsai { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure enum conversions
            modelBuilder.Entity<Naudotojas>()
                .Property(n => n.role)
                .HasConversion<string>();

            modelBuilder.Entity<Irasas>()
                .Property(i => i.tipas)
                .HasConversion<string>();

            // Unique constraints
            modelBuilder.Entity<Naudotojas>()
                .HasIndex(n => n.el_pastas)
                .IsUnique();

            modelBuilder.Entity<Naudotojas>()
                .HasIndex(n => n.slapyvardis)
                .IsUnique();

            modelBuilder.Entity<Zanras>()
                .HasIndex(z => z.pavadinimas)
                .IsUnique();

            modelBuilder.Entity<Knyga>()
                .HasIndex(k => k.ISBN)
                .IsUnique();

            // KnygaZanras - Composite primary key
            modelBuilder.Entity<KnygaZanras>()
                .HasKey(kz => new { kz.KnygaId, kz.ZanrasId });

            modelBuilder.Entity<KnygaZanras>()
                .HasOne(kz => kz.Knyga)
                .WithMany(k => k.KnygaZanrai)
                .HasForeignKey(kz => kz.KnygaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KnygaZanras>()
                .HasOne(kz => kz.Zanras)
                .WithMany(z => z.KnygaZanrai)
                .HasForeignKey(kz => kz.ZanrasId)
                .OnDelete(DeleteBehavior.Cascade);

            // KnygaNuotaika - Composite primary key
            modelBuilder.Entity<KnygaNuotaika>()
                .HasKey(kn => new { kn.KnygaId, kn.NuotaikaId });

            modelBuilder.Entity<KnygaNuotaika>()
                .HasOne(kn => kn.Knyga)
                .WithMany(k => k.KnygaNuotaikos)
                .HasForeignKey(kn => kn.KnygaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<KnygaNuotaika>()
                .HasOne(kn => kn.Nuotaika)
                .WithMany(n => n.KnygaNuotaikos)
                .HasForeignKey(kn => kn.NuotaikaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Autoriaus_sekimas - Composite primary key
            modelBuilder.Entity<Autoriaus_sekimas>()
                .HasKey(a => new { a.NaudotojasId, a.AutoriusId });

            modelBuilder.Entity<Autoriaus_sekimas>()
                .HasOne(a => a.Naudotojas)
                .WithMany(n => n.Autoriaus_sekimai)
                .HasForeignKey(a => a.NaudotojasId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Autoriaus_sekimas>()
                .HasOne(a => a.Autorius)
                .WithMany(au => au.Autoriaus_sekimai)
                .HasForeignKey(a => a.AutoriusId)
                .OnDelete(DeleteBehavior.Cascade);

            // Autorius -> Knyga (1 to many)
            modelBuilder.Entity<Knyga>()
                .HasOne(k => k.Autorius)
                .WithMany(a => a.Knygos)
                .HasForeignKey(k => k.AutoriusId)
                .OnDelete(DeleteBehavior.Restrict);

            // Autorius -> Citata (1 to many)
            modelBuilder.Entity<Citata>()
                .HasOne(c => c.Autorius)
                .WithMany(a => a.Citatos)
                .HasForeignKey(c => c.AutoriusId)
                .OnDelete(DeleteBehavior.Cascade);

            // Knyga -> Komentaras (1 to many, optional)
            modelBuilder.Entity<Komentaras>()
                .HasOne(k => k.Knyga)
                .WithMany(kn => kn.Komentarai)
                .HasForeignKey(k => k.KnygaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Naudotojas -> Komentaras (1 to many)
            modelBuilder.Entity<Komentaras>()
                .HasOne(k => k.Naudotojas)
                .WithMany(n => n.Komentarai)
                .HasForeignKey(k => k.NaudotojasId)
                .OnDelete(DeleteBehavior.Cascade);

            // Tema -> Komentaras (1 to many, optional)
            modelBuilder.Entity<Komentaras>()
                .HasOne(k => k.Tema)
                .WithMany(t => t.Komentarai)
                .HasForeignKey(k => k.TemaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Knyga -> Dirbtinio_intelekto_komentaras (1 to many)
            modelBuilder.Entity<Dirbtinio_intelekto_komentaras>()
                .HasOne(d => d.Knyga)
                .WithMany(k => k.DI_Komentarai)
                .HasForeignKey(d => d.KnygaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Naudotojas -> Irasas (1 to many)
            modelBuilder.Entity<Irasas>()
                .HasOne(i => i.Naudotojas)
                .WithMany(n => n.Irasai)
                .HasForeignKey(i => i.NaudotojasId)
                .OnDelete(DeleteBehavior.Cascade);

            // Knyga -> Irasas (1 to many)
            modelBuilder.Entity<Irasas>()
                .HasOne(i => i.Knyga)
                .WithMany(k => k.Irasai)
                .HasForeignKey(i => i.KnygaId)
                .OnDelete(DeleteBehavior.Cascade);

            // Knygos_rekomendacija -> Irasas (1 to many, optional)
            modelBuilder.Entity<Irasas>()
                .HasOne(i => i.Knygos_rekomendacija)
                .WithMany(kr => kr.Irasai)
                .HasForeignKey(i => i.KnygosRekomendacijaId)
                .OnDelete(DeleteBehavior.SetNull);

            // Naudotojas -> Knygos_rekomendacija (1 to many)
            modelBuilder.Entity<Knygos_rekomendacija>()
                .HasOne(kr => kr.Naudotojas)
                .WithMany(n => n.Knygos_rekomendacijos)
                .HasForeignKey(kr => kr.NaudotojasId)
                .OnDelete(DeleteBehavior.Cascade);

            // Naudotojas -> Tema (1 to many)
            modelBuilder.Entity<Tema>()
                .HasOne(t => t.Naudotojas)
                .WithMany(n => n.Temos)
                .HasForeignKey(t => t.NaudotojasId)
                .OnDelete(DeleteBehavior.Cascade);

            // Balsavimas -> Knyga (selected book, optional)
            modelBuilder.Entity<Balsavimas>()
                .HasOne(b => b.IsrinktaKnyga)
                .WithMany()
                .HasForeignKey(b => b.isrinkta_knyga_id)
                .OnDelete(DeleteBehavior.SetNull);

            // Balsavimas -> Balsas (1 to many)
            modelBuilder.Entity<Balsas>()
                .HasOne(b => b.Balsavimas)
                .WithMany(ba => ba.Balsai)
                .HasForeignKey(b => b.BalsavimasId)
                .OnDelete(DeleteBehavior.Cascade);

            // Naudotojas -> Balsas (1 to many)
            modelBuilder.Entity<Balsas>()
                .HasOne(b => b.Naudotojas)
                .WithMany(n => n.Balsai)
                .HasForeignKey(b => b.NaudotojasId)
                .OnDelete(DeleteBehavior.Cascade);

            // Knyga -> Balsas (1 to many)
            modelBuilder.Entity<Balsas>()
                .HasOne(b => b.Knyga)
                .WithMany(k => k.Balsai)
                .HasForeignKey(b => b.KnygaId)
                .OnDelete(DeleteBehavior.NoAction);

            // Seed data for Nuotaikos
            modelBuilder.Entity<Nuotaika>().HasData(
                new Nuotaika { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), pavadinimas = "Džiugi" },
                new Nuotaika { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), pavadinimas = "Liūdna" },
                new Nuotaika { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), pavadinimas = "Neutrali" }
            );

            // Seed data for Zanrai
            modelBuilder.Entity<Zanras>().HasData(
                new Zanras { Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), pavadinimas = "Fantastika" },
                new Zanras { Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), pavadinimas = "Detektyvai" },
                new Zanras { Id = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc"), pavadinimas = "Romanai" },
                new Zanras { Id = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"), pavadinimas = "Mokslinė fantastika" },
                new Zanras { Id = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee"), pavadinimas = "Istoriniai" },
                new Zanras { Id = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff"), pavadinimas = "Biografijos" },
                new Zanras { Id = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), pavadinimas = "Trileriai" },
                new Zanras { Id = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), pavadinimas = "Poezija" }
            );

            // Seed data for Autoriai
            modelBuilder.Entity<Autorius>().HasData(
                new Autorius
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000001"),
                    vardas = "Jonas",
                    pavarde = "Biliūnas",
                    gimimo_metai = new DateTime(1879, 4, 11),
                    mirties_data = new DateTime(1907, 12, 8),
                    curiculum_vitae = "Lietuvių rašytojas, poetas, publicistas. Vienas žymiausių lietuvių novelistų.",
                    knygu_skaicius = 2
                },
                new Autorius
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000002"),
                    vardas = "Vincas",
                    pavarde = "Krėvė-Mickevičius",
                    gimimo_metai = new DateTime(1882, 10, 19),
                    mirties_data = new DateTime(1954, 7, 7),
                    curiculum_vitae = "Lietuvių rašytojas, dramaturgas, profesorius. Parašė daug dramų, apsakymų ir romanų.",
                    knygu_skaicius = 2
                },
                new Autorius
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000003"),
                    vardas = "Balys",
                    pavarde = "Sruoga",
                    gimimo_metai = new DateTime(1896, 2, 2),
                    mirties_data = new DateTime(1947, 10, 16),
                    curiculum_vitae = "Lietuvių poetas, dramaturgas, literatūros kritikas ir teatro teoretikas.",
                    knygu_skaicius = 1
                },
                new Autorius
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000004"),
                    vardas = "Kristina",
                    pavarde = "Sabaliauskaitė",
                    gimimo_metai = new DateTime(1974, 9, 2),
                    curiculum_vitae = "Šiuolaikinė lietuvių rašytoja, meno istorikė. Garsėja istorinių romanų serija „Silva Rerum",
                    knygu_skaicius = 2
                },
                new Autorius
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000005"),
                    vardas = "George",
                    pavarde = "Orwell",
                    gimimo_metai = new DateTime(1903, 6, 25),
                    mirties_data = new DateTime(1950, 1, 21),
                    curiculum_vitae = "Anglų rašytojas ir žurnalistas, garsėjantis distopiniais romanais ir socialine kritika.",
                    knygu_skaicius = 2
                }
            );

            // Seed data for Knygos
            modelBuilder.Entity<Knyga>().HasData(
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"),
                    knygos_pavadinimas = "Liūdna pasaka",
                    leidimo_metai = new DateTime(1907, 1, 1),
                    aprasymas = "Viena garsiausių J. Biliūno novelių apie vargšę mergaitę ir jos sunkų gyvenimą.",
                    psl_skaicius = 24,
                    ISBN = "9785417012345",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000001")
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000002"),
                    knygos_pavadinimas = "Kliudžiau",
                    leidimo_metai = new DateTime(1906, 1, 1),
                    aprasymas = "Novelė apie kaltės jausmą ir žmogaus sąžinę.",
                    psl_skaicius = 18,
                    ISBN = "9785417012346",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000001")
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000003"),
                    knygos_pavadinimas = "Skirgaila",
                    leidimo_metai = new DateTime(1925, 1, 1),
                    aprasymas = "Istorinė drama apie Lietuvos kunigaikštį Skirgailą ir jo vidines kovas.",
                    psl_skaicius = 180,
                    ISBN = "9785417012347",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000002")
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000004"),
                    knygos_pavadinimas = "Dainavos šalies senų žmonių padavimai",
                    leidimo_metai = new DateTime(1912, 1, 1),
                    aprasymas = "Padavimų rinkinys iš Dzūkijos krašto.",
                    psl_skaicius = 256,
                    ISBN = "9785417012348",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000002")
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000005"),
                    knygos_pavadinimas = "Dievų miškas",
                    leidimo_metai = new DateTime(1957, 1, 1),
                    aprasymas = "Memuarai apie autoriaus patirtį Štuthofo koncentracijos stovykloje.",
                    psl_skaicius = 320,
                    ISBN = "9785417012349",
                    bestseleris = true,
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000003")
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000006"),
                    knygos_pavadinimas = "Silva Rerum",
                    leidimo_metai = new DateTime(2008, 1, 1),
                    aprasymas = "Pirmoji istorinių romanų serijos knyga apie Narvoišių giminę XVII amžiaus Lietuvoje.",
                    psl_skaicius = 456,
                    ISBN = "9789955234562",
                    bestseleris = true,
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000004")
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000007"),
                    knygos_pavadinimas = "Silva Rerum II",
                    leidimo_metai = new DateTime(2011, 1, 1),
                    aprasymas = "Antroji serijos knyga, tęsianti Narvoišių giminės istoriją.",
                    psl_skaicius = 512,
                    ISBN = "9789955234563",
                    bestseleris = true,
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000004")
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000008"),
                    knygos_pavadinimas = "1984",
                    leidimo_metai = new DateTime(1949, 6, 8),
                    aprasymas = "Distopinis romanas apie totalitarinę visuomenę, kurioje valdžia kontroliuoja kiekvieną gyvenimo aspektą.",
                    psl_skaicius = 328,
                    ISBN = "9780451524935",
                    bestseleris = true,
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000005")
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000009"),
                    knygos_pavadinimas = "Gyvulių ūkis",
                    leidimo_metai = new DateTime(1945, 8, 17),
                    aprasymas = "Alegorinė pasaka apie gyvulius, kurie sukyla prieš savo šeimininkus ir bando sukurti lygybės visuomenę.",
                    psl_skaicius = 112,
                    ISBN = "9780451526342",
                    bestseleris = true,
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000005")
                }
            );

            // Seed data for KnygaZanras (Book-Genre relationships)
            modelBuilder.Entity<KnygaZanras>().HasData(
                // Liūdna pasaka - Romanai
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000001"), ZanrasId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                // Kliudžiau - Romanai
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000002"), ZanrasId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                // Skirgaila - Istoriniai
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000003"), ZanrasId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                // Dainavos padavimai - Fantastika
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000004"), ZanrasId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                // Dievų miškas - Biografijos
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000005"), ZanrasId = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff") },
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000005"), ZanrasId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                // Silva Rerum - Istoriniai, Romanai
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000006"), ZanrasId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000006"), ZanrasId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                // Silva Rerum II - Istoriniai, Romanai
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000007"), ZanrasId = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee") },
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000007"), ZanrasId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc") },
                // 1984 - Mokslinė fantastika, Trileriai
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000008"), ZanrasId = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd") },
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000008"), ZanrasId = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa") },
                // Gyvulių ūkis - Fantastika
                new KnygaZanras { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000009"), ZanrasId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa") }
            );

            // Seed data for KnygaNuotaika (Book-Mood relationships)
            modelBuilder.Entity<KnygaNuotaika>().HasData(
                // Liūdna pasaka - Liūdna
                new KnygaNuotaika { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000001"), NuotaikaId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                // Kliudžiau - Liūdna
                new KnygaNuotaika { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000002"), NuotaikaId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                // Skirgaila - Neutrali
                new KnygaNuotaika { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000003"), NuotaikaId = Guid.Parse("33333333-3333-3333-3333-333333333333") },
                // Dainavos padavimai - Džiugi
                new KnygaNuotaika { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000004"), NuotaikaId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
                // Dievų miškas - Liūdna
                new KnygaNuotaika { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000005"), NuotaikaId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                // Silva Rerum - Neutrali
                new KnygaNuotaika { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000006"), NuotaikaId = Guid.Parse("33333333-3333-3333-3333-333333333333") },
                // Silva Rerum II - Neutrali
                new KnygaNuotaika { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000007"), NuotaikaId = Guid.Parse("33333333-3333-3333-3333-333333333333") },
                // 1984 - Liūdna
                new KnygaNuotaika { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000008"), NuotaikaId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
                // Gyvulių ūkis - Neutrali
                new KnygaNuotaika { KnygaId = Guid.Parse("c0000000-0000-0000-0000-000000000009"), NuotaikaId = Guid.Parse("33333333-3333-3333-3333-333333333333") }
            );
        }
    }
}
