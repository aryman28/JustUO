using System; 
using System.Collections;
using Server.Network; 
using Server.Mobiles; 
using Server.Items; 
using Server.Gumps;

namespace Server.Items.Crops 
{ 
	public class ZiemniakSeed : BaseCrop 
	{ 
		// return true to allow planting on Dirt Item (ItemID 0x32C9)
		// See CropHelper.cs for other overriddable types
		public override bool CanGrowGarden{ get{ return true; } }
		
		[Constructable]
		public ZiemniakSeed() : this( 1 )
		{
		}

		[Constructable]
		public ZiemniakSeed( int amount ) : base( 0xF27 )
		{
			Stackable = true; 
			Weight = .5; 
			Hue = 0x5E2; 

			Movable = true; 
			
			Amount = amount;
			Name = "nasiona ziemniaka"; 
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( from.Mounted && !CropHelper.CanWorkMounted )
			{
				from.SendMessage( 33, "Nie mozesz uprawiac roslin na wierzchowcu!" ); 
				return; 
			}

			Point3D m_pnt = from.Location;
			Map m_map = from.Map;

			if ( !IsChildOf( from.Backpack ) ) 
			{ 
				from.SendLocalizedMessage( 1042010 ); //You must have the object in your backpack to use it. 
				return; 
			} 

			else if ( !CropHelper.CheckCanGrow( this, m_map, m_pnt.X, m_pnt.Y ) )
			{
				from.SendMessage( 33, "Ta sadzonka nie urosnie tutaj!" ); 
				return; 
			}
			
			//check for BaseCrop on this tile
			ArrayList cropshere = CropHelper.CheckCrop( m_pnt, m_map, 0 );
			if ( cropshere.Count > 0 )
			{
				from.SendMessage( 33, "Tutaj juz cos rosnie!" ); 
				return;
			}

			//check for over planting prohibt if 4 maybe 3 neighboring crops
			ArrayList cropsnear = CropHelper.CheckCrop( m_pnt, m_map, 1 );
			if ( ( cropsnear.Count > 8 ) || (( cropsnear.Count == 8 ) && Utility.RandomBool() ) )
			{
				from.SendMessage( 33, "W tym miejscu jest zbyt wiele roslin!" ); 
				return;
			}

if (from.Skills.Rolnictwo.Value >= 100 )
{
			if ( this.BumpZ ) ++m_pnt.Z;

			if ( !from.Mounted )
				from.Animate( 32, 5, 1, true, false, 0 ); // Bow

			from.SendMessage( 58, "Zasadziles rosline!" ); 
			this.Consume(); 
			Item item = new ZiemniakSeedling( from ); 
			item.Location = m_pnt; 
			item.Map = m_map; 
}			
if (from.Skills.Rolnictwo.Value < 100 )
{
from.SendMessage( 33, "Twoja wiedza jest za mala aby zasiac te rosline!");
}

		} 

		public ZiemniakSeed( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
		} 
	} 


	public class ZiemniakSeedling : BaseCrop 
	{ 
		private static Mobile m_sower;
		public Timer thisTimer;

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Sower{ get{ return m_sower; } set{ m_sower = value; } }
		
		[Constructable] 
		public ZiemniakSeedling( Mobile sower ) : base( 0xCB6 ) 
		{ 
			Movable = false; 
			Name = "sadzonka ziemniaka"; 
			m_sower = sower;
			
			init( this );
		} 

		public static void init( ZiemniakSeedling plant )
		{
			plant.thisTimer = new CropHelper.GrowTimer( plant, typeof(ZiemniakCrop), plant.Sower ); 
			plant.thisTimer.Start(); 
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 
			if ( from.Mounted && !CropHelper.CanWorkMounted )
			{
				from.SendMessage( "Nie mozesz tego zebrac bedac na wierzchowcu." ); 
				return; 
			}

			else from.SendMessage( 33, "Roslina jest za mloda!" ); 
		}

		public ZiemniakSeedling( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 0 ); 
			writer.Write( m_sower );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
			m_sower = reader.ReadMobile();

