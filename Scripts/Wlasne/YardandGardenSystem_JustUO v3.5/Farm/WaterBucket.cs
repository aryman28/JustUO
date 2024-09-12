using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Gumps;

namespace Server.ACC.YS

{
	public class WaterBucket : Item
	{
		public int Litry;
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int m_litry
		{
			get { return Litry; }
			set { Litry = value; }
		}
		
		[Constructable]
		public WaterBucket() : base( 0x0FFA )
		{
			Weight = 10.0;
			Name = "Pojemnik Na Wode: (Pusty)";
			Hue = 1001;
		}
		
		public override void OnSingleClick( Mobile from )
		{
			base.OnSingleClick( from );
			this.LabelTo( from,Litry.ToString());
		}
		
		
		public override void OnDoubleClick (Mobile from )
		{
			if (! from.InRange( this.GetWorldLocation(), 1 ))
			{
				from.LocalOverheadMessage( MessageType.Regular, 906, 1019045 ); // I can't reach that.
			}
			else
			{
				from.Target = new OnW( this );
				from.SendMessage(0x96D, "Na czym chcesz tego u¿yæ?" );
			}
		}
		public WaterBucket( Serial serial ) : base( serial )
		{
		}
		
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			
			writer.Write( (int) 0 );
			writer.Write( (int) Litry );
		}
		
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			
			int version = reader.ReadInt();
			switch ( version )
			{
				        case 0:
					{
						Litry = reader.ReadInt();
						break;
					}
			}
		}
	}
	
	public class OnW : Target
	{
		private WaterBucket m_pojemnik;
		Mobile m_mobile = null;

		private static int[,] WaterTiles_Land = new int[3,2]
		{	{ 168, 171, }, { 310, 311, }, { 16368, 16371 }};
		private static int[,] WaterTiles_Static = new int[18,2]
		{	{ 2881, 2884 }, { 4088, 4089 }, { 4090, 4090 },
			{ 4104, 4104 }, { 5453, 5453 }, { 5456, 5462 },
			{ 5464, 5465 }, { 5937, 5978 }, { 6038, 6066 },
			{ 6595, 6636 }, { 8081, 8084 }, { 8093, 8094 },
			{ 8099, 8138 },	{ 9299, 9309 }, { 13422, 13445 },
			{ 13456, 13483 }, { 13493, 13525 }, { 13549, 13616 }};
		private static int[,] Farm_Land = new int[7,2]                                                      //Ziemia StaticTile//Ziemia1 StaticTile
		{	{ 0x009, 0x00A, }, { 0x00C, 0x00E, }, { 0x013, 0x015, }, { 0x150, 0x155, }, { 0x15A, 0x15C }, { 13001, 13001}, { 12788, 12788}};


                public OnW( WaterBucket m_saut ) : base( 3, true, TargetFlags.None )
		{
			m_pojemnik = m_saut;
		}
		
		protected override void OnTarget( Mobile from, object o )
		{
				int id;
				bool found = false;

			if( o is Item )
                        {
                              Item targ = (Item)o;

                           if (!from.InRange(targ.GetWorldLocation(), 1))
                           {
                               from.SendLocalizedMessage(500446); // That is too far away.
                               return;
                           }
                        }
			if( o is LandTarget )
			{
				id = (o as LandTarget).TileID;

				for( int i = 0; i < WaterTiles_Land.Length / 2; i++ )
				{
					if( id >= WaterTiles_Land[i,0] && id <= WaterTiles_Land[i,1] )
					{
                                                  if ( m_pojemnik.m_litry <= 9 )
                                                  {
                                                         m_pojemnik.m_litry = 10;
                                                         from.PlaySound(0X027);
					                 m_pojemnik.Name ="Pojemnik Na Wode: " + m_pojemnik.m_litry.ToString() + "/10 litrów wody";
					                 from.SendMessage(0x96D, "Pobra³eœ 10 litrów wody." );
                                                  }

						found = true;
						break;
					}
				}
                        }
			if ( o is StaticTarget || o is Item )
			{

                                   if( o is WaterBucket )
                                   {
                                       from.SendMessage(0x96D, "To nie jest dobry pomys³!" );
                                       return;
                                   }

				if( o is Item )
					id = (o as Item).ItemID;
				else
					id = (o as StaticTarget).ItemID;

				for( int i = 0; i < WaterTiles_Static.Length / 2; i++ )
				{
					if( id >= WaterTiles_Static[i,0] && id <= WaterTiles_Static[i,1] )
					{
                                                  if ( m_pojemnik.m_litry <= 9 )
                                                  {
                                                         m_pojemnik.m_litry = 10;
                                                         from.PlaySound(0X027);
					                 m_pojemnik.Name ="Pojemnik Na Wode: " + m_pojemnik.m_litry.ToString() + "/10 litrów wody";
					                 from.SendMessage(0x96D, "Pobra³eœ 10 litrów wody." );
                                                  }
							found = true;
							break;
					}
				 }        
			  }	
			  if ( o is YardFarm )
			  {
					id = (o as YardFarm).ItemID;
                                        Item item = o as YardFarm;

				for( int i = 0; i < Farm_Land.Length / 2; i++ )
				{
					if( id >= Farm_Land[i,0] && id <= Farm_Land[i,1] )
					{
                                                  if ( m_pojemnik.m_litry > 0 )
                                                  {
					               m_pojemnik.m_litry = m_pojemnik.m_litry -1;
                                                       item.Hue = 2110;
                                                       from.PlaySound(0X026);
					               from.SendMessage (0x96D,"Wyla³eœ 1 litr wody.");
					               m_pojemnik.Name ="Pojemnik Na Wode: " + m_pojemnik.m_litry.ToString() + "/10 litrów wody.";
                                                  }
							found = true;
							break;
					}
				}
                         }

				if ( m_pojemnik.m_litry <= 0 )
				{
					m_pojemnik.Name = "Pojemnik Na Wode: (Pusty)";
				}

		}
	}
}

