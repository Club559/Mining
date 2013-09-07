using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using wServer.realm;
using wServer.logic.attack;
using wServer.logic.movement;
using wServer.logic.loot;
using wServer.logic.taunt;
using wServer.logic.cond;

namespace wServer.logic
{
    partial class BehaviorDb
    {
        static _ Mine = Behav()
            .InitMany(0x196f, 0x1971, Behaves("Gold Pot",
                loot: new LootBehavior(LootDef.Empty,
                        Tuple.Create(100, new LootDef(0, 1, 0, 1,
                            Tuple.Create(1.0, (ILoot)new GoldLoot(100))
                        ))
                )
            ))
            .Init(0x1975, Behaves("Pot of Descent",
                condBehaviors: new ConditionalBehavior[] {
                    new OnDeath(new PlaceDescendPortal())
                }
            ))
            .Init(0x1960, Behaves("Destructible Mine Wall",
                loot: new LootBehavior(LootDef.Empty,
                    Tuple.Create(100, new LootDef(0, 1, 0, 1,
                        Tuple.Create(0.05, (ILoot)new GoldLoot(1, 10))
                    ))
                )
            ))
            .Init(0x1961, Behaves("Iron Mine",
                loot: new LootBehavior(LootDef.Empty,
                    Tuple.Create(100, new LootDef(0, 1, 0, 1,
                        Tuple.Create(1.0, (ILoot)new ItemLoot("Iron Nugget"))
                    ))
                )
            ))
            .Init(0x1962, Behaves("Silver Mine",
                loot: new LootBehavior(LootDef.Empty,
                    Tuple.Create(100, new LootDef(0, 1, 0, 1,
                        Tuple.Create(1.0, (ILoot)new ItemLoot("Silver Nugget"))
                    ))
                )
            ))
            .Init(0x1963, Behaves("Copper Mine",
                loot: new LootBehavior(LootDef.Empty,
                    Tuple.Create(100, new LootDef(0, 1, 0, 1,
                        Tuple.Create(1.0, (ILoot)new ItemLoot("Copper Nugget"))
                    ))
                )
            ))
            .Init(0x1964, Behaves("Gold Mine",
                loot: new LootBehavior(LootDef.Empty,
                    Tuple.Create(100, new LootDef(0, 1, 0, 1,
                        Tuple.Create(1.0, (ILoot)new ItemLoot("Gold Nugget"))
                    ))
                )
            )).Init(0x1976, Behaves("Vlux Mine",
                loot: new LootBehavior(LootDef.Empty,
                    Tuple.Create(100, new LootDef(0, 1, 0, 1,
                        Tuple.Create(1.0, (ILoot)new ItemLoot("Vlux Nugget"))
                    ))
                )
            ));
    }
}
