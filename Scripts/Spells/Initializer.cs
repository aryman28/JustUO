using System;

namespace Server.Spells
{
    public class Initializer
    {
        public static void Initialize()
        {
            // First circle
            Register(00, typeof(First.ClumsySpell));
            Register(01, typeof(First.CreateFoodSpell));
            Register(02, typeof(First.FeeblemindSpell));
            Register(03, typeof(First.HealSpell));
            Register(04, typeof(First.MagicArrowSpell));
            Register(05, typeof(First.NightSightSpell));
            Register(06, typeof(First.ReactiveArmorSpell));
            Register(07, typeof(First.WeakenSpell));

            // Second circle
            Register(08, typeof(Second.AgilitySpell));
            Register(09, typeof(Second.CunningSpell));
            Register(10, typeof(Second.CureSpell));
            Register(11, typeof(Second.HarmSpell));
            Register(12, typeof(Second.MagicTrapSpell));
            Register(13, typeof(Second.RemoveTrapSpell));
            Register(14, typeof(Second.ProtectionSpell));
            Register(15, typeof(Second.StrengthSpell));

            // Third circle
            Register(16, typeof(Third.BlessSpell));
            Register(17, typeof(Third.FireballSpell));
            Register(18, typeof(Third.MagicLockSpell));
            Register(19, typeof(Third.PoisonSpell));
            Register(20, typeof(Third.TelekinesisSpell));
            Register(21, typeof(Third.TeleportSpell));
            Register(22, typeof(Third.UnlockSpell));
            Register(23, typeof(Third.WallOfStoneSpell));

            // Fourth circle
            Register(24, typeof(Fourth.ArchCureSpell));
            Register(25, typeof(Fourth.ArchProtectionSpell));
            Register(26, typeof(Fourth.CurseSpell));
            Register(27, typeof(Fourth.FireFieldSpell));
            Register(28, typeof(Fourth.GreaterHealSpell));
            Register(29, typeof(Fourth.LightningSpell));
            Register(30, typeof(Fourth.ManaDrainSpell));
            Register(31, typeof(Fourth.RecallSpell));

            // Fifth circle
            Register(32, typeof(Fifth.BladeSpiritsSpell));
            Register(33, typeof(Fifth.DispelFieldSpell));
            Register(34, typeof(Fifth.IncognitoSpell));
            Register(35, typeof(Fifth.MagicReflectSpell));
            Register(36, typeof(Fifth.MindBlastSpell));
            Register(37, typeof(Fifth.ParalyzeSpell));
            Register(38, typeof(Fifth.PoisonFieldSpell));
            Register(39, typeof(Fifth.SummonCreatureSpell));

            // Sixth circle
            Register(40, typeof(Sixth.DispelSpell));
            Register(41, typeof(Sixth.EnergyBoltSpell));
            Register(42, typeof(Sixth.ExplosionSpell));
            Register(43, typeof(Sixth.InvisibilitySpell));
            Register(44, typeof(Sixth.MarkSpell));
            Register(45, typeof(Sixth.MassCurseSpell));
            Register(46, typeof(Sixth.ParalyzeFieldSpell));
            Register(47, typeof(Sixth.RevealSpell));

            // Seventh circle
            Register(48, typeof(Seventh.ChainLightningSpell));
            Register(49, typeof(Seventh.EnergyFieldSpell));
            Register(50, typeof(Seventh.FlameStrikeSpell));
            Register(51, typeof(Seventh.GateTravelSpell));
            Register(52, typeof(Seventh.ManaVampireSpell));
            Register(53, typeof(Seventh.MassDispelSpell));
            Register(54, typeof(Seventh.MeteorSwarmSpell));
            Register(55, typeof(Seventh.PolymorphSpell));

            // Eighth circle
            Register(56, typeof(Eighth.EarthquakeSpell));
            Register(57, typeof(Eighth.EnergyVortexSpell));
            Register(58, typeof(Eighth.ResurrectionSpell));
            Register(59, typeof(Eighth.AirElementalSpell));
            Register(60, typeof(Eighth.SummonDaemonSpell));
            Register(61, typeof(Eighth.EarthElementalSpell));
            Register(62, typeof(Eighth.FireElementalSpell));
            Register(63, typeof(Eighth.WaterElementalSpell));

            //Bard

                Register(351, typeof(Song.BalladaMaga));
                Register(352, typeof(Song.BohaterskaEtiuda));
                Register(353, typeof(Song.MagicznyFinal));
                Register(354, typeof(Song.OgnistaZemsta));
                Register(355, typeof(Song.PiesnZywiolow));
                Register(356, typeof(Song.PonuryZywiol));
                Register(357, typeof(Song.RzekaZycia));
                Register(358, typeof(Song.TarczaOdwagi));

            //Berserker

            //Register(251, typeof(Berserker.SzalBitewny));
        

            //Msciciel

                /*Register(251, typeof (Msciciel.DuszenieSpell));
                Register(252, typeof (Msciciel.FuriaSpell));
                Register(253, typeof (Msciciel.LeczenieRanSpell));
                Register(254, typeof (Msciciel.MrocznyKielSpell));
                Register(255, typeof (Msciciel.PoswieconaBronSpell));
                Register(256, typeof (Msciciel.PrzekletaBronSpell));
                Register(257, typeof (Msciciel.PrzywolanieUmarlychSpell));
                Register(258, typeof (Msciciel.ZemstaSpell));*/

                

            if (Core.AOS)
            {
                // Nekromancja spells
                Register(100, typeof(Nekromancja.AnimateDeadSpell));
                Register(101, typeof(Nekromancja.BloodOathSpell));
                Register(102, typeof(Nekromancja.CorpseSkinSpell));
                Register(103, typeof(Nekromancja.CurseWeaponSpell));
                Register(104, typeof(Nekromancja.EvilOmenSpell));
                Register(105, typeof(Nekromancja.HorrificBeastSpell));
                Register(106, typeof(Nekromancja.LichFormSpell));
                Register(107, typeof(Nekromancja.MindRotSpell));
                Register(108, typeof(Nekromancja.PainSpikeSpell));
                Register(109, typeof(Nekromancja.PoisonStrikeSpell));
                Register(110, typeof(Nekromancja.StrangleSpell));
                Register(111, typeof(Nekromancja.SummonFamiliarSpell));
                Register(112, typeof(Nekromancja.VampiricEmbraceSpell));
                Register(113, typeof(Nekromancja.VengefulSpiritSpell));
                Register(114, typeof(Nekromancja.WitherSpell));
                Register(115, typeof(Nekromancja.WraithFormSpell));

                if (Core.SE)
                    Register(116, typeof(Nekromancja.ExorcismSpell));

                // Paladin abilities
                Register(200, typeof(Rycerstwo.CleanseByFireSpell));
                Register(201, typeof(Rycerstwo.CloseWoundsSpell));
                Register(202, typeof(Rycerstwo.ConsecrateWeaponSpell));
                Register(203, typeof(Rycerstwo.DispelEvilSpell));
                Register(204, typeof(Rycerstwo.DivineFurySpell));
                Register(205, typeof(Rycerstwo.EnemyOfOneSpell));
                Register(206, typeof(Rycerstwo.HolyLightSpell));
                Register(207, typeof(Rycerstwo.NobleSacrificeSpell));
                Register(208, typeof(Rycerstwo.RemoveCurseSpell));
                Register(209, typeof(Rycerstwo.SacredJourneySpell));

                // Druid Spells
                Register(301, typeof(Druid.ShieldOfEarthSpell));
                Register(302, typeof(Druid.HollowReedSpell));
                //Register(303, typeof(Druid.PackOfBeastSpell));
                Register(303, typeof(Druid.SpringOfLifeSpell));
                Register(304, typeof(Druid.GraspingRootsSpell));
                Register(305, typeof(Druid.CircleOfThornsSpell));
                //Register(307, typeof(Druid.SwarmOfInsectsSpell));
                //Register(308, typeof(Druid.VolcanicEruptionSpell));
                //Register(309, typeof(Druid.TreefellowSpell));
                //Register(310, typeof(Druid.DeadlySporesSpell));
                Register(306, typeof(Druid.EnchantedGroveSpell));
                Register(307, typeof(Druid.LureStoneSpell));
                Register(308, typeof(Druid.HurricaneSpell));
                Register(309, typeof(Druid.MushroomGatewaySpell));
                Register(310, typeof(Druid.RestorativeSoilSpell));
                //Register(316, typeof(Druid.FireflySpell));
                Register(311, typeof(Druid.ForestKinSpell));
                Register(312, typeof(Druid.BarkSkinSpell));
                Register(313, typeof(Druid.ManaSpringSpell));
                Register(314, typeof(Druid.HibernateSpell));

                         

                if (Core.SE)
                {
                    // Samurai abilities
                    Register(400, typeof(Fanatyzm.HonorableExecution));
                    Register(401, typeof(Fanatyzm.Confidence));
                    Register(402, typeof(Fanatyzm.Evasion));
                    Register(403, typeof(Fanatyzm.CounterAttack));
                    Register(404, typeof(Fanatyzm.LightningStrike));
                    Register(405, typeof(Fanatyzm.MomentumStrike));

                    // Ninja abilities
                    Register(500, typeof(Skrytobojstwo.FocusAttack));
                    Register(501, typeof(Skrytobojstwo.DeathStrike));
                    Register(502, typeof(Skrytobojstwo.AnimalForm));
                    Register(503, typeof(Skrytobojstwo.KiAttack));
                    Register(504, typeof(Skrytobojstwo.SurpriseAttack));
                    Register(505, typeof(Skrytobojstwo.Backstab));
                    Register(506, typeof(Skrytobojstwo.Shadowjump));
                    Register(507, typeof(Skrytobojstwo.MirrorImage));
                }

                if (Core.ML)
                {
                    Register(600, typeof(Druidyzm.ArcaneCircleSpell));
                    Register(601, typeof(Druidyzm.GiftOfRenewalSpell));
                    Register(602, typeof(Druidyzm.ImmolatingWeaponSpell));
                    Register(603, typeof(Druidyzm.AttuneWeaponSpell));
                    Register(604, typeof(Druidyzm.ThunderstormSpell));
                    Register(605, typeof(Druidyzm.NatureFurySpell));
                    Register(606, typeof(Druidyzm.SummonFeySpell));
                    Register(607, typeof(Druidyzm.SummonFiendSpell));
                    Register(608, typeof(Druidyzm.ReaperFormSpell));
                    Register( 609, typeof( Druidyzm.WildfireSpell ) );
                    Register(610, typeof(Druidyzm.EssenceOfWindSpell));
                    Register( 611, typeof( Druidyzm.DryadAllureSpell ) );
                    Register(612, typeof(Druidyzm.EtherealVoyageSpell));
                    Register(613, typeof(Druidyzm.WordOfDeathSpell));
                    Register(614, typeof(Druidyzm.GiftOfLifeSpell));
                    Register( 615, typeof( Druidyzm.ArcaneEmpowermentSpell ) );

                    
                }
                #region Stygian Abyss
                if (Core.SA)
                {
                    Register(677, typeof(Mystic.NetherBoltSpell));
                    Register(678, typeof(Mystic.HealingStoneSpell));
                    Register(679, typeof(Mystic.PurgeMagicSpell));
                    Register(680, typeof(Mystic.EnchantSpell));
                    Register(681, typeof(Mystic.SleepSpell));
                    Register(682, typeof(Mystic.EagleStrikeSpell));
                    Register(683, typeof(Mystic.AnimatedWeaponSpell));
                    Register(684, typeof(Mystic.StoneFormSpell));
                    Register(685, typeof(Mystic.SpellTriggerSpell));
                    Register(686, typeof(Mystic.MassSleepSpell));
                    Register(687, typeof(Mystic.CleansingWindsSpell));
                    Register(688, typeof(Mystic.BombardSpell));
                    Register(689, typeof(Mystic.SpellPlagueSpell));
                    Register(690, typeof(Mystic.HailStormSpell));
                    Register(691, typeof(Mystic.NetherCycloneSpell));
                    Register(692, typeof(Mystic.RisingColossusSpell));
                }
                #endregion
 
            }
        }

        public static void Register(int spellID, Type type)
        {
            SpellRegistry.Register(spellID, type);
        }
    }
}