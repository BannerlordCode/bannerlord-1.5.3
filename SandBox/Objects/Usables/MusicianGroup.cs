using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.AI;
using SandBox.Objects.AnimationPoints;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.ObjectSystem;

namespace SandBox.Objects.Usables
{
	// Token: 0x0200004C RID: 76
	public class MusicianGroup : UsableMachine
	{
		// Token: 0x060002C9 RID: 713 RVA: 0x0000FFF8 File Offset: 0x0000E1F8
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			return null;
		}

		// Token: 0x060002CA RID: 714 RVA: 0x0000FFFB File Offset: 0x0000E1FB
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return null;
		}

		// Token: 0x060002CB RID: 715 RVA: 0x0000FFFE File Offset: 0x0000E1FE
		public override UsableMachineAIBase CreateAIBehaviorObject()
		{
			return new UsablePlaceAI(this);
		}

		// Token: 0x060002CC RID: 716 RVA: 0x00010006 File Offset: 0x0000E206
		public void SetPlayList(List<SettlementMusicData> playList)
		{
			this._playList = playList.ToList<SettlementMusicData>();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00010014 File Offset: 0x0000E214
		protected override void OnInit()
		{
			base.OnInit();
			this._playList = new List<SettlementMusicData>();
			this._musicianPoints = base.StandingPoints.OfType<PlayMusicPoint>().ToList<PlayMusicPoint>();
		}

		// Token: 0x060002CE RID: 718 RVA: 0x0001003D File Offset: 0x0000E23D
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x060002CF RID: 719 RVA: 0x00010047 File Offset: 0x0000E247
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			this.CheckNewTrackStart();
			this.CheckTrackEnd();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x0001005C File Offset: 0x0000E25C
		private void CheckNewTrackStart()
		{
			if (this._playList.Count > 0 && this._trackEvent == null && (this._gapTimer == null || this._gapTimer.ElapsedTime > 8f))
			{
				if (this._musicianPoints.Any<PlayMusicPoint>((PlayMusicPoint x) => x.HasUser))
				{
					this._currentTrackIndex++;
					if (this._currentTrackIndex == this._playList.Count)
					{
						this._currentTrackIndex = 0;
					}
					this.SetupInstruments();
					this.StartTrack();
					this._gapTimer = null;
				}
			}
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00010108 File Offset: 0x0000E308
		private void CheckTrackEnd()
		{
			if (this._trackEvent != null)
			{
				if (this._trackEvent.IsPlaying())
				{
					if (!this._musicianPoints.Any<PlayMusicPoint>((PlayMusicPoint x) => x.HasUser))
					{
						this._trackEvent.Stop();
					}
				}
				if (this._trackEvent != null && !this._trackEvent.IsPlaying())
				{
					this._trackEvent.Release();
					this._trackEvent = null;
					this.StopMusicians();
					this._gapTimer = new BasicMissionTimer();
				}
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0001019C File Offset: 0x0000E39C
		private void StopMusicians()
		{
			foreach (PlayMusicPoint playMusicPoint in this._musicianPoints)
			{
				if (playMusicPoint.HasUser)
				{
					playMusicPoint.EndLoop();
				}
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000101F8 File Offset: 0x0000E3F8
		private void SetupInstruments()
		{
			List<PlayMusicPoint> list = this._musicianPoints.ToList<PlayMusicPoint>();
			list.Shuffle<PlayMusicPoint>();
			SettlementMusicData settlementMusicData = this._playList[this._currentTrackIndex];
			using (List<InstrumentData>.Enumerator enumerator = settlementMusicData.Instruments.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					InstrumentData instrumentData = enumerator.Current;
					PlayMusicPoint playMusicPoint = list.FirstOrDefault<PlayMusicPoint>((PlayMusicPoint x) => x.GameEntity.Parent.Tags.Contains(instrumentData.Tag) || string.IsNullOrEmpty(instrumentData.Tag));
					if (playMusicPoint != null)
					{
						Tuple<InstrumentData, float> tuple = new Tuple<InstrumentData, float>(instrumentData, (float)settlementMusicData.Tempo / 120f);
						playMusicPoint.ChangeInstrument(tuple);
						list.Remove(playMusicPoint);
					}
				}
			}
			Tuple<InstrumentData, float> instrumentEmptyData = this.GetInstrumentEmptyData(settlementMusicData.Tempo);
			foreach (PlayMusicPoint playMusicPoint2 in list)
			{
				playMusicPoint2.ChangeInstrument(instrumentEmptyData);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00010304 File Offset: 0x0000E504
		private Tuple<InstrumentData, float> GetInstrumentEmptyData(int tempo)
		{
			Tuple<InstrumentData, float> tuple;
			if (tempo > 130)
			{
				tuple = new Tuple<InstrumentData, float>(MBObjectManager.Instance.GetObject<InstrumentData>("cheerful"), 1f);
			}
			else if (tempo > 100)
			{
				tuple = new Tuple<InstrumentData, float>(MBObjectManager.Instance.GetObject<InstrumentData>("active"), 1f);
			}
			else
			{
				tuple = new Tuple<InstrumentData, float>(MBObjectManager.Instance.GetObject<InstrumentData>("calm"), 1f);
			}
			return tuple;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00010374 File Offset: 0x0000E574
		private void StartTrack()
		{
			int eventIdFromString = SoundEvent.GetEventIdFromString(this._playList[this._currentTrackIndex].MusicPath);
			this._trackEvent = SoundEvent.CreateEvent(eventIdFromString, Mission.Current.Scene);
			this._trackEvent.SetPosition(base.GameEntity.GetGlobalFrame().origin);
			this._trackEvent.Play();
			foreach (PlayMusicPoint playMusicPoint in this._musicianPoints)
			{
				playMusicPoint.StartLoop(this._trackEvent);
			}
		}

		// Token: 0x04000133 RID: 307
		public const int GapBetweenTracks = 8;

		// Token: 0x04000134 RID: 308
		public const bool DisableAmbientMusic = true;

		// Token: 0x04000135 RID: 309
		private const int TempoMidValue = 120;

		// Token: 0x04000136 RID: 310
		private const int TempoSpeedUpLimit = 130;

		// Token: 0x04000137 RID: 311
		private const int TempoSlowDownLimit = 100;

		// Token: 0x04000138 RID: 312
		private List<PlayMusicPoint> _musicianPoints;

		// Token: 0x04000139 RID: 313
		private SoundEvent _trackEvent;

		// Token: 0x0400013A RID: 314
		private BasicMissionTimer _gapTimer;

		// Token: 0x0400013B RID: 315
		private List<SettlementMusicData> _playList;

		// Token: 0x0400013C RID: 316
		private int _currentTrackIndex = -1;
	}
}
