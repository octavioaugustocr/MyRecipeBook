using Azure.Storage.Blobs;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyRecipeBook.Domain.Enums;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Recipe;
using MyRecipeBook.Domain.Repositories.User;
using MyRecipeBook.Domain.Security.Cryptography;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Domain.Services.GoogleAI;
using MyRecipeBook.Domain.Services.LoggedUser;
using MyRecipeBook.Domain.Services.OpenAI;
using MyRecipeBook.Domain.Services.Storage;
using MyRecipeBook.Infrastructure.DataAccess;
using MyRecipeBook.Infrastructure.DataAccess.Repositories;
using MyRecipeBook.Infrastructure.Extensions;
using MyRecipeBook.Infrastructure.Security.Cryptography;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Generator;
using MyRecipeBook.Infrastructure.Security.Tokens.Access.Validator;
using MyRecipeBook.Infrastructure.Services.GoogleAI;
using MyRecipeBook.Infrastructure.Services.LoggedUser;
using MyRecipeBook.Infrastructure.Services.OpenAI;
using MyRecipeBook.Infrastructure.Services.Storage;
using OpenAI.Chat;
using System.Reflection;

namespace MyRecipeBook.Infrastructure
{
    public static class DependecyInjectionExtension
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            AddRepositories(services);
            AddLoggedUser(services);
            AddTokens(services, configuration);
            AddPasswordEncrypter(services, configuration);
            // AddOpenAI(services, configuration);
            AddGoogleAI(services, configuration);
            AddLocalStorage(services, configuration);

            if (configuration.IsUnitTestEnviroment())
                return;

            var databaseType = configuration.DatabaseType();

            if (databaseType == DatabaseType.MySql)
            {
                AddDbContext_MySql(services, configuration);
                AddFluentMigrator_MySql(services, configuration);
            }
            else
            {
                AddDbContext_SqlServer(services, configuration);
                AddFluentMigrator_SqlServer(services, configuration);
            }
        }

        private static void AddDbContext_MySql(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("ConnectionMySqlServer");

            services.AddDbContext<MyRecipeBookDbContext>(options =>
            {
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
            });
        }

        private static void AddDbContext_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            var connectionString2 = configuration.ConnectionString();

            services.AddDbContext<MyRecipeBookDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
            services.AddScoped<IUserReadOnlyRepository, UserRepository>();
            services.AddScoped<IUserUpdateOnlyRepository, UserRepository>();
            services.AddScoped<IRecipeWriteOnlyRepository, RecipeRepository>();
            services.AddScoped<IRecipeReadOnlyRepository, RecipeRepository>();
            services.AddScoped<IRecipeUpdateOnlyRepository, RecipeRepository>();
        }

        private static void AddFluentMigrator_MySql(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options.AddMySql5()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
            });
        }

        private static void AddFluentMigrator_SqlServer(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.ConnectionString();

            services.AddFluentMigratorCore().ConfigureRunner(options =>
            {
                options.AddSqlServer()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(Assembly.Load("MyRecipeBook.Infrastructure")).For.All();
            });
        }

        private static void AddTokens(IServiceCollection services, IConfiguration configuration)
        {
            var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
            var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

            services.AddScoped<IAccessTokenGenerator>(option => new JwtTokenGenerator(expirationTimeMinutes, signingKey!));
            services.AddScoped<IAccessTokenValidator>(option => new JwtTokenValidator(signingKey!));
        }

        private static void AddLoggedUser(IServiceCollection services) => services.AddScoped<ILoggedUser, LoggedUser>();

        private static void AddPasswordEncrypter(IServiceCollection services, IConfiguration configuration)
        {
            // var additionalKey = configuration.GetSection("Settings:Password:AdditionalKey").Value;

            // Nuget Package: Microsoft.Extensions.Configuration.Binder
            var additionalKey = configuration.GetValue<string>("Settings:Password:AdditionalKey");

            services.AddScoped<IPasswordEncripter>(option => new Sha512Encripter(additionalKey!));
        }

        /*
        private static void AddOpenAI(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IGenerateRecipeAI, ChatGPTService>();

            var apiKey = configuration.GetValue<string>("Settings:OpenAI:ApiKey");

            services.AddScoped(c => new ChatClient(MyRecipeBookRuleConstants.CHAT_MODEL, apiKey));
        }
        */

        private static void AddGoogleAI(IServiceCollection services, IConfiguration configuration)
        {
            var apiKey = configuration.GetValue<string>("Settings:GoogleAI:ApiKey");

            services.AddScoped<GoogleAIConfig>(_ => new GoogleAIConfig
            {
                ApiKey = apiKey!
            });

            services.AddScoped<IGenerateRecipeGoogleAI, GoogleAIService>();
        }

        private static void AddAzureStorage(IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetValue<string>("Settings:BlobStorage:Azure");

            services.AddScoped<IBlobStorageService>(c => new AzureStorageService(new BlobServiceClient(connectionString)));
        }

        private static void AddLocalStorage(IServiceCollection services, IConfiguration configuration)
        {
            var basePath = configuration.GetValue<string>("Settings:BlobStorage:Local");

            services.AddScoped<IBlobStorageService>(c => new LocalStorageService(basePath!));
        }
    }
}
