using McMaster.Extensions.CommandLineUtils;
using Figgle;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace solrevdev.seedfolder;

// Template metadata structure for future extensibility
internal record TemplateFile(string ResourceName, string FileName, string Description = "");

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

            // Handle list templates (foundation for future template system)
            if (firstArg is "--list-templates" or "--list")
            {
                ShowTemplates();
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

        // Define default template files (foundation for future template system)
        var defaultTemplate = GetDefaultTemplate();

        // Copy template files using cross-platform path handling
        foreach (var templateFile in defaultTemplate)
        {
            var destinationPath = Path.Combine(finalFolderName, templateFile.FileName);
            WriteLine($"‍▲   Copying {templateFile.FileName} to {destinationPath}");
            
            try
            {
                await WriteFileAsync(templateFile.ResourceName, destinationPath).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                WriteLine($"▲   Error copying {templateFile.FileName}: {ex.Message}", ConsoleColor.DarkRed);
                return;
            }
        }

        WriteLine("▲   Done!");
    }

    private static TemplateFile[] GetDefaultTemplate()
    {
        return new TemplateFile[]
        {
            new("dockerignore", ".dockerignore", "Docker ignore patterns"),
            new("editorconfig", ".editorconfig", "Editor configuration"),
            new("gitattributes", ".gitattributes", "Git attributes"),
            new("gitignore", ".gitignore", "Git ignore patterns"),
            new("prettierignore", ".prettierignore", "Prettier ignore patterns"),
            new("prettierrc", ".prettierrc", "Prettier configuration"),
            new("omnisharp.json", "omnisharp.json", "OmniSharp configuration")
        };
    }

    private static void ShowTemplates()
    {
        WriteLine("▲   Available template files:");
        WriteLine("");
        
        var templates = GetDefaultTemplate();
        foreach (var template in templates)
        {
            WriteLine($"  • {template.FileName,-20} {template.Description}");
        }
        
        WriteLine("");
        WriteLine("▲   Note: Template system will be expanded in future versions to support");
        WriteLine("▲         different project types (node, python, ruby, etc.)");
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
  --help, -h, -?        Show this help message
  --version, -v         Show version information
  --list-templates      Show available template files

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
        if (string.IsNullOrWhiteSpace(name))
            return string.Empty;
            
        var invalids = Path.GetInvalidFileNameChars();
        var result = new string(name.Select(c => invalids.Contains(c) ? replace : c).ToArray());
        
        // Remove any leading/trailing dashes and handle edge cases
        result = result.Trim(replace);
        
        // Ensure we don't end up with an empty string after cleaning
        return string.IsNullOrWhiteSpace(result) ? string.Empty : result;
    }
}
