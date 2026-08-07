using System;
using System.Collections.Generic;

namespace DndCharacterSheet.ConsoleUI
{
    internal class InventoryMenu
    {
        private readonly Character character;
        private const int InventoryCapacity = 20;
        
        public InventoryMenu(Character character)
        {
            this.character = character;
        }
        
        public void Run()
        {
            ShowInventoryMenu();
        }

        private void ShowInventoryMenu()
        {
            Action<Item> weightFailHandler = (item) =>
            {
                Console.Clear();
                Console.WriteLine("\n╔════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ ОШИБКА: Превышен максимальный вес инвентаря!                       ║");
                Console.WriteLine($"║ Предмет \"{item.Name}\" слишком тяжелый и не поместился в рюкзак.   ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
                Console.Write("\nНажмите любую клавишу, чтобы продолжить...");
                Console.ReadKey();
            };

            character.Inventory.OnItemAddFailed += weightFailHandler;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");

                string header =
                    $" ИНВЕНТАРЬ (Вес: {character.Inventory.GetTotalWeight():F1} / {character.Inventory.MaxWeight:F1} кг)";
                Console.WriteLine($"║{header.PadRight(68)}║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");

                bool hasItems = false;
                for (int i = 0; i < InventoryCapacity; i++)
                {
                    try
                    {
                        Item item = character.Inventory[i];
                        hasItems = true;

                        string qtyStr = item is IHasQuantity q ? $"{q.Quantity} шт" : "1 шт";
                        string desc = GetItemDescription(item);

                        double totalItemWeight = item is IHasQuantity stackable
                            ? item.Weight * stackable.Quantity
                            : item.Weight;

                        string line = $" [{i}] {item.Name,-15} │ {qtyStr,6} │ {totalItemWeight,4:F1} кг │ {desc}";

                        Console.WriteLine($"║{line.PadRight(68)}║");
                    }
                    catch
                    {
                        break;
                    }
                }

                if (!hasItems)
                {
                    string emptyMsg = " Рюкзак пуст...";
                    Console.WriteLine($"║{emptyMsg.PadRight(68)}║");
                }

                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                string controls = " [A] Добавить  │  [D] Удалить  │  [E] Экипировать  │  [B] Назад";
                Console.WriteLine($"║{controls.PadRight(68)}║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
                Console.Write(" Выберите действие > ");

                var input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "B")
                {
                    character.Inventory.OnItemAddFailed -= weightFailHandler;
                    return;
                }
                else if (input == "A")
                {
                    ShowAddItemMenu();
                }
                else if (input == "D")
                {
                    ShowDeleteItemMenu();
                }
                else if (input == "E")
                {
                    ShowEquipmentMenu();
                }
            }
        }

        private void ShowEquipmentMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ УПРАВЛЕНИЕ СНАРЯЖЕНИЕМ                                             ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");

                string bodyName = character.BodyArmor != null
                    ? $"{character.BodyArmor.Name} [Экипирован]"
                    : "Без брони";
                string shieldName = character.Shield != null ? $"{character.Shield.Name} [Экипирован]" : "Без щита";

                Console.WriteLine($"║ Текущая броня: {bodyName.PadRight(52)}║");
                Console.WriteLine($"║ Текущий щит:   {shieldName.PadRight(52)}║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ Доступная броня и щиты в инвентаре:                                ║");

                List<Armor> armorItems = new List<Armor>();
                for (int i = 0; i < InventoryCapacity; i++)
                {
                    try
                    {
                        Item item = character.Inventory[i];
                        if (item is Armor armor)
                        {
                            armorItems.Add(armor);
                        }
                    }
                    catch
                    {
                        break;
                    }
                }

