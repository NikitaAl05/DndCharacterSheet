namespace DndCharacterSheet.ConsoleUI;

internal class DiceRollerMenu
{
    private readonly Character character;
    private readonly CharacterRender renderer;
    private const int InventoryCapacity = 20;

    public DiceRollerMenu(Character character)
    {
        this.character = character;
        this.renderer = new CharacterRender(character);
    }

    public void Run()
    {
        DiceRoller roller = new DiceRoller();

        while (true)
        {
            renderer.DrawDiceMainMenu();
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

                renderer.DrawWeaponAttackResult(character.Level, profBonus, weapon, attackRollRaw, statName, statMod, totalAttack, weaponDamageRaw, totalDamage);
                Console.ReadKey();
                continue;
            }

            if (mainChoice == "3")
            {
                var (_, statName, statMod) = SelectAbilityStat();
                int profBonus = character.ProficiencyBonus;

                int spellRollRaw = roller.Roll(DiceType.D20);
                int totalSpellAttack = spellRollRaw + statMod + profBonus;

                renderer.DrawSpellAttackResult(character.Level, profBonus, spellRollRaw, statName, statMod, totalSpellAttack);
                Console.ReadKey();
                continue;
            }

            renderer.DrawDiceTypeSelectionMenu();
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

            renderer.DrawSimpleRollResult(diceType, statNameStr, rawRoll, modifier, total);
            Console.ReadKey();
        }
    }

    private (int statValue, string statName, int modifier) SelectAbilityStat()
    {
        renderer.DrawAbilityStatSelectionMenu();
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
            renderer.DrawNoWeaponsErrorScreen();
            return null;
        }

        renderer.DrawWeaponSelectionMenu(weapons);
        if (int.TryParse(Console.ReadLine()?.Trim(), out int index) && index >= 0 && index < weapons.Count)
        {
            return weapons[index];
        }

        return null;
    }
}