using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Cvijecara_Sanja_Tica_IT80_2019.Entities;

public partial class CvijecaraContext : DbContext
{
    public CvijecaraContext()
    {
    }

    public CvijecaraContext(DbContextOptions<CvijecaraContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DetaljiIsporuke> DetaljiIsporukes { get; set; }

    public virtual DbSet<Kategorija> Kategorijas { get; set; }

    public virtual DbSet<Korisnik> Korisniks { get; set; }

    public virtual DbSet<Korpa> Korpas { get; set; }

    public virtual DbSet<Pakovanje> Pakovanjes { get; set; }

    public virtual DbSet<Porudzbina> Porudzbinas { get; set; }

    public virtual DbSet<Proizvod> Proizvods { get; set; }

    public virtual DbSet<StavkaKorpe> StavkaKorpes { get; set; }

    public virtual DbSet<TipKorisnika> TipKorisnikas { get; set; }

    public virtual DbSet<Transakcija> Transakcijas { get; set; }

    public virtual DbSet<Vrstum> Vrsta { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-RCH3286\\SQLEXPRESS01;Initial Catalog=Cvijecara;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DetaljiIsporuke>(entity =>
        {
            entity.HasKey(e => e.IsporukaId);

            entity.ToTable("DetaljiIsporuke", "GiftShop");

            entity.HasIndex(e => e.PorudzbinaId, "UC_porudzbina").IsUnique();

            entity.Property(e => e.IsporukaId).HasColumnName("isporuka_id");
            entity.Property(e => e.Adresa)
                .HasMaxLength(50)
                .HasColumnName("adresa");
            entity.Property(e => e.BrojTelefona)
                .HasMaxLength(25)
                .HasColumnName("broj_telefona");
            entity.Property(e => e.DatumIsporuke)
                .HasColumnType("date")
                .HasColumnName("datum_isporuke");
            entity.Property(e => e.Drzava)
                .HasMaxLength(50)
                .HasColumnName("drzava");
            entity.Property(e => e.Grad)
                .HasMaxLength(50)
                .HasColumnName("grad");
            entity.Property(e => e.Isporuceno).HasColumnName("isporuceno");
            entity.Property(e => e.PorudzbinaId).HasColumnName("porudzbina_id");
            entity.Property(e => e.Region)
                .HasMaxLength(50)
                .HasColumnName("region");
            entity.Property(e => e.Tip)
                .HasMaxLength(15)
                .HasColumnName("tip");
        });

        modelBuilder.Entity<Kategorija>(entity =>
        {
            entity.HasKey(e => e.KategorijaId);

            entity.ToTable("Kategorija", "GiftShop");

            entity.Property(e => e.KategorijaId).HasColumnName("kategorija_id");
            entity.Property(e => e.Naziv)
                .HasMaxLength(50)
                .HasColumnName("naziv");
            entity.Property(e => e.Opis)
                .HasMaxLength(100)
                .HasColumnName("opis");
        });

        modelBuilder.Entity<Korisnik>(entity =>
        {
            entity.HasKey(e => e.KorisnikId);

            entity.ToTable("Korisnik", "GiftShop");

            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.Adresa)
                .HasMaxLength(50)
                .HasColumnName("adresa");
            entity.Property(e => e.BrojTelefona)
                .HasMaxLength(20)
                .HasColumnName("broj_telefona");
            entity.Property(e => e.Email)
                .HasMaxLength(35)
                .HasColumnName("email");
            entity.Property(e => e.Ime)
                .HasMaxLength(25)
                .HasColumnName("ime");
            entity.Property(e => e.KorisnickoIme)
                .HasMaxLength(30)
                .HasColumnName("korisnicko_ime");
            entity.Property(e => e.Lozinka)
                .HasMaxLength(35)
                .HasColumnName("lozinka");
            entity.Property(e => e.Prezime)
                .HasMaxLength(30)
                .HasColumnName("prezime");
            entity.Property(e => e.StatusKorisnika)
                .HasMaxLength(50)
                .HasColumnName("status_korisnika");
            entity.Property(e => e.TipId).HasColumnName("tip_id");

            entity.HasOne(d => d.Tip).WithMany(p => p.Korisniks)
                .HasForeignKey(d => d.TipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Korisnik_Tip");
        });

        modelBuilder.Entity<Korpa>(entity =>
        {
            entity.HasKey(e => e.KorpaId);

            entity.ToTable("Korpa", "GiftShop");

            entity.HasIndex(e => e.KorisnikId, "UC_korisnik_id").IsUnique();

            entity.Property(e => e.KorpaId).HasColumnName("korpa_id");
            entity.Property(e => e.Kolicina).HasColumnName("kolicina");
            entity.Property(e => e.KorisnikId).HasColumnName("korisnik_id");
            entity.Property(e => e.UkupanIznos)
                .HasColumnType("numeric(8, 2)")
                .HasColumnName("ukupan_iznos");
            entity.Property(e => e.Valuta)
                .HasMaxLength(5)
                .HasColumnName("valuta");
            entity.Property(e => e.PaymentIntentId).HasColumnName("payment_intent_id").IsRequired(false);
            entity.Property(e => e.ClientSecret).HasColumnName("client_secret").IsRequired(false);
        });

        modelBuilder.Entity<Pakovanje>(entity =>
        {
            entity.HasKey(e => e.PakovanjeId);

            entity.ToTable("Pakovanje", "GiftShop");

            entity.Property(e => e.PakovanjeId).HasColumnName("pakovanje_id");
            entity.Property(e => e.Cijena)
                .HasColumnType("numeric(8, 2)")
                .HasColumnName("cijena");
            entity.Property(e => e.Posveta)
                .HasMaxLength(150)
                .HasColumnName("posveta");
            entity.Property(e => e.Ukrasi)
                .HasMaxLength(50)
                .HasColumnName("ukrasi");
            entity.Property(e => e.Valuta)
                .HasMaxLength(5)
                .HasColumnName("valuta");
            entity.Property(e => e.Vrsta)
                .HasMaxLength(30)
                .HasColumnName("vrsta");
        });

        modelBuilder.Entity<Porudzbina>(entity =>
        {
            entity.HasKey(e => e.PorudzbinaId);

            entity.ToTable("Porudzbina", "GiftShop", tb => tb.HasTrigger("Popust"));

            entity.Property(e => e.PorudzbinaId).HasColumnName("porudzbina_id");
            entity.Property(e => e.DatumKreiranja)
                .HasColumnType("date")
                .HasColumnName("datum_kreiranja");
            entity.Property(e => e.Popust)
                .HasColumnType("numeric(8, 2)")
                .HasColumnName("popust");
            entity.Property(e => e.Racun)
                .HasColumnType("numeric(8, 2)")
                .HasColumnName("racun");
            entity.Property(e => e.RedniBroj)
                .HasMaxLength(15)
                .HasColumnName("redni_broj");
            entity.Property(e => e.StatusPorudzbine)
                .HasMaxLength(15)
                .HasColumnName("status_porudzbine");
            entity.Property(e => e.PaymentIntentId).HasColumnName("payment_intent_id").IsRequired(false);
            entity.Property(e => e.ClientSecret).HasColumnName("client_secret").IsRequired(false);
        });

        modelBuilder.Entity<Proizvod>(entity =>
        {
            entity.HasKey(e => e.ProizvodId);

            entity.ToTable("Proizvod", "GiftShop");

            entity.Property(e => e.ProizvodId).HasColumnName("proizvod_id");
            entity.Property(e => e.Cijena)
                .HasColumnType("numeric(8, 2)")
                .HasColumnName("cijena");
            entity.Property(e => e.KategorijaId).HasColumnName("kategorija_id");
            entity.Property(e => e.Naziv)
                .HasMaxLength(50)
                .HasColumnName("naziv");
            entity.Property(e => e.PakovanjeId).HasColumnName("pakovanje_id");
            entity.Property(e => e.Valuta)
                .HasMaxLength(5)
                .HasColumnName("valuta");
            entity.Property(e => e.Velicina)
                .HasMaxLength(50)
                .HasColumnName("velicina");
            entity.Property(e => e.VrstaId).HasColumnName("vrsta_id");
            entity.Property(e => e.Slika).HasColumnName("slika");
            entity.Property(e => e.Zalihe)
                .HasColumnType("numeric(8, 2)")
                .HasColumnName("zalihe");

            entity.HasOne(d => d.Kategorija).WithMany(p => p.Proizvods)
                .HasForeignKey(d => d.KategorijaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Proizvod_Kategorija");

            entity.HasOne(d => d.Pakovanje).WithMany(p => p.Proizvods)
                .HasForeignKey(d => d.PakovanjeId)
                .HasConstraintName("FK_Proizvod_Pakovanje");

            entity.HasOne(d => d.Vrsta).WithMany(p => p.Proizvods)
                .HasForeignKey(d => d.VrstaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Proizvod_Vrsta");
        });

        modelBuilder.Entity<StavkaKorpe>(entity =>
        {
            entity.HasKey(e => new { e.ProizvodId, e.KorpaId });

            entity.ToTable("Stavka_Korpe", "GiftShop", tb => tb.HasTrigger("IznosPorudzbine"));
            entity.Property(e => e.ProizvodId).HasColumnName("proizvod_id");
            entity.Property(e => e.KorpaId).HasColumnName("korpa_id");
            entity.Property(e => e.Kolicina).HasColumnName("kolicina");
            entity.Property(e => e.PorudzbinaId).HasColumnName("porudzbina_id");

            entity.HasOne(d => d.Korpa).WithMany(p => p.StavkaKorpes)
                .HasForeignKey(d => d.KorpaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StavkaKorpe_Korpa");

            entity.HasOne(d => d.Porudzbina).WithMany(p => p.StavkaKorpes)
                .HasForeignKey(d => d.PorudzbinaId)
                .HasConstraintName("FK_StavkaKorpe_Porudzbina");

            entity.HasOne(d => d.Proizvod).WithMany(p => p.StavkaKorpes)
                .HasForeignKey(d => d.ProizvodId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StavkaKorpe_Proizvod");
        });

        modelBuilder.Entity<TipKorisnika>(entity =>
        {
            entity.HasKey(e => e.TipId);

            entity.ToTable("Tip_Korisnika", "GiftShop");

            entity.Property(e => e.TipId).HasColumnName("tip_id");
            entity.Property(e => e.Tip)
                .HasMaxLength(25)
                .HasColumnName("tip");
        });

        modelBuilder.Entity<Transakcija>(entity =>
        {
            entity.HasKey(e => e.TransakcijaId);

            entity.ToTable("Transakcija", "GiftShop");

            entity.HasIndex(e => e.PorudzbinaId, "UC_porudzbina_id").IsUnique();

            entity.Property(e => e.TransakcijaId).HasColumnName("transakcija_id");
            entity.Property(e => e.DatumRealizacije)
                .HasColumnType("date")
                .HasColumnName("datum_realizacije");
            entity.Property(e => e.NacinPlacanja)
                .HasMaxLength(25)
                .HasColumnName("nacin_placanja");
            entity.Property(e => e.Placeno).HasColumnName("placeno");
            entity.Property(e => e.PorudzbinaId).HasColumnName("porudzbina_id");
        });

        modelBuilder.Entity<Vrstum>(entity =>
        {
            entity.HasKey(e => e.VrstaId);

            entity.ToTable("Vrsta", "GiftShop");

            entity.Property(e => e.VrstaId).HasColumnName("vrsta_id");
            entity.Property(e => e.Naziv)
                .HasMaxLength(35)
                .HasColumnName("naziv");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
