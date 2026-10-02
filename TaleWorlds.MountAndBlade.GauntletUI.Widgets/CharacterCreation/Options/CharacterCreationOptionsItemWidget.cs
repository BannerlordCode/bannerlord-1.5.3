using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.CharacterCreation.Options
{
	// Token: 0x0200018B RID: 395
	public class CharacterCreationOptionsItemWidget : Widget
	{
		// Token: 0x0600149F RID: 5279 RVA: 0x00038484 File Offset: 0x00036684
		public CharacterCreationOptionsItemWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x00038494 File Offset: 0x00036694
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isDirty)
			{
				if (this.Type == 0)
				{
					this.ActionOptionWidget.IsVisible = false;
					this.BooleanOptionWidget.IsVisible = true;
					this.SelectionOptionWidget.IsVisible = false;
					this.NumericOptionWidget.IsVisible = false;
				}
				else if (this.Type == 1)
				{
					this.ActionOptionWidget.IsVisible = false;
					this.BooleanOptionWidget.IsVisible = false;
					this.SelectionOptionWidget.IsVisible = false;
					this.NumericOptionWidget.IsVisible = true;
				}
				else if (this.Type == 2)
				{
					this.ActionOptionWidget.IsVisible = false;
					this.BooleanOptionWidget.IsVisible = false;
					this.SelectionOptionWidget.IsVisible = true;
					this.NumericOptionWidget.IsVisible = false;
				}
				else if (this.Type == 3)
				{
					this.ActionOptionWidget.IsVisible = true;
					this.BooleanOptionWidget.IsVisible = false;
					this.SelectionOptionWidget.IsVisible = false;
					this.NumericOptionWidget.IsVisible = false;
				}
				this.ResetNavigationIndices();
				this._isDirty = false;
			}
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x000385AC File Offset: 0x000367AC
		private void ResetNavigationIndices()
		{
			if (base.GamepadNavigationIndex == -1)
			{
				return;
			}
			bool flag = false;
			Widget booleanOptionWidget = this.BooleanOptionWidget;
			if (booleanOptionWidget != null && booleanOptionWidget.IsVisible)
			{
				this.BooleanOptionWidget.GamepadNavigationIndex = base.GamepadNavigationIndex;
				flag = true;
			}
			else
			{
				Widget numericOptionWidget = this.NumericOptionWidget;
				if (numericOptionWidget != null && numericOptionWidget.IsVisible)
				{
					this.NumericOptionWidget.GamepadNavigationIndex = base.GamepadNavigationIndex;
					flag = true;
				}
				else
				{
					Widget selectionOptionWidget = this.SelectionOptionWidget;
					if (selectionOptionWidget != null && selectionOptionWidget.IsVisible)
					{
						this.SelectionOptionWidget.GamepadNavigationIndex = base.GamepadNavigationIndex;
						flag = true;
					}
					else
					{
						Widget actionOptionWidget = this.ActionOptionWidget;
						if (actionOptionWidget != null && actionOptionWidget.IsVisible)
						{
							this.ActionOptionWidget.GamepadNavigationIndex = base.GamepadNavigationIndex;
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				base.GamepadNavigationIndex = -1;
			}
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x00038671 File Offset: 0x00036871
		protected override void OnGamepadNavigationIndexUpdated(int newIndex)
		{
			base.OnGamepadNavigationIndexUpdated(newIndex);
			this.ResetNavigationIndices();
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x060014A3 RID: 5283 RVA: 0x00038680 File Offset: 0x00036880
		// (set) Token: 0x060014A4 RID: 5284 RVA: 0x00038688 File Offset: 0x00036888
		[Editor(false)]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged(value, "Type");
					this._isDirty = true;
				}
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x060014A5 RID: 5285 RVA: 0x000386AD File Offset: 0x000368AD
		// (set) Token: 0x060014A6 RID: 5286 RVA: 0x000386B5 File Offset: 0x000368B5
		[Editor(false)]
		public Widget ActionOptionWidget
		{
			get
			{
				return this._actionOptionWidget;
			}
			set
			{
				if (this._actionOptionWidget != value)
				{
					this._actionOptionWidget = value;
					base.OnPropertyChanged<Widget>(value, "ActionOptionWidget");
				}
			}
		}

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x060014A7 RID: 5287 RVA: 0x000386D3 File Offset: 0x000368D3
		// (set) Token: 0x060014A8 RID: 5288 RVA: 0x000386DB File Offset: 0x000368DB
		[Editor(false)]
		public Widget NumericOptionWidget
		{
			get
			{
				return this._numericOptionWidget;
			}
			set
			{
				if (this._numericOptionWidget != value)
				{
					this._numericOptionWidget = value;
					base.OnPropertyChanged<Widget>(value, "NumericOptionWidget");
				}
			}
		}

		// Token: 0x17000751 RID: 1873
		// (get) Token: 0x060014A9 RID: 5289 RVA: 0x000386F9 File Offset: 0x000368F9
		// (set) Token: 0x060014AA RID: 5290 RVA: 0x00038701 File Offset: 0x00036901
		[Editor(false)]
		public Widget SelectionOptionWidget
		{
			get
			{
				return this._selectionOptionWidget;
			}
			set
			{
				if (this._selectionOptionWidget != value)
				{
					this._selectionOptionWidget = value;
					base.OnPropertyChanged<Widget>(value, "SelectionOptionWidget");
				}
			}
		}

		// Token: 0x17000752 RID: 1874
		// (get) Token: 0x060014AB RID: 5291 RVA: 0x0003871F File Offset: 0x0003691F
		// (set) Token: 0x060014AC RID: 5292 RVA: 0x00038727 File Offset: 0x00036927
		[Editor(false)]
		public Widget BooleanOptionWidget
		{
			get
			{
				return this._booleanOptionWidget;
			}
			set
			{
				if (this._booleanOptionWidget != value)
				{
					this._booleanOptionWidget = value;
					base.OnPropertyChanged<Widget>(value, "BooleanOptionWidget");
				}
			}
		}

		// Token: 0x04000965 RID: 2405
		private bool _isDirty = true;

		// Token: 0x04000966 RID: 2406
		private int _type;

		// Token: 0x04000967 RID: 2407
		private Widget _actionOptionWidget;

		// Token: 0x04000968 RID: 2408
		private Widget _numericOptionWidget;

		// Token: 0x04000969 RID: 2409
		private Widget _selectionOptionWidget;

		// Token: 0x0400096A RID: 2410
		private Widget _booleanOptionWidget;
	}
}
