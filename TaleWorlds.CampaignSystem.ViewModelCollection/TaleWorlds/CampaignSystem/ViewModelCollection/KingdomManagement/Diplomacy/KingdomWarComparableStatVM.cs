using System;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.KingdomManagement.Diplomacy
{
	// Token: 0x02000077 RID: 119
	public class KingdomWarComparableStatVM : ViewModel
	{
		// Token: 0x06000969 RID: 2409 RVA: 0x0002A0E8 File Offset: 0x000282E8
		public KingdomWarComparableStatVM(int faction1Stat, int faction2Stat, TextObject name, string faction1Color, string faction2Color, int defaultRange, BasicTooltipViewModel faction1Hint = null, BasicTooltipViewModel faction2Hint = null)
		{
			int num = MathF.Max(MathF.Max(faction1Stat, faction2Stat), defaultRange);
			if (num == 0)
			{
				num = 1;
			}
			this.Faction1Color = faction1Color;
			this.Faction2Color = faction2Color;
			this.Faction1Value = faction1Stat;
			this.Faction2Value = faction2Stat;
			this._defaultRange = defaultRange;
			this.Faction1Percentage = MathF.Round((float)faction1Stat / (float)num * 100f);
			this.Faction2Percentage = MathF.Round((float)faction2Stat / (float)num * 100f);
			this._nameObj = name;
			this.Faction1Hint = faction1Hint;
			this.Faction2Hint = faction2Hint;
			this.RefreshValues();
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0002A17E File Offset: 0x0002837E
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this._nameObj.ToString();
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0002A197 File Offset: 0x00028397
		// (set) Token: 0x0600096C RID: 2412 RVA: 0x0002A19F File Offset: 0x0002839F
		[DataSourceProperty]
		public BasicTooltipViewModel Faction1Hint
		{
			get
			{
				return this._faction1Hint;
			}
			set
			{
				if (value != this._faction1Hint)
				{
					this._faction1Hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Faction1Hint");
				}
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0002A1BD File Offset: 0x000283BD
		// (set) Token: 0x0600096E RID: 2414 RVA: 0x0002A1C5 File Offset: 0x000283C5
		[DataSourceProperty]
		public BasicTooltipViewModel Faction2Hint
		{
			get
			{
				return this._faction2Hint;
			}
			set
			{
				if (value != this._faction2Hint)
				{
					this._faction2Hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Faction2Hint");
				}
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x0600096F RID: 2415 RVA: 0x0002A1E3 File Offset: 0x000283E3
		// (set) Token: 0x06000970 RID: 2416 RVA: 0x0002A1EB File Offset: 0x000283EB
		[DataSourceProperty]
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0002A20E File Offset: 0x0002840E
		// (set) Token: 0x06000972 RID: 2418 RVA: 0x0002A216 File Offset: 0x00028416
		[DataSourceProperty]
		public string Faction1Color
		{
			get
			{
				return this._faction1Color;
			}
			set
			{
				if (value != this._faction1Color)
				{
					this._faction1Color = value;
					base.OnPropertyChangedWithValue<string>(value, "Faction1Color");
				}
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0002A239 File Offset: 0x00028439
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x0002A241 File Offset: 0x00028441
		[DataSourceProperty]
		public string Faction2Color
		{
			get
			{
				return this._faction2Color;
			}
			set
			{
				if (value != this._faction2Color)
				{
					this._faction2Color = value;
					base.OnPropertyChangedWithValue<string>(value, "Faction2Color");
				}
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000975 RID: 2421 RVA: 0x0002A264 File Offset: 0x00028464
		// (set) Token: 0x06000976 RID: 2422 RVA: 0x0002A26C File Offset: 0x0002846C
		[DataSourceProperty]
		public int Faction1Percentage
		{
			get
			{
				return this._faction1Percentage;
			}
			set
			{
				if (value != this._faction1Percentage)
				{
					this._faction1Percentage = value;
					base.OnPropertyChangedWithValue(value, "Faction1Percentage");
				}
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0002A28A File Offset: 0x0002848A
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x0002A292 File Offset: 0x00028492
		[DataSourceProperty]
		public int Faction1Value
		{
			get
			{
				return this._faction1Value;
			}
			set
			{
				if (value != this._faction1Value)
				{
					this._faction1Value = value;
					base.OnPropertyChangedWithValue(value, "Faction1Value");
				}
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x0002A2B0 File Offset: 0x000284B0
		// (set) Token: 0x0600097A RID: 2426 RVA: 0x0002A2B8 File Offset: 0x000284B8
		[DataSourceProperty]
		public int Faction2Percentage
		{
			get
			{
				return this._faction2Percentage;
			}
			set
			{
				if (value != this._faction2Percentage)
				{
					this._faction2Percentage = value;
					base.OnPropertyChangedWithValue(value, "Faction2Percentage");
				}
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0002A2D6 File Offset: 0x000284D6
		// (set) Token: 0x0600097C RID: 2428 RVA: 0x0002A2DE File Offset: 0x000284DE
		[DataSourceProperty]
		public int Faction2Value
		{
			get
			{
				return this._faction2Value;
			}
			set
			{
				if (value != this._faction2Value)
				{
					this._faction2Value = value;
					base.OnPropertyChangedWithValue(value, "Faction2Value");
				}
			}
		}

		// Token: 0x04000415 RID: 1045
		private TextObject _nameObj;

		// Token: 0x04000416 RID: 1046
		private int _defaultRange;

		// Token: 0x04000417 RID: 1047
		private BasicTooltipViewModel _faction1Hint;

		// Token: 0x04000418 RID: 1048
		private BasicTooltipViewModel _faction2Hint;

		// Token: 0x04000419 RID: 1049
		private string _name;

		// Token: 0x0400041A RID: 1050
		private string _faction1Color;

		// Token: 0x0400041B RID: 1051
		private string _faction2Color;

		// Token: 0x0400041C RID: 1052
		private int _faction1Percentage;

		// Token: 0x0400041D RID: 1053
		private int _faction1Value;

		// Token: 0x0400041E RID: 1054
		private int _faction2Percentage;

		// Token: 0x0400041F RID: 1055
		private int _faction2Value;
	}
}