                if (armorItems.Count == 0)
                {
                    Console.WriteLine("║ (В инвентаре нет доспехов или щитов)                               ║");
                }
                else
                {
                    for (int i = 0; i < armorItems.Count; i++)
                    {
                        var armor = armorItems[i];
                        string isEquippedMark = (character.BodyArmor == armor || character.Shield == armor)
                            ? " [Экипирован]"
                            : "";

                        string armorDesc = armor.ArmorType switch
                        {
                            ArmorType.Light => $"КД {armor.ArmorClassBonus} + ЛОВ (Лёгкий)",
                            ArmorType.Medium => $"КД {armor.ArmorClassBonus} + макс. 2 от ЛОВ (Средний)",
                            ArmorType.Heavy => $"КД {armor.ArmorClassBonus} фикс. (Тяжелый)",
                            ArmorType.Shield => $"Бонус +{armor.ArmorClassBonus} КД (Щит)",
                            _ => $"КД {armor.ArmorClassBonus}"
                        };

                        string line = $" [{i + 1}] {armor.Name,-14}{isEquippedMark,-13} │ {armorDesc}";
                        Console.WriteLine($"║{line.PadRight(68)}║");
                    }
                }

                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ [1-N] Надеть/Снять │ [0] Снять всю защиту │ [B] Назад              ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                Console.Write(" Выберите действие > ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (input == "b") break;

                if (input == "0")
                {
                    character.BodyArmor = null;
                    character.Shield = null;
                    continue;
                }

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= armorItems.Count)
                {
                    var selectedArmor = armorItems[choice - 1];

                    if (selectedArmor.ArmorType == ArmorType.Shield)
                    {
                        character.Shield = character.Shield == selectedArmor ? null : selectedArmor;
                    }
                    else
                    {
                        character.BodyArmor = character.BodyArmor == selectedArmor ? null : selectedArmor;
                    }
                }
            }
        }

        private void ShowAddItemMenu()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ ДОБАВЛЕНИЕ ПРЕДМЕТА                                                ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Выберите категорию:                                                ║");
            Console.WriteLine("║ [1] Оружие                                                         ║");
            Console.WriteLine("║ [2] Доспех                                                         ║");
            Console.WriteLine("║ [3] Зелье                                                          ║");
            Console.WriteLine("║ [4] Снаряжение                                                     ║");
            Console.WriteLine("║ [0] Отмена                                                         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write(" Выберите категорию > ");

            var choice = Console.ReadLine()?.Trim();
            if (choice == "0" || string.IsNullOrEmpty(choice)) return;

            Item? newItem = choice switch
            {
                "1" => CreateWeaponBuild(),
                "2" => CreateArmorBuild(),
                "3" => CreatePotionInteractive(),
                "4" => CreateEquipmentBuild(),
                _ => null
            };

            if (newItem != null)
            {
                character.Inventory.AddItem(newItem);
            }
        }

        private Equipment CreateEquipmentBuild()
        {
            string name = "Снаряжение";
            string errorMessage = "";

            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ СОЗДАНИЕ СНАРЯЖЕНИЯ                      [ Шаг 1 из 3: Название ]  ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ Введите название снаряжения:                                       ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ [Enter] Оставить по умолчанию (\"Снаряжение\")                       ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine($"Ошибка: {errorMessage}");
                    errorMessage = "";
                }

                Console.Write(" Название > ");
                string inputName = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(inputName))
                {
                    name = "Снаряжение";
                    break;
                }
                else if (inputName.Length > 17)
                {
                    errorMessage = "Название слишком длинное! Максимум 17 символов.";
                    continue;
                }
                else
                {
                    name = inputName;
                    break;
                }
            }

            double weight = 1.0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ СОЗДАНИЕ СНАРЯЖЕНИЯ                      [ Шаг 2 из 3: Вес ]       ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ Введите вес одной штуки (кг):                                      ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ [Enter] Использовать вес по умолчанию (1.0 кг)                     ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine($"Ошибка: {errorMessage}");
                    errorMessage = "";
                }

                Console.Write(" Вес (кг) > ");
                string weightInput = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(weightInput))
                {
                    weight = 1.0;
                    break;
                }

                if ((double.TryParse(weightInput, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedWeight) && parsedWeight >= 0) ||
                    (double.TryParse(weightInput, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"), out parsedWeight) && parsedWeight >= 0))
                {
                    weight = parsedWeight;
                    break;
                }
                else
                {
                    errorMessage = "Неверный формат веса! Введите положительное число.";
                }
            }
            
            int quantity = 1;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ СОЗДАНИЕ СНАРЯЖЕНИЯ                  [ Шаг 3 из 3: Количество ]    ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ Введите количество штук:                                           ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ [Enter] Использовать количество по умолчанию (1 шт)                ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine($"Ошибка: {errorMessage}");
                    errorMessage = "";
                }

                Console.Write(" Количество > ");
                string qtyInput = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(qtyInput))
                {
                    quantity = 1;
                    break;
                }

                if (int.TryParse(qtyInput, out int parsedQty) && parsedQty >= 1 && parsedQty <= 99)
                {
                    quantity = parsedQty;
                    break;
                }
                else
                {
                    errorMessage = "Введите целое число от 1 до 99.";
                }
            }

            return new Equipment(name, weight, quantity);
        }

        private Armor CreateArmorBuild()
        {
            string name = "Броня";
            string errorMessage = "";

            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ СОЗДАНИЕ ДОСПЕХА                         [ Шаг 1 из 3: Название ]  ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ Введите название доспеха (максимум 17 символов):                   ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ [Enter] Оставить по умолчанию (\"Броня\")                            ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine($"Ошибка: {errorMessage}");
                    errorMessage = "";
                }

                Console.Write(" Название > ");
                string inputName = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(inputName))
                {
                    name = "Броня";
                    break;
                }
                else if (inputName.Length > 17)
                {
                    errorMessage = "Название слишком длинное! Максимум 17 символов.";
                    continue;
                }
                else
                {
                    name = inputName;
                    break;
                }
            }

            double weight = 5.0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ СОЗДАНИЕ ДОСПЕХА                         [ Шаг 2 из 3: Вес ]       ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ Введите вес доспеха в кг (например, 4.5 или 10.0):                 ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ [Enter] Использовать вес по умолчанию (5.0 кг)                     ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine($"Ошибка: {errorMessage}");
                    errorMessage = "";
                }

                Console.Write(" Вес (кг) > ");
                string weightInput = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(weightInput))
                {
                    weight = 5.0;
                    break;
                }

                if (double.TryParse(weightInput, out double parsedWeight) && parsedWeight >= 0)
                {
                    weight = parsedWeight;
                    break;
                }
                else
                {
                    errorMessage = "Неверный формат веса! Введите положительное число.";
                }
            }

            ArmorType armorType = ArmorType.Light;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ СОЗДАНИЕ ДОСПЕХА                         [ Шаг 3 из 3: Тип ]       ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ Выберите тип брони:                                                ║");
                Console.WriteLine("║   [1] Легкая броня   (КД 11 + ЛОВ)                                 ║");
                Console.WriteLine("║   [2] Средняя броня  (КД 14 + макс. 2 от ЛОВ)                      ║");
                Console.WriteLine("║   [3] Тяжелая броня  (КД 18 фиксированно)                          ║");
                Console.WriteLine("║   [4] Щит            (+2 к КД)                                     ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ [1-4] Выбрать тип                                                  ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine($"Ошибка: {errorMessage}");
                    errorMessage = "";
                }

                Console.Write(" Выберите тип (номер) > ");
                string typeInput = Console.ReadLine()?.Trim() ?? "";

                if (int.TryParse(typeInput, out int choice) && choice >= 1 && choice <= 4)
                {
                    armorType = choice switch
                    {
                        1 => ArmorType.Light,
                        2 => ArmorType.Medium,
                        3 => ArmorType.Heavy,
                        4 => ArmorType.Shield,
                        _ => ArmorType.Light
                    };
                    break;
                }
                else
                {
                    errorMessage = "Неверный выбор! Введите цифру от 1 до 4.";
                }
            }

            int acBonus = armorType switch
            {
                ArmorType.Light => 11,
                ArmorType.Medium => 14,
                ArmorType.Heavy => 18,
                ArmorType.Shield => 2,
                _ => 11
            };

            return new Armor(name, weight, armorType, acBonus);
        }

        private Weapon CreateWeaponBuild()
        {
            string name = "Оружие";
            while (true)
            {
                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║ СОЗДАНИЕ ОРУЖИЯ (Шаг 1 из 4)                                       ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ Введите название (максимум 17 символов):                           ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
                Console.Write(" Название > ");
                string inputName = Console.ReadLine()?.Trim() ?? "";

                if (string.IsNullOrEmpty(inputName))
                {
                    name = "Оружие";
                    break;
                }
                else if (inputName.Length > 17)
                {
                    Console.WriteLine("\n Название слишком длинное! Максимум 17 символов. Нажмите любую клавишу...");
                    Console.ReadKey();
                }
                else
                {
                    name = inputName;
                    break;
                }
            }

            Console.Write(" Вес (кг) > ");
            if (!double.TryParse(Console.ReadLine(), out double weight)) weight = 2.0;

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ СОЗДАНИЕ ОРУЖИЯ (Шаг 2 из 4)                                       ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Выберите кость урона:                                              ║");
            Console.WriteLine("║ [1] d4    [2] d6    [3] d8    [4] d10    [5] d12                   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write(" Выберите кубик (1-5) > ");

            DiceType diceType = Console.ReadLine()?.Trim() switch
            {
                "1" => DiceType.D4,
                "2" => DiceType.D6,
                "3" => DiceType.D8,
                "4" => DiceType.D10,
                "5" => DiceType.D12,
                _ => DiceType.D8
            };

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ СОЗДАНИЕ ОРУЖИЯ (Шаг 3 из 4)                                       ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Введите бонус к урону (0 для обычного, 1-3 для магического):        ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write(" Бонус урона > ");
            if (!int.TryParse(Console.ReadLine(), out int damageBonus)) damageBonus = 0;

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ СОЗДАНИЕ ОРУЖИЯ (Шаг 4 из 4)                                       ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Выберите тип урона:                                                ║");

            var damageTypes = Enum.GetValues<DamageType>();
            for (int i = 0; i < damageTypes.Length; i += 2)
            {
                string t1 = $"[{i + 1,2}] {damageTypes[i].ToRussian()}";
                string t2 = (i + 1 < damageTypes.Length) ? $"[{i + 2,2}] {damageTypes[i + 1].ToRussian()}" : "";
                string line = $"  {t1,-28} {t2}";
                Console.WriteLine($"║{line.PadRight(68)}║");
            }

            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write(" Выберите тип урона (номер) > ");

            DamageType damageType = DamageType.Slashing;
            if (int.TryParse(Console.ReadLine()?.Trim(), out int dtIndex) && dtIndex >= 1 &&
                dtIndex <= damageTypes.Length)
            {
                damageType = damageTypes[dtIndex - 1];
            }

            return new Weapon(name, weight, diceType, damageBonus, damageType);
        }

        private Potion? CreatePotionInteractive()
        {
            PotionEffectType selectedEffect = PotionEffectType.Heal;
            double weightPerItem = 1.0;
            int quantity = 1;
            int currentStep = 1;
            string errorMessage = "";

            while (currentStep >= 1 && currentStep <= 3)
            {
                switch (currentStep)
                {
                    case 1:
                    {
                        Console.Clear();
                        Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║ СОЗДАНИЕ ЗЕЛЬЯ                             [ Шаг 1 из 3: Тип ]     ║");
                        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                        Console.WriteLine("║ Выберите тип зелья:                                                ║");

                        var values = Enum.GetValues<PotionEffectType>();
                        int half = (values.Length + 1) / 2;
                        for (int i = 0; i < half; i++)
                        {
                            var leftType = values[i];
                            string leftStr = $"   [{i + 1,2}] {leftType.ToRussian()}";

                            string rightStr = "";
                            int rightIndex = i + half;
                            if (rightIndex < values.Length)
                            {
                                var rightType = values[rightIndex];
                                rightStr = $"   [{rightIndex + 1,2}] {rightType.ToRussian()}";
                            }

                            string rowLine = leftStr.PadRight(34) + rightStr;
                            Console.WriteLine($"║{rowLine.PadRight(68)}║");
                        }

                        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                        string menuLine = $" [1-{values.Length}] Выбрать зелье                  [0] Отмена";
                        Console.WriteLine($"║{menuLine.PadRight(68)}║");
                        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            Console.WriteLine($"Ошибка: {errorMessage}");
                            errorMessage = "";
                        }

                        Console.Write("Введите номер зелья > ");
                        var input = Console.ReadLine()?.Trim();

                        if (input == "0") return null;

                        if (int.TryParse(input, out var choice) && choice >= 1 && choice <= values.Length)
                        {
                            selectedEffect = values[choice - 1];
                            currentStep = 2;
                        }
                        else
                        {
                            errorMessage = $"Введите число от 1 до {values.Length}.";
                        }

                        break;
                    }
                    case 2:
                    {
                        Console.Clear();
                        Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║ СОЗДАНИЕ ЗЕЛЬЯ                             [ Шаг 2 из 3: Вес ]     ║");
                        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                        string infoLine = $" Выбранное зелье: {selectedEffect.ToRussian()}";
                        Console.WriteLine($"║{infoLine.PadRight(68)}║");
                        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                        Console.WriteLine("║ Введите вес ОДНОГО зелья в кг (например, 2,0 за баночку):          ║");
                        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                        string menuLine = " [0] Назад к выбору типа";
                        Console.WriteLine($"║{menuLine.PadRight(68)}║");
                        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            Console.WriteLine($"Ошибка: {errorMessage}");
                            errorMessage = "";
                        }

                        Console.Write("Введите вес одной штуки > ");
                        var input = Console.ReadLine()?.Trim();

                        if (input == "0")
                        {
                            currentStep = 1;
                            break;
                        }

                        if (double.TryParse(input, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.InvariantCulture, out double parsedWeight) &&
                            parsedWeight >= 0 ||
                            double.TryParse(input, System.Globalization.NumberStyles.Any,
                                System.Globalization.CultureInfo.GetCultureInfo("ru-RU"), out parsedWeight) &&
                            parsedWeight >= 0)
                        {
                            weightPerItem = parsedWeight;
                            currentStep = 3;
                        }
                        else
                        {
                            errorMessage = "Введите корректное число (например, 2.0 или 0.5).";
                        }

                        break;
                    }
                    case 3:
                    {
                        Console.Clear();
                        Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                        Console.WriteLine("║ СОЗДАНИЕ ЗЕЛЬЯ                         [ Шаг 3 из 3: Количество ]  ║");
                        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                        string infoLine = $" Зелье: {selectedEffect.ToRussian()} │ Вес 1 шт: {weightPerItem} кг";
                        Console.WriteLine($"║{infoLine.PadRight(68)}║");
                        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                        Console.WriteLine("║ Введите количество штук:                                           ║");
                        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                        string menuLine = " [0] Назад к весу";
                        Console.WriteLine($"║{menuLine.PadRight(68)}║");
                        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            Console.WriteLine($"Ошибка: {errorMessage}");
                            errorMessage = "";
                        }

                        Console.Write("Введите количество > ");
                        var input = Console.ReadLine()?.Trim();

                        if (input == "0")
                        {
                            currentStep = 2;
                            break;
                        }

                        if (int.TryParse(input, out int parsedQty) && parsedQty >= 1 && parsedQty <= 9)
                        {
                            quantity = parsedQty;
                            currentStep = 4;
                        }
                        else
                        {
                            errorMessage = "Введите число от 1 до 9.";
                        }

                        break;
                    }
                }
            }

            return new Potion(selectedEffect, weightPerItem, quantity);
        }

        private void ShowDeleteItemMenu()
        {
            if (character.Inventory.Count == 0)
            {
                Console.WriteLine("\nРюкзак пуст, нечего удалять!");
                Console.ReadKey();
                return;
            }

            Console.Write("\nВведите индекс предмета для удаления [0, 1, 2...]: ");
            if (int.TryParse(Console.ReadLine(), out int index))
            {
                if (index >= 0 && index < character.Inventory.Count)
                {
                    var item = character.Inventory[index];

                    if (item is IHasQuantity stackable && stackable.Quantity > 1)
                    {
                        Console.Write($"У вас в инвентаре {stackable.Quantity} шт. Сколько штук удалить? > ");
                        if (int.TryParse(Console.ReadLine(), out int qtyToRemove) && qtyToRemove > 0)
                        {
                            character.Inventory.DecreaseItemQuantity(index, qtyToRemove);
                        }
                    }
                    else
                    {
                        if (item == character.BodyArmor)
                        {
                            character.BodyArmor = null;
                        }
                        if (item == character.Shield)
                        {
                            character.Shield = null;
                        }
                        
                        character.Inventory.RemoveItemAt(index);
                    }
                }
                else
                {
                    Console.WriteLine("\nОшибка: Предмета с таким индексом не существует.");
                    Console.ReadKey();
                }
            }
        }

        private string GetItemDescription(Item item)
        {
            return item switch
            {
                Weapon w => $"Оружие (d{(int)w.DamageDice}, {w.DamageType.ToRussian()}" +
                            (w.DamageBonus != 0 ? $", +{w.DamageBonus}" : "") + ")",
                Armor a => $"Броня (КД +{a.ArmorClassBonus})",
                Potion => "Зелье",
                Equipment => "Снаряжение",
                _ => "Предмет"
            };
        }
    }
}