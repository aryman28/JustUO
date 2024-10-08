using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Zw�oki mr�wki")] // TODO: Corpse name?
    public class BlackSolenInfiltratorQueen : BaseCreature
    {
        [Constructable]
        public BlackSolenInfiltratorQueen()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Infiltrator - Czarna mr�wka (kr�lowa)";
            this.Body = 807;
            this.BaseSoundID = 959;
            this.Hue = 0x453;

            this.SetStr(326, 350);
            this.SetDex(141, 165);
            this.SetInt(96, 120);

            this.SetHits(151, 162);

            this.SetDamage(10, 15);

            this.SetDamageType(ResistanceType.Physical, 70);
            this.SetDamageType(ResistanceType.Poison, 30);

            this.SetResistance(ResistanceType.Physical, 30, 40);
            this.SetResistance(ResistanceType.Fire, 30, 35);
            this.SetResistance(ResistanceType.Cold, 25, 35);
            this.SetResistance(ResistanceType.Poison, 35, 40);
            this.SetResistance(ResistanceType.Energy, 25, 30);

            this.SetSkill(SkillName.ObronaPrzedMagia, 90.0);
            this.SetSkill(SkillName.Taktyka, 90.0);
            this.SetSkill(SkillName.Boks, 90.0);

            this.Fame = 6500;
            this.Karma = -6500;

            this.VirtualArmor = 50;

            SolenHelper.PackPicnicBasket(this);

            this.PackItem(new ZoogiFungus((0.05 > Utility.RandomDouble()) ? 4 : 16));
        }

        public BlackSolenInfiltratorQueen(Serial serial)
            : base(serial)
        {
        }

        public override int GetAngerSound()
        {
            return 0x259;
        }

        public override int GetIdleSound()
        {
            return 0x259;
        }

        public override int GetAttackSound()
        {
            return 0x195;
        }

        public override int GetHurtSound()
        {
            return 0x250;
        }

        public override int GetDeathSound()
        {
            return 0x25B;
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Rich);
        }

        public override bool IsEnemy(Mobile m)
        {
            if (SolenHelper.CheckBlackFriendship(m))
                return false;
            else
                return base.IsEnemy(m);
        }

        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            SolenHelper.OnBlackDamage(from);

            base.OnDamage(amount, from, willKill);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}