using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.ScreenSystem;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x0200002D RID: 45
	[OverrideView(typeof(MissionBoundaryCrossingView))]
	public class MissionGauntletBoundaryCrossingView : MissionBattleUIBaseView
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0000AFF8 File Offset: 0x000091F8
		protected override void OnCreateView()
		{
			this._dataSource = new BoundaryCrossingVM(base.Mission, new Action<bool>(this.OnEscapeMenuToggled));
			this._gauntletLayer = new GauntletLayer("BoundaryCrossing", 47, false);
			this._gauntletLayer.LoadMovie("BoundaryCrossing", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000B05D File Offset: 0x0000925D
		protected override void OnDestroyView()
		{
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000B078 File Offset: 0x00009278
		protected override void OnSuspendView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, true);
			}
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000B08E File Offset: 0x0000928E
		protected override void OnResumeView()
		{
			if (this._gauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, false);
			}
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000B0A4 File Offset: 0x000092A4
		private void OnEscapeMenuToggled(bool isOpened)
		{
			if (base.IsViewCreated)
			{
				ScreenManager.SetSuspendLayer(this._gauntletLayer, !isOpened);
			}
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000B0BD File Offset: 0x000092BD
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000B0E2 File Offset: 0x000092E2
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (base.IsViewCreated)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x040000F3 RID: 243
		private GauntletLayer _gauntletLayer;

		// Token: 0x040000F4 RID: 244
		private BoundaryCrossingVM _dataSource;
	}
}
