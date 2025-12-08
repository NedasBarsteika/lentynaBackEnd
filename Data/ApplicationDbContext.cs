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

            // Zanras -> Knyga (1 to many)
            modelBuilder.Entity<Knyga>()
                .HasOne(k => k.Zanras)
                .WithMany(z => z.Knygos)
                .HasForeignKey(k => k.ZanrasId)
                .OnDelete(DeleteBehavior.Restrict);

            // Nuotaika - JSON stulpelis žanrų sąrašui
            modelBuilder.Entity<Nuotaika>()
                .Property(n => n.ZanrasIds)
                .HasColumnType("json");

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

            // Seed data for Zanrai
            var zanrasFantastika = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var zanrasDetektyvai = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
            var zanrasRomanai = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
            var zanrasMoksline = Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd");
            var zanrasIstoriniai = Guid.Parse("eeeeeeee-eeee-eeee-eeee-eeeeeeeeeeee");
            var zanrasBiografijos = Guid.Parse("ffffffff-ffff-ffff-ffff-ffffffffffff");
            var zanrasTrileriai = Guid.Parse("11111111-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var zanrasPoezija = Guid.Parse("22222222-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

            modelBuilder.Entity<Zanras>().HasData(
                new Zanras { Id = zanrasFantastika, pavadinimas = "Fantastika" },
                new Zanras { Id = zanrasDetektyvai, pavadinimas = "Detektyvai" },
                new Zanras { Id = zanrasRomanai, pavadinimas = "Romanai" },
                new Zanras { Id = zanrasMoksline, pavadinimas = "Mokslune fantastika" },
                new Zanras { Id = zanrasIstoriniai, pavadinimas = "Istoriniai" },
                new Zanras { Id = zanrasBiografijos, pavadinimas = "Biografijos" },
                new Zanras { Id = zanrasTrileriai, pavadinimas = "Trileriai" },
                new Zanras { Id = zanrasPoezija, pavadinimas = "Poezija" }
            );

            // Seed data for Nuotaikos
            // PASTABA: ZanrasIds bus užpildyti per migraciją SQL komandomis
            modelBuilder.Entity<Nuotaika>().HasData(
                new Nuotaika { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), pavadinimas = "Dziugi" },
                new Nuotaika { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), pavadinimas = "Liudna" },
                new Nuotaika { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), pavadinimas = "Neutrali" },
                new Nuotaika { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), pavadinimas = "Itemptas" },
                new Nuotaika { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), pavadinimas = "Romantiskas" }
            );

            // Seed data for Autoriai
            modelBuilder.Entity<Autorius>().HasData(
                new Autorius
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000001"),
                    vardas = "Jonas",
                    pavarde = "Biliunas",
                    gimimo_metai = new DateTime(1879, 4, 11),
                    mirties_data = new DateTime(1907, 12, 8),
                    curiculum_vitae = "Lietuviu rasytojas, poetas, publicistas.",
                    tautybe = "Lietuvis",
                    knygu_skaicius = 2
                },
                new Autorius
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000002"),
                    vardas = "Vincas",
                    pavarde = "Kreve-Mickevicius",
                    gimimo_metai = new DateTime(1882, 10, 19),
                    mirties_data = new DateTime(1954, 7, 7),
                    curiculum_vitae = "Lietuviu rasytojas, dramaturgas.",
                    tautybe = "Lietuvis",
                    knygu_skaicius = 2
                },
                new Autorius
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000003"),
                    vardas = "Balys",
                    pavarde = "Sruoga",
                    gimimo_metai = new DateTime(1896, 2, 2),
                    mirties_data = new DateTime(1947, 10, 16),
                    curiculum_vitae = "Lietuviu poetas, dramaturgas.",
                    tautybe = "Lietuvis",
                    knygu_skaicius = 1
                },
                new Autorius
                {
                    Id = Guid.Parse("b0000000-0000-0000-0000-000000000004"),
                    vardas = "George",
                    pavarde = "Orwell",
                    gimimo_metai = new DateTime(1903, 6, 25),
                    mirties_data = new DateTime(1950, 1, 21),
                    curiculum_vitae = "Anglu rasytojas ir zurnalistas.",
                    tautybe = "Anglas",
                    knygu_skaicius = 2
                }
            );

            // Seed data for Knygos (with single ZanrasId)
            modelBuilder.Entity<Knyga>().HasData(
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000001"),
                    knygos_pavadinimas = "Liudna pasaka",
                    leidimo_metai = new DateTime(1907, 1, 1),
                    aprasymas = "Viena garsiausiu J. Biliuno noveliu.",
                    psl_skaicius = 24,
                    ISBN = "9785417012345",
                    kalba = "Lietuviu",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000001"),
                    ZanrasId = zanrasRomanai
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000002"),
                    knygos_pavadinimas = "Kliudziau",
                    leidimo_metai = new DateTime(1906, 1, 1),
                    aprasymas = "Novele apie kaltes jausma.",
                    psl_skaicius = 18,
                    ISBN = "9785417012346",
                    kalba = "Lietuviu",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000001"),
                    ZanrasId = zanrasRomanai
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000003"),
                    knygos_pavadinimas = "Skirgaila",
                    leidimo_metai = new DateTime(1925, 1, 1),
                    aprasymas = "Istorine drama apie Lietuvos kunigaiksti.",
                    psl_skaicius = 180,
                    ISBN = "9785417012347",
                    kalba = "Lietuviu",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000002"),
                    ZanrasId = zanrasIstoriniai
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000004"),
                    knygos_pavadinimas = "Dievu miskas",
                    leidimo_metai = new DateTime(1957, 1, 1),
                    aprasymas = "Memuarai apie Stuthofo koncentracijos stovykla.",
                    psl_skaicius = 320,
                    ISBN = "9785417012349",
                    bestseleris = true,
                    kalba = "Lietuviu",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000003"),
                    ZanrasId = zanrasBiografijos
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000005"),
                    knygos_pavadinimas = "1984",
                    leidimo_metai = new DateTime(1949, 6, 8),
                    aprasymas = "Distopinis romanas apie totalitarine visuomene.",
                    psl_skaicius = 328,
                    ISBN = "9780451524935",
                    bestseleris = true,
                    kalba = "Anglu",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000004"),
                    ZanrasId = zanrasMoksline
                },
                new Knyga
                {
                    Id = Guid.Parse("c0000000-0000-0000-0000-000000000006"),
                    knygos_pavadinimas = "Gyvuliu ukis",
                    leidimo_metai = new DateTime(1945, 8, 17),
                    aprasymas = "Alegorine pasaka apie gyvulius.",
                    psl_skaicius = 112,
                    ISBN = "9780451526342",
                    bestseleris = true,
                    kalba = "Anglu",
                    AutoriusId = Guid.Parse("b0000000-0000-0000-0000-000000000004"),
                    ZanrasId = zanrasFantastika
                }
            );
        }
    }
}
