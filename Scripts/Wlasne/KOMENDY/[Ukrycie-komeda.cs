using System; 
using System.Collections; 
using Server; 
using Server.Mobiles; 
using Server.Network; 
using Server.Prompts;
using Server.Items;
using Server.Guilds;
using Server.Gumps;
using Server.Targeting;
using Server.Commands;

namespace Server.Items
{
	public class RepSystem3
	{ 

		public static void Initialize()
		{
			CommandSystem.Register( "u", AccessLevel.Player, new CommandEventHandler( Ukrycie_OnCommand ) );    
		} 

		public static void Ukrycie_OnCommand( CommandEventArgs args )
		{ 
			Mobile m = args.Mobile; 
			PlayerMobile from = m as PlayerMobile; 
          
			if( from != null ) 
			{  
				from.SendMessage ( "mozesz ukryc lub odkryc wlasciwosci przedmiotu." );
				m.Target = new InternalTarget();
			} 
		} 

		private class InternalTarget : Target
		{
			public InternalTarget() : base( 18, false, TargetFlags.None )
			{
			}

		protected override void OnTarget( Mobile from, object target )
		{

      Item BaseArmor = from.Backpack.FindItemByType( typeof( BaseArmor ) );

         if ( target is BaseArmor ) 
         { 
		BaseArmor bas = (BaseArmor)target;

		if( bas.Ukrycie == true )
			bas.Ukrycie = false;
		else
			bas.Ukrycie = true;
	  }
         //else if ( target is BaseJewelery ) 
         //{ 
 
         //} 
		  }


		}
	} 
} 
