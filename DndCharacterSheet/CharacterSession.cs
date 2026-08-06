using System;
using System.Collections.Generic;
using DndCharacterSheet.ConsoleUI;

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
                    var diceMenu = new DiceRollerMenu(character);
                    diceMenu.Run();
                    break;
                case "2":
                    var inventoryMenu = new InventoryMenu(character);
                    inventoryMenu.Run();
                    break;
                case "3":
                    var damageMenu = new DamageAndHealingMenu(character);
                    damageMenu.Run();
                    break;
                case "4":
                    HandleLevelUp();
                    break;
                case "5":
                    HandleSave();
                    break;
                case "6":
                    var walletMenu = new WalletMenu(character);
                    walletMenu.Run();
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
    
    private void HandleLevelUp()
    {
        if (character.Level < 20)
        {
            character.LevelUp();
            Console.WriteLine($"\nУровень повышен! Теперь ваш персонаж {character.Level} уровня.");
            Console.WriteLine("Максимальное здоровье и текущее здоровье увеличены.");
        }
        else
        {
            Console.WriteLine("\nДостигнут максимальный 20 уровень!");
        }
        Console.ReadKey();
    }
    
    private void HandleSave()
    {
        string filePath = $"{character.Name}.json";
    
        CharacterStorage.Save(character, filePath);
    
        Console.WriteLine($"\nПерсонаж успешно сохранен в файл: {filePath}");
        Console.ReadKey();
    }
    
    private void DrawCharacterCard()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
        
        string left = $" {character.Name}";
        string right = $"[ {character.CharacterRace.ToRussian()} {character.CharacterClass.ToRussian()} - Ур. {character.Level} ] ";
        string headerLine = left + right.PadLeft(68 - left.Length);
        Console.WriteLine($"║{headerLine}║");
        
        Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");
        
        string hpBar = CharacterRender.GenerateHpBar(character.CurrentHealth, character.MaxHealth);
        string hpLine = $" HP: [{hpBar}] {character.CurrentHealth}/{character.MaxHealth}   КД: {character.ArmorClass}   БМ: +{character.ProficiencyBonus}   Золото: {character.Gold} $";
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
    
    private string FormatMod(int statValue)
    {
        int modifier = Character.CalculateModifier(statValue);
        return modifier >= 0 ? $"+{modifier}" : $"{modifier}";
    }

}