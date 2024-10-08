using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{ 
    public class ParoxysmusSuccubiQuest : BaseQuest
    { 
        public ParoxysmusSuccubiQuest()
            : base()
        { 
            this.AddObjective(new SlayObjective(typeof(Succubus), "succubi", 3, "Palace of Paroxysmus"));
			
            this.AddReward(new BaseReward(typeof(LargeTreasureBag), 1072706));
        }

        /* Paroxysmus' Succubi */
        public override object Title
        {
            get
            {
                return 1073067;
            }
        }
        /* The succubi that have congregated within the sinkhole to worship Paroxysmus pose a tremendous 
        danger. Will you enter the lair and see to their destruction? */
        public override object Description
        {
            get
            {
                return 1074696;
            }
        }
        /* Well, okay. But if you decide you are up for it after all, c'mon back and see me. */
        public override object Refuse
        {
            get
            {
                return 1072270;
            }
        }
        /* You're not quite done yet.  Get back to work! */
        public override object Uncomplete
        {
            get
            {
                return 1072271;
            }
        }
        public override bool CanOffer()
        {
            return MondainsLegacy.PalaceOfParoxysmus;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class ParoxysmusMolochQuest : BaseQuest
    { 
        public ParoxysmusMolochQuest()
            : base()
        { 
            this.AddObjective(new SlayObjective(typeof(Moloch), "molochs", 3, "Palace of Paroxysmus"));
			
            this.AddReward(new BaseReward(typeof(LargeTreasureBag), 1072706));
        }

        /* Paroxysmus' Moloch */
        public override object Title
        {
            get
            {
                return 1073068;
            }
        }
        /* The moloch daemons that have congregated to worship Paroxysmus pose a tremendous danger. Will 
        you enter the lair and see to their destruction? */
        public override object Description
        {
            get
            {
                return 1074695;
            }
        }
        /* Well, okay. But if you decide you are up for it after all, c'mon back and see me. */
        public override object Refuse
        {
            get
            {
                return 1072270;
            }
        }
        /* You're not quite done yet.  Get back to work! */
        public override object Uncomplete
        {
            get
            {
                return 1072271;
            }
        }
        public override bool CanOffer()
        {
            return MondainsLegacy.PalaceOfParoxysmus;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class ParoxysmusDaemonsQuest : BaseQuest
    { 
        public ParoxysmusDaemonsQuest()
            : base()
        { 
            this.AddObjective(new SlayObjective(typeof(Daemon), "daemons", 10, "Palace of Paroxysmus"));
			
            this.AddReward(new BaseReward(typeof(LargeTreasureBag), 1072706));
        }

        /* Paroxysmus' Daemons */
        public override object Title
        {
            get
            {
                return 1073069;
            }
        }
        /* The daemons that have congregated to worship Paroxysmus pose a tremendous danger. Will you enter 
        the lair and see to their destruction? */
        public override object Description
        {
            get
            {
                return 1074695;
            }
        }
        /* Well, okay. But if you decide you are up for it after all, c'mon back and see me. */
        public override object Refuse
        {
            get
            {
                return 1072270;
            }
        }
        /* You're not quite done yet.  Get back to work! */
        public override object Uncomplete
        {
            get
            {
                return 1072271;
            }
        }
        public override bool CanOffer()
        {
            return MondainsLegacy.PalaceOfParoxysmus;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class ParoxysmusArcaneDaemonsQuest : BaseQuest
    { 
        public ParoxysmusArcaneDaemonsQuest()
            : base()
        { 
            this.AddObjective(new SlayObjective(typeof(ArcaneDaemon), "arcane daemons", 10, "Palace of Paroxysmus"));
			
            this.AddReward(new BaseReward(typeof(LargeTreasureBag), 1072706));
        }

        /* Paroxysmus' Arcane Daemons */
        public override object Title
        {
            get
            {
                return 1073070;
            }
        }
        /* The arcane daemons that worship Paroxysmus pose a tremendous danger. Will you enter the lair and 
        see to their destruction? */
        public override object Description
        {
            get
            {
                return 1074697;
            }
        }
        /* Well, okay. But if you decide you are up for it after all, c'mon back and see me. */
        public override object Refuse
        {
            get
            {
                return 1072270;
            }
        }
        /* You're not quite done yet.  Get back to work! */
        public override object Uncomplete
        {
            get
            {
                return 1072271;
            }
        }
        public override bool CanOffer()
        {
            return MondainsLegacy.PalaceOfParoxysmus;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CausticComboQuest : BaseQuest
    { 
        public CausticComboQuest()
            : base()
        { 
            this.AddObjective(new SlayObjective(typeof(PoisonElemental), "poison elementals", 3, "Palace of Paroxysmus"));
            this.AddObjective(new SlayObjective(typeof(ToxicElemental), "acid elementals", 6, "Palace of Paroxysmus"));
			
            this.AddReward(new BaseReward(typeof(LargeTreasureBag), 1072706));
        }

        /* Caustic Combo */
        public override object Title
        {
            get
            {
                return 1073062;
            }
        }
        /* Vile creatures have exited the sinkhole and begun terrorizing the surrounding area.  The demons are bad enough, 
        but the elementals are an abomination, their poisons seeping into the fertile ground here.  Will you enter the 
        sinkhole and put a stop to their depredations? */
        public override object Description
        {
            get
            {
                return 1074693;
            }
        }
        /* Well, okay. But if you decide you are up for it after all, c'mon back and see me. */
        public override object Refuse
        {
            get
            {
                return 1072270;
            }
        }
        /* You're not quite done yet.  Get back to work! */
        public override object Uncomplete
        {
            get
            {
                return 1072271;
            }
        }
        public override bool CanOffer()
        {
            return MondainsLegacy.PalaceOfParoxysmus;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class PlagueLordQuest : BaseQuest
    { 
        public PlagueLordQuest()
            : base()
        { 
            this.AddObjective(new SlayObjective(typeof(PlagueSpawn), "plague spawns", 10, "Palace of Paroxysmus"));
            this.AddObjective(new SlayObjective(typeof(PlagueBeast), "plague beasts", 3, "Palace of Paroxysmus"));
            //AddObjective( new SlayObjective( typeof( PlagueBeastLord ), "plague beast lord", 1, "Palace of Paroxysmus" ) );
			
            this.AddReward(new BaseReward(typeof(LargeTreasureBag), 1072706));
        }

        /* Plague Lord */
        public override object Title
        {
            get
            {
                return 1073061;
            }
        }
        /* Some of the most horrific creatures have slithered out of the sinkhole there and begun terrorizing the surrounding 
        area. The plague creatures are one of the most destruction of the minions of Paroxysmus.  Are you willing to do 
        something about them? */
        public override object Description
        {
            get
            {
                return 1074692;
            }
        }
        /* Well, okay. But if you decide you are up for it after all, c'mon back and see me. */
        public override object Refuse
        {
            get
            {
                return 1072270;
            }
        }
        /* You're not quite done yet.  Get back to work! */
        public override object Uncomplete
        {
            get
            {
                return 1072271;
            }
        }
        public override bool CanOffer()
        {
            return MondainsLegacy.PalaceOfParoxysmus;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Nillaen : MondainQuester
    { 
        [Constructable]
        public Nillaen()
            : base("Lorekeeper Nillaen", "the keeper of tradition")
        { 
            this.SetSkill(SkillName.Medytacja, 60.0, 83.0);
            this.SetSkill(SkillName.Logistyka, 60.0, 83.0);
        }

        public Nillaen(Serial serial)
            : base(serial)
        {
        }

        public override Type[] Quests
        { 
            get
            {
                return new Type[] 
                {
                    typeof(ParoxysmusSuccubiQuest),
                    typeof(ParoxysmusMolochQuest),
                    typeof(ParoxysmusDaemonsQuest),
                    typeof(ParoxysmusArcaneDaemonsQuest),
                    typeof(CausticComboQuest),
                    typeof(PlagueLordQuest)
                };
            }
        }
        public override void InitBody()
        {
            this.InitStats(100, 100, 25);
			
            this.Female = true;
            this.Race = Race.Elf;
			
            this.Hue = 0x8367;
            this.HairItemID = 0x2FCF;
            this.HairHue = 0x26B;
        }

        public override void InitOutfit()
        {
            this.AddItem(new Shoes(0x1BB));
            this.AddItem(new LongPants(0x1FB));
            this.AddItem(new ElvenShirt());
            this.AddItem(new GemmedCirclet());
            this.AddItem(new BodySash(0x25));
            this.AddItem(new BlackStaff());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
}