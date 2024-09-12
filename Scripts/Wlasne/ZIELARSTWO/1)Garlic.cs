using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items.Crops
{
	public class GarlicPlant : BaseCrop 
	{ 
		private double mageValue;
		private DateTime lastpicked;

		[Constructable] 
		public GarlicPlant() : base( Utility.RandomList( 0xC68, 0xC68 ) ) 
		{ 
			Movable = false; 
			Name = "Sadzonka czosnku"; 
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
						from.Direction = from.GetDirectionTo( this );
						from.Animate( 32, 5, 1, true, false, 0 ); // Bow

                                                from.CheckSkill( SkillName.Zielarstwo, 0.0, 40.0 );

						from.SendMessage( 58, "Wyrwales rosline!"); 
						this.Delete(); 

						from.AddToBackpack( new GarlicUprooted() );
				} 
				else 
				{ 
					from.SendMessage( 33, "Jestes za daleko!" ); 
				} 
			}
		} 

		public GarlicPlant( Serial serial ) : base( serial ) 
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

	[FlipableAttribute( 0x18E3, 0x18E4 )]
	public class GarlicUprooted : Item, ICarvable
	{
		public void Carve( Mobile from, Item item )
		{
			int count = Utility.Random( 10 );
			if ( count == 0 ) 
			{
				from.SendMessage( 33, "Nie udalo sie zdobyc ziol!" ); 
				this.Consume();
			}
			else
			{
                            from.CheckSkill( SkillName.Zielarstwo, 0.0, 40.0 );
				base.ScissorHelper( from, new Garlic(), count );
				from.SendMessage( 58, "Pozyskales {0} czosnku{1}.", count, ( count == 1 ? "" : "" ) ); 
			}

		}

		[Constructable]
		public GarlicUprooted() : this( 1 )
		{
		}

		[Constructable]
		public GarlicUprooted( int amount ) : base( Utility.RandomList( 0x18E3, 0x18E4 ) )
		{
			Stackable = false;
			Weight = 1.0;
			
			Movable = true; 
			Amount = amount;

			Name = "glowka czosnku";
		}

		public GarlicUprooted( Serial serial ) : base( serial )
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
