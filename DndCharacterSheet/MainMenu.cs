namespace DndCharacterSheet;

internal sealed class MainMenu
{
    internal Character? Run()
    {
        while (true)
        {
            int welcomeChoice = this.ShowWelcomeScreen();

            if (welcomeChoice == 0)
            {
                return null;
            }
            
            if (welcomeChoice == 1)
            {
                var builder = new CharacterCreationBuild();
                var newCharacter = builder.Run();
                
                if (newCharacter != null)
                {
                    return newCharacter;
                }
            }
            else if (welcomeChoice == 2)
            {
                var loadedCharacter = this.LoadExistingCharacter();
                if (loadedCharacter != null)
                {
                    return loadedCharacter;
                }
            }
        }
    }
    private int ShowWelcomeScreen()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                                    ║");
            Console.WriteLine("║                D & D   C H A R A C T E R   S H E E T               ║");
            Console.WriteLine("║                     Электронный лист персонажа                     ║");
            Console.WriteLine("║                                                                    ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║                                                                    ║");
            Console.WriteLine("║   Добро пожаловать в мир приключений, странник!                    ║");
            Console.WriteLine("║   Создай своего героя, снаряди его в путь и готовься к броску      ║");
            Console.WriteLine("║   двадцатигранного кубика (d20).                                   ║");
            Console.WriteLine("║                                                                    ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║   [1] Создать нового персонажа                                     ║");
            Console.WriteLine("║   [2] Выбрать существующего                                        ║");
            Console.WriteLine("║   [0] Выход из программы                                           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write(" Выберите действие > ");

            var input = Console.ReadLine()?.Trim();

            if (input == "1" || input == "2" || input == "0")
            {
                return int.Parse(input);
            }

            Console.WriteLine("Неверный ввод! Введите 1, 2 или 0. Нажмите любую клавишу...");
            Console.ReadKey();
        }
    }
    
    private Character? LoadExistingCharacter()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ ЗАГРУЗКА ПЕРСОНАЖА                                                 ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Введите имя персонажа для загрузки:                                ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ [0] Назад в главное меню                                           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write("Имя героя > ");

            var input = Console.ReadLine()?.Trim();

            if (input == "0" || string.IsNullOrEmpty(input))
            {
                return null;
            }

            string fileName = $"{input}.json";

            if (System.IO.File.Exists(fileName))
            {
                try
                {
                    var json = System.IO.File.ReadAllText(fileName);
                    var character = System.Text.Json.JsonSerializer.Deserialize<Character>(json);
                    
                    if (character != null)
                    {
                        Console.WriteLine($"Персонаж '{input}' успешно загружен! Нажмите клавишу...");
                        Console.ReadKey();
                        return character;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка чтения файла: {ex.Message}. Нажмите клавишу...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine($"Файл сохранения '{fileName}' не найден! Нажмите клавишу...");
                Console.ReadKey();
            }
        }
    }
}