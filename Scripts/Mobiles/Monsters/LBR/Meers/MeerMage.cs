using System;
using System.Collections;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a meer's corpse")]
    public class MeerMage : BaseCreature
    {
        private static readonly Hashtable m_Table = new Hashtable();
        private DateTime m_NextAbilityTime;
        [Constructable]
        public MeerMage()
            : base(AIType.AI_Mage, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.Name = "a meer mage";
            this.Body = 770;

            this.SetStr(171, 200);
            this.SetDex(126, 145);
            this.SetInt(276, 305);

            this.SetHits(103, 120);

            this.SetDamage(24, 26);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 45, 55);
            this.SetResistance(ResistanceType.Fire, 15, 25);
            this.SetResistance(ResistanceType.Cold, 50);
            this.SetResistance(ResistanceType.Poison, 25, 35);
            this.SetResistance(ResistanceType.Energy, 25, 35);

            this.SetSkill(SkillName.Intelekt, 100.0);
            this.SetSkill(SkillName.Magia, 70.1, 80.0);
            this.SetSkill(SkillName.Medytacja, 85.1, 95.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 80.1, 100.0);
            this.SetSkill(SkillName.Taktyka, 70.1, 90.0);
            this.SetSkill(SkillName.Boks, 60.1, 80.0);

            this.Fame = 8000;
            this.Karma = 8000;

            this.VirtualArmor = 16;

			switch (Utility.Random(8))
            {
                case 0: PackItem(new StrangleScroll()); break;
                case 1: PackItem(new WitherScroll()); break;
			}

            this.m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(2, 5));
        }

        public MeerMage(Serial serial)
            : base(serial)
        {
        }

        public override bool AutoDispel
        {
            get
            {
                return true;
            }
        }
        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Lethal;
            }
        }
        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override int TreasureMapLevel
        {
            get
            {
                return 3;
            }
        }
        public override bool InitialInnocent
        {
            get
            {
                return true;
            }
        }
        public static bool UnderEffect(Mobile m)
        {
            return m_Table.Contains(m);
        }

        public static void StopEffect(Mobile m, bool message)
        {
            Timer t = (Timer)m_Table[m];

            if (t != null)
            {
                if (message)
                    m.PublicOverheadMessage(Network.MessageType.Emote, m.SpeechHue, true, "* The open flame begins to scatter the swarm of insects *");

                t.Stop();
                m_Table.Remove(m);
            }
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.FilthyRich);
            this.AddLoot(LootPack.MedScrolls, 2);
            // TODO: Daemon bone ...
        }

        public override int GetHurtSound()
        {
            return 0x14D;
        }

        public override int GetDeathSound()
        {
            return 0x314;
        }

        public override int GetAttackSound()
        {
            return 0x75;
        }

        public override void OnThink()
        {
            if (DateTime.UtcNow >= this.m_NextAbilityTime)
            {
                Mobile combatant = this.Combatant;

                if (combatant != null && combatant.Map == this.Map && combatant.InRange(this, 12) && this.IsEnemy(combatant) && !UnderEffect(combatant))
                {
                    this.m_NextAbilityTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));

                    if (combatant is BaseCreature)
                    {
                        BaseCreature bc = (BaseCreature)combatant;

                        if (bc.Controlled && bc.ControlMaster != null && !bc.ControlMaster.Deleted && bc.ControlMaster.Alive)
                        {
                            if (bc.ControlMaster.Map == this.Map && bc.ControlMaster.InRange(this, 12) && !UnderEffect(bc.ControlMaster))
                            {
                                this.Combatant = combatant = bc.ControlMaster;
                            }
                        }
                    }

                    if (Utility.RandomDouble() < .1)
                    {
                        int[][] coord = 
                        {
                            new int[] { -4, -6 }, new int[] { 4, -6 }, new int[] { 0, -8 }, new int[] { -5, 5 }, new int[] { 5, 5 }
                        };

                        BaseCreature rabid;

                        for (int i = 0; i < 5; i++)
                        {
                            int x = combatant.X + coord[i][0];
                            int y = combatant.Y + coord[i][1];

                            Point3D loc = new Point3D(x, y, combatant.Map.GetAverageZ(x, y));

                            if (!combatant.Map.CanSpawnMobile(loc))
                                continue;

                            switch ( i )
                            {
                                case 0:
                                    rabid = new EnragedRabbit(this);
                                    break;
                                case 1:
                                    rabid = new EnragedHind(this);
                                    break;
                                case 2:
                                    rabid = new EnragedHart(this);
                                    break;
                                case 3:
                                    rabid = new EnragedBlackBear(this);
                                    break;
                                default:
                                    rabid = new EnragedEagle(this);
                                    break;
                            }

                            rabid.FocusMob = combatant;
                            rabid.MoveToWorld(loc, combatant.Map);
                        }
                        this.Say(1071932); //Creatures of the forest, I call to thee!  Aid me in the fight against all that is evil!
                    }
                    else if (combatant.Player)
                    {
                        this.Say(true, "I call a plague of insects to sting your flesh!");
                        m_Table[combatant] = Timer.DelayCall(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(7.0), new TimerStateCallback(DoEffect), new object[] { combatant, 0 });
                    }
                }
            }

            base.OnThink();
        }

        public void DoEffect(object state)
        {
            object[] states = (object[])state;

            Mobile m = (Mobile)states[0];
            int count = (int)states[1];

            if (!m.Alive)
            {
                StopEffect(m, false);
            }
            else
            {
                Torch torch = m.FindItemOnLayer(Layer.TwoHanded) as Torch;

                if (torch != null && torch.Burning)
                {
                    StopEffect(m, true);
                }
                else
                {
                    if ((count % 4) == 0)
                    {
                        m.LocalOverheadMessage(Network.MessageType.Emote, m.SpeechHue, true, "* The swarm of insects bites and stings your flesh! *");
                        m.NonlocalOverheadMessage(Network.MessageType.Emote, m.SpeechHue, true, String.Format("* {0} is stung by a swarm of insects *", m.Name));
                    }

                    m.FixedParticles(0x91C, 10, 180, 9539, EffectLayer.Waist);
                    m.PlaySound(0x00E);
                    m.PlaySound(0x1BC);

                    AOS.Damage(m, this, Utility.RandomMinMax(30, 40) - (Core.AOS ? 0 : 10), 100, 0, 0, 0, 0);

                    states[1] = count + 1;

                    if (!m.Alive)
                        StopEffect(m, false);
                }
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}