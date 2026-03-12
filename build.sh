#!/bin/bash

# build.sh
VALHEIM_PATH="${1:-/home/steam/valheim-server}"
OUTPUT_DIR="${2:-./Build}"

echo "=== Сборка Valheim Inventory API Mods ==="

# Устанавливаем переменную окружения
export VALHEIM_INSTALL="$VALHEIM_PATH"

# Создаем выходную директорию
mkdir -p "$OUTPUT_DIR"

# Сборка серверного мода
echo -e "\nСборка серверного мода..."
cd ServerMod
dotnet clean
dotnet build -c Release
if [ $? -ne 0 ]; then
    echo "Ошибка сборки серверного мода!"
    exit 1
fi
cd ..

# Сборка клиентского мода
echo -e "\nСборка клиентского мода..."
cd ClientMod
dotnet clean
dotnet build -c Release
if [ $? -ne 0 ]; then
    echo "Ошибка сборки клиентского мода!"
    exit 1
fi
cd ..

echo -e "\n=== Сборка завершена успешно! ==="
echo "Файлы в: $OUTPUT_DIR"