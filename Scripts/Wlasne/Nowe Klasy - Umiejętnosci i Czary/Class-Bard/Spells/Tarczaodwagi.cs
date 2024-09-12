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
	public class TarczaOdwagi : Song
	{
		
		private static SpellInfo m_Info = new SpellInfo(
				"Tarcza odwagi", "Resistus",
				//SpellCircle.First,
				//212,9041
				-1
			);

		private SongBook m_Book;
		//public override double CastDelay{ get{ return 3; } }
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 5 ); } }
		public override double RequiredSkill{ get{ return 45.0; } }
		public override int RequiredMana{ get{ return 12; } }
		
		public TarczaOdwagi( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
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
                Caster.SendMessage("The instrument you are trying to play is unaccessible. You can select the instrument through your Song Book");
                return;
            }
            Caster.PlaySound(m_Book.Instrument.SuccessSound);

			if( CheckSequence() )
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
					//int amount = (int)( Caster.Skills[SkillName.Musicianship].Value * .18 );
                    int amount = (int)((Caster.Skills[SkillName.Muzykowanie].Value + Caster.Skills[SkillName.Uspokajanie].Value + Caster.Skills[SkillName.Prowokacja].Value + Caster.Skills[SkillName.Manipulacja].Value) / 4 * .125);
	
					m.SendMessage( "Twoja odporność na ataki fizyczne wzrasta." );
					ResistanceMod mod1 = new ResistanceMod( ResistanceType.Physical, + amount );
						
					m.AddResistanceMod( mod1 );
						
					m.FixedParticles( 0x373A, 10, 15, 5012, 0x450, 3, EffectLayer.Waist );
						
					new ExpireTimer( m, mod1, duration ).Start();
					
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
				m_Mobile.RemoveResistanceMod( m_Mods );
				
				Stop();
			}

			protected override void OnTick()
			{
				if ( m_Mobile != null )
				{
					m_Mobile.SendMessage( "Wracasz do normalnego stanu." );
					DoExpire();
				}
			}
		}
	}
}
