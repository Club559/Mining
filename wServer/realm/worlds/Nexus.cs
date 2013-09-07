using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using wServer.realm.entities;
using wServer.svrPackets;

namespace wServer.realm.worlds
{
    public class Nexus : Mine
    {
        public Nexus()
        {
            Id = NEXUS_ID;
            Background = 0;
            AllowTeleport = true;
            Mining = true;
            Level = 0;
            base.FromWorldMap(typeof(RealmManager).Assembly.GetManifestResourceStream("wServer.realm.worlds.mine.wmap"));

            Init(0);
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
        }

        public override void PlacePortal()
        {
            var prtal = Portal.Resolve(0x1972);
            prtal.Move(PortalPos.X + 1f, PortalPos.Y + 0.5f);
            EnterWorld(prtal);

            BroadcastPacket(new TextPacket()
            {
                Name = "",
                BubbleTime = 0,
                Stars = -1,
                Text = "A new floor to the mine has been opened!"
            }, null);
        }
    }
}
