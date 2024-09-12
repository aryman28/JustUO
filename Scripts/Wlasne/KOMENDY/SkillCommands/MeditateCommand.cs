using Server.Commands;
using System;
using Server;
using Server.Network;

namespace Server.Commands
{
    public class MeditateCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Meditate", AccessLevel.Player, new CommandEventHandler(Meditate_OnCommand));
        }


        [Usage("Meditate")]
        [Description("Meditates on Command.")]
        public static void Meditate_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            Server.Skills.UseSkill(e.Mobile ,SkillName.Medytacja);
        }
    }
}