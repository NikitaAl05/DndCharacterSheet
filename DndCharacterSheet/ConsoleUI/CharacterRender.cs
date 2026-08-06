namespace DndCharacterSheet.ConsoleUI;

internal class CharacterRender
{
    private readonly Character character;
    
    public CharacterRender(Character character)
    {
        this.character = character;
    }
    
    public static string GenerateHpBar(int currentHp, int maxHp)
    {
        int totalBlocks = 14;
        if (maxHp <= 0) maxHp = 1;
                
        double percentage = (double)currentHp / maxHp;
        int filledBlocks = (int)Math.Round(percentage * totalBlocks);
                
        filledBlocks = Math.Clamp(filledBlocks, 0, totalBlocks);
        int emptyBlocks = totalBlocks - filledBlocks;
                
        return new string('■', filledBlocks) + new string('□', emptyBlocks);
    }
    
    #region Броски кубиков и боевой интерфейс
        public void DrawDiceMainMenu()
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
        }
        
        public void DrawWeaponAttackResult(int level, int profBonus, Weapon weapon, int attackRollRaw, string statName, int statMod, int totalAttack, int weaponDamageRaw, int totalDamage)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            string weaponHeader = $" АТАКА ОРУЖИЕМ (Ур. {level} | Мастерство: +{profBonus})";
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
        }
        
        public void DrawSpellAttackResult(int level, int profBonus, int spellRollRaw, string statName, int statMod, int totalSpellAttack)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            string spellHeader = $" АТАКА ЗАКЛИНАНИЕМ (Ур. {level} | Мастерство: +{profBonus})";
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
        }
        
        public void DrawDiceTypeSelectionMenu()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ ГЕНЕРАТОР БРОСКОВ                                                  ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ Выберите кость:                                                    ║");
            Console.WriteLine("║ [1] d4    [2] d6    [3] d8    [4] d10    [5] d12    [6] d20        ║");
            Console.WriteLine("║ [0] Назад                                                          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write(" Выберите кость (1-6) > ");
        }
        
        public void DrawSimpleRollResult(DiceType diceType, string statNameStr, int rawRoll, int modifier, int total)
        {
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
        }
        
        public void DrawAbilityStatSelectionMenu()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ ВЫБОР ХАРАКТЕРИСТИКИ                                               ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ [1] Сила          [3] Телосложение    [5] Мудрость                 ║");
            Console.WriteLine("║ [2] Ловкость      [4] Интеллект       [6] Харизма                  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write(" Выберите характеристику > ");
        }
        
        public void DrawNoWeaponsErrorScreen()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║ ОШИБКА АТАКИ                                                       ║");
            Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
            Console.WriteLine("║ В вашем инвентаре нет оружия! Добавьте его через меню инвентаря.   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
            Console.Write("\n Нажмите любую клавишу для возврата...");
            Console.ReadKey();
        }
        
        public void DrawWeaponSelectionMenu(List<Weapon> weapons)
        {
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
        }
    
    #endregion

}