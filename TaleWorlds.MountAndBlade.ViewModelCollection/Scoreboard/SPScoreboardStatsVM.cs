using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Scoreboard
{
	// Token: 0x02000015 RID: 21
	public class SPScoreboardStatsVM : ViewModel
	{
		// Token: 0x0600019D RID: 413 RVA: 0x000064E4 File Offset: 0x000046E4
		public SPScoreboardStatsVM(TextObject name)
		{
			this._nameTextObject = name;
			this.Kill = 0;
			this.Dead = 0;
			this.Wounded = 0;
			this.Routed = 0;
			this.Remaining = 0;
			this.ReadyToUpgrade = 0;
			this.IsMainParty = false;
			this.IsMainHero = false;
			this.RefreshValues();
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006547 File Offset: 0x00004747
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject nameTextObject = this._nameTextObject;
			this.NameText = ((nameTextObject != null) ? nameTextObject.ToString() : null) ?? "";
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006570 File Offset: 0x00004770
		public void UpdateScores(int numberRemaining, int numberDead, int numberWounded, int numberRouted, int numberKilled, int numberReadyToUpgrade)
		{
			this.Kill += numberKilled;
			this.Dead += numberDead;
			this.Wounded += numberWounded;
			this.Routed += numberRouted;
			this.Remaining += numberRemaining;
			this.ReadyToUpgrade += numberReadyToUpgrade;
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x000065D4 File Offset: 0x000047D4
		public bool IsAnyStatRelevant()
		{
			return this.Remaining >= 1 || this.Routed >= 1;
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x000065F0 File Offset: 0x000047F0
		public SPScoreboardStatsVM GetScoreForOneAliveMember()
		{
			return new SPScoreboardStatsVM(TextObject.GetEmpty())
			{
				Remaining = MathF.Min(1, this.Remaining),
				Dead = 0,
				Wounded = 0,
				Routed = MathF.Min(1, this.Routed),
				Kill = 0,
				ReadyToUpgrade = 0
			};
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060001A2 RID: 418 RVA: 0x00006647 File Offset: 0x00004847
		// (set) Token: 0x060001A3 RID: 419 RVA: 0x0000664F File Offset: 0x0000484F
		[DataSourceProperty]
		public string NameText
		{
			get
			{
				return this._nameText;
			}
			set
			{
				if (value != this._nameText)
				{
					this._nameText = value;
					base.OnPropertyChangedWithValue<string>(value, "NameText");
				}
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x00006672 File Offset: 0x00004872
		// (set) Token: 0x060001A5 RID: 421 RVA: 0x0000667A File Offset: 0x0000487A
		[DataSourceProperty]
		public bool IsMainHero
		{
			get
			{
				return this._isMainHero;
			}
			set
			{
				if (value != this._isMainHero)
				{
					this._isMainHero = value;
					base.OnPropertyChangedWithValue(value, "IsMainHero");
				}
			}
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060001A6 RID: 422 RVA: 0x00006698 File Offset: 0x00004898
		// (set) Token: 0x060001A7 RID: 423 RVA: 0x000066A0 File Offset: 0x000048A0
		[DataSourceProperty]
		public bool IsMainParty
		{
			get
			{
				return this._isMainParty;
			}
			set
			{
				if (value != this._isMainParty)
				{
					this._isMainParty = value;
					base.OnPropertyChangedWithValue(value, "IsMainParty");
				}
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x000066BE File Offset: 0x000048BE
		// (set) Token: 0x060001A9 RID: 425 RVA: 0x000066C6 File Offset: 0x000048C6
		[DataSourceProperty]
		public int Kill
		{
			get
			{
				return this._kill;
			}
			set
			{
				if (value != this._kill)
				{
					this._kill = value;
					base.OnPropertyChangedWithValue(value, "Kill");
				}
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060001AA RID: 426 RVA: 0x000066E4 File Offset: 0x000048E4
		// (set) Token: 0x060001AB RID: 427 RVA: 0x000066EC File Offset: 0x000048EC
		[DataSourceProperty]
		public int Dead
		{
			get
			{
				return this._dead;
			}
			set
			{
				if (value != this._dead)
				{
					this._dead = value;
					base.OnPropertyChangedWithValue(value, "Dead");
				}
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060001AC RID: 428 RVA: 0x0000670A File Offset: 0x0000490A
		// (set) Token: 0x060001AD RID: 429 RVA: 0x00006712 File Offset: 0x00004912
		[DataSourceProperty]
		public int Wounded
		{
			get
			{
				return this._wounded;
			}
			set
			{
				if (value != this._wounded)
				{
					this._wounded = value;
					base.OnPropertyChangedWithValue(value, "Wounded");
				}
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060001AE RID: 430 RVA: 0x00006730 File Offset: 0x00004930
		// (set) Token: 0x060001AF RID: 431 RVA: 0x00006738 File Offset: 0x00004938
		[DataSourceProperty]
		public int Routed
		{
			get
			{
				return this._routed;
			}
			set
			{
				if (value != this._routed)
				{
					this._routed = value;
					base.OnPropertyChangedWithValue(value, "Routed");
				}
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060001B0 RID: 432 RVA: 0x00006756 File Offset: 0x00004956
		// (set) Token: 0x060001B1 RID: 433 RVA: 0x0000675E File Offset: 0x0000495E
		[DataSourceProperty]
		public int Remaining
		{
			get
			{
				return this._remaining;
			}
			set
			{
				if (value != this._remaining)
				{
					this._remaining = value;
					base.OnPropertyChangedWithValue(value, "Remaining");
				}
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060001B2 RID: 434 RVA: 0x0000677C File Offset: 0x0000497C
		// (set) Token: 0x060001B3 RID: 435 RVA: 0x00006784 File Offset: 0x00004984
		[DataSourceProperty]
		public int ReadyToUpgrade
		{
			get
			{
				return this._readyToUpgrade;
			}
			set
			{
				if (value != this._readyToUpgrade)
				{
					this._readyToUpgrade = value;
					base.OnPropertyChangedWithValue(value, "ReadyToUpgrade");
				}
			}
		}

		// Token: 0x040000C4 RID: 196
		private TextObject _nameTextObject;

		// Token: 0x040000C5 RID: 197
		private string _nameText = "";

		// Token: 0x040000C6 RID: 198
		private int _kill;

		// Token: 0x040000C7 RID: 199
		private int _dead;

		// Token: 0x040000C8 RID: 200
		private int _wounded;

		// Token: 0x040000C9 RID: 201
		private int _routed;

		// Token: 0x040000CA RID: 202
		private int _remaining;

		// Token: 0x040000CB RID: 203
		private int _readyToUpgrade;

		// Token: 0x040000CC RID: 204
		private bool _isMainParty;

		// Token: 0x040000CD RID: 205
		private bool _isMainHero;
	}
}
