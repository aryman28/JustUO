using System;

namespace Server.Items
{
    public class Grimmur : Spellbook
    {
        [Constructable]
        public Grimmur()
            : this((ulong)0)
        {
        }

        [Constructable]
        public Grimmur(ulong content)
            : base(content, 0xEFA)
        {
            this.Weight = 3.0;
	    this.Layer = Layer.OneHanded;
	    this.LootType = LootType.Blessed;
            this.Name = "Grimmur";
            this.Hue = 0;
        }

        public Grimmur(Serial serial)
            : base(serial)
        {
        }

        public override SpellbookType SpellbookType
        {
            get
            {
                return SpellbookType.Regular;
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();
        }
    }
}