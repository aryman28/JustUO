using System;
using System.Collections;
using System.Collections.Generic;
using Server.Items;
using Server.Targeting;
using Server.Multis;
using Server.Items;
using Server.ContextMenus;
using Server.Misc;
using Server.Network;
using Server.Mobiles;

namespace Server.Mobiles
{
    public class ShipsPirateCaptain : BaseCreature
    {
        private PirateShip m_PirateShip;

        [Constructable]
        public ShipsPirateCaptain()
            : base(AIType.AI_Archer, FightMode.Closest, 15, 1, 0.2, 0.4)
        {
            SpeechHue = Utility.RandomDyedHue();
            Hue = Utility.RandomSkinHue();

            if (this.Female = Utility.RandomBool())
            {
                Title = "Kapitan pirat�w";
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem(new ThighBoots());
            }
            else
            {
                Title = "Pirate Captain";
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem(new ThighBoots());
            }

            SetStr(495, 500);
            SetDex(781, 895);
            SetInt(61, 75);
            SetHits(1488, 1508);

            SetDamage(20, 40);

            SetSkill( SkillName.WalkaSzpadami, 86.0, 97.5 );
			SetSkill( SkillName.WalkaObuchami, 85.0, 87.5 );
			SetSkill( SkillName.ObronaPrzedMagia, 55.0, 67.5 );
			SetSkill( SkillName.WalkaMieczami, 85.0, 87.5 );
			SetSkill( SkillName.Taktyka, 85.0, 87.5 );
			SetSkill( SkillName.Boks, 35.0, 37.5 );
			SetSkill( SkillName.Lucznictwo, 85.0, 87.5 );

            Fame = 5000;
            Karma = -5000;
            VirtualArmor = 66;

            switch (Utility.Random(1))
            {
                case 0: AddItem(new LongPants(Utility.RandomRedHue())); break;
                case 1: AddItem(new ShortPants(Utility.RandomRedHue())); break;
            }
            AddItem(new FancyShirt(1153));

            AddItem(new TricorneHat(33));

            switch (Utility.Random(5))
            {
                case 0: AddItem(new Bow()); break;
                case 1: AddItem(new CompositeBow()); break;
                case 2: AddItem(new Crossbow()); break;
                case 3: AddItem(new RepeatingCrossbow()); break;
                case 4: AddItem(new HeavyCrossbow()); break;
            }

            Item hair = new Item(Utility.RandomList(0x203B, 0x2049, 0x2048, 0x204A));
            hair.Hue = Utility.RandomNondyedHue();
            hair.Layer = Layer.Hair;
            hair.Movable = false;
            AddItem(hair);

            Container cont = new WoodenBox();
            cont.ItemID = 0xE7D;
            cont.Hue = 0x489;

            int count = Utility.RandomMinMax(30, 50);

            for (int i = 0; i < count; ++i)
            {
                cont.DropItem(Loot.RandomGem());
            }

            PackItem(cont);

        }

        public override bool PlayerRangeSensitive { get { return false; } }

        private bool BoatSpawn;
        private DateTime m_NextPickup;
        private Mobile m_Mobile;
        private BaseBoat m_findboat;
        private ArrayList list;
        private Direction hostiledirection;

        public override void OnThink()
        {
            if (BoatSpawn == false)
            {
                Map map = this.Map;
                if (map == null)
                    return;
                this.Z = 0;
                m_PirateShip = new PirateShip();
                Point3D loc = this.Location;
                Point3D loccrew = this.Location;
                loc = new Point3D(this.X, this.Y - 1, -5);
                loccrew = new Point3D(this.X, this.Y - 1, this.Z);
                m_PirateShip.MoveToWorld(loc, map);
                BoatSpawn = true;

                //for (int i = 0; i < 5; ++i)
                //{
                //    ShipsPirateCaptian m_pirate = new ShipsPirateCaptian();
                //    m_pirate.MoveToWorld(loccrew, map);
                //}
            }

            base.OnThink();
            if (DateTime.Now < m_NextPickup)
                return;

            if (m_PirateShip == null)
            {
                return;
            }

            m_NextPickup = DateTime.Now + TimeSpan.FromSeconds(Utility.RandomMinMax(1, 2));


            hostiledirection = Direction.North; //default nord
            foreach (Item enemy in this.GetItemsInRange(200))
            {
                if (enemy is BaseBoat && enemy != m_PirateShip && !(enemy is PirateShip))
                {

                    List<Mobile> targets = new List<Mobile>();
                    IPooledEnumerable eable = enemy.GetMobilesInRange(16);

                    foreach (Mobile m in eable)
                    {
                        if (m is PlayerMobile)
                            targets.Add(m);
                    }
                    eable.Free();
                    if (targets.Count > 0)
                    {
                        m_findboat = enemy as BaseBoat;
                        hostiledirection = this.GetDirectionTo(m_findboat);
                        break;
                    }
                }
            }
            if (m_findboat == null)
            {
                return;
            }


            if (m_PirateShip != null && m_findboat != null)
            {
                if (m_PirateShip != null && (hostiledirection == Direction.North) && m_PirateShip.Facing != Direction.North)
                {
                    m_PirateShip.Facing = Direction.North;
                }
                else if (m_PirateShip != null && (hostiledirection == Direction.South) && m_PirateShip.Facing != Direction.South)
                {
                    m_PirateShip.Facing = Direction.South;
                }
                else if (m_PirateShip != null && (hostiledirection == Direction.East || hostiledirection == Direction.Right || hostiledirection == Direction.Down) && m_PirateShip.Facing != Direction.East)
                {
                    m_PirateShip.Facing = Direction.East;
                }
                else if (m_PirateShip != null && (hostiledirection == Direction.West || hostiledirection == Direction.Left || hostiledirection == Direction.Up) && m_PirateShip.Facing != Direction.West)
                {
                    m_PirateShip.Facing = Direction.West;
                }
                m_PirateShip.StartMove(Direction.North, true); //, false); //Vollgas!

                if (m_PirateShip != null && this.InRange(m_findboat, 10) && m_PirateShip.IsMoving == true) // In Reichweite? Stop!
                {
                    m_PirateShip.StopMove(false);
                }
            }
            else
            {
                if (m_PirateShip != null && m_PirateShip.IsMoving == true)
                {
                    m_PirateShip.StopMove(false); //keiner da? anhalten.
                }
            }
        }
        public override void OnDelete()
        {
            if (m_PirateShip != null)
            {
                new SinkTimer(m_PirateShip, this).Start();
            }
        }

