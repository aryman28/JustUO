using System;
using Server;
using Server.Network;
using Server.Gumps;

namespace Server.Items
{
	public class  MagiaNaturyScroll: Item
	{
		public  int mD01BarkSkinSpell = 0;
		public 	int mD02CircleOfThornsSpell = 0;
		public 	int mD03EnchantedGroveSpell = 0;
		public 	int mD04ForestKinSpell = 0;
		public 	int mD05GraspingRootsSpell = 0;
		public 	int mD06HibernateSpell = 0;
		public 	int mD07HollowReedSpell = 0;
		public 	int mD08HurricaneSpell = 0;
		public 	int mD09LureStoneSpell = 0;
		public 	int mD10ManaSpringSpell = 0;
		public 	int mD11MushroomGatewaySpell = 0;
		public 	int mD12RestorativeSoilSpell = 0;
		public 	int mD13ShieldOfEarthSpell = 0;
		public 	int mD14SpringOfLifeSpell = 0;

		[CommandProperty(AccessLevel.GameMaster)]
		public int D01BarkSkinSpell { get { return mD01BarkSkinSpell; } set { mD01BarkSkinSpell = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D02CircleOfThornsSpell { get { return mD02CircleOfThornsSpell; } set { mD02CircleOfThornsSpell = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D03EnchantedGroveSpell { get { return mD03EnchantedGroveSpell; } set { mD03EnchantedGroveSpell = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D04ForestKinSpell { get { return mD04ForestKinSpell; } set { mD04ForestKinSpell = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D05GraspingRootsSpell { get { return mD05GraspingRootsSpell; } set { mD05GraspingRootsSpell = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D06HibernateSpell{ get { return mD06HibernateSpell; } set { mD06HibernateSpell= value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D07HollowReedSpell { get { return mD07HollowReedSpell; } set { mD07HollowReedSpell = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D08HurricaneSpell { get { return mD08HurricaneSpell; } set { mD08HurricaneSpell = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D09LureStoneSpell  { get { return mD09LureStoneSpell ; } set { mD09LureStoneSpell  = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D10ManaSpringSpell { get { return mD10ManaSpringSpell; } set { mD10ManaSpringSpell = value; } }
        //
        [CommandProperty(AccessLevel.GameMaster)]
		public int D11MushroomGatewaySpell { get { return mD11MushroomGatewaySpell; } set { mD11MushroomGatewaySpell = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D12RestorativeSoilSpell { get { return mD12RestorativeSoilSpell; } set { mD12RestorativeSoilSpell = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D13ShieldOfEarthSpell  { get { return mD13ShieldOfEarthSpell ; } set { mD13ShieldOfEarthSpell  = value; } }
		[CommandProperty(AccessLevel.GameMaster)]
		public int D14SpringOfLifeSpell { get { return mD14SpringOfLifeSpell; } set { mD14SpringOfLifeSpell = value; } }
        

		[Constructable]
		public MagiaNaturyScroll() : base( 0x14F0 )
		{
			LootType = LootType.Blessed;
			Hue = 0x8A2;
			Name = "Zaklęcia Magii Natury";
		}

		public MagiaNaturyScroll( Serial serial ) : base( serial )
		{
		}

		public override void OnDoubleClick( Mobile from )
		{
			if (!IsChildOf(from.Backpack))
			{
				from.SendLocalizedMessage(1042001);
			}
			else
			{
				from.CloseGump( typeof( MagiaNaturyScrollGump ) );
				from.SendGump( new MagiaNaturyScrollGump( from, this ) );
			}
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );
			writer.Write( (int) 0 ); // version
			writer.Write(mD01BarkSkinSpell);
			writer.Write(mD02CircleOfThornsSpell);
			writer.Write(mD03EnchantedGroveSpell);
			writer.Write(mD04ForestKinSpell);
			writer.Write(mD05GraspingRootsSpell);
			writer.Write(mD06HibernateSpell);
			writer.Write(mD07HollowReedSpell);
			writer.Write(mD08HurricaneSpell);
			writer.Write(mD09LureStoneSpell);
			writer.Write(mD10ManaSpringSpell);
            //
            writer.Write(mD11MushroomGatewaySpell);
			writer.Write(mD12RestorativeSoilSpell);
			writer.Write(mD13ShieldOfEarthSpell);
			writer.Write(mD14SpringOfLifeSpell);

		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );
			int version = reader.ReadInt();
			mD01BarkSkinSpell = reader.ReadInt();
			mD02CircleOfThornsSpell = reader.ReadInt();
			mD03EnchantedGroveSpell = reader.ReadInt();
			mD04ForestKinSpell = reader.ReadInt();
			mD05GraspingRootsSpell = reader.ReadInt();
			mD06HibernateSpell= reader.ReadInt();
			mD07HollowReedSpell = reader.ReadInt();
			mD08HurricaneSpell = reader.ReadInt();
			mD09LureStoneSpell  = reader.ReadInt();
			mD10ManaSpringSpell = reader.ReadInt();
            //
            mD11MushroomGatewaySpell = reader.ReadInt();
			mD12RestorativeSoilSpell = reader.ReadInt();
			mD13ShieldOfEarthSpell  = reader.ReadInt();
			mD14SpringOfLifeSpell = reader.ReadInt();

		}
	}
}