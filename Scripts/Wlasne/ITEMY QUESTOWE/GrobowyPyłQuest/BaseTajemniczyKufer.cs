using System;

namespace Server.Items
{
    public class BaseTajemniczyKufer : LockableTajemniczyContainer
    {
        private TreasureLevel m_TreasureLevel;
        private short m_MaxSpawnTime = 2;
        private short m_MinSpawnTime = 1;
        private TreasureResetTimer m_ResetTimer;
        public BaseTajemniczyKufer(int itemID)
            : this(itemID, TreasureLevel.Level1)
        {
        }

        public BaseTajemniczyKufer(int itemID, TreasureLevel level)
            : base(itemID)
        {
            this.m_TreasureLevel = level;
            this.Locked = true;
            this.KeyValue = 101;
            this.Movable = false;

            this.SetLockLevel();
            this.GenerateTreasure();
        }

        public BaseTajemniczyKufer(Serial serial)
            : base(serial)
        {
        }

        public enum TreasureLevel
        {
            Level1, 
            Level2, 
            Level3, 
            Level4, 
            Level5,
            Level6,
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public TreasureLevel Level
        {
            get
            {
                return this.m_TreasureLevel;
            }
            set
            {
                this.m_TreasureLevel = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public short MaxSpawnTime
        {
            get
            {
                return this.m_MaxSpawnTime;
            }
            set
            {
                this.m_MaxSpawnTime = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public short MinSpawnTime
        {
            get
            {
                return this.m_MinSpawnTime;
            }
            set
            {
                this.m_MinSpawnTime = value;
            }
        }
        [CommandProperty(AccessLevel.GameMaster)]
        public override bool Locked
        {
            get
            {
                return base.Locked;
            }
            set
            {
                if (base.Locked != value)
                {
                    base.Locked = value;
					
                    if (!value)
                        this.StartResetTimer();
                }
            }
        }
        public override bool IsDecoContainer
        {
            get
            {
                return false;
            }
        }
        public override string DefaultName
        {
            get
            {
                if (this.Locked)
                    return "zamkniêty tajemniczy kufer";

                return "tajemniczy kufer";
            }
        }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write((int)0);
            writer.Write((byte)this.m_TreasureLevel);
            writer.Write(this.m_MinSpawnTime);
            writer.Write(this.m_MaxSpawnTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();

            this.m_TreasureLevel = (TreasureLevel)reader.ReadByte();
            this.m_MinSpawnTime = reader.ReadShort();
            this.m_MaxSpawnTime = reader.ReadShort();

            if (!this.Locked)
                this.StartResetTimer();
        }

        public void ClearContents()
        {
            for (int i = this.Items.Count - 1; i >= 0; --i)
            {
                if (i < this.Items.Count)
                    this.Items[i].Delete();
            }
        }

        public void Reset()
        {
            if (this.m_ResetTimer != null)
            {
                if (this.m_ResetTimer.Running)
                    this.m_ResetTimer.Stop();
            }

            this.Locked = true;
            this.ClearContents();
            this.GenerateTreasure();
        }

        protected virtual void SetLockLevel()
        {
            switch( this.m_TreasureLevel )
            {
                case TreasureLevel.Level1:
                    this.RequiredSkill = this.LockLevel = 120;
                    break;
                case TreasureLevel.Level2:
                    this.RequiredSkill = this.LockLevel = 120;
                    break;
                case TreasureLevel.Level3:
                    this.RequiredSkill = this.LockLevel = 120;
                    break;
                case TreasureLevel.Level4:
                    this.RequiredSkill = this.LockLevel = 120;
                    break;
                case TreasureLevel.Level5:
                    this.RequiredSkill = this.LockLevel = 120;
                    break;
                case TreasureLevel.Level6:
                    this.RequiredSkill = this.LockLevel = 120;
                    break;
            }
        }

        protected virtual void GenerateTreasure()
        {
            int MinGold = 1;
            int MaxGold = 2;

            switch( this.m_TreasureLevel )
            {
                case TreasureLevel.Level1:
                    MinGold = 100;
                    MaxGold = 200;
                    break;
                case TreasureLevel.Level2:
                    MinGold = 300;
                    MaxGold = 600;
                    break;
                case TreasureLevel.Level3:
                    MinGold = 600;
                    MaxGold = 900;
                    break;
                case TreasureLevel.Level4:
                    MinGold = 900;
                    MaxGold = 1200;
                    break;
                case TreasureLevel.Level5:
                    MinGold = 1200;
                    MaxGold = 5000;
                    break;
                case TreasureLevel.Level6:
                    MinGold = 5000;
                    MaxGold = 9000;
                    break;
            }

            this.DropItem(new Gold(MinGold, MaxGold));
            this.DropItem(new MrocznyGrimmur());
            this.DropItem(new CurseWeaponScroll(1));
            this.DropItem(new BatWing(100));
            this.DropItem(new PigIron(100));
            this.DropItem(new DaemonBlood(20));
            this.DropItem(new NoxCrystal(20));
            this.DropItem(new KodeksNekromanty());
        }

        private void StartResetTimer()
        {
            if (this.m_ResetTimer == null)
                this.m_ResetTimer = new TreasureResetTimer(this);
            else
                this.m_ResetTimer.Delay = TimeSpan.FromMinutes(Utility.Random(this.m_MinSpawnTime, this.m_MaxSpawnTime));

            this.m_ResetTimer.Start();
        }

        private class TreasureResetTimer : Timer
        {
            private readonly BaseTajemniczyKufer m_Chest;
            public TreasureResetTimer(BaseTajemniczyKufer chest)
                : base(TimeSpan.FromMinutes(Utility.Random(chest.MinSpawnTime, chest.MaxSpawnTime)))
            {
                this.m_Chest = chest;
                this.Priority = TimerPriority.OneMinute;
            }

            protected override void OnTick()
            {
                this.m_Chest.Reset();
            }
        }
        ; 
    }
}