using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Lurg corpse")]
    public class Lurg : Troglodyte
    {
        [Constructable]
        public Lurg()
        {
            Name = "Lurg";
            Hue = 0x455;

            SetStr(584, 625);
            SetDex(163, 176);
            SetInt(90, 106);

            SetHits(3034, 3189);
            SetStam(163, 176);
            SetMana(90, 106);

            SetDamage(16, 19);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 53);
            SetResistance(ResistanceType.Fire, 45, 47);
            SetResistance(ResistanceType.Cold, 56, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 41, 56);

            SetSkill(SkillName.Boks, 122.7, 130.5);
            SetSkill(SkillName.Taktyka, 109.3, 118.5);
            SetSkill(SkillName.ObronaPrzedMagia, 72.9, 87.6);
            SetSkill(SkillName.Anatomia, 110.5, 124.0);
            SetSkill(SkillName.Leczenie, 84.1, 105.0);

            Fame = 10000;
            Karma = -10000;
        }

        public Lurg(Serial serial)
            : base(serial)
        {
        }
        public override bool AllureImmune
        {
            get
            {
                return true;
            }
        }
        public override bool GivesMLMinorArtifact
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
                return 4;
            }
        }
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
        }

        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.CrushingBlow;
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