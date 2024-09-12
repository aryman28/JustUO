using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells.Song
{
	public class OgnistaZemsta : Song
	{
		
		private static SpellInfo m_Info = new SpellInfo(
				"Pieśń o zemście ognia", "canticum ignem vindictae",
				//SpellCircle.Sixth,
				//212,9041
				-1
			);
		
		public OgnistaZemsta( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
		{
			
		}
		
		private SongBook m_Book;
		//public override double CastDelay{ get{ return 3; } }
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 2 ); } }
		public override double RequiredSkill{ get{ return 55.0; } }
		public override int RequiredMana{ get{ return 18; } }
		
		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			if ( !Caster.CanSee( m ) )
			{
				Caster.SendLocalizedMessage( 500237 ); // Target can not be seen.
			}
			else if ( CheckHSequence( m ) )
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

				Mobile source = Caster;
				
				SpellHelper.Turn( Caster, m );

				//double damage = ( Utility.Random( 28, 23 ) + Utility.Random( 5, 1 ) );
                double damage = ((Caster.Skills[SkillName.Muzykowanie].Value + Caster.Skills[SkillName.Uspokajanie].Value + Caster.Skills[SkillName.Prowokacja].Value + Caster.Skills[SkillName.Manipulacja].Value)/4 *0.25);
				//if ( CheckResisted( m ) )
				//{
				//	m.SendLocalizedMessage( 501783 ); // You feel yourself resisting magical energy.
				//	damage *= .4;
				//}
				
				//sound damage, all resistances
				SpellHelper.Damage( this, m, damage, 20, 20, 20, 20, 20 );

				m.FixedParticles( 0x374A, 10, 15, 5028, EffectLayer.Head );
                                source.MovingParticles( m, 0x379F, 7, 0, false, true, 3043, 4043, 0x211 );
				m.PlaySound( 0x1EA );
			}
			
			FinishSequence();
		}
		
		private class InternalTarget : Target
		{
			private OgnistaZemsta m_Owner;

			public InternalTarget( OgnistaZemsta owner ) : base( 12, false, TargetFlags.Harmful )
			{
				m_Owner = owner;
			}

			protected override void OnTarget( Mobile from, object o )
			{
				if ( o is Mobile )
					m_Owner.Target( (Mobile)o );
			}

			protected override void OnTargetFinish( Mobile from )
			{
				m_Owner.FinishSequence();
			}
		}
	}
}
