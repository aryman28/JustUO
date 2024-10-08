using System;
using Server;
using Server.Engines.XmlSpawner2;
using Server.Guilds;

namespace Server.Items
{
    public class OrderShield : BaseShield
    {
        [Constructable]
        public OrderShield()
            : base(0x1BC4)
        {
            if (!Core.AOS)
                this.LootType = LootType.Newbied;

            Name = "Tarcza Sprawiedliwości";
            this.Weight = 7.0;
            switch(Utility.Random(1))
            {
                    case 0:
                    // add a specific list of custom defenses like this
                    XmlAttach.AttachTo(this, 
                        new XmlCustomDefenses(
                            new XmlCustomDefenses.SpecialDefenses []
                            { 
                                XmlCustomDefenses.SpecialDefenses.SpikeShield,
                                XmlCustomDefenses.SpecialDefenses.ParalyzingFear
                            }
                        )
                    );
                    break;
            }
        }

        public OrderShield(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 3;
            }
        }
        public override int BaseFireResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BaseColdResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BasePoisonResistance
        {
            get
            {
                return 0;
            }
        }
        public override int BaseEnergyResistance
        {
            get
            {
                return 0;
            }
        }
        public override int InitMinHits
        {
            get
            {
                return 100;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 125;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 95;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 30;
            }
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            if (this.Weight == 6.0)
                this.Weight = 7.0;
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);//version
        }

        public override bool OnEquip(Mobile from)
        {
            return this.Validate(from) && base.OnEquip(from);
        }

        public override void OnSingleClick(Mobile from)
        {
            if (this.Validate(this.Parent as Mobile))
                base.OnSingleClick(from);
        }

        public virtual bool Validate(Mobile m)
        {
            if (Core.AOS || m == null || !m.Player || m.IsStaff())
                return true;

            Guild g = m.Guild as Guild;

            if (g == null || g.Type != GuildType.Order)
            {
                m.FixedEffect(0x3728, 10, 13);
                this.Delete();

                return false;
            }

            return true;
        }
    }
}