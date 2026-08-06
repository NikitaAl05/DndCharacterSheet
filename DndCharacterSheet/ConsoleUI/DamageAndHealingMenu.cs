using System;

namespace DndCharacterSheet.ConsoleUI
{
    internal class DamageAndHealingMenu
    {
        private readonly Character character;

        public DamageAndHealingMenu(Character character)
        {
            this.character = character;
        }

        public void Run()
        {
            HandleDamageAndHealing();
        }

        private void HandleDamageAndHealing() 
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
                
                string hpBar = CharacterRender.GenerateHpBar(character.CurrentHealth, character.MaxHealth);
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
        
    }
}