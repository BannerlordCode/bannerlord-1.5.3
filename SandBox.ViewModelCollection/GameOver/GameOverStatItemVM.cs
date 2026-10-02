using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace SandBox.ViewModelCollection.GameOver
{
	// Token: 0x02000059 RID: 89
	public class GameOverStatItemVM : ViewModel
	{
		// Token: 0x0600059F RID: 1439 RVA: 0x000154D3 File Offset: 0x000136D3
		public GameOverStatItemVM(StatItem item)
		{
			this._item = item;
			this.RefreshValues();
		}

		// Token: 0x060005A0 RID: 1440 RVA: 0x000154E8 File Offset: 0x000136E8
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.DefinitionText = GameTexts.FindText("str_game_over_stat_item", this._item.ID).ToString();
			this.ValueText = this._item.Value;
			this.StatTypeAsString = Enum.GetName(typeof(StatItem.StatType), this._item.Type);
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x00015551 File Offset: 0x00013751
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x00015559 File Offset: 0x00013759
		[DataSourceProperty]
		public string DefinitionText
		{
			get
			{
				return this._definitionText;
			}
			set
			{
				if (value != this._definitionText)
				{
					this._definitionText = value;
					base.OnPropertyChangedWithValue<string>(value, "DefinitionText");
				}
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x0001557C File Offset: 0x0001377C
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x00015584 File Offset: 0x00013784
		[DataSourceProperty]
		public string ValueText
		{
			get
			{
				return this._valueText;
			}
			set
			{
				if (value != this._valueText)
				{
					this._valueText = value;
					base.OnPropertyChangedWithValue<string>(value, "ValueText");
				}
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x000155A7 File Offset: 0x000137A7
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x000155AF File Offset: 0x000137AF
		[DataSourceProperty]
		public string StatTypeAsString
		{
			get
			{
				return this._statTypeAsString;
			}
			set
			{
				if (value != this._statTypeAsString)
				{
					this._statTypeAsString = value;
					base.OnPropertyChangedWithValue<string>(value, "StatTypeAsString");
				}
			}
		}

		// Token: 0x040002D1 RID: 721
		private readonly StatItem _item;

		// Token: 0x040002D2 RID: 722
		private string _definitionText;

		// Token: 0x040002D3 RID: 723
		private string _valueText;

		// Token: 0x040002D4 RID: 724
		private string _statTypeAsString;
	}
}
