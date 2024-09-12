using Server.Commands;
using System;
using Server;
using Server.Network;

namespace Server.Commands
{
    public class SpiritSpeakCommand
    {
        public static void Initialize()
        {
            CommandSystem.Register("MowaDuchow", AccessLevel.Player, new CommandEventHandler(SpiritSpeak_OnCommand));
        }


        [Usage("MowaDuchow")]
        [Description("Spirit Speaks on Command.")]
        public static void SpiritSpeak_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;
            Server.Skills.UseSkill(e.Mobile ,SkillName.MowaDuchow);
        }
    }
}