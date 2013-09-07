using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace wServer.realm.entities.player
{
    public class Combinations
    {
        public Dictionary<string[], Tuple<string, int>> combos = new Dictionary<string[], Tuple<string, int>>();
        public List<string> Recipes = new List<string>();
        public Dictionary<string[], Tuple<string, int, bool>> advCombos = new Dictionary<string[], Tuple<string, int, bool>>();

        public List<int> SlotList = new List<int>();

        public Tuple<string, int> Combo = new Tuple<string, int>("Short Sword", 0);

        public Combinations()
        {
            // ---- ADVANCED COMBOS ---- //
            AddAdvCombo("Robe of the Sphinx", 12000, "Robe of the Ancient Intellect", "Robe of the Star Mother", "Robe of the Grand Sorcerer");
            AddAdvCombo("Armor of the Nile", 12000, "Annihilation Armor", "Dominion Armor", "Acropolis Armor");
            AddAdvCombo("Pyramid Leather Armor", 12000, "Hydra Skin Armor", "Wyrmhide Armor", "Leviathan Armor");


            // ---- Pickaxes ---- //
            AddCombo("Rusty Pickaxe", 500, "Old Rusty Pickaxe");
            AddCombo("Old Copper Pickaxe", 1000, "Rusty Pickaxe", "Copper Nugget", "Copper Nugget", "Copper Nugget", "Copper Nugget", "Copper Nugget", "Copper Nugget");
            AddCombo("Copper Pickaxe", 500, "Old Copper Pickaxe");
            AddCombo("Old Iron Pickaxe", 1000, "Copper Pickaxe", "Iron Nugget", "Iron Nugget", "Iron Nugget", "Iron Nugget", "Iron Nugget", "Iron Nugget");
            AddCombo("Iron Pickaxe", 500, "Old Iron Pickaxe");
            AddCombo("Old Silver Pickaxe", 1000, "Iron Pickaxe", "Silver Nugget", "Silver Nugget", "Silver Nugget", "Silver Nugget", "Silver Nugget", "Silver Nugget");
            AddCombo("Silver Pickaxe", 500, "Old Silver Pickaxe");
            AddCombo("Old Gold Pickaxe", 1000, "Silver Pickaxe", "Gold Nugget", "Gold Nugget", "Gold Nugget", "Gold Nugget", "Gold Nugget", "Gold Nugget");
            AddCombo("Gold Pickaxe", 500, "Old Gold Pickaxe");
            AddCombo("Vlux Pickaxe", 1000, "Gold Pickaxe", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget");
            AddCombo("Enhanced Vlux Pickaxe", 1000, "Vlux Pickaxe");

            // ---- Helmets ---- //
            AddCombo("Copper Helm", 500, "Rusty Helm", "Copper Nugget", "Copper Nugget", "Copper Nugget", "Copper Nugget", "Copper Nugget", "Copper Nugget");
            AddCombo("Iron Helm", 500, "Copper Helm", "Iron Nugget", "Iron Nugget", "Iron Nugget", "Iron Nugget", "Iron Nugget", "Iron Nugget");
            AddCombo("Silver Helm", 500, "Iron Helm", "Silver Nugget", "Silver Nugget", "Silver Nugget", "Silver Nugget", "Silver Nugget", "Silver Nugget");
            AddCombo("Gold Helm", 500, "Silver Helm", "Gold Nugget", "Gold Nugget", "Gold Nugget", "Gold Nugget", "Gold Nugget", "Gold Nugget");
            AddCombo("Golden Vlux Helm", 1000, "Gold Helm", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget");

            // ---- Armors ---- //
            AddCombo("Copper Armor", 300, "Rusty Armor", "Copper Nugget", "Copper Nugget", "Copper Nugget", "Copper Nugget");
            AddCombo("Iron Armor", 300, "Copper Armor", "Iron Nugget", "Iron Nugget", "Iron Nugget", "Iron Nugget");
            AddCombo("Silver Armor", 300, "Iron Armor", "Silver Nugget", "Silver Nugget", "Silver Nugget", "Silver Nugget");
            AddCombo("Gold Armor", 300, "Silver Armor", "Gold Nugget", "Gold Nugget", "Gold Nugget", "Gold Nugget");
            AddCombo("Golden Vlux Armor", 600, "Gold Armor", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget");

            // ---- Rings ---- //      NOTE: THE RINGS HAVE NOT BEEN FULLY MADE YET
            AddCombo("Copper Ring", 300, "Rusty Ring", "Copper Nugget", "Copper Nugget", "Copper Nugget", "Copper Nugget");
            AddCombo("Iron Ring", 300, "Copper Ring", "Iron Nugget", "Iron Nugget", "Iron Nugget", "Iron Nugget");
            AddCombo("Silver Ring", 300, "Iron Ring", "Silver Nugget", "Silver Nugget", "Silver Nugget", "Silver Nugget");
            AddCombo("Gold Ring", 300, "Silver Ring", "Gold Nugget", "Gold Nugget", "Gold Nugget", "Gold Nugget");
            AddCombo("Golden Vlux Ring", 600, "Gold Ring", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget", "Vlux Nugget");
        }

        public bool SetCombo(string[] items)
        {
            foreach (var i in combos)
            {
                if (i.Key.Length == items.Length)
                {
                    bool pass = true;
                    foreach (var e in items)
                    {
                        if (!i.Key.Contains(e))
                        {
                            pass = false;
                        }
                    }
                    if (pass)
                    {
                        Combo = i.Value;
                        return true;
                    }
                }
            }
            return false;
        }

        public bool SetComboAdv(string[] items, bool forgeAmmy)
        {
            foreach (var i in advCombos)
            {
                if (i.Key.Length == items.Length)
                {
                    bool pass = true;
                    foreach (var e in items)
                    {
                        if (!i.Key.Contains(e))
                        {
                            pass = false;
                        }
                        else
                        {
                            if (i.Value.Item3 && !forgeAmmy)
                                pass = false;
                        }
                    }
                    if (pass)
                    {
                        Combo = new Tuple<string, int>(i.Value.Item1, i.Value.Item2);
                        return true;
                    }
                }
            }
            return false;
        }

        public void AddCombo(string result, int price, params string[] items)
        {
            combos.Add(items, new Tuple<string, int>(result, price));
            advCombos.Add(items, new Tuple<string, int, bool>(result, price, false));
        }

        public void AddAdvCombo(string result, int price, params string[] items)
        {
            advCombos.Add(items, new Tuple<string, int, bool>(result, price, true));
        }
    }
}
