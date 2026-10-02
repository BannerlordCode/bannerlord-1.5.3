using System;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200029C RID: 668
	public class ReplayMissionLogic : MissionLogic
	{
		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06002530 RID: 9520 RVA: 0x000872CB File Offset: 0x000854CB
		// (set) Token: 0x06002531 RID: 9521 RVA: 0x000872D3 File Offset: 0x000854D3
		public string FileName { get; private set; }

		// Token: 0x06002532 RID: 9522 RVA: 0x000872DC File Offset: 0x000854DC
		public ReplayMissionLogic(bool isMultiplayer, string fileName = "")
		{
			if (!string.IsNullOrEmpty(fileName))
			{
				this.FileName = fileName;
			}
			this._isMultiplayer = isMultiplayer;
		}

		// Token: 0x06002533 RID: 9523 RVA: 0x000872FA File Offset: 0x000854FA
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			if (this._isMultiplayer)
			{
				GameNetwork.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			}
			MBCommon.CurrentGameType = MBCommon.GameType.SingleReplay;
			GameNetwork.InitializeClientSide(null, 0, -1, -1);
			base.Mission.Recorder.RestoreRecordFromFile(this.FileName);
		}

		// Token: 0x06002534 RID: 9524 RVA: 0x00087335 File Offset: 0x00085535
		public override void OnRemoveBehavior()
		{
			if (this._isMultiplayer)
			{
				GameNetwork.AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Remove);
				GameNetwork.EndReplay();
			}
			GameNetwork.TerminateClientSide();
			base.Mission.Recorder.ClearRecordBuffers();
			base.OnRemoveBehavior();
		}

		// Token: 0x04000E5A RID: 3674
		private bool _isMultiplayer;
	}
}
