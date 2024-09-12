using System;
using Server;
using Server.Items;
using System.Text;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Druid;


namespace Server.Commands
{
    public class CastDruidSpells
    {
        public static void Initialize()
        {
            CommandSystem.Prefix = ".";

            //CommandSystem.Register("SummonFirefly", AccessLevel.Player, new CommandEventHandler(SummonFirefly_OnCommand));

            CommandSystem.Register("HollowReed", AccessLevel.Player, new CommandEventHandler(HollowReed_OnCommand));

           // CommandSystem.Register("PackOfBeasts", AccessLevel.Player, new CommandEventHandler(PackOfBeasts_OnCommand));

            CommandSystem.Register("SpringOfLife", AccessLevel.Player, new CommandEventHandler(SpringOfLife_OnCommand));

            CommandSystem.Register("GraspingRoots", AccessLevel.Player, new CommandEventHandler(GraspingRoots_OnCommand));

            CommandSystem.Register("CircleOfThorns", AccessLevel.Player, new CommandEventHandler(CircleOfThorns_OnCommand));

            //CommandSystem.Register("SwarmOfInsects", AccessLevel.Player, new CommandEventHandler(SwarmOfInsects_OnCommand));

            //CommandSystem.Register("VolcanicEruption", AccessLevel.Player, new CommandEventHandler(VolcanicEruption_OnCommand));

            //CommandSystem.Register("SummonTreefellow", AccessLevel.Player, new CommandEventHandler(SummonTreefellow_OnCommand));

            CommandSystem.Register("EnchantedGrove", AccessLevel.Player, new CommandEventHandler(EnchantedGrove_OnCommand));

            CommandSystem.Register("LureStone", AccessLevel.Player, new CommandEventHandler(LureStone_OnCommand));

            CommandSystem.Register("Hurricane", AccessLevel.Player, new CommandEventHandler(Hurricane_OnCommand));

            CommandSystem.Register("MushroomGateway", AccessLevel.Player, new CommandEventHandler(MushroomGateway_OnCommand));

            CommandSystem.Register("RestorativeSoil", AccessLevel.Player, new CommandEventHandler(RestorativeSoil_OnCommand));

            CommandSystem.Register("ShieldOfEarth", AccessLevel.Player, new CommandEventHandler(ShieldOfEarth_OnCommand));

            CommandSystem.Register("ManaSpring", AccessLevel.Player, new CommandEventHandler(MushroomGateway_OnCommand));

            CommandSystem.Register("ForestKin", AccessLevel.Player, new CommandEventHandler(RestorativeSoil_OnCommand));

            CommandSystem.Register("BarkSkin", AccessLevel.Player, new CommandEventHandler(ShieldOfEarth_OnCommand));

            CommandSystem.Register("Hibernate", AccessLevel.Player, new CommandEventHandler(Hibernate_OnCommand));
        }

        public static void Register(string command, AccessLevel access, CommandEventHandler handler)
        {
            Server.Commands.CommandSystem.Register(command, access, handler);
        }

        public static bool HasSpell(Mobile from, int spellID)
        {
            Spellbook book = Spellbook.Find(from, spellID);

            return (book != null && book.HasSpell(spellID));
        }

        [Usage("ShieldOfEarth")]
        [Description("Casts ShieldOfEarth spell.")]
        public static void ShieldOfEarth_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 301))
            {
                new ShieldOfEarthSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }
        [Usage("HollowReed")]
        [Description("Casts HollowReed spell.")]
        public static void HollowReed_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 302))
            {
                new HollowReedSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        /*[Usage("PackOfBeasts")]
        [Description("Casts PackOfBeasts spell.")]
        public static void PackOfBeasts_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 303))
            {
                new PackOfBeastSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }*/

        [Usage("SpringOfLife")]
        [Description("Casts SpringOfLife spell.")]
        public static void SpringOfLife_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 303))
            {
                new SpringOfLifeSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("GraspingRoots")]
        [Description("Casts GraspingRoots spell.")]
        public static void GraspingRoots_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 304))
            {
                new GraspingRootsSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("CircleOfThorns")]
        [Description("Casts CircleOfThorns spell.")]
        public static void CircleOfThorns_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 305))
            {
                new CircleOfThornsSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        /*[Usage("SwarmOfInsects")]
        [Description("Casts SwarmOfInsects spell.")]
        public static void SwarmOfInsects_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 307))
            {
                new SwarmOfInsectsSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("VolcanicEruption")]
        [Description("Casts VolcanicEruption spell.")]
        public static void VolcanicEruption_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 308))
            {
                new VolcanicEruptionSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("SummonTreefellow")]
        [Description("Casts SummonTreefellow spell.")]
        public static void SummonTreefellow_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 309))
            {
                new TreefellowSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }
        [Usage("DeadlySpores")]
        [Description("Casts DeadlySpores spell.")]
        public static void DeadlySpores_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 310))
            {
                new DeadlySporesSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }*/
        [Usage("EnchantedGrove")]
        [Description("Casts EnchantedGrove spell.")]
        public static void EnchantedGrove_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 306))
            {
                new EnchantedGroveSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("LureStone")]
        [Description("Casts LureStone spell.")]
        public static void LureStone_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 307))
            {
                new LureStoneSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("Hurricane")]
        [Description("Casts Hurricane spell.")]
        public static void Hurricane_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 308))
            {
                new HurricaneSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("MushroomGateway")]
        [Description("Casts MushroomGateway spell.")]
        public static void MushroomGateway_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 309))
            {
                new MushroomGatewaySpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("RestorativeSoil")]
        [Description("Casts RestorativeSoil spell.")]
        public static void RestorativeSoil_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 310))
            {
                new RestorativeSoilSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        /*[Usage("SummonFirefly")]
        [Description("Casts SummonFirefly spell.")]
        public static void SummonFirefly_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 316))
            {

                new FireflySpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }*/
        [Usage("ForestKin")]
        [Description("Casts ForestKin spell.")]
        public static void ForestKin_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 311))
            {
                new ForestKinSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("BarkSkin")]
        [Description("Casts BarkSkin spell.")]
        public static void BarkSkin_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 312))
            {
                new BarkSkinSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }

        [Usage("ManaSpring")]
        [Description("Casts ManaSpring spell.")]
        public static void ManaSpring_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 313))
            {

                new ManaSpringSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }
        [Usage("Hibernate")]
        [Description("Casts Hibernate spell.")]
        public static void Hibernate_OnCommand(CommandEventArgs e)
        {
            Mobile from = e.Mobile;

            if (!Multis.DesignContext.Check(e.Mobile))
                return; // They are customizing

            if (HasSpell(from, 314))
            {

                new HibernateSpell(e.Mobile, null).Cast();
            }
            else
            {
                from.SendLocalizedMessage(500015); // You do not have that spell!
            }
        }
    }
}
