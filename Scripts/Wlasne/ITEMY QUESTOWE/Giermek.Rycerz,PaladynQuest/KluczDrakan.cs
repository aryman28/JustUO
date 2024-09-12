using System;
using Server.Network;
using Server.Prompts;
using Server.Targeting;

namespace Server.Items
{
    public enum OKluczType
    {
        Copper = 0x100E,
        Gold = 0x100F,
        Iron = 0x1010,
        Rusty = 0x1013
    }

    public interface IXOLockable
    {
        bool Locked { get; set; }
        uint KeyValue { get; set; }
    }

    public class KluczDrakana : Item
    {
        private string m_Description;
        private uint m_KeyVal;
        private Item m_Link;
        private int m_MaxRange;
        [Constructable]
        public KluczDrakana()
            : this(OKluczType.Iron, 0)
        {
        }

        [Constructable]
        public KluczDrakana(OKluczType type)
            : this(type, 0)
        {
        }

        [Constructable]
        public KluczDrakana(uint val)
            : this(OKluczType.Iron, val)
        {
        }

        [Constructable]
        public KluczDrakana(OKluczType type, uint LockVal)
            : this(type, LockVal, null)
        {
            //this.m_KeyVal = LockVal;
            this.m_KeyVal = 100;
        }

        public KluczDrakana(OKluczType type, uint LockVal, Item link)
            : base((int)type)
        {
            this.Weight = 1.0;
            this.Name = "Klucz Drakana";
            this.Hue = 488;
            this.m_MaxRange = 3;
            //this.m_KeyVal = LockVal;
            this.m_Link = link;
        }

        public KluczDrakana(Serial serial)
            : base(serial)
        {
        }

