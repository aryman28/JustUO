using System;
using System.Xml;
using Server;
using Server.Spells;
using Server.Mobiles;

namespace Server.Regions
{
	public class Linhal : BaseRegion
	{
		public Linhal( XmlElement xml, Map map, Region parent ) : base( xml, map, parent )
		{
		}
     
                public override void OnEnter( Mobile from )
		{
		from.SendMessage( 0x35, "Wkroczyles na teren osady Linhal!" );
                }

		public override void OnExit( Mobile from )
		{
		from.SendMessage( 0x35, "Opusciles teren osady Linhal!" );
                }

		public override bool AllowHousing( Mobile from, Point3D p )
		{
			return true;
		}



	}
}