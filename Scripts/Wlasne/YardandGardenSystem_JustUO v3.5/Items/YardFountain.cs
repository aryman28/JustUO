using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Multis;
using Server.Targeting;

namespace Server.ACC.YS
{
	public enum TypeOfFountain
	{
		Sand,
		Stone,
	}

	public class YardFountain : Item
	{
		private ArrayList m_Components;
		private static Mobile m_Placer;
		private static int m_Value = 0;
                private BaseHouse m_House;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Placer
		{
			get{ return m_Placer; }
			set{ m_Placer = value; }
		}

        [CommandProperty(AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get { return m_House; }
            set { m_House = value; }
        }

		[CommandProperty( AccessLevel.GameMaster )]
		public int Price
		{
			get{ return m_Value; }
			set{ m_Value = value; }
		}

//PIECES\\
		private class Piece : Item
		{
			YardFountain IsPartOf;
			public Piece( int itemID, String name, YardFountain ThisFountain ) : base( itemID )
			{
				Movable = false;
				Name = name;
				IsPartOf=ThisFountain;
			}

			public Piece( Serial serial ) : base( serial )
			{
			}

			public override void OnAfterDelete()
			{
				if(IsPartOf != null)
					IsPartOf.OnAfterDelete();
				else
					base.OnAfterDelete();
			}

			public override void OnDoubleClick( Mobile from )
			{
				if(IsPartOf != null)
					IsPartOf.OnDoubleClick(from);
				else
					base.OnDoubleClick(from);
			}

			public override void Serialize( GenericWriter writer )
			{
				base.Serialize( writer );
				writer.Write( (int) 0 ); // version
				writer.Write( IsPartOf );
			}
			public override void Deserialize( GenericReader reader )
			{
				base.Deserialize( reader );
				int version = reader.ReadInt();
				switch (version)
				{
					case 0:
					{
						IsPartOf = reader.ReadItem() as YardFountain;
						break;
					}
				}
			}

		}
//END PIECES\\

		public YardFountain( Mobile from, int price, BaseHouse house, TypeOfFountain type, Point3D loc )
		{
			m_Value = price;
			m_Placer = from;
			int id = 0;
			string name = "";
			name = from.Name + "(Fontanna)";
			Name = name;
			Movable = false;
			MoveToWorld( loc, from.Map );

            if (house == null)
            {
                FindHouseOfPlacer();
            }
            else
            {
                House = house;
            }

			m_Components = new ArrayList();

			switch ( type )
			{
				case TypeOfFountain.Stone:
				{
					id = 0x1731;
					ItemID = id+9;
				}
				break;
				case TypeOfFountain.Sand:
				{
					id = 0x19C3;
					ItemID = id+9;
				}
				break;
			}
			AddPiece( -2, +1, 0, id++, name, loc);
			AddPiece( -1, +1, 0, id++, name, loc);
			AddPiece( +0, +1, 0, id++, name, loc);
			AddPiece( +1, +1, 0, id++, name, loc);

			AddPiece( +1, +0, 0, id++, name, loc);
			AddPiece( +1, -1, 0, id++, name, loc);
			AddPiece( +1, -2, 0, id++, name, loc);

			AddPiece( +0, -2, 0, id++, name, loc);
			AddPiece( +0, -1, 0, id++, name, loc);
			//AddPiece( +0, +0, 0, id++, name, loc);
			id++;
			AddPiece( -1, +0, 0, id++, name, loc);
			AddPiece( -2, +0, 0, id++, name, loc);

			AddPiece( -2, -1, 0, id++, name, loc);
			AddPiece( -1, -1, 0, id++, name, loc);

			AddPiece( -1, -2, 0, id++, name, loc);
			AddPiece( -2, -2, 0, ++id, name, loc);

			m_Components.Add( this );
		}

        public void FindHouseOfPlacer()
        {
            if (Placer == null || House != null)
            {
                return;
            }

            IPooledEnumerable eable = Map.GetItemsInRange(Location, 20);
            foreach (Item item in eable)
            {
                if (item is BaseHouse)
                {
                    BaseHouse house = (BaseHouse)item;
                    if (house.Owner == Placer)
                    {
                        House = house;
                        return;
                    }
                }
            }
        }

		private void AddPiece( int x, int y, int z, int itemID, string name, Point3D loc)
		{
			PlaceAndAdd( x, y, z, new Piece( itemID, name, this), loc );
		}

		private void PlaceAndAdd( int x, int y, int z, Item item, Point3D loc )
		{
			item.MoveToWorld( new Point3D( loc.X + x, loc.Y + y, loc.Z + z ), m_Placer.Map );
			m_Components.Add( item );
		}


		public YardFountain( Serial serial ) : base( serial )
		{
		}



		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write( m_Placer );
			writer.Write( m_Value );
			writer.WriteItemList( m_Components );

            if (House == null || House.Deleted)
            {
                YardSystem.AddOrphanedItem(this);
            }

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			switch ( version )
			{
				case 0:
				{
					m_Placer = reader.ReadMobile();
					m_Value = reader.ReadInt();
					m_Components = reader.ReadItemList();
					break;
				}
			}
		}

		public override void OnAfterDelete()
		{
			for ( int i = 0; i < m_Components.Count; ++i )
				((Item)m_Components[i]).Delete();
		}

		public override void OnDoubleClick( Mobile from )
		{
			if( from.InRange( this.GetWorldLocation(), 10 ) )
			{
				if( this.Placer == null || from == this.Placer || from.AccessLevel >= AccessLevel.GameMaster )
				{
					Container c = m_Placer.Backpack;
					Gold t = new Gold( m_Value );
					if( c.TryDropItem( m_Placer, t, true ) )
					{
						this.Delete();
						m_Placer.SendMessage( "Ten przedmiot zostal usuniêty a monety zwrócono tobie" );
					}
					else
					{
						t.Delete();
						m_Placer.SendMessage("Z jakiegoœ powodu zwrot monet nie dzia³a! skontaktuj siê z ekip¹");
					}
				}
				else
				{
					from.SendMessage( "Wynocha z mojego ogrodu!" );
				}
			}
			else
			{
				from.SendMessage( "Ten przedmiot jest za daleko" );
			}
		}
	}
}
