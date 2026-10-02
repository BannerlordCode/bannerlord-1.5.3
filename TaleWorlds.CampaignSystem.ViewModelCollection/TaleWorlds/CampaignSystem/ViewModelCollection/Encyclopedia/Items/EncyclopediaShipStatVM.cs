using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Encyclopedia.Items
{
	// Token: 0x020000F0 RID: 240
	public class EncyclopediaShipStatVM : ViewModel
	{
		// Token: 0x060015E8 RID: 5608 RVA: 0x000564B8 File Offset: 0x000546B8
		public EncyclopediaShipStatVM(string statId, TextObject name, string value, Func<List<TooltipProperty>> getTooltipProperties = null)
		{
			this._nameTextObj = name;
			this.ValueText = value;
			this.StatId = statId;
			if (getTooltipProperties != null)
			{
				this.Tooltip = new BasicTooltipViewModel(getTooltipProperties);
			}
			else
			{
				this.Tooltip = new BasicTooltipViewModel(() => GameTexts.FindText("str_ship_stat_explanation", this.StatId).ToString());
			}
			this.RefreshValues();
		}

		// Token: 0x060015E9 RID: 5609 RVA: 0x00056510 File Offset: 0x00054710
		public override void RefreshValues()
		{
			base.RefreshValues();
			TextObject textObject = GameTexts.FindText("str_LEFT_colon", null);
			textObject.SetTextVariable("LEFT", this._nameTextObj.ToString());
			this.Name = textObject.ToString();
		}

		// Token: 0x17000733 RID: 1843
		// (get) Token: 0x060015EA RID: 5610 RVA: 0x00056552 File Offset: 0x00054752
		// (set) Token: 0x060015EB RID: 5611 RVA: 0x0005655A File Offset: 0x0005475A
		[DataSourceProperty]
		public string StatId
		{
			get
			{
				return this._statId;
			}
			set
			{
				if (value != this._statId)
				{
					this._statId = value;
					base.OnPropertyChangedWithValue<string>(value, "StatId");
				}
			}
		}

		// Token: 0x17000734 RID: 1844
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x0005657D File Offset: 0x0005477D
		// (set) Token: 0x060015ED RID: 5613 RVA: 0x00056585 File Offset: 0x00054785
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

		// Token: 0x17000735 RID: 1845
		// (get) Token: 0x060015EE RID: 5614 RVA: 0x000565A8 File Offset: 0x000547A8
		// (set) Token: 0x060015EF RID: 5615 RVA: 0x000565B0 File Offset: 0x000547B0
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

		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x060015F0 RID: 5616 RVA: 0x000565D3 File Offset: 0x000547D3
		// (set) Token: 0x060015F1 RID: 5617 RVA: 0x000565DB File Offset: 0x000547DB
		[DataSourceProperty]
		public BasicTooltipViewModel Tooltip
		{
			get
			{
				return this._tooltip;
			}
			set
			{
				if (value != this._tooltip)
				{
					this._tooltip = value;
					base.OnPropertyChangedWithValue<BasicTooltipViewModel>(value, "Tooltip");
				}
			}
		}

		// Token: 0x040009E9 RID: 2537
		private readonly TextObject _nameTextObj;

		// Token: 0x040009EA RID: 2538
		private string _statId;

		// Token: 0x040009EB RID: 2539
		private string _name;

		// Token: 0x040009EC RID: 2540
		private string _valueText;

		// Token: 0x040009ED RID: 2541
		private BasicTooltipViewModel _tooltip;
	}
}
