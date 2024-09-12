using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Engines.Quests
{
    public class ItsHammerTimeQuest : BaseQuest
    { 
        public override bool DoneOnce
        {
            get
            {
                return true;
            }
        }
		
        /* It's Hammer Time! */
        public override object Title
        {
            get
            {
                return 1077732;
            }
        }
		
        /* Create new daggers and maces using the forge and anvl in George's shop. Try making daggers up to 45 skill, then 
        switch to making maces until 50 skill. Hail, and welcome to my humble shop. I am George Hephaestus, New Haven's 
        blacksmith. I assume that you're here to ask me to train you to be an Appretice Blacksmith. I certainly can do that, 
        but your doing to have to supply your own ingots. You can always buy them at the market, but i highly suggest that 
        you mine your own. That way, any items you sell will be pure profit!. So, once you have a supply of ingots, use my 
        forge and anvil to create items. You'll also need a supply of the proper tools, you can use a smith's hammer, a 
        sledgehammer or tongs. You can either make them yourself if you have the tinkering skill, or buy them from a tinker 
        at the market. Since I'll be around to give you advice, you'll learn faster here than anywhere else. Start off making 
        daggers until you reach 45 blacksmithing skill, then switch to maces until you've achived 50. Once you've done that, 
        come talk to me and I'll give you something for your hard work */ 
        public override object Description
        {
            get
            {
                return 1077735;
            }
        }
		
        /* You're not interested in learning to be a smith, eh? I thought for sure that's why you were here. Oh well if you 
        change your mind, you can always come back and talk to me. */
        public override object Refuse
        {
            get
            {
                return 1077736;
            }
        }
		
        /* You're doing well, but you're not quite there yet. Remember that the quickest way to learn is to make daggers up 
        to 45 skill, and then switch to maces. Also, don't forget that using my forge and anvil will help you learn faster. */
        public override object Uncomplete
        {
            get
            {
                return 1077737;
            }
        }
		
        /*I've been watching you get better and better as you've been smithinh, and I have to say, you're a natural! It's a long road to being a Grandmaster Blacksmith, but I have no doubt that if you put your mind to it you'll get there someday. Let me give you one final piece of advice. If you're smithing just to practics and improve your skill, make items that are oderately difficult (60%-80% success chance), and try to stick to ones that use less ingots.
        Now that you're an Apprentice Blacksmith, I have something for you. While you were busy practicing. I was crafting this hammer for you. It's finely balanced, and has a bit of magic imbued within that will help you craft better items. However, that magic needs to restore itself over time, so you can only use it so many times per day. I hope you find it useful*/
        public override object Complete
        {
            get
            {
                return 1077739;
            }
        }
		
        public ItsHammerTimeQuest()
            : base()
        { 
            this.AddObjective(new ApprenticeObjective(SkillName.Kowalstwo, 50, "Gorge's Shop", 1077733, 1077734));		 
			
            // 1077733 By using George�s forge and anvil, he is able to give you advice as you create blacksmithing items. This helps you hone your Blacksmithing skill a bit faster than normal.
            // 1077734 You�re not using George�s forge and anvil any longer, and he cannot give you advice. Your Blacksmithing learning potential is no longer enhanced. 
		  
            this.AddReward(new BaseReward(typeof(HammerOfHephaestus), 1077740));
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
                return this.Owner.Skills.Kowalstwo.Base < 50;
        }
		
        public override void OnCompleted()
        { 
            this.Owner.SendLocalizedMessage(1077738, null, 0x23); // You have achieved the rank of Apprentice Blacksmith. Return to George Hephaestus in New Haven to see what kind of reward he has waiting for you.
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
	
    public class GeorgeHephaestus : MondainQuester
    {
        public override Type[] Quests
        { 
            get
            {
                return new Type[] 
                {
                    typeof(ItsHammerTimeQuest)
                };
            }
        }
		
        public override void InitSBInfo()
        { 
            this.SBInfos.Add(new SBBlacksmith());
        }
		
        [Constructable]
        public GeorgeHephaestus()
            : base("George Hephaestus", "The Blacksmith Instructor")
        { 
            this.SetSkill(SkillName.WiedzaOUzbrojeniu, 120.0, 120.0);
            this.SetSkill(SkillName.Kowalstwo, 120.0, 120.0);
            this.SetSkill(SkillName.Magia, 120.0, 120.0);
            this.SetSkill(SkillName.Taktyka, 120.0, 120.0);
            this.SetSkill(SkillName.WalkaMieczami, 120.0, 120.0);
            this.SetSkill(SkillName.Majsterkowanie, 120.0, 120.0);
            this.SetSkill(SkillName.Gornictwo, 120.0, 120.0);
        }
		
        public GeorgeHephaestus(Serial serial)
            : base(serial)
        {
        }
		
        public override void Advertise()
        {
            this.Say(1078122); // Wanna learn how to make powerful weapons and armor? Talk to me.
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
            this.AddItem(new Boots(0x973));
            this.AddItem(new LongPants());
            this.AddItem(new Bascinet());
            this.AddItem(new FullApron(0x8AB));
			
            Item item;
			
            item = new SmithHammer();
            item.Hue = 0x8AB;
            this.AddItem(item);			
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