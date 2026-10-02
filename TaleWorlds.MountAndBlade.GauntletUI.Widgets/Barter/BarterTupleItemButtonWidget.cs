using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Barter
{
	// Token: 0x02000194 RID: 404
	public class BarterTupleItemButtonWidget : ButtonWidget
	{
		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x060014F4 RID: 5364 RVA: 0x000394B0 File Offset: 0x000376B0
		// (set) Token: 0x060014F5 RID: 5365 RVA: 0x000394B8 File Offset: 0x000376B8
		public ListPanel SliderParentList { get; set; }

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x060014F6 RID: 5366 RVA: 0x000394C1 File Offset: 0x000376C1
		// (set) Token: 0x060014F7 RID: 5367 RVA: 0x000394C9 File Offset: 0x000376C9
		public TextWidget CountText { get; set; }

		// Token: 0x060014F8 RID: 5368 RVA: 0x000394D2 File Offset: 0x000376D2
		public BarterTupleItemButtonWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x000394DB File Offset: 0x000376DB
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.Refresh();
				this._initialized = true;
			}
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x000394FC File Offset: 0x000376FC
		private void Refresh()
		{
			this.SliderParentList.IsVisible = this.IsMultiple && this.IsOffered;
			this.CountText.IsHidden = this.IsMultiple && this.IsOffered;
			base.IsSelected = this.IsOffered;
			base.DoNotAcceptEvents = this.IsOffered;
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x060014FB RID: 5371 RVA: 0x00039559 File Offset: 0x00037759
		// (set) Token: 0x060014FC RID: 5372 RVA: 0x00039561 File Offset: 0x00037761
		[Editor(false)]
		public bool IsMultiple
		{
			get
			{
				return this._isMultiple;
			}
			set
			{
				if (this._isMultiple != value)
				{
					this._isMultiple = value;
					base.OnPropertyChanged(value, "IsMultiple");
					this.Refresh();
				}
			}
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x060014FD RID: 5373 RVA: 0x00039585 File Offset: 0x00037785
		// (set) Token: 0x060014FE RID: 5374 RVA: 0x0003958D File Offset: 0x0003778D
		[Editor(false)]
		public bool IsOffered
		{
			get
			{
				return this._isOffered;
			}
			set
			{
				if (this._isOffered != value)
				{
					this._isOffered = value;
					base.OnPropertyChanged(value, "IsOffered");
					this.Refresh();
				}
			}
		}

		// Token: 0x0400098D RID: 2445
		private bool _initialized;

		// Token: 0x0400098E RID: 2446
		private bool _isMultiple;

		// Token: 0x0400098F RID: 2447
		private bool _isOffered;
	}
}
