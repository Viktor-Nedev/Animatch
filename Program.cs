using Animatch.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Animatch
{
	public class Program
	{
		public static void Main(string[] args)
		{
			WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

			string connectionString = builder.Configuration.GetConnectionString("ConnectionToSQLServer") ?? throw new InvalidOperationException("Connection string 'ConnectionToSQLServer' not found.");

			builder.Services.AddDbContext<AnimalManagerDbContext>(options =>
				options.UseSqlServer(connectionString));
			builder.Services.AddDatabaseDeveloperPageExceptionFilter();

			builder.Services.AddDefaultIdentity<IdentityUser>(options => {

				ConfigyIdentityOptions(options, builder.Configuration);

            })
				.AddEntityFrameworkStores<AnimalManagerDbContext>();
			builder.Services.AddControllersWithViews();

			WebApplication app = builder.Build();

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

			app.Run();
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
