using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Targeting;
using Server.Gumps;
using Server.Spells;

namespace Server.Spells.Song
{
	public class PiesnZywiolow : Song
	{
		
		private static SpellInfo m_Info = new SpellInfo(
				"Pieśń Żywiolów", "Elementa Natus",
				//SpellCircle.First,
				266,
				9040,
				false
			);

		private SongBook m_Book;
		//public override double CastDelay{ get{ return TimeSpan.FromSeconds( 60 ); } }
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 10 ); } }
		public override double RequiredSkill{ get{ return 30.0; } }
		public override int RequiredMana{ get{ return 12; } }
		
		public PiesnZywiolow( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
		{
			
		}

        public override void OnCast()
        {
            //get songbook instrument
            Spellbook book = Spellbook.Find(Caster, -1, SpellbookType.Song);
            if (book == null)
            {
                return;
            }
            m_Book = (SongBook)book;
            if (m_Book.Instrument == null || !(Caster.InRange(m_Book.Instrument.GetWorldLocation(), 1)))
            {
                Caster.SendMessage("Instrument na którym chcesz zagrać jest niedostępnyessible. Możesz go wskazać poprzez śpiewnik.");
                return;
            }
            Caster.PlaySound(m_Book.Instrument.SuccessSound);

                //added this to test
                if (m_Book.Instrument == null || !(Caster.InRange(m_Book.Instrument.GetWorldLocation(), 1)))
                {
                    Caster.SendMessage("Potrzebujesz instrumentu aby odegrac ten utwór!");
                }
                else if (CheckSequence())
                    //added end
			// if( CheckSequence() )
			{
				ArrayList targets = new ArrayList();

				foreach ( Mobile m in Caster.GetMobilesInRange( 3 ) )
				{
					if ( Caster.CanBeBeneficial( m, false, true ) && !(m is Golem) )
						targets.Add( m );
				}
				
				
				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile)targets[i];
					
					TimeSpan duration = TimeSpan.FromSeconds( Caster.Skills[SkillName.Muzykowanie].Value * 2.5 ); 
					//int amount = (int)( Caster.Skills[SkillName.Musicianship].Value * .125 );
                    int amount = (int)((Caster.Skills[SkillName.Muzykowanie].Value + Caster.Skills[SkillName.Uspokajanie].Value + Caster.Skills[SkillName.Prowokacja].Value + Caster.Skills[SkillName.Manipulacja].Value) / 4 * .125);	
					m.SendMessage( "Twoja Odporność wzrasta." );
					ResistanceMod mod1 = new ResistanceMod( ResistanceType.Fire, + amount );
					ResistanceMod mod2 = new ResistanceMod( ResistanceType.Cold, + amount );
					ResistanceMod mod3 = new ResistanceMod( ResistanceType.Energy, + amount );
					ResistanceMod mod4 = new ResistanceMod( ResistanceType.Poison, + amount );
						
					m.AddResistanceMod( mod1 );
					m.AddResistanceMod( mod2 );
					m.AddResistanceMod( mod3 );
					m.AddResistanceMod( mod4 );
						
					m.FixedParticles( 0x373A, 10, 15, 5012, 0x21, 3, EffectLayer.Waist );


					new ExpireTimer( m, mod1, duration ).Start();
					new ExpireTimer( m, mod2, duration ).Start();
					new ExpireTimer( m, mod3, duration ).Start();
					new ExpireTimer( m, mod4, duration ).Start();
					
				}
			}
			
			FinishSequence();
		}

		private class ExpireTimer : Timer
		{
			private Mobile m_Mobile;
			private ResistanceMod m_Mods;

			public ExpireTimer( Mobile m, ResistanceMod mod, TimeSpan delay ) : base( delay )
			{
				m_Mobile = m;
				m_Mods = mod;
			}

			public void DoExpire()
			{
				PlayerMobile dpm = m_Mobile as PlayerMobile;
				m_Mobile.RemoveResistanceMod( m_Mods );
				
							
				Stop();
			}

			protected override void OnTick()
			{
				if ( m_Mobile != null )
				{
					m_Mobile.SendMessage( "Wracasz do normalnego stanu. Pieśń przestała działać." );
					m_Mobile.FixedParticles(0x3709, 1, 30, 9934, 0, 7, EffectLayer.Waist);
					DoExpire();
				}
			}
		}
	}
}
