using System;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x0200001C RID: 28
	[OverrideView(typeof(MissionMultiplayerVoiceChatUIHandler))]
	public class MissionGauntletVoiceChat : MissionView
	{
		// Token: 0x0600013F RID: 319 RVA: 0x0000818E File Offset: 0x0000638E
		public MissionGauntletVoiceChat()
		{
			this.ViewOrderPriority = 60;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x000081A0 File Offset: 0x000063A0
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._dataSource = new MultiplayerVoiceChatVM(base.Mission);
			this._gauntletLayer = new GauntletLayer("MultiplayerVoiceChat", this.ViewOrderPriority, false);
			this._gauntletLayer.LoadMovie("MultiplayerVoiceChat", this._dataSource);
			base.MissionScreen.AddLayer(this._gauntletLayer);
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008203 File Offset: 0x00006403
		public override void OnMissionScreenFinalize()
		{
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._dataSource.OnFinalize();
			this._dataSource = null;
			this._gauntletLayer = null;
			base.OnMissionScreenFinalize();
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00008235 File Offset: 0x00006435
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this._dataSource.OnTick(dt);
		}

		// Token: 0x04000093 RID: 147
		private MultiplayerVoiceChatVM _dataSource;

		// Token: 0x04000094 RID: 148
		private GauntletLayer _gauntletLayer;
	}
}
