using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnemyReader_JSON
{
    internal class Enemy
    {
        public string name { get; set; } = "";
        public int hitpoints { get; set; }
        public int stamina { get; set; }
        public int attack_damage { get; set; }
        public int mana { get; set; }


        public override string ToString()
        {
            return $"name: {name}\n" +
                   $"hitpoints: {hitpoints}\n" +
                   $"stamina: {stamina}\n" +
                   $"attack_damage: {attack_damage}\n" +
                   $"mana: {mana}";
        }
    }
}
