using System.Diagnostics ;
using CliWrap ;

namespace Idasen.RestApi.Service ;

public class ServiceInstaller
{
    public async Task Uninstall()
    {
        await StopService ( ) ;
        await DeleteService ( ) ;
    }

    private static async Task DeleteService ( )
    {
        try
        {
            await Cli.Wrap ( "sc" )
                     .WithArguments ( new [ ]
                                      {
                                          "delete" ,
                                          ServiceName
                                      } )
                     .ExecuteAsync ( ) ;
        }
        catch ( Exception e )
        {
            Console.WriteLine ( e.Message ) ;

            Environment.Exit ( 1 ) ;
        }
    }

    private static async Task StopService ( )
    {
        try
        {
            await Cli.Wrap ( "sc" )
                     .WithArguments ( new [ ]
                                      {
                                          "stop" ,
                                          ServiceName
                                      } )
                     .ExecuteAsync ( ) ;
        }
        catch ( Exception e )
        {
            // do nothing, most likely service is not running
        }
    }

    private static async Task StartService()
    {
        try
        {
            await Cli.Wrap("sc")
                     .WithArguments(new[]
                                    {
                                        "start" ,
                                        ServiceName
                                    })
                     .ExecuteAsync();
        }
        catch (Exception e)
        {
            // do nothing, most likely service is not running
        }
    }

    public async Task Install()
    {
        try
        {
            var filenameExe = Process.GetCurrentProcess().MainModule?.FileName;

            if (filenameExe == null)
                throw new Exception("Failed to get *.exe filename");

            var executablePath =
                Path.Combine(AppContext.BaseDirectory,
                             filenameExe);

            await CreateService ( executablePath ) ;

            await StartService ( ) ;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.ReadLine();

            Environment.Exit(1);
        }
    }

    private static async Task CreateService ( string executablePath )
    {
        await Cli.Wrap ( "sc.exe" )
                 .WithArguments ( new [ ]
                                  {
                                      "create" ,
                                      ServiceName ,
                                      $"binpath={executablePath}" ,
                                      "start=auto"
                                  } )
                 .ExecuteAsync ( ) ;
    }

    public const string ServiceName = "Idasen RestAPI Service" ;
}