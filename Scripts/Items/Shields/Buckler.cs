using System;
using Server;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class Buckler : BaseShield
    {
        [Constructable]
        public Buckler()
            : base(0x1B73)
        {
            Name = "Puklerz";
            this.Weight = 5.0;
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

        public Buckler(Serial serial)
            : base(serial)
        {
        }

        public override int BasePhysicalResistance
        {
            get
            {
                return 0;
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
                return 1;
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
                return 40;
            }
        }
        public override int InitMaxHits
        {
            get
            {
                return 50;
            }
        }
        public override int AosStrReq
        {
            get
            {
                return 20;
            }
        }
        public override int ArmorBase
        {
            get
            {
                return 7;
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

            writer.Write((int)0);//version
        }
    }
}