
using Autofac;
using Autofac.Extension;
using Microsoft.Extensions.Logging;
using NexusCataclysm.Core;
using NLog.Config;
using NLog.Extensions.Logging;
using Pillar;
using System.Diagnostics;
using System.Reflection;
using Utopia.Core;

namespace Sample;

public partial class Launcher
{

    [STAThread]
    private static int Main(string[] args)
    {
        Thread.CurrentThread.Name = "main";
        ExtendedHost? host = null;
        try
        {
            host = CreateWithArguments(args).Launch();
            host.StartInCurrentThread();
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while running the application.");
            Console.WriteLine(ex.ToColoredStringDemystified());
            return 1;
        }
        finally
        {
            host?.StopApplication();
        }
        return 0;
    }

    private Launcher()
    {
        HomeDirectory = Path.GetFullPath(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
            ?? ".");

        PluginDirectory = Path.Combine(HomeDirectory, "plugins");
        ConfigurationDirectory = Path.Combine(HomeDirectory, "configs");
        EnvironmentName = GetInitialEnvironmentName();
    }

    public string HomeDirectory { get; set; }

    public string PluginDirectory { get; set; }

    public string ConfigurationDirectory { get; set; }

    public string EnvironmentName { get; set; }

    protected virtual void CreateDirectories()
    {
        if (!Directory.Exists(PluginDirectory))
        {
            Directory.CreateDirectory(PluginDirectory);
        }
        if (!Directory.Exists(ConfigurationDirectory))
        {
            Directory.CreateDirectory(ConfigurationDirectory);
        }
    }

    protected virtual IEnumerable<string> GetPluginAssemblies()
    {
        return Directory.GetFiles(PluginDirectory, "*.dll", SearchOption.AllDirectories);
    }

    protected virtual string GetInitialEnvironmentName()
    {
        var envName =
#if DEBUG
            "Development"
#else
			"Production"
#endif
            ;
        return envName;
    }

    protected virtual ExtendedHostBuilder CreateDefaultBuilder()
    {
        var builder = new ExtendedHostBuilder(new ExtendedHostEnvironment(
            "sample",
            HomeDirectory,
            EnvironmentName));

        builder.RegisterHost<Client>();

        builder.Builder.AddOptions();

        builder.Builder.Register(context =>
        {
            return Logging.CreateDefaultConfiguration();
        }).As<LoggingConfiguration>().SingleInstance();

        builder.Builder.Register(context =>
        {
            return Logging.CreateDefaultOptions();
        }).As<NLogProviderOptions>().SingleInstance();

        builder.Builder.AddLogging(context =>
        {
            var configuration = context.Resolve<LoggingConfiguration>();
            var options = context.Resolve<NLogProviderOptions>();
            return (loggingBuilder) =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddNLog(configuration, options);
            };
        });

        builder.Builder.Configure<ClientOptions>(null, (options) =>
        {
            options.HomeDirectory = HomeDirectory;
            options.PluginDirectory = PluginDirectory;
            options.ConfigurationDirectory = ConfigurationDirectory;
        });

        return builder;
    }

    protected virtual void Initialize(ExtendedHostBuilder builder)
    {
        builder.Builder.RegisterType<FNAGame>().AsSelf().SingleInstance();
        return;
    }

    public ExtendedHost Launch()
    {
        CreateDirectories();
        var builder = CreateDefaultBuilder();
        Initialize(builder);
        return (ExtendedHost)builder.Build();
    }

    public static Launcher CreateWithArguments(string[] args)
    {
        var launcher = Create();

        int index = 0;

        while (index != args.Length)
        {
            var arg = args[index];

            if (arg.StartsWith("--home="))
            {
                launcher.HomeDirectory = arg["--home=".Length..].Trim('"', '\'');
            }
            else if (arg.StartsWith("--plugin="))
            {
                launcher.PluginDirectory = arg["--plugin=".Length..].Trim('"', '\'');
            }
            else if (arg.StartsWith("--config="))
            {
                launcher.ConfigurationDirectory = arg["--config=".Length..].Trim('"', '\'');
            }
            else
            {
                throw new ArgumentException("unknown argument: " + arg);
            }

            index++;
        }

        return launcher;
    }

    public static Launcher Create()
    {
        var launcher = new Launcher();
        return launcher;
    }
}
