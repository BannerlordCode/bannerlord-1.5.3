using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000042 RID: 66
	public class SortButtonWidget : ButtonWidget
	{
		// Token: 0x060003CB RID: 971 RVA: 0x0000C1F4 File Offset: 0x0000A3F4
		public SortButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003CC RID: 972 RVA: 0x0000C200 File Offset: 0x0000A400
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.SortVisualWidget != null)
			{
				if (base.IsSelected)
				{
					switch (this.SortState)
					{
					case 0:
						this.SortVisualWidget.SetState("Default");
						return;
					case 1:
						this.SortVisualWidget.SetState("Ascending");
						return;
					case 2:
						this.SortVisualWidget.SetState("Descending");
						return;
					default:
						return;
					}
				}
				else
				{
					this.SortVisualWidget.SetState("Default");
				}
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060003CD RID: 973 RVA: 0x0000C281 File Offset: 0x0000A481
		// (set) Token: 0x060003CE RID: 974 RVA: 0x0000C289 File Offset: 0x0000A489
		[Editor(false)]
		public int SortState
		{
			get
			{
				return this._sortState;
			}
			set
			{
				if (this._sortState != value)
				{
					this._sortState = value;
					base.OnPropertyChanged(value, "SortState");
				}
			}
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060003CF RID: 975 RVA: 0x0000C2A7 File Offset: 0x0000A4A7
		// (set) Token: 0x060003D0 RID: 976 RVA: 0x0000C2AF File Offset: 0x0000A4AF
		[Editor(false)]
		public BrushWidget SortVisualWidget
		{
			get
			{
				return this._sortVisualWidget;
			}
			set
			{
				if (this._sortVisualWidget != value)
				{
					this._sortVisualWidget = value;
					base.OnPropertyChanged<BrushWidget>(value, "SortVisualWidget");
				}
			}
		}

		// Token: 0x04000195 RID: 405
		private int _sortState;

		// Token: 0x04000196 RID: 406
		private BrushWidget _sortVisualWidget;
	}
}
