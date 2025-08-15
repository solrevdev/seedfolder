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
▲   Choose a project template:
    1. markdown  - Documentation project with README
    2. dotnet    - .NET project with standard dotfiles
    3. node      - Node.js project with package.json
    4. python    - Python project with requirements.txt
    5. ruby      - Ruby project with Gemfile
    6. universal - Basic project with minimal files
▲   Select template (1-6): 3
▲   Do you want to prefix the folder with the date? [Y/n] y
▲   What do you want the folder to be named? my-awesome-app
▲   [1/6] Copying package.json
    ✅ Created 2024-01-15_my-awesome-app/package.json
▲   [2/6] Copying index.js
    ✅ Created 2024-01-15_my-awesome-app/index.js
▲   Done!
▲   Successfully created 6 files in '2024-01-15_my-awesome-app' using Node template.
```

### Direct Template Selection
```bash
# Create different project types
seedfolder --template node myapp
seedfolder -t python "machine learning project"  # Spaces converted to dashes
seedfolder --type ruby my_gem_name
seedfolder -t markdown my-docs
seedfolder --template universal basic-project

# Using folder name argument (skips interactive mode)
seedfolder myproject                    # Creates markdown project (default)
seedfolder --template python myapp      # Creates Python project
```

### Preview and Force Operations
```bash
# Preview what would be created (dry-run mode)
seedfolder --dry-run -t node myapp
▲   [DRY RUN] Would create directory: myapp
▲   [DRY RUN] Would copy package.json → myapp/package.json
▲   [DRY RUN] Would copy index.js → myapp/index.js
▲   [DRY RUN] Would copy .gitignore → myapp/.gitignore
▲   [DRY RUN] 6 files would be created

# Force overwrite existing directory
seedfolder --force existing-directory
seedfolder --force -t python existing-python-project

# Quiet mode for scripting
seedfolder --quiet -t node myapp
```

### Template Information
```bash
# List all available templates with file details
seedfolder --list-templates

# Show version information
seedfolder --version

# Show help
seedfolder --help
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
▲   [1/6] Copying package.json
    ✅ Created test-project/package.json
▲   [2/6] Copying index.js
    ✅ Created test-project/index.js
▲   Done!
▲   Successfully created 6 files in 'test-project' using Node template.
```

### 🔄 **Smart Input Handling**
- **Space Conversion** - Spaces in folder names automatically converted to dashes
- **Path Sanitization** - Invalid filesystem characters replaced with safe alternatives
- **Date Prefixing** - Optional YYYY-MM-DD prefix for organized project structure

## Requirements

This tool requires **.NET 8.0 or .NET 9.0 SDK** to be installed on your system.

- **Supported Platforms**: Windows, macOS, Linux
- **Runtime**: .NET 8.0 or later
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
# Run directly during development
dotnet run --project src/solrevdev.seedfolder.csproj

# With arguments
dotnet run --project src/solrevdev.seedfolder.csproj -- --template node myapp
dotnet run --project src/solrevdev.seedfolder.csproj -- --help
```

### Building and Testing
```bash
# Build the project
dotnet build

# Run tests
dotnet test

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
