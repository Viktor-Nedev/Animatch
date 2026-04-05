using Animatch.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Animatch.Services;

namespace Animatch
{
	public class Program
	{

		public static async Task Main(string[] args)
		{
			if (File.Exists(".env"))
			{
				foreach (var line in File.ReadAllLines(".env"))
				{
					var parts = line.Split('=', 2);
					if (parts.Length == 2) Environment.SetEnvironmentVariable(parts[0], parts[1]);
				}
			}

			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
			builder.Configuration.AddEnvironmentVariables();

			string connectionString = builder.Configuration.GetConnectionString("ConnectionToSQLServer") ?? throw new InvalidOperationException("Connection string 'ConnectionToSQLServer' not found.");

			builder.Services.AddDbContext<AnimalManagerDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<IdentityUser>(options => {

				ConfigyIdentityOptions(options, builder.Configuration);

            })
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<AnimalManagerDbContext>();
			builder.Services.AddControllersWithViews();
			builder.Services.AddScoped<IAnimalService, AnimalService>();
			builder.Services.AddScoped<ICategoryService, CategoryService>();
			builder.Services.AddScoped<IEventService, EventService>();
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

			WebApplication app = builder.Build();

			using (var scope = app.Services.CreateScope())
			{
				var services = scope.ServiceProvider;
				try
				{
					await SeedData.InitializeAsync(services, builder.Configuration);
				}
				catch (Exception ex)
				{
					var logger = services.GetRequiredService<ILogger<Program>>();
					logger.LogError(ex, "An error occurred while seeding roles and admin user.");
				}
			}

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseMigrationsEndPoint();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthorization();

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}");
			app.MapRazorPages();

			await app.RunAsync();
		}


		private static void ConfigyIdentityOptions(IdentityOptions options, ConfigurationManager configuration) {

			
            options.SignIn.RequireConfirmedAccount = configuration
                .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedAccount");
            options.SignIn.RequireConfirmedPhoneNumber = configuration
                .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedPhoneNumber");
            options.SignIn.RequireConfirmedEmail = configuration
                .GetValue<bool>("IdentityOptions:SignIn:RequireConfirmedEmail");

            options.User.RequireUniqueEmail = configuration
				.GetValue<bool>("IdentityOptions:User:RequireUniqueEmail");

            options.Lockout.MaxFailedAccessAttempts = configuration
				.GetValue<int>("IdentityOptions:Lockout:MaxFailedAccessAttempts");
            options.Lockout.DefaultLockoutTimeSpan = configuration
				.GetValue<TimeSpan>("IdentityOptions:Lockout:DefaultLockoutTimeSpan");

            options.Password.RequireDigit = configuration
				.GetValue<bool>("IdentityOptions:Password:RequireDigit");
            options.Password.RequireLowercase = configuration
				.GetValue<bool>("IdentityOptions:Password:RequireLowercase");
            options.Password.RequireNonAlphanumeric = configuration
				.GetValue<bool>("IdentityOptions:Password:RequireNonAlphanumeric");
            options.Password.RequireUppercase = configuration
				.GetValue<bool>("IdentityOptions:Password:RequireUppercase");
            options.Password.RequiredLength = configuration
				.GetValue<int>("IdentityOptions:Password:RequiredLength");
            options.Password.RequiredUniqueChars = configuration
				.GetValue<int>("IdentityOptions:Password:RequiredUniqueChars");




        }

    }
}
