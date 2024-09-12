using System;
using System.Xml;
using Server;
using Server.Spells;
using Server.Mobiles;

using Server.Spells.Rycerstwo;
using Server.Spells.Fourth;
using Server.Spells.Seventh;
using Server.Spells.Sixth;

namespace Server.Regions
{
	public class MrocznaForteca : BaseRegion
	{
		public MrocznaForteca( XmlElement xml, Map map, Region parent ) : base( xml, map, parent )
		{
		}
     
                public override void OnEnter( Mobile from )
		{
		from.SendMessage( 0x35, "Wkroczy³eœ na teren Mrocznej Fortecy!" );
                }

		public override void OnExit( Mobile from )
		{
		from.SendMessage( 0x35, "Opuœci³eœ Mroczn¹ Fortece!" );
                }

	public override bool AllowHousing( Mobile from, Point3D p )
	{
		return false;
	}

        public override bool AllowBeneficial(Mobile from, Mobile target)
        {
            if (from.IsPlayer())
                from.SendMessage("Tutaj nie mo¿esz tego zrobiæ.");

            return (from.IsStaff());
        }

        public override bool AllowHarmful(Mobile from, Mobile target)
        {
            if (from.IsPlayer())
                from.SendMessage("Tutaj nie mo¿esz tego zrobiæ.");

            return (from.IsStaff());
        }

        public override void AlterLightLevel(Mobile m, ref int global, ref int personal)
        {
            global = LightCycle.JailLevel;
        }

        public override bool OnBeginSpellCast(Mobile from, ISpell s)
        {
            if (from.IsPlayer())
                from.SendLocalizedMessage(502629); // You cannot cast spells here.

            return (from.IsStaff());
        }

        public override bool OnSkillUse(Mobile from, int Skill)
        {
            if (from.IsPlayer())
                from.SendMessage("Tutaj nie mo¿esz tego zrobiæ.");

            return (from.IsStaff());
        }

        public override bool OnCombatantChange(Mobile from, Mobile Old, Mobile New)
        {
            return (from.IsStaff());
        }


	}
}