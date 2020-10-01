using System;
using System.IO;
using McMaster.Extensions.CommandLineUtils;
using Figgle;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace solrevdev.seedfolder
{
    internal static class Program
    {
        public static async Task Main(string[] args)
        {
            ShowHeader();

            if (args?.Length > 0)
            {
                Console.WriteLine($"▲   Found {args?.Length} params to process");
            }

            Console.WriteLine($"▲   Running in the path {Directory.GetCurrentDirectory()}");

            var sb = new StringBuilder();
            var trelloCard = Prompt.GetString("▲   Enter the Trello card reference or leave empty if none exists");
            var folderName = Prompt.GetString("▲   Enter the folder name to appear after the Trello reference if exists");

            if (string.IsNullOrWhiteSpace(trelloCard))
            {
                sb.Append(DateTime.Now.Year).Append('-').AppendFormat("{0:d2}", DateTime.Now.Month).Append('-').AppendFormat("{0:d2}", DateTime.Now.Day);
            }
            else
            {
                sb.Append(trelloCard.ToUpper());
            }

            sb.Append("_");

            if (string.IsNullOrWhiteSpace(folderName))
            {
                Console.WriteLine("▲   You must enter a folder name.", System.Drawing.Color.DarkRed);
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
                Console.WriteLine($"▲   Sorry but {finalFolderName} already exists.", System.Drawing.Color.DarkRed);
                return;
            }

            Console.WriteLine($"‍▲   Creating the directory {finalFolderName}");
            Directory.CreateDirectory(finalFolderName);

            Console.WriteLine($"‍▲   Copying .dockerignore to {finalFolderName}{Path.DirectorySeparatorChar}.dockerignore");
            await WriteFile("dockerignore", $"{finalFolderName}{Path.DirectorySeparatorChar}.dockerignore").ConfigureAwait(false);

            Console.WriteLine($"‍▲   Copying .editorconfig to {finalFolderName}{Path.DirectorySeparatorChar}.editorconfig");
            await WriteFile("editorconfig", $"{finalFolderName}{Path.DirectorySeparatorChar}.editorconfig").ConfigureAwait(false);

            Console.WriteLine($"‍▲   Copying .gitattributes to {finalFolderName}{Path.DirectorySeparatorChar}.gitattributes");
            await WriteFile("gitattributes", $"{finalFolderName}{Path.DirectorySeparatorChar}.gitattributes").ConfigureAwait(false);

            Console.WriteLine($"‍▲   Copying .gitignore to {finalFolderName}{Path.DirectorySeparatorChar}.gitignore");
            await WriteFile("gitignore", $"{finalFolderName}{Path.DirectorySeparatorChar}.gitignore").ConfigureAwait(false);

            Console.WriteLine($"‍▲   Copying .prettierignore to {finalFolderName}{Path.DirectorySeparatorChar}.prettierignore");
            await WriteFile("prettierignore", $"{finalFolderName}{Path.DirectorySeparatorChar}.prettierignore").ConfigureAwait(false);

            Console.WriteLine($"‍▲   Copying .prettierrc to {finalFolderName}{Path.DirectorySeparatorChar}.prettierrc");
            await WriteFile("prettierrc", $"{finalFolderName}{Path.DirectorySeparatorChar}.prettierrc").ConfigureAwait(false);

            Console.WriteLine($"‍▲   Copying omnisharp.json to {finalFolderName}{Path.DirectorySeparatorChar}omnisharp.json");
            await WriteFile("omnisharp.json", $"{finalFolderName}{Path.DirectorySeparatorChar}omnisharp.json").ConfigureAwait(false);

            Console.WriteLine("▲   Done!");
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
            Console.WriteLine(programTitle, System.Drawing.Color.DarkGreen);

            WriteLines();
        }

        private static void WriteLines(int howMany = 2)
        {
            for (var i = 0; i <= howMany; i++)
            {
                Console.WriteLine("");
            }
        }

        private static string RemoveSpaces(string name, char replacement = '-')
        {
            return name.Replace(' ', replacement);
        }

        private static string SafeNameForFileSystem(string name, char replace = '-')
        {
            var invalids = Path.GetInvalidFileNameChars();
            return new string(name.Select(c => invalids.Contains(c) ? replace : c).ToArray());
        }
    }
}
