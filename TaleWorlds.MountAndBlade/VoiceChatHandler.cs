using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromClient;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.Options;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Network.Messages;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D2 RID: 722
	public class VoiceChatHandler : MissionNetwork
	{
		// Token: 0x14000079 RID: 121
		// (add) Token: 0x060029E5 RID: 10725 RVA: 0x0009DAD0 File Offset: 0x0009BCD0
		// (remove) Token: 0x060029E6 RID: 10726 RVA: 0x0009DB08 File Offset: 0x0009BD08
		public event Action OnVoiceRecordStarted;

		// Token: 0x1400007A RID: 122
		// (add) Token: 0x060029E7 RID: 10727 RVA: 0x0009DB40 File Offset: 0x0009BD40
		// (remove) Token: 0x060029E8 RID: 10728 RVA: 0x0009DB78 File Offset: 0x0009BD78
		public event Action OnVoiceRecordStopped;

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x060029E9 RID: 10729 RVA: 0x0009DBB0 File Offset: 0x0009BDB0
		// (remove) Token: 0x060029EA RID: 10730 RVA: 0x0009DBE8 File Offset: 0x0009BDE8
		public event Action<MissionPeer, bool> OnPeerVoiceStatusUpdated;

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x060029EB RID: 10731 RVA: 0x0009DC20 File Offset: 0x0009BE20
		// (remove) Token: 0x060029EC RID: 10732 RVA: 0x0009DC58 File Offset: 0x0009BE58
		public event Action<MissionPeer> OnPeerMuteStatusUpdated;

		// Token: 0x170007E9 RID: 2025
		// (get) Token: 0x060029ED RID: 10733 RVA: 0x0009DC8D File Offset: 0x0009BE8D
		// (set) Token: 0x060029EE RID: 10734 RVA: 0x0009DC98 File Offset: 0x0009BE98
		private bool IsVoiceRecordActive
		{
			get
			{
				return this._isVoiceRecordActive;
			}
			set
			{
				if (!this._isVoiceChatDisabled)
				{
					this._isVoiceRecordActive = value;
					if (this._isVoiceRecordActive)
					{
						SoundManager.StartVoiceRecording();
						Action onVoiceRecordStarted = this.OnVoiceRecordStarted;
						if (onVoiceRecordStarted == null)
						{
							return;
						}
						onVoiceRecordStarted();
						return;
					}
					else
					{
						SoundManager.StopVoiceRecording();
						Action onVoiceRecordStopped = this.OnVoiceRecordStopped;
						if (onVoiceRecordStopped == null)
						{
							return;
						}
						onVoiceRecordStopped();
					}
				}
			}
		}

		// Token: 0x060029EF RID: 10735 RVA: 0x0009DCE7 File Offset: 0x0009BEE7
		protected override void AddRemoveMessageHandlers(GameNetwork.NetworkMessageHandlerRegistererContainer registerer)
		{
			if (GameNetwork.IsClient)
			{
				registerer.RegisterBaseHandler<SendVoiceToPlay>(new GameNetworkMessage.ServerMessageHandlerDelegate<GameNetworkMessage>(this.HandleServerEventSendVoiceToPlay));
				return;
			}
			if (GameNetwork.IsServer)
			{
				registerer.RegisterBaseHandler<SendVoiceRecord>(new GameNetworkMessage.ClientMessageHandlerDelegate<GameNetworkMessage>(this.HandleClientEventSendVoiceRecord));
			}
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x0009DD1C File Offset: 0x0009BF1C
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			if (!GameNetwork.IsDedicatedServer)
			{
				this._playerVoiceDataList = new List<VoiceChatHandler.PeerVoiceData>();
				SoundManager.InitializeVoicePlayEvent();
				this._voiceToSend = new Queue<byte>();
			}
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x0009DD48 File Offset: 0x0009BF48
		public override void AfterStart()
		{
			this.UpdateVoiceChatEnabled();
			if (!this._isVoiceChatDisabled)
			{
				MissionPeer.OnTeamChanged += this.MissionPeerOnTeamChanged;
				Mission.Current.GetMissionBehavior<MissionNetworkComponent>().OnClientSynchronizedEvent += this.OnPlayerSynchronized;
			}
			NativeOptions.OnNativeOptionChanged = (NativeOptions.OnNativeOptionChangedDelegate)Delegate.Combine(NativeOptions.OnNativeOptionChanged, new NativeOptions.OnNativeOptionChangedDelegate(this.OnNativeOptionChanged));
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Combine(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x0009DDD0 File Offset: 0x0009BFD0
		public override void OnRemoveBehavior()
		{
			if (!this._isVoiceChatDisabled)
			{
				MissionPeer.OnTeamChanged -= this.MissionPeerOnTeamChanged;
			}
			if (!GameNetwork.IsDedicatedServer)
			{
				if (this.IsVoiceRecordActive)
				{
					this.IsVoiceRecordActive = false;
				}
				SoundManager.FinalizeVoicePlayEvent();
			}
			NativeOptions.OnNativeOptionChanged = (NativeOptions.OnNativeOptionChangedDelegate)Delegate.Remove(NativeOptions.OnNativeOptionChanged, new NativeOptions.OnNativeOptionChangedDelegate(this.OnNativeOptionChanged));
			ManagedOptions.OnManagedOptionChanged = (ManagedOptions.OnManagedOptionChangedDelegate)Delegate.Remove(ManagedOptions.OnManagedOptionChanged, new ManagedOptions.OnManagedOptionChangedDelegate(this.OnManagedOptionChanged));
			base.OnRemoveBehavior();
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x0009DE57 File Offset: 0x0009C057
		public override void OnPreDisplayMissionTick(float dt)
		{
			if (!GameNetwork.IsDedicatedServer && !this._isVoiceChatDisabled)
			{
				this.VoiceTick(dt);
			}
		}

		// Token: 0x060029F4 RID: 10740 RVA: 0x0009DE70 File Offset: 0x0009C070
		private bool HandleClientEventSendVoiceRecord(NetworkCommunicator peer, GameNetworkMessage baseMessage)
		{
			SendVoiceRecord sendVoiceRecord = (SendVoiceRecord)baseMessage;
			MissionPeer component = peer.GetComponent<MissionPeer>();
			if (sendVoiceRecord.BufferLength > 0 && component.Team != null)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
					if (networkCommunicator.IsSynchronized && component2 != null && component2.Team == component.Team && (sendVoiceRecord.ReceiverList == null || sendVoiceRecord.ReceiverList.Contains(networkCommunicator.VirtualPlayer)) && component2 != component)
					{
						GameNetwork.BeginModuleEventAsServerUnreliable(component2.Peer);
						GameNetwork.WriteMessage(new SendVoiceToPlay(peer, sendVoiceRecord.Buffer, sendVoiceRecord.BufferLength));
						GameNetwork.EndModuleEventAsServerUnreliable();
					}
				}
			}
			return true;
		}

		// Token: 0x060029F5 RID: 10741 RVA: 0x0009DF4C File Offset: 0x0009C14C
		private void HandleServerEventSendVoiceToPlay(GameNetworkMessage baseMessage)
		{
			SendVoiceToPlay sendVoiceToPlay = (SendVoiceToPlay)baseMessage;
			if (!this._isVoiceChatDisabled)
			{
				MissionPeer component = sendVoiceToPlay.Peer.GetComponent<MissionPeer>();
				if (component != null && sendVoiceToPlay.BufferLength > 0 && !component.IsMutedFromGameOrPlatform && !MultiplayerGlobalMutedPlayersManager.IsUserMuted(component.Peer.Id))
				{
					for (int i = 0; i < this._playerVoiceDataList.Count; i++)
					{
						if (this._playerVoiceDataList[i].Peer == component)
						{
							byte[] array = new byte[8640];
							int num;
							this.DecompressVoiceChunk(sendVoiceToPlay.Peer.Index, sendVoiceToPlay.Buffer, sendVoiceToPlay.BufferLength, ref array, out num);
							this._playerVoiceDataList[i].WriteVoiceData(array, num);
							return;
						}
					}
				}
			}
		}

		// Token: 0x060029F6 RID: 10742 RVA: 0x0009E00E File Offset: 0x0009C20E
		private void CheckStopVoiceRecord()
		{
			if (this._stopRecordingOnNextTick)
			{
				this.IsVoiceRecordActive = false;
				this._stopRecordingOnNextTick = false;
			}
		}

		// Token: 0x060029F7 RID: 10743 RVA: 0x0009E028 File Offset: 0x0009C228
		private void VoiceTick(float dt)
		{
			int num = 120;
			if (this._playedAnyVoicePreviousTick)
			{
				int num2 = MathF.Ceiling(dt * 1000f);
				num = MathF.Min(num, num2);
				this._playedAnyVoicePreviousTick = false;
			}
			foreach (VoiceChatHandler.PeerVoiceData peerVoiceData in this._playerVoiceDataList)
			{
				Action<MissionPeer, bool> onPeerVoiceStatusUpdated = this.OnPeerVoiceStatusUpdated;
				if (onPeerVoiceStatusUpdated != null)
				{
					onPeerVoiceStatusUpdated(peerVoiceData.Peer, peerVoiceData.HasAnyVoiceData());
				}
			}
			int num3 = num * 12;
			for (int i = 0; i < num3; i++)
			{
				for (int j = 0; j < this._playerVoiceDataList.Count; j++)
				{
					this._playerVoiceDataList[j].ProcessVoiceData();
				}
			}
			for (int k = 0; k < this._playerVoiceDataList.Count; k++)
			{
				Queue<short> voiceToPlayForTick = this._playerVoiceDataList[k].GetVoiceToPlayForTick();
				if (voiceToPlayForTick.Count > 0)
				{
					int count = voiceToPlayForTick.Count;
					byte[] array = new byte[count * 2];
					for (int l = 0; l < count; l++)
					{
						byte[] bytes = BitConverter.GetBytes(voiceToPlayForTick.Dequeue());
						array[l * 2] = bytes[0];
						array[l * 2 + 1] = bytes[1];
					}
					SoundManager.UpdateVoiceToPlay(array, array.Length, k);
					this._playedAnyVoicePreviousTick = true;
				}
			}
			if (this.IsVoiceRecordActive)
			{
				byte[] array2 = new byte[72000];
				int num4;
				SoundManager.GetVoiceData(array2, 72000, out num4);
				for (int m = 0; m < num4; m++)
				{
					this._voiceToSend.Enqueue(array2[m]);
				}
				this.CheckStopVoiceRecord();
			}
			while (this._voiceToSend.Count > 0 && (this._voiceToSend.Count >= 1440 || !this.IsVoiceRecordActive))
			{
				int num5 = MathF.Min(this._voiceToSend.Count, 1440);
				byte[] array3 = new byte[1440];
				for (int n = 0; n < num5; n++)
				{
					array3[n] = this._voiceToSend.Dequeue();
				}
				if (GameNetwork.IsClient)
				{
					byte[] array4 = new byte[8640];
					int num6;
					this.CompressVoiceChunk(0, array3, ref array4, out num6);
					GameNetwork.BeginModuleEventAsClientUnreliable();
					GameNetwork.WriteMessage(new SendVoiceRecord(array4, num6));
					GameNetwork.EndModuleEventAsClientUnreliable();
				}
				else if (GameNetwork.IsServer)
				{
					VoiceChatHandler.<>c__DisplayClass38_0 CS$<>8__locals1 = new VoiceChatHandler.<>c__DisplayClass38_0();
					VoiceChatHandler.<>c__DisplayClass38_0 CS$<>8__locals2 = CS$<>8__locals1;
					NetworkCommunicator myPeer = GameNetwork.MyPeer;
					CS$<>8__locals2.myMissionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
					if (CS$<>8__locals1.myMissionPeer != null)
					{
						this._playerVoiceDataList.Single<VoiceChatHandler.PeerVoiceData>((VoiceChatHandler.PeerVoiceData x) => x.Peer == CS$<>8__locals1.myMissionPeer).WriteVoiceData(array3, num5);
					}
				}
			}
			if (!this.IsVoiceRecordActive && base.Mission.InputManager.IsGameKeyPressed(33))
			{
				NetworkCommunicator myPeer2 = GameNetwork.MyPeer;
				MissionPeer missionPeer = ((myPeer2 != null) ? myPeer2.GetComponent<MissionPeer>() : null);
				if (missionPeer != null && missionPeer.Team != null && missionPeer.Team != Mission.Current.SpectatorTeam)
				{
					this.IsVoiceRecordActive = true;
				}
			}
			if (this.IsVoiceRecordActive && base.Mission.InputManager.IsGameKeyReleased(33))
			{
				this._stopRecordingOnNextTick = true;
			}
		}

		// Token: 0x060029F8 RID: 10744 RVA: 0x0009E360 File Offset: 0x0009C560
		private void DecompressVoiceChunk(int clientID, byte[] compressedVoiceBuffer, int compressedBufferLength, ref byte[] voiceBuffer, out int bufferLength)
		{
			SoundManager.DecompressData(clientID, compressedVoiceBuffer, compressedBufferLength, voiceBuffer, out bufferLength);
		}

		// Token: 0x060029F9 RID: 10745 RVA: 0x0009E36F File Offset: 0x0009C56F
		private void CompressVoiceChunk(int clientIndex, byte[] voiceBuffer, ref byte[] compressedBuffer, out int compressedBufferLength)
		{
			SoundManager.CompressData(clientIndex, voiceBuffer, 1440, compressedBuffer, out compressedBufferLength);
		}

		// Token: 0x060029FA RID: 10746 RVA: 0x0009E384 File Offset: 0x0009C584
		private VoiceChatHandler.PeerVoiceData GetPlayerVoiceData(MissionPeer missionPeer)
		{
			for (int i = 0; i < this._playerVoiceDataList.Count; i++)
			{
				if (this._playerVoiceDataList[i].Peer == missionPeer)
				{
					return this._playerVoiceDataList[i];
				}
			}
			return null;
		}

		// Token: 0x060029FB RID: 10747 RVA: 0x0009E3CC File Offset: 0x0009C5CC
		private void AddPlayerToVoiceChat(MissionPeer missionPeer)
		{
			VirtualPlayer peer = missionPeer.Peer;
			this._playerVoiceDataList.Add(new VoiceChatHandler.PeerVoiceData(missionPeer));
			SoundManager.CreateVoiceEvent();
			PlatformServices.Instance.CheckPermissionWithUser(Permission.CommunicateUsingVoice, missionPeer.Peer.Id, delegate(bool hasPermission)
			{
				if (Mission.Current != null && Mission.Current.CurrentState == Mission.State.Continuing)
				{
					VoiceChatHandler.PeerVoiceData playerVoiceData = this.GetPlayerVoiceData(missionPeer);
					if (playerVoiceData != null)
					{
						if (!hasPermission)
						{
							PlayerIdProvidedTypes providedType = missionPeer.Peer.Id.ProvidedType;
							LobbyClient gameClient = NetworkMain.GameClient;
							PlayerIdProvidedTypes? playerIdProvidedTypes = ((gameClient != null) ? new PlayerIdProvidedTypes?(gameClient.PlayerID.ProvidedType) : null);
							if ((providedType == playerIdProvidedTypes.GetValueOrDefault()) & (playerIdProvidedTypes != null))
							{
								missionPeer.SetMutedFromPlatform(true);
							}
						}
						playerVoiceData.SetReadyOnPlatform();
					}
				}
			});
			missionPeer.SetMuted(PermaMuteList.IsPlayerMuted(missionPeer.Peer.Id) || MultiplayerGlobalMutedPlayersManager.IsUserMuted(missionPeer.Peer.Id));
			SoundManager.AddSoundClientWithId((ulong)((long)peer.Index));
			Action<MissionPeer> onPeerMuteStatusUpdated = this.OnPeerMuteStatusUpdated;
			if (onPeerMuteStatusUpdated == null)
			{
				return;
			}
			onPeerMuteStatusUpdated(missionPeer);
		}

		// Token: 0x060029FC RID: 10748 RVA: 0x0009E498 File Offset: 0x0009C698
		private void RemovePlayerFromVoiceChat(int indexInVoiceDataList)
		{
			VirtualPlayer peer = this._playerVoiceDataList[indexInVoiceDataList].Peer.Peer;
			SoundManager.DeleteSoundClientWithId((ulong)((long)this._playerVoiceDataList[indexInVoiceDataList].Peer.Peer.Index));
			SoundManager.DestroyVoiceEvent(indexInVoiceDataList);
			this._playerVoiceDataList.RemoveAt(indexInVoiceDataList);
		}

		// Token: 0x060029FD RID: 10749 RVA: 0x0009E4EF File Offset: 0x0009C6EF
		private void MissionPeerOnTeamChanged(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (this._localUserInitialized && peer.VirtualPlayer.Id != PlayerId.Empty)
			{
				this.CheckPlayerForVoiceChatOnTeamChange(peer, previousTeam, newTeam);
			}
		}

		// Token: 0x060029FE RID: 10750 RVA: 0x0009E51C File Offset: 0x0009C71C
		private void OnPlayerSynchronized(NetworkCommunicator networkPeer)
		{
			if (this._localUserInitialized)
			{
				MissionPeer component = networkPeer.GetComponent<MissionPeer>();
				if (!component.IsMine && component.Team != null)
				{
					this.CheckPlayerForVoiceChatOnTeamChange(networkPeer, null, component.Team);
					return;
				}
			}
			else if (networkPeer.IsMine)
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
				this.CheckPlayerForVoiceChatOnTeamChange(GameNetwork.MyPeer, null, missionPeer.Team);
			}
		}

		// Token: 0x060029FF RID: 10751 RVA: 0x0009E584 File Offset: 0x0009C784
		private void CheckPlayerForVoiceChatOnTeamChange(NetworkCommunicator peer, Team previousTeam, Team newTeam)
		{
			if (GameNetwork.VirtualPlayers[peer.Index] == peer.VirtualPlayer)
			{
				NetworkCommunicator myPeer = GameNetwork.MyPeer;
				MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
				if (missionPeer != null)
				{
					MissionPeer component = peer.GetComponent<MissionPeer>();
					if (missionPeer == component)
					{
						this._localUserInitialized = true;
						for (int i = this._playerVoiceDataList.Count - 1; i >= 0; i--)
						{
							this.RemovePlayerFromVoiceChat(i);
						}
						if (newTeam == null || newTeam == Mission.Current.SpectatorTeam)
						{
							return;
						}
						using (List<NetworkCommunicator>.Enumerator enumerator = GameNetwork.NetworkPeers.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								NetworkCommunicator networkCommunicator = enumerator.Current;
								MissionPeer component2 = networkCommunicator.GetComponent<MissionPeer>();
								if (missionPeer != component2 && ((component2 != null) ? component2.Team : null) != null && component2.Team == newTeam && networkCommunicator.VirtualPlayer.Id != PlayerId.Empty)
								{
									this.AddPlayerToVoiceChat(component2);
								}
							}
							return;
						}
					}
					if (this._localUserInitialized && missionPeer.Team != null)
					{
						if (missionPeer.Team == previousTeam)
						{
							for (int j = 0; j < this._playerVoiceDataList.Count; j++)
							{
								if (this._playerVoiceDataList[j].Peer == component)
								{
									this.RemovePlayerFromVoiceChat(j);
									return;
								}
							}
							return;
						}
						if (missionPeer.Team != Mission.Current.SpectatorTeam && missionPeer.Team == newTeam)
						{
							this.AddPlayerToVoiceChat(component);
						}
					}
				}
			}
		}

		// Token: 0x06002A00 RID: 10752 RVA: 0x0009E708 File Offset: 0x0009C908
		private void UpdateVoiceChatEnabled()
		{
			float num = 1f;
			this._isVoiceChatDisabled = !BannerlordConfig.EnableVoiceChat || num <= 1E-05f || Game.Current.GetGameHandler<ChatBox>().IsContentRestricted;
		}

		// Token: 0x06002A01 RID: 10753 RVA: 0x0009E742 File Offset: 0x0009C942
		private void OnNativeOptionChanged(NativeOptions.NativeOptionsType changedNativeOptionsType)
		{
			if (changedNativeOptionsType == NativeOptions.NativeOptionsType.VoiceChatVolume)
			{
				this.UpdateVoiceChatEnabled();
			}
		}

		// Token: 0x06002A02 RID: 10754 RVA: 0x0009E74E File Offset: 0x0009C94E
		private void OnManagedOptionChanged(ManagedOptions.ManagedOptionsType changedManagedOptionType)
		{
			if (changedManagedOptionType == ManagedOptions.ManagedOptionsType.EnableVoiceChat)
			{
				this.UpdateVoiceChatEnabled();
			}
		}

		// Token: 0x06002A03 RID: 10755 RVA: 0x0009E75C File Offset: 0x0009C95C
		public override void OnPlayerDisconnectedFromServer(NetworkCommunicator networkPeer)
		{
			base.OnPlayerDisconnectedFromServer(networkPeer);
			NetworkCommunicator myPeer = GameNetwork.MyPeer;
			MissionPeer missionPeer = ((myPeer != null) ? myPeer.GetComponent<MissionPeer>() : null);
			MissionPeer component = networkPeer.GetComponent<MissionPeer>();
			if (((component != null) ? component.Team : null) != null && ((missionPeer != null) ? missionPeer.Team : null) != null && component.Team == missionPeer.Team)
			{
				for (int i = 0; i < this._playerVoiceDataList.Count; i++)
				{
					if (this._playerVoiceDataList[i].Peer == component)
					{
						this.RemovePlayerFromVoiceChat(i);
						return;
					}
				}
			}
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x0009E7E8 File Offset: 0x0009C9E8
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			if (networkPeer.IsMuted)
			{
				MultiplayerGlobalMutedPlayersManager.MutePlayer(networkPeer.VirtualPlayer.Id);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SyncPlayerMuteState(networkPeer.VirtualPlayer.Id, networkPeer.IsMuted));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
			}
			GameNetwork.BeginModuleEventAsServer(networkPeer);
			GameNetwork.WriteMessage(new SyncMutedPlayers(MultiplayerGlobalMutedPlayersManager.MutedPlayers));
			GameNetwork.EndModuleEventAsServer();
		}

		// Token: 0x04001008 RID: 4104
		private const int MillisecondsToShorts = 12;

		// Token: 0x04001009 RID: 4105
		private const int MillisecondsToBytes = 24;

		// Token: 0x0400100A RID: 4106
		private const int OpusFrameSizeCoefficient = 6;

		// Token: 0x0400100B RID: 4107
		private const int VoiceFrameRawSizeInMilliseconds = 60;

		// Token: 0x0400100C RID: 4108
		public const int VoiceFrameRawSizeInBytes = 1440;

		// Token: 0x0400100D RID: 4109
		private const int CompressionMaxChunkSizeInBytes = 8640;

		// Token: 0x0400100E RID: 4110
		private const int VoiceRecordMaxChunkSizeInBytes = 72000;

		// Token: 0x04001013 RID: 4115
		private List<VoiceChatHandler.PeerVoiceData> _playerVoiceDataList;

		// Token: 0x04001014 RID: 4116
		private bool _isVoiceChatDisabled = true;

		// Token: 0x04001015 RID: 4117
		private bool _isVoiceRecordActive;

		// Token: 0x04001016 RID: 4118
		private bool _stopRecordingOnNextTick;

		// Token: 0x04001017 RID: 4119
		private Queue<byte> _voiceToSend;

		// Token: 0x04001018 RID: 4120
		private bool _playedAnyVoicePreviousTick;

		// Token: 0x04001019 RID: 4121
		private bool _localUserInitialized;

		// Token: 0x020005B9 RID: 1465
		private class PeerVoiceData
		{
			// Token: 0x17000AA4 RID: 2724
			// (get) Token: 0x06003EE9 RID: 16105 RVA: 0x000F8F37 File Offset: 0x000F7137
			// (set) Token: 0x06003EEA RID: 16106 RVA: 0x000F8F3F File Offset: 0x000F713F
			public bool IsReadyOnPlatform { get; private set; }

			// Token: 0x06003EEB RID: 16107 RVA: 0x000F8F48 File Offset: 0x000F7148
			public PeerVoiceData(MissionPeer peer)
			{
				this.Peer = peer;
				this._voiceData = new Queue<short>();
				this._voiceToPlayInTick = new Queue<short>();
				this._nextPlayDelayResetTime = MissionTime.Now;
			}

			// Token: 0x06003EEC RID: 16108 RVA: 0x000F8F78 File Offset: 0x000F7178
			public void WriteVoiceData(byte[] dataBuffer, int bufferSize)
			{
				if (this._voiceData.Count == 0 && this._nextPlayDelayResetTime.IsPast)
				{
					this._playDelayRemainingSizeInBytes = 3600;
				}
				for (int i = 0; i < bufferSize; i += 2)
				{
					short num = (short)((int)dataBuffer[i] | ((int)dataBuffer[i + 1] << 8));
					this._voiceData.Enqueue(num);
				}
			}

			// Token: 0x06003EED RID: 16109 RVA: 0x000F8FCF File Offset: 0x000F71CF
			public void SetReadyOnPlatform()
			{
				this.IsReadyOnPlatform = true;
			}

			// Token: 0x06003EEE RID: 16110 RVA: 0x000F8FD8 File Offset: 0x000F71D8
			public bool ProcessVoiceData()
			{
				if (this.IsReadyOnPlatform && this._voiceData.Count > 0)
				{
					bool flag = this.Peer.IsMutedFromGameOrPlatform || MultiplayerGlobalMutedPlayersManager.IsUserMuted(this.Peer.Peer.Id);
					if (this._playDelayRemainingSizeInBytes > 0)
					{
						this._playDelayRemainingSizeInBytes -= 2;
					}
					else
					{
						short num = this._voiceData.Dequeue();
						this._nextPlayDelayResetTime = MissionTime.Now + MissionTime.Milliseconds(300f);
						if (!flag)
						{
							this._voiceToPlayInTick.Enqueue(num);
						}
					}
					return !flag;
				}
				return false;
			}

			// Token: 0x06003EEF RID: 16111 RVA: 0x000F9078 File Offset: 0x000F7278
			public Queue<short> GetVoiceToPlayForTick()
			{
				return this._voiceToPlayInTick;
			}

			// Token: 0x06003EF0 RID: 16112 RVA: 0x000F9080 File Offset: 0x000F7280
			public bool HasAnyVoiceData()
			{
				return this.IsReadyOnPlatform && this._voiceData.Count > 0;
			}

			// Token: 0x04001F61 RID: 8033
			private const int PlayDelaySizeInMilliseconds = 150;

			// Token: 0x04001F62 RID: 8034
			private const int PlayDelaySizeInBytes = 3600;

			// Token: 0x04001F63 RID: 8035
			private const float PlayDelayResetTimeInMilliseconds = 300f;

			// Token: 0x04001F64 RID: 8036
			public readonly MissionPeer Peer;

			// Token: 0x04001F66 RID: 8038
			private readonly Queue<short> _voiceData;

			// Token: 0x04001F67 RID: 8039
			private readonly Queue<short> _voiceToPlayInTick;

			// Token: 0x04001F68 RID: 8040
			private int _playDelayRemainingSizeInBytes;

			// Token: 0x04001F69 RID: 8041
			private MissionTime _nextPlayDelayResetTime;
		}
	}
}
