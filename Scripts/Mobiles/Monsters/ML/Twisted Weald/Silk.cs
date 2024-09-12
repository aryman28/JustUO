using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a Silk corpse")]
    public class Silk : GiantBlackWidow
    {
        [Constructable]
        public Silk()
        {

            this.Name = "Silk";
            this.Hue = 0x47E;

            this.SetStr(80, 131);
            this.SetDex(126, 156);
            this.SetInt(63, 102);

            this.SetHits(279, 378);
            this.SetStam(126, 156);
            this.SetMana(63, 102);

            this.SetDamage(15, 22);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 40, 50);
            this.SetResistance(ResistanceType.Fire, 30, 39);
            this.SetResistance(ResistanceType.Cold, 30, 40);
            this.SetResistance(ResistanceType.Poison, 70, 76);
            this.SetResistance(ResistanceType.Energy, 30, 40);

            this.SetSkill(SkillName.Boks, 114.1, 123.7);
            this.SetSkill(SkillName.Taktyka, 102.6, 118.3);
            this.SetSkill(SkillName.ObronaPrzedMagia, 78.6, 94.8);
            this.SetSkill(SkillName.Anatomia, 81.3, 105.7);
            this.SetSkill(SkillName.Zatruwanie, 106.0, 119.2);

            this.Fame = 18900;
            this.Karma = -18900;
        }

        public Silk(Serial serial)
            : base(serial)
        {
        }

        public override bool GivesMLMinorArtifact
        {
            get
            {
                return true;
            }
        }
        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.UltraRich, 2);
        }

        public override WeaponAbility GetWeaponAbility()
        {
            return WeaponAbility.ParalyzingBlow;
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