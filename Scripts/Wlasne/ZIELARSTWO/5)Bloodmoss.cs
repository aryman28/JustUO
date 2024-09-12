using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items.Crops
{
	public class BloodmossPlant : BaseCrop 
	{ 
		private double mageValue;
		private DateTime lastpicked;

		[Constructable] 
		public BloodmossPlant() : base( Utility.RandomList( 0x18E5, 0x18E6 ) ) 
		{ 
			Movable = false; 
			Hue = 33;
			Name = "Sadzonka krwawego mchu"; 
			lastpicked = DateTime.Now;
		} 

		public override void OnDoubleClick(Mobile from) 
		{ 
			if ( from == null || !from.Alive ) return;

			if ( DateTime.Now > lastpicked.AddSeconds(3) ) // 3 seconds between picking
			{
				lastpicked = DateTime.Now;

				if ( from.InRange( this.GetWorldLocation(), 1 ) ) 
				{ 

                                                if ( from.Skills.Zielarstwo.Value < 100 )
                                                {
                                                from.SendMessage(33, "Nie udalo ci sie zebrac ziol!"); 
                                                return;
                                                }

						from.Direction = from.GetDirectionTo( this );
						from.Animate( 32, 5, 1, true, false, 0 ); // Bow

                                                from.CheckSkill( SkillName.Zielarstwo, 100.0, 100.0 );

						from.SendMessage( 58, "Wyrwales rosline!"); 
						this.Delete(); 

						from.AddToBackpack( new BloodmossUprooted() );
				} 
				else 
				{ 
					from.SendMessage( 33, "Jestes za daleko!" ); 
				} 
			}
		} 

		public BloodmossPlant( Serial serial ) : base( serial ) 
		{ 
			lastpicked = DateTime.Now;
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

	[FlipableAttribute( 0x18E7, 0x18E8 )]
	public class BloodmossUprooted : Item, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			int count = Utility.Random( 10 );
			if ( count == 0 ) 
			{
				from.SendMessage( 33, "Nie udalo ci sie zdobyc ziol!" ); 
				this.Consume();
			}
			else
			{
				base.ScissorHelper( from, new Bloodmoss(), count );
				from.SendMessage( 58, "Pozyskales {0} krwawego mchu{1}.", count, ( count == 1 ? "" : "" ) ); 
			}

		}

		[Constructable]
		public BloodmossUprooted() : this( 1 )
		{
		}

		[Constructable]
		public BloodmossUprooted( int amount ) : base( Utility.RandomList( 0x18E7, 0x18E8 ) )
		{
			Stackable = false;
			Weight = 1.0;
			Hue = 33;
			Movable = true; 
			Amount = amount;

			Name = "korzen krwawego mchu";
		}

		public BloodmossUprooted( Serial serial ) : base( serial )
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
}