using System;
using SandBox.View.Map;
using SandBox.ViewModelCollection.SaveLoad;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000040 RID: 64
	[OverrideView(typeof(MapSaveView))]
	public class GauntletMapSaveView : MapView
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x00011D68 File Offset: 0x0000FF68
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new MapSaveVM(new Action<bool>(this.OnStateChange));
			this._layerAsGauntletLayer = new GauntletLayer("MapSave", 10000, false);
			this._layerAsGauntletLayer.LoadMovie("MapSave", this._dataSource);
			base.Layer = this._layerAsGauntletLayer;
			base.Layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.MouseButtons | InputUsageMask.Keyboardkeys);
			base.MapScreen.AddLayer(base.Layer);
		}

		// Token: 0x060002FA RID: 762 RVA: 0x00011DF0 File Offset: 0x0000FFF0
		private void OnStateChange(bool isActive)
		{
			if (isActive)
			{
				base.Layer.IsFocusLayer = true;
				ScreenManager.TrySetFocus(base.Layer);
				base.Layer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.All);
				return;
			}
			base.Layer.IsFocusLayer = false;
			ScreenManager.TryLoseFocus(base.Layer);
			base.Layer.InputRestrictions.ResetInputRestrictions();
		}

		// Token: 0x060002FB RID: 763 RVA: 0x00011E51 File Offset: 0x00010051
		protected override void OnFinalize()
		{
			base.OnFinalize();
			this._dataSource.OnFinalize();
			base.MapScreen.RemoveLayer(base.Layer);
			base.Layer = null;
			this._dataSource = null;
		}

		// Token: 0x060002FC RID: 764 RVA: 0x00011E83 File Offset: 0x00010083
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x00011E9F File Offset: 0x0001009F
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x04000122 RID: 290
		private GauntletLayer _layerAsGauntletLayer;

		// Token: 0x04000123 RID: 291
		private MapSaveVM _dataSource;
	}
}
