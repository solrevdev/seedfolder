#!/bin/bash
# Integration test for seedfolder - validates all template types work correctly

set -e

echo "🚀 Starting seedfolder integration tests..."

# Navigate to project root
cd "$(dirname "$0")/.."

# Build the project
echo "📦 Building project..."
dotnet build src/solrevdev.seedfolder.csproj --configuration Release --framework net8.0

# Create test directory
TEST_DIR="test-output"
rm -rf "$TEST_DIR"
mkdir -p "$TEST_DIR"
cd "$TEST_DIR"

echo "✅ Testing all template types..."

# Test markdown template (default)
echo "🔹 Testing markdown template (default)..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet test-default-markdown
[[ -f "test-default-markdown/README.md" ]] || { echo "❌ default markdown README.md missing"; exit 1; }
[[ -f "test-default-markdown/.gitignore" ]] || { echo "❌ default markdown .gitignore missing"; exit 1; }

# Test dotnet template
echo "🔹 Testing dotnet template..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet --template dotnet test-dotnet
[[ -f "test-dotnet/.gitignore" ]] || { echo "❌ dotnet .gitignore missing"; exit 1; }
[[ -f "test-dotnet/omnisharp.json" ]] || { echo "❌ dotnet omnisharp.json missing"; exit 1; }

# Test node template
echo "🔹 Testing node template..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t node test-node
[[ -f "test-node/package.json" ]] || { echo "❌ node package.json missing"; exit 1; }
[[ -f "test-node/index.js" ]] || { echo "❌ node index.js missing"; exit 1; }

# Test python template
echo "🔹 Testing python template..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet --template python test-python
[[ -f "test-python/main.py" ]] || { echo "❌ python main.py missing"; exit 1; }
[[ -f "test-python/requirements.txt" ]] || { echo "❌ python requirements.txt missing"; exit 1; }

# Test ruby template
echo "🔹 Testing ruby template..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet --type ruby test-ruby
[[ -f "test-ruby/Gemfile" ]] || { echo "❌ ruby Gemfile missing"; exit 1; }
[[ -f "test-ruby/main.rb" ]] || { echo "❌ ruby main.rb missing"; exit 1; }

# Test markdown template
echo "🔹 Testing markdown template..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t markdown test-markdown
[[ -f "test-markdown/README.md" ]] || { echo "❌ markdown README.md missing"; exit 1; }

# Test universal template
echo "🔹 Testing universal template..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t universal test-universal
[[ -f "test-universal/README.md" ]] || { echo "❌ universal README.md missing"; exit 1; }

# Test dry-run mode
echo "🔹 Testing dry-run mode..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --dry-run -t node test-dry-run
[[ ! -d "test-dry-run" ]] || { echo "❌ dry-run should not create directory"; exit 1; }

# Test force mode
echo "🔹 Testing force mode..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t node test-force-existing
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet --force -t python test-force-existing
[[ -f "test-force-existing/main.py" ]] || { echo "❌ force mode should overwrite with python template"; exit 1; }

# Test space handling
echo "🔹 Testing space handling..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --quiet -t node "test space project"
[[ -d "test-space-project" ]] || { echo "❌ space handling failed"; exit 1; }

# Test error handling - invalid template type
echo "🔹 Testing error handling - invalid template..."
if dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- -t invalidtype test-error 2>/dev/null; then
    echo "❌ Should have failed with invalid template type"
    exit 1
fi

# Test error handling - missing template argument
echo "🔹 Testing error handling - missing template argument..."
if dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --template 2>/dev/null; then
    echo "❌ Should have failed with missing template argument"
    exit 1
fi

# Test help command
echo "🔹 Testing help command..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --help > /dev/null || { echo "❌ help command failed"; exit 1; }

# Test version command
echo "🔹 Testing version command..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --version > /dev/null || { echo "❌ version command failed"; exit 1; }

# Test list templates command
echo "🔹 Testing list templates command..."
dotnet run --project ../src/solrevdev.seedfolder.csproj --framework net8.0 -- --list-templates > /dev/null || { echo "❌ list templates command failed"; exit 1; }

# Clean up
cd ..
rm -rf "$TEST_DIR"

echo "🎉 All integration tests passed!"
echo "✅ Tested templates: dotnet, node, python, ruby, markdown, universal"
echo "✅ Tested features: dry-run, force, quiet, space handling, error handling, help, version, list-templates"