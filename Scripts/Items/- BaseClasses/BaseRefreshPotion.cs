using System;
using Server.Network;

namespace Server.Items
{
    public abstract class BaseRefreshPotion : BasePotion
    {
        public BaseRefreshPotion(PotionEffect effect)
            : base(0xF0B, effect)
        {
        }

        public BaseRefreshPotion(Serial serial)
            : base(serial)
        {
        }

        public abstract double Delay { get; }

        public abstract double Refresh { get; }
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

        public override void Drink(Mobile from)
        {
            if (from.Stam < from.StamMax)
            {
                    if (from.BeginAction(typeof(BaseRefreshPotion)))
                    {
                      from.Stam += Scale(from, (int)(this.Refresh * from.StamMax));

                      BasePotion.PlayDrinkEffect(from);

                     if (!Engines.ConPVP.DuelContext.IsFreeConsume(from))
                      this.Consume();

                      Timer.DelayCall(TimeSpan.FromSeconds(this.Delay), new TimerStateCallback(ReleaseHealLock), from);
                   }
            
                         else
                         {
                         from.SendMessage(21, "Musisz odczekaæ chwile przed wypiciem kolejnej mikstury.");
                         }
             }
                         if ( from.Stam == from.StamMax && from.BeginAction(typeof(BaseRefreshPotion)) )
                         {
                         from.SendMessage(11, "Wypi³eœ miksture ale nie da³a efektu gdy¿ by³eœ w pe³ni swojej wytrzyma³oœci.");
                         BasePotion.PlayDrinkEffect(from);
                         this.Consume();
                         Timer.DelayCall(TimeSpan.FromSeconds(this.Delay), new TimerStateCallback(ReleaseHealLock), from);
                         }

                         else
                         {
                         from.SendMessage(21, "Musisz odczekaæ chwile przed wypiciem kolejnej mikstury.");
                         }

         }

        private static void ReleaseHealLock(object state)
        {
            ((Mobile)state).EndAction(typeof(BaseRefreshPotion));
        }
    }
}