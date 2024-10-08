using System;

namespace Server.Items
{
    public class InfusedAlchemistsGem : Item
    {
        [Constructable]
        public InfusedAlchemistsGem()
            : base(0x1EA7)
        {
            this.Weight = 1.0;
        }

        public InfusedAlchemistsGem(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1113006;
            }
        }
        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
              
            list.Add(1070722, "Alchemia Skill Increaser + 1");
        }

        public override void OnDoubleClick(Mobile from)
        {
            from.Skills[SkillName.Alchemia].Base += 1;
            from.SendMessage("You have increased your Alchemia Skill by 1 Point !."); 
            this.Delete();
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