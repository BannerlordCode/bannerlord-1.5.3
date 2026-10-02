using System;
using System.Linq;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000018 RID: 24
	public class MultiplayerVoiceChatVM : ViewModel
	{
		// Token: 0x0600014D RID: 333 RVA: 0x000060C8 File Offset: 0x000042C8
		public MultiplayerVoiceChatVM(Mission mission)
		{
			this._mission = mission;
			this._voiceChatHandler = this._mission.GetMissionBehavior<VoiceChatHandler>();
			if (this._voiceChatHandler != null)
			{
				this._voiceChatHandler.OnPeerVoiceStatusUpdated += this.OnPeerVoiceStatusUpdated;
				this._voiceChatHandler.OnVoiceRecordStarted += this.OnVoiceRecordStarted;
				this._voiceChatHandler.OnVoiceRecordStopped += this.OnVoiceRecordStopped;
			}
			this.ActiveVoicePlayers = new MBBindingList<MPVoicePlayerVM>();
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000614C File Offset: 0x0000434C
		public override void OnFinalize()
		{
			if (this._voiceChatHandler != null)
			{
				this._voiceChatHandler.OnPeerVoiceStatusUpdated -= this.OnPeerVoiceStatusUpdated;
				this._voiceChatHandler.OnVoiceRecordStarted -= this.OnVoiceRecordStarted;
				this._voiceChatHandler.OnVoiceRecordStopped -= this.OnVoiceRecordStopped;
			}
			base.OnFinalize();
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000061AC File Offset: 0x000043AC
		public void OnTick(float dt)
		{
			for (int i = 0; i < this.ActiveVoicePlayers.Count; i++)
			{
				if (!this.ActiveVoicePlayers[i].IsMyPeer && this.ActiveVoicePlayers[i].UpdatesSinceSilence >= 30)
				{
					this.ActiveVoicePlayers.RemoveAt(i);
					i--;
				}
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00006208 File Offset: 0x00004408
		private void OnPeerVoiceStatusUpdated(MissionPeer peer, bool isTalking)
		{
			MPVoicePlayerVM mpvoicePlayerVM = this.ActiveVoicePlayers.FirstOrDefault<MPVoicePlayerVM>((MPVoicePlayerVM vp) => vp.Peer == peer);
			if (!isTalking)
			{
				if (!isTalking && mpvoicePlayerVM != null)
				{
					mpvoicePlayerVM.UpdatesSinceSilence++;
				}
				return;
			}
			if (mpvoicePlayerVM == null)
			{
				this.ActiveVoicePlayers.Add(new MPVoicePlayerVM(peer));
				return;
			}
			mpvoicePlayerVM.UpdatesSinceSilence = 0;
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00006273 File Offset: 0x00004473
		private void OnVoiceRecordStarted()
		{
			this.ActiveVoicePlayers.Add(new MPVoicePlayerVM(GameNetwork.MyPeer.GetComponent<MissionPeer>()));
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00006290 File Offset: 0x00004490
		private void OnVoiceRecordStopped()
		{
			MPVoicePlayerVM mpvoicePlayerVM = this.ActiveVoicePlayers.FirstOrDefault<MPVoicePlayerVM>((MPVoicePlayerVM vp) => vp.Peer == GameNetwork.MyPeer.GetComponent<MissionPeer>());
			this.ActiveVoicePlayers.Remove(mpvoicePlayerVM);
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x06000153 RID: 339 RVA: 0x000062D5 File Offset: 0x000044D5
		// (set) Token: 0x06000154 RID: 340 RVA: 0x000062DD File Offset: 0x000044DD
		[DataSourceProperty]
		public MBBindingList<MPVoicePlayerVM> ActiveVoicePlayers
		{
			get
			{
				return this._activeVoicePlayers;
			}
			set
			{
				if (value != this._activeVoicePlayers)
				{
					this._activeVoicePlayers = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPVoicePlayerVM>>(value, "ActiveVoicePlayers");
				}
			}
		}

		// Token: 0x040000B0 RID: 176
		private readonly Mission _mission;

		// Token: 0x040000B1 RID: 177
		private readonly VoiceChatHandler _voiceChatHandler;

		// Token: 0x040000B2 RID: 178
		private MBBindingList<MPVoicePlayerVM> _activeVoicePlayers;
	}
}
