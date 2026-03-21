# ⚠️ В активной разработке ⚠️

# valheim-rest-api
Мод для игры Valheim. Суть мода в проведении специальных действий в игровом мире путем отправки серверу с игрой http запросов.

## 📦 Состав мода

### server
- Запускает HTTP сервер на порту 8080
- Принимает запросы от внешних приложений
- Управляет очередью запросов к игрокам
- Отправляет RPC запросы клиентским модам

### client
- Устанавливается каждым игроком
- Принимает RPC запросы от сервера
- Взаимодействует с локальным игроком
- Отправляет данные обратно на сервер

## 🚀 Установка

### Серверная часть
1. Скопируйте `ValheimInventoryAPI.Server.dll` в папку `BepInEx/plugins/` на сервере
2. Перезапустите сервер

### Клиентская часть (каждому игроку)
1. Скопируйте `ValheimInventoryAPI.Client.dll` в папку `BepInEx/plugins/` клиента
2. Перезапустите игру

## 📡 API Endpoints

### Отправка тестового сообщения клиенту
```bash
POST /api/debug

{
    playerName: string,
    message: string
}

RESPONSE

{
    status: "ok"
}
```

### Получить содержимое инвентаря игрока
```bash
POST /api/inventory

{
    playerName: string
}

RESPONSE

{
    items: [
        {
            name: string,
            prefabName: string,
            stack: number,
            quality: number,
            durability: number,
            isEquipable: boolean
        },
        ...
    ]
}
```

## 🛠️ Сборка из исходников
### Windows
```bash
.\build.ps1 -ValheimPath "C:\путь\к\серверу" -OutputDir ".\Build"
```
### Linux (не проверено)
```bash
./build.sh "/home/steam/valheim-server" "./Build"
```

## ⚙️ Конфигурация
Порт HTTP сервера можно изменить в server\ValheimRestApiPlugin.cs.

## 📋 Логи
Логи пишутся в стандартный вывод BepInEx (консоль сервера/игры).


