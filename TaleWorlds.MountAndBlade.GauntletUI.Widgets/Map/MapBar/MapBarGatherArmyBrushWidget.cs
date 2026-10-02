using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.MapBar
{
	// Token: 0x0200012D RID: 301
	public class MapBarGatherArmyBrushWidget : BrushWidget
	{
		// Token: 0x06000FE0 RID: 4064 RVA: 0x0002C0AA File Offset: 0x0002A2AA
		public MapBarGatherArmyBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FE1 RID: 4065 RVA: 0x0002C0B3 File Offset: 0x0002A2B3
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (!this._initialized)
			{
				this.UpdateVisualState();
				this._initialized = true;
			}
		}

		// Token: 0x06000FE2 RID: 4066 RVA: 0x0002C0D4 File Offset: 0x0002A2D4
		private void UpdateVisualState()
		{
			base.IsEnabled = this.IsGatherArmyVisible;
			if (!this.IsGatherArmyVisible)
			{
				this.SetState("Disabled");
				return;
			}
			if (this._isInfoBarExtended)
			{
				this.SetState("Extended");
				return;
			}
			this.SetState("Default");
		}

		// Token: 0x06000FE3 RID: 4067 RVA: 0x0002C120 File Offset: 0x0002A320
		private void OnMapInfoBarExtendStateChange(bool newState)
		{
			this._isInfoBarExtended = newState;
			this.UpdateVisualState();
		}

		// Token: 0x170005A3 RID: 1443
		// (get) Token: 0x06000FE4 RID: 4068 RVA: 0x0002C12F File Offset: 0x0002A32F
		// (set) Token: 0x06000FE5 RID: 4069 RVA: 0x0002C137 File Offset: 0x0002A337
		public MapInfoBarWidget InfoBarWidget
		{
			get
			{
				return this._infoBarWidget;
			}
			set
			{
				if (this._infoBarWidget != value)
				{
					this._infoBarWidget = value;
					this._infoBarWidget.OnMapInfoBarExtendStateChange += this.OnMapInfoBarExtendStateChange;
				}
			}
		}

		// Token: 0x170005A4 RID: 1444
		// (get) Token: 0x06000FE6 RID: 4070 RVA: 0x0002C160 File Offset: 0x0002A360
		// (set) Token: 0x06000FE7 RID: 4071 RVA: 0x0002C168 File Offset: 0x0002A368
		public bool IsGatherArmyEnabled
		{
			get
			{
				return this._isGatherArmyEnabled;
			}
			set
			{
				if (this._isGatherArmyEnabled != value)
				{
					this._isGatherArmyEnabled = value;
					this.UpdateVisualState();
				}
			}
		}

		// Token: 0x170005A5 RID: 1445
		// (get) Token: 0x06000FE8 RID: 4072 RVA: 0x0002C180 File Offset: 0x0002A380
		// (set) Token: 0x06000FE9 RID: 4073 RVA: 0x0002C188 File Offset: 0x0002A388
		public bool IsGatherArmyVisible
		{
			get
			{
				return this._isGatherArmyVisible;
			}
			set
			{
				if (this._isGatherArmyVisible != value)
				{
					this._isGatherArmyVisible = value;
					this.UpdateVisualState();
				}
			}
		}

		// Token: 0x0400073F RID: 1855
		private bool _isInfoBarExtended;

		// Token: 0x04000740 RID: 1856
		private bool _initialized;

		// Token: 0x04000741 RID: 1857
		private MapInfoBarWidget _infoBarWidget;

		// Token: 0x04000742 RID: 1858
		private bool _isGatherArmyEnabled;

		// Token: 0x04000743 RID: 1859
		private bool _isGatherArmyVisible;
	}
}
