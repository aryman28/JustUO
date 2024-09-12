using System;
using System.Collections;
using Server.Network;
using Server.Items;
using Server.Multis;

using Server.Items.Crops;

namespace Server.ACC.YS
{
	public class YardFarm : Item
	{
		public Mobile m_Placer;
		public int m_Value = 0;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Placer
		{
			get{ return m_Placer; }
			set{ m_Placer = value; }
		}

        private BaseHouse m_House;
        [CommandProperty(AccessLevel.GameMaster)]
        public BaseHouse House
        {
            get { return m_House; }
            set { m_House = value; }
        }

        private BaseCrop m_Crop;
        [CommandProperty(AccessLevel.GameMaster)]
        public BaseCrop Crop
        {
            get { return m_Crop; }
            set { m_Crop = value; }
        }

		[Constructable]
		public YardFarm( Mobile from, int id, Point3D loc, int price, BaseHouse house, BaseCrop crop)
		{
			m_Value = price;
			Movable = false;
			MoveToWorld( loc, from.Map );
			m_Placer = from;
			Name = from.Name + "(ziemia uprawna)";
			ItemID = id;
			Light = LightType.Circle150;

            if (house == null)
            {
                FindHouseOfPlacer();
            }
            if (crop == null)
            {
                FindCropOfPlacer();
            }
            else
            {
                House = house;
                Crop = crop;
            }


		}

		public YardFarm( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{

            base.Serialize(writer);
            writer.Write((int)1); // version

            //Version 1
            if (House == null || House.Deleted || Crop == null || Crop.Deleted )
            {
                writer.Write(false);
                YardFarmSystem.AddFarmItem(this);
            }
            else
            {
                writer.Write(true);
                writer.Write(House);
                writer.Write(Crop);
            }

            //Version 0
	        writer.Write( m_Placer );
	        writer.Write( m_Value );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();

            switch (version)
            {
                case 1:
                    {
                        if (reader.ReadBool())
                        {
                            House = reader.ReadItem() as BaseHouse;
                            Crop = reader.ReadItem() as BaseCrop;
                        }
                        goto case 0;
                    }
                case 0:
                    {
			m_Placer = reader.ReadMobile();
			m_Value = reader.ReadInt();
                        break;
                    }
             }
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

        public void FindCropOfPlacer()
        {
            if (Placer == null || Crop != null)
            {
                return;
            }

            IPooledEnumerable eable = Map.GetItemsInRange(Location, 0);
            foreach (Item item in eable)
            {
                if (item is BaseCrop)
                {
                  if (House == null && Crop == null)
                  {
                        item.Delete();
                        return;
                  }
                    BaseCrop crop = (BaseCrop)item;
                    
                        Crop = crop;
                        return;
                }
            }
        }

      public override void OnDoubleClick( Mobile from )
      {
			if( from.InRange( this.GetWorldLocation(), 2 ) )
			{
                                if ( this.Hue == 2110 && from == m_Placer )
                                {
                                   m_Placer.SendMessage( 33, "Musisz poczekaæ a¿ ziemia wyschnie aby j¹ usunaæ!" );
                                   return;
                                }
				if( m_Placer == null || from == m_Placer || from.AccessLevel >= AccessLevel.GameMaster )
				{
					Container c = m_Placer.Backpack;
					Gold t = new Gold( m_Value );
					if( c.TryDropItem( m_Placer, t, true ) )
					{
						this.Delete();
						m_Placer.SendMessage( 248, "Ten przedmiot zosta³ usuniêty a monety zwrócono tobie" );
					}
					else
					{
						t.Delete();
						m_Placer.SendMessage( 33, "Z jakiegoœ powodu zwrot monet nie dzia³a! skontaktuj siê z ekip¹");
					}
				}
				else
				{
					from.SendMessage( 33, "Wynocha z mojego ogrodu!" );
				}
			}
			else
			{
				from.SendMessage( 33, "Ten przedmiot jest za daleko" );
			}
		}
	}
}