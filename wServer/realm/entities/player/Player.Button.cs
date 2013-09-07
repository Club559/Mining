using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.cliPackets;
using wServer.svrPackets;

namespace wServer.realm.entities
{
    partial class Player
    {
        public void TextBoxButton(TextBoxButtonPacket pkt)
        {
            int button = pkt.Button;
            string type = pkt.Type;
            if (type == "NewClient")
            {
                psr.Disconnect();
            }
            if (type.Substring(0, 6) == "RBPage" && button == 2)
            {
                RecipePage(type);
            }
        }

        public void RecipePage(string type)
        {
            try
            {
                string pNum = type.Substring(6);
                if (pNum != "2")
                {
                    int newNum = Convert.ToInt32(pNum) + 1;
                    psr.SendPacket(new TextBoxPacket()
                    {
                        Button1 = "Close",
                        Button2 = "Next",
                        Message = GetRecipePage(pNum),
                        Title = "Recipes",
                        Type = "RBPage" + newNum
                    });
                }
                else
                {
                    psr.SendPacket(new TextBoxPacket()
                    {
                        Button1 = "Close",
                        Button2 = "First",
                        Message = GetRecipePage(pNum),
                        Title = "Recipes",
                        Type = "RBPage1"
                    });
                }
            }
            catch { }
        }

        public string GetRecipePage(string page)
        {
            switch (page)
            {
                case "1":
                    return @"<b>Upgrade Old Pickaxe</b>: Old Pickaxe + 500 gold
<b>Next Tier Pickaxe</b>: Normal Pickaxe + 6 Ore + 1000 gold
<b>Next Tier Helmet</b>: Helmet + 6 Ore + 500 gold";
                case "2":
                    return @"<b>Next Tier Armor</b>: Armor + 4 Ore + 300 gold
<b>Next Tier Ring</b>: Ring + 4 Ore + 300 gold";
                default:
                    return "Page not found";
            }
        }
    }
}
