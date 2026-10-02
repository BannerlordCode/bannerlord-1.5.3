using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.CampaignSystem.Actions;
using TaleWorlds.CampaignSystem.Encounters;
using TaleWorlds.CampaignSystem.GameMenus;
using TaleWorlds.CampaignSystem.GameState;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.CampaignSystem.Settlements;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.CampaignSystem.Siege
{
	// Token: 0x020002F3 RID: 755
	public static class PlayerSiege
	{
		// Token: 0x170009F7 RID: 2551
		// (get) Token: 0x06002946 RID: 10566 RVA: 0x000ABC4C File Offset: 0x000A9E4C
		public static SiegeEvent PlayerSiegeEvent
		{
			get
			{
				SiegeEvent siegeEvent;
				if ((siegeEvent = MobileParty.MainParty.SiegeEvent) == null)
				{
					Settlement currentSettlement = MobileParty.MainParty.CurrentSettlement;
					if (currentSettlement == null)
					{
						return null;
					}
					siegeEvent = currentSettlement.SiegeEvent;
				}
				return siegeEvent;
			}
		}

		// Token: 0x170009F8 RID: 2552
		// (get) Token: 0x06002947 RID: 10567 RVA: 0x000ABC71 File Offset: 0x000A9E71
		public static Settlement BesiegedSettlement
		{
			get
			{
				SiegeEvent playerSiegeEvent = PlayerSiege.PlayerSiegeEvent;
				if (playerSiegeEvent == null)
				{
					return null;
				}
				return playerSiegeEvent.BesiegedSettlement;
			}
		}

		// Token: 0x170009F9 RID: 2553
		// (get) Token: 0x06002948 RID: 10568 RVA: 0x000ABC83 File Offset: 0x000A9E83
		public static BattleSideEnum PlayerSide
		{
			get
			{
				if (MobileParty.MainParty.BesiegerCamp == null)
				{
					return BattleSideEnum.Defender;
				}
				return BattleSideEnum.Attacker;
			}
		}

		// Token: 0x170009FA RID: 2554
		// (get) Token: 0x06002949 RID: 10569 RVA: 0x000ABC94 File Offset: 0x000A9E94
		public static bool IsRebellion
		{
			get
			{
				return PlayerSiege.BesiegedSettlement != null && PlayerSiege.BesiegedSettlement.IsUnderRebellionAttack();
			}
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x000ABCA9 File Offset: 0x000A9EA9
		private static void SetPlayerSiegeEvent()
		{
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x000ABCAB File Offset: 0x000A9EAB
		public static void StartSiegePreparation()
		{
			if (Campaign.Current.CurrentMenuContext != null)
			{
				GameMenu.ExitToLast();
			}
			GameMenu.ActivateGameMenu("menu_siege_strategies");
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000ABCC8 File Offset: 0x000A9EC8
		public static void OnSiegeEventFinalized(bool besiegerPartyDefeated)
		{
			MapState mapState = Game.Current.GameStateManager.ActiveState as MapState;
			if (PlayerSiege.IsRebellion)
			{
				if (mapState != null && mapState.AtMenu)
				{
					GameMenu.ExitToLast();
					return;
				}
			}
			else if (PlayerSiege.PlayerSide == BattleSideEnum.Defender && !PlayerSiege.IsRebellion)
			{
				if (Settlement.CurrentSettlement != null)
				{
					if (mapState != null && !mapState.AtMenu)
					{
						GameMenu.ActivateGameMenu(besiegerPartyDefeated ? "siege_attacker_defeated" : "siege_attacker_left");
						return;
					}
					GameMenu.SwitchToMenu(besiegerPartyDefeated ? "siege_attacker_defeated" : "siege_attacker_left");
					return;
				}
			}
			else if (Hero.MainHero.PartyBelongedTo != null && Hero.MainHero.PartyBelongedTo.Army != null && Hero.MainHero.PartyBelongedTo.Army.LeaderParty != MobileParty.MainParty)
			{
				if (MobileParty.MainParty.CurrentSettlement != null)
				{
					LeaveSettlementAction.ApplyForParty(MobileParty.MainParty);
				}
				if (PlayerEncounter.Battle == null)
				{
					if (mapState != null)
					{
						if (mapState.AtMenu)
						{
							GameMenu.SwitchToMenu("army_wait");
							return;
						}
						GameMenu.ActivateGameMenu("army_wait");
						return;
					}
					else
					{
						Campaign.Current.GameMenuManager.SetNextMenu("army_wait");
					}
				}
			}
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000ABDE0 File Offset: 0x000A9FE0
		public static void StartPlayerSiege(BattleSideEnum playerSide, bool isSimulation = false, Settlement settlement = null)
		{
			if (MobileParty.MainParty.Army == null || MobileParty.MainParty.Army.LeaderParty == MobileParty.MainParty)
			{
				MobileParty.MainParty.SetMoveModeHold();
			}
			PlayerSiege.SetPlayerSiegeEvent();
			if (!isSimulation)
			{
				GameState gameState = Game.Current.GameStateManager.GameStates.FirstOrDefault<GameState>((GameState s) => s is MapState);
				if (gameState != null)
				{
					MapState mapState = gameState as MapState;
					if (mapState != null)
					{
						mapState.OnPlayerSiegeActivated();
					}
				}
			}
			CampaignEventDispatcher.Instance.OnPlayerSiegeStarted();
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000ABE74 File Offset: 0x000AA074
		public static void FinalizePlayerSiege()
		{
			if (PlayerSiege.PlayerSiegeEvent == null)
			{
				return;
			}
			PlayerSiege.BesiegedSettlement.Party.SetVisualAsDirty();
			MobileParty.MainParty.SetMoveModeHold();
			GameState gameState = Game.Current.GameStateManager.GameStates.FirstOrDefault<GameState>((GameState s) => s is MapState);
			if (gameState != null)
			{
				MapState mapState = gameState as MapState;
				if (mapState == null)
				{
					return;
				}
				mapState.OnPlayerSiegeDeactivated();
			}
		}

		// Token: 0x0600294F RID: 10575 RVA: 0x000ABEEC File Offset: 0x000AA0EC
		public static void StartSiegeMission(Settlement settlement = null)
		{
			Settlement besiegedSettlement = PlayerSiege.BesiegedSettlement;
			Settlement.SiegeState currentSiegeState = besiegedSettlement.CurrentSiegeState;
			if (currentSiegeState == Settlement.SiegeState.OnTheWalls)
			{
				List<MissionSiegeWeapon> preparedAndActiveSiegeEngines = PlayerSiege.PlayerSiegeEvent.GetPreparedAndActiveSiegeEngines(PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Attacker));
				List<MissionSiegeWeapon> preparedAndActiveSiegeEngines2 = PlayerSiege.PlayerSiegeEvent.GetPreparedAndActiveSiegeEngines(PlayerSiege.PlayerSiegeEvent.GetSiegeEventSide(BattleSideEnum.Defender));
				bool flag = preparedAndActiveSiegeEngines.Exists((MissionSiegeWeapon data) => data.Type == DefaultSiegeEngineTypes.SiegeTower);
				int wallLevel = besiegedSettlement.Town.GetWallLevel();
				CampaignMission.OpenSiegeMissionWithDeployment(besiegedSettlement.LocationComplex.GetLocationWithId("center").GetSceneName(wallLevel), besiegedSettlement.SettlementWallSectionHitPointsRatioList.ToArray(), flag, preparedAndActiveSiegeEngines, preparedAndActiveSiegeEngines2, PlayerEncounter.Current.PlayerSide == BattleSideEnum.Attacker, wallLevel, false, false);
				return;
			}
			if (currentSiegeState != Settlement.SiegeState.Invalid)
			{
				return;
			}
			Debug.FailedAssert("Siege state is invalid!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Siege\\PlayerSiege.cs", "StartSiegeMission", 181);
		}
	}
}
