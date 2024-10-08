using System;
using System.Collections;
using Server.Network;
using Server.Regions;

namespace Server.Items
{
    public class Kindling : Item
    {
        [Constructable]
        public Kindling()
            : this(1)
        {
        }

        [Constructable]
        public Kindling(int amount)
            : base(0xDE1)
        {
            this.Stackable = true;
            this.Weight = 5.0;
            this.Amount = amount;
        }

        public Kindling(Serial serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.VerifyMove(from))
                return;

            if (!from.InRange(this.GetWorldLocation(), 2))
            {
                from.LocalOverheadMessage(MessageType.Regular, 0x3B2, 1019045); // I can't reach that.
                return;
            }

            Point3D fireLocation = this.GetFireLocation(from);

            if (fireLocation == Point3D.Zero)
            {
                from.SendLocalizedMessage(501695); // There is not a spot nearby to place your campfire.
            }
            else if (!from.CheckSkill(SkillName.Obozowanie, 0.0, 100.0))
            {
                from.SendLocalizedMessage(501696); // You fail to ignite the campfire.
            }
            else
            {
                this.Consume();

                if (!this.Deleted && this.Parent == null)
                    from.PlaceInBackpack(this);

                new Campfire().MoveToWorld(fireLocation, from.Map);
            }
        }

        private Point3D GetFireLocation(Mobile from)
        {
            if (from.Region.IsPartOf(typeof(DungeonRegion)))
                return Point3D.Zero;

            if (this.Parent == null)
                return this.Location;

            ArrayList list = new ArrayList(4);

            this.AddOffsetLocation(from, 0, -1, list);
            this.AddOffsetLocation(from, -1, 0, list);
            this.AddOffsetLocation(from, 0, 1, list);
            this.AddOffsetLocation(from, 1, 0, list);

            if (list.Count == 0)
                return Point3D.Zero;

            int idx = Utility.Random(list.Count);
            return (Point3D)list[idx];
        }

        private void AddOffsetLocation(Mobile from, int offsetX, int offsetY, ArrayList list)
        {
            Map map = from.Map;

            int x = from.X + offsetX;
            int y = from.Y + offsetY;

            Point3D loc = new Point3D(x, y, from.Z);

            if (map.CanFit(loc, 1) && from.InLOS(loc))
            {
                list.Add(loc);
            }
            else
            {
                loc = new Point3D(x, y, map.GetAverageZ(x, y));

                if (map.CanFit(loc, 1) && from.InLOS(loc))
                    list.Add(loc);
            }
        }
    }
}