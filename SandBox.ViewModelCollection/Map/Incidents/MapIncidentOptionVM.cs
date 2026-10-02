using System;
using TaleWorlds.CampaignSystem.Incidents;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace SandBox.ViewModelCollection.Map.Incidents
{
	// Token: 0x0200004C RID: 76
	public class MapIncidentOptionVM : ViewModel
	{
		// Token: 0x060004C0 RID: 1216 RVA: 0x00012CC4 File Offset: 0x00010EC4
		public MapIncidentOptionVM(TextObject description, IncidentHint hint, int index, Action<MapIncidentOptionVM> onSelected, Action<MapIncidentOptionVM> onFocused)
		{
			this.Index = index;
			this._descriptionText = description;
			this._onSelected = onSelected;
			this._onFocused = onFocused;
			this.Hint = new MapIncidentHintVM(hint, false);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00012CF7 File Offset: 0x00010EF7
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Description = this._descriptionText.ToString();
			this.Hint.RefreshValues();
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00012D1B File Offset: 0x00010F1B
		public void ExecuteSelect()
		{
			this._onSelected(this);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00012D29 File Offset: 0x00010F29
		public void ExecuteFocus()
		{
			this._onFocused(this);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00012D37 File Offset: 0x00010F37
		public void ExecuteUnfocus()
		{
			this._onFocused(null);
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060004C5 RID: 1221 RVA: 0x00012D45 File Offset: 0x00010F45
		// (set) Token: 0x060004C6 RID: 1222 RVA: 0x00012D4D File Offset: 0x00010F4D
		[DataSourceProperty]
		public bool IsSelected
		{
			get
			{
				return this._isSelected;
			}
			set
			{
				if (value != this._isSelected)
				{
					this._isSelected = value;
					base.OnPropertyChangedWithValue(value, "IsSelected");
				}
			}
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x060004C7 RID: 1223 RVA: 0x00012D6B File Offset: 0x00010F6B
		// (set) Token: 0x060004C8 RID: 1224 RVA: 0x00012D73 File Offset: 0x00010F73
		[DataSourceProperty]
		public bool IsFocused
		{
			get
			{
				return this._isFocused;
			}
			set
			{
				if (value != this._isFocused)
				{
					this._isFocused = value;
					base.OnPropertyChangedWithValue(value, "IsFocused");
				}
			}
		}

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x060004C9 RID: 1225 RVA: 0x00012D91 File Offset: 0x00010F91
		// (set) Token: 0x060004CA RID: 1226 RVA: 0x00012D99 File Offset: 0x00010F99
		[DataSourceProperty]
		public string Description
		{
			get
			{
				return this._description;
			}
			set
			{
				if (value != this._description)
				{
					this._description = value;
					base.OnPropertyChangedWithValue<string>(value, "Description");
				}
			}
		}

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x060004CB RID: 1227 RVA: 0x00012DBC File Offset: 0x00010FBC
		// (set) Token: 0x060004CC RID: 1228 RVA: 0x00012DC4 File Offset: 0x00010FC4
		[DataSourceProperty]
		public MapIncidentHintVM Hint
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
					base.OnPropertyChangedWithValue<MapIncidentHintVM>(value, "Hint");
				}
			}
		}

		// Token: 0x04000265 RID: 613
		public readonly int Index;

		// Token: 0x04000266 RID: 614
		private readonly TextObject _descriptionText;

		// Token: 0x04000267 RID: 615
		private readonly Action<MapIncidentOptionVM> _onSelected;

		// Token: 0x04000268 RID: 616
		private readonly Action<MapIncidentOptionVM> _onFocused;

		// Token: 0x04000269 RID: 617
		private bool _isSelected;

		// Token: 0x0400026A RID: 618
		private bool _isFocused;

		// Token: 0x0400026B RID: 619
		private string _description;

		// Token: 0x0400026C RID: 620
		private MapIncidentHintVM _hint;
	}
}
