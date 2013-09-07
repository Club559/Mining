using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm.entities;
using wServer;
using wServer.realm.worlds;

namespace wServer.realm
{
    public static class GenLogic
    {
        public static void GenWalls(World Owner, Wall w)
        {
            Random r = new Random();
            for (var tx = -1; tx <= 1; tx++)
                for (var ty = -1; ty <= 1; ty++)
                {
                    if (Owner.Map[(int)w.X + tx, (int)w.Y + ty].TileId == 0xff && Owner.Map[(int)w.X + tx, (int)w.Y + ty].ObjId == 0)
                    {
                        WmapTile tile = Owner.Map[(int)w.X + tx, (int)w.Y + ty];
                        tile.TileId = Owner.Map[(int)w.X, (int)w.Y].TileId;
                        Owner.Map[(int)w.X + tx, (int)w.Y + ty] = tile;
                        GenWall(Owner, w.X + tx, w.Y + ty, r);
                    }
                }
        }
        public static void GenWall(World world, float x, float y, Random rand)
        {
            short objType = 0x1960;
            List<Tuple<short,int>> ores = (world as Mine).GetOres();
            ores.Shuffle();
            if (rand.Next(0, ores.First().Item2) == 1)
            {
                objType = ores.First().Item1;
            }
            Wall e = new Wall(objType, XmlDatas.TypeToElement[objType]);
            e.Move(x, y);
            world.EnterWorld(e);
        }
        public static bool GenRandomRoom(World world, float x, float y, Wall theWall)
        {
            try
            {
                Random rand = new Random();
                if (rand.Next(1, 60) != 1)
                    return false;
                //Console.Out.WriteLine("Generating room...");
                List<string> dirs = new List<string>();
                for (int tx = -1; tx <= 1; tx++)
                    for (int ty = -1; ty <= 1; ty++)
                    {
                        WmapTile targetTile = world.Map[(int)x + tx, (int)y + ty];
                        WmapTile thisTile = world.Map[(int)x, (int)y];
                        if (targetTile.TileId == 0xff)
                        {
                            if (tx == -1 && ty == 0)
                                dirs.Add("left");
                            else if (tx == 1 && ty == 0)
                                dirs.Add("right");
                            else if (tx == 0 && ty == 1)
                                dirs.Add("down");
                            else if (tx == 0 && ty == -1)
                                dirs.Add("up");
                        }
                    }
                if(dirs.Count < 1)
                    return false;
                dirs.Shuffle();
                //Console.Out.WriteLine("Room direction: " + dirs.First());
                float mainX = x;
                float mainY = y;
                float entranceX = x;
                float entranceY = y;

                int rsX = 1;
                int rsY = 1;
                do
                    rsX = rand.Next(6, 12 + 1);
                while (rsX % 2 > 0);

                do
                    rsY = rand.Next(6, 12 + 1);
                while (rsY % 2 > 0);

                //Console.Out.WriteLine("Room size: " + rsX + ", " + rsY);

                switch (dirs.First())
                {
                    case "up":
                        mainX = x - (rsX / 2); mainY = y - rsY;
                        entranceY = y - 1; break;
                    case "down":
                        mainX = x - (rsX / 2); mainY = y + 1;
                        entranceY = y + 1; break;
                    case "left":
                        mainX = x - rsX; mainY = y - ((rsY - 2)/2);
                        entranceX = x - 1; break;
                    case "right":
                        mainX = x + 1; mainY = y - ((rsY - 2)/2);
                        entranceX = x + 1; break;
                }
                entranceX -= 0.5f;
                entranceY -= 0.5f;
                List<WmapTile> addedTiles = new List<WmapTile>();
                for(int ty = (int)mainY; ty <= mainY + (rsY - 1); ty++)
                    for (int tx = (int)mainX; tx <= mainX + (rsX - 1); tx++)
                    {
                        WmapTile tTile = world.Map[tx, ty];
                        if (tTile.TileId != 0xff || tTile.ObjType != 0)
                        {
                            Console.Out.WriteLine("Found collision while generating room!");
                            return false;
                        }
                        tTile.TileId = world.Map[(int)x, (int)y].TileId;
                        addedTiles.Add(tTile);
                    }
                //Console.Out.WriteLine("Generated tiles, placing...");
                int tileNum = 0;
                float blackPotSpotX = (float)rand.Next((int)mainX + 1, (int)mainX + rsX);
                float blackPotSpotY = (float)rand.Next((int)mainY + 1, (int)mainY + rsY);
                for(int ty = (int)mainY; ty <= mainY + (rsY - 1); ty++)
                    for (int tx = (int)mainX; tx <= mainX + (rsX - 1); tx++)
                    {
                        WmapTile ctile = addedTiles[tileNum];
                        world.Map[tx, ty] = ctile;
                        if ((tx == (int)mainX || tx == (int)mainX + (rsX - 1) || ty == (int)mainY || ty == (int)mainY + (rsY - 1)) && !(tx == entranceX && ty == entranceY))
                        {
                            //Console.Out.WriteLine(tx + ", " + ty + " - " + entranceX + ", " + entranceY);
                            //Console.Out.WriteLine("Placed wall");
                            GenWall(world, tx + 0.5f, ty + 0.5f, rand);
                        }
                        else
                        {
                            //Console.Out.WriteLine("Placed treasure");
                            if (rand.Next(1, 25) == 1)
                            {
                                if ((world as Mine).RoomsFound == 3 ? (tx != blackPotSpotX && ty != blackPotSpotY) : true)
                                {
                                    Entity e = Entity.Resolve((short)rand.Next(0x196f, 0x1972));
                                    e.Move(tx + 0.5f, ty + 0.5f);
                                    world.EnterWorld(e);
                                }
                            }
                        }
                    }
                (world as Mine).RoomsFound++;
                if ((world as Mine).RoomsFound == 4)
                {
                    Entity e = Entity.Resolve(XmlDatas.IdToType["Pot of Descent"]);
                    e.Move(blackPotSpotX + 0.5f, blackPotSpotY + 0.5f);
                    world.EnterWorld(e);
                }
                //Console.Out.WriteLine("Placed tiles!");
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
