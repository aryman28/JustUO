using System; 
using Server; 
using System.Collections; 
using Server.Gumps; 
using Server.Items; 
using Server.Network; 
using Server.Targeting; 
using Server.ContextMenus; 

namespace Server.Mobiles 
{ 
   public class AnimalTrainerLord : BaseCreature//was BaseVendor 
   { 
      
      public virtual bool IsInvulnerable{ get{ return true; } }
      
      
       [Constructable]
      public AnimalTrainerLord() : base(AIType.AI_Thief, FightMode.None, 10, 1, 0.4, 1.6 ) 
 
      { 
      	InitStats( 85, 75, 65 ); 
      	Name = NameList.RandomName( "female" );
      	Title = "- Handlarz Bestii";
        Blessed = true;
	Body = 0x191;
      	Hue = Utility.RandomSkinHue(); 
			
			AddItem( new Boots( Utility.RandomBirdHue() ) );
			AddItem( new ShepherdsCrook() );
			AddItem( new Cloak( Utility.RandomBirdHue() ) );
			AddItem( new FancyShirt( Utility.RandomBirdHue() ) );
			AddItem( new Kilt( Utility.RandomBirdHue() ) );
			AddItem( new BodySash( Utility.RandomBirdHue() ) );
			
	HairItemID = 0x203C;   // The ItemID of the hair you want
        HairHue = 1175; 
                
      }
    
      private class PetSaleTarget : Target 
      { 
         private AnimalTrainerLord m_Trainer; 

         public PetSaleTarget( AnimalTrainerLord trainer ) : base( 12, false, TargetFlags.None ) 
         { 
            m_Trainer = trainer; 
         } 

         protected override void OnTarget( Mobile from, object targeted ) 
         { 
            if ( targeted is BaseCreature ) 
               m_Trainer.EndPetSale( from, (BaseCreature)targeted ); 
            else if ( targeted == from ) 
               m_Trainer.SayTo( from, 502672 ); // HA HA HA! Sorry, I am not an inn. 
            
         } 
      } 

      public void BeginPetSale( Mobile from ) 
      { 
         if ( Deleted || !from.CheckAlive() ) 
            return; 

         SayTo( from, "Ktoro zwierze chcesz sprzedac?" ); 

         from.Target = new PetSaleTarget( this ); 
         
      } 

	//RUFO beginfunction
	private void SellPetForGold(Mobile from, BaseCreature pet, int goldamount)
	{
               		Item gold = new Gold(goldamount);
               		pet.ControlTarget = null; 
               		pet.ControlOrder = OrderType.None; 
               		pet.Internalize(); 
               		pet.SetControlMaster( null ); 
               		pet.SummonMaster = null;
               		pet.Delete();
               		
               		Container backpack = from.Backpack;
               		if ( backpack == null || !backpack.TryDropItem( from, gold, false ) ) 
            		{ 
            			gold.MoveToWorld( from.Location, from.Map );
            		}

	}
	//RUFO endfunction


