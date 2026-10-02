using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000018 RID: 24
	public class DoubleTabControlListPanel : ListPanel
	{
		// Token: 0x0600014B RID: 331 RVA: 0x00005956 File Offset: 0x00003B56
		public DoubleTabControlListPanel(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0000595F File Offset: 0x00003B5F
		public void OnFirstTabClick(Widget widget)
		{
			if (!this._firstList.IsVisible && this._secondList.IsVisible)
			{
				this._secondList.IsVisible = false;
				this._firstList.IsVisible = true;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00005993 File Offset: 0x00003B93
		public void OnSecondTabClick(Widget widget)
		{
			if (this._firstList.IsVisible && !this._secondList.IsVisible)
			{
				this._secondList.IsVisible = true;
				this._firstList.IsVisible = false;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x0600014E RID: 334 RVA: 0x000059C7 File Offset: 0x00003BC7
		// (set) Token: 0x0600014F RID: 335 RVA: 0x000059D0 File Offset: 0x00003BD0
		[Editor(false)]
		public ButtonWidget FirstListButton
		{
			get
			{
				return this._firstListButton;
			}
			set
			{
				if (this._firstListButton != value)
				{
					this._firstListButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FirstListButton");
					if (this.FirstListButton != null && !this.FirstListButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnFirstTabClick)))
					{
						this.FirstListButton.ClickEventHandlers.Add(new Action<Widget>(this.OnFirstTabClick));
					}
				}
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x06000150 RID: 336 RVA: 0x00005A3B File Offset: 0x00003C3B
		// (set) Token: 0x06000151 RID: 337 RVA: 0x00005A44 File Offset: 0x00003C44
		[Editor(false)]
		public ButtonWidget SecondListButton
		{
			get
			{
				return this._secondListButton;
			}
			set
			{
				if (this._secondListButton != value)
				{
					this._secondListButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "SecondListButton");
					if (this.SecondListButton != null && !this.SecondListButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnSecondTabClick)))
					{
						this.SecondListButton.ClickEventHandlers.Add(new Action<Widget>(this.OnSecondTabClick));
					}
				}
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000152 RID: 338 RVA: 0x00005AAF File Offset: 0x00003CAF
		// (set) Token: 0x06000153 RID: 339 RVA: 0x00005AB7 File Offset: 0x00003CB7
		[Editor(false)]
		public Widget FirstList
		{
			get
			{
				return this._firstList;
			}
			set
			{
				if (this._firstList != value)
				{
					this._firstList = value;
					base.OnPropertyChanged<Widget>(value, "FirstList");
				}
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000154 RID: 340 RVA: 0x00005AD5 File Offset: 0x00003CD5
		// (set) Token: 0x06000155 RID: 341 RVA: 0x00005ADD File Offset: 0x00003CDD
		[Editor(false)]
		public Widget SecondList
		{
			get
			{
				return this._secondList;
			}
			set
			{
				if (this._secondList != value)
				{
					this._secondList = value;
					base.OnPropertyChanged<Widget>(value, "SecondList");
				}
			}
		}

		// Token: 0x0400009D RID: 157
		private ButtonWidget _firstListButton;

		// Token: 0x0400009E RID: 158
		private ButtonWidget _secondListButton;

		// Token: 0x0400009F RID: 159
		private Widget _firstList;

		// Token: 0x040000A0 RID: 160
		private Widget _secondList;
	}
}
