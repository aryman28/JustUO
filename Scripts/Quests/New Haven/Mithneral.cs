using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ScholarlyTaskQuest : BaseQuest
    { 
        public override bool DoneOnce
        {
            get
            {
                return true;
            }
        }
		
        /* A Scholarly Task */
        public override object Title
        {
            get
            {
                return 1077603;
            }
        }
		
        /* Head East out of town and go to Old Haven.Use Evaluating Intelligence on all creatures you see there. You can also 
        cast Magia spells as well to raise Evaluating Intelligence. Do these activities until you have raised your Evaluating 
        Intelligence skill to 50. Hello. Truly knowing your opponent is essential for landing you offnesive spells with precision. 
        I can teach you how to enhance the effectiveness of you offensive spells, but first you must learn how to size up your 
        opponents intellectually. I have a scholarly task for you. Head East out of town and go to Old Haven.Use Evaluating 
        Intelligence on all creatures you see there. You can also cast Magia spells as well to raise Evaluating Intelligence.
        Come back to me once you feel that you are worthy of the rank Apprentice Scholar and i will reward you with a arcane prize. */ 
        public override object Description
        {
            get
            {
                return 1077604;
            }
        }
		
        /* Return to me if you reconsider and wish to become an Apprentice Scholar. */
        public override object Refuse
        {
            get
            {
                return 1077605;
            }
        }
		
        /* You have not achived the rank of Apprentice Scholar. Come back to me once you feel that you are worthy of the rank 
        Apprentice Scholar and i will reward you with a arcane prize. */		
        public override object Uncomplete
        {
            get
            {
                return 1077629;
            }
        }
		
        /* You have completed the task. Well Done. On behalf of the New Haven Mage Council i wish to present you with this ring. 
        When worn the Ring of the Savant enhances your inellectual aptitude and increases your mana pool. Your spell castng 
        abilities will take less time to invoke and recovering from such spell casting will be bastened. I hope the Ring of the 
        Savant serves you well. */
        public override object Complete
        {
            get
            {
                return 1077607;
            }
        }
		
        public ScholarlyTaskQuest()
            : base()
        { 
            this.AddObjective(new ApprenticeObjective(SkillName.Intelekt, 50, "Old Haven Training", 1077491, 1077585));
			
            // 1077491 Your Evaluating Intelligence potential is greatly enhanced while questing in this area.
            // 1077585 You are not in the quest area for Apprentice Scholar. Your Evaluating Intelligence potential is not enhanced here.
			
            this.AddReward(new BaseReward(typeof(RingOfTheSavant), 1077608)); 
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
                return this.Owner.Skills.Intelekt.Base < 50;
        }
		
        public override void OnCompleted()
        { 
            this.Owner.SendLocalizedMessage(1077606, null, 0x23); // You have achieved the rank of Apprentice Scholar. Return to Mithneral in New Haven to receive your arcane prize.
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
	
    public class Mithneral : MondainQuester
    {
        public override Type[] Quests
        { 
            get
            {
                return new Type[] 
                {
                    typeof(ScholarlyTaskQuest)
                };
            }
        }
		
        [Constructable]
        public Mithneral()
            : base("Mithneral", "The Evaluating Intelligence Instructor")
        { 
            this.SetSkill(SkillName.Intelekt, 120.0, 120.0);
            this.SetSkill(SkillName.Inskrypcja, 120.0, 120.0);
            this.SetSkill(SkillName.Magia, 120.0, 120.0);
            this.SetSkill(SkillName.ObronaPrzedMagia, 120.0, 120.0);
            this.SetSkill(SkillName.Boks, 120.0, 120.0);
            this.SetSkill(SkillName.Medytacja, 120.0, 120.0);
        }
		
        public Mithneral(Serial serial)
            : base(serial)
        {
        }
		
        public override void Advertise()
        {
            this.Say(1078127); // Want to maximize your spell damage? I have a scholarly task for you!
        }
		
        public override void OnOfferFailed()
        { 
            this.Say(1077772); // I cannot teach you, for you know all I can teach!
        }
		
        public override void InitBody()
        { 
            this.Female = false;
            this.CantWalk = true;
            this.Race = Race.Human;		
		
            base.InitBody();
        }
		
        public override void InitOutfit()
        {
            this.AddItem(new Backpack());
            this.AddItem(new HoodedShroudOfShadows(0x51C));
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