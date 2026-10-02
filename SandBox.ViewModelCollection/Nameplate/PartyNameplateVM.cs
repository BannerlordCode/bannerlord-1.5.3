using System;
using System.Collections.Generic;
using Helpers;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Issues;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.CampaignSystem.ViewModelCollection;
using TaleWorlds.CampaignSystem.ViewModelCollection.Quests;
using TaleWorlds.Core;
using TaleWorlds.Core.ViewModelCollection.ImageIdentifiers;
using TaleWorlds.Core.ViewModelCollection.Tutorial;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.ViewModelCollection.Nameplate
{
	// Token: 0x0200001A RID: 26
	public class PartyNameplateVM : NameplateVM
	{
		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000271 RID: 625 RVA: 0x0000ABCF File Offset: 0x00008DCF
		// (set) Token: 0x06000272 RID: 626 RVA: 0x0000ABD7 File Offset: 0x00008DD7
		public MobileParty Party { get; private set; }

		// Token: 0x06000273 RID: 627 RVA: 0x0000ABE0 File Offset: 0x00008DE0
		public PartyNameplateVM()
		{
			this.Quests = new MBBindingList<QuestMarkerVM>();
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000AC08 File Offset: 0x00008E08
		public void InitializeWith(MobileParty party, Camera mapCamera)
		{
			this._mapCamera = mapCamera;
			this.Party = party;
			this._isPartyBannerDirty = true;
			this.Quests.Clear();
			this.RegisterEvents();
		}

		// Token: 0x06000275 RID: 629 RVA: 0x0000AC30 File Offset: 0x00008E30
		public virtual void Clear()
		{
			this._mapCamera = null;
			this.Party = null;
			this._isPartyBannerDirty = false;
			this._latestNameTextObject = null;
			this._previousQuestsBind = CampaignUIHelper.IssueQuestFlags.None;
			this.Quests.Clear();
			this.OnFinalize();
			this.UnregisterEvents();
		}

		// Token: 0x06000276 RID: 630 RVA: 0x0000AC6C File Offset: 0x00008E6C
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.RefreshDynamicProperties(true);
		}

		// Token: 0x06000277 RID: 631 RVA: 0x0000AC7C File Offset: 0x00008E7C
		public void RegisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.AddNonSerializedListener(this, new Action<Settlement, bool, Hero, Hero, Hero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail>(this.OnSettlementOwnerChanged));
			CampaignEvents.OnClanChangedKingdomEvent.AddNonSerializedListener(this, new Action<Clan, Kingdom, Kingdom, ChangeKingdomAction.ChangeKingdomActionDetail, bool>(this.OnClanChangeKingdom));
			CampaignEvents.OnClanLeaderChangedEvent.AddNonSerializedListener(this, new Action<Hero, Hero>(this.OnClanLeaderChanged));
			CampaignEvents.OnHeroTeleportationRequestedEvent.AddNonSerializedListener(this, new Action<Hero, Settlement, MobileParty, TeleportHeroAction.TeleportationDetail>(this.OnHeroTeleportationRequested));
			if (Game.Current != null)
			{
				Game.Current.EventManager.RegisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(base.OnTutorialNotificationElementChanged));
			}
		}

		// Token: 0x06000278 RID: 632 RVA: 0x0000AD08 File Offset: 0x00008F08
		public void UnregisterEvents()
		{
			CampaignEvents.OnSettlementOwnerChangedEvent.ClearListeners(this);
			CampaignEvents.OnClanChangedKingdomEvent.ClearListeners(this);
			CampaignEvents.OnClanLeaderChangedEvent.ClearListeners(this);
			CampaignEvents.OnHeroTeleportationRequestedEvent.ClearListeners(this);
			Game.Current.EventManager.UnregisterEvent<TutorialNotificationElementChangeEvent>(new Action<TutorialNotificationElementChangeEvent>(base.OnTutorialNotificationElementChanged));
		}

		// Token: 0x06000279 RID: 633 RVA: 0x0000AD5C File Offset: 0x00008F5C
		private void AddQuestBindFlagsForParty(MobileParty party)
		{
			if (party != MobileParty.MainParty && party != this.Party)
			{
				Hero leaderHero = party.LeaderHero;
				if (((leaderHero != null) ? leaderHero.Issue : null) != null && (this._questsBind & CampaignUIHelper.IssueQuestFlags.TrackedIssue) == CampaignUIHelper.IssueQuestFlags.None && ((this._questsBind & CampaignUIHelper.IssueQuestFlags.AvailableIssue) == CampaignUIHelper.IssueQuestFlags.None || (this._questsBind & CampaignUIHelper.IssueQuestFlags.ActiveIssue) == CampaignUIHelper.IssueQuestFlags.None))
				{
					this._questsBind |= CampaignUIHelper.GetIssueType(party.LeaderHero.Issue);
				}
				if (((this._questsBind & CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest) == CampaignUIHelper.IssueQuestFlags.None && (this._questsBind & CampaignUIHelper.IssueQuestFlags.ActiveIssue) == CampaignUIHelper.IssueQuestFlags.None) || (this._questsBind & CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest) == CampaignUIHelper.IssueQuestFlags.None)
				{
					List<QuestBase> questsRelatedToParty = CampaignUIHelper.GetQuestsRelatedToParty(party);
					for (int i = 0; i < questsRelatedToParty.Count; i++)
					{
						QuestBase questBase = questsRelatedToParty[i];
						if (party.LeaderHero != null && questBase.QuestGiver == party.LeaderHero)
						{
							if (questBase.IsSpecialQuest && (this._questsBind & CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest) == CampaignUIHelper.IssueQuestFlags.None)
							{
								this._questsBind |= CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest;
							}
							else if (!questBase.IsSpecialQuest && (this._questsBind & CampaignUIHelper.IssueQuestFlags.ActiveIssue) == CampaignUIHelper.IssueQuestFlags.None)
							{
								this._questsBind |= CampaignUIHelper.IssueQuestFlags.ActiveIssue;
							}
						}
						else if (questBase.IsSpecialQuest && (this._questsBind & CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest) == CampaignUIHelper.IssueQuestFlags.None)
						{
							this._questsBind |= CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest;
						}
						else if (!questBase.IsSpecialQuest && (this._questsBind & CampaignUIHelper.IssueQuestFlags.TrackedIssue) == CampaignUIHelper.IssueQuestFlags.None)
						{
							this._questsBind |= CampaignUIHelper.IssueQuestFlags.TrackedIssue;
						}
					}
				}
			}
		}

		// Token: 0x0600027A RID: 634 RVA: 0x0000AEB8 File Offset: 0x000090B8
		public override void RefreshDynamicProperties(bool forceUpdate)
		{
			base.RefreshDynamicProperties(forceUpdate);
			if (this._isVisibleOnMapBind || forceUpdate)
			{
				MobileParty party = this.Party;
				IssueBase issueBase;
				if (party == null)
				{
					issueBase = null;
				}
				else
				{
					Hero leaderHero = party.LeaderHero;
					issueBase = ((leaderHero != null) ? leaderHero.Issue : null);
				}
				IssueBase issueBase2 = issueBase;
				this._questsBind = CampaignUIHelper.IssueQuestFlags.None;
				if (this.Party != MobileParty.MainParty)
				{
					if (issueBase2 != null)
					{
						this._questsBind |= CampaignUIHelper.GetIssueType(issueBase2);
					}
					List<QuestBase> questsRelatedToParty = CampaignUIHelper.GetQuestsRelatedToParty(this.Party);
					for (int i = 0; i < questsRelatedToParty.Count; i++)
					{
						QuestBase questBase = questsRelatedToParty[i];
						if (questBase.QuestGiver != null && questBase.QuestGiver == this.Party.LeaderHero)
						{
							this._questsBind |= (questBase.IsSpecialQuest ? CampaignUIHelper.IssueQuestFlags.ActiveStoryQuest : CampaignUIHelper.IssueQuestFlags.ActiveIssue);
						}
						else
						{
							this._questsBind |= (questBase.IsSpecialQuest ? CampaignUIHelper.IssueQuestFlags.TrackedStoryQuest : CampaignUIHelper.IssueQuestFlags.TrackedIssue);
						}
					}
				}
			}
			this._isInArmyBind = this.Party.Army != null && this.Party.AttachedTo != null;
			this._isArmyBind = this.Party.Army != null && this.Party.Army.LeaderParty == this.Party;
			MobileParty party2 = this.Party;
			this._isInSettlementBind = ((party2 != null) ? party2.CurrentSettlement : null) != null;
			if (this._isArmyBind && (this._isVisibleOnMapBind || forceUpdate))
			{
				this.AddQuestBindFlagsForParty(this.Party.Army.LeaderParty);
				for (int j = 0; j < this.Party.Army.LeaderParty.AttachedParties.Count; j++)
				{
					MobileParty mobileParty = this.Party.Army.LeaderParty.AttachedParties[j];
					this.AddQuestBindFlagsForParty(mobileParty);
				}
			}
			if (this._isArmyBind || !this._isInArmy || forceUpdate)
			{
				int partyHealthyCount = SandBoxUIHelper.GetPartyHealthyCount(this.Party);
				if (partyHealthyCount != this._latestTotalCount)
				{
					this._latestTotalCount = partyHealthyCount;
					this._countBind = (this.Party.IsInfoHidden ? "?" : partyHealthyCount.ToString());
				}
				int allWoundedMembersAmount = SandBoxUIHelper.GetAllWoundedMembersAmount(this.Party);
				int allPrisonerMembersAmount = SandBoxUIHelper.GetAllPrisonerMembersAmount(this.Party);
				if (this._latestWoundedAmount != allWoundedMembersAmount || this._latestPrisonerAmount != allPrisonerMembersAmount)
				{
					if (this._latestWoundedAmount != allWoundedMembersAmount)
					{
						this._woundedBind = ((allWoundedMembersAmount == 0) ? "" : (this.Party.IsInfoHidden ? "?" : SandBoxUIHelper.GetPartyWoundedText(allWoundedMembersAmount)));
						this._latestWoundedAmount = allWoundedMembersAmount;
					}
					if (this._latestPrisonerAmount != allPrisonerMembersAmount)
					{
						this._prisonerBind = ((allPrisonerMembersAmount == 0) ? "" : (this.Party.IsInfoHidden ? "?" : SandBoxUIHelper.GetPartyPrisonerText(allPrisonerMembersAmount)));
						this._latestPrisonerAmount = allPrisonerMembersAmount;
					}
					this._extraInfoTextBind = this._woundedBind + this._prisonerBind;
				}
			}
			if (!this.Party.IsMainParty)
			{
				Army army = this.Party.Army;
				if (army == null || !army.LeaderParty.AttachedParties.Contains(MobileParty.MainParty) || !this.Party.Army.LeaderParty.AttachedParties.Contains(this.Party))
				{
					Hero mainHero = Hero.MainHero;
					if (((mainHero != null) ? mainHero.MapFaction : null) != null)
					{
						IFaction mapFaction = this.Party.MapFaction;
						Hero mainHero2 = Hero.MainHero;
						if (FactionManager.IsAtWarAgainstFaction(mapFaction, (mainHero2 != null) ? mainHero2.MapFaction : null))
						{
							this._factionColorBind = ((this.Party.Army != null && this.Party.Army.LeaderParty == this.Party) ? PartyNameplateVM.NegativeArmyIndicator : PartyNameplateVM.NegativeIndicator);
							goto IL_04CD;
						}
					}
					if (DiplomacyHelper.IsSameFactionAndNotEliminated(this.Party.MapFaction, Hero.MainHero.MapFaction))
					{
						this._factionColorBind = ((this.Party.Army != null && this.Party.Army.LeaderParty == this.Party) ? PartyNameplateVM.PositiveArmyIndicator : PartyNameplateVM.PositiveIndicator);
						goto IL_04CD;
					}
					IFaction mapFaction2 = this.Party.MapFaction;
					Hero mainHero3 = Hero.MainHero;
					if (DiplomacyHelper.HasAllianceWithFaction(mapFaction2, (mainHero3 != null) ? mainHero3.MapFaction : null))
					{
						this._factionColorBind = ((this.Party.Army != null && this.Party.Army.LeaderParty == this.Party) ? PartyNameplateVM.AllianceArmyIndicator : PartyNameplateVM.AllianceIndicator);
						goto IL_04CD;
					}
					this._factionColorBind = ((this.Party.Army != null && this.Party.Army.LeaderParty == this.Party) ? PartyNameplateVM.NeutralArmyIndicator : PartyNameplateVM.NeutralIndicator);
					goto IL_04CD;
				}
			}
			this._factionColorBind = ((this.Party.Army != null && this.Party.Army.LeaderParty == this.Party) ? PartyNameplateVM.MainPartyArmyIndicator : PartyNameplateVM.MainPartyIndicator);
			IL_04CD:
			if (this._isPartyBannerDirty || forceUpdate)
			{
				this.PartyBanner = new BannerImageIdentifierVM(this.Party.Banner, true);
				this._isPartyBannerDirty = false;
			}
			if (this._isVisibleOnMapBind && (this._isInArmyBind || this._isInSettlementBind || (!this.Party.IsMainParty && this.Party.IsInNavalAutoTravel)))
			{
				this._isVisibleOnMapBind = false;
			}
			Army army2 = this.Party.Army;
			TextObject textObject;
			if (army2 != null && army2.DoesLeaderPartyAndAttachedPartiesContain(this.Party))
			{
				textObject = this.Party.ArmyName;
			}
			else if (this.Party.LeaderHero != null)
			{
				textObject = this.Party.LeaderHero.Name;
			}
			else
			{
				textObject = this.Party.Name;
			}
			this._isDisorganizedBind = this.Party.IsDisorganized;
			if (this._latestNameTextObject == null || forceUpdate || !this._latestNameTextObject.Equals(textObject))
			{
				this._latestNameTextObject = textObject;
				this._fullNameBind = this._latestNameTextObject.ToString();
			}
			if (this.Party.IsActive && !this._cachedSpeed.ApproximatelyEqualsTo(this.Party.Speed, 0.01f))
			{
				this._cachedSpeed = this.Party.Speed;
				this._movementSpeedTextBind = this._cachedSpeed.ToString("F1");
			}
			this._isCurrentlyAtSeaBind = this.Party.IsCurrentlyAtSea;
			if (this._isArmyBind)
			{
				this._hasBloodFeudBind = false;
				for (int k = 0; k < this.Party.Army.Parties.Count; k++)
				{
					Clan actualClan = this.Party.Army.Parties[k].ActualClan;
					if (actualClan != null && actualClan.HasBloodFeudWithPlayer)
					{
						this._hasBloodFeudBind = true;
						return;
					}
				}
				return;
			}
			bool flag;
			if (!this.Party.IsMainParty)
			{
				Clan actualClan2 = this.Party.ActualClan;
				flag = actualClan2 != null && actualClan2.HasBloodFeudWithPlayer;
			}
			else
			{
				flag = false;
			}
			this._hasBloodFeudBind = flag;
		}

		// Token: 0x0600027B RID: 635 RVA: 0x0000B588 File Offset: 0x00009788
		public override void RefreshPosition()
		{
			base.RefreshPosition();
			this._latestX = 0f;
			this._latestY = 0f;
			this._latestW = 0f;
			Vec3 vec = (this.Party.Position + this.Party.EventPositionAdder).AsVec3();
			MapEvent mapEvent = this.Party.MapEvent;
			bool flag;
			if (mapEvent == null)
			{
				flag = false;
			}
			else
			{
				Settlement mapEventSettlement = mapEvent.MapEventSettlement;
				bool? flag2 = ((mapEventSettlement != null) ? new bool?(mapEventSettlement.IsVillage) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag && this.Party.IsCurrentlyAtSea)
			{
				MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, vec, ref this._latestX, ref this._latestY, ref this._latestW);
				this._shipBannerPositionBind = new Vec2(this._latestX, this._latestY);
				this._isShipBannerVisibleBind = this._latestW > 0f && this._latestW < 100f && this._mapCamera.Position.z < 200f;
				vec = this.Party.MapEvent.MapEventSettlement.GatePosition.AsVec3();
				vec += new Vec3(this.Party.RandomFloatWithSeed((uint)this.Party.RandomValue, -0.3f, 0.3f), this.Party.RandomFloatWithSeed((uint)this.Party.RandomValue, -0.3f, 0.3f), 0f, -1f);
			}
			else
			{
				this._isShipBannerVisibleBind = false;
			}
			Vec3 vec2 = vec + new Vec3(0f, 0f, 0.8f, -1f);
			MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, vec, ref this._latestX, ref this._latestY, ref this._latestW);
			this._partyPositionBind = new Vec2(this._latestX, this._latestY);
			MBWindowManager.WorldToScreenInsideUsableArea(this._mapCamera, vec2, ref this._latestX, ref this._latestY, ref this._latestW);
			this._headPositionBind = new Vec2(this._latestX, this._latestY);
			base.DistanceToCamera = vec.Distance(this._mapCamera.Position);
		}

		// Token: 0x0600027C RID: 636 RVA: 0x0000B7D0 File Offset: 0x000099D0
		public override void RefreshTutorialStatus(string newTutorialHighlightElementID)
		{
			base.RefreshTutorialStatus(newTutorialHighlightElementID);
			MobileParty party = this.Party;
			bool flag;
			if (party == null)
			{
				flag = null != null;
			}
			else
			{
				PartyBase party2 = party.Party;
				flag = ((party2 != null) ? party2.Id : null) != null;
			}
			if (!flag)
			{
				Debug.FailedAssert("Mobile party id is null when refreshing tutorial status", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\SandBox.ViewModelCollection\\Nameplate\\PartyNameplateVM.cs", "RefreshTutorialStatus", 388);
				return;
			}
			this._bindIsTargetedByTutorial = this.Party.Party.Id == newTutorialHighlightElementID;
		}

		// Token: 0x0600027D RID: 637 RVA: 0x0000B840 File Offset: 0x00009A40
		public void DetermineIsVisibleOnMap()
		{
			this._isVisibleOnMapBind = this._latestW < 100f && this._latestW > 0f && this._mapCamera.Position.z < 200f;
		}

		// Token: 0x0600027E RID: 638 RVA: 0x0000B87C File Offset: 0x00009A7C
		private bool IsInsideWindow()
		{
			return this._latestX <= Screen.RealScreenResolutionWidth && this._latestY <= Screen.RealScreenResolutionHeight && this._latestX + 100f >= 0f && this._latestY + 30f >= 0f;
		}

		// Token: 0x0600027F RID: 639 RVA: 0x0000B8D0 File Offset: 0x00009AD0
		public virtual void RefreshBinding()
		{
			base.Position = this._partyPositionBind;
			this.HeadPosition = this._headPositionBind;
			this.ShipBannerPosition = this._shipBannerPositionBind;
			this.IsShipBannerVisible = this._isShipBannerVisibleBind;
			base.IsVisibleOnMap = this._isVisibleOnMapBind;
			this.IsInSettlement = this._isInSettlementBind;
			base.FactionColor = this._factionColorBind;
			this.IsHigh = this._isHighBind;
			this.Count = this._countBind;
			this.Prisoner = this._prisonerBind;
			this.Wounded = this._woundedBind;
			this.IsBehind = this._isBehindBind;
			this.FullName = this._fullNameBind;
			base.IsTargetedByTutorial = this._bindIsTargetedByTutorial;
			this.IsInArmy = this._isInArmyBind;
			this.IsArmy = this._isArmyBind;
			this.ExtraInfoText = this._extraInfoTextBind;
			this.IsDisorganized = this._isDisorganizedBind;
			this.MovementSpeedText = this._movementSpeedTextBind;
			this.IsCurrentlyAtSea = this._isCurrentlyAtSeaBind;
			this.HasBloodFeud = this._hasBloodFeudBind;
			if (this._previousQuestsBind != this._questsBind)
			{
				this.Quests.Clear();
				for (int i = 0; i < CampaignUIHelper.IssueQuestFlagsValues.Length; i++)
				{
					CampaignUIHelper.IssueQuestFlags issueQuestFlags = CampaignUIHelper.IssueQuestFlagsValues[i];
					if (issueQuestFlags != CampaignUIHelper.IssueQuestFlags.None && (this._questsBind & issueQuestFlags) != CampaignUIHelper.IssueQuestFlags.None)
					{
						this.Quests.Add(new QuestMarkerVM(issueQuestFlags, null, null));
					}
				}
				this._previousQuestsBind = this._questsBind;
			}
		}

		// Token: 0x06000280 RID: 640 RVA: 0x0000BA38 File Offset: 0x00009C38
		private void OnSettlementOwnerChanged(Settlement settlement, bool openToClaim, Hero newOwner, Hero oldOwner, Hero capturerHero, ChangeOwnerOfSettlementAction.ChangeOwnerOfSettlementDetail detail)
		{
			bool flag = this.Party.HomeSettlement != null && (this.Party.HomeSettlement.IsVillage ? settlement.BoundVillages.Contains(this.Party.HomeSettlement.Village) : (this.Party.HomeSettlement == settlement));
			if ((this.Party.IsCaravan || this.Party.IsVillager) && flag)
			{
				this._isPartyBannerDirty = true;
			}
		}

		// Token: 0x06000281 RID: 641 RVA: 0x0000BAB9 File Offset: 0x00009CB9
		private void OnClanChangeKingdom(Clan arg1, Kingdom arg2, Kingdom arg3, ChangeKingdomAction.ChangeKingdomActionDetail arg4, bool showNotification)
		{
			Hero leaderHero = this.Party.LeaderHero;
			if (((leaderHero != null) ? leaderHero.Clan : null) == arg1)
			{
				this._isPartyBannerDirty = true;
			}
		}

		// Token: 0x06000282 RID: 642 RVA: 0x0000BADC File Offset: 0x00009CDC
		private void OnClanLeaderChanged(Hero arg1, Hero arg2)
		{
			if (arg2.MapFaction == this.Party.MapFaction)
			{
				this._isPartyBannerDirty = true;
			}
		}

		// Token: 0x06000283 RID: 643 RVA: 0x0000BAF8 File Offset: 0x00009CF8
		private void OnHeroTeleportationRequested(Hero arg1, Settlement arg2, MobileParty arg3, TeleportHeroAction.TeleportationDetail arg4)
		{
			if (arg1.MapFaction == this.Party.MapFaction)
			{
				this._isPartyBannerDirty = true;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000284 RID: 644 RVA: 0x0000BB14 File Offset: 0x00009D14
		// (set) Token: 0x06000285 RID: 645 RVA: 0x0000BB1C File Offset: 0x00009D1C
		public Vec2 HeadPosition
		{
			get
			{
				return this._headPosition;
			}
			set
			{
				if (value != this._headPosition)
				{
					this._headPosition = value;
					base.OnPropertyChangedWithValue(value, "HeadPosition");
				}
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000286 RID: 646 RVA: 0x0000BB3F File Offset: 0x00009D3F
		// (set) Token: 0x06000287 RID: 647 RVA: 0x0000BB47 File Offset: 0x00009D47
		public Vec2 ShipBannerPosition
		{
			get
			{
				return this._shipBannerPosition;
			}
			set
			{
				if (value != this._shipBannerPosition)
				{
					this._shipBannerPosition = value;
					base.OnPropertyChangedWithValue(value, "ShipBannerPosition");
				}
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000288 RID: 648 RVA: 0x0000BB6A File Offset: 0x00009D6A
		// (set) Token: 0x06000289 RID: 649 RVA: 0x0000BB72 File Offset: 0x00009D72
		public bool IsShipBannerVisible
		{
			get
			{
				return this._isShipBannerVisible;
			}
			set
			{
				if (value != this._isShipBannerVisible)
				{
					this._isShipBannerVisible = value;
					base.OnPropertyChangedWithValue(value, "IsShipBannerVisible");
				}
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x0600028A RID: 650 RVA: 0x0000BB90 File Offset: 0x00009D90
		// (set) Token: 0x0600028B RID: 651 RVA: 0x0000BB98 File Offset: 0x00009D98
		public string Count
		{
			get
			{
				return this._count;
			}
			set
			{
				if (value != this._count)
				{
					this._count = value;
					base.OnPropertyChangedWithValue<string>(value, "Count");
				}
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600028C RID: 652 RVA: 0x0000BBBB File Offset: 0x00009DBB
		// (set) Token: 0x0600028D RID: 653 RVA: 0x0000BBC3 File Offset: 0x00009DC3
		public string Prisoner
		{
			get
			{
				return this._prisoner;
			}
			set
			{
				if (value != this._prisoner)
				{
					this._prisoner = value;
					base.OnPropertyChangedWithValue<string>(value, "Prisoner");
				}
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600028E RID: 654 RVA: 0x0000BBE6 File Offset: 0x00009DE6
		// (set) Token: 0x0600028F RID: 655 RVA: 0x0000BBEE File Offset: 0x00009DEE
		public MBBindingList<QuestMarkerVM> Quests
		{
			get
			{
				return this._quests;
			}
			set
			{
				if (value != this._quests)
				{
					this._quests = value;
					base.OnPropertyChangedWithValue<MBBindingList<QuestMarkerVM>>(value, "Quests");
				}
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000290 RID: 656 RVA: 0x0000BC0C File Offset: 0x00009E0C
		// (set) Token: 0x06000291 RID: 657 RVA: 0x0000BC14 File Offset: 0x00009E14
		public string Wounded
		{
			get
			{
				return this._wounded;
			}
			set
			{
				if (value != this._wounded)
				{
					this._wounded = value;
					base.OnPropertyChangedWithValue<string>(value, "Wounded");
				}
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000292 RID: 658 RVA: 0x0000BC37 File Offset: 0x00009E37
		// (set) Token: 0x06000293 RID: 659 RVA: 0x0000BC3F File Offset: 0x00009E3F
		public string ExtraInfoText
		{
			get
			{
				return this._extraInfoText;
			}
			set
			{
				if (value != this._extraInfoText)
				{
					this._extraInfoText = value;
					base.OnPropertyChangedWithValue<string>(value, "ExtraInfoText");
				}
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000294 RID: 660 RVA: 0x0000BC62 File Offset: 0x00009E62
		// (set) Token: 0x06000295 RID: 661 RVA: 0x0000BC6A File Offset: 0x00009E6A
		public string MovementSpeedText
		{
			get
			{
				return this._movementSpeedText;
			}
			set
			{
				if (value != this._movementSpeedText)
				{
					this._movementSpeedText = value;
					base.OnPropertyChangedWithValue<string>(value, "MovementSpeedText");
				}
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000296 RID: 662 RVA: 0x0000BC8D File Offset: 0x00009E8D
		// (set) Token: 0x06000297 RID: 663 RVA: 0x0000BC95 File Offset: 0x00009E95
		public string FullName
		{
			get
			{
				return this._fullName;
			}
			set
			{
				if (value != this._fullName)
				{
					this._fullName = value;
					base.OnPropertyChangedWithValue<string>(value, "FullName");
				}
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000298 RID: 664 RVA: 0x0000BCB8 File Offset: 0x00009EB8
		// (set) Token: 0x06000299 RID: 665 RVA: 0x0000BCC0 File Offset: 0x00009EC0
		public bool IsInArmy
		{
			get
			{
				return this._isInArmy;
			}
			set
			{
				if (value != this._isInArmy)
				{
					this._isInArmy = value;
					base.OnPropertyChangedWithValue(value, "IsInArmy");
				}
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x0600029A RID: 666 RVA: 0x0000BCDE File Offset: 0x00009EDE
		// (set) Token: 0x0600029B RID: 667 RVA: 0x0000BCE6 File Offset: 0x00009EE6
		public bool IsInSettlement
		{
			get
			{
				return this._isInSettlement;
			}
			set
			{
				if (value != this._isInSettlement)
				{
					this._isInSettlement = value;
					base.OnPropertyChangedWithValue(value, "IsInSettlement");
				}
			}
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x0600029C RID: 668 RVA: 0x0000BD04 File Offset: 0x00009F04
		// (set) Token: 0x0600029D RID: 669 RVA: 0x0000BD0C File Offset: 0x00009F0C
		public bool IsDisorganized
		{
			get
			{
				return this._isDisorganized;
			}
			set
			{
				if (value != this._isDisorganized)
				{
					this._isDisorganized = value;
					base.OnPropertyChangedWithValue(value, "IsDisorganized");
				}
			}
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600029E RID: 670 RVA: 0x0000BD2A File Offset: 0x00009F2A
		// (set) Token: 0x0600029F RID: 671 RVA: 0x0000BD32 File Offset: 0x00009F32
		public bool IsCurrentlyAtSea
		{
			get
			{
				return this._isCurrentlyAtSea;
			}
			set
			{
				if (value != this._isCurrentlyAtSea)
				{
					this._isCurrentlyAtSea = value;
					base.OnPropertyChangedWithValue(value, "IsCurrentlyAtSea");
				}
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000BD50 File Offset: 0x00009F50
		// (set) Token: 0x060002A1 RID: 673 RVA: 0x0000BD58 File Offset: 0x00009F58
		public bool HasBloodFeud
		{
			get
			{
				return this._hasBloodFeud;
			}
			set
			{
				if (value != this._hasBloodFeud)
				{
					this._hasBloodFeud = value;
					base.OnPropertyChangedWithValue(value, "HasBloodFeud");
				}
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x0000BD76 File Offset: 0x00009F76
		// (set) Token: 0x060002A3 RID: 675 RVA: 0x0000BD7E File Offset: 0x00009F7E
		public bool IsArmy
		{
			get
			{
				return this._isArmy;
			}
			set
			{
				if (value != this._isArmy)
				{
					this._isArmy = value;
					base.OnPropertyChangedWithValue(value, "IsArmy");
				}
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x0000BD9C File Offset: 0x00009F9C
		// (set) Token: 0x060002A5 RID: 677 RVA: 0x0000BDA4 File Offset: 0x00009FA4
		public bool IsBehind
		{
			get
			{
				return this._isBehind;
			}
			set
			{
				if (value != this._isBehind)
				{
					this._isBehind = value;
					base.OnPropertyChangedWithValue(value, "IsBehind");
				}
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000BDC2 File Offset: 0x00009FC2
		// (set) Token: 0x060002A7 RID: 679 RVA: 0x0000BDCA File Offset: 0x00009FCA
		public bool IsHigh
		{
			get
			{
				return this._isHigh;
			}
			set
			{
				if (value != this._isHigh)
				{
					this._isHigh = value;
					base.OnPropertyChangedWithValue(value, "IsHigh");
				}
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x0000BDE8 File Offset: 0x00009FE8
		// (set) Token: 0x060002A9 RID: 681 RVA: 0x0000BDFA File Offset: 0x00009FFA
		public bool ShouldShowFullName
		{
			get
			{
				return this._shouldShowFullName || base.IsTargetedByTutorial;
			}
			set
			{
				if (value != this._shouldShowFullName)
				{
					this._shouldShowFullName = value;
					base.OnPropertyChangedWithValue(value, "ShouldShowFullName");
				}
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002AA RID: 682 RVA: 0x0000BE18 File Offset: 0x0000A018
		// (set) Token: 0x060002AB RID: 683 RVA: 0x0000BE20 File Offset: 0x0000A020
		public BannerImageIdentifierVM PartyBanner
		{
			get
			{
				return this._partyBanner;
			}
			set
			{
				if (value != this._partyBanner)
				{
					this._partyBanner = value;
					base.OnPropertyChangedWithValue<BannerImageIdentifierVM>(value, "PartyBanner");
				}
			}
		}

		// Token: 0x04000119 RID: 281
		public static string PositiveIndicator = Color.FromUint(4285650500U).ToString();

		// Token: 0x0400011A RID: 282
		public static string PositiveArmyIndicator = Color.FromUint(4288804731U).ToString();

		// Token: 0x0400011B RID: 283
		public static string NegativeIndicator = Color.FromUint(4292232774U).ToString();

		// Token: 0x0400011C RID: 284
		public static string NegativeArmyIndicator = Color.FromUint(4294931829U).ToString();

		// Token: 0x0400011D RID: 285
		public static string NeutralIndicator = Color.FromUint(4291877096U).ToString();

		// Token: 0x0400011E RID: 286
		public static string NeutralArmyIndicator = Color.FromUint(4294573055U).ToString();

		// Token: 0x0400011F RID: 287
		public static string MainPartyIndicator = Color.FromUint(4287421380U).ToString();

		// Token: 0x04000120 RID: 288
		public static string MainPartyArmyIndicator = Color.FromUint(4289593317U).ToString();

		// Token: 0x04000121 RID: 289
		public static string AllianceIndicator = Color.FromUint(4279460044U).ToString();

		// Token: 0x04000122 RID: 290
		public static string AllianceArmyIndicator = Color.FromUint(4279476684U).ToString();

		// Token: 0x04000124 RID: 292
		protected float _latestX;

		// Token: 0x04000125 RID: 293
		protected float _latestY;

		// Token: 0x04000126 RID: 294
		protected float _latestW;

		// Token: 0x04000127 RID: 295
		protected float _cachedSpeed;

		// Token: 0x04000128 RID: 296
		protected Camera _mapCamera;

		// Token: 0x04000129 RID: 297
		protected int _latestPrisonerAmount = -1;

		// Token: 0x0400012A RID: 298
		protected int _latestWoundedAmount = -1;

		// Token: 0x0400012B RID: 299
		protected int _latestTotalCount = -1;

		// Token: 0x0400012C RID: 300
		protected bool _isPartyBannerDirty;

		// Token: 0x0400012D RID: 301
		protected TextObject _latestNameTextObject;

		// Token: 0x0400012E RID: 302
		protected CampaignUIHelper.IssueQuestFlags _previousQuestsBind;

		// Token: 0x0400012F RID: 303
		protected CampaignUIHelper.IssueQuestFlags _questsBind;

		// Token: 0x04000130 RID: 304
		protected Vec2 _partyPositionBind;

		// Token: 0x04000131 RID: 305
		protected Vec2 _headPositionBind;

		// Token: 0x04000132 RID: 306
		protected Vec2 _shipBannerPositionBind;

		// Token: 0x04000133 RID: 307
		protected bool _isShipBannerVisibleBind;

		// Token: 0x04000134 RID: 308
		protected bool _isHighBind;

		// Token: 0x04000135 RID: 309
		protected bool _isBehindBind;

		// Token: 0x04000136 RID: 310
		protected bool _isInArmyBind;

		// Token: 0x04000137 RID: 311
		protected bool _isInSettlementBind;

		// Token: 0x04000138 RID: 312
		protected bool _isVisibleOnMapBind;

		// Token: 0x04000139 RID: 313
		protected bool _isArmyBind;

		// Token: 0x0400013A RID: 314
		protected bool _isDisorganizedBind;

		// Token: 0x0400013B RID: 315
		protected bool _isCurrentlyAtSeaBind;

		// Token: 0x0400013C RID: 316
		protected bool _hasBloodFeudBind;

		// Token: 0x0400013D RID: 317
		protected string _factionColorBind;

		// Token: 0x0400013E RID: 318
		protected string _countBind;

		// Token: 0x0400013F RID: 319
		protected string _woundedBind;

		// Token: 0x04000140 RID: 320
		protected string _prisonerBind;

		// Token: 0x04000141 RID: 321
		protected string _extraInfoTextBind;

		// Token: 0x04000142 RID: 322
		protected string _fullNameBind;

		// Token: 0x04000143 RID: 323
		protected string _movementSpeedTextBind;

		// Token: 0x04000144 RID: 324
		private string _count;

		// Token: 0x04000145 RID: 325
		private string _wounded;

		// Token: 0x04000146 RID: 326
		private string _prisoner;

		// Token: 0x04000147 RID: 327
		private MBBindingList<QuestMarkerVM> _quests;

		// Token: 0x04000148 RID: 328
		private string _fullName;

		// Token: 0x04000149 RID: 329
		private string _extraInfoText;

		// Token: 0x0400014A RID: 330
		private string _movementSpeedText;

		// Token: 0x0400014B RID: 331
		private bool _isBehind;

		// Token: 0x0400014C RID: 332
		private bool _isHigh;

		// Token: 0x0400014D RID: 333
		private bool _shouldShowFullName;

		// Token: 0x0400014E RID: 334
		private bool _isInArmy;

		// Token: 0x0400014F RID: 335
		private bool _isArmy;

		// Token: 0x04000150 RID: 336
		private bool _isInSettlement;

		// Token: 0x04000151 RID: 337
		private bool _isDisorganized;

		// Token: 0x04000152 RID: 338
		private bool _isCurrentlyAtSea;

		// Token: 0x04000153 RID: 339
		private bool _hasBloodFeud;

		// Token: 0x04000154 RID: 340
		private bool _isShipBannerVisible;

		// Token: 0x04000155 RID: 341
		private BannerImageIdentifierVM _partyBanner;

		// Token: 0x04000156 RID: 342
		private Vec2 _headPosition;

		// Token: 0x04000157 RID: 343
		private Vec2 _shipBannerPosition;
	}
}
