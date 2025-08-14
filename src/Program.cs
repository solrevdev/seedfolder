using McMaster.Extensions.CommandLineUtils;
using Figgle;
using System.Text;
using System.Reflection;
using System.Globalization;
namespace solrevdev.seedfolder;

internal static class Program
{
    public static async Task Main(string[] args)
    {
        ShowHeader();

        WriteLine($"▲   Running in the path {Directory.GetCurrentDirectory()}");

        var folderName = "";

        if (args?.Length > 0)
        {
            var firstArg = args[0].ToLower(CultureInfo.InvariantCulture);
            
            // Handle help flags
            if (firstArg is "--help" or "-h" or "-?")
            {
                ShowHelp();
                return;
            }
            
            // Handle version flags  
            if (firstArg is "--version" or "-v")
            {
                ShowVersion();
                return;
            }

            WriteLine($"▲   Found {args.Length} params to process.");
            folderName = args[0];
        }

        var sb = new StringBuilder();
        if (string.IsNullOrWhiteSpace(folderName))
        {
            var prefixWithDate = Prompt.GetYesNo("▲   Do you want to prefix the folder with the date?", defaultAnswer: true);
            if (prefixWithDate)
            {
                sb.Append(DateTime.Now.Year).Append('-').AppendFormat("{0:d2}", DateTime.Now.Month).Append('-').AppendFormat("{0:d2}", DateTime.Now.Day);
                sb.Append('_');
            }

            folderName = Prompt.GetString("▲   What do you want the folder to be named?");
        }

        // Validate and sanitize folder name
        if (string.IsNullOrWhiteSpace(folderName))
        {
            WriteLine("▲   Error: You must enter a folder name.", ConsoleColor.DarkRed);
            return;
        }

        folderName = RemoveSpaces(folderName);
        folderName = SafeNameForFileSystem(folderName);
        
        if (string.IsNullOrWhiteSpace(folderName))
        {
            WriteLine("▲   Error: Folder name contains only invalid characters.", ConsoleColor.DarkRed);
            return;
        }
        
        sb.Append(folderName);

        var finalFolderName = sb.ToString();
        
        // Check if directory already exists
        if (Directory.Exists(finalFolderName))
        {
            WriteLine($"▲   Error: Directory '{finalFolderName}' already exists.", ConsoleColor.DarkRed);
            WriteLine("▲   Please choose a different name or remove the existing directory.", ConsoleColor.DarkYellow);
            return;
        }

        // Create directory with error handling
        WriteLine($"‍▲   Creating the directory {finalFolderName}");
        try
        {
            Directory.CreateDirectory(finalFolderName);
        }
        catch (Exception ex)
        {
            WriteLine($"▲   Error creating directory: {ex.Message}", ConsoleColor.DarkRed);
            return;
        }

        // Copy template files using cross-platform path handling
        var templateFiles = new[]
        {
            ("dockerignore", ".dockerignore"),
            ("editorconfig", ".editorconfig"),
            ("gitattributes", ".gitattributes"),
            ("gitignore", ".gitignore"),
            ("prettierignore", ".prettierignore"),
            ("prettierrc", ".prettierrc"),
            ("omnisharp.json", "omnisharp.json")
        };

        foreach (var (resourceName, fileName) in templateFiles)
        {
            var destinationPath = Path.Combine(finalFolderName, fileName);
            WriteLine($"‍▲   Copying {fileName} to {destinationPath}");
            
            try
            {
                await WriteFileAsync(resourceName, destinationPath).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                WriteLine($"▲   Error copying {fileName}: {ex.Message}", ConsoleColor.DarkRed);
                return;
            }
        }

        WriteLine("▲   Done!");
    }

    private static void ShowVersion()
    {
        var version = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "Unknown";
        WriteLine($"▲   seedfolder version {version}");
    }

    private static void ShowHelp()
    {
        var version = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "Unknown";

        WriteLine($"▲   seedfolder version {version}");
        WriteLine("");
        const string help = @"▲   Usage: seedfolder [options] [folderName]

Options:
  --help, -h, -?     Show this help message
  --version, -v      Show version information

Arguments:
  folderName         Name of the folder to create (optional)

If no folder name is provided, seedfolder will interactively ask for the folder name
and whether to prefix it with the current date.

Examples:

  seedfolder                    # Interactive mode
  seedfolder myproject          # Create 'myproject' folder  
  seedfolder ""my project""       # Create 'my-project' folder (spaces converted to dashes)

seedfolder creates a new directory and copies standard dotfiles into it:
  • .dockerignore
  • .editorconfig  
  • .gitattributes
  • .gitignore
  • .prettierignore
  • .prettierrc
  • omnisharp.json";
        WriteLine(help);
    }

    private static void WriteLine(string text, ConsoleColor color = default)
    {
        if (color == default)
        {
            Console.WriteLine(text);
        }
        else
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
    }

    private static async Task WriteFileAsync(string filename, string destination)
    {
        var assembly = Assembly.GetEntryAssembly();
        var resourceName = $"solrevdev.seedfolder.Data.{filename}";
        var resourceStream = assembly?.GetManifestResourceStream(resourceName);
        
        if (resourceStream == null)
        {
            throw new InvalidOperationException($"Could not find embedded resource: {resourceName}");
        }

        using var reader = new StreamReader(resourceStream, Encoding.UTF8);
        var fileContents = await reader.ReadToEndAsync().ConfigureAwait(false);
        
        // Ensure destination directory exists
        var destinationDir = Path.GetDirectoryName(destination);
        if (!string.IsNullOrEmpty(destinationDir) && !Directory.Exists(destinationDir))
        {
            Directory.CreateDirectory(destinationDir);
        }
        
        await File.WriteAllTextAsync(destination, fileContents).ConfigureAwait(false);
    }

    private static void ShowHeader()
    {
        var programTitle = FiggleFonts.Standard.Render("seedfolder");
        WriteLine(programTitle, ConsoleColor.DarkGreen);

        AppendBlankLines();
    }

    private static void AppendBlankLines(int howMany = 2)
    {
        for (var i = 0; i <= howMany; i++)
        {
            WriteLine("");
        }
    }

    private static string RemoveSpaces(string name, char replacement = '-') => name.Replace(' ', replacement);

    private static string SafeNameForFileSystem(string name, char replace = '-')
    {
        var invalids = Path.GetInvalidFileNameChars();
        return new string(name.Select(c => invalids.Contains(c) ? replace : c).ToArray());
    }
}
