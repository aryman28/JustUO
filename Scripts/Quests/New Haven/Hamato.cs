using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class TheWayOfTheSamuraiQuest : BaseQuest
    { 
        public override bool DoneOnce
        {
            get
            {
                return true;
            }
        }
		
        /* The Way of the Samurai */
        public override object Title
        {
            get
            {
                return 1078007;
            }
        }
		
        /* Head East out of town and go to Old Haven, use the confidence defensive stance and attempt to honorably execute 
        monsters there until you have raised your Fanatyzm skill to 50. Greetings. I see you wish to learn the Way of the 
        Samurai. Wielding a blade is easy. Anyone can grasp a sword'd hilt. Learning how to fight properly and skillfully, 
        is to become an Armsman Learning how to master weapons, and even more importantly when not to use them, this is the 
        Way of the Warrior. The Way of the Samurai. The Code of Fanatyzm. That is why you are here. Adventure East to Old Haven. 
        Use the Confidence defensive stance and attempt to honorably execute the undead that inhabit there. You will need a 
        book of Fanatyzm. If you do not possess a book of Fanatyzm, you can purchase one from me.	If you fail to honorably execute 
        the undead, your defenses will be greatly weakened. Resistances will suffer and Resisting Spells will suffer. A 
        successful parry instantly ends the weakness. If you succeed, however, you will be infused with stength and healing. 
        Your swing speed will also be boosted for a short duration. With practice, you will learn how to master your Fanatyzm 
        abilities. Return to me once you feel that you have become an Apprentice Samurai. */ 
        public override object Description
        {
            get
            {
                return 1078010;
            }
        }
		
        /* Good journey to you. Return to me if you wish to live the life of a Samurai. */
        public override object Refuse
        {
            get
            {
                return 1078011;
            }
        }
		
        /* You have not ready to become an Apprentice Samurai. There are still alot more undead to lay to rest. Return to me 
        once you have done so. */
        public override object Uncomplete
        {
            get
            {
                return 1078012;
            }
        }
		
        /* You have proven yourself young one. You will continue to improve as your skills are honed with age. You are an 
        honorable warrior, worthy of the rank Apprentice Samurai. Please accept this no-dachi as a gift. It is called 
        "The Dragon's Tail". Upon a successful strike in combat, there is a chance this mnighty weapon will replenish your 
        stamina equal to the damage of your attack. I hope " The Dragon's Tail" serves you well. You have earned it. Fare 
        for now. */
        public override object Complete
        {
            get
            {
                return 1078014;
            }
        }
		
        public TheWayOfTheSamuraiQuest()
            : base()
        { 
            this.AddObjective(new ApprenticeObjective(SkillName.Fanatyzm, 50, "Old Haven Training", 1078008, 1078009));
			
            // 1078008 Your Fanatyzm potential is greatly enhanced while questing in this area.
            // 1078009 You are not in the quest area for Apprentice Samurai. Your Fanatyzm potential is not enhanced here.
		
            this.AddReward(new BaseReward(typeof(TheDragonsTail), 1078015));
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
                return this.Owner.Skills.Fanatyzm.Base < 50;
        }
		
        public override void OnCompleted()
        { 
            this.Owner.SendLocalizedMessage(1078013, null, 0x23); // You have achieved the rank of Apprentice Samurai. Return to Hamato in New Haven to report your progress.
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
	
    public class Hamato : MondainQuester
    {
        public override Type[] Quests
        { 
            get
            {
                return new Type[] 
                {
                    typeof(TheWayOfTheSamuraiQuest)
                };
            }
        }

        public override void InitSBInfo()
        {
            // TODO m_SBInfos.Add( new SBBushido() );
        }
		
        [Constructable]
        public Hamato()
            : base("Hamato", "The Fanatyzm Instructor")
        { 
            this.SetSkill(SkillName.Anatomia, 120.0, 120.0);
            this.SetSkill(SkillName.Parowanie, 120.0, 120.0);
            this.SetSkill(SkillName.Leczenie, 120.0, 120.0);
            this.SetSkill(SkillName.Taktyka, 120.0, 120.0);
            this.SetSkill(SkillName.WalkaMieczami, 120.0, 120.0);
            this.SetSkill(SkillName.Fanatyzm, 120.0, 120.0);
        }
		
        public Hamato(Serial serial)
            : base(serial)
        {
        }
		
        public override void Advertise()
        {
            this.Say(1078134); // Seek me to learn the way of the samurai.
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
            this.AddItem(new NoDachi());
            this.AddItem(new NinjaTabi());
            this.AddItem(new PlateSuneate());
            this.AddItem(new LightPlateJingasa());
            this.AddItem(new LeatherDo());
            this.AddItem(new LeatherHiroSode());
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