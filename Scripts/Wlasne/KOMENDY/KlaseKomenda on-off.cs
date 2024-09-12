using Server.Commands;
using System;
using Server;
using Server.Network;
using Server.Mobiles;

namespace Server.Commands
{
    public class DisplayCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Klasa", AccessLevel.Player, new CommandEventHandler(Display_OnCommand));
        }


        [Usage("Klasa")]
        [Description("Poka¿ lub ukryj podpis klasy.")]
        public static void Display_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

			if (from == null)
				return;

            PlayerMobile pm = (PlayerMobile)from;

           if ( pm.DisplayKlasaTitle == false )
	   {
	   pm.DisplayKlasaTitle = true; 
           }
           else if ( pm.DisplayKlasaTitle == true )
	   {
	   pm.DisplayKlasaTitle = false; 
           }
        }
    }
}