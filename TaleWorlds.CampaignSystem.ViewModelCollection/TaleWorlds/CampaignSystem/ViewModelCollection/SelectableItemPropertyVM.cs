using System;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.ViewModelCollection
{
	// Token: 0x0200001E RID: 30
	public class SelectableItemPropertyVM : ViewModel
	{
		// Token: 0x060001CA RID: 458 RVA: 0x0000CA7A File Offset: 0x0000AC7A
		public SelectableItemPropertyVM(string name, string value, bool isWarning = false, BasicTooltipViewModel hint = null)
		{
			this.Name = name;
			this.Value = value;
			this.Hint = hint;
			this.Type = 0;
			this.IsWarning = isWarning;
			this.RefreshValues();
		}

		// Token: 0x060001CB RID: 459 RVA: 0x0000CAAC File Offset: 0x0000ACAC
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.ColonText = GameTexts.FindText("str_colon", null).ToString();
		}

		// Token: 0x060001CC RID: 460 RVA: 0x0000CACA File Offset: 0x0000ACCA
		private void ExecuteLink(string link)
		{
			Campaign.Current.EncyclopediaManager.GoToLink(link);
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x060001CD RID: 461 RVA: 0x0000CADC File Offset: 0x0000ACDC
		// (set) Token: 0x060001CE RID: 462 RVA: 0x0000CAE4 File Offset: 0x0000ACE4
		[DataSourceProperty]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000CB02 File Offset: 0x0000AD02
		// (set) Token: 0x060001D0 RID: 464 RVA: 0x0000CB0A File Offset: 0x0000AD0A
		[DataSourceProperty]
		public bool IsWarning
		{
			get
			{
				return this._isWarning;
			}
			set
			{
				if (value != this._isWarning)
				{
					this._isWarning = value;
					base.OnPropertyChangedWithValue(value, "IsWarning");
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x060001D1 RID: 465 RVA: 0x0000CB28 File Offset: 0x0000AD28
		// (set) Token: 0x060001D2 RID: 466 RVA: 0x0000CB30 File Offset: 0x0000AD30
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

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000CB53 File Offset: 0x0000AD53
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000CB5B File Offset: 0x0000AD5B
		[DataSourceProperty]
		public string Value
		{
			get
			{
				return this._value;
			}
			set
			{
				if (value != this._value)
				{
					this._value = value;
					base.OnPropertyChangedWithValue<string>(value, "Value");
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001D5 RID: 469 RVA: 0x0000CB7E File Offset: 0x0000AD7E
		// (set) Token: 0x060001D6 RID: 470 RVA: 0x0000CB86 File Offset: 0x0000AD86
		[DataSourceProperty]
		public BasicTooltipViewModel Hint
		{
			get
			{
				return this._hint;
			}
			set
			{
				if (value != this._hint)
				{
					this._hint = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Hint");
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x060001D7 RID: 471 RVA: 0x0000CBA4 File Offset: 0x0000ADA4
		// (set) Token: 0x060001D8 RID: 472 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		[DataSourceProperty]
		public string ColonText
		{
			get
			{
				return this._colonText;
			}
			set
			{
				if (value != this._colonText)
				{
					this._colonText = value;
					base.OnPropertyChangedWithValue<string>(value, "ColonText");
				}
			}
		}

		// Token: 0x040000D5 RID: 213
		private int _type;

		// Token: 0x040000D6 RID: 214
		private bool _isWarning;

		// Token: 0x040000D7 RID: 215
		private string _name;

		// Token: 0x040000D8 RID: 216
		private string _value;

		// Token: 0x040000D9 RID: 217
		private BasicTooltipViewModel _hint;

		// Token: 0x040000DA RID: 218
		private string _colonText;

		// Token: 0x02000180 RID: 384
		public enum PropertyType
		{
			// Token: 0x0400106C RID: 4204
			None,
			// Token: 0x0400106D RID: 4205
			Wall,
			// Token: 0x0400106E RID: 4206
			Garrison,
			// Token: 0x0400106F RID: 4207
			Militia,
			// Token: 0x04001070 RID: 4208
			Prosperity,
			// Token: 0x04001071 RID: 4209
			Food,
			// Token: 0x04001072 RID: 4210
			Loyalty,
			// Token: 0x04001073 RID: 4211
			Security,
			// Token: 0x04001074 RID: 4212
			Shipyard,
			// Token: 0x04001075 RID: 4213
			Patrol,
			// Token: 0x04001076 RID: 4214
			CoastalPatrol
		}
	}
}
