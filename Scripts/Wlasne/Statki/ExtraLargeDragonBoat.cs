using System;

namespace Server.Multis
{
    public class ExtraLargeDragonBoat : BaseBoat
    {
        [Constructable]
        public ExtraLargeDragonBoat()
        {
        }

        public ExtraLargeDragonBoat(Serial serial)
            : base(serial)
        {
        }

        public override int NorthID
        {
            get
            {
                return 0x18;
            }
        }
        public override int EastID
        {
            get
            {
                return 0x19;
            }
        }
        public override int SouthID
        {
            get
            {
                return 0x1A;
            }
        }
        public override int WestID
        {
            get
            {
                return 0x1B;
            }
        }
        public override int HoldDistance
        {
            get
            {
                return 5;
            }
        }
        public override int TillerManDistance
        {
            get
            {
                return -9;
            }
        }
        public override Point2D StarboardOffset
        {
            get
            {
                return new Point2D(2, -1);
            }
        }
        public override Point2D PortOffset
        {
            get
            {
                return new Point2D(-2, -1);
            }
        }
        public override Point3D MarkOffset
        {
            get
            {
                return new Point3D(0, 0, 3);
            }
        }
        public override BaseDockedBoat DockedBoat
        {
            get
            {
                return new LargeDockedDragonBoat(this);
            }
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
    }

    public class ExtraLargeDragonBoatDeed : BaseBoatDeed
    {
        [Constructable]
        public ExtraLargeDragonBoatDeed()
            : base(0x14, new Point3D(0, -1, 0))
        {
        }

        public ExtraLargeDragonBoatDeed(Serial serial)
            : base(serial)
        {
        }

        public override int LabelNumber
        {
            get
            {
                return 1041210;
            }
        }// large dragon ship deed
        public override BaseBoat Boat
        {
            get
            {
                return new ExtraLargeDragonBoat();
            }
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
    }

    public class ExtraLargeDockedDragonBoat : BaseDockedBoat
    {
        public ExtraLargeDockedDragonBoat(BaseBoat boat)
            : base(0x18, new Point3D(0, -1, 0), boat)
        {
        }

        public ExtraLargeDockedDragonBoat(Serial serial)
            : base(serial)
        {
        }

        public override BaseBoat Boat
        {
            get
            {
                return new LargeDragonBoat();
            }
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
        }
    }
}