			init( this );
		} 
	} 

	public class ZiemniakCrop : BaseCrop 
	{ 
		private const int max = 3;
		private int fullGraphic;
		private int pickedGraphic;
		private DateTime lastpicked;

		private Mobile m_sower;
		private int m_yield;

		public Timer regrowTimer;

                /////Czas Niszczenia
                public Timer destroyTimer;
                /////
		private DateTime m_lastvisit;

		[CommandProperty( AccessLevel.GameMaster )] 
		public DateTime LastSowerVisit{ get{ return m_lastvisit; } }

		[CommandProperty( AccessLevel.GameMaster )] // debuging
		public bool Growing{ get{ return regrowTimer.Running; } }

		[CommandProperty( AccessLevel.GameMaster )]
		public Mobile Sower{ get{ return m_sower; } set{ m_sower = value; } }
		
		[CommandProperty( AccessLevel.GameMaster )]
		public int Yield{ get{ return m_yield; } set{ m_yield = value; } }

		public int Capacity{ get{ return max; } }
		public int FullGraphic{ get{ return fullGraphic; } set{ fullGraphic = value; } }
		public int PickGraphic{ get{ return pickedGraphic; } set{ pickedGraphic = value; } }
		public DateTime LastPick{ get{ return lastpicked; } set{ lastpicked = value; } }
		
		[Constructable] 
		public ZiemniakCrop( Mobile sower ) : base( 0xC63 ) 
		{ 
			Movable = false; 
			Name = "ziemniak"; 
                        Hue = 0x290;
			m_sower = sower;
			m_lastvisit = DateTime.Now;

			init( this, false );
		}

		public static void init ( ZiemniakCrop plant, bool full )
		{
			plant.PickGraphic = ( 0xC61 );
			plant.FullGraphic = ( 0xC63 );

			plant.LastPick = DateTime.Now;
			plant.regrowTimer = new CropTimer( plant );
                        /////Czas Niszczenia
                        plant.destroyTimer = new DestroyTimer( plant );
                        /////

			if ( full )
			{
				plant.Yield = plant.Capacity;
				((Item)plant).ItemID = plant.FullGraphic;

                                  /////Rozpoczyna sie czas niszczenia
                                  if ( !plant.destroyTimer.Running )
                                  {
                                        plant.destroyTimer.Start();
                                  }
                                  /////
			}
			else
			{
				plant.Yield = 0;
				((Item)plant).ItemID = plant.PickGraphic;
				plant.regrowTimer.Start();
			}
		}

		public override void OnDoubleClick( Mobile from ) 
		{ 

/////Wymagana Kosa
if (from.FindItemOnLayer(Layer.OneHanded) is Kosa)
{
BaseAxe kosa = from.FindItemOnLayer(Layer.OneHanded) as BaseAxe;

  if ( kosa.UsesRemaining <= 0 )
  {
  kosa.Delete();
  from.SendMessage( 33, "Kosa rozpadla sie!" ); 
  }
			if ( m_sower == null || m_sower.Deleted ) 
				m_sower = from;

			if ( from != m_sower ) 
			{ 
			from.SendMessage( 33, "To nie jest twoja roslina!" ); 
			from.Criminal = true;
			}

			if ( from.Mounted && !CropHelper.CanWorkMounted )
			{
				from.SendMessage( 33, "Nie mozesz tego zebrac bedac na wierzchowcu!" ); 
				return; 
			}

			if ( DateTime.Now > lastpicked.AddSeconds(3) ) // 3 seconds between picking
			{
				lastpicked = DateTime.Now;
			
				if ( from.InRange( this.GetWorldLocation(), 1 ) ) 
				{ 
					if ( m_yield < 1 )
					{
						from.SendMessage( 33, "Nie ma tu nic do zebrania!" ); 

						if ( PlayerCanDestroy && !( m_sower.AccessLevel > AccessLevel.Counselor ) )
						{  
						  from.SendMessage( 33, "Wyrwales rosline z korzeniami!" ); 
						  this.Delete();
						}
					}
					else //check sower
					{
						from.Direction = from.GetDirectionTo( this );

						//from.Animate( from.Mounted ? 29:32, 5, 1, true, false, 0 ); 
              from.CheckSkill( SkillName.Rolnictwo, 100.0, 100.0 );
              kosa.UsesRemaining -= 1;
              Effects.PlaySound( from.Location, from.Map, 0x23A);
              from.Animate( 13, 5, 2, true, false, 0 ); //zcinanie 2 powtórzenia

						if ( from == m_sower ) 
						{
							m_lastvisit = DateTime.Now;
						}

if (from.Skills.Rolnictwo.Value < 100 )
{
from.SendMessage( 33, "Nie udalo ci sie tego zebrac!");
}
else if (from.Skills.Rolnictwo.Value >= 100 )
{

						 //int pick = Utility.Random( m_yield + 1 );
			                         int pick = 1;

if (from.Skills.Rolnictwo.Value >= 70 && m_yield >= 3 || from.Skills.Rolnictwo.Value >= 70 && m_yield >= 2)
{
pick = 2;
}
if (from.Skills.Rolnictwo.Value >= 70 && m_yield == 1 )
{
pick = 1;
}
if (from.Skills.Rolnictwo.Value >= 95 )
{
pick = m_yield;
}

						m_yield -= pick;
						from.SendMessage( 58, "Zebrales {0} Ziemniaki{1}!", pick, ( pick == 1 ? "" : "" ) ); 

						((Item)this).ItemID = pickedGraphic;

                                                if ( pick > 0 )
						{
						Ziemniak crop = new Ziemniak( pick ); 
						from.AddToBackpack( crop );
                                                }

						if ( !regrowTimer.Running )
						{
							regrowTimer.Start();
						}
}
					}
				} 
				else 
				{ 
					from.SendMessage( 33, "Jestes za daleko!" ); 
				} 
			}
}
else
{
from.SendMessage( 33, "Potrzebujesz do tego Kosy!" );
}
		} 

		private class CropTimer : Timer
		{
			private ZiemniakCrop i_plant;

			public CropTimer( ZiemniakCrop plant ) : base( TimeSpan.FromMinutes( 20 ), TimeSpan.FromMinutes( 30 ) )
			{
				Priority = TimerPriority.OneSecond;
				i_plant = plant;
			}

			protected override void OnTick()
			{
				if ( ( i_plant != null ) && ( !i_plant.Deleted ) )
				{
					int current = i_plant.Yield;

					if ( ++current >= i_plant.Capacity )
					{
						current = i_plant.Capacity;
						((Item)i_plant).ItemID = i_plant.FullGraphic;
						Stop();
					}
					else if ( current <= 0 )
						current = 1;

					i_plant.Yield = current;
					//i_plant.PublicOverheadMessage( MessageType.Regular, 0x22, false, string.Format( "{0}", current )); 
				}
				else Stop();
			}
		}

                /////Niszczenie
		private class DestroyTimer : Timer
		{
			private ZiemniakCrop i_plant;

			public DestroyTimer( ZiemniakCrop plant ) : base( TimeSpan.FromHours( 20 ), TimeSpan.FromHours( 24 ) )
			{
				Priority = TimerPriority.OneSecond;
				i_plant = plant;
			}

			protected override void OnTick()
			{
				if ( i_plant != null )
				{
                                          i_plant.Delete();					
						    Stop(); 
				}
				else Stop();
			}
		}
                /////Niszczenie

		public ZiemniakCrop( Serial serial ) : base( serial ) 
		{ 
		} 

		public override void Serialize( GenericWriter writer ) 
		{ 
			base.Serialize( writer ); 
			writer.Write( (int) 1 ); 
			writer.Write( m_lastvisit );
			writer.Write( m_sower );
		} 

		public override void Deserialize( GenericReader reader ) 
		{ 
			base.Deserialize( reader ); 
			int version = reader.ReadInt(); 
			switch ( version )
			{
				case 1:
				{
					m_lastvisit = reader.ReadDateTime();
					goto case 0;
				}
				case 0:
				{
					m_sower = reader.ReadMobile();
					break;
				}
			}

			if ( version == 0 ) 
				m_lastvisit = DateTime.Now;

			init( this, true );
		} 
	} 
} 
