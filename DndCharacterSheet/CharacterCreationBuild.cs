namespace DndCharacterSheet;

internal sealed class CharacterCreationBuild
{
    private const int MinMenuChoice = 1;
    private const int MaxClassChoice = 13;
    private const int MaxRaceChoice = 13;
    private const int MinStatValue = 1;
    private const int MaxStatValue = 20;
    private const int StatCount = 6;
    
    private string name = string.Empty;
    
    private CharacterClass characterClass = CharacterClass.Fighter;
    private CharacterRace characterRace = CharacterRace.Human;
    
    private int strength = 10;
    private int dexterity = 10;
    private int constitution = 10;
    private int intelligence = 10;
    private int wisdom = 10;
    private int charisma = 10;
    
    internal Armor? BodyArmor { get; set; }
    internal Armor? Shield { get; set; }

    internal Character? Run()
    {
        int currentStep = 1;

        while (currentStep >= 1 && currentStep <= 5)
        {
            switch (currentStep)
            {
                case 1:
                    currentStep = this.Step1Name();
                    break;
                case 2:
                    currentStep = this.Step2Class();
                    break;
                case 3:
                    currentStep = this.Step3Race();
                    break;
                case 4:
                    currentStep = this.Step4Stats();
                    break;
                case 5:
                    currentStep = this.Step5Equipment();
                    break;
            }
        }

        if (currentStep < 1)
        {
            return null; 
        }
        
        var character = new Character(this.name, this.characterRace, this.characterClass);
        character.SetStats(
            this.strength,
            this.dexterity,
            this.constitution,
            this.intelligence,
            this.wisdom,
            this.charisma);
        character.BodyArmor = this.BodyArmor;
        character.Shield = this.Shield;

        if (this.BodyArmor != null)
        {
            character.Inventory.AddItem(this.BodyArmor); 
        }

        if (this.Shield != null)
        {
            character.Inventory.AddItem(this.Shield);
        }
        
        return character;
    }

    private int Step1Name()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ СОЗДАНИЕ ПЕРСОНАЖА                         [ Шаг 1 из 5: Имя ]     ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║                                                                    ║");
            Console.WriteLine("║ Введите имя героя (до 20 символов):                                ║");
            Console.WriteLine("║                                                                    ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ [0] Отмена / Выход                                                 ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write("Введите имя героя > ");

            var input = Console.ReadLine();

            if (input?.Trim() == "0")
            {
                return 0; 
            }
            
            if (!string.IsNullOrWhiteSpace(input))
            {
                var words = input.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var cleanedInput = string.Join(" ", words);
                
                if (cleanedInput.Length > 20)
                {
                    Console.WriteLine("Имя слишком длинное! Максимум 20 символов. Нажмите любую клавишу...");
                    Console.ReadKey();
                    continue;
                }

                var cultureInfo = System.Globalization.CultureInfo.GetCultureInfo("ru-RU");
                this.name = cultureInfo.TextInfo.ToTitleCase(cleanedInput.ToLower());
                return 2;
            }

            Console.WriteLine("Имя не может быть пустым! Нажмите любую клавишу для повтора...");
            Console.ReadKey();
        }
    }

    private int Step2Class()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ СОЗДАНИЕ ПЕРСОНАЖА                         [ Шаг 2 из 5: Класс ]   ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
        
            string nameLine = $" Имя героя: {this.name}";
            Console.WriteLine($"║{nameLine.PadRight(68)}║");
        
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Выберите класс:                                                    ║");
            Console.WriteLine("║   [1] Плут             [8] Изобретатель                            ║");
            Console.WriteLine("║   [2] Следопыт         [9] Варвар                                  ║");
            Console.WriteLine("║   [3] Бард             [10] Жрец                                   ║");
            Console.WriteLine("║   [4] Друид            [11] Монах                                  ║");
            Console.WriteLine("║   [5] Воин             [12] Чародей                                ║");
            Console.WriteLine("║   [6] Волшебник        [13] Колдун                                 ║");
            Console.WriteLine("║   [7] Паладин                                                      ║");
        
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            string menuLine = " [1-13] Выбрать класс                  [0] Назад к имени";
            Console.WriteLine($"║{menuLine.PadRight(68)}║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
        
            Console.Write("Введите цифру класса > ");

            var input = Console.ReadLine();

            if (input?.Trim() == "0")
            {
                return 1; 
            }
            
            if (int.TryParse(input, out var choice) && choice >= MinMenuChoice && choice <= MaxClassChoice)
            {
                this.characterClass = choice switch
                {
                    1 => CharacterClass.Rogue,
                    2 => CharacterClass.Ranger,
                    3 => CharacterClass.Bard,
                    4 => CharacterClass.Druid,
                    5 => CharacterClass.Fighter,
                    6 => CharacterClass.Wizard,
                    7 => CharacterClass.Paladin,
                    8 => CharacterClass.Artificer,
                    9 => CharacterClass.Barbarian,
                    10 => CharacterClass.Cleric,
                    11 => CharacterClass.Monk,
                    12 => CharacterClass.Sorcerer,
                    13 => CharacterClass.Warlock,
                    _ => CharacterClass.Fighter
                };
                return 3;
            }

            Console.WriteLine("Неверный ввод. Введите число от 1 до 13. Нажмите клавишу...");
            Console.ReadKey();
        }
    }
    private int Step3Race()
    {
        while (true)
        {
            var className = this.characterClass.ToRussian();

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ СОЗДАНИЕ ПЕРСОНАЖА                         [ Шаг 3 из 5: Раса ]    ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
        
            string infoLine = $" Имя героя: {this.name} │ Класс: {className}";
            Console.WriteLine($"║{infoLine.PadRight(68)}║");
        
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Выберите расу:                                                     ║");
            Console.WriteLine("║   [1] Человек          [8] Полуорк                                 ║");
            Console.WriteLine("║   [2] Эльф             [9] Тифлинг                                 ║");
            Console.WriteLine("║   [3] Дварф            [10] Аасимар                                ║");
            Console.WriteLine("║   [4] Полурослик       [11] Генази                                 ║");
            Console.WriteLine("║   [5] Драконорожденный [12] Голиаф                                 ║");
            Console.WriteLine("║   [6] Гном             [13] Табакси                                ║");
            Console.WriteLine("║   [7] Полуэльф                                                     ║");
            
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            string menuLine = " [1-13] Выбрать расу                   [0] Назад к классу";
            Console.WriteLine($"║{menuLine.PadRight(68)}║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            
            Console.Write("Введите цифру расы > ");

            var input = Console.ReadLine();

            if (input?.Trim() == "0")
            {
                return 2; 
            }
            
            if (int.TryParse(input, out var choice) && choice >= MinMenuChoice && choice <= MaxRaceChoice)
            {
                this.characterRace = choice switch
                {
                    1 => CharacterRace.Human,
                    2 => CharacterRace.Elf,
                    3 => CharacterRace.Dwarf,
                    4 => CharacterRace.Halfling,
                    5 => CharacterRace.Dragonborn,
                    6 => CharacterRace.Gnome,
                    7 => CharacterRace.HalfElf,
                    8 => CharacterRace.HalfOrc,
                    9 => CharacterRace.Tiefling,
                    10 => CharacterRace.Aasimar,
                    11 => CharacterRace.Genasi,
                    12 => CharacterRace.Goliath,
                    13 => CharacterRace.Tabaxi,
                    _ => CharacterRace.Human
                };
                return 4;
            }

            Console.WriteLine("Неверный ввод. Введите число от 1 до 13. Нажмите клавишу...");
            Console.ReadKey();
        }
    }
    private int Step4Stats()
    {
        string errorMessage = "";

        while (true)
        {
            var className = this.characterClass.ToRussian();
            var raceName = this.characterRace.ToRussian();

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            
            string titleLine = (" СОЗДАНИЕ ПЕРСОНАЖА".PadRight(38) + "[ Шаг 4 из 5: Характеристики ]").PadRight(68);
            Console.WriteLine($"║{titleLine}║");

            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            
            string infoLine = $" Имя: {this.name} │ Класс: {className} │ Раса: {raceName}";
            Console.WriteLine($"║{infoLine.PadRight(68)}║");

            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            
            string s1 = $" [1] Сила (СИЛ):         [{this.strength,2}] ({this.FormatMod(this.strength)})";
            Console.WriteLine($"║{s1.PadRight(68)}║");

            string s2 = $" [2] Ловкость (ЛОВ):     [{this.dexterity,2}] ({this.FormatMod(this.dexterity)})";
            Console.WriteLine($"║{s2.PadRight(68)}║");

            string s3 = $" [3] Телосложение (ВЫН): [{this.constitution,2}] ({this.FormatMod(this.constitution)})";
            Console.WriteLine($"║{s3.PadRight(68)}║");

            string s4 = $" [4] Интеллект (ИНТ):    [{this.intelligence,2}] ({this.FormatMod(this.intelligence)})";
            Console.WriteLine($"║{s4.PadRight(68)}║");

            string s5 = $" [5] Мудрость (МУД):     [{this.wisdom,2}] ({this.FormatMod(this.wisdom)})";
            Console.WriteLine($"║{s5.PadRight(68)}║");

            string s6 = $" [6] Харизма (ХАР):      [{this.charisma,2}] ({this.FormatMod(this.charisma)})";
            Console.WriteLine($"║{s6.PadRight(68)}║");

            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");

            string menuLine = " [1-6] Выбрать стат   │   [Enter] Далее (Шаг 5)   │   [0] Назад";
            Console.WriteLine($"║{menuLine.PadRight(68)}║");

            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine($"Ошибка: {errorMessage}");
                errorMessage = "";
            }

            Console.Write("Выберите действие > ");
            var input = Console.ReadLine()?.Trim();

            if (input == "0")
            {
                return 3; 
            }

            if (string.IsNullOrEmpty(input))
            {
                return 5; 
            }
            
            if (int.TryParse(input, out var statChoice) && statChoice >= MinMenuChoice && statChoice <= StatCount)
            {
                var statName = statChoice switch
                {
                    1 => "Силы (СИЛ)",
                    2 => "Ловкости (ЛОВ)",
                    3 => "Телосложения (ВЫН)",
                    4 => "Интеллекта (ИНТ)",
                    5 => "Мудрости (МУД)",
                    6 => "Харизмы (ХАР)",
                    _ => "стата"
                };

                Console.Write($"Введите новое значение для {statName} [от 1 до 20] > ");
                var valueInput = Console.ReadLine();

                if (int.TryParse(valueInput, out var value) && value >= MinStatValue && value <= MaxStatValue)
                {
                    switch (statChoice)
                    {
                        case 1:
                            this.strength = value;
                            break;
                        case 2:
                            this.dexterity = value;
                            break;
                        case 3:
                            this.constitution = value;
                            break;
                        case 4:
                            this.intelligence = value;
                            break;
                        case 5:
                            this.wisdom = value;
                            break;
                        case 6:
                            this.charisma = value;
                            break;
                    }
                }
                else
                {
                    errorMessage = "Значение характеристики должно быть числом от 1 до 20!";
                }
            }
            else
            {
                errorMessage = "Неверный выбор! Введите цифру от 1 до 6 или нажмите Enter.";
            }
        }
    }

    private int Step5Equipment()
    {
        string errorMessage = "";

        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            
            string titleLine = (" СОЗДАНИЕ ПЕРСОНАЖА".PadRight(38) + "[ Шаг 5: Экипировка ]").PadRight(68);
            Console.WriteLine($"║{titleLine}║");

            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            
            string currentArmor = this.BodyArmor != null ? this.BodyArmor.Name : "Без брони";
            string infoLine = $" Текущая броня: {currentArmor}";
            Console.WriteLine($"║{infoLine.PadRight(68)}║");

            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            
            string s1 = " [1] Легкая кожаная   (Вес: 4.5 кг, КД 11 + ЛОВ)";
            Console.WriteLine($"║{s1.PadRight(68)}║");

            string s2 = " [2] Средняя кольчуга (Вес: 10.0 кг, КД 14 + макс. 2 от ЛОВ)";
            Console.WriteLine($"║{s2.PadRight(68)}║");

            string s3 = " [3] Тяжелые латы     (Вес: 29.5 кг, КД 18 фиксированно)";
            Console.WriteLine($"║{s3.PadRight(68)}║");

            string s4 = " [4] Без брони        (КД 10 + ЛОВ)";
            Console.WriteLine($"║{s4.PadRight(68)}║");

            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");

            string menuLine = " [1-4] Выбрать броню  │   [Enter] К щиту          │   [0] Назад";
            Console.WriteLine($"║{menuLine.PadRight(68)}║");

            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine($"Ошибка: {errorMessage}");
                errorMessage = "";
            }

            Console.Write("Выберите действие > ");
            var input = Console.ReadLine()?.Trim();

            if (input == "0")
            {
                return 4; 
            }

            if (string.IsNullOrEmpty(input))
            {
                break; 
            }

            if (int.TryParse(input, out var choice) && choice >= 1 && choice <= 4)
            {
                switch (choice)
                {
                    case 1:
                        this.BodyArmor = new Armor("Легкая кожаная броня", 4.5, ArmorType.Light, 11);
                        break;
                    case 2:
                        this.BodyArmor = new Armor("Средняя кольчуга", 10.0, ArmorType.Medium, 14);
                        break;
                    case 3:
                        this.BodyArmor = new Armor("Тяжелые латы", 29.5, ArmorType.Heavy, 18);
                        break;
                    case 4:
                        this.BodyArmor = null;
                        break;
                }
            }
            else
            {
                errorMessage = "Неверный выбор! Введите цифру от 1 до 4 или нажмите Enter.";
            }
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            
            string titleLine = (" СОЗДАНИЕ ПЕРСОНАЖА".PadRight(38) + "[ Шаг 5: Экипировка ]").PadRight(68);
            Console.WriteLine($"║{titleLine}║");

            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            
            string currentShield = this.Shield != null ? "Щит активен (+2 КД)" : "Без щита";
            string infoLine = $" Текущий щит: {currentShield}";
            Console.WriteLine($"║{infoLine.PadRight(68)}║");

            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            
            string s1 = " [1] Да  (Щит: Вес 2.7 кг, Бонус КД +2)";
            Console.WriteLine($"║{s1.PadRight(68)}║");

            string s2 = " [2] Нет (Без щита)";
            Console.WriteLine($"║{s2.PadRight(68)}║");

            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");

            string menuLine = " [1-2] Выбрать щит    │   [Enter] Завершить       │   [0] Назад";
            Console.WriteLine($"║{menuLine.PadRight(68)}║");

            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

            if (!string.IsNullOrEmpty(errorMessage))
            {
                Console.WriteLine($"Ошибка: {errorMessage}");
                errorMessage = "";
            }

            Console.Write("Выберите действие > ");
            var input = Console.ReadLine()?.Trim();

            if (input == "0")
            {
                return Step5Equipment(); 
            }

            if (string.IsNullOrEmpty(input))
            {
                return 6; 
            }

            if (int.TryParse(input, out var choice) && choice >= 1 && choice <= 2)
            {
                if (choice == 1)
                {
                    this.Shield = new Armor("Щит", 2.7, ArmorType.Shield, 2);
                }
                else
                {
                    this.Shield = null;
                }
                return 6;
            }
            else
            {
                errorMessage = "Неверный выбор! Введите 1 или 2, либо нажмите Enter.";
            }
        }
    }
    private string FormatMod(int statValue)
    {
        int modifier = Character.CalculateModifier(statValue); 
        return modifier >= 0 ? $"+{modifier}" : $"{modifier}";
    }
}