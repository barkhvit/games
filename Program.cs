using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Millionaire.BackGroundServices;
using Millionaire.Core.Interfaces;
using Millionaire.Data;
using Millionaire.Data.DataContext;
using Millionaire.Data.Repositories;
using Millionaire.GamesManager.Manager;
using Millionaire.Services.Services;
using Millionaire.TelegramBot.Handlers;
using Millionaire.TelegramBot.Views;
using System.Configuration;
using System.Reflection;
using Telegram.Bot;

namespace Millionaire
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                IHost host = Host.CreateDefaultBuilder(args).UseWindowsService(options =>
                {
                    options.ServiceName = "MillionaireService";
                })
                .ConfigureServices(ConfigureServices)
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                    logging.AddDebug();

                    if (OperatingSystem.IsWindows())
                    {
                        logging.AddEventLog(settings =>
                        {
                            if(OperatingSystem.IsWindows())
                                settings.SourceName = "MillionaireService";
                        });
                    }
                    
                })
                .Build();

                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                // Логируйте в файл или другую систему
                File.WriteAllText("crash_log.txt", ex.ToString());

                // Даем время прочитать сообщение перед закрытием
                Console.WriteLine("Нажмите любую клавишу для выхода...");
                Console.ReadKey();
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            // Регистрируем бота
            services.AddSingleton<ITelegramBotClient>(sp =>
            {
                string? token = Environment.GetEnvironmentVariable("Millionaire", EnvironmentVariableTarget.Machine);
                //string token = Environment.GetEnvironmentVariable("BotForTesting", EnvironmentVariableTarget.Machine);
                if(token!=null)
                    return new TelegramBotClient(token);

                throw new ArgumentException("Не задан Токен для Telegram-бота");
            });

            //база данных
            services.AddSingleton<IDataContextFactory<DBContext>>(sp =>
            {
                string connectionString = ConfigurationManager.ConnectionStrings["localhost"].ConnectionString;
                return new DataContextFactory(connectionString);
            });

            // Обработчики сообщений
            // Автоматически регистрируем все классы, наследующие от BaseHandler
            var botHandlersTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseHandler)));

            foreach (var bht in botHandlersTypes)
            {
                services.AddScoped(bht);
            }

            //репозитории
            services.AddScoped<IUsersRepository, UsersSqlRepository>();
            services.AddScoped<IGamesRepository, GamesSqlRepository>();

            //сервисы
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<IGamesService, GamesService>();
            services.AddScoped<IMessageService, TelegramMessageService>();

            //Views
            // Автоматически регистрируем все классы, наследующие от BaseView
            var viewTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(BaseView)));

            foreach (var viewType in viewTypes)
            {
                services.AddScoped(viewType);
            }

            //GamesManager
            services.AddSingleton<GameSessionManager>();
            services.AddHostedService<GameSessionManager>();
            services.AddTransient<GameSession>();
            services.AddScoped<GameFabric>();

            // фоновый сервис для бота
            services.AddHostedService<BotBackgroundService>();
            services.AddHostedService<GamesManagerBackGroundService>();
        }
    }
}
