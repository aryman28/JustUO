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
	public class MagicznyFinal : Song
	{
		
		private static SpellInfo m_Info = new SpellInfo(
				"Magiczne odesłanie", "Dispersus",
				//SpellCircle.First,
				//212,9041
				-1
			);

		private SongBook m_Book;
		//public override double CastDelay{ get{ return 3; } }
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 5 ); } }
		public override double RequiredSkill{ get{ return 80.0; } }
		public override int RequiredMana{ get{ return 15; } }
		
		public MagicznyFinal( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
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

				foreach ( Mobile m in Caster.GetMobilesInRange( 4 ) )
				{
					if ( m is BaseCreature && ((BaseCreature)m).Summoned )
						targets.Add( m );
				}
				
				Caster.FixedParticles( 0x3709, 1, 30, 9965, 5, 7, EffectLayer.Waist );
				
				for ( int i = 0; i < targets.Count; ++i )
				{
					Mobile m = (Mobile)targets[i];
					
					Effects.SendLocationParticles( EffectItem.Create( m.Location, m.Map, EffectItem.DefaultDuration ), 0x3728, 8, 20, 5042 );

					m.Delete();
				}
			}
			
			FinishSequence();
		}
	}
}

