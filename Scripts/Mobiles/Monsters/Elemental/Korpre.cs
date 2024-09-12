using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a korpre corpse")]
    public class Korpre : BaseCreature
    {
        [Constructable]
        public Korpre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Korpre";
            Body = 51;
            BaseSoundID = 456;

            Hue = 2071;

            SetStr(22, 34);
            SetDex(16, 21);
            SetInt(16, 20);

            SetHits(15, 19);

            SetDamage(1, 5);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 5, 10);
            SetResistance(ResistanceType.Poison, 15, 20);

            SetSkill(SkillName.Zatruwanie, 36.0, 49.1);
            SetSkill(SkillName.Anatomia, 0);
            SetSkill(SkillName.ObronaPrzedMagia, 15.9, 18.9);
            SetSkill(SkillName.Taktyka, 24.6, 26.1);
            SetSkill(SkillName.Boks, 24.9, 26.1);

            Fame = 300;
            Karma = -300;

            QLPoints = 3;

            VirtualArmor = 8;
        }

        //TODO: Damage weapon via acid
        public Korpre(Serial serial)
            : base(serial)
        {
        }

        public override Poison PoisonImmune
        {
            get
            {
                return Poison.Regular;
            }
        }
        public override Poison HitPoison
        {
            get
            {
                return Poison.Regular;
            }
        }
        public override FoodType FavoriteFood
        {
            get
            {
                return FoodType.Fish;
            }
        }
        /*		public override void OnGotMeleeAttack(Mobile attacker)
        {
        base.OnGotMeleeAttack(attacker);

        DodeSpawn(attacker);
        }

        public void DodeSpawn(Mobile attacker)
        {
        if (attacker is BaseCreature && ((BaseCreature)atacker) == 6)
        {
        DeSpawn.Mobile(Korpre);
        Spawn.Mobile(Anlorzen);
        }
        else if (attacker == killed.BaseCreature && ((BaseCreature)Isweaker)
        {
        DeSpawn.Mobile(Korpre);
        Spawn.Mobile(new Betballem);
        }
        else if (attacker is BaseCreature && ((BaseCreature)IsAlive) > Time.Span(100)
        {
        DeSpawn.Mobile(Korpre);
        Spawn.Mobile(new Anzuanords);
        }
        else
        {
        Math.Rnd(0,100);
        Math.Rnd = 50;
				
        DeSpawn.Mobile(Korpre);
        Spawn.Mobile(new Ortanord);
        }
        }*/
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Poor);
            AddLoot(LootPack.Gems);
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);		
			
            if (Utility.RandomDouble() < 0.05)
            {
                c.DropItem(new VoidOrb());
            }
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