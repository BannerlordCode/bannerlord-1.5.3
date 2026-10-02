using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.Map;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000438 RID: 1080
	public class MapTracksCampaignBehavior : CampaignBehaviorBase, IMapTracksCampaignBehavior, ICampaignBehavior
	{
		// Token: 0x17000E99 RID: 3737
		// (get) Token: 0x0600459D RID: 17821 RVA: 0x0015166E File Offset: 0x0014F86E
		public MBReadOnlyList<Track> DetectedTracks
		{
			get
			{
				return this._detectedTracksCache;
			}
		}

		// Token: 0x0600459E RID: 17822 RVA: 0x00151678 File Offset: 0x0014F878
		public MapTracksCampaignBehavior()
		{
			this._trackPool = new MapTracksCampaignBehavior.TrackPool(2048);
		}

		// Token: 0x0600459F RID: 17823 RVA: 0x001516D0 File Offset: 0x0014F8D0
		public override void RegisterEvents()
		{
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.OnHourlyTick));
			CampaignEvents.OnGameLoadFinishedEvent.AddNonSerializedListener(this, new Action(this.GameLoadFinished));
			CampaignEvents.HourlyTickPartyEvent.AddNonSerializedListener(this, new Action<MobileParty>(this.OnHourlyTickParty));
			CampaignEvents.MobilePartyDestroyed.AddNonSerializedListener(this, new Action<MobileParty, PartyBase>(this.OnMobilePartyDestroyed));
			CampaignEvents.OnNewGameCreatedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreated));
		}

		// Token: 0x060045A0 RID: 17824 RVA: 0x00151750 File Offset: 0x0014F950
		private void OnMobilePartyDestroyed(MobileParty mobileParty, PartyBase destroyerParty)
		{
			if (this._trackDataDictionary.ContainsKey(mobileParty))
			{
				this._trackDataDictionary.Remove(mobileParty);
			}
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x0015176D File Offset: 0x0014F96D
		private void OnNewGameCreated(CampaignGameStarter gameStarted)
		{
			this.AddEventHandler();
		}

		// Token: 0x060045A2 RID: 17826 RVA: 0x00151778 File Offset: 0x0014F978
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<List<Track>>("_allTracks", ref this._allTracks);
			dataStore.SyncData<Dictionary<MobileParty, CampaignVec2>>("_trackDataDictionary2", ref this._trackDataDictionary);
			if (dataStore.IsLoading && MBSaveLoad.IsUpdatingGameVersion && MBSaveLoad.LastLoadedGameVersion.IsOlderThan(ApplicationVersion.FromString("v1.3.0", 0)))
			{
				Dictionary<MobileParty, Vec2> dictionary = new Dictionary<MobileParty, Vec2>();
				dataStore.SyncData<Dictionary<MobileParty, Vec2>>("_trackDataDictionary", ref dictionary);
				if (dictionary.Any<KeyValuePair<MobileParty, Vec2>>())
				{
					foreach (KeyValuePair<MobileParty, Vec2> keyValuePair in dictionary)
					{
						this._trackDataDictionary.Add(keyValuePair.Key, new CampaignVec2(keyValuePair.Value, true));
					}
				}
			}
		}

		// Token: 0x060045A3 RID: 17827 RVA: 0x0015184C File Offset: 0x0014FA4C
		private void OnHourlyTickParty(MobileParty mobileParty)
		{
			if (Campaign.Current.Models.MapTrackModel.CanPartyLeaveTrack(mobileParty))
			{
				CampaignVec2 campaignVec = CampaignVec2.Zero;
				if (this._trackDataDictionary.ContainsKey(mobileParty))
				{
					campaignVec = this._trackDataDictionary[mobileParty];
				}
				if (campaignVec.DistanceSquared(mobileParty.Position.ToVec2()) > 5f && this.IsTrackDropped(mobileParty))
				{
					CampaignVec2 position = mobileParty.Position;
					Vec2 vec = mobileParty.Position.ToVec2() - campaignVec.ToVec2();
					vec.Normalize();
					this.AddTrack(mobileParty, position, vec);
					this._trackDataDictionary[mobileParty] = position;
				}
			}
		}

		// Token: 0x060045A4 RID: 17828 RVA: 0x001518F9 File Offset: 0x0014FAF9
		private void OnHourlyTick()
		{
			this.RemoveExpiredTracks();
		}

		// Token: 0x060045A5 RID: 17829 RVA: 0x00151904 File Offset: 0x0014FB04
		private void GameLoadFinished()
		{
			this._allTracks.RemoveAll((Track x) => x.IsExpired);
			this._detectedTracksCache = this._allTracks.Where<Track>((Track x) => x.IsDetected).ToMBList<Track>();
			this.AddEventHandler();
			foreach (Track track in this._allTracks)
			{
				this._trackLocator.UpdateLocator(track);
			}
			foreach (MobileParty mobileParty in this._trackDataDictionary.Keys.ToList<MobileParty>())
			{
				if (!mobileParty.IsActive)
				{
					this._trackDataDictionary.Remove(mobileParty);
				}
			}
		}

		// Token: 0x060045A6 RID: 17830 RVA: 0x00151A20 File Offset: 0x0014FC20
		private void AddEventHandler()
		{
			this._quarterHourlyTick = CampaignPeriodicEventManager.CreatePeriodicEvent(CampaignTime.Hours(0.25f), CampaignTime.Hours(0.1f));
			this._quarterHourlyTick.AddHandler(new MBCampaignEvent.CampaignEventDelegate(this.QuarterHourlyTick));
		}

		// Token: 0x060045A7 RID: 17831 RVA: 0x00151A58 File Offset: 0x0014FC58
		private void QuarterHourlyTick(MBCampaignEvent campaignEvent, object[] delegateParams)
		{
			if (!PartyBase.MainParty.IsValid)
			{
				return;
			}
			int num = ((MobileParty.MainParty.EffectiveScout != null) ? MobileParty.MainParty.EffectiveScout.GetSkillValue(DefaultSkills.Scouting) : 0);
			if (num != 0)
			{
				float maxTrackSpottingDistanceForMainParty = Campaign.Current.Models.MapTrackModel.GetMaxTrackSpottingDistanceForMainParty();
				LocatableSearchData<Track> locatableSearchData = this._trackLocator.StartFindingLocatablesAroundPosition(MobileParty.MainParty.Position.ToVec2(), maxTrackSpottingDistanceForMainParty);
				for (Track track = this._trackLocator.FindNextLocatable(ref locatableSearchData); track != null; track = this._trackLocator.FindNextLocatable(ref locatableSearchData))
				{
					if (!track.IsDetected && this._allTracks.Contains(track) && Campaign.Current.Models.MapTrackModel.GetTrackDetectionDifficultyForMainParty(track, maxTrackSpottingDistanceForMainParty) < (float)num)
					{
						this.TrackDetected(track);
					}
				}
			}
		}

		// Token: 0x060045A8 RID: 17832 RVA: 0x00151B2C File Offset: 0x0014FD2C
		private void RemoveExpiredTracks()
		{
			for (int i = this._allTracks.Count - 1; i >= 0; i--)
			{
				Track track = this._allTracks[i];
				if (track.IsExpired)
				{
					this._allTracks.Remove(track);
					if (this._detectedTracksCache.Contains(track))
					{
						this._detectedTracksCache.Remove(track);
						CampaignEventDispatcher.Instance.TrackLost(track);
					}
					this._trackLocator.RemoveLocatable(track);
					this._trackPool.ReleaseTrack(track);
				}
			}
		}

		// Token: 0x060045A9 RID: 17833 RVA: 0x00151BB1 File Offset: 0x0014FDB1
		private void TrackDetected(Track track)
		{
			track.IsDetected = true;
			this._detectedTracksCache.Add(track);
			CampaignEventDispatcher.Instance.TrackDetected(track);
			SkillLevelingManager.OnTrackDetected(track);
		}

		// Token: 0x060045AA RID: 17834 RVA: 0x00151BD8 File Offset: 0x0014FDD8
		public bool IsTrackDropped(MobileParty mobileParty)
		{
			float skipTrackChance = Campaign.Current.Models.MapTrackModel.GetSkipTrackChance(mobileParty);
			if (MBRandom.RandomFloat < skipTrackChance)
			{
				return false;
			}
			float num = mobileParty.Position.DistanceSquared(MobileParty.MainParty.Position);
			float num2 = (MobileParty.MainParty.IsActive ? (MobileParty.MainParty._lastCalculatedSpeed * Campaign.Current.Models.MapTrackModel.MaxTrackLife) : 0f);
			return num2 * num2 > num;
		}

		// Token: 0x060045AB RID: 17835 RVA: 0x00151C58 File Offset: 0x0014FE58
		public void AddTrack(MobileParty party, CampaignVec2 trackPosition, Vec2 trackDirection)
		{
			Track track = this._trackPool.RequestTrack(party, trackPosition, trackDirection);
			this._allTracks.Add(track);
			this._trackLocator.UpdateLocator(track);
		}

		// Token: 0x060045AC RID: 17836 RVA: 0x00151C90 File Offset: 0x0014FE90
		public void AddMapArrow(TextObject pointerName, CampaignVec2 trackPosition, Vec2 trackDirection, float life)
		{
			Track track = this._trackPool.RequestMapArrow(pointerName, trackPosition, trackDirection, life);
			this._allTracks.Add(track);
			this._trackLocator.UpdateLocator(track);
			this.TrackDetected(track);
		}

		// Token: 0x0400140B RID: 5131
		private const float PartyTrackPositionDelta = 5f;

		// Token: 0x0400140C RID: 5132
		private List<Track> _allTracks = new List<Track>();

		// Token: 0x0400140D RID: 5133
		private MBList<Track> _detectedTracksCache = new MBList<Track>();

		// Token: 0x0400140E RID: 5134
		private Dictionary<MobileParty, CampaignVec2> _trackDataDictionary = new Dictionary<MobileParty, CampaignVec2>();

		// Token: 0x0400140F RID: 5135
		private MBCampaignEvent _quarterHourlyTick;

		// Token: 0x04001410 RID: 5136
		private LocatorGrid<Track> _trackLocator = new LocatorGrid<Track>(5f, 32, 32);

		// Token: 0x04001411 RID: 5137
		private MapTracksCampaignBehavior.TrackPool _trackPool;

		// Token: 0x02000874 RID: 2164
		private class TrackPool
		{
			// Token: 0x170015A6 RID: 5542
			// (get) Token: 0x06006AF6 RID: 27382 RVA: 0x001DA259 File Offset: 0x001D8459
			private int MaxSize { get; }

			// Token: 0x170015A7 RID: 5543
			// (get) Token: 0x06006AF7 RID: 27383 RVA: 0x001DA261 File Offset: 0x001D8461
			public int Size
			{
				get
				{
					Stack<Track> stack = this._stack;
					if (stack == null)
					{
						return 0;
					}
					return stack.Count;
				}
			}

			// Token: 0x06006AF8 RID: 27384 RVA: 0x001DA274 File Offset: 0x001D8474
			public TrackPool(int size)
			{
				this.MaxSize = size;
				this._stack = new Stack<Track>();
				for (int i = 0; i < size; i++)
				{
					this._stack.Push(new Track());
				}
			}

			// Token: 0x06006AF9 RID: 27385 RVA: 0x001DA2B8 File Offset: 0x001D84B8
			public Track RequestTrack(MobileParty party, CampaignVec2 trackPosition, Vec2 trackDirection)
			{
				Track track = ((this._stack.Count > 0) ? this._stack.Pop() : new Track());
				int num = party.Party.NumberOfAllMembers;
				int num2 = party.Party.NumberOfHealthyMembers;
				int num3 = party.Party.NumberOfMenWithHorse;
				int num4 = party.Party.NumberOfMenWithoutHorse;
				int num5 = party.Party.NumberOfPackAnimals;
				int num6 = party.Party.NumberOfPrisoners;
				TextObject textObject = party.Name;
				if (party.Army != null && party.Army.LeaderParty == party)
				{
					textObject = party.ArmyName;
					foreach (MobileParty mobileParty in party.Army.LeaderParty.AttachedParties)
					{
						num += mobileParty.Party.NumberOfAllMembers;
						num2 += mobileParty.Party.NumberOfHealthyMembers;
						num3 += mobileParty.Party.NumberOfMenWithHorse;
						num4 += mobileParty.Party.NumberOfMenWithoutHorse;
						num5 += mobileParty.Party.NumberOfPackAnimals;
						num6 += mobileParty.Party.NumberOfPrisoners;
					}
				}
				track.Position = trackPosition;
				track.Direction = trackDirection.RotationInRadians;
				track.PartyType = Track.GetPartyTypeEnum(party);
				track.PartyName = textObject;
				track.Culture = party.Party.Culture;
				if (track.Culture == null)
				{
					string text = string.Format("Track culture is null for {0}: {1}", party.StringId, party.Name);
					Debug.Print(text, 0, Debug.DebugColor.White, 17592186044416UL);
					Debug.FailedAssert(text, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\CampaignBehaviors\\MapTracksCampaignBehavior.cs", "RequestTrack", 62);
				}
				track.Speed = party.Speed;
				track.Life = (float)Campaign.Current.Models.MapTrackModel.GetTrackLife(party);
				track.IsEnemy = FactionManager.IsAtWarAgainstFaction(Hero.MainHero.MapFaction, party.MapFaction);
				track.NumberOfAllMembers = num;
				track.NumberOfHealthyMembers = num2;
				track.NumberOfMenWithHorse = num3;
				track.NumberOfMenWithoutHorse = num4;
				track.NumberOfPackAnimals = num5;
				track.NumberOfPrisoners = num6;
				track.IsPointer = false;
				track.IsDetected = false;
				track.CreationTime = CampaignTime.Now;
				return track;
			}

			// Token: 0x06006AFA RID: 27386 RVA: 0x001DA50C File Offset: 0x001D870C
			public Track RequestMapArrow(TextObject pointerName, CampaignVec2 trackPosition, Vec2 trackDirection, float life)
			{
				Track track = ((this._stack.Count > 0) ? this._stack.Pop() : new Track());
				track.Position = trackPosition;
				track.Direction = trackDirection.RotationInRadians;
				track.PartyName = pointerName;
				track.Life = life;
				track.IsPointer = true;
				track.IsDetected = true;
				track.CreationTime = CampaignTime.Now;
				return track;
			}

			// Token: 0x06006AFB RID: 27387 RVA: 0x001DA575 File Offset: 0x001D8775
			public void ReleaseTrack(Track track)
			{
				track.Reset();
				if (this._stack.Count < this.MaxSize)
				{
					this._stack.Push(track);
				}
			}

			// Token: 0x06006AFC RID: 27388 RVA: 0x001DA59C File Offset: 0x001D879C
			public override string ToString()
			{
				return string.Format("TrackPool: {0}", this.Size);
			}

			// Token: 0x040024CD RID: 9421
			private Stack<Track> _stack;
		}
	}
}
