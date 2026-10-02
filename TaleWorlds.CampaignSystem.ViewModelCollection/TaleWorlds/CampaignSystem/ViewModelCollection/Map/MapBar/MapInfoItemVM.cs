using System;
using System.Collections.Generic;
using TaleWorlds.Core.ViewModelCollection.Information;
using TaleWorlds.Library;
using TaleWorlds.Library.Information;

namespace TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar
{
	// Token: 0x02000061 RID: 97
	public class MapInfoItemVM : ViewModel
	{
		// Token: 0x060006AD RID: 1709 RVA: 0x000219ED File Offset: 0x0001FBED
		public MapInfoItemVM(string itemId, Func<List<TooltipProperty>> getTooltip)
		{
			this.ItemId = itemId;
			this.VisualId = itemId;
			this._tooltip = new BasicTooltipViewModel(getTooltip);
			this.FloatValue = -1f;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00021A1A File Offset: 0x0001FC1A
		public MapInfoItemVM(string itemId, TooltipTriggerVM tooltipTrigger)
		{
			this.ItemId = itemId;
			this.VisualId = itemId;
			this._tooltipTrigger = tooltipTrigger;
			this.FloatValue = -1f;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x00021A42 File Offset: 0x0001FC42
		public void ExecuteBeginHint()
		{
			if (this._tooltip != null)
			{
				this._tooltip.ExecuteBeginHint();
				return;
			}
			if (this._tooltipTrigger != null)
			{
				this._tooltipTrigger.ExecuteBeginHint();
			}
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x00021A6B File Offset: 0x0001FC6B
		public void ExecuteEndHint()
		{
			if (this._tooltip != null)
			{
				this._tooltip.ExecuteEndHint();
				return;
			}
			if (this._tooltipTrigger != null)
			{
				this._tooltipTrigger.ExecuteEndHint();
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x00021A94 File Offset: 0x0001FC94
		public void SetOverriddenVisualId(string visualId)
		{
			this.VisualId = (string.IsNullOrEmpty(visualId) ? this.ItemId : visualId);
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x00021AAD File Offset: 0x0001FCAD
		// (set) Token: 0x060006B3 RID: 1715 RVA: 0x00021AB5 File Offset: 0x0001FCB5
		[DataSourceProperty]
		public bool HasWarning
		{
			get
			{
				return this._hasWarning;
			}
			set
			{
				if (value != this._hasWarning)
				{
					this._hasWarning = value;
					base.OnPropertyChangedWithValue(value, "HasWarning");
				}
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x00021AD3 File Offset: 0x0001FCD3
		// (set) Token: 0x060006B5 RID: 1717 RVA: 0x00021ADB File Offset: 0x0001FCDB
		[DataSourceProperty]
		public int IntValue
		{
			get
			{
				return this._intValue;
			}
			set
			{
				if (value != this._intValue)
				{
					this._intValue = value;
					base.OnPropertyChangedWithValue(value, "IntValue");
					this.FloatValue = (float)value;
				}
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00021B01 File Offset: 0x0001FD01
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00021B09 File Offset: 0x0001FD09
		[DataSourceProperty]
		public float FloatValue
		{
			get
			{
				return this._floatValue;
			}
			set
			{
				if (value != this._floatValue)
				{
					this._floatValue = value;
					base.OnPropertyChangedWithValue(value, "FloatValue");
					this.IntValue = (int)value;
				}
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00021B2F File Offset: 0x0001FD2F
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00021B37 File Offset: 0x0001FD37
		[DataSourceProperty]
		public string VisualId
		{
			get
			{
				return this._visualId;
			}
			set
			{
				if (value != this._visualId)
				{
					this._visualId = value;
					base.OnPropertyChangedWithValue<string>(value, "VisualId");
				}
			}
		}

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00021B5A File Offset: 0x0001FD5A
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x00021B62 File Offset: 0x0001FD62
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

		// Token: 0x040002D3 RID: 723
		public readonly string ItemId;

		// Token: 0x040002D4 RID: 724
		private readonly BasicTooltipViewModel _tooltip;

		// Token: 0x040002D5 RID: 725
		private readonly TooltipTriggerVM _tooltipTrigger;

		// Token: 0x040002D6 RID: 726
		private bool _hasWarning;

		// Token: 0x040002D7 RID: 727
		private int _intValue;

		// Token: 0x040002D8 RID: 728
		private float _floatValue;

		// Token: 0x040002D9 RID: 729
		private string _visualId;

		// Token: 0x040002DA RID: 730
		private string _value;
	}
}
