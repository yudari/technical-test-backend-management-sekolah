using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Menambahkan layanan controller ke DI container untuk memproses request HTTP di controller.
builder.Services.AddControllers();

// Menambahkan AuthService ke DI container agar bisa digunakan sebagai dependency injection.
builder.Services.AddScoped<AuthService>();

// Menambahkan API explorer untuk mendukung endpoint discovery oleh Swagger.
builder.Services.AddEndpointsApiExplorer();

// Konfigurasi Swagger untuk dokumentasi API.
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Management Sekolah API v1", Version = "v1" }); // Membuat dokumen API versi 1 dengan title.

    // Menambahkan definisi keamanan JWT untuk Swagger, agar memungkinkan pengguna memasukkan token JWT di UI Swagger.
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, // JWT diambil dari header request.
        Description = "Masukkan token JWT seperti ini: Bearer {token}", // Keterangan untuk pengguna mengenai format token.
        Name = "Authorization", // Nama header yang digunakan untuk autentikasi JWT.
        Type = SecuritySchemeType.ApiKey, // Tipe security scheme-nya ApiKey.
        Scheme = "Bearer" // Skema autentikasi Bearer.
    });

    // Menambahkan requirement agar semua endpoint harus menggunakan security Bearer token.
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer" // Referensi ke definisi Bearer token.
                }
            },
            new string[] {} // Skema Bearer tidak membutuhkan scope khusus.
        }
    });
});

// Konfigurasi DbContext untuk PostgreSQL, menggunakan string koneksi yang didefinisikan di appsettings.json.
builder.Services.AddDbContext<SchoolContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Konfigurasi autentikasi JWT.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // Skema autentikasi default adalah JWT.
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // Skema challenge default juga JWT.
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false; // Tidak mewajibkan HTTPS saat testing lokal.
    options.SaveToken = true; // Token akan disimpan di HttpContext.

    // Parameter validasi token JWT.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true, // Validasi issuer token.
        ValidateAudience = true, // Validasi audience token.
        ValidateLifetime = true, // Validasi waktu kadaluarsa token.
        ValidateIssuerSigningKey = true, // Validasi signing key token.
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // Issuer yang sah berasal dari appsettings.json.
        ValidAudience = builder.Configuration["Jwt:Audience"], // Audience yang sah berasal dari appsettings.json.
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])) // Kunci simetrik untuk memvalidasi token.
    };

    // Menambahkan event saat autentikasi JWT gagal.
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Token authentication failed: {context.Exception.Message}"); // Log pesan saat token tidak valid.
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"Token validated: {context.SecurityToken}"); // Log pesan saat token valid.
            return Task.CompletedTask;
        }
    };
});

// Menambahkan kebijakan otorisasi untuk peran Admin dan User.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin")); // Kebijakan hanya untuk role Admin.
    options.AddPolicy("UserOnly", policy => policy.RequireRole("User")); // Kebijakan hanya untuk role User.
});

var app = builder.Build();

// Konfigurasi HTTP request pipeline untuk development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Mengaktifkan Swagger.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Management Sekolah API v1"); // Menentukan endpoint untuk Swagger UI.
        c.RoutePrefix = string.Empty; // Akses Swagger langsung dari root URL.
    });
}

// Menggunakan middleware untuk mengalihkan semua HTTP ke HTTPS.
app.UseHttpsRedirection();

// Middleware custom untuk log header Authorization.
app.Use(async (context, next) =>
{
    var authHeader = context.Request.Headers["Authorization"].FirstOrDefault(); // Mengambil header Authorization dari request.
    Console.WriteLine($"Authorization Header: {authHeader}"); // Log nilai header Authorization.
    await next(); // Lanjutkan ke middleware berikutnya.
});

// Mengaktifkan middleware autentikasi JWT.
app.UseAuthentication();

// Mengaktifkan middleware otorisasi untuk mengecek akses berdasarkan role.
app.UseAuthorization();

// Mendaftarkan route controller untuk menangani request.
app.MapControllers();

// Menjalankan aplikasi.
app.Run();
