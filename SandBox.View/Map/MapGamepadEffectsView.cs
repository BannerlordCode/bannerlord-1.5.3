using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Election;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.View.Map
{
	// Token: 0x02000051 RID: 81
	public class MapGamepadEffectsView : MapView
	{
		// Token: 0x06000296 RID: 662 RVA: 0x00017C74 File Offset: 0x00015E74
		protected internal override void CreateLayout()
		{
			base.CreateLayout();
			this.RegisterEvents();
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00017C82 File Offset: 0x00015E82
		protected internal override void OnFinalize()
		{
			base.OnFinalize();
			this.UnregisterEvents();
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00017C90 File Offset: 0x00015E90
		private void RegisterEvents()
		{
			CampaignEvents.VillageBeingRaided.AddNonSerializedListener(this, new Action<Village>(this.OnVillageRaid));
			CampaignEvents.OnSiegeBombardmentWallHitEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType, bool>(this.OnSiegeBombardmentWallHit));
			CampaignEvents.OnSiegeEngineDestroyedEvent.AddNonSerializedListener(this, new Action<MobileParty, Settlement, BattleSideEnum, SiegeEngineType>(this.OnSiegeEngineDestroyed));
			CampaignEvents.WarDeclared.AddNonSerializedListener(this, new Action<IFaction, IFaction, DeclareWarAction.DeclareWarDetail>(this.OnWarDeclared));
			CampaignEvents.OnPeaceOfferedToPlayerEvent.AddNonSerializedListener(this, new Action<IFaction, int, int>(this.OnPeaceOfferedToPlayer));
			CampaignEvents.ArmyDispersed.AddNonSerializedListener(this, new Action<Army, Army.ArmyDispersionReason, bool>(this.OnArmyDispersed));
			CampaignEvents.HeroLevelledUp.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroLevelUp));
			CampaignEvents.KingdomDecisionAdded.AddNonSerializedListener(this, new Action<KingdomDecision, bool>(this.OnKingdomDecisionAdded));
			CampaignEvents.OnMainPartyStarvingEvent.AddNonSerializedListener(this, new Action(this.OnMainPartyStarving));
			CampaignEvents.RebellionFinished.AddNonSerializedListener(this, new Action<Settlement, Clan>(this.OnRebellionFinished));
			CampaignEvents.OnHideoutSpottedEvent.AddNonSerializedListener(this, new Action<PartyBase, PartyBase>(this.OnHideoutSpotted));
			CampaignEvents.HeroCreated.AddNonSerializedListener(this, new Action<Hero, bool>(this.OnHeroCreated));
			CampaignEvents.MakePeace.AddNonSerializedListener(this, new Action<IFaction, IFaction, MakePeaceAction.MakePeaceDetail>(this.OnMakePeace));
			CampaignEvents.HeroOrPartyTradedGold.AddNonSerializedListener(this, new Action<ValueTuple<Hero, PartyBase>, ValueTuple<Hero, PartyBase>, ValueTuple<int, string>, bool>(this.OnHeroOrPartyTradedGold));
			CampaignEvents.PartyAttachedAnotherParty.AddNonSerializedListener(this, new Action<MobileParty>(this.OnPartyAttachedAnotherParty));
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00017DF8 File Offset: 0x00015FF8
		private void UnregisterEvents()
		{
			CampaignEvents.VillageBeingRaided.ClearListeners(this);
			CampaignEvents.OnSiegeBombardmentWallHitEvent.ClearListeners(this);
			CampaignEvents.OnSiegeEngineDestroyedEvent.ClearListeners(this);
			CampaignEvents.WarDeclared.ClearListeners(this);
			CampaignEvents.OnPeaceOfferedToPlayerEvent.ClearListeners(this);
			CampaignEvents.ArmyDispersed.ClearListeners(this);
			CampaignEvents.HeroLevelledUp.ClearListeners(this);
			CampaignEvents.KingdomDecisionAdded.ClearListeners(this);
			CampaignEvents.OnMainPartyStarvingEvent.ClearListeners(this);
			CampaignEvents.RebellionFinished.ClearListeners(this);
			CampaignEvents.OnHideoutSpottedEvent.ClearListeners(this);
			CampaignEvents.HeroCreated.ClearListeners(this);
			CampaignEvents.MakePeace.ClearListeners(this);
			CampaignEvents.HeroOrPartyTradedGold.ClearListeners(this);
			CampaignEvents.PartyAttachedAnotherParty.ClearListeners(this);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x00017EAA File Offset: 0x000160AA
		private void OnVillageRaid(Village village)
		{
			if (MobileParty.MainParty.CurrentSettlement == village.Settlement)
			{
				this.SetRumbleWithRandomValues(0.2f, 0.4f, 5);
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00017ECF File Offset: 0x000160CF
		private void OnSiegeBombardmentWallHit(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType weapon, bool isWallCracked)
		{
			if (isWallCracked && (besiegerParty == MobileParty.MainParty || besiegedSettlement == MobileParty.MainParty.CurrentSettlement))
			{
				this.SetRumbleWithRandomValues(0.3f, 0.8f, 5);
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00017EFB File Offset: 0x000160FB
		private void OnSiegeEngineDestroyed(MobileParty besiegerParty, Settlement besiegedSettlement, BattleSideEnum side, SiegeEngineType destroyedEngine)
		{
			if (besiegerParty == MobileParty.MainParty || besiegedSettlement == MobileParty.MainParty.CurrentSettlement)
			{
				this.SetRumbleWithRandomValues(0.05f, 0.3f, 4);
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x00017F23 File Offset: 0x00016123
		private void OnWarDeclared(IFaction faction1, IFaction faction2, DeclareWarAction.DeclareWarDetail declareWarDetail)
		{
			if (faction1 == Clan.PlayerClan.MapFaction || faction2 == Clan.PlayerClan.MapFaction)
			{
				this.SetRumbleWithRandomValues(0.3f, 0.5f, 3);
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00017F50 File Offset: 0x00016150
		private void OnPeaceOfferedToPlayer(IFaction opponentFaction, int tributeAmount, int tributeDurationInDays)
		{
			this.SetRumbleWithRandomValues(0.2f, 0.4f, 3);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00017F63 File Offset: 0x00016163
		private void OnArmyDispersed(Army army, Army.ArmyDispersionReason reason, bool isPlayersArmy)
		{
			if (isPlayersArmy)
			{
				this.SetRumbleWithRandomValues((float)army.TotalManCount / 2000f, (float)army.TotalManCount / 1000f, 5);
			}
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00017F89 File Offset: 0x00016189
		private void OnHeroLevelUp(Hero hero, bool shouldNotify)
		{
			if (hero == Hero.MainHero && !(GameStateManager.Current.ActiveState is GameLoadingState))
			{
				this.SetRumbleWithRandomValues(0.1f, 0.3f, 3);
			}
		}

		// Token: 0x060002A1 RID: 673 RVA: 0x00017FB5 File Offset: 0x000161B5
		private void OnKingdomDecisionAdded(KingdomDecision decision, bool isPlayerInvolved)
		{
			if (isPlayerInvolved)
			{
				this.SetRumbleWithRandomValues(0.1f, 0.3f, 2);
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00017FCB File Offset: 0x000161CB
		private void OnMainPartyStarving()
		{
			this.SetRumbleWithRandomValues(0.2f, 0.4f, 5);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00017FDE File Offset: 0x000161DE
		private void OnRebellionFinished(Settlement settlement, Clan oldOwnerClan)
		{
			if (oldOwnerClan == Clan.PlayerClan)
			{
				this.SetRumbleWithRandomValues(0.2f, 0.4f, 5);
			}
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00017FF9 File Offset: 0x000161F9
		private void OnHideoutSpotted(PartyBase party, PartyBase hideoutParty)
		{
			this.SetRumbleWithRandomValues(0.1f, 0.3f, 3);
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x0001800C File Offset: 0x0001620C
		private void OnHeroCreated(Hero hero, bool isBornNaturally = false)
		{
			if (hero.Father == Hero.MainHero || hero.Mother == Hero.MainHero)
			{
				this.SetRumbleWithRandomValues(0.2f, 0.4f, 3);
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00018039 File Offset: 0x00016239
		private void OnMakePeace(IFaction side1Faction, IFaction side2Faction, MakePeaceAction.MakePeaceDetail detail)
		{
			if (side1Faction == Clan.PlayerClan.MapFaction || side2Faction == Clan.PlayerClan.MapFaction)
			{
				this.SetRumbleWithRandomValues(0.2f, 0.4f, 3);
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x00018066 File Offset: 0x00016266
		private void OnHeroOrPartyTradedGold(ValueTuple<Hero, PartyBase> giver, ValueTuple<Hero, PartyBase> recipient, ValueTuple<int, string> goldAmount, bool showNotification)
		{
			if (giver.Item1 == Hero.MainHero && Hero.MainHero.Gold == 0)
			{
				this.SetRumbleWithRandomValues(0.1f, 0.3f, 3);
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x00018092 File Offset: 0x00016292
		private void OnPartyAttachedAnotherParty(MobileParty party)
		{
			if (party.Army != null && party.Army.LeaderParty == MobileParty.MainParty)
			{
				this.SetRumbleWithRandomValues(0.1f, 0.3f, 3);
			}
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x000180BF File Offset: 0x000162BF
		protected internal override void OnFrameTick(float dt)
		{
			base.OnFrameTick(dt);
			if (Input.IsKeyDown(InputKey.BackSpace))
			{
				this.SetRumbleWithRandomValues(0.5f, 0f, 5);
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x000180E2 File Offset: 0x000162E2
		private void SetRumbleWithRandomValues(float baseValue = 0f, float offsetRange = 1f, int frequencyCount = 5)
		{
			this.SetRandomRumbleValues(baseValue, offsetRange, frequencyCount);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x000180F0 File Offset: 0x000162F0
		private void SetRandomRumbleValues(float baseValue, float offsetRange, int frequencyCount)
		{
			baseValue = MBMath.ClampFloat(baseValue, 0f, 1f);
			offsetRange = MBMath.ClampFloat(offsetRange, 0f, 1f - baseValue);
			frequencyCount = MBMath.ClampInt(frequencyCount, 2, 5);
			for (int i = 0; i < frequencyCount; i++)
			{
				this._lowFrequencyLevels[i] = baseValue + MBRandom.RandomFloatRanged(offsetRange);
				this._lowFrequencyDurations[i] = baseValue + MBRandom.RandomFloatRanged(offsetRange);
				this._highFrequencyLevels[i] = baseValue + MBRandom.RandomFloatRanged(offsetRange);
				this._highFrequencyDurations[i] = baseValue + MBRandom.RandomFloatRanged(offsetRange);
			}
		}

		// Token: 0x04000173 RID: 371
		private readonly float[] _lowFrequencyLevels = new float[5];

		// Token: 0x04000174 RID: 372
		private readonly float[] _lowFrequencyDurations = new float[5];

		// Token: 0x04000175 RID: 373
		private readonly float[] _highFrequencyLevels = new float[5];

		// Token: 0x04000176 RID: 374
		private readonly float[] _highFrequencyDurations = new float[5];
	}
}
