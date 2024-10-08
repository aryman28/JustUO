using System;

namespace Server.Items
{
    public abstract class BaseOuterLegs : BaseClothing
    {
        public BaseOuterLegs(int itemID)
            : this(itemID, 0)
        {
        }

        public BaseOuterLegs(int itemID, int hue)
            : base(itemID, Layer.OuterLegs, hue)
        {
        }

        public BaseOuterLegs(Serial serial)
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

    [Flipable(0x230C, 0x230B)]
    public class FurSarong : BaseOuterLegs
    {
        [Constructable]
        public FurSarong()
            : this(0)
        {
        }

        [Constructable]
        public FurSarong(int hue)
            : base(0x230C, hue)
        {
            this.Weight = 3.0;
            this.Name = "Sk�rzana sp�dnica";
        }

        public FurSarong(Serial serial)
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

            if (this.Weight == 4.0)
                this.Weight = 3.0;
        }
    }

    [Flipable(0x1516, 0x1531)]
    public class Skirt : BaseOuterLegs
    {
        [Constructable]
        public Skirt()
            : this(0)
        {
        }

        [Constructable]
        public Skirt(int hue)
            : base(0x1516, hue)
        {
            this.Weight = 4.0;
            this.Name = "Sp�dnica";
        }

        public Skirt(Serial serial)
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

    [Flipable(0x1537, 0x1538)]
    public class Kilt : BaseOuterLegs
    {
        [Constructable]
        public Kilt()
            : this(0)
        {
        }

        [Constructable]
        public Kilt(int hue)
            : base(0x1537, hue)
        {
            this.Weight = 2.0;
            this.Name = "Kilt";
        }

        public Kilt(Serial serial)
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

    [Flipable(0x279A, 0x27E5)]
    public class Hakama : BaseOuterLegs
    {
        [Constructable]
        public Hakama()
            : this(0)
        {
        }

        [Constructable]
        public Hakama(int hue)
            : base(0x279A, hue)
        {
            this.Weight = 2.0;
        }

        public Hakama(Serial serial)
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

    public class GargishClothKilt : BaseClothing
    {
        [Constructable]
        public GargishClothKilt()
            : this(0)
        {
        }

        [Constructable]
        public GargishClothKilt(int hue)
            : base(0x0408, Layer.OuterLegs, hue)
        {
            this.Weight = 2.0;
        }

        public GargishClothKilt(Serial serial)
            : base(serial)
        {
        }

        public override Race RequiredRace
        {
            get
            {
                return Race.Gargoyle;
            }
        }
        public override bool CanBeWornByGargoyles
        {
            get
            {
                return true;
            }
        }
        public override void OnAdded(object parent)
        {
            if (parent is Mobile)
            {
                if (((Mobile)parent).Female)
                    this.ItemID = 0x0407;
                else
                    this.ItemID = 0x0408;
            }
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

    public class FemaleGargishClothKilt : BaseClothing
    {
        [Constructable]
        public FemaleGargishClothKilt()
            : this(0)
        {
        }

        [Constructable]
        public FemaleGargishClothKilt(int hue)
            : base(0x0407, Layer.OuterLegs, hue)
        {
            this.Weight = 2.0;
        }

        public FemaleGargishClothKilt(Serial serial)
            : base(serial)
        {
        }

        public override Race RequiredRace
        {
            get
            {
                return Race.Gargoyle;
            }
        }
        public override bool CanBeWornByGargoyles
        {
            get
            {
                return true;
            }
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

    public class MaleGargishClothKilt : BaseClothing
    {
        [Constructable]
        public MaleGargishClothKilt()
            : this(0)
        {
        }

        [Constructable]
        public MaleGargishClothKilt(int hue)
            : base(0x0408, Layer.OuterLegs, hue)
        {
            this.Weight = 2.0;
        }

        public MaleGargishClothKilt(Serial serial)
            : base(serial)
        {
        }

        public override Race RequiredRace
        {
            get
            {
                return Race.Gargoyle;
            }
        }
        public override bool CanBeWornByGargoyles
        {
            get
            {
                return true;
            }
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