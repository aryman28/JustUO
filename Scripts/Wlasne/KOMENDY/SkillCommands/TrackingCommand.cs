using Server.Commands;
using System;
using Server;
using Server.Network;

namespace Server.Commands
{
    public class TrackingCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Tropienie", AccessLevel.Player, new CommandEventHandler(Tracking_OnCommand));
        }


        [Usage("Tropienie")]
        [Description("Tracks on Command.")]
        public static void Tracking_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            Server.Skills.UseSkill(e.Mobile ,SkillName.Tropienie);
        }
    }
}