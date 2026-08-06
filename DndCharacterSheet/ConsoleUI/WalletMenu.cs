using System;

namespace DndCharacterSheet.ConsoleUI
{
    internal class WalletMenu
    {
        private readonly Character character;

        public WalletMenu(Character character)
        {
            this.character = character;
        }

        public void Run()
        {
            ShowWalletMenu();
        }

        private void ShowWalletMenu()
        {
            while (true)
            {
                Console.Clear();

                Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
                
                string title = " КОШЕЛЕК";
                Console.WriteLine($"║{title.PadRight(68)}║");

                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");

                string balanceLine = $" Баланс: {character.Gold} $";
                Console.WriteLine($"║{balanceLine.PadRight(68)}║");

                Console.WriteLine("╠════════════════════════════════════════════════════════════════════╣");

                string opt1 = " [1] Добавить золото";
                string opt2 = " [2] Потратить золото";
                string opt0 = " [0] Назад";

                Console.WriteLine($"║{opt1.PadRight(68)}║");
                Console.WriteLine($"║{opt2.PadRight(68)}║");
                Console.WriteLine($"║{opt0.PadRight(68)}║");

                Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

                Console.Write("Выберите действие > ");
                var input = Console.ReadLine()?.Trim();

                if (input == "0")
                {
                    break;
                }
                else if (input == "1")
                {
                    Console.Write("Введите сумму для добавления > ");
                    if (int.TryParse(Console.ReadLine()?.Trim(), out int addAmount) && addAmount > 0)
                    {
                        if (character.Gold + addAmount > 9999999)
                        {
                            character.Gold = 9999999;
                            Console.WriteLine("\nКошелек переполнен! Больше золото не помещается.");
                            Console.ReadKey();
                        }
                        else
                        {
                            character.Gold += addAmount;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nОшибка: введите корректное положительное число.");
                        Console.ReadKey();
                    }
                }
                else if (input == "2")
                {
                    Console.Write("Введите сумму для списания > ");
                    if (int.TryParse(Console.ReadLine()?.Trim(), out int spendAmount) && spendAmount > 0)
                    {
                        if (character.Gold >= spendAmount)
                        {
                            character.Gold -= spendAmount;
                        }
                        else
                        {
                            Console.WriteLine("\nОшибка: недостаточно средств в кошельке!");
                            Console.ReadKey();
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nОшибка: введите корректное положительное число.");
                        Console.ReadKey();
                    }
                }
            }
        }
    }
}