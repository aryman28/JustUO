using System;

namespace Server.Mobiles
{
    [CorpseName("a sea horse corpse")]
    public class SeaHorse : BaseMount
    {
        [Constructable]
        public SeaHorse()
            : this("a sea horse")
        {
        }

        [Constructable]
        public SeaHorse(string name)
            : base(name, 0x90, 0x3EB3, AIType.AI_Animal, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            this.InitStats(Utility.Random(50, 30), Utility.Random(50, 30), 10);
            this.Skills[SkillName.ObronaPrzedMagia].Base = 25.0 + (Utility.RandomDouble() * 5.0);
            this.Skills[SkillName.Boks].Base = 35.0 + (Utility.RandomDouble() * 10.0);
            this.Skills[SkillName.Taktyka].Base = 30.0 + (Utility.RandomDouble() * 15.0);
        }

        public SeaHorse(Serial serial)
            : base(serial)
        {
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