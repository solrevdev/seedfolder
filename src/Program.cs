using McMaster.Extensions.CommandLineUtils;
using Figgle;
using System.Text;
using System.Reflection;

namespace solrevdev.seedfolder
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            ShowHeader();

            WriteLine($"▲   Running in the path {Directory.GetCurrentDirectory()}");

            var folderName = "";

            if (args?.Length > 0)
            {
                var opts = new[] { "--help", "-h", "-?", "--version", "-v" };
                if (opts.Contains(args[0].ToLower()))
                {
                    ShowHelp();
                    return;
                }

                WriteLine($"▲   Found {args?.Length} params to process. ");
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

            if (string.IsNullOrWhiteSpace(folderName))
            {
                WriteLine("▲   You must enter a folder name.", ConsoleColor.DarkRed);
                return;
            }
            else
            {
                folderName = RemoveSpaces(folderName);
                folderName = SafeNameForFileSystem(folderName);
                sb.Append(folderName);
            }

            var finalFolderName = sb.ToString();
            if (Directory.Exists(finalFolderName))
            {
                WriteLine($"▲   Sorry but {finalFolderName} already exists.", ConsoleColor.DarkRed);
                return;
            }

            WriteLine($"‍▲   Creating the directory {finalFolderName}");
            Directory.CreateDirectory(finalFolderName);

            WriteLine($"‍▲   Copying .dockerignore to {finalFolderName}{Path.DirectorySeparatorChar}.dockerignore");
            await WriteFile("dockerignore", $"{finalFolderName}{Path.DirectorySeparatorChar}.dockerignore").ConfigureAwait(false);

            WriteLine($"‍▲   Copying .editorconfig to {finalFolderName}{Path.DirectorySeparatorChar}.editorconfig");
            await WriteFile("editorconfig", $"{finalFolderName}{Path.DirectorySeparatorChar}.editorconfig").ConfigureAwait(false);

            WriteLine($"‍▲   Copying .gitattributes to {finalFolderName}{Path.DirectorySeparatorChar}.gitattributes");
            await WriteFile("gitattributes", $"{finalFolderName}{Path.DirectorySeparatorChar}.gitattributes").ConfigureAwait(false);

            WriteLine($"‍▲   Copying .gitignore to {finalFolderName}{Path.DirectorySeparatorChar}.gitignore");
            await WriteFile("gitignore", $"{finalFolderName}{Path.DirectorySeparatorChar}.gitignore").ConfigureAwait(false);

            WriteLine($"‍▲   Copying .prettierignore to {finalFolderName}{Path.DirectorySeparatorChar}.prettierignore");
            await WriteFile("prettierignore", $"{finalFolderName}{Path.DirectorySeparatorChar}.prettierignore").ConfigureAwait(false);

            WriteLine($"‍▲   Copying .prettierrc to {finalFolderName}{Path.DirectorySeparatorChar}.prettierrc");
            await WriteFile("prettierrc", $"{finalFolderName}{Path.DirectorySeparatorChar}.prettierrc").ConfigureAwait(false);

            WriteLine($"‍▲   Copying omnisharp.json to {finalFolderName}{Path.DirectorySeparatorChar}omnisharp.json");
            await WriteFile("omnisharp.json", $"{finalFolderName}{Path.DirectorySeparatorChar}omnisharp.json").ConfigureAwait(false);

            WriteLine("▲   Done!");
        }

        private static void ShowHelp()
        {
            var version = typeof(Program).GetTypeInfo().Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;

            WriteLine($"▲   Version {version}");
            const string help = @"▲   Usage: dotnet run [folderName]

Passing no folderName will then interactively ask you for the folder name. otherwise it will use the folderName you pass and create a new directory in your current folder.

For example:

seedfolder
▲   Do you want to prefix the folder with the date? [Y/n] y
▲   What do you want the folder to be named? temp
▲   Creating the directory 2020-12-10_temp
▲   Done!

seedfolder
▲   Do you want to prefix the folder with the date? [Y/n] n
▲   What do you want the folder to be named? temp
▲   Creating the directory temp
▲   Done!

seedfolder temp
▲   Found 1 params to process.
▲   Creating the directory temp
▲   Done!

seedfolder will also copy various dotfiles to that folder.
";
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

        private static async Task WriteFile(string filename, string destination)
        {
            var assembly = Assembly.GetEntryAssembly();
            var resourceStream = assembly.GetManifestResourceStream($"solrevdev.seedfolder.Data.{filename}");
            if (resourceStream != null)
            {
                using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                {
                    var fileContents = await reader.ReadToEndAsync().ConfigureAwait(false);
                    File.WriteAllText(destination, fileContents);
                }
            }
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
}
