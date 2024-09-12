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
	public class NowaBrytania : BaseRegion
	{
		public NowaBrytania( XmlElement xml, Map map, Region parent ) : base( xml, map, parent )
		{
		}
     
                public override void OnEnter( Mobile from )
		{
		from.SendMessage( 0x35, "Wkroczy³eœ na teren Nowej Brytanii!" );
                }

		public override void OnExit( Mobile from )
		{
		from.SendMessage( 0x35, "Opuœci³eœ Now¹ Brytanie!" );
                }
        }
}