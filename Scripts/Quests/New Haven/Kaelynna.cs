using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TheMagesApprenticeQuest : BaseQuest
    { 
        public override bool DoneOnce
        {
            get
            {
                return true;
            }
        }
		
        /* The Mage's Apprentice */
        public override object Title
        {
            get
            {
                return 1077576;
            }
        }
		
        /* Head East out of town and go to Old Haven. Cast fireballs and lightning bolts against monsters there until you 
        have raised your Magia skill to 50. Greetings. You seek to unlock the secrets of the arcane art of Magia. The 
        New Haven Mage Council has an assignment for you. Undead have plagued the town of Old Haven. We need your assistance 
        in cleansing the town of this evil influence. Old Haven is located east of here. I suggest using your offensive 
        Magia spells such as Fireball and Lightning Bolt against the Undead that inhabit there. Make sure you have plenty 
        of reagents before embarking on your journey. Reagents are required to cast Magia spells. You can purchase extra 
        reagents at the nearby Reagent shop, or you can find reagents growing in the nearby wooded areas. You can see which 
        reagents are required for each spell by looking in your spellbook. Come back to me once you feel that you are worthy 
        of the rank of Apprentice Mage and I will reward you with an arcane prize. */ 
        public override object Description
        {
            get
            {
                return 1077577;
            }
        }
		
        /* Very well, come back to me when you are ready to practice Magia. You have so much arcane potential. 'Tis a shame 
        to see it go to waste. The New Haven Mage Council could really use your help. */
        public override object Refuse
        {
            get
            {
                return 1077578;
            }
        }
		
        /* You have not achieved the rank of Apprentice Mage. Come back to me once you feel that you are worthy of the rank 
        of Apprentice Mage and I will reward you with an arcane prize. */
        public override object Uncomplete
        {
            get
            {
                return 1077579;
            }
        }
		
        /* Well done! On behalf of the New Haven Mage Council I wish to present you with this staff. Normally a mage must 
        unequip weapons before spell casting. While wielding your new Ember Staff, however, you will be able to invoke your 
        Magia spells. Even if you do not currently possess skill in Mace Fighting, the Ember Staff will allow you to fight 
        as if you do. However, your Magia skill will be temporarily reduced while doing so. Finally, the Ember Staff 
        occasionally smites a foe with a Fireball while wielding it in melee combat. I hope the Ember Staff serves you well. */
        public override object Complete
        {
            get
            {
                return 1077581;
            }
        }
		
        public TheMagesApprenticeQuest()
            : base()
        { 
            this.AddObjective(new ApprenticeObjective(SkillName.Magia, 50, "Old Haven Training", 1077489, 1077583));
			
            // 1077489 Your Magia potential is greatly enhanced while questing in this area.
            // 1077583 You are not in the quest area for Apprentice Magia. Your Magia potential is not enhanced here.
		
            this.AddReward(new BaseReward(typeof(EmberStaff), 1077582));
        }
		
        public override bool CanOffer()
        {
            #region Scroll of Alacrity
            PlayerMobile pm = this.Owner as PlayerMobile;
            if (pm.AcceleratedStart > DateTime.UtcNow)
            {
                this.Owner.SendLocalizedMessage(1077951); // You are already under the effect of an accelerated skillgain scroll.
                return false;
            }
            #endregion
            else
                return this.Owner.Skills.Magia.Base < 50;
        }
		
        public override void OnCompleted()
        { 
            this.Owner.SendLocalizedMessage(1077580, null, 0x23); // You have achieved the rank of Apprentice Mage. Return to Kaelynna in New Haven to receive your arcane prize.
            this.Owner.PlaySound(this.CompleteSound);
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
			
            writer.Write((int)0); // version
        }
		
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
			
            int version = reader.ReadInt();
        }
    }
	
    public class Kaelynna : MondainQuester
    {
        public override Type[] Quests
        { 
            get
            {
                return new Type[] 
                {
                    typeof(TheMagesApprenticeQuest)
                };
            }
        }

        public override void InitSBInfo()
        {
            this.SBInfos.Add(new SBMage());
        }
		
        [Constructable]
        public Kaelynna()
            : base("Kaelynna", "The Magia Instructor")
        { 
            this.SetSkill(SkillName.Intelekt, 120.0, 120.0);
            this.SetSkill(SkillName.Inskrypcja, 120.0, 120.0);
            this.SetSkill(SkillName.Magia, 120.0, 120.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 120.0, 120.0);
            this.SetSkill(SkillName.Boks, 120.0, 120.0);
            this.SetSkill(SkillName.Medytacja, 120.0, 120.0);
        }
		
        public Kaelynna(Serial serial)
            : base(serial)
        {
        }
		
        public override void Advertise()
        {
            this.Say(1078125); // Want to unlock the secrets of magery?
        }
		
        public override void OnOfferFailed()
        { 
            this.Say(1077772); // I cannot teach you, for you know all I can teach!
        }
		
        public override void InitBody()
        { 
            this.Female = true;
            this.CantWalk = true;
            this.Race = Race.Human;		
		
            base.InitBody();
        }
		
        public override void InitOutfit()
        {
            this.AddItem(new Backpack());
            this.AddItem(new Robe(0x592));
            this.AddItem(new Sandals());
        }
		
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
			
            writer.Write((int)0); // version
        }
		
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
			
            int version = reader.ReadInt();
        }
    }
}