      public void EndPetSale( Mobile from, BaseCreature pet ) 
      { 
         if ( Deleted || !from.CheckAlive() ) 
            return;
            

	if ( !pet.Controlled || pet.ControlMaster != from ) 
		SayTo( from, 1042562 ); // You do not own that pet! 
	else if ( pet.IsDeadPet ) 
		SayTo( from, 1049668 ); // Living pets only, please. 
	else if ( pet.Summoned ) 
		SayTo( from, 502673 ); // I can not PetSale summoned creatures. 
	else if ( pet.Body.IsHuman ) 
		SayTo( from, 502672 ); // HA HA HA! Sorry, I am not an inn. 
	else if ( (pet is PackLlama || pet is PackHorse || pet is Beetle) && (pet.Backpack != null && pet.Backpack.Items.Count > 0) ) 
		SayTo( from, 1042563 ); // You need to unload your pet. 
	else if ( pet.Combatant != null && pet.InRange( pet.Combatant, 12 ) && pet.Map == pet.Combatant.Map ) 
            SayTo( from, 1042564 ); // I'm sorry.  Your pet seems to be busy. 
	else 
	{ 
           	if ( pet is Chicken )
           	{
				SellPetForGold(from, pet, 25);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
           	}
		    else if (pet is PackHorse )
		        {
				SellPetForGold(from, pet, 303);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		        }
		    else if (pet is Rabbit )
		        {
				SellPetForGold(from, pet, 39);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		        }
            else if (pet is PackLlama )
              	{
				SellPetForGold(from, pet, 245);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is Dog )
               	{
				SellPetForGold(from, pet, 90);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
               	}
            else if (pet is Cat )
              	{
				SellPetForGold(from, pet, 69);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is Pig )
              	{
				SellPetForGold(from, pet, 50);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is Horse )
              	{
				SellPetForGold(from, pet, 25);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
		    else if (pet is ForestOstard )
		       	{
				SellPetForGold(from, pet, 301);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		       	}
            else if (pet is Cow )
              	{
				SellPetForGold(from, pet, 75);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
		    else if (pet is Bull )
		        {
				SellPetForGold(from, pet, 150);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		        }
		    else if (pet is Hind )
		        {
				SellPetForGold(from, pet, 75);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		        }
		    else if (pet is GreatHart )
		        {
				SellPetForGold(from, pet, 200);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		        }
           	else if (pet is Eagle )
              	{
				SellPetForGold(from, pet, 201);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is Sheep )
              	{
				SellPetForGold(from, pet, 75);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is Goat )
              	{
				SellPetForGold(from, pet, 75);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is BlackBear )
              	{
				SellPetForGold(from, pet, 317);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is Bird )
              	{
				SellPetForGold(from, pet, 25);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is TimberWolf )
              	{
				SellPetForGold(from, pet, 384);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is GreyWolf )
              	{
				SellPetForGold(from, pet, 384);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is DireWolf )
              	{
				SellPetForGold(from, pet, 384);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is BlackBear )
              	{
				SellPetForGold(from, pet, 210);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
		    else if (pet is Panther )
		        {
				SellPetForGold(from, pet, 635);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		        }
		    else if (pet is Cougar )
		        {
				SellPetForGold(from, pet, 635);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		        }
            else if (pet is BrownBear )
              	{
				SellPetForGold(from, pet, 427);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
		    else if (pet is GrizzlyBear )
		        {
				SellPetForGold(from, pet, 883);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		        }
		    else if (pet is Rat )
		       	{
				SellPetForGold(from, pet, 53);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
		       	}
            else if (pet is RidableLlama )
              	{
				SellPetForGold(from, pet, 101);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}
            else if (pet is Llama )
              	{
				SellPetForGold(from, pet, 150);
				this.Say( "Dziekuje ci {0}, zaopiekuje sie nim!",from.Name  );
              	}

           		else 
	            	this.Say( "{0}, Nie potrzebuje tego zwierzaka.",from.Name  );
            }
      }
            
      public override bool HandlesOnSpeech( Mobile from ) 
      { 
         return true; 
      } 

      public override void OnSpeech( SpeechEventArgs e ) 
      {
      	if( e.Mobile.InRange( this, 4 ))
      	{
      	if ( ( e.Speech.ToLower() == "sprzedac" ) )//was sellpet
	     {
      		BeginPetSale( e.Mobile );
         }
         else 
         { 
            base.OnSpeech( e ); 
         }
      	}
      
      } 

      public AnimalTrainerLord( Serial serial ) : base( serial ) 
      { 
      } 

      public override void Serialize( GenericWriter writer ) 
      { 
         base.Serialize( writer ); 

         writer.Write( (int) 0 ); // version 
      } 

      public override void Deserialize( GenericReader reader ) 
      { 
         base.Deserialize( reader ); 

         int version = reader.ReadInt(); 
      } 
   } 
} 



