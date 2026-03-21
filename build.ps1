# build.ps1
param(
    [string]$ValheimPath = "C:\Program Files (x86)\Steam\steamapps\common\Valheim dedicated server",
    [string]$OutputDir = ".\Build",
    [string]$OutputDirServer = "C:\Program Files (x86)\Steam\steamapps\common\Valheim dedicated server\BepInEx\plugins\valheim-rest-api",
    [string]$OutputDirClient = "C:\Program Files (x86)\Steam\steamapps\common\Valheim\BepInEx\plugins\valheim-rest-api"
)

Write-Host "=== Сборка Valheim Rest API Mods ===" -ForegroundColor Green

# Устанавливаем переменную окружения для сборки
$env:VALHEIM_INSTALL = $ValheimPath

# Создаем выходную директорию если её нет
if (!(Test-Path $OutputDir)) {
    New-Item -ItemType Directory -Path $OutputDir -Force | Out-Null
}

if (!(Test-Path $OutputDirServer)) {
    New-Item -ItemType Directory -Path $OutputDirServer -Force | Out-Null
}

if (!(Test-Path $OutputDirClient)) {
    New-Item -ItemType Directory -Path $OutputDirClient -Force | Out-Null
}

# Функция для форматирования размера файла
function Format-FileSize {
    param([long]$bytes)
    if ($bytes -gt 1MB) {
        return "{0:N2} MB" -f ($bytes / 1MB)
    }
    elseif ($bytes -gt 1KB) {
        return "{0:N2} KB" -f ($bytes / 1KB)
    }
    else {
        return "{0} B" -f $bytes
    }
}

# Сборка серверного мода
Write-Host "`nСборка серверного мода..." -ForegroundColor Yellow
Push-Location .\server
dotnet clean
dotnet build -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "Ошибка сборки серверного мода!" -ForegroundColor Red
    Pop-Location
    exit 1
}
Pop-Location

# Сборка клиентского мода
Write-Host "`nСборка клиентского мода..." -ForegroundColor Yellow
Push-Location .\client
dotnet clean
dotnet build -c Release
if ($LASTEXITCODE -ne 0) {
    Write-Host "Ошибка сборки клиентского мода!" -ForegroundColor Red
    Pop-Location
    exit 1
}
Pop-Location

Write-Host "`n=== Сборка завершена успешно! ===" -ForegroundColor Green
Write-Host "Файлы находятся в: $OutputDir" -ForegroundColor Cyan

# Показываем список собранных файлов
Write-Host "`nСобранные файлы:" -ForegroundColor Yellow
Get-ChildItem $OutputDir -Filter "*.dll" | ForEach-Object {
    $size = Format-FileSize -bytes $_.Length
    Write-Host "  - $($_.Name) ($size)" -ForegroundColor White
}

# Копируем файлы если они не в выходной директории
$serverDll = ".\server\bin\Release\net48\ValheimRestApi.Server.dll"
$clientDll = ".\client\bin\Release\net48\ValheimRestApi.Client.dll"

if (Test-Path $serverDll) {
    Copy-Item $serverDll $OutputDir -Force
    Write-Host "`nСерверный мод скопирован: $OutputDir\ValheimRestApi.Server.dll" -ForegroundColor Green
}

if (Test-Path $clientDll) {
    Copy-Item $clientDll $OutputDir -Force
    Write-Host "Клиентский мод скопирован: $OutputDir\ValheimRestApi.Client.dll" -ForegroundColor Green
}

Copy-Item "$OutputDir\Client\ValheimRestApi.Client.dll" $OutputDirClient -Force
Copy-Item "$OutputDir\Client\ValheimRestApi.Shared.dll" $OutputDirClient -Force
Copy-Item "$OutputDir\Client\Newtonsoft.Json.dll" $OutputDirClient -Force

Copy-Item "$OutputDir\Server\ValheimRestApi.Server.dll" $OutputDirServer -Force
Copy-Item "$OutputDir\Server\ValheimRestApi.Shared.dll" $OutputDirServer -Force
Copy-Item "$OutputDir\Server\Newtonsoft.Json.dll" $OutputDirServer -Force

Write-Host "`nДля установки:" -ForegroundColor Yellow
Write-Host "  Сервер: Скопируйте ValheimRestApi.Server.dll в BepInEx/plugins/ на сервере" -ForegroundColor White
Write-Host "  Клиент: Скопируйте ValheimRestApi.Client.dll в BepInEx/plugins/ каждому игроку" -ForegroundColor White