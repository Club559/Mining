using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.realm.worlds
{
    public class Mine : World
    {
        public int Level = 0;
        public Position PortalPos = new Position();
        public Dictionary<int, Tuple<string, int>> OresList = new Dictionary<int, Tuple<string, int>>();
        public int RoomsFound = 0;
        Entity mainPortal;
        public Mine(int level = 0)
        {
            Name = "Underground Mine - Level " + level;
            Background = 0;
            AllowTeleport = true;
            Mining = true;
            Level = level;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.mine.wmap"));

            InitOres();
            Init(level);
        }

        public List<Tuple<short,int>> GetOres()
        {
            List<Tuple<short, int>> ret = new List<Tuple<short, int>>();

            foreach (var i in OresList)
                if (i.Key <= Level)
                    ret.Add(new Tuple<short,int>(XmlDatas.IdToType[i.Value.Item1], i.Value.Item2));

            return ret;
        }

        public void OreLevel(int level, string name, int rarity)
        {
            OresList.Add(level, new Tuple<string, int>(name, rarity));
        }

        public virtual void InitOres()
        {
            OreLevel(0, "Copper Mine", 30);
            OreLevel(2, "Iron Mine", 30);
            OreLevel(4, "Silver Mine", 30);
            OreLevel(6, "Gold Mine", 30);
            OreLevel(10, "Vlux Mine", 50);
        }

        void Init(int lvl)
        {
            for (int ty = 0; ty < Map.Height; ty++)
                for (int tx = 0; tx < Map.Width; tx++)
                {
                    if (Map[(int)tx, (int)ty].Region == TileRegion.Hallway)
                    {
                        PortalPos.X = tx;
                        PortalPos.Y = ty;
                    }
                }
            /*var prtal = Portal.Resolve(0x1972);
            prtal.Move(PortalPos.X + 0.5f, PortalPos.Y + 0.5f);
            EnterWorld(prtal);*/

            mainPortal = Portal.Resolve(0x1973);
            mainPortal.Move(PortalPos.X + 1f, PortalPos.Y + 0.5f);
            EnterWorld(mainPortal);
        }

        public virtual void PlacePortal()
        {
            mainPortal.Move(PortalPos.X + 1.5f, PortalPos.Y + 0.5f);

            var prtal = Portal.Resolve(0x1972);
            prtal.Move(PortalPos.X + 0.5f, PortalPos.Y + 0.5f);
            EnterWorld(prtal);

            BroadcastPacket(new TextPacket()
            {
                Name = "",
                BubbleTime = 0,
                Stars = -1,
                Text = "A new floor to the mine has been opened!"
            }, null);
        }

        public override World GetInstance(ClientProcessor psr)
        {
            return RealmManager.AddWorld(new Mine(Level));
        }
    }
}
