using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AzureAD.Provisioning.ScimReference.Api.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Microsoft.AzureAD.Provisioning.ScimReference.Api
{
	/// <summary>
	/// Startup.
	/// </summary>
	public class Startup
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		/// <summary>
		/// Configuration.
		/// </summary>
		public IConfiguration Configuration { get; }

		/// <summary>
		/// This method gets called by the runtime. Use this method to add services to the container.
		/// </summary>
		public static void ConfigureServices(IServiceCollection services)
		{
			services.AddDbContext<ScimContext>(opt => opt.UseInMemoryDatabase("ONAD"));

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			  .AddJwtBearer(options =>
			  {
				  options.TokenValidationParameters =
					   new TokenValidationParameters
					   {
						   ValidateIssuer = true,
						   ValidateAudience = true,
						   ValidateLifetime = true,
						   ValidateIssuerSigningKey = true,
						   ValidIssuer = "Microsoft.Security.Bearer",
						   ValidAudience = "Microsoft.Security.Bearer",
						   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Microsoft.Security.Bearer"))
					   };
			  });

			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}

		/// <summary>
		/// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
		/// </summary>
		public static void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseMvc();
		}
	}
}
