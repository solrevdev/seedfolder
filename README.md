# Solrevdev.SeedFolder

[![GitHub last commit](https://img.shields.io/github/last-commit/solrevdev/seedfolder)](https://github.com/solrevdev/seedfolder) [![CI](https://github.com/solrevdev/seedfolder/workflows/CI/badge.svg)](https://github.com/solrevdev/seedfolder) [![Twitter Follow](https://img.shields.io/twitter/follow/solrevdev?label=Follow&style=social)](https://twitter.com/solrevdev)

```
                     _  __       _     _
  ___  ___  ___  __| |/ _| ___ | | __| | ___ _ __
 / __|/ _ \/ _ \/ _` | |_ / _ \| |/ _` |/ _ \ '__|
 \__ \  __/  __/ (_| |  _| (_) | | (_| |  __/ |
 |___/\___|\___|\__,_|_|  \___/|_|\__,_|\___|_|
```

## Overview

**SeedFolder** is a powerful .NET Global Tool that creates project directories and seeds them with curated template files for different project types. Whether you're starting a .NET project, Node.js app, Python script, Ruby gem, or documentation project, SeedFolder provides the right foundation with just one command.

🚀 **Multi-Template System** - Support for 6 different project types
⚡ **Professional CLI** - Comprehensive command-line interface with dry-run, force mode, and more
🌐 **Cross-Platform** - Works on Windows, macOS, and Linux
🔄 **Interactive Mode** - Guided project creation with template selection
📊 **Progress Tracking** - Real-time feedback with visual progress indicators

## Quick Start

```bash
# Install globally
dotnet tool install --global solrevdev.seedfolder

# Interactive mode - choose template and folder name
seedfolder

# Create specific project types
seedfolder --template node myapp
seedfolder -t python data-analysis
seedfolder --type ruby my_gem

# Preview before creating
seedfolder --dry-run -t node myapp
```

## Supported Project Templates

### 📝 **markdown** - Documentation Project (Default)
Documentation and content projects:
- `README.md` - Project documentation template
- `.gitignore` - Documentation specific git ignore patterns
- `.gitattributes` - Documentation-focused git attributes with LFS for images/videos
- `.editorconfig` - Documentation optimized editor configuration

### 🏗️ **dotnet** - .NET Project
Complete .NET development environment with standard dotfiles:
- `.dockerignore` - Docker ignore patterns
- `.editorconfig` - Comprehensive C# editor configuration with .NET naming conventions
- `.gitattributes` - Comprehensive .NET git attributes with LFS for binaries and C# language detection
- `.gitignore` - .NET specific git ignore patterns
- `.prettierignore` - Prettier ignore patterns
- `.prettierrc` - Prettier configuration
- `omnisharp.json` - OmniSharp configuration

### 📦 **node** - Node.js Project
Modern Node.js project setup:
- `package.json` - Node.js package configuration
- `index.js` - Main application entry point
- `.gitignore` - Node.js specific git ignore patterns
- `.gitattributes` - JavaScript/TypeScript focused git attributes with web asset handling
- `.editorconfig` - JavaScript/TypeScript optimized editor configuration
- `.prettierignore` - Prettier ignore patterns
- `.prettierrc` - Prettier configuration

### 🐍 **python** - Python Project
Python development environment:
- `main.py` - Main application entry point
- `requirements.txt` - Python dependencies
- `.gitignore` - Python specific git ignore patterns
- `.gitattributes` - Python-focused git attributes with LFS for wheels, data files, and ML models
- `.editorconfig` - PEP 8 compliant editor configuration

### 💎 **ruby** - Ruby Project
Ruby development setup:
- `Gemfile` - Ruby dependencies
- `main.rb` - Main application entry point
- `.gitignore` - Ruby specific git ignore patterns
- `.gitattributes` - Ruby-focused git attributes with gem handling and ERB template support
- `.editorconfig` - Ruby standard editor configuration

### 🌐 **universal** - Basic Project
Minimal project setup for any use case:
- `README.md` - Project documentation template
- `.gitignore` - Basic git ignore patterns
- `.gitattributes` - Conservative cross-platform git attributes with basic binary handling
- `.editorconfig` - Universal editor configuration

## Command Line Interface

```
Usage: seedfolder [options] [folderName]

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
  markdown                Documentation project with README (default)
  dotnet                  .NET project with standard dotfiles
  node                    Node.js project with package.json
  python                  Python project with requirements.txt
  ruby                    Ruby project with Gemfile
  universal               Basic project with minimal files
```

## Usage Examples

### Interactive Mode
```bash
# Start interactive mode with template selection
seedfolder

# Example interactive session:
                    _  __       _     _
  ___  ___  ___  __| |/ _| ___ | | __| | ___ _ __
 / __|/ _ \/ _ \/ _` | |_ / _ \| |/ _` |/ _ \ '__|
 \__ \  __/  __/ (_| |  _| (_) | | (_| |  __/ |
 |___/\___|\___|\__,_|_|  \___/|_|\__,_|\___|_|

▲   Running in the path /current/directory
▲   Available project templates:
    1. markdown  - Documentation project with README
    2. dotnet    - .NET project with standard dotfiles
    3. node      - Node.js project with package.json
    4. python    - Python project with requirements.txt
    5. ruby      - Ruby project with Gemfile
    6. universal - Basic project with minimal files

▲   Select template type (1-6) or press Enter for markdown: 3
▲   Selected template: Node
▲   Do you want to prefix the folder with the date? [Y/n] y
▲   What do you want the folder to be named? my-awesome-app
‍▲   Creating the directory 2024-01-15_my-awesome-app
‍▲   [1/7] Copying package.json
    ✅ Created 2024-01-15_my-awesome-app/package.json
‍▲   [2/7] Copying index.js
    ✅ Created 2024-01-15_my-awesome-app/index.js
‍▲   [3/7] Copying .gitignore
    ✅ Created 2024-01-15_my-awesome-app/.gitignore
‍▲   [4/7] Copying .gitattributes
    ✅ Created 2024-01-15_my-awesome-app/.gitattributes
‍▲   [5/7] Copying .editorconfig
    ✅ Created 2024-01-15_my-awesome-app/.editorconfig
‍▲   [6/7] Copying .prettierignore
    ✅ Created 2024-01-15_my-awesome-app/.prettierignore
‍▲   [7/7] Copying .prettierrc
    ✅ Created 2024-01-15_my-awesome-app/.prettierrc
▲   Done!
▲   Successfully created 7 files in '2024-01-15_my-awesome-app' using Node template.

▲   To initialize git and make your first commit, copy and paste these commands:

cd "2024-01-15_my-awesome-app"
git init
git lfs install 2>/dev/null || echo "Git LFS not available"
git add .
git commit -m "Initial commit"
```

### Direct Template Selection
```bash
# Create different project types
seedfolder --template node myapp
▲   Using template type: Node
                    _  __       _     _
  ___  ___  ___  __| |/ _| ___ | | __| | ___ _ __
 / __|/ _ \/ _ \/ _` | |_ / _ \| |/ _` |/ _ \ '__|
 \__ \  __/  __/ (_| |  _| (_) | | (_| |  __/ |
 |___/\___|\___|\__,_|_|  \___/|_|\__,_|\___|_|

▲   Running in the path /current/directory
‍▲   Creating the directory myapp
‍▲   [1/7] Copying package.json
    ✅ Created myapp/package.json
‍▲   [2/7] Copying index.js
    ✅ Created myapp/index.js
‍▲   [3/7] Copying .gitignore
    ✅ Created myapp/.gitignore
‍▲   [4/7] Copying .gitattributes
    ✅ Created myapp/.gitattributes
‍▲   [5/7] Copying .editorconfig
    ✅ Created myapp/.editorconfig
‍▲   [6/7] Copying .prettierignore
    ✅ Created myapp/.prettierignore
‍▲   [7/7] Copying .prettierrc
    ✅ Created myapp/.prettierrc
▲   Done!
▲   Successfully created 7 files in 'myapp' using Node template.

▲   To initialize git and make your first commit, copy and paste these commands:

cd "myapp"
git init
git lfs install 2>/dev/null || echo "Git LFS not available"
git add .
git commit -m "Initial commit"

# Create Python project (spaces converted to dashes)
seedfolder -t python "machine learning project"
▲   Using template type: Python
                    _  __       _     _
  ___  ___  ___  __| |/ _| ___ | | __| | ___ _ __
 / __|/ _ \/ _ \/ _` | |_ / _ \| |/ _` |/ _ \ '__|
 \__ \  __/  __/ (_| |  _| (_) | | (_| |  __/ |
 |___/\___|\___|\__,_|_|  \___/|_|\__,_|\___|_|

▲   Running in the path /current/directory
‍▲   Creating the directory machine-learning-project
‍▲   [1/5] Copying main.py
    ✅ Created machine-learning-project/main.py
‍▲   [2/5] Copying requirements.txt
    ✅ Created machine-learning-project/requirements.txt
‍▲   [3/5] Copying .gitignore
    ✅ Created machine-learning-project/.gitignore
‍▲   [4/5] Copying .gitattributes
    ✅ Created machine-learning-project/.gitattributes
‍▲   [5/5] Copying .editorconfig
    ✅ Created machine-learning-project/.editorconfig
▲   Done!
▲   Successfully created 5 files in 'machine-learning-project' using Python template.

▲   To initialize git and make your first commit, copy and paste these commands:

cd "machine-learning-project"
git init
git lfs install 2>/dev/null || echo "Git LFS not available"
git add .
git commit -m "Initial commit"

# Other examples
seedfolder --type ruby my_gem_name      # Creates Ruby project
seedfolder -t markdown my-docs          # Creates documentation project
seedfolder --template universal basic-project  # Creates universal project

# Using folder name argument (skips interactive mode)
seedfolder myproject                    # Creates markdown project (default)
seedfolder --template python myapp      # Creates Python project
```

### Preview and Force Operations
```bash
# Preview what would be created (dry-run mode)
seedfolder --dry-run -t node myapp
▲   Using template type: Node
                    _  __       _     _
  ___  ___  ___  __| |/ _| ___ | | __| | ___ _ __
 / __|/ _ \/ _ \/ _` | |_ / _ \| |/ _` |/ _ \ '__|
 \__ \  __/  __/ (_| |  _| (_) | | (_| |  __/ |
 |___/\___|\___|\__,_|_|  \___/|_|\__,_|\___|_|

▲   Running in the path /current/directory
▲   DRY RUN: Would create directory 'myapp' with template 'Node'
▲   Files that would be created:
    • myapp/package.json
    • myapp/index.js
    • myapp/.gitignore
    • myapp/.gitattributes
    • myapp/.editorconfig
    • myapp/.prettierignore
    • myapp/.prettierrc
▲   Use without --dry-run to actually create the files.

# Force overwrite existing directory
seedfolder --force existing-directory
▲   Warning: Directory 'existing-directory' exists, will overwrite files.
‍▲   Creating the directory existing-directory
‍▲   [1/4] Copying README.md
    ✅ Created existing-directory/README.md
‍▲   [2/4] Copying .gitignore
    ✅ Created existing-directory/.gitignore
‍▲   [3/4] Copying .gitattributes
    ✅ Created existing-directory/.gitattributes
‍▲   [4/4] Copying .editorconfig
    ✅ Created existing-directory/.editorconfig
▲   Done!
▲   Successfully created 4 files in 'existing-directory' using Markdown template.

▲   To initialize git and make your first commit, copy and paste these commands:

cd "existing-directory"
git init
git lfs install 2>/dev/null || echo "Git LFS not available"
git add .
git commit -m "Initial commit"

# Quiet mode for scripting (no output shown)
seedfolder --quiet -t node myapp
```

### Template Information
```bash
# List all available templates with file details
seedfolder --list-templates
▲   Available project templates:

  markdown     - Documentation project with README
    • README.md            Project documentation
    • .gitignore           Documentation specific git ignore patterns
    • .gitattributes       Git attributes for documentation projects
    • .editorconfig        Editor configuration for Markdown

  dotnet       - Dotnet project with standard dotfiles
    • .dockerignore        Docker ignore patterns
    • .editorconfig        Editor configuration for .NET
    • .gitattributes       Git attributes
    • .gitignore           Git ignore patterns
    • .prettierignore      Prettier ignore patterns
    • .prettierrc          Prettier configuration
    • omnisharp.json       OmniSharp configuration

  node         - Node.js project with package.json
    • package.json         Node.js package configuration
    • index.js             Main application entry point
    • .gitignore           Node.js specific git ignore patterns
    • .gitattributes       Git attributes for Node.js projects
    • .editorconfig        Editor configuration for Node.js
    • .prettierignore      Prettier ignore patterns
    • .prettierrc          Prettier configuration

  python       - Python project with requirements.txt
    • main.py              Main application entry point
    • requirements.txt     Python dependencies
    • .gitignore           Python specific git ignore patterns
    • .gitattributes       Git attributes for Python projects
    • .editorconfig        Editor configuration for Python

  ruby         - Ruby project with Gemfile
    • Gemfile              Ruby dependencies
    • main.rb              Main application entry point
    • .gitignore           Ruby specific git ignore patterns
    • .gitattributes       Git attributes for Ruby projects
    • .editorconfig        Editor configuration for Ruby

  universal    - Basic project with minimal files
    • README.md            Project documentation
    • .gitignore           Basic git ignore patterns
    • .gitattributes       Git attributes for universal projects
    • .editorconfig        Editor configuration for universal projects

▲   Usage examples:
    seedfolder --template node myproject
    seedfolder -t python myapp
    seedfolder --type ruby mygem

# Show version information
seedfolder --version
▲   seedfolder version 1.3.1

# Show help
seedfolder --help
▲   seedfolder version 1.3.1

▲   Usage: seedfolder [options] [folderName]

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
  dotnet                  .NET project with standard dotfiles
  node                    Node.js project with package.json
  python                  Python project with requirements.txt
  ruby                    Ruby project with Gemfile
  markdown                Documentation project with README (default)
  universal               Basic project with minimal files

Examples:
  seedfolder                              # Interactive mode with template selection
  seedfolder myproject                    # Create 'myproject' folder with markdown template
  seedfolder --template node myapp        # Create Node.js project
  seedfolder -t python "my project"       # Create Python project (spaces converted to dashes)
  seedfolder --type ruby mygem            # Create Ruby project
  seedfolder --dry-run -t node myapp      # Preview Node.js project creation
  seedfolder --force myproject            # Overwrite existing 'myproject' directory
  seedfolder --quiet -t python myapp      # Create Python project with no output
```

## Advanced Features

### 🛡️ **Error Handling & Validation**
- **Disk Space Validation** - Checks for minimum 10MB free space before operations
- **Path Validation** - Automatically sanitizes folder names and handles invalid characters
- **Permission Checks** - Graceful handling of permission errors with actionable suggestions
- **Rollback Support** - Failed operations provide clear rollback information

### 📊 **Progress Indicators**
Real-time progress tracking with visual feedback:
```bash
‍▲   [1/5] Copying main.py
    ✅ Created test-python/main.py
‍▲   [2/5] Copying requirements.txt
    ✅ Created test-python/requirements.txt
‍▲   [3/5] Copying .gitignore
    ✅ Created test-python/.gitignore
‍▲   [4/5] Copying .gitattributes
    ✅ Created test-python/.gitattributes
‍▲   [5/5] Copying .editorconfig
    ✅ Created test-python/.editorconfig
▲   Done!
▲   Successfully created 5 files in 'test-python' using Python template.

▲   To initialize git and make your first commit, copy and paste these commands:

cd "test-python"
git init
git lfs install 2>/dev/null || echo "Git LFS not available"
git add .
git commit -m "Initial commit"
```

### 🔄 **Smart Input Handling**
- **Space Conversion** - Spaces in folder names automatically converted to dashes
- **Path Sanitization** - Invalid filesystem characters replaced with safe alternatives
- **Date Prefixing** - Optional YYYY-MM-DD prefix for organized project structure

## Requirements

This tool requires the **.NET 10.0 or .NET 11.0 SDK** to be installed on your system.

- **Supported Platforms**: Windows, macOS, Linux
- **Runtime**: .NET 10.0 or later
- **Older runtimes**: From 1.7.0 the tool no longer targets .NET 8 or .NET 9, which left support on 10 November 2026. A machine with only .NET 8 or .NET 9 cannot install new releases. Install .NET 10 or later, or pin version 1.6.2.
- **Installation**: Via .NET Global Tools

## Installation

### From NuGet (Recommended)
```bash
dotnet tool install --global solrevdev.seedfolder
```

### Local Development Installation
```bash
# Clone the repository
git clone https://github.com/solrevdev/seedfolder.git
cd seedfolder

# Build and install locally
dotnet pack -c release -o artifacts/nupkg
dotnet tool uninstall -g solrevdev.seedfolder  # If previously installed
dotnet tool install -g --add-source artifacts/nupkg solrevdev.seedfolder
```

### Uninstall
```bash
dotnet tool uninstall --global solrevdev.seedfolder
```

## Development

### Local Testing
```bash
## Run directly during development

# Interactive mode
dotnet run --project src/solrevdev.seedfolder.csproj --framework net10.0

## With arguments

# Show the application's version (pass `--version` to the app via `--`)
dotnet run --project src/solrevdev.seedfolder.csproj --framework net10.0 -- --version

# Show the application's help text (pass `--help` to the app via `--`)
dotnet run --project src/solrevdev.seedfolder.csproj --framework net10.0 -- --help

# Create a Node template project using the app (arguments after `--` are for the app)
dotnet run --project src/solrevdev.seedfolder.csproj --framework net10.0 -- --template node myapp

# If you omit `--` and run `dotnet run --project ... --help`, the .NET CLI help will be shown.
```

### Building and Testing
```bash
# Build the project
dotnet build

# Run the integration tests for each target framework
./tests/integration-test.sh net10.0
./tests/integration-test.sh net11.0

# Create package
dotnet pack -c release
```

## Roadmap

### ✅ Completed (v1.3.x)
- Multi-template system with 6 project types
- Professional CLI interface with comprehensive help
- Cross-platform testing and CI/CD
- Interactive template selection
- Dry-run mode and force operations
- Progress indicators and enhanced error handling

### 🔮 Future Enhancements
- **External Template Sources** - Support for custom template directories
- **Configuration File Support** - User preferences and default settings
- **Template Management System** - Validation, versioning, and template marketplace

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

## Publishing to NuGet

The project uses GitHub Actions for automatic publishing to NuGet. Simply bump the version in `src/solrevdev.seedfolder.csproj`:

```xml
<Version>1.3.2</Version>
```

Commit to the `master` branch and GitHub Actions will automatically build and publish to NuGet.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- 🐛 **Issues**: [GitHub Issues](https://github.com/solrevdev/seedfolder/issues)
- 💬 **Discussions**: [GitHub Discussions](https://github.com/solrevdev/seedfolder/discussions)
- 🐦 **Twitter**: [@solrevdev](https://twitter.com/solrevdev)
