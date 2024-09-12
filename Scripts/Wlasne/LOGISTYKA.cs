/***************************************************************************
 *   New Ability System script by Lokai. This program is free software; you 
 *   can redistribute it and/or modify it under the terms of the GNU GPL. 
 ***************************************************************************/
using System;
using Server;
using Server.Targeting;
using Server.Items;
using Server.Network;
using Server.Multis;
using Server.Mobiles;
using System.Collections.Generic;

namespace Server.SkillHandlers
{
    public enum RidingRank
    {
        None            =   0,
        Pedestrian      =   10,
        Novice          =   20,
        Amateur         =   30,
        Intermediate    =   40,
        Rider           =   50,
        Advanced        =   60,
        Equestrian      =   75,
        Master          =   90
    }

    public class Riding
    {

        private static bool RIDING_ACTIVE = true; //Set to 'false' to disable Riding checks

////Dismount Delay
        public static readonly TimeSpan OwnerRemountDelay = TimeSpan.FromSeconds(3.0);
//////

        private static Dictionary<Mobile, DateTime> m_RideTime;

        public static void Initialize()
        {
            EventSink.Movement += new MovementEventHandler(EventSink_RidingMovement);
            m_RideTime = new Dictionary<Mobile, DateTime>();
        }

        static void EventSink_RidingMovement(MovementEventArgs e)
        {
            if (RIDING_ACTIVE)
                if (e.Mobile is PlayerMobile && e.Mobile.Mounted && DesignContext.Find(e.Mobile) == null)
                {
                    //if (e.Mobile.Mount is Item) RideEthereal(e.Mobile, (Item)e.Mobile.Mount);
                    if (e.Mobile.Mount is BaseCreature) RideMount(e.Mobile, (BaseCreature)e.Mobile.Mount);
                }
        }

        public static void RideMount(Mobile from, BaseCreature mount)
        {

if (from is PlayerMobile)       
{

if ( from.Skills.Logistyka.Value <= 20 && from.Stam > 1)
{
             if ( from.Skills.Logistyka.Value > Utility.RandomMinMax( 0, 1000 ) ) //10%
             {
             from.Dismount();
             from.Damage(Utility.Random(4, 8));
             from.SendMessage(33, "Upadasz!");
             BaseMount.SetMountPrevention( mount.ControlMaster, BlockMountType.DismountRecovery, OwnerRemountDelay);
             from.PlaySound(0x13D);
             }
}
else if ( from.Skills.Logistyka.Value > 20 && from.Skills.Logistyka.Value <= 40 && from.Stam > 1)
{
             if ( from.Skills.Logistyka.Value > Utility.RandomMinMax( 0, 4000 ) ) //8%
             {
             from.Dismount();
             from.Damage(Utility.Random(4, 6));
             from.SendMessage(33, "Upadasz!");
             BaseMount.SetMountPrevention( mount.ControlMaster, BlockMountType.DismountRecovery, OwnerRemountDelay);
             from.PlaySound(0x13D);
             }
}
else if ( from.Skills.Logistyka.Value > 40 && from.Skills.Logistyka.Value <= 60 && from.Stam > 1)
{
             if ( from.Skills.Logistyka.Value > Utility.RandomMinMax( 0, 8000 ) ) //6%
             {
             from.Dismount();
             from.Damage(Utility.Random(3, 4));
             from.SendMessage(33, "Upadasz! chodz jestes doswiadczonym jezdzcem.");
             BaseMount.SetMountPrevention( mount.ControlMaster, BlockMountType.DismountRecovery, OwnerRemountDelay);
             from.PlaySound(0x13D);
             }
}
else if ( from.Skills.Logistyka.Value > 60 && from.Skills.Logistyka.Value <= 80 && from.Stam > 1)
{
             if ( from.Skills.Logistyka.Value > Utility.RandomMinMax( 0, 16000 ) ) //4%
             {
             from.Dismount();
             from.Damage(Utility.Random(2, 3));
             from.SendMessage(33, "Upadasz! chodz jestes ekspertem jezdziectwa.");
             BaseMount.SetMountPrevention( mount.ControlMaster, BlockMountType.DismountRecovery, OwnerRemountDelay);
             from.PlaySound(0x13D);
             }
}
else if ( from.Skills.Logistyka.Value > 80 && from.Skills.Logistyka.Value <= 95 && from.Stam > 1)
{
             if ( from.Skills.Logistyka.Value > Utility.RandomMinMax( 0, 32000 ) ) //2%
             {
             from.Dismount();
             from.Damage(Utility.Random(2, 3));
             from.SendMessage(33, "Upadasz! nawet mistrzom jezdziectwa zdarzaja sie upadki.");
             BaseMount.SetMountPrevention( mount.ControlMaster, BlockMountType.DismountRecovery, OwnerRemountDelay);
             from.PlaySound(0x13D);
             }
}
else if ( from.Skills.Logistyka.Value > 95 && from.Skills.Logistyka.Value <= 120 && from.Stam > 1)
{
             if ( from.Skills.Logistyka.Value > Utility.RandomMinMax( 0, 40000 ) ) //1%
             {
             from.Dismount();
             from.SendMessage(33, "Upadasz! nawet arcymistrzom jezdziectwa zdarzaja sie upadki.");
             BaseMount.SetMountPrevention( mount.ControlMaster, BlockMountType.DismountRecovery, OwnerRemountDelay);
             from.PlaySound(0x13D);
             }
}


    if ( from.Stam > 5 )
    {
    from.CheckSkill( SkillName.Logistyka, 0.0, 100.0 );
    }

}
       }
     }
   }