using CargoTrack.Business;
using CargoTrack.Business.Services.Abouts;
using CargoTrack.Business.Services.AuditLogs;
using CargoTrack.Business.Services.Branches;
using CargoTrack.Business.Services.CargoPrices;
using CargoTrack.Business.Services.CargoPricings;
using CargoTrack.Business.Services.Cargos;
using CargoTrack.Business.Services.Cities;
using CargoTrack.Business.Services.Dashboards;
using CargoTrack.Business.Services.Employees;
using CargoTrack.Business.Services.Reports;
using CargoTrack.Business.Services.TransferCenters;
using CargoTrack.DataAccess.Context;
using CargoTrack.DataAccess.Repositories.Abouts;
using CargoTrack.DataAccess.Repositories.Addresses;
using CargoTrack.DataAccess.Repositories.AuditLogs;
using CargoTrack.DataAccess.Repositories.Branches;
using CargoTrack.DataAccess.Repositories.CargoPrices;
using CargoTrack.DataAccess.Repositories.Cargos;
using CargoTrack.DataAccess.Repositories.Cities;
using CargoTrack.DataAccess.Repositories.Deliveries;
using CargoTrack.DataAccess.Repositories.Employees;
using CargoTrack.DataAccess.Repositories.TransferCenters;
using CargoTrack.Entity.Entities;
using CargoTrack.Entity.Entities.Enums;
using CargoTrack.WebUI.Middleware;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// IOC Container

builder.Services.AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters()
                .AddValidatorsFromAssembly(typeof(BusinessAssembly).Assembly);

builder.Services.AddScoped<IAboutRepository, AboutRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>();
builder.Services.AddScoped<ICargoRepository, CargoRepository>();
builder.Services.AddScoped<ICargoPriceRepository, CargoPriceRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ITransferCenterRepository, TransferCenterRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();


builder.Services.AddScoped<IAboutService, AboutService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<ICargoService, CargoService>();
builder.Services.AddScoped<ICargoPricingService, CargoPricingService>();
builder.Services.AddScoped<ITransferCenterService, TransferCenterService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ICargoPriceService, CargoPriceService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportService, ReportService>();




builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

    options.UseLazyLoadingProxies();
});

builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = "/Login/Index";
    config.LogoutPath = "/Login/Logout";
    config.AccessDeniedPath = "/ErrorPages/AccessDenied";
    config.Cookie.Name = "CargoTrackCookie";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    // Veritabanının var olduğundan emin olun (Migration kullanıyorsanız context.Database.Migrate() yapın)
    context.Database.EnsureCreated();

    // Eğer veritabanında hiç şehir yoksa ekle
    if (!context.Cities.Any())
    {
        var cities = new List<City>
        {
            new City { Id = Guid.NewGuid(), Name = "İstanbul" },
            new City { Id = Guid.NewGuid(), Name = "Ankara" },
            new City { Id = Guid.NewGuid(), Name = "İzmir" },
            new City { Id = Guid.NewGuid(), Name = "Bursa" },
            new City { Id = Guid.NewGuid(), Name = "Antalya" },
            new City { Id = Guid.NewGuid(), Name = "Adana" },
            new City { Id = Guid.NewGuid(), Name = "Konya" },
            new City { Id = Guid.NewGuid(), Name = "Şanlıurfa" },
            new City { Id = Guid.NewGuid(), Name = "Gaziantep" },
            new City { Id = Guid.NewGuid(), Name = "Kocaeli" },
            new City { Id = Guid.NewGuid(), Name = "Mersin" },
            new City { Id = Guid.NewGuid(), Name = "Diyarbakır" },
            new City { Id = Guid.NewGuid(), Name = "Hatay" },
            new City { Id = Guid.NewGuid(), Name = "Kayseri" },
            new City { Id = Guid.NewGuid(), Name = "Sakarya" },
            new City { Id = Guid.NewGuid(), Name = "Samsun" },
            new City { Id = Guid.NewGuid(), Name = "Balıkesir" },
            new City { Id = Guid.NewGuid(), Name = "Kahramanmaraş" },
            new City { Id = Guid.NewGuid(), Name = "Van" },
            new City { Id = Guid.NewGuid(), Name = "Aydın" },
            new City { Id = Guid.NewGuid(), Name = "Tekirdağ" }
        };

        context.Cities.AddRange(cities);
        context.SaveChanges();
    }
    // Eğer veritabanında hiç rol yoksa ekle
    if (!context.Roles.Any())
    {
        var roles = new List<AppRole>
        {
            new AppRole { Name = "Admin", NormalizedName = "ADMIN" },
            new AppRole { Name = "Manager", NormalizedName = "MANAGER" },
            new AppRole { Name = "User", NormalizedName = "USER" }
        };

        context.Roles.AddRange(roles);
        context.SaveChanges();
    }

    if (!context.CargoPrices.Any())
    {
        context.CargoPrices.AddRange(
            new CargoPrice { Name = "Standart", CargoType = CargoType.Standard, BasePrice = 50, PricePerKg = 5, PricePerDesi = 5, IsActive = true },
            new CargoPrice { Name = "Acil", CargoType = CargoType.Urgent, BasePrice = 80, PricePerKg = 8, PricePerDesi = 8, IsActive = true },
            new CargoPrice { Name = "Kırılgan", CargoType = CargoType.Fragile, BasePrice = 70, PricePerKg = 6, PricePerDesi = 6, IsActive = true },
            new CargoPrice { Name = "Ağır", CargoType = CargoType.Heavy, BasePrice = 100, PricePerKg = 10, PricePerDesi = 10, IsActive = true }
        );
        context.SaveChanges();
    }


    if (!context.Cargos.Any())
    {
        var users = context.Users.Take(2).ToList();
        var branches = context.Branches.Take(2).ToList();

        if (users.Count >= 2 && branches.Count >= 2)
        {
            var cargo = new Cargo
            {
                Id = Guid.NewGuid(),
                SenderId = users[0].Id,
                ReceiverId = users[1].Id,
                OriginBranchId = branches[0].Id,
                DestinationBranchId = branches[1].Id,
                TrackCode = "CT202609081234",
                ShipmentDate = DateTime.Now,
                EstimatedArrivalDate = DateTime.Now.AddDays(2),
                Weight = 2.5,
                CargoType = CargoTrack.Entity.Entities.Enums.CargoType.Standard,
                CargoStatus = CargoTrack.Entity.Entities.Enums.CargoStatus.AtTransferCenter
            };

            context.Add(cargo);
            context.SaveChanges();
        }
    }

    if (!context.Employees.Any())
    {
        var managerRole = context.Roles.FirstOrDefault(r => r.Name == "Manager");
        var branch = context.Branches.FirstOrDefault(b => !b.IsDeleted);

        if (managerRole is not null && branch is not null)
        {
            var managerUserId = context.Set<IdentityUserRole<Guid>>()
                .Where(ur => ur.RoleId == managerRole.Id)
                .Select(ur => ur.UserId)
                .FirstOrDefault();

            if (managerUserId != Guid.Empty)
            {
                var managerUser = context.Users.FirstOrDefault(u => u.Id == managerUserId);
                if (managerUser is not null)
                {
                    context.Employees.Add(new Employee
                    {
                        FirstName = managerUser.FirstName,
                        LastName = managerUser.LastName,
                        Title = "Şube Müdürü",
                        BranchId = branch.Id,
                        AppUserId = managerUser.Id
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}

app.Run();
