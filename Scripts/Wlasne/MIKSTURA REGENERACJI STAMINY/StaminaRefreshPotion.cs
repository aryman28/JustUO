/*----------------*/
/*--- Scripted ---*/
/*--- By: JBob ---*/
/*----------------*/
using System;
using Server;

namespace Server.Items
{
	public class StaminaRefreshPotion : BaseStaminaRefreshPotion
	{
		public override double StaminaRefresh{ get{ return 30.0; } }
		[Constructable]
		public StaminaRefreshPotion() : base( PotionEffect.StaminaRefresh )
		{
			Name = "Mikstura Regeneracji Wytrzyma³oœci";
		}
		public StaminaRefreshPotion( Serial serial ) : base( serial )
		{
		}
		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
		}
		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
		}
	}
}