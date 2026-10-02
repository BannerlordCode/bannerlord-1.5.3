using System;
using Helpers;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.MapEvents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Siege;
using TaleWorlds.Core;
using TaleWorlds.LinQuick;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x02000465 RID: 1125
	public class SiegeAmbushCampaignBehavior : CampaignBehaviorBase
	{
		// Token: 0x060048C3 RID: 18627 RVA: 0x0016BB84 File Offset: 0x00169D84
		public override void RegisterEvents()
		{
			CampaignEvents.OnAfterSessionLaunchedEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnAfterSessionLaunched));
			CampaignEvents.MapEventStarted.AddNonSerializedListener(this, new Action<MapEvent, PartyBase, PartyBase>(this.OnMapEventStarted));
			CampaignEvents.OnSiegeEventEndedEvent.AddNonSerializedListener(this, new Action<SiegeEvent>(this.OnSiegeEventEnded));
			CampaignEvents.HourlyTickEvent.AddNonSerializedListener(this, new Action(this.HourlyTick));
			CampaignEvents.OnMissionEndedEvent.AddNonSerializedListener(this, new Action<IMission>(this.OnMissionEnded));
			CampaignEvents.OnNewGameCreatedPartialFollowUpEndEvent.AddNonSerializedListener(this, new Action<CampaignGameStarter>(this.OnNewGameCreatedPartialFollowUpEnd));
		}

		// Token: 0x060048C4 RID: 18628 RVA: 0x0016BC1B File Offset: 0x00169E1B
		private void OnNewGameCreatedPartialFollowUpEnd(CampaignGameStarter starter)
		{
			this._lastAmbushTime = CampaignTime.Never;
		}

		// Token: 0x060048C5 RID: 18629 RVA: 0x0016BC28 File Offset: 0x00169E28
		public override void SyncData(IDataStore dataStore)
		{
			dataStore.SyncData<CampaignTime>("_lastAmbushTime", ref this._lastAmbushTime);
		}

		// Token: 0x060048C6 RID: 18630 RVA: 0x0016BC3C File Offset: 0x00169E3C
		private void OnAfterSessionLaunched(CampaignGameStarter campaignGameStarter)
		{
			this.AddGameMenus(campaignGameStarter);
		}

		// Token: 0x060048C7 RID: 18631 RVA: 0x0016BC45 File Offset: 0x00169E45
		private void OnMapEventStarted(MapEvent mapEvent, PartyBase attackerParty, PartyBase defenderParty)
		{
			if (mapEvent.IsSiegeAmbush)
			{
				this._lastAmbushTime = CampaignTime.Now;
			}
		}

		// Token: 0x060048C8 RID: 18632 RVA: 0x0016BC5A File Offset: 0x00169E5A
		private void OnSiegeEventEnded(SiegeEvent siegeEvent)
		{
			if (siegeEvent == PlayerSiege.PlayerSiegeEvent)
			{
				this._lastAmbushTime = CampaignTime.Never;
			}
		}

		// Token: 0x060048C9 RID: 18633 RVA: 0x0016BC6F File Offset: 0x00169E6F
		private void HourlyTick()
		{
			if (PlayerSiege.PlayerSiegeEvent != null && this._lastAmbushTime == CampaignTime.Never && PlayerSiege.PlayerSiegeEvent.BesiegerCamp.IsPreparationComplete)
			{
				this._lastAmbushTime = CampaignTime.Now;
			}
		}

		// Token: 0x060048CA RID: 18634 RVA: 0x0016BCA6 File Offset: 0x00169EA6
		private void OnMissionEnded(IMission mission)
		{
			MapEvent battle = PlayerEncounter.Battle;
			if (battle != null && battle.IsSiegeAmbush)
			{
				PlayerEncounter.Current.FinalizeBattle();
				PlayerEncounter.Current.SetIsSallyOutAmbush(false);
			}
		}

		// Token: 0x060048CB RID: 18635 RVA: 0x0016BCD0 File Offset: 0x00169ED0
		private bool CanMainHeroAmbush(out TextObject reason)
		{
			if (this._lastAmbushTime.ElapsedHoursUntilNow < 24f)
			{
				reason = new TextObject("{=lCYPxuWN}The enemy is alert, you cannot ambush right now.", null);
				return false;
			}
			if (Hero.MainHero.IsWounded)
			{
				reason = new TextObject("{=pQaQW1As}You cannot ambush right now due to your wounds.", null);
				return false;
			}
			SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
			if (playerSiegeEvent.BesiegerCamp.LeaderParty.MapEvent != null && MobileParty.MainParty.MapEvent == null)
			{
				reason = new TextObject("{=GAh1gNYn}Enemies are already engaged in battle.", null);
				return false;
			}
			if (playerSiegeEvent.GetPreparedSiegeEnginesAsDictionary(playerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker)).Count <= 0)
			{
				reason = new TextObject("{=f4g7r0xF}The enemy does not have any vulnerabilities.", null);
				return false;
			}
			if (playerSiegeEvent.BesiegedSettlement.SettlementWallSectionHitPointsRatioList.AnyQ<float>((float r) => r <= 0f))
			{
				reason = new TextObject("{=Nzt8Xkro}You cannot ambush because the settlement walls are breached.", null);
				return false;
			}
			reason = TextObject.GetEmpty();
			return true;
		}

		// Token: 0x060048CC RID: 18636 RVA: 0x0016BDB8 File Offset: 0x00169FB8
		private void AddGameMenus(CampaignGameStarter campaignGameSystemStarter)
		{
			campaignGameSystemStarter.AddGameMenuOption("menu_siege_strategies", "menu_siege_strategies_ambush", "{=LEKzuGzi}Ambush", new GameMenuOption.OnConditionDelegate(this.menu_siege_strategies_ambush_condition), new GameMenuOption.OnConsequenceDelegate(this.menu_siege_strategies_ambush_on_consequence), false, -1, false, null);
		}

		// Token: 0x060048CD RID: 18637 RVA: 0x0016BDF8 File Offset: 0x00169FF8
		private bool menu_siege_strategies_ambush_condition(MenuCallbackArgs args)
		{
			if (PlayerSiege.PlayerSiegeEvent == null || PlayerSiege.PlayerSide != BattleSideEnum.Defender)
			{
				return false;
			}
			args.optionLeaveType = GameMenuOption.LeaveType.SiegeAmbush;
			TextObject textObject;
			if (!this.CanMainHeroAmbush(out textObject))
			{
				args.IsEnabled = false;
				args.Tooltip = textObject;
			}
			return true;
		}

		// Token: 0x060048CE RID: 18638 RVA: 0x0016BE38 File Offset: 0x0016A038
		private void menu_siege_strategies_ambush_on_consequence(MenuCallbackArgs args)
		{
			this._lastAmbushTime = CampaignTime.Now;
			if (PlayerEncounter.EncounterSettlement != null && PlayerEncounter.EncounterSettlement.SiegeEvent != null && !PlayerEncounter.EncounterSettlement.MapFaction.IsAtWarWith(MobileParty.MainParty.MapFaction))
			{
				PlayerEncounter.RestartPlayerEncounter(PartyBase.MainParty, PlayerEncounter.EncounterSettlement.SiegeEvent.BesiegerCamp.LeaderParty.Party, false, false);
			}
			PlayerEncounter.Current.SetIsSallyOutAmbush(true);
			PlayerEncounter.StartBattle();
			MenuHelper.EncounterAttackConsequence(args);
		}

		// Token: 0x0400148B RID: 5259
		private const int SiegeAmbushCooldownPeriodAsHours = 24;

		// Token: 0x0400148C RID: 5260
		private CampaignTime _lastAmbushTime;
	}
}