        public override void OnDamagedBySpell(Mobile caster)
        {
            if (caster == this)
                return;

            SpawnPirate(caster);
        }

        public void SpawnPirate(Mobile target)
        {
            Map map = target.Map;

            if (map == null)
                return;

            int shipspirates = 0;

            foreach (Mobile m in this.GetMobilesInRange(10))
            {
                if (m is ShipsPirate)
                    ++shipspirates;
            }

            if (shipspirates < 10 && Utility.RandomDouble() <= 0.25)
            {
                BaseCreature shipspirate = new ShipsPirate();

                Point3D loc = target.Location;
                bool validLocation = false;

                for (int j = 0; !validLocation && j < 10; ++j)
                {
                    int x = target.X + Utility.Random(3) - 1;
                    int y = target.Y + Utility.Random(3) - 1;
                    int z = map.GetAverageZ(x, y);

                    if (validLocation = map.CanFit(x, y, this.Z, 16, false, false))
                        loc = new Point3D(x, y, Z);
                    else if (validLocation = map.CanFit(x, y, z, 16, false, false))
                        loc = new Point3D(x, y, z);
                }

                shipspirate.MoveToWorld(loc, map);

                shipspirate.Combatant = target;
            }
        }

        public override bool OnBeforeDeath()
        {
            if (m_PirateShip != null)
            {
                new SinkTimer(m_PirateShip, this).Start();
            }
            return true;
        }
        private class SinkTimer : Timer
        {
            private BaseBoat m_Boat;
            private int m_Count;
            private Mobile m_mobile;

            public SinkTimer(BaseBoat boat, Mobile m)
                : base(TimeSpan.FromSeconds(1.0), TimeSpan.FromSeconds(4.0))
            {
                m_Boat = boat;
                m_mobile = m;

                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Count == 4)
                {
                    List<Mobile> targets = new List<Mobile>();
                    IPooledEnumerable eable = m_Boat.GetMobilesInRange(16);

                    foreach (Mobile m in eable)
                    {
                        if (m is ShipsPirate)
                            targets.Add(m);
                    }
                    eable.Free();
                    if (targets.Count > 0)
                    {
                        for (int i = 0; i < targets.Count; ++i)
                        {
                            Mobile m = targets[i];
                            m.Kill();
                        }
                    }
                }
                if (m_Count >= 15)
                {
                    m_Boat.Delete();
                    Stop();
                }
                else
                {
                    if (m_Count < 5)
                    {
                        m_Boat.Location = new Point3D(m_Boat.X, m_Boat.Y, m_Boat.Z - 1);

                        if (m_Boat.TillerMan != null && m_Count < 5)
                            m_Boat.TillerMan.Say(1007168 + m_Count);
                    }
                    else
                    {
                        m_Boat.Location = new Point3D(m_Boat.X, m_Boat.Y, m_Boat.Z - 3);

                    }
                    ++m_Count;
                }
            }
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override Poison PoisonImmune { get { return Poison.Regular; } }

        public ShipsPirateCaptain(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((Item)m_PirateShip);
            writer.Write((bool)BoatSpawn);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            m_PirateShip = reader.ReadItem() as PirateShip;
            BoatSpawn = reader.ReadBool();
            int version = reader.ReadInt();
        }
    }
}
