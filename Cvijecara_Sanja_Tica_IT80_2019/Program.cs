using Cvijecara_Sanja_Tica_IT80_2019.Data.DetaljiIsporukeData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KategorijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.KorisnikData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.PakovanjeData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.PorudzbinaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.ProizvodData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.TipKorisnikaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.TransakcijaData;
using Cvijecara_Sanja_Tica_IT80_2019.Data.VrstaData;
using Cvijecara_Sanja_Tica_IT80_2019.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<CvijecaraContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<ITipKorisnikaRepository, TipKorisnikaRepository>();
builder.Services.AddScoped<IPakovanjeRepository, PakovanjeRepository>();
builder.Services.AddScoped<IKategorijaRepository, KategorijaRepository>();
builder.Services.AddScoped<IVrstaRepository, VrstaRepository>();
builder.Services.AddScoped<IPorudzbinaRepository, PorudzbinaRepository>();
builder.Services.AddScoped<ITransakcijaRepository, TransakcijaRepository>();
builder.Services.AddScoped<IDetaljiIsporukeRepository, DetaljiIsporukeRepository>();
builder.Services.AddScoped<IKorisnikRepository, KorisnikRepository>();
builder.Services.AddScoped<IProizvodRepository, ProizvodRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
