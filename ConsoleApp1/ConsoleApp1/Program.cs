using System.ComponentModel.Design;

namespace ConsoleApp1
{
    internal class Program
    {
        enum PlayerAction
        {
            Attack,
            Defent,
        }
        string int RandomDamage(int minDamage, int maxDamage)
        {
            // todo : arvo vahinko
            return 1;
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");

            int ritariHP = 15;
            int orcHP = 15;
            PlayerAction action = PlayerAction.Attack;
            // näytä tilanne
            Console.WriteLine($"Ritarin HP: {ritariHP} Örkin HP: {orcHP}");


            //nätyänkomeenot
            Console.WriteLine($"give command:\n1. {PlayerAction.Attack}.\n2.{PlayerAction.Defent}");
            //1.hyökkää 2. puilusta
            //while luuppi
            while (true)
            {
                //kysy komaennt

                //tellanna vastaus
                string? vastaus = Console.ReadLine();
                //joa vastsaus on 1 tai 2 hyväkay vastaus 
                if (action == PlayerAction.Attack)
                {
                    int damage = RandomDamage(1, 10);
                    Console.WriteLine($"Attack hit with {damage} damage");
                    orcHP -= damage;
                }
                else if (vastaus == "2")
                {
                    action = PlayerAction.Defent;
                    break;
                }
                //jos jotain muuta; kysy uudellen

                // jos while loppuu
                if (action == PlayerAction.Attack)
                {

                }
                else if (action == PlayerAction.Defent)
                {

                }
                //jos  hyökkää niin...

                //jos puolusaa .....
            }
        }
    }
}
