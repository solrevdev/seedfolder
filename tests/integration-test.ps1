# Integration test for seedfolder - validates all template types work correctly (PowerShell)

$ErrorActionPreference = "Stop"

Write-Host "🚀 Starting seedfolder integration tests..." -ForegroundColor Green

# Navigate to project root
Set-Location "$(Split-Path $PSScriptRoot -Parent)"

# Build the project
Write-Host "📦 Building project..." -ForegroundColor Yellow
dotnet build src/solrevdev.seedfolder.csproj --configuration Release --framework net8.0

# Create test directory
$TestDir = "test-output"
if (Test-Path $TestDir) { Remove-Item $TestDir -Recurse -Force }
New-Item -ItemType Directory -Path $TestDir | Out-Null
Set-Location $TestDir

Write-Host "✅ Testing all template types..." -ForegroundColor Green

try {
    # Test dotnet template (default)
    Write-Host "🔹 Testing dotnet template..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet test-dotnet
    if (!(Test-Path "test-dotnet/.gitignore")) { throw "dotnet .gitignore missing" }
    if (!(Test-Path "test-dotnet/omnisharp.json")) { throw "dotnet omnisharp.json missing" }

    # Test node template
    Write-Host "🔹 Testing node template..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t node test-node
    if (!(Test-Path "test-node/package.json")) { throw "node package.json missing" }
    if (!(Test-Path "test-node/index.js")) { throw "node index.js missing" }

    # Test python template
    Write-Host "🔹 Testing python template..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet --template python test-python
    if (!(Test-Path "test-python/main.py")) { throw "python main.py missing" }
    if (!(Test-Path "test-python/requirements.txt")) { throw "python requirements.txt missing" }

    # Test ruby template
    Write-Host "🔹 Testing ruby template..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet --type ruby test-ruby
    if (!(Test-Path "test-ruby/Gemfile")) { throw "ruby Gemfile missing" }
    if (!(Test-Path "test-ruby/main.rb")) { throw "ruby main.rb missing" }

    # Test markdown template
    Write-Host "🔹 Testing markdown template..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t markdown test-markdown
    if (!(Test-Path "test-markdown/README.md")) { throw "markdown README.md missing" }

    # Test universal template
    Write-Host "🔹 Testing universal template..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t universal test-universal
    if (!(Test-Path "test-universal/README.md")) { throw "universal README.md missing" }

    # Test dry-run mode
    Write-Host "🔹 Testing dry-run mode..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --dry-run -t node test-dry-run
    if (Test-Path "test-dry-run") { throw "dry-run should not create directory" }

    # Test force mode
    Write-Host "🔹 Testing force mode..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t node test-force-existing
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet --force -t python test-force-existing
    if (!(Test-Path "test-force-existing/main.py")) { throw "force mode should overwrite with python template" }

    # Test space handling
    Write-Host "🔹 Testing space handling..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t node "test space project"
    if (!(Test-Path "test-space-project")) { throw "space handling failed" }

    # Test error handling - invalid template type
    Write-Host "🔹 Testing error handling - invalid template..." -ForegroundColor Cyan
    $result = dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- -t invalidtype test-error 2>$null
    if ($LASTEXITCODE -eq 0) { throw "Should have failed with invalid template type" }

    # Test error handling - missing template argument
    Write-Host "🔹 Testing error handling - missing template argument..." -ForegroundColor Cyan
    $result = dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --template 2>$null
    if ($LASTEXITCODE -eq 0) { throw "Should have failed with missing template argument" }

    # Test help command
    Write-Host "🔹 Testing help command..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --help >$null
    if ($LASTEXITCODE -ne 0) { throw "help command failed" }

    # Test version command
    Write-Host "🔹 Testing version command..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --version >$null
    if ($LASTEXITCODE -ne 0) { throw "version command failed" }

    # Test list templates command
    Write-Host "🔹 Testing list templates command..." -ForegroundColor Cyan
    dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --list-templates >$null
    if ($LASTEXITCODE -ne 0) { throw "list templates command failed" }

    Write-Host "🎉 All integration tests passed!" -ForegroundColor Green
    Write-Host "✅ Tested templates: dotnet, node, python, ruby, markdown, universal" -ForegroundColor Green
    Write-Host "✅ Tested features: dry-run, force, quiet, space handling, error handling, help, version, list-templates" -ForegroundColor Green
}
catch {
    Write-Host "❌ Test failed: $_" -ForegroundColor Red
    exit 1
}
finally {
    # Clean up
    Set-Location ".."
    if (Test-Path $TestDir) { Remove-Item $TestDir -Recurse -Force }
}