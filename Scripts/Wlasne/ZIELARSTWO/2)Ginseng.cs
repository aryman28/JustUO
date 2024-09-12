using System;
using Server.Network;
using Server.Targeting;

namespace Server.Items.Crops
{
	public class GinsengPlant : BaseCrop 
	{ 
		private double mageValue;
		private DateTime lastpicked;

		[Constructable] 
		public GinsengPlant() : base( Utility.RandomList( 0x18E9, 0x18EA ) ) 
		{ 
			Movable = false; 
			Name = "Sadzonka zen-szenia"; 
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

                                                if ( from.Skills.Zielarstwo.Value < 40 )
                                                {
                                                from.SendMessage( 33, "Nie udalo ci sie zebrac ziol!"); 
                                                return;
                                                }

						from.Direction = from.GetDirectionTo( this );
						from.Animate( 32, 5, 1, true, false, 0 ); // Bow

                                                from.CheckSkill( SkillName.Zielarstwo, 40.0, 60.0 );

						from.SendMessage( 58, "Wyrwales rosline!"); 
						this.Delete(); 

						from.AddToBackpack( new GinsengUprooted() );
				} 
				else 
				{ 
					from.SendMessage( 33, "Jestes za daleko!" ); 
				} 
			}
		} 

		public GinsengPlant( Serial serial ) : base( serial ) 
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
	public class GinsengUprooted : Item, ICarvable
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
                            from.CheckSkill( SkillName.Zielarstwo, 40.0, 60.0 );
				base.ScissorHelper( from, new Ginseng(), count );
				from.SendMessage( 58, "Pozyskales {0} zen-szenia{1}.", count, ( count == 1 ? "" : "" ) ); 
			}

		}

		[Constructable]
		public GinsengUprooted() : this( 1 )
		{
		}

		[Constructable]
		public GinsengUprooted( int amount ) : base( Utility.RandomList( 0x18EB, 0x18EC ) )
		{
			Stackable = false;
			Weight = 1.0;
			
			Movable = true; 
			Amount = amount;

			Name = "korzen zen-szenia";
		}

		public GinsengUprooted( Serial serial ) : base( serial )
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
