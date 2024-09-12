using System;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("Rakktavi [Renowned] corpse")]
    public class RakktaviRenowned : BaseRenowned
    {
        [Constructable]
        public RakktaviRenowned()
            : base(AIType.AI_Archer)
        {
            Name = "Rakktavi";
            Title = "[Renowned]";
            Body = 0x8E;
            BaseSoundID = 437;

            SetStr(119);
            SetDex(279);
            SetInt(327);

            SetHits(50000);
			SetMana(327);
			SetStam(279);

            SetDamage(8, 10);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 30);
            SetResistance(ResistanceType.Fire, 10, 25);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 10, 20);
            SetResistance(ResistanceType.Energy, 10, 20);

            SetSkill(SkillName.Anatomia, 0);
            SetSkill(SkillName.Lucznictwo, 80.1, 90.0);
            SetSkill(SkillName.ObronaPrzedMagia, 66.0);
            SetSkill(SkillName.Taktyka, 68.1);
            SetSkill(SkillName.Boks, 85.5);

            Fame = 6500;
            Karma = -6500;

            VirtualArmor = 56;

            PackItem(new EssenceBalance());
			
            AddItem(new Bow());
            PackItem(new Arrow(Utility.RandomMinMax(10, 30)));
        }

        public RakktaviRenowned(Serial serial)
            : base(serial)
        {
        }

        public override Type[] UniqueSAList
        {
            get
            {
                return new Type[] { typeof(TatteredAncientScroll) };
            }
        }
        public override Type[] SharedSAList
        {
            get
            {
                return new Type[] { typeof(CavalrysFolly), typeof(ArcanicRuneStone), typeof(CrushedGlass), typeof(AbyssalCloth), typeof(TorcOfTheGuardians) };
            }
        }
        public override InhumanSpeech SpeechType
        {
            get
            {
                return InhumanSpeech.Ratman;
            }
        }
        public override bool CanRummageCorpses
        {
            get
            {
                return true;
            }
        }
        public override int Hides
        {
            get
            {
                return 8;
            }
        }
        public override HideType HideType
        {
            get
            {
                return HideType.Spined;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }
        public override bool AllureImmune
        {
            get
            {
                return true;
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

            if (Body == 42)
            {
                Body = 0x8E;
                Hue = 0;
            }
        }
    }
}