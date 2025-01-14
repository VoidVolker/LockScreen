# Lock Screen

Windows lockscreen wallpaper application for multimonitor setup.

Приложение для добавления обоев на дополнительные мониторы на экране блокировки.

## English

### How to use

- Download version for your platform from Releases page (x86 will not work on x64 OS)
- Install application from installer or use portable version in 7z archive
- Check the service `Logon Service` is installed and running at `Settings` tab (install in portable version)
- Configure wallpaper for additional monitor - application will save settings immediately; do not use files from network storages and with limited access; use files from SSD for better performance
- Use `Preview` button to see result
- Close application and lock the PC: you will see shortly selected wallpapers on all additional monitors

### Features

- All monitors list
- Monitor find (double click to it's name)
- One wallpaper for all additional monitor
- Individual wallpaper for each monitor
- Wallpaper preview
- Wallpaper only on additional monitors (feel free to request main monitor wallpaper feature)
- Supported images formats: `bmp`, `gif`, `jpg`, `jpeg`, `png`, `tiff`
- Small and tiny background system service to detect OS lock events
- One image layout option available: `ImageLayout.Zoom` - _The image is enlarged within the control's client rectangle_ [source](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.imagelayout?view=windowsdesktop-9.0) (other options in plans)

### How it works?

- `Logon Service` detects Windows Logoff and Lock events
- It starts `Wallpaper.exe` application
- `Wallpaper.exe` reads wallpapers configuration
- Finds and loads wallpapers to memory
- Creates windows for each wallpaper
- Draw images in windows

Wallpaper windows is hidden from `Alt+Tab` hotkey and do not interact with clicks or keyboard and hotkeys like `Alt+F4` or `Alt+Space`

### Supported OS

- Windows 7
- WIndows 10
- Windows 11

### Development

- Windows 10
- Visual Studio 2022
- NSIS
- DotNet 9 SDK

---

## Русский

### Как использовать

- Скачайте версию для вашей платформы на странице релизов (x86 не будет работать на x64 ОС)
- Установите приложение с помощью установщика или просто распакуйте портативную версию из 7z архива
- Убедитесь, что сервис 'Logon Service' установился и запущен на вкладке `Настройки` приложения (установите в случае портативной версии)
- Выберите обои для дополнительных мониторов - приложение сразу сохранит все настройки; не используйте файлы c сетевых хранилищ, а так же с ограниченными правами; используйте файлы с SSD для большей производительности
- Для проверки обоев используйте кнопку `Просмотр`
- Закройте приложение и заблокируйте ПК: практически сразу после блокировки на дополнительных экранах появятся обои

### Функции

- Список всех мониторов
- Поиск монитора (двойной клик на имени)
- Одна картинка для всех мониторов
- Своя картинка для каждого монитора
- Прдварительный просмотр обоев
- Обои доступны только для дополнительных мониторов (если нужны обои на основном мониторе - пишите запрос на гитхабе)
- Поддреживаемые форматы изображений: `bmp`, `gif`, `jpg`, `jpeg`, `png`, `tiff`
- Маленький и легкий фоновый системный сервис для обнаружения блокировки экрана и экрана входа в систему
- Доступна одна опция размещения изображения в окне: `ImageLayout.Zoom` - _The image is enlarged within the control's client rectangle_ [source](https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.imagelayout?view=windowsdesktop-9.0) (остальные опции в планах)

### Как это работает?

- Сервис `Logon Service` отслеживает активацю экрана блокировки
- Запускает приложение `Wallpaper.exe`
- Приложение `Wallpaper.exe` читает ранее сохранёные настройки
- Ищет и загружает в память изображения
- Создаёт окно для каждого изображения
- Рисует изображения в окнах

Окна с обоями спрятаны от хоткея `Alt-Tab` и игнорируют клики и все остальные хоткеи типа `Alt+F4` или `Alt+Space`

### Поддерживаемые ОС

- Windows 7
- WIndows 10
- Windows 11

### Разработка

### Development

- Windows 10
- Visual Studio 2022
- NSIS
- DotNet 9 SDK
