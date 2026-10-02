using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets
{
	// Token: 0x02000043 RID: 67
	public class TabControlWidget : Widget
	{
		// Token: 0x060003D1 RID: 977 RVA: 0x0000C2CD File Offset: 0x0000A4CD
		public TabControlWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x0000C2D8 File Offset: 0x0000A4D8
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			if (!this.FirstButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnFirstButtonClick)))
			{
				this.FirstButton.ClickEventHandlers.Add(new Action<Widget>(this.OnFirstButtonClick));
			}
			if (!this.SecondButton.ClickEventHandlers.Contains(new Action<Widget>(this.OnSecondButtonClick)))
			{
				this.SecondButton.ClickEventHandlers.Add(new Action<Widget>(this.OnSecondButtonClick));
			}
			this.FirstButton.IsSelected = this.FirstItem.IsVisible;
			this.SecondButton.IsSelected = this.SecondItem.IsVisible;
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x0000C38C File Offset: 0x0000A58C
		public void OnFirstButtonClick(Widget widget)
		{
			if (!this._firstItem.IsVisible && this._secondItem.IsVisible)
			{
				this._secondItem.IsVisible = false;
				this._firstItem.IsVisible = true;
			}
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x0000C3C0 File Offset: 0x0000A5C0
		public void OnSecondButtonClick(Widget widget)
		{
			if (this._firstItem.IsVisible && !this._secondItem.IsVisible)
			{
				this._secondItem.IsVisible = true;
				this._firstItem.IsVisible = false;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060003D5 RID: 981 RVA: 0x0000C3F4 File Offset: 0x0000A5F4
		// (set) Token: 0x060003D6 RID: 982 RVA: 0x0000C3FC File Offset: 0x0000A5FC
		[Editor(false)]
		public ButtonWidget FirstButton
		{
			get
			{
				return this._firstButton;
			}
			set
			{
				if (this._firstButton != value)
				{
					this._firstButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "FirstButton");
				}
			}
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x0000C41A File Offset: 0x0000A61A
		// (set) Token: 0x060003D8 RID: 984 RVA: 0x0000C422 File Offset: 0x0000A622
		[Editor(false)]
		public ButtonWidget SecondButton
		{
			get
			{
				return this._secondButton;
			}
			set
			{
				if (this._secondButton != value)
				{
					this._secondButton = value;
					base.OnPropertyChanged<ButtonWidget>(value, "SecondButton");
				}
			}
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x0000C440 File Offset: 0x0000A640
		// (set) Token: 0x060003DA RID: 986 RVA: 0x0000C448 File Offset: 0x0000A648
		[Editor(false)]
		public Widget SecondItem
		{
			get
			{
				return this._secondItem;
			}
			set
			{
				if (this._secondItem != value)
				{
					this._secondItem = value;
					base.OnPropertyChanged<Widget>(value, "SecondItem");
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060003DB RID: 987 RVA: 0x0000C466 File Offset: 0x0000A666
		// (set) Token: 0x060003DC RID: 988 RVA: 0x0000C46E File Offset: 0x0000A66E
		[Editor(false)]
		public Widget FirstItem
		{
			get
			{
				return this._firstItem;
			}
			set
			{
				if (this._firstItem != value)
				{
					this._firstItem = value;
					base.OnPropertyChanged<Widget>(value, "FirstItem");
				}
			}
		}

		// Token: 0x04000197 RID: 407
		private ButtonWidget _firstButton;

		// Token: 0x04000198 RID: 408
		private ButtonWidget _secondButton;

		// Token: 0x04000199 RID: 409
		private Widget _firstItem;

		// Token: 0x0400019A RID: 410
		private Widget _secondItem;
	}
}
