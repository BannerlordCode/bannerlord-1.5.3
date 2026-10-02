using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Map.Menu.Overlay
{
	// Token: 0x02000128 RID: 296
	public class ArmyOverlayCohesionFillBarWidget : FillBarWidget
	{
		// Token: 0x06000FBC RID: 4028 RVA: 0x0002BBC2 File Offset: 0x00029DC2
		public ArmyOverlayCohesionFillBarWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x0002BBD2 File Offset: 0x00029DD2
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._isWarningDirty)
			{
				this.DetermineBarAnimState();
				this._isWarningDirty = false;
			}
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x0002BBF0 File Offset: 0x00029DF0
		private void DetermineBarAnimState()
		{
			BrushWidget brushWidget;
			if (base.FillWidget != null && (brushWidget = base.FillWidget as BrushWidget) != null)
			{
				brushWidget.RegisterBrushStatesOfWidget();
				if (this.IsCohesionWarningEnabled)
				{
					if (brushWidget.CurrentState == "WarningLeader")
					{
						brushWidget.BrushRenderer.RestartAnimation();
						return;
					}
					if (this.IsArmyLeader)
					{
						brushWidget.SetState("WarningLeader");
						return;
					}
					brushWidget.SetState("WarningNormal");
					return;
				}
				else
				{
					if (brushWidget.CurrentState == "Default")
					{
						brushWidget.BrushRenderer.RestartAnimation();
						return;
					}
					brushWidget.SetState("Default");
				}
			}
		}

		// Token: 0x17000598 RID: 1432
		// (get) Token: 0x06000FBF RID: 4031 RVA: 0x0002BC8C File Offset: 0x00029E8C
		// (set) Token: 0x06000FC0 RID: 4032 RVA: 0x0002BC94 File Offset: 0x00029E94
		[Editor(false)]
		public bool IsCohesionWarningEnabled
		{
			get
			{
				return this._isCohesionWarningEnabled;
			}
			set
			{
				if (value != this._isCohesionWarningEnabled)
				{
					this._isCohesionWarningEnabled = value;
					base.OnPropertyChanged(value, "IsCohesionWarningEnabled");
					this.DetermineBarAnimState();
					this._isWarningDirty = true;
				}
			}
		}

		// Token: 0x17000599 RID: 1433
		// (get) Token: 0x06000FC1 RID: 4033 RVA: 0x0002BCBF File Offset: 0x00029EBF
		// (set) Token: 0x06000FC2 RID: 4034 RVA: 0x0002BCC7 File Offset: 0x00029EC7
		[Editor(false)]
		public bool IsArmyLeader
		{
			get
			{
				return this._isArmyLeader;
			}
			set
			{
				if (value != this._isArmyLeader)
				{
					this._isArmyLeader = value;
					base.OnPropertyChanged(value, "IsArmyLeader");
					this.DetermineBarAnimState();
					this._isWarningDirty = true;
				}
			}
		}

		// Token: 0x04000733 RID: 1843
		private bool _isWarningDirty = true;

		// Token: 0x04000734 RID: 1844
		private bool _isCohesionWarningEnabled;

		// Token: 0x04000735 RID: 1845
		private bool _isArmyLeader;
	}
}
