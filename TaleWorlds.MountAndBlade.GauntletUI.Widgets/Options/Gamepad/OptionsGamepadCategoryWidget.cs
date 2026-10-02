using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Options.Gamepad
{
	// Token: 0x0200007A RID: 122
	public class OptionsGamepadCategoryWidget : Widget
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00013B00 File Offset: 0x00011D00
		// (set) Token: 0x060006B7 RID: 1719 RVA: 0x00013B08 File Offset: 0x00011D08
		public Widget Playstation4LayoutParentWidget { get; set; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00013B11 File Offset: 0x00011D11
		// (set) Token: 0x060006B9 RID: 1721 RVA: 0x00013B19 File Offset: 0x00011D19
		public Widget Playstation5LayoutParentWidget { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x00013B22 File Offset: 0x00011D22
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x00013B2A File Offset: 0x00011D2A
		public Widget XboxLayoutParentWidget { get; set; }

		// Token: 0x060006BC RID: 1724 RVA: 0x00013B33 File Offset: 0x00011D33
		public OptionsGamepadCategoryWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00013B43 File Offset: 0x00011D43
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initalized)
			{
				this.SetGamepadLayoutVisibility(this.CurrentGamepadType);
				this._initalized = true;
			}
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00013B68 File Offset: 0x00011D68
		private void SetGamepadLayoutVisibility(int gamepadType)
		{
			this.XboxLayoutParentWidget.IsVisible = false;
			this.Playstation4LayoutParentWidget.IsVisible = false;
			this.Playstation5LayoutParentWidget.IsVisible = false;
			if (gamepadType == 0)
			{
				this.XboxLayoutParentWidget.IsVisible = true;
				return;
			}
			if (gamepadType == 1)
			{
				this.Playstation4LayoutParentWidget.IsVisible = true;
				return;
			}
			if (gamepadType == 2)
			{
				this.Playstation5LayoutParentWidget.IsVisible = true;
				return;
			}
			this.XboxLayoutParentWidget.IsVisible = true;
			Debug.FailedAssert("This kind of gamepad is not visually supported", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Options\\Gamepad\\OptionsGamepadCategoryWidget.cs", "SetGamepadLayoutVisibility", 47);
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x00013BED File Offset: 0x00011DED
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x00013BF5 File Offset: 0x00011DF5
		public int CurrentGamepadType
		{
			get
			{
				return this._currentGamepadType;
			}
			set
			{
				if (this._currentGamepadType != value)
				{
					this._currentGamepadType = value;
				}
			}
		}

		// Token: 0x040002DF RID: 735
		private bool _initalized;

		// Token: 0x040002E0 RID: 736
		private int _currentGamepadType = -1;
	}
}
