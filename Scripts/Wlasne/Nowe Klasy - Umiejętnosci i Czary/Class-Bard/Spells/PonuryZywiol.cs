using System;
using Server.Targeting;
using Server.Network;
using Server.Mobiles;
using Server.Items;

namespace Server.Spells.Song
{
	public class PonuryZywiol : Song
	{
		
		private static SpellInfo m_Info = new SpellInfo(
				"Pieśń o ponurych żywiołach", "canticum truci de elementis",
				//SpellCircle.First,
				//212,9041
				-1
			);
		
		public PonuryZywiol( Mobile caster, Item scroll) : base( caster, scroll, m_Info )
		{
			
		}

		private SongBook m_Book;
		//public override double CastDelay{ get{ return 2; } }
		public override TimeSpan CastDelayBase { get { return TimeSpan.FromSeconds( 5 ); } }
		public override double RequiredSkill{ get{ return 50.0; } }
		public override int RequiredMana{ get{ return 12; } }

		public override void OnCast()
		{
			Caster.Target = new InternalTarget( this );
		}

		public void Target( Mobile m )
		{
			PlayerMobile p = m as PlayerMobile;
			
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
				SpellHelper.Turn( source, m );

				//SpellHelper.CheckReflect( (int)this.Circle, ref source, ref m );
				
				//int amount = (int)( Caster.Skills[SkillName.Musicianship].Base * 0.17 );
                int amount = (int)((Caster.Skills[SkillName.Muzykowanie].Value + Caster.Skills[SkillName.Uspokajanie].Value + Caster.Skills[SkillName.Prowokacja].Value + Caster.Skills[SkillName.Manipulacja].Value) / 4 * .167);
				TimeSpan duration = TimeSpan.FromSeconds( Caster.Skills[SkillName.Muzykowanie].Base * 0.3 ); 
				
				m.SendMessage( "Twoje odporności maleją." );
				ResistanceMod mod1 = new ResistanceMod( ResistanceType.Fire, - amount );
				ResistanceMod mod2 = new ResistanceMod( ResistanceType.Cold, - amount );
				ResistanceMod mod3 = new ResistanceMod( ResistanceType.Poison, - amount );
				ResistanceMod mod4 = new ResistanceMod( ResistanceType.Energy, - amount );
				
				m.FixedParticles( 0x374A, 10, 30, 5013, 0x489, 2, EffectLayer.Waist );
				
				m.AddResistanceMod( mod1 );
				m.AddResistanceMod( mod2 );
				m.AddResistanceMod( mod3 );
				m.AddResistanceMod( mod4 );

				ExpireTimer timer1 = new ExpireTimer( m, mod1, duration );
				timer1.Start();
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
				PlayerMobile p = m_Mobile as PlayerMobile;
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

		private class InternalTarget : Target
		{
			private PonuryZywiol m_Owner;

			public InternalTarget( PonuryZywiol owner ) : base( 12, false, TargetFlags.Harmful )
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
