using System;
using SandBox.View.Map;
using SandBox.View.Map.Navigation;
using TaleWorlds.CampaignSystem.ViewModelCollection.Map.MapBar;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.ScreenSystem;

namespace SandBox.GauntletUI.Map
{
	// Token: 0x0200002D RID: 45
	[OverrideView(typeof(MapBarView))]
	public class GauntletMapBarView : MapView
	{
		// Token: 0x0600022D RID: 557 RVA: 0x0000DDC4 File Offset: 0x0000BFC4
		protected override void OnMapConversationStart()
		{
			base.OnMapConversationStart();
			this._mapBarGlobalLayer.OnMapConversationStarted();
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000DDD7 File Offset: 0x0000BFD7
		protected override void OnMapConversationOver()
		{
			base.OnMapConversationOver();
			this._mapBarGlobalLayer.OnMapConversationOver();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000DDEA File Offset: 0x0000BFEA
		protected override void CreateLayout()
		{
			base.CreateLayout();
			this._mapBarGlobalLayer = new GauntletMapBarGlobalLayer(base.MapScreen, new MapNavigationHandler(), 8.5f);
			this._mapBarGlobalLayer.Initialize(new MapBarVM());
			ScreenManager.AddGlobalLayer(this._mapBarGlobalLayer, true);
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000DE29 File Offset: 0x0000C029
		protected override void OnFinalize()
		{
			this._mapBarGlobalLayer.OnFinalize();
			ScreenManager.RemoveGlobalLayer(this._mapBarGlobalLayer, true);
			base.OnFinalize();
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000DE48 File Offset: 0x0000C048
		protected override void OnResume()
		{
			base.OnResume();
			this._mapBarGlobalLayer.Refresh();
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000DE5B File Offset: 0x0000C05B
		protected override bool IsEscaped()
		{
			return this._mapBarGlobalLayer.IsEscaped();
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000DE68 File Offset: 0x0000C068
		protected override TutorialContexts GetTutorialContext()
		{
			if (this._mapBarGlobalLayer.IsInArmyManagement)
			{
				return TutorialContexts.ArmyManagement;
			}
			return base.GetTutorialContext();
		}

		// Token: 0x040000C0 RID: 192
		protected GauntletMapBarGlobalLayer _mapBarGlobalLayer;
	}
}
