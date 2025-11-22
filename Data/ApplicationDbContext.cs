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
        }
    }
}
