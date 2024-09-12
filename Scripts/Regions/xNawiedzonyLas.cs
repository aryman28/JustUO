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
	public class NawiedzonyLas : BaseRegion
	{
		public NawiedzonyLas( XmlElement xml, Map map, Region parent ) : base( xml, map, parent )
		{
		}
     
                public override void OnEnter( Mobile from )
		{
		from.SendMessage( 0x35, "Wkroczy³eœ na teren Nawiedzonego Lasu!" );
                }

		public override void OnExit( Mobile from )
		{
		from.SendMessage( 0x35, "Opuœci³eœ Nawiedzony Las!" );
                }

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return false;
		}

        public override bool OnBeginSpellCast(Mobile m, ISpell s)
        {
            if ((s is GateTravelSpell || s is RecallSpell || s is MarkSpell || s is SacredJourneySpell) && m.IsPlayer())
            {
                m.SendMessage("Nie mo¿esz tu rzuciæ tego zaklêcia.");
                return false;
            }
            else
            {
                return base.OnBeginSpellCast(m, s);
            }
        }

	}
}