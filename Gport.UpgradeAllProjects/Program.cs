using System.CommandLine;
using System.Diagnostics;
using System.Reflection;

if (args.Length == 0)
{
    var versionString = Assembly.GetEntryAssembly()?
                            .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?
                            .InformationalVersion
                            .ToString();

    Console.WriteLine($"upgrade-all-projects v{versionString}");
    Console.WriteLine("-------------");
    Console.WriteLine("\nUsage:");
    Console.WriteLine("upgrade-all-projects sdk --solutionDirectory <path>");
    return;
}
var root = new RootCommand
{
    Name = "upgrade-all-projects"
};
var path = new Option<DirectoryInfo>("--solutionDirectory", "The toplevel directory to search for .csproj files");
var sdk = new Command("sdk");
sdk.AddOption(path);
sdk.SetHandler(async solutionDirectory => {
    var matchingFiles = Directory.EnumerateFiles(solutionDirectory.FullName, "*.csproj", SearchOption.AllDirectories)
                                 .ToArray(); 
    foreach (var file in matchingFiles)
    {
        Console.WriteLine(file);
    }
    Console.WriteLine("Identified {0} C# projects", matchingFiles.Length);
    Console.Write("Would you like to attempt to upgrade all projects to .NET SDK-style? (Y/N): ");
    var confirm = Console.ReadLine();
    if (String.Equals(confirm?.ToUpperInvariant(), "Y", StringComparison.OrdinalIgnoreCase))
    {
        foreach (var file in matchingFiles)
        {
            await Process.Start("upgrade-assistant", new[] {"upgrade", $"{file}", $"--operation", "feature.sdkstyle", "--non-interactive" } ).WaitForExitAsync();
        }
    }
}, path);

root.AddCommand(sdk);
await root.InvokeAsync(args); 