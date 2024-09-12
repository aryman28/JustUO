using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("Zw�oki mr�wki")]
    public class BlackSolenWorker : BaseCreature
    {
        [Constructable]
        public BlackSolenWorker()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "Czarna mr�wka - robotnik";
            this.Body = 805;
            this.BaseSoundID = 959;
            this.Hue = 0x453;

            this.SetStr(96, 120);
            this.SetDex(81, 105);
            this.SetInt(36, 60);

            this.SetHits(58, 72);

            this.SetDamage(5, 7);

            this.SetDamageType(ResistanceType.Physical, 100);

            this.SetResistance(ResistanceType.Physical, 25, 30);
            this.SetResistance(ResistanceType.Fire, 20, 30);
            this.SetResistance(ResistanceType.Cold, 10, 20);
            this.SetResistance(ResistanceType.Poison, 10, 20);
            this.SetResistance(ResistanceType.Energy, 20, 30);

            this.SetSkill(SkillName.ObronaPrzedMagia, 60.0);
            this.SetSkill(SkillName.Taktyka, 65.0);
            this.SetSkill(SkillName.Boks, 60.0);

            this.Fame = 1500;
            this.Karma = -1500;

            this.VirtualArmor = 28;

            this.PackGold(Utility.Random(100, 180));

            SolenHelper.PackPicnicBasket(this);

            this.PackItem(new ZoogiFungus((0.05 > Utility.RandomDouble()) ? 1 : 6));
        }

        public BlackSolenWorker(Serial serial)
            : base(serial)
        {
        }

        public override int GetAngerSound()
        {
            return 0x269;
        }

        public override int GetIdleSound()
        {
            return 0x269;
        }

        public override int GetAttackSound()
        {
            return 0x186;
        }

        public override int GetHurtSound()
        {
            return 0x1BE;
        }

        public override int GetDeathSound()
        {
            return 0x8E;
        }

        public override void GenerateLoot()
        {
            this.AddLoot(LootPack.Gems, Utility.RandomMinMax(1, 2));
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