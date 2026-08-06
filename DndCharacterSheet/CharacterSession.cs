using System;
using System.Collections.Generic;

namespace DndCharacterSheet;

internal sealed class CharacterSession
{
    private readonly Character character;
    private const int InventoryCapacity = 20;

    public CharacterSession(Character character)
    {
        this.character = character;
    }
    
    internal void Run()
    {
        while (true)
        {
            Console.Clear();
            DrawCharacterCard();

            Console.Write("Выберите действие > ");
            var input = Console.ReadLine()?.Trim();

            switch (input)
            {
                case "1":
                    ShowDiceRollerMenu();
                    break;
                case "2":
                    ShowInventoryMenu();
                    break;
                case "3":
                    HandleDamageAndHealing(character);
                    break;
                case "4":
                    ShowStub("Поднять уровень");
                    break;
                case "5":
                    ShowStub("Сохранить в JSON");
                    break;
                case "0":
                    return; 
                default:
                    Console.WriteLine("Неверный выбор! Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    // Отрисовывает главную карточку персонажа в консоли (имя, класс, раса, ХП, КД)
    private void DrawCharacterCard()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
        
        string left = $" {character.Name}";
        string right = $"[ {character.CharacterRace.ToRussian()} {character.CharacterClass.ToRussian()} - Ур. {character.Level} ] ";
        string headerLine = left + right.PadLeft(68 - left.Length);
        Console.WriteLine($"║{headerLine}║");
        
        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
        
        string hpBar = GenerateHpBar(character.CurrentHealth, character.MaxHealth);
        string hpLine = $" HP: [{hpBar}] {character.CurrentHealth}/{character.MaxHealth}    КД: {character.ArmorClass}    БМ: +{character.ProficiencyBonus}    Золото: {character.Gold}";
        Console.WriteLine($"║{hpLine.PadRight(68)}║");
        
        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
        
        string statsLine1 = $" СИЛ: {character.Strength,2} ({FormatMod(character.Strength)})  │  ЛОВ: {character.Dexterity,2} ({FormatMod(character.Dexterity)})  │  ТЕЛ: {character.Constitution,2} ({FormatMod(character.Constitution)})";
        string statsLine2 = $" ИНТ: {character.Intelligence,2} ({FormatMod(character.Intelligence)})  │  МУД: {character.Wisdom,2} ({FormatMod(character.Wisdom)})  │  ХАР: {character.Charisma,2} ({FormatMod(character.Charisma)})";
        
        Console.WriteLine($"║{statsLine1.PadRight(68)}║");
        Console.WriteLine($"║{statsLine2.PadRight(68)}║");
        
        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
        
        string menu1 = " [1] Бросить кубик             [3] Получить урон / Лечение";
        string menu2 = " [2] Открыть инвентарь         [4] Поднять уровень";
        string menu3 = " [5] Сохранить в JSON          [0] Выход";
        string menu4 = " [6] Кошелек";
        
        Console.WriteLine($"║{menu1.PadRight(68)}║");
        Console.WriteLine($"║{menu2.PadRight(68)}║");
        Console.WriteLine($"║{menu3.PadRight(68)}║");
        Console.WriteLine($"║{menu4.PadRight(68)}║");
        
        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
    }
    
    // Генерирует визуальную текстовую шкалу здоровья (HP-бар)
    private string GenerateHpBar(int currentHp, int maxHp)
    {
        int totalBlocks = 14;
        if (maxHp <= 0) maxHp = 1;
        
        double percentage = (double)currentHp / maxHp;
        int filledBlocks = (int)Math.Round(percentage * totalBlocks);
        
        filledBlocks = Math.Clamp(filledBlocks, 0, totalBlocks);
        int emptyBlocks = totalBlocks - filledBlocks;
        
        return new string('■', filledBlocks) + new string('□', emptyBlocks);
    }
    
    // Управляет экраном выбора или распределения характеристик персонажа
    private (int statValue, string statName, int modifier) SelectAbilityStat()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║ ВЫБОР ХАРАКТЕРИСТИКИ                                               ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
        Console.WriteLine("║ [1] Сила          [3] Телосложение    [5] Мудрость                 ║");
        Console.WriteLine("║ [2] Ловкость      [4] Интеллект       [6] Харизма                  ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
        Console.Write(" Выберите характеристику > ");

        var input = Console.ReadLine()?.Trim();
        int stat = input switch
        {
            "1" => character.Strength,
            "2" => character.Dexterity,
            "3" => character.Constitution,
            "4" => character.Intelligence,
            "5" => character.Wisdom,
            "6" => character.Charisma,
            _ => character.Strength
        };

        string name = input switch
        {
            "1" => "Силы",
            "2" => "Ловкости",
            "3" => "Телосложения",
            "4" => "Интеллекта",
            "5" => "Мудрости",
            "6" => "Харизмы",
            _ => "Силы"
        };

        int mod = Character.CalculateModifier(stat);
        return (stat, name, mod);
    }
    
    // Управляет экрано лечения или полученя урона
    private void HandleDamageAndHealing(Character character) 
    {
        string errorMessage = "";

        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ УРОН И ЛЕЧЕНИЕ                                                     ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            
            string infoLine1 = $" Имя: {character.Name} │ Класс: {character.CharacterClass.ToRussian()}";
            Console.WriteLine($"║{infoLine1.PadRight(68)}║");
            
            string hpBar = GenerateHpBar(character.CurrentHealth, character.MaxHealth);
            string infoLine2 = $" HP: [{hpBar}] {character.CurrentHealth} / {character.MaxHealth}";
            Console.WriteLine($"║{infoLine2.PadRight(68)}║");
            
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Выберите действие:                                                 ║");
            Console.WriteLine("║   [1] Получить урон        (уменьшить текущее HP)                  ║");
            Console.WriteLine("║   [2] Получить лечение     (восстановить HP)                       ║");
            Console.WriteLine("║   [0] Назад в меню                                                 ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            string menuLine = " [1-2] Выбрать действие               [0] Назад в главное меню      ";
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
                break;
            }

            if (input == "1" || input == "2")
            {
                bool isDamage = input == "1";
                string actionName = isDamage ? "урона" : "лечения";

                Console.Write($"Введите количество {actionName} > ");
                var valueInput = Console.ReadLine();

                if (int.TryParse(valueInput, out var amount) && amount >= 0)
                {
                    if (isDamage)
                    {
                        character.TakeDamage(amount);
                        Console.WriteLine($"Получено урона: {amount}. Текущее HP: {character.CurrentHealth}/{character.MaxHealth}");
                    }
                    else
                    {
                        character.Heal(amount);
                        Console.WriteLine($"Восстановлено HP: {amount}. Текущее HP: {character.CurrentHealth}/{character.MaxHealth}");
                    }

                    Console.WriteLine("Нажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                }
                else
                {
                    errorMessage = "Введите корректное положительное число!";
                }
            }
            else
            {
                errorMessage = "Неверный выбор! Введите 1, 2 или 0.";
            } 
        }
    }

    // Открывает интерактивное меню бросков кубиков
    private void ShowDiceRollerMenu()
    {
        DiceRoller roller = new DiceRoller();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ ГЕНЕРАТОР БРОСКОВ                                                  ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Выберите тип броска:                                               ║");
            Console.WriteLine("║ [1] Проверка характеристики                                        ║");
            Console.WriteLine("║ [2] Атака оружием (с выбором характеристики)                       ║");
            Console.WriteLine("║ [3] Атака заклинанием                                              ║");
            Console.WriteLine("║ [0] Назад в меню                                                   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write(" Выберите действие > ");

            var mainChoice = Console.ReadLine()?.Trim();
            if (mainChoice == "0") return;

            if (mainChoice == "2")
            {
                Weapon? weapon = SelectWeaponForAttack();
                if (weapon == null) continue;

                var (_, statName, statMod) = SelectAbilityStat();
                int profBonus = character.ProficiencyBonus;

                int attackRollRaw = roller.Roll(DiceType.D20);
                int totalAttack = attackRollRaw + statMod + profBonus;

                int weaponDamageRaw = roller.Roll(weapon.DamageDice);
                int totalDamage = Math.Max(1, weaponDamageRaw + statMod + weapon.DamageBonus);

                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                string weaponHeader = $" АТАКА ОРУЖИЕМ (Ур. {character.Level} | Мастерство: +{profBonus})";
                Console.WriteLine($"║{weaponHeader.PadRight(68)}║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                
                string wNameLine = $" Оружие: {weapon.Name} ({weapon.DamageType.ToRussian()})";
                Console.WriteLine($"║{wNameLine.PadRight(68)}║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║    ┌──────────────────────────────────────────────────────────┐    ║");
                
                string a1 = " БРОСОК АТАКИ (Попадание):";
                string a2 = $"    d20: {attackRollRaw} | Мод.{statName}: {statMod:+0;-0;0} | Мастерство: +{profBonus}";
                string a3 = $"    ИТОГ АТАКИ: {totalAttack}";
                
                string d1 = " БРОСОК УРОНА:";
                string d2 = $"    Кубик d{(int)weapon.DamageDice}: {weaponDamageRaw} | Мод.{statName}: {statMod:+0;-0;0} | Оружие: +{weapon.DamageBonus}";
                string d3 = $"    ИТОГ УРОНА: {totalDamage} ({weapon.DamageType.ToRussian()})";

                Console.WriteLine($"║    │{a1.PadRight(58)}│    ║");
                Console.WriteLine($"║    │{a2.PadRight(58)}│    ║");
                Console.WriteLine($"║    │{a3.PadRight(58)}│    ║");
                Console.WriteLine($"║    │{"".PadRight(58)}│    ║");
                Console.WriteLine($"║    │{d1.PadRight(58)}│    ║");
                Console.WriteLine($"║    │{d2.PadRight(58)}│    ║");
                Console.WriteLine($"║    │{d3.PadRight(58)}│    ║");
                
                Console.WriteLine("║    └──────────────────────────────────────────────────────────┘    ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ [Нажмите любую клавишу для продолжения...]                         ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
                Console.ReadKey();
                continue;
            }

            if (mainChoice == "3")
            {
                var (_, statName, statMod) = SelectAbilityStat();
                int profBonus = character.ProficiencyBonus;

                int spellRollRaw = roller.Roll(DiceType.D20);
                int totalSpellAttack = spellRollRaw + statMod + profBonus;

                Console.Clear();
                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                string spellHeader = $" АТАКА ЗАКЛИНАНИЕМ (Ур. {character.Level} | Мастерство: +{profBonus})";
                Console.WriteLine($"║{spellHeader.PadRight(68)}║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║    ┌──────────────────────────────────────────────────────────┐    ║");
                
                string s1 = " БРОСОК АТАКИ ЗАКЛИНАНИЕМ:";
                string s2 = $"    d20: {spellRollRaw} | Мод.{statName}: {statMod:+0;-0;0} | Мастерство: +{profBonus}";
                string s3 = $"    ИТОГ АТАКИ ЗАКЛИНАНИЕМ: {totalSpellAttack}";

                Console.WriteLine($"║    │{s1.PadRight(58)}│    ║");
                Console.WriteLine($"║    │{s2.PadRight(58)}│    ║");
                Console.WriteLine($"║    │{s3.PadRight(58)}│    ║");
                
                Console.WriteLine("║    └──────────────────────────────────────────────────────────┘    ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
                Console.WriteLine("║ [Нажмите любую клавишу для продолжения...]                         ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
                Console.ReadKey();
                continue;
            }

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ ГЕНЕРАТОР БРОСКОВ                                                  ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Выберите кость:                                                    ║");
            Console.WriteLine("║ [1] d4    [2] d6    [3] d8    [4] d10    [5] d12    [6] d20        ║");
            Console.WriteLine("║ [0] Назад                                                          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write(" Выберите кость (1-6) > ");

            var diceInput = Console.ReadLine()?.Trim();
            if (diceInput == "0") continue;

            DiceType diceType = diceInput switch
            {
                "1" => DiceType.D4,
                "2" => DiceType.D6,
                "3" => DiceType.D8,
                "4" => DiceType.D10,
                "5" => DiceType.D12,
                "6" => DiceType.D20,
                _ => DiceType.D20
            };

            var (_, statNameStr, modifier) = SelectAbilityStat();
            int rawRoll = roller.Roll(diceType);
            int total = rawRoll + modifier;
            string modString = modifier >= 0 ? $"+{modifier}" : $"{modifier}";

            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ ГЕНЕРАТОР БРОСКОВ                                                  ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            
            string selectedLine = $" Выбран кубик: d{(int)diceType} (Проверка {statNameStr})";
            Console.WriteLine($"║{selectedLine.PadRight(68)}║");
            
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║    ┌──────────────────────────────────────────────────────────┐    ║");
            
            string r1 = " РЕЗУЛЬТАТ БРОСКА:";
            string r2 = $" Выпало на кубике: {rawRoll}";
            string r3 = $" Модификатор {statNameStr}: {modString}";
            string r4 = $" ИТОГО: {total}";

            Console.WriteLine($"║    │{r1.PadRight(58)}│    ║");
            Console.WriteLine($"║    │{r2.PadRight(58)}│    ║");
            Console.WriteLine($"║    │{r3.PadRight(58)}│    ║");
            Console.WriteLine($"║    │{r4.PadRight(58)}│    ║");
            
            Console.WriteLine("║    └──────────────────────────────────────────────────────────┘    ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ [Нажмите любую клавишу, чтобы вернуться в меню...]                 ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            
            Console.ReadKey();
        }
    }

    // Позволяет выбрать оружие из инвентаря для атаки
    private Weapon? SelectWeaponForAttack()
    {
        var weapons = new List<Weapon>();
        
        for (int i = 0; i < InventoryCapacity; i++)
        {
            try
            {
                if (character.Inventory[i] is Weapon w)
                {
                    weapons.Add(w);
                }
            }
            catch
            {
                break;
            }
        }

        if (weapons.Count == 0)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ ОШИБКА АТАКИ                                                       ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ В вашем инвентаре нет оружия! Добавьте его через меню инвентаря.   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write("\n Нажмите любую клавишу для возврата...");
            Console.ReadKey();
            return null;
        }

        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║ ВЫБОР ОРУЖИЯ ДЛЯ АТАКИ                                             ║");
        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
        
        for (int i = 0; i < weapons.Count; i++)
        {
            var w = weapons[i];
            string bonusStr = w.DamageBonus != 0 ? $", +{w.DamageBonus}" : "";
            string weaponInfo = $" [{i}] {w.Name,-15} │ d{(int)w.DamageDice}, {w.DamageType.ToRussian()}{bonusStr}";
            Console.WriteLine($"║{weaponInfo.PadRight(68)}║");
        }
        
        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
        Console.Write(" Выберите номер оружия > ");

        if (int.TryParse(Console.ReadLine()?.Trim(), out int index) && index >= 0 && index < weapons.Count)
        {
            return weapons[index];
        }

        return null;
    }
    
    // Отображает главное меню инвентаря, текущий вес и список предметов
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
            
            string header = $" ИНВЕНТАРЬ (Вес: {character.Inventory.GetTotalWeight():F1} / {character.Inventory.MaxWeight:F1} кг)";
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
                    
                    double totalItemWeight = item is IHasQuantity stackable ? item.Weight * stackable.Quantity : item.Weight;
                    
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
    
    // Открывает меню управления экипировкой (надевание/снятие брони и щитов)
    private void ShowEquipmentMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ УПРАВЛЕНИЕ СНАРЯЖЕНИЕМ                                             ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            
            string bodyName = character.BodyArmor != null ? $"{character.BodyArmor.Name} [Экипирован]" : "Без брони";
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
                    string isEquippedMark = (character.BodyArmor == armor || character.Shield == armor) ? " [Экипирован]" : "";
                    
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

            if (input == "b")
            {
                break;
            }

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
                    if (character.Shield == selectedArmor)
                    {
                        character.Shield = null;
                    }
                    else
                    {
                        character.Shield = selectedArmor;
                    }
                }
                else
                {
                    if (character.BodyArmor == selectedArmor)
                    {
                        character.BodyArmor = null;
                    }
                    else
                    {
                        character.BodyArmor = selectedArmor;
                    }
                }
            }
        }
    }
    
    // Показывает меню выбора категории для добавления нового предмета
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
        Console.WriteLine("║ [5] Своё (кастомный предмет)                                       ║");
        Console.WriteLine("║ [0] Отмена                                                         ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
        Console.Write(" Выберите категорию > ");

        var choice = Console.ReadLine()?.Trim();
        if (choice == "0" || string.IsNullOrEmpty(choice)) return;

        Item? newItem = null;

        if (choice == "5")
        {
            Console.Clear();
            Console.WriteLine(" Выберите базовую категорию для своего предмета:");
            Console.WriteLine(" [1] Оружие  [2] Доспех  [3] Зелье  [4] Снаряжение");
            Console.Write(" > ");
            var subChoice = Console.ReadLine()?.Trim();

            if (subChoice == "1")
            {
                newItem = CreateWeaponBuild();
            }
            else
            {
                Console.Write("Введите название предмета > ");
                string customName = Console.ReadLine()?.Trim() ?? "Предмет";

                Console.Write("Введите вес предмета (кг) > ");
                if (double.TryParse(Console.ReadLine(), out double customWeight))
                {
                    newItem = subChoice switch
                    {
                        "2" => new Armor(customName, customWeight, ArmorType.Light, 11),
                        "3" => new Potion(PotionEffectType.Heal, customWeight, 1),
                        _ => new Equipment(customName, customWeight, 1)
                    };
                }
            }
        }
        else
        {
            switch (choice)
            {
                case "1":
                    newItem = CreateWeaponBuild();
                    break;
                case "2":
                    newItem = CreateArmorBuild();
                    break;
                case "3":
                    newItem = CreatePotionInteractive();
                    break;
                case "4":
                    Console.Write("Введите название снаряжения > ");
                    string eqName = Console.ReadLine()?.Trim() ?? "Снаряжение";
                    Console.Write("Введите вес (кг) > ");
                    double eqWeight = double.TryParse(Console.ReadLine(), out var ew) ? ew : 1.0;
                    newItem = new Equipment(eqName, eqWeight, 1);
                    break;
            }
        }

        if (newItem != null)
        {
            character.Inventory.AddItem(newItem);
        }
    }

    // Пошаговый интерактивный мастер создания доспеха ---
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

    // Пошаговый интерактивный мастер создания оружия ---
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
        if (int.TryParse(Console.ReadLine()?.Trim(), out int dtIndex) && dtIndex >= 1 && dtIndex <= damageTypes.Length)
        {
            damageType = damageTypes[dtIndex - 1];
        }

        return new Weapon(name, weight, diceType, damageBonus, damageType);
    }
    
    // Пошаговый интерактивный мастер создания зелья ---
    internal Potion? CreatePotionInteractive()
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

                    if (input == "0")
                    {
                        return null;
                    }

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

                    if (double.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double parsedWeight) && parsedWeight >= 0)
                    {
                        weightPerItem = parsedWeight;
                        currentStep = 3;
                    }
                    else if (double.TryParse(input, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.GetCultureInfo("ru-RU"), out parsedWeight) && parsedWeight >= 0)
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
    
    // Открывает интерфейс для удаления предметов из рюкзака
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
    
    // Возвращает отформатированную текстовую строку с описанием предмета
    private string GetItemDescription(Item item)
    {
        return item switch
        {
            Weapon w => $"Оружие (d{(int)w.DamageDice}, {w.DamageType.ToRussian()}" + (w.DamageBonus != 0 ? $", +{w.DamageBonus}" : "") + ")",
            Armor a => $"Броня (КД +{a.ArmorClassBonus})",
            Potion => "Зелье",
            Equipment => "Снаряжение",
            _ => "Предмет"
        };
    }
    
    // Превращает значение характеристики в строку модификатора (например, "+2" или "-1")
    private string FormatMod(int statValue)
    {
        int modifier = Character.CalculateModifier(statValue);
        return modifier >= 0 ? $"+{modifier}" : $"{modifier}";
    }
    
    private void ShowStub(string actionName)
    {
        Console.Clear();
        Console.WriteLine($"\n[Функция '{actionName}' находится в разработке]");
        Console.WriteLine("Нажмите любую клавишу, чтобы вернуться...");
        Console.ReadKey();
    }
}