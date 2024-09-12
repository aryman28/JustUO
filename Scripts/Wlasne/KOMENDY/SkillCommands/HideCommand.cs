using Server.Commands;
using System;
using Server;
using Server.Network;

namespace Server.Commands
{
    public class HideCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("Hide", AccessLevel.Player, new CommandEventHandler(Hide_OnCommand));
        }


        [Usage("Hide")]
        [Description("Hides on Command.")]
        public static void Hide_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            Server.Skills.UseSkill(e.Mobile ,SkillName.Ukrywanie);
        }
    }
}