using System;

namespace Server.Mobiles
{
    [CorpseName("ciało drzewca")]
    public class ArcaneFiend : BaseCreature
    {
        [Constructable]
        public ArcaneFiend()
             : base( AIType.AI_Melee, FightMode.Evil, 10, 1, 0.2, 0.4 )
        {
            Name = "drzewiec";
            Body = 301;
            BaseSoundID = 442;

            SetStr( 196, 220 );
            SetDex( 31, 55 );
            SetInt( 66, 90 );

            SetHits( 130, 150 );

            SetDamage( 10, 14 );

            SetDamageType( ResistanceType.Physical, 100 );

            SetResistance( ResistanceType.Physical, 70, 100 );
            SetResistance( ResistanceType.Cold, 50, 60 );
            SetResistance( ResistanceType.Poison, 30, 35 );
            SetResistance( ResistanceType.Energy, 20, 30 );

            SetSkill( SkillName.ObronaPrzedMagia, 100.0 );
            SetSkill( SkillName.Taktyka, 100.0 );
            SetSkill( SkillName.Boks, 100.0 );

            VirtualArmor = 34;
            ControlSlots = 3;
        }


        public ArcaneFiend(Serial serial)
            : base(serial)
        {
        }

        public override double DispelDifficulty
        {
            get
            {
                return 70.0;
            }
        }
        public override double DispelFocus
        {
            get
            {
                return 20.0;
            }
        }
        public override PackInstinct PackInstinct
        {
            get
            {
                return PackInstinct.Daemon;
            }
        }
        public override bool BleedImmune
        {
            get
            {
                return true;
            }
        }//TODO: Verify on OSI.  Guide says this.
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}