        [CommandProperty(AccessLevel.GameMaster)]
        public string Description
        {
            get
            {
                return this.m_Description;
            }
            set
            {
                this.m_Description = value;
                this.InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public int MaxRange
        {
            get
            {
                return this.m_MaxRange;
            }

            set
            {
                this.m_MaxRange = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public uint KeyValue
        {
            get
            {
                return this.m_KeyVal;
            }

            set
            {
                this.m_KeyVal = value;
                this.InvalidateProperties();
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public Item Link
        {
            get
            {
                return this.m_Link;
            }

            set
            {
                this.m_Link = value;
            }
        }
        public static uint RandomValue()
        {
            return (uint)(0xFFFFFFFE * Utility.RandomDouble()) + 1;
        }

        public static void RemoveKeys(Mobile m, uint keyValue)
        {
            if (keyValue == 0)
                return;

            RemoveKeys(m.Backpack, keyValue);
            RemoveKeys(m.BankBox, keyValue);
        }

        public static void RemoveKeys(Container cont, uint keyValue)
        {
            if (cont == null || keyValue == 0)
                return;

            Item[] items = cont.FindItemsByType(new Type[] { typeof(Key), typeof(KeyRing) });

            foreach (Item item in items)
            {
                if (item is KluczDrakana)
                {
                    KluczDrakana key = (KluczDrakana)item;

                    if (key.KeyValue == keyValue)
                        key.Delete();
                }
                else
                {
                    KeyRing keyRing = (KeyRing)item;

                    keyRing.RemoveKeys(keyValue);
                }
            }
        }

        public static bool ContainsKey(Container cont, uint keyValue)
        {
            if (cont == null)
                return false;

            Item[] items = cont.FindItemsByType(new Type[] { typeof(Key), typeof(KeyRing) });

            foreach (Item item in items)
            {
                if (item is KluczDrakana)
                {
                    KluczDrakana key = (KluczDrakana)item;

                    if (key.KeyValue == keyValue)
                        return true;
                }
                else
                {
                    KeyRing keyRing = (KeyRing)item;

                    if (keyRing.ContainsKey(keyValue))
                        return true;
                }
            }

            return false;
        }

                public override void AddNameProperty( ObjectPropertyList list )
		{ 
		base.AddNameProperty( list );
		list.Add( "<BASEFONT COLOR=#FACC2E>(Przedmiot Questowy)<BASEFONT COLOR=#FFFFFF>" );	
                }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)2); // version

            writer.Write((int)this.m_MaxRange);

            writer.Write((Item)this.m_Link);

            writer.Write((string)this.m_Description);
            writer.Write((uint)this.m_KeyVal);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            switch ( version )
            {
                case 2:
                    {
                        this.m_MaxRange = reader.ReadInt();

                        goto case 1;
                    }
                case 1:
                    {
                        this.m_Link = reader.ReadItem();

                        goto case 0;
                    }
                case 0:
                    {
                        if (version < 2 || this.m_MaxRange == 0)
                            this.m_MaxRange = 3;

                        this.m_Description = reader.ReadString();

                        this.m_KeyVal = reader.ReadUInt();

                        break;
                    }
            }
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!this.IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(501661); // That key is unreachable.
                return;
            }

            Target t;
            int number;

            if (this.m_KeyVal != 0)
            {
                number = 501662; // What shall I use this key on?
                t = new UnlockTarget(this);
            }
            else
            {
                number = 501663; // This key is a key blank. Which key would you like to make a copy of?
                t = new CopyTarget(this);
            }

            from.SendLocalizedMessage(number);
            from.Target = t;
        }

        public override void GetProperties(ObjectPropertyList list)
        {
            base.GetProperties(list);

            string desc;

            if (this.m_KeyVal == 0)
                desc = "(bez zamka)";
            else if ((desc = this.m_Description) == null || (desc = desc.Trim()).Length <= 0)
                desc = null;

            if (desc != null)
                list.Add(desc);
        }

        public override void OnSingleClick(Mobile from)
        {
            base.OnSingleClick(from);

            string desc;

            if (this.m_KeyVal == 0)
                desc = "(bez zamka)";
            else if ((desc = this.m_Description) == null || (desc = desc.Trim()).Length <= 0)
                desc = "";

            if (desc.Length > 0)
                from.Send(new UnicodeMessage(this.Serial, this.ItemID, MessageType.Regular, 0x3B2, 3, "ENU", "", desc));
        }

        public bool UseOn(Mobile from, IXOLockable o)
        {
            if (o.KeyValue == this.KeyValue)
            {
                    if (o is KuferDrakan)
                    {
                        from.SendMessage(33, "Niestety klucz z³ama³ siê w zamku!");
                        this.Delete();
                    }

                if (o is BaseDoor && !((BaseDoor)o).UseLocks())
                {
                    return false;
                }
                else
                {
                    o.Locked = !o.Locked;

                    if (o is LockableContainer)
                    {
                        LockableContainer cont = (LockableContainer)o;

                        if (cont.LockLevel == -255)
                            cont.LockLevel = cont.RequiredSkill - 10;
                    }

                    if (o is Item)
                    {
                        Item item = (Item)o;

                        if (o.Locked)
                            item.SendLocalizedMessageTo(from, 1048000); // You lock it.
                        else
                            item.SendLocalizedMessageTo(from, 1048001); // You unlock it.

                        if (item is LockableContainer)
                        {
                            LockableContainer cont = (LockableContainer)item;

                            if (cont.TrapType != TrapType.None && cont.TrapOnLockpick)
                            {
                                if (o.Locked)
                                    item.SendLocalizedMessageTo(from, 501673); // You re-enable the trap.
                                else
                                    item.SendLocalizedMessageTo(from, 501672); // You disable the trap temporarily.  Lock it again to re-enable it.
                            }
                        }
                    }

                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        private class RenamePrompt : Prompt
        {
            private readonly KluczDrakana m_Key;
            public RenamePrompt(KluczDrakana key)
            {
                this.m_Key = key;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (this.m_Key.Deleted || !this.m_Key.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(501661); // That key is unreachable.
                    return;
                }

                this.m_Key.Description = Utility.FixHtml(text);
            }
        }

        private class UnlockTarget : Target
        {
            private readonly KluczDrakana m_Key;
            public UnlockTarget(KluczDrakana key)
                : base(key.MaxRange, false, TargetFlags.None)
            {
                this.m_Key = key;
                this.CheckLOS = false;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (this.m_Key.Deleted || !this.m_Key.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(501661); // That key is unreachable.
                    return;
                }

                int number;

                if (targeted == this.m_Key)
                {
                    //number = 501665; // Enter a description for this key.
                    //from.Prompt = new RenamePrompt(this.m_Key);
                    from.SendMessage(33, "Dziwne runy wyryte na kluczu nie daj¹ siê usun¹æ.");
                    return;
                }
                else if (targeted is IXOLockable)
                {
                    if (this.m_Key.UseOn(from, (IXOLockable)targeted))
                        number = -1;
                    else
                        number = 501668; // This key doesn't seem to unlock that.
                }
                else
                {
                    number = 501666; // You can't unlock that!
                }

                if (number != -1)
                {
                    from.SendLocalizedMessage(number);
                }
            }
        }

        private class CopyTarget : Target
        {
            private readonly KluczDrakana m_Key;
            public CopyTarget(KluczDrakana key)
                : base(3, false, TargetFlags.None)
            {
                this.m_Key = key;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (this.m_Key.Deleted || !this.m_Key.IsChildOf(from.Backpack))
                {
                    from.SendLocalizedMessage(501661); // That key is unreachable.
                    return;
                }

                int number;

                if (targeted is KluczDrakana)
                {
                    KluczDrakana k = (KluczDrakana)targeted;

                    if (k.m_KeyVal == 0)
                    {
                        number = 501675; // This key is also blank.
                    }
                    else if (from.CheckTargetSkill(SkillName.Majsterkowanie, k, 0, 75.0))
                    {
                        number = 501676; // You make a copy of the key.

                        this.m_Key.Description = k.Description;
                        this.m_Key.KeyValue = k.KeyValue;
                        this.m_Key.Link = k.Link;
                        this.m_Key.MaxRange = k.MaxRange;
                    }
                    else if (Utility.RandomDouble() <= 0.1) // 10% chance to destroy the key
                    {
                        from.SendLocalizedMessage(501677); // You fail to make a copy of the key.

                        number = 501678; // The key was destroyed in the attempt.

                        this.m_Key.Delete();
                    }
                    else
                    {
                        number = 501677; // You fail to make a copy of the key.
                    }
                }
                else
                {
                    number = 501688; // Not a key.
                }

                from.SendLocalizedMessage(number);
            }
        }
    }
}