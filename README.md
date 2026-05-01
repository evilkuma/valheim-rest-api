# ⚠️ В активной разработке ⚠️

# valheim-streamer-api
Мод для игры Valheim. Суть мода в проведении специальных действий в игровом мире путем отправки серверу с игрой http запросов.

## 📦 Состав мода

### server
- Запускает HTTP сервер на порту из конфигурации (по-умолчанию 8080)
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

### Отправка тестового сообщения клиенту (вывод сообщения в консоль)
```bash
POST /api/debug

{
    playerName: string, // ник игрока
    message: string     // текст сообщения
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
    playerName: string  // ник игрока
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

### Спавн игрового обьекта перед игроком (в планах больше параметров)
```bash
POST /api/spawn

{
    playerName: string, // ник игрока
    prefabName: string, // кодовое название предмета или моба
    amount: number,     // количество предметов или мобов
    level: number,      // уровень предмета или моба
    pickup: boolean     // положить в инвентарь
}

RESPONSE

{
    status: "ok" | string
}
```

### Телепорт игрока по названию биома
```bash
POST /api/location/teleport-to-biome

{
    playerName: string,
    biome: "Meadows" | "Swamp" | "Mountain" | "BlackForest" | "Plains" | "AshLands" | "DeepNorth" | "Ocean" | "Mistlands"
}

RESPONSE

{
    status: "ok" | string
}
```

### Комманды
```bash
POST /api/command

{
    playerName: string, // ник игрока
    command: string,    // название комманды
    data: {}            // параметры комманды
}
```

### Список комманд и типы данных
- undress - снять снаряжение с игрока
    ```bash
    "
    data:
    {
        // комманда без параметров
    }

    RESPONSE

    {
        status: "ok" | string
    }
    "
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
Порт HTTP сервера можно изменить в конфигурации BepInEx (BepInEx\config\ru.evilkuma.valheimstreamerapi.server.cfg)

## 📋 Логи
Логи пишутся в стандартный вывод BepInEx (консоль сервера/игры).


