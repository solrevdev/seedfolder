using McMaster.Extensions.CommandLineUtils;
using Figgle;
using System.Text;
using System.Reflection;
using System.Globalization;

namespace solrevdev.seedfolder;

// Template metadata structure for future extensibility
internal record TemplateFile(string ResourceName, string FileName, string Description = "");

// Enum for supported project types
internal enum ProjectType
{
    Dotnet,
    Node,
    Python,
    Ruby,
    Markdown,
    Universal
}

internal static class Program
{
    public static async Task Main(string[] args)
    {
        var folderName = "";
        var projectType = ProjectType.Dotnet; // Default to dotnet
        var isDryRun = false;
        var isForce = false;
        var isQuiet = false;

        if (args?.Length > 0)
        {
            var argIndex = 0;
            while (argIndex < args.Length)
            {
                var arg = args[argIndex].ToLower(CultureInfo.InvariantCulture);
                
                // Handle flags
                if (arg is "--help" or "-h" or "-?")
                {
                    ShowHelp();
                    return;
                }
                
                if (arg is "--version" or "-v")
                {
                    ShowVersion();
                    return;
                }

                if (arg is "--list-templates" or "--list")
                {
                    ShowTemplates();
                    return;
                }

                if (arg is "--dry-run" or "--dry")
                {
                    isDryRun = true;
                    argIndex++;
                    continue;
                }

                if (arg is "--force" or "-f")
                {
                    isForce = true;
                    argIndex++;
                    continue;
                }

                if (arg is "--quiet" or "-q")
                {
                    isQuiet = true;
                    argIndex++;
                    continue;
                }

                if (arg is "--template" or "--type" or "-t")
                {
                    if (argIndex + 1 >= args.Length)
                    {
                        WriteLine("▲   Error: --template requires a template type.", ConsoleColor.DarkRed);
                        WriteLine("▲   Available types: dotnet, node, python, ruby, markdown, universal", ConsoleColor.DarkYellow);
                        return;
                    }

                    var templateArg = args[argIndex + 1].ToLower(CultureInfo.InvariantCulture);
                    if (!TryParseProjectType(templateArg, out projectType))
                    {
                        WriteLine($"▲   Error: Unknown template type '{args[argIndex + 1]}'.", ConsoleColor.DarkRed);
                        WriteLine("▲   Available types: dotnet, node, python, ruby, markdown, universal", ConsoleColor.DarkYellow);
                        return;
                    }

                    argIndex += 2;
                    
                    // Get folder name if provided
                    if (argIndex < args.Length)
                    {
                        folderName = args[argIndex];
                        argIndex++;
                    }

                    if (!isQuiet)
                        WriteLine($"▲   Using template type: {projectType}");
                    break;
                }
                else
                {
                    // This is the folder name
                    folderName = args[argIndex];
                    argIndex++;
                    break;
                }
            }
        }

        if (!isQuiet)
        {
            ShowHeader();
            WriteLine($"▲   Running in the path {Directory.GetCurrentDirectory()}");
        }

        var sb = new StringBuilder();
        if (string.IsNullOrWhiteSpace(folderName))
        {
            // Interactive template selection
            if (projectType == ProjectType.Dotnet) // Only prompt if no template was specified
            {
                if (!isQuiet)
                {
                    WriteLine("▲   Available project templates:");
                    WriteLine("    1. dotnet    - .NET project with standard dotfiles");
                    WriteLine("    2. node      - Node.js project with package.json");
                    WriteLine("    3. python    - Python project with requirements.txt");
                    WriteLine("    4. ruby      - Ruby project with Gemfile");
                    WriteLine("    5. markdown  - Documentation project with README");
                    WriteLine("    6. universal - Basic project with minimal files");
                    WriteLine("");
                }

                var templateChoice = Prompt.GetString("▲   Select template type (1-6) or press Enter for dotnet", "1");
                
                projectType = templateChoice switch
                {
                    "2" or "node" => ProjectType.Node,
                    "3" or "python" => ProjectType.Python,
                    "4" or "ruby" => ProjectType.Ruby,
                    "5" or "markdown" => ProjectType.Markdown,
                    "6" or "universal" => ProjectType.Universal,
                    _ => ProjectType.Dotnet
                };

                if (!isQuiet)
                    WriteLine($"▲   Selected template: {projectType}");
            }

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
            if (!isForce)
            {
                WriteLine($"▲   Error: Directory '{finalFolderName}' already exists.", ConsoleColor.DarkRed);
                WriteLine("▲   Use --force to overwrite existing directory.", ConsoleColor.DarkYellow);
                return;
            }
            else if (!isQuiet)
            {
                WriteLine($"▲   Warning: Directory '{finalFolderName}' exists, will overwrite files.", ConsoleColor.DarkYellow);
            }
        }

        // Define template files based on project type
        var templateFiles = GetTemplateFiles(projectType);

        if (isDryRun)
        {
            WriteLine($"▲   DRY RUN: Would create directory '{finalFolderName}' with template '{projectType}'", ConsoleColor.Cyan);
            WriteLine("▲   Files that would be created:", ConsoleColor.Cyan);
            foreach (var templateFile in templateFiles)
            {
                var destinationPath = Path.Combine(finalFolderName, templateFile.FileName);
                WriteLine($"    • {destinationPath}", ConsoleColor.Cyan);
            }
            WriteLine("▲   Use without --dry-run to actually create the files.", ConsoleColor.Cyan);
            return;
        }

        // Create directory with error handling
        if (!isQuiet)
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
        foreach (var templateFile in templateFiles)
        {
            var destinationPath = Path.Combine(finalFolderName, templateFile.FileName);
            
            if (!isQuiet)
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

        if (!isQuiet)
            WriteLine("▲   Done!");
    }

    private static bool TryParseProjectType(string input, out ProjectType projectType)
    {
        projectType = input switch
        {
            "dotnet" or "net" or "csharp" => ProjectType.Dotnet,
            "node" or "nodejs" or "javascript" or "js" => ProjectType.Node,
            "python" or "py" => ProjectType.Python,
            "ruby" or "rb" => ProjectType.Ruby,
            "markdown" or "md" or "docs" => ProjectType.Markdown,
            "universal" or "basic" or "minimal" => ProjectType.Universal,
            _ => ProjectType.Dotnet
        };
        
        return input is "dotnet" or "net" or "csharp" or "node" or "nodejs" or "javascript" or "js" 
            or "python" or "py" or "ruby" or "rb" or "markdown" or "md" or "docs" 
            or "universal" or "basic" or "minimal";
    }

    private static TemplateFile[] GetTemplateFiles(ProjectType projectType)
    {
        return projectType switch
        {
            ProjectType.Node => GetNodeTemplate(),
            ProjectType.Python => GetPythonTemplate(),
            ProjectType.Ruby => GetRubyTemplate(),
            ProjectType.Markdown => GetMarkdownTemplate(),
            ProjectType.Universal => GetUniversalTemplate(),
            _ => GetDotnetTemplate()
        };
    }

    private static TemplateFile[] GetDotnetTemplate()
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

    private static TemplateFile[] GetNodeTemplate()
    {
        return new TemplateFile[]
        {
            new("package.json", "package.json", "Node.js package configuration"),
            new("index.js", "index.js", "Main application entry point"),
            new("gitignore-node", ".gitignore", "Node.js specific git ignore patterns"),
            new("editorconfig", ".editorconfig", "Editor configuration"),
            new("prettierignore", ".prettierignore", "Prettier ignore patterns"),
            new("prettierrc", ".prettierrc", "Prettier configuration")
        };
    }

    private static TemplateFile[] GetPythonTemplate()
    {
        return new TemplateFile[]
        {
            new("main.py", "main.py", "Main application entry point"),
            new("requirements.txt", "requirements.txt", "Python dependencies"),
            new("gitignore-python", ".gitignore", "Python specific git ignore patterns"),
            new("editorconfig", ".editorconfig", "Editor configuration")
        };
    }

    private static TemplateFile[] GetRubyTemplate()
    {
        return new TemplateFile[]
        {
            new("Gemfile", "Gemfile", "Ruby dependencies"),
            new("main.rb", "main.rb", "Main application entry point"),
            new("gitignore-ruby", ".gitignore", "Ruby specific git ignore patterns"),
            new("editorconfig", ".editorconfig", "Editor configuration")
        };
    }

    private static TemplateFile[] GetMarkdownTemplate()
    {
        return new TemplateFile[]
        {
            new("README.md", "README.md", "Project documentation"),
            new("gitignore-markdown", ".gitignore", "Documentation specific git ignore patterns"),
            new("editorconfig", ".editorconfig", "Editor configuration")
        };
    }

    private static TemplateFile[] GetUniversalTemplate()
    {
        return new TemplateFile[]
        {
            new("README.md", "README.md", "Project documentation"),
            new("gitignore", ".gitignore", "Basic git ignore patterns"),
            new("editorconfig", ".editorconfig", "Editor configuration")
        };
    }

    private static TemplateFile[] GetDefaultTemplate()
    {
        return GetDotnetTemplate();
    }

    private static void ShowTemplates()
    {
        WriteLine("▲   Available project templates:");
        WriteLine("");
        
        var templates = new[]
        {
            ("dotnet", "Dotnet project with standard dotfiles", GetDotnetTemplate()),
            ("node", "Node.js project with package.json", GetNodeTemplate()),
            ("python", "Python project with requirements.txt", GetPythonTemplate()),
            ("ruby", "Ruby project with Gemfile", GetRubyTemplate()),
            ("markdown", "Documentation project with README", GetMarkdownTemplate()),
            ("universal", "Basic project with minimal files", GetUniversalTemplate())
        };

        foreach (var (name, description, files) in templates)
        {
            WriteLine($"  {name,-12} - {description}");
            foreach (var file in files)
            {
                WriteLine($"    • {file.FileName,-20} {file.Description}");
            }
            WriteLine("");
        }
        
        WriteLine("▲   Usage examples:");
        WriteLine("    seedfolder --template node myproject");
        WriteLine("    seedfolder -t python myapp");
        WriteLine("    seedfolder --type ruby mygem");
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
  --help, -h, -?           Show this help message
  --version, -v            Show version information
  --list-templates         Show available template files
  --template, --type, -t   Specify project template type
  --dry-run, --dry         Preview operations without creating files
  --force, -f              Overwrite existing directory and files
  --quiet, -q              Suppress output (useful for scripting)

Arguments:
  folderName              Name of the folder to create (optional)

Template Types:
  dotnet                  .NET project with standard dotfiles (default)
  node                    Node.js project with package.json
  python                  Python project with requirements.txt
  ruby                    Ruby project with Gemfile
  markdown                Documentation project with README
  universal               Basic project with minimal files

If no folder name is provided, seedfolder will interactively ask for the folder name
and whether to prefix it with the current date.

Examples:

  seedfolder                              # Interactive mode with template selection
  seedfolder myproject                    # Create 'myproject' folder with dotnet template
  seedfolder --template node myapp        # Create Node.js project
  seedfolder -t python ""my project""       # Create Python project (spaces converted to dashes)
  seedfolder --type ruby mygem            # Create Ruby project
  seedfolder --dry-run -t node myapp      # Preview Node.js project creation
  seedfolder --force myproject            # Overwrite existing 'myproject' directory
  seedfolder --quiet -t python myapp      # Create Python project with no output";
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
