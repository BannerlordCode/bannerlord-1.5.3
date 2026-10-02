using System;
using SandBox.View.Map;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200003F RID: 63
	[OverrideView(typeof(MapReadyView))]
	public class GauntletMapReadyView : MapReadyView
	{
		// Token: 0x060002F3 RID: 755 RVA: 0x00011C70 File Offset: 0x0000FE70
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new BoolItemWithActionVM(null, true, null);
			this._layerAsGauntletLayer = new GauntletLayer("MapReadyBlocker", 9999, false);
			this._layerAsGauntletLayer.LoadMovie("MapReadyBlocker", this._dataSource);
			base.Layer = this._layerAsGauntletLayer;
			base.MapScreen.AddLayer(base.Layer);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00011CDB File Offset: 0x0000FEDB
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._dataSource.OnFinalize();
			base.MapScreen.RemoveLayer(base.Layer);
			base.Layer = null;
			this._dataSource = null;
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00011D0D File Offset: 0x0000FF0D
		public override void SetIsMapSceneReady(bool isReady)
		{
			base.SetIsMapSceneReady(isReady);
			this._dataSource.IsActive = !isReady;
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00011D25 File Offset: 0x0000FF25
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00011D41 File Offset: 0x0000FF41
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x04000120 RID: 288
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x04000121 RID: 289
		private BoolItemWithActionVM _dataSource;
	}
}
