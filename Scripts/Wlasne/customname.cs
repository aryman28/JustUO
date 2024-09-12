/**************************************
*Script Name: Custom Name Giver       *
*Author: Matheus Primo AKA "Primo"    *
*For use with ServUO 54 and RunUO 2.0 *
*Client Tested with: 7.0.10.3         *
*Version: 1.0                         *
*Initial Release: 13/07/15            *
*Revision Date: --/--/--              *
**************************************/
using System;
using System.Collections;
using System.IO;
using System.Text;
using Server;
using Server.Mobiles;
using Server.Multis;
using Server.Network;
using Server.Commands;
using Server.Targeting;

namespace CustomName
{
    class CallCustomName
    {
        public static void Initialize()
        {
            CommandSystem.Register("r", AccessLevel.Player, new CommandEventHandler(CustomName_OnCommand));
        }
        [Usage("r")]
        [Description("Gives the custom name to a player")]
        public static void CustomName_OnCommand(CommandEventArgs args)
        {
            Mobile m = args.Mobile;
            m.SendMessage("Use the modal [r Name to choose a name");
            m.SendMessage("Choose who you wish to nominate:");
            string[] input = args.Arguments;
            m.Target = new InternalTarget(input);
            

        }

        private class InternalTarget : Target
        {
            private string[] m_Arguments;
            string m_Name = "";
            public InternalTarget(string[] args)
                : base(8, false, TargetFlags.None)
            {
                m_Arguments = args;
                m_Name = string.Join(" ", m_Arguments);
                
                
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                Mobile target;

                if (from is PlayerMobile && targeted is Mobile)
                {
                    if (targeted is PlayerMobile && ((PlayerMobile)targeted).Player)
                    {
                        
                        ((PlayerMobile)targeted).NameMod = m_Name;
                        return;
                    }


                }
            }
        }
    }
}