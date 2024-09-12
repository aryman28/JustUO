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
	public class BohaterskaEtiuda : Song
	{
		
		private static SpellInfo m_Info = new SpellInfo(
				"BohaterskaEtiuda", "Fortitus",
				//SpellCircle.First,
				//212,9041
				-1
			);

		private SongBook m_Book;
		//public override double CastDelay{ get{ return 3; } }
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2 ); } }
		public override double RequiredSkill{ get{ return 70.0; } }
		public override int RequiredMana{ get{ return 12; } }
		
		public BohaterskaEtiuda( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
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
					
					Mobile source = Caster;
					double allvalue = Caster.Skills[SkillName.Muzykowanie].Value + Caster.Skills[SkillName.Prowokacja].Value + Caster.Skills[SkillName.Manipulacja].Value + Caster.Skills[SkillName.Uspokajanie].Value;

					
                    //int amount = (int)(Math.Max((Caster.Skills[SkillName.Muzykowanie].Base * 0.1),(allvalue-360*0.15)));
                    int amount = (int)( Caster.Skills[SkillName.Muzykowanie].Base * 0.25 );
					string str = "str";
					string dex = "dex";
					string intt = "int";
						
					double duration = ( Caster.Skills[SkillName.Muzykowanie].Base); 
						
					StatMod mod1 = new StatMod( StatType.Str, str, + amount, TimeSpan.FromSeconds( duration ) );
					StatMod mod2 = new StatMod( StatType.Dex, dex, + amount, TimeSpan.FromSeconds( duration ) );
					StatMod mod3 = new StatMod( StatType.Int, intt, + amount, TimeSpan.FromSeconds( duration ) );

					m.AddStatMod( mod1 );
					m.AddStatMod( mod2 );
					m.AddStatMod( mod3 );
						
					m.FixedParticles( 0x375A, 10, 15, 5017, 0x224, 3, EffectLayer.Waist );
					
				}
			}
			
			FinishSequence();
		}
	}
}
