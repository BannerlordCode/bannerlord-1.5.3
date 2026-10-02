using System;
using SandBox.View.Map;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x02000031 RID: 49
	[OverrideView(typeof(MapCampaignOptionsView))]
	public class GauntletMapCampaignOptionsView : MapCampaignOptionsView
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000EC68 File Offset: 0x0000CE68
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._dataSource = new CampaignOptionsVM(new Action(this.OnClose));
			base.Layer = new GauntletLayer("MapCampaignOptions", 4401, false)
			{
				IsFocusLayer = true
			};
			this._layerAsGauntletLayer = base.Layer as GauntletLayer;
			this._layerAsGauntletLayer.LoadMovie("CampaignOptions", this._dataSource);
			base.Layer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
			base.Layer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
			base.MapScreen.AddLayer(base.Layer);
			base.MapScreen.PauseAmbientSounds();
			ScreenManager.TrySetFocus(base.Layer);
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000ED2A File Offset: 0x0000CF2A
		private void OnClose()
		{
			MapScreen.Instance.CloseCampaignOptions();
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000ED36 File Offset: 0x0000CF36
		protected override void OnIdleTick(float dt)
		{
			base.OnFrameTick(dt);
			if (base.Layer.Input.IsHotKeyReleased("Exit"))
			{
				UISoundsHelper.PlayUISound("event:/ui/default");
				this._dataSource.ExecuteDone();
			}
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000ED6C File Offset: 0x0000CF6C
		protected override void OnFinalize()
		{
			base.OnFinalize();
			base.Layer.InputRestrictions.ResetInputRestrictions();
			base.MapScreen.RemoveLayer(base.Layer);
			base.MapScreen.RestartAmbientSounds();
			ScreenManager.TryLoseFocus(base.Layer);
			base.Layer = null;
			this._dataSource = null;
			this._layerAsGauntletLayer = null;
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000EDCB File Offset: 0x0000CFCB
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, true);
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000EDE7 File Offset: 0x0000CFE7
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			if (this._layerAsGauntletLayer != null)
			{
				ScreenManager.SetSuspendLayer(this._layerAsGauntletLayer, false);
			}
		}

		// Token: 0x040000D1 RID: 209
		private CampaignOptionsVM _dataSource;

		// Token: 0x040000D2 RID: 210
		private GauntletLayer _layerAsGauntletLayer;
	}
}
