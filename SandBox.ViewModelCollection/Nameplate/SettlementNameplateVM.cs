using System;
using System.Collections.Generic;
using Helpers;
using SandBox.ViewModelCollection.Nameplate.NameplateNotifications.SettlementNotificationTypes;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.Settlements.Buildings;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x02000021 RID: 33
	public class SettlementNameplateVM : NameplateVM
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000318 RID: 792 RVA: 0x0000DB73 File Offset: 0x0000BD73
		public Settlement Settlement { get; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000319 RID: 793 RVA: 0x0000DB7B File Offset: 0x0000BD7B
		// (set) Token: 0x0600031A RID: 794 RVA: 0x0000DB83 File Offset: 0x0000BD83
		public SettlementNameplateVM.Type SettlementTypeEnum { get; private set; }

		// Token: 0x0600031B RID: 795 RVA: 0x0000DB8C File Offset: 0x0000BD8C
		public SettlementNameplateVM(Settlement settlement, GameEntity entity, Camera mapCamera, Action<CampaignVec2> fastMoveCameraToPosition)
		{
			this.Settlement = settlement;
			this._mapCamera = mapCamera;
			this._entity = entity;
			this._fastMoveCameraToPosition = fastMoveCameraToPosition;
			this.SettlementNotifications = new SettlementNameplateNotificationsVM(settlement);
			this.SettlementParties = new SettlementNameplatePartyMarkersVM(settlement);
			this.SettlementEvents = new SettlementNameplateEventsVM(settlement);
			this.Name = this.Settlement.Name.ToString();
			this.IsTracked = Campaign.Current.VisualTrackerManager.CheckTracked(settlement);
			if (this.Settlement.IsCastle)
			{
				this.SettlementTypeEnum = SettlementNameplateVM.Type.Castle;
				this._isCastle = true;
			}
			else if (this.Settlement.IsVillage)
			{
				this.SettlementTypeEnum = SettlementNameplateVM.Type.Village;
				this._isVillage = true;
			}
			else if (this.Settlement.IsTown)
			{
				this.SettlementTypeEnum = SettlementNameplateVM.Type.Town;
				this._isTown = true;
			}
			else
			{
				this.SettlementTypeEnum = SettlementNameplateVM.Type.Village;
				this._isTown = true;
			}
			this.SettlementType = (int)this.SettlementTypeEnum;
			if (this._entity != null)
			{
				this._worldPos = this._entity.GlobalPosition;
			}
			else
			{
				this._worldPos = this.Settlement.GetPositionAsVec3();
			}
			this.RefreshDynamicProperties(true);
			this._rebelliousClans = new List<Clan>();
			if (Game.Current != null)
			{
				Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(base.OnTutorialNotificationElementChanged));
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x0000DCEA File Offset: 0x0000BEEA
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.SettlementNotifications.UnloadEvents();
			this.SettlementParties.UnloadEvents();
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(base.OnTutorialNotificationElementChanged));
		}

		// Token: 0x0600031D RID: 797 RVA: 0x0000DD23 File Offset: 0x0000BF23
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.Name = this.Settlement.Name.ToString();
		}

		// Token: 0x0600031E RID: 798 RVA: 0x0000DD44 File Offset: 0x0000BF44
		public override void RefreshDynamicProperties(bool forceUpdate)
		{
			base.RefreshDynamicProperties(forceUpdate);
			if ((this._bindIsVisibleOnMap && this._currentFaction != this.Settlement.MapFaction) || forceUpdate)
			{
				string text = "#";
				IFaction mapFaction = this.Settlement.MapFaction;
				this._bindFactionColor = text + Color.UIntToColorString((mapFaction != null) ? mapFaction.Color : uint.MaxValue);
				Banner banner = this.Settlement.Banner;
				int num = ((banner != null) ? banner.GetVersionNo() : 0);
				if ((this._latestBanner != banner && !this._latestBanner.IsContentsSameWith(banner)) || this._latestBannerVersionNo != num)
				{
					this._bindBanner = ((banner != null) ? new BannerImageIdentifierVM(banner, true) : new BannerImageIdentifierVM(null, false));
					this._latestBannerVersionNo = num;
					this._latestBanner = banner;
				}
				this._currentFaction = this.Settlement.MapFaction;
			}
			PartyBase party = this.Settlement.Party;
			if ((party != null && party.IsVisualDirty) || forceUpdate)
			{
				this._bindName = this.Settlement.Party.Name.ToString();
			}
			this._bindIsTracked = Campaign.Current.VisualTrackerManager.CheckTracked(this.Settlement);
			if (this.Settlement.IsHideout)
			{
				this._bindIsInRange = this.Settlement.IsVisible;
			}
			else
			{
				this._bindIsInRange = this.Settlement.IsInspected;
			}
			if (this._bindIsVisibleOnMap || forceUpdate)
			{
				this._bindHasPort = this.Settlement.HasPort;
				this._bindPortLevel = 0;
				if (this._bindHasPort)
				{
					Settlement settlement = this.Settlement;
					MBList<Building> mblist;
					if (settlement == null)
					{
						mblist = null;
					}
					else
					{
						Town town = settlement.Town;
						mblist = ((town != null) ? town.Buildings : null);
					}
					MBList<Building> mblist2 = mblist;
					if (mblist2 != null)
					{
						for (int i = 0; i < mblist2.Count; i++)
						{
							if (mblist2[i].BuildingType.StringId == "building_shipyard")
							{
								this._bindPortLevel = mblist2[i].CurrentLevel;
							}
						}
					}
				}
				this._bindHasFerry = this.Settlement.FerryTarget != null;
			}
		}

		// Token: 0x0600031F RID: 799 RVA: 0x0000DF44 File Offset: 0x0000C144
		public override void RefreshRelationStatus()
		{
			this._bindRelation = 0;
			if (this.Settlement.OwnerClan != null)
			{
				if (FactionManager.IsAtWarAgainstFaction(this.Settlement.MapFaction, Hero.MainHero.MapFaction))
				{
					this._bindRelation = 2;
					return;
				}
				if (DiplomacyHelper.IsSameFactionAndNotEliminated(this.Settlement.MapFaction, Hero.MainHero.MapFaction))
				{
					this._bindRelation = 1;
					return;
				}
				if (DiplomacyHelper.HasAllianceWithFaction(this.Settlement.MapFaction, Hero.MainHero.MapFaction))
				{
					this._bindRelation = 3;
				}
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x0000DFD0 File Offset: 0x0000C1D0
		public override void RefreshPosition()
		{
			base.RefreshPosition();
			this._bindWPos = this._wPosAfterPositionCalculation;
			this._bindWSign = (int)this._bindWPos;
			this._bindIsInside = this._latestIsInsideWindow;
			if (this._bindIsVisibleOnMap)
			{
				this._bindPosition = new Vec2(this._latestX, this._latestY);
				return;
			}
			this._bindPosition = new Vec2(-1000f, -1000f);
		}

		// Token: 0x06000321 RID: 801 RVA: 0x0000E040 File Offset: 0x0000C240
		public override void RefreshTutorialStatus(string newTutorialHighlightElementID)
		{
			base.RefreshTutorialStatus(newTutorialHighlightElementID);
			Settlement settlement = this.Settlement;
			bool flag;
			if (settlement == null)
			{
				flag = null != null;
			}
			else
			{
				PartyBase party = settlement.Party;
				flag = ((party != null) ? party.Id : null) != null;
			}
			if (!flag)
			{
				Debug.FailedAssert("Settlement party id is null when refreshing tutorial status", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Nameplate\\SettlementNameplateVM.cs", "RefreshTutorialStatus", 273);
				return;
			}
			this._bindIsTargetedByTutorial = this.Settlement.Party.Id == newTutorialHighlightElementID;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x0000E0AC File Offset: 0x0000C2AC
		public void OnSiegeEventStartedOnSettlement(SiegeEvent siegeEvent)
		{
			this.MapEventVisualType = 2;
			if (this.Settlement.MapFaction == Hero.MainHero.MapFaction && (BannerlordConfig.AutoTrackAttackedSettlements == 0 || (BannerlordConfig.AutoTrackAttackedSettlements == 1 && this.Settlement.MapFaction.Leader == Hero.MainHero)))
			{
				this.Track();
			}
		}

		// Token: 0x06000323 RID: 803 RVA: 0x0000E104 File Offset: 0x0000C304
		public void OnSiegeEventEndedOnSettlement(SiegeEvent siegeEvent)
		{
			Settlement settlement = this.Settlement;
			bool flag;
			if (settlement == null)
			{
				flag = null != null;
			}
			else
			{
				PartyBase party = settlement.Party;
				flag = ((party != null) ? party.MapEvent : null) != null;
			}
			if (flag && !this.Settlement.Party.MapEvent.IsFinalized)
			{
				this.OnMapEventStartedOnSettlement(this.Settlement.Party.MapEvent);
			}
			else
			{
				this.OnMapEventEndedOnSettlement();
			}
			if (!this._isTrackedManually && BannerlordConfig.AutoTrackAttackedSettlements < 2 && this.Settlement.MapFaction == Hero.MainHero.MapFaction)
			{
				this.Untrack();
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x0000E194 File Offset: 0x0000C394
		public void OnMapEventStartedOnSettlement(MapEvent mapEvent)
		{
			this.MapEventVisualType = (int)SandBoxUIHelper.GetMapEventVisualTypeFromMapEvent(mapEvent);
			if (this.Settlement.MapFaction == Hero.MainHero.MapFaction && (this.Settlement.IsUnderRaid || this.Settlement.IsUnderSiege || this.Settlement.InRebelliousState) && (BannerlordConfig.AutoTrackAttackedSettlements == 0 || (BannerlordConfig.AutoTrackAttackedSettlements == 1 && this.Settlement.MapFaction.Leader == Hero.MainHero)))
			{
				this.Track();
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x0000E218 File Offset: 0x0000C418
		public void OnMapEventEndedOnSettlement()
		{
			this.MapEventVisualType = 0;
			if (!this._isTrackedManually && BannerlordConfig.AutoTrackAttackedSettlements < 2 && !this.Settlement.IsUnderSiege && !this.Settlement.IsUnderRaid && !this.Settlement.InRebelliousState)
			{
				this.Untrack();
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x0000E26C File Offset: 0x0000C46C
		public void OnRebelliousClanFormed(Clan clan)
		{
			this.MapEventVisualType = 4;
			this._rebelliousClans.Add(clan);
			if (this.Settlement.MapFaction == Hero.MainHero.MapFaction && (BannerlordConfig.AutoTrackAttackedSettlements == 0 || (BannerlordConfig.AutoTrackAttackedSettlements == 1 && this.Settlement.MapFaction.Leader == Hero.MainHero)))
			{
				this.Track();
			}
		}

		// Token: 0x06000327 RID: 807 RVA: 0x0000E2D0 File Offset: 0x0000C4D0
		public void OnRebelliousClanDisbanded(Clan clan)
		{
			this._rebelliousClans.Remove(clan);
			if (this._rebelliousClans.IsEmpty<Clan>())
			{
				if (this.Settlement.IsUnderSiege)
				{
					this.MapEventVisualType = 2;
					return;
				}
				this.MapEventVisualType = 0;
				if (!this._isTrackedManually && BannerlordConfig.AutoTrackAttackedSettlements < 2)
				{
					this.Untrack();
				}
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x0000E329 File Offset: 0x0000C529
		public void UpdateNameplateMT(Vec3 cameraPosition)
		{
			this.CalculatePosition(in cameraPosition);
			this.DetermineIsInsideWindow();
			this.DetermineIsVisibleOnMap(in cameraPosition);
			this.RefreshPosition();
			this.RefreshDynamicProperties(false);
		}

		// Token: 0x06000329 RID: 809 RVA: 0x0000E350 File Offset: 0x0000C550
		private void CalculatePosition(in Vec3 cameraPosition)
		{
			this._worldPosWithHeight = this._worldPos;
			if (this._isVillage)
			{
				this._heightOffset = 0.5f + MathF.Clamp(cameraPosition.z / 30f, 0f, 1f) * 2.5f;
			}
			else if (this._isCastle)
			{
				this._heightOffset = 0.5f + MathF.Clamp(cameraPosition.z / 30f, 0f, 1f) * 3f;
			}
			else if (this._isTown)
			{
				this._heightOffset = 0.5f + MathF.Clamp(cameraPosition.z / 30f, 0f, 1f) * 6f;
			}
			else
			{
				this._heightOffset = 1f;
			}
			this._worldPosWithHeight += new Vec3(0f, 0f, this._heightOffset, -1f);
			if (this._worldPosWithHeight.IsValidXYZW && this._mapCamera.Position.IsValidXYZW)
			{
				this._latestX = 0f;
				this._latestY = 0f;
				this._latestW = 0f;
				MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, this._worldPosWithHeight, ref this._latestX, ref this._latestY, ref this._latestW);
			}
			this._wPosAfterPositionCalculation = ((this._latestW < 0f) ? (-1f) : 1.1f);
		}

		// Token: 0x0600032A RID: 810 RVA: 0x0000E4CE File Offset: 0x0000C6CE
		private void DetermineIsVisibleOnMap(in Vec3 cameraPosition)
		{
			this._bindIsVisibleOnMap = this.IsVisible(in cameraPosition);
		}

		// Token: 0x0600032B RID: 811 RVA: 0x0000E4DD File Offset: 0x0000C6DD
		private void DetermineIsInsideWindow()
		{
			this._latestIsInsideWindow = this.IsInsideWindow();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E4EC File Offset: 0x0000C6EC
		public void RefreshBindValues()
		{
			base.FactionColor = this._bindFactionColor;
			this.Banner = this._bindBanner;
			this.Relation = this._bindRelation;
			this.WPos = this._bindWPos;
			this.WSign = this._bindWSign;
			this.IsInside = this._bindIsInside;
			base.Position = this._bindPosition;
			base.IsVisibleOnMap = this._bindIsVisibleOnMap;
			this.IsInRange = this._bindIsInRange;
			this.HasPort = this._bindHasPort;
			this.PortLevel = this._bindPortLevel;
			this.HasFerry = this._bindHasFerry;
			this.IsTracked = this._bindIsTracked;
			base.IsTargetedByTutorial = this._bindIsTargetedByTutorial;
			base.DistanceToCamera = this._bindDistanceToCamera;
			this.Name = this._bindName;
			if (this.SettlementNotifications.IsEventsRegistered)
			{
				this.SettlementNotifications.Tick();
			}
			if (this.SettlementEvents.IsEventsRegistered)
			{
				this.SettlementEvents.Tick();
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000E5EC File Offset: 0x0000C7EC
		private bool IsVisible(in Vec3 cameraPosition)
		{
			this._bindDistanceToCamera = this._worldPos.Distance(cameraPosition);
			if (this.IsTracked)
			{
				return true;
			}
			if (this.WPos < 0f || !this._latestIsInsideWindow)
			{
				return false;
			}
			if (cameraPosition.z > 400f)
			{
				return this.Settlement.IsTown;
			}
			if (cameraPosition.z > 200f)
			{
				return this.Settlement.IsFortification;
			}
			return this._bindDistanceToCamera < cameraPosition.z + 100f;
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000E678 File Offset: 0x0000C878
		private bool IsInsideWindow()
		{
			float num = Screen.RealScreenResolutionWidth * 0.00052083336f;
			return this._latestX <= Screen.RealScreenResolutionWidth + 200f * num && this._latestY <= Screen.RealScreenResolutionHeight + 100f * num && this._latestX + 200f * num >= 0f && this._latestY + 100f * num >= 0f;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000E6EA File Offset: 0x0000C8EA
		public void ExecuteTrack()
		{
			if (this.IsTracked)
			{
				this.Untrack();
				this._isTrackedManually = false;
				return;
			}
			this.Track();
			this._isTrackedManually = true;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000E70F File Offset: 0x0000C90F
		private void Track()
		{
			this.IsTracked = true;
			if (!Campaign.Current.VisualTrackerManager.CheckTracked(this.Settlement))
			{
				Campaign.Current.VisualTrackerManager.RegisterObject(this.Settlement);
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x0000E744 File Offset: 0x0000C944
		private void Untrack()
		{
			this.IsTracked = false;
			if (Campaign.Current.VisualTrackerManager.CheckTracked(this.Settlement))
			{
				Campaign.Current.VisualTrackerManager.RemoveTrackedObject(this.Settlement, false);
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x0000E77A File Offset: 0x0000C97A
		public void ExecuteSetCameraPosition()
		{
			this._fastMoveCameraToPosition(this.Settlement.Position);
		}

		// Token: 0x06000333 RID: 819 RVA: 0x0000E792 File Offset: 0x0000C992
		public void ExecuteOpenEncyclopedia()
		{
			Campaign.Current.EncyclopediaManager.GoToLink(this.Settlement.EncyclopediaLink);
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000E7AE File Offset: 0x0000C9AE
		// (set) Token: 0x06000335 RID: 821 RVA: 0x0000E7B6 File Offset: 0x0000C9B6
		public SettlementNameplateNotificationsVM SettlementNotifications
		{
			get
			{
				return this._settlementNotifications;
			}
			set
			{
				if (value != this._settlementNotifications)
				{
					this._settlementNotifications = value;
					base.OnPropertyChangedWithValue<SettlementNameplateNotificationsVM>(value, "SettlementNotifications");
				}
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000E7D4 File Offset: 0x0000C9D4
		// (set) Token: 0x06000337 RID: 823 RVA: 0x0000E7DC File Offset: 0x0000C9DC
		public SettlementNameplatePartyMarkersVM SettlementParties
		{
			get
			{
				return this._settlementParties;
			}
			set
			{
				if (value != this._settlementParties)
				{
					this._settlementParties = value;
					base.OnPropertyChangedWithValue<SettlementNameplatePartyMarkersVM>(value, "SettlementParties");
				}
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x06000338 RID: 824 RVA: 0x0000E7FA File Offset: 0x0000C9FA
		// (set) Token: 0x06000339 RID: 825 RVA: 0x0000E802 File Offset: 0x0000CA02
		public SettlementNameplateEventsVM SettlementEvents
		{
			get
			{
				return this._settlementEvents;
			}
			set
			{
				if (value != this._settlementEvents)
				{
					this._settlementEvents = value;
					base.OnPropertyChangedWithValue<SettlementNameplateEventsVM>(value, "SettlementEvents");
				}
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600033A RID: 826 RVA: 0x0000E820 File Offset: 0x0000CA20
		// (set) Token: 0x0600033B RID: 827 RVA: 0x0000E828 File Offset: 0x0000CA28
		public int Relation
		{
			get
			{
				return this._relation;
			}
			set
			{
				if (value != this._relation)
				{
					this._relation = value;
					base.OnPropertyChangedWithValue(value, "Relation");
				}
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000E846 File Offset: 0x0000CA46
		// (set) Token: 0x0600033D RID: 829 RVA: 0x0000E84E File Offset: 0x0000CA4E
		public int MapEventVisualType
		{
			get
			{
				return this._mapEventVisualType;
			}
			set
			{
				if (value != this._mapEventVisualType)
				{
					this._mapEventVisualType = value;
					base.OnPropertyChangedWithValue(value, "MapEventVisualType");
				}
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000E86C File Offset: 0x0000CA6C
		// (set) Token: 0x0600033F RID: 831 RVA: 0x0000E874 File Offset: 0x0000CA74
		public int WSign
		{
			get
			{
				return this._wSign;
			}
			set
			{
				if (value != this._wSign)
				{
					this._wSign = value;
					base.OnPropertyChangedWithValue(value, "WSign");
				}
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000E892 File Offset: 0x0000CA92
		// (set) Token: 0x06000341 RID: 833 RVA: 0x0000E89A File Offset: 0x0000CA9A
		public float WPos
		{
			get
			{
				return this._wPos;
			}
			set
			{
				if (value != this._wPos)
				{
					this._wPos = value;
					base.OnPropertyChangedWithValue(value, "WPos");
				}
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000342 RID: 834 RVA: 0x0000E8B8 File Offset: 0x0000CAB8
		// (set) Token: 0x06000343 RID: 835 RVA: 0x0000E8C0 File Offset: 0x0000CAC0
		public BannerImageIdentifierVM Banner
		{
			get
			{
				return this._banner;
			}
			set
			{
				if (value != this._banner)
				{
					this._banner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "Banner");
				}
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000344 RID: 836 RVA: 0x0000E8DE File Offset: 0x0000CADE
		// (set) Token: 0x06000345 RID: 837 RVA: 0x0000E8E6 File Offset: 0x0000CAE6
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x06000346 RID: 838 RVA: 0x0000E909 File Offset: 0x0000CB09
		// (set) Token: 0x06000347 RID: 839 RVA: 0x0000E91B File Offset: 0x0000CB1B
		public bool IsTracked
		{
			get
			{
				return this._isTracked || this._bindIsTargetedByTutorial;
			}
			set
			{
				if (value != this._isTracked)
				{
					this._isTracked = value;
					base.OnPropertyChangedWithValue(value, "IsTracked");
				}
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x06000348 RID: 840 RVA: 0x0000E939 File Offset: 0x0000CB39
		// (set) Token: 0x06000349 RID: 841 RVA: 0x0000E941 File Offset: 0x0000CB41
		public bool IsInside
		{
			get
			{
				return this._isInside;
			}
			set
			{
				if (value != this._isInside)
				{
					this._isInside = value;
					base.OnPropertyChangedWithValue(value, "IsInside");
				}
			}
		}

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x0600034A RID: 842 RVA: 0x0000E95F File Offset: 0x0000CB5F
		// (set) Token: 0x0600034B RID: 843 RVA: 0x0000E968 File Offset: 0x0000CB68
		public bool IsInRange
		{
			get
			{
				return this._isInRange;
			}
			set
			{
				if (value != this._isInRange)
				{
					this._isInRange = value;
					base.OnPropertyChangedWithValue(value, "IsInRange");
					if (this.IsInRange)
					{
						this.SettlementNotifications.RegisterEvents();
						this.SettlementParties.RegisterEvents();
						SettlementNameplateEventsVM settlementEvents = this.SettlementEvents;
						if (settlementEvents == null)
						{
							return;
						}
						settlementEvents.RegisterEvents();
						return;
					}
					else
					{
						this.SettlementNotifications.UnloadEvents();
						this.SettlementParties.UnloadEvents();
						SettlementNameplateEventsVM settlementEvents2 = this.SettlementEvents;
						if (settlementEvents2 == null)
						{
							return;
						}
						settlementEvents2.UnloadEvents();
					}
				}
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x0600034C RID: 844 RVA: 0x0000E9E6 File Offset: 0x0000CBE6
		// (set) Token: 0x0600034D RID: 845 RVA: 0x0000E9EE File Offset: 0x0000CBEE
		public bool HasPort
		{
			get
			{
				return this._hasPort;
			}
			set
			{
				if (value != this._hasPort)
				{
					this._hasPort = value;
					base.OnPropertyChangedWithValue(value, "HasPort");
				}
			}
		}

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x0600034E RID: 846 RVA: 0x0000EA0C File Offset: 0x0000CC0C
		// (set) Token: 0x0600034F RID: 847 RVA: 0x0000EA14 File Offset: 0x0000CC14
		public int PortLevel
		{
			get
			{
				return this._portLevel;
			}
			set
			{
				if (value != this._portLevel)
				{
					this._portLevel = value;
					base.OnPropertyChangedWithValue(value, "PortLevel");
				}
			}
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000350 RID: 848 RVA: 0x0000EA32 File Offset: 0x0000CC32
		// (set) Token: 0x06000351 RID: 849 RVA: 0x0000EA3A File Offset: 0x0000CC3A
		public bool HasFerry
		{
			get
			{
				return this._hasFerry;
			}
			set
			{
				if (value != this._hasFerry)
				{
					this._hasFerry = value;
					base.OnPropertyChangedWithValue(value, "HasFerry");
				}
			}
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x06000352 RID: 850 RVA: 0x0000EA58 File Offset: 0x0000CC58
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0000EA60 File Offset: 0x0000CC60
		public int SettlementType
		{
			get
			{
				return this._settlementType;
			}
			set
			{
				if (value != this._settlementType)
				{
					this._settlementType = value;
					base.OnPropertyChangedWithValue(value, "SettlementType");
				}
			}
		}

		// Token: 0x04000181 RID: 385
		private readonly Camera _mapCamera;

		// Token: 0x04000184 RID: 388
		private float _latestX;

		// Token: 0x04000185 RID: 389
		private float _latestY;

		// Token: 0x04000186 RID: 390
		private float _latestW;

		// Token: 0x04000187 RID: 391
		private float _heightOffset;

		// Token: 0x04000188 RID: 392
		private bool _latestIsInsideWindow;

		// Token: 0x04000189 RID: 393
		private Banner _latestBanner;

		// Token: 0x0400018A RID: 394
		private int _latestBannerVersionNo;

		// Token: 0x0400018B RID: 395
		private bool _isTrackedManually;

		// Token: 0x0400018C RID: 396
		private readonly GameEntity _entity;

		// Token: 0x0400018D RID: 397
		private Vec3 _worldPos;

		// Token: 0x0400018E RID: 398
		private Vec3 _worldPosWithHeight;

		// Token: 0x0400018F RID: 399
		private IFaction _currentFaction;

		// Token: 0x04000190 RID: 400
		private readonly Action<CampaignVec2> _fastMoveCameraToPosition;

		// Token: 0x04000191 RID: 401
		private readonly bool _isVillage;

		// Token: 0x04000192 RID: 402
		private readonly bool _isCastle;

		// Token: 0x04000193 RID: 403
		private readonly bool _isTown;

		// Token: 0x04000194 RID: 404
		private float _wPosAfterPositionCalculation;

		// Token: 0x04000195 RID: 405
		private string _bindName;

		// Token: 0x04000196 RID: 406
		private string _bindFactionColor;

		// Token: 0x04000197 RID: 407
		private bool _bindIsTracked;

		// Token: 0x04000198 RID: 408
		private BannerImageIdentifierVM _bindBanner;

		// Token: 0x04000199 RID: 409
		private int _bindRelation;

		// Token: 0x0400019A RID: 410
		private float _bindWPos;

		// Token: 0x0400019B RID: 411
		private float _bindDistanceToCamera;

		// Token: 0x0400019C RID: 412
		private int _bindWSign;

		// Token: 0x0400019D RID: 413
		private bool _bindIsInside;

		// Token: 0x0400019E RID: 414
		private Vec2 _bindPosition;

		// Token: 0x0400019F RID: 415
		private bool _bindIsVisibleOnMap;

		// Token: 0x040001A0 RID: 416
		private bool _bindIsInRange;

		// Token: 0x040001A1 RID: 417
		private bool _bindHasPort;

		// Token: 0x040001A2 RID: 418
		private int _bindPortLevel;

		// Token: 0x040001A3 RID: 419
		private bool _bindHasFerry;

		// Token: 0x040001A4 RID: 420
		private List<Clan> _rebelliousClans;

		// Token: 0x040001A5 RID: 421
		private string _name;

		// Token: 0x040001A6 RID: 422
		private int _settlementType = -1;

		// Token: 0x040001A7 RID: 423
		private BannerImageIdentifierVM _banner;

		// Token: 0x040001A8 RID: 424
		private int _relation;

		// Token: 0x040001A9 RID: 425
		private int _wSign;

		// Token: 0x040001AA RID: 426
		private float _wPos;

		// Token: 0x040001AB RID: 427
		private bool _isTracked;

		// Token: 0x040001AC RID: 428
		private bool _isInside;

		// Token: 0x040001AD RID: 429
		private bool _isInRange;

		// Token: 0x040001AE RID: 430
		private bool _hasPort;

		// Token: 0x040001AF RID: 431
		private int _portLevel;

		// Token: 0x040001B0 RID: 432
		private bool _hasFerry;

		// Token: 0x040001B1 RID: 433
		private int _mapEventVisualType;

		// Token: 0x040001B2 RID: 434
		private SettlementNameplateNotificationsVM _settlementNotifications;

		// Token: 0x040001B3 RID: 435
		private SettlementNameplatePartyMarkersVM _settlementParties;

		// Token: 0x040001B4 RID: 436
		private SettlementNameplateEventsVM _settlementEvents;

		// Token: 0x02000092 RID: 146
		public enum Type
		{
			// Token: 0x040003CD RID: 973
			Village,
			// Token: 0x040003CE RID: 974
			Castle,
			// Token: 0x040003CF RID: 975
			Town
		}

		// Token: 0x02000093 RID: 147
		public enum RelationType
		{
			// Token: 0x040003D1 RID: 977
			Neutral,
			// Token: 0x040003D2 RID: 978
			SameFaction,
			// Token: 0x040003D3 RID: 979
			Enemy,
			// Token: 0x040003D4 RID: 980
			Ally
		}

		// Token: 0x02000094 RID: 148
		public enum IssueTypes
		{
			// Token: 0x040003D6 RID: 982
			None,
			// Token: 0x040003D7 RID: 983
			Possible,
			// Token: 0x040003D8 RID: 984
			Active
		}

		// Token: 0x02000095 RID: 149
		public enum MainQuestTypes
		{
			// Token: 0x040003DA RID: 986
			None,
			// Token: 0x040003DB RID: 987
			Possible,
			// Token: 0x040003DC RID: 988
			Active
		}
	}
}
