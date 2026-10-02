using System;
using System.Collections.Generic;
using System.Threading;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000007 RID: 7
	public class MultiplayerGameLogger : GameHandler
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000018 RID: 24 RVA: 0x00002AD1 File Offset: 0x00000CD1
		public IReadOnlyList<GameLog> GameLogs
		{
			get
			{
				return this._gameLogs.AsReadOnly();
			}
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002ADE File Offset: 0x00000CDE
		public MultiplayerGameLogger()
		{
			this._lastLogId = 0;
			this._gameLogs = new List<GameLog>();
		}

		// Token: 0x0600001A RID: 26 RVA: 0x00002AF8 File Offset: 0x00000CF8
		public void Log(GameLog log)
		{
			log.Id = Interlocked.Increment(ref this._lastLogId);
			List<GameLog> gameLogs = this._gameLogs;
			if (gameLogs == null)
			{
				return;
			}
			gameLogs.Add(log);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002B1C File Offset: 0x00000D1C
		protected override void OnGameStart()
		{
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002B1E File Offset: 0x00000D1E
		public override void OnBeforeSave()
		{
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002B20 File Offset: 0x00000D20
		public override void OnAfterSave()
		{
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B24 File Offset: 0x00000D24
		protected override void OnGameNetworkBegin()
		{
			this._chatBox = Game.Current.GetGameHandler<ChatBox>();
			GameNetwork.NetworkMessageHandlerRegisterer networkMessageHandlerRegisterer = new GameNetwork.NetworkMessageHandlerRegisterer(GameNetwork.NetworkMessageHandlerRegisterer.RegisterMode.Add);
			if (GameNetwork.IsServer)
			{
				networkMessageHandlerRegisterer.Register<PlayerMessageAll>(new GameNetworkMessage.ClientMessageHandlerDelegate<PlayerMessageAll>(this.HandleClientEventPlayerMessageAll));
				networkMessageHandlerRegisterer.Register<PlayerMessageTeam>(new GameNetworkMessage.ClientMessageHandlerDelegate<PlayerMessageTeam>(this.HandleClientEventPlayerMessageTeam));
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002B74 File Offset: 0x00000D74
		private bool HandleClientEventPlayerMessageAll(NetworkCommunicator networkPeer, PlayerMessageAll message)
		{
			GameLog gameLog = new GameLog(GameLogType.ChatMessage, networkPeer.VirtualPlayer.Id, MBCommon.GetTotalMissionTime());
			gameLog.Data.Add("Message", message.Message);
			gameLog.Data.Add("IsTeam", false.ToString());
			Dictionary<string, string> data = gameLog.Data;
			string text = "IsMuted";
			ChatBox chatBox = this._chatBox;
			data.Add(text, ((chatBox != null) ? new bool?(chatBox.IsPlayerMuted(networkPeer.VirtualPlayer.Id)) : null).ToString());
			gameLog.Data.Add("IsGlobalMuted", networkPeer.IsMuted.ToString());
			this.Log(gameLog);
			return true;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002C38 File Offset: 0x00000E38
		private bool HandleClientEventPlayerMessageTeam(NetworkCommunicator networkPeer, PlayerMessageTeam message)
		{
			GameLog gameLog = new GameLog(GameLogType.ChatMessage, networkPeer.VirtualPlayer.Id, MBCommon.GetTotalMissionTime());
			gameLog.Data.Add("Message", message.Message);
			gameLog.Data.Add("IsTeam", true.ToString());
			Dictionary<string, string> data = gameLog.Data;
			string text = "IsMuted";
			ChatBox chatBox = this._chatBox;
			data.Add(text, ((chatBox != null) ? new bool?(chatBox.IsPlayerMuted(networkPeer.VirtualPlayer.Id)) : null).ToString());
			gameLog.Data.Add("IsGlobalMuted", networkPeer.IsMuted.ToString());
			this.Log(gameLog);
			return true;
		}

		// Token: 0x04000002 RID: 2
		public const int PreInitialLogId = 0;

		// Token: 0x04000003 RID: 3
		private ChatBox _chatBox;

		// Token: 0x04000004 RID: 4
		private int _lastLogId;

		// Token: 0x04000005 RID: 5
		private List<GameLog> _gameLogs;
	}
}
