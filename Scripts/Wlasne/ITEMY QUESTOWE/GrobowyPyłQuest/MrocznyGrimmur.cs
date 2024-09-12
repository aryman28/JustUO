using System;

namespace Server.Items
{
    public class MrocznyGrimmur : Spellbook
    {
        [Constructable]
        public MrocznyGrimmur()
            : this((ulong)0)
        {
        }

        [Constructable]
        public MrocznyGrimmur(ulong content)
            : base(content, 0x2253)
        {
            this.Weight = 3.0;
	    this.Layer = (Core.ML ? Layer.OneHanded : Layer.Invalid);
	    this.LootType = LootType.Blessed;
            this.Name = "Mroczny Grimmur";
            this.Hue = 0;
        }
        public override int BookOffset
        {
            get
            {
                return 100;
            }
        }
        public override int BookCount
        {
            get
            {
                return ((Core.SE) ? 17 : 16);
            }
        }
        public MrocznyGrimmur(Serial serial)
            : base(serial)
        {
        }

        public override SpellbookType SpellbookType
        {
            get
            {
                return SpellbookType.Necromancer;
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