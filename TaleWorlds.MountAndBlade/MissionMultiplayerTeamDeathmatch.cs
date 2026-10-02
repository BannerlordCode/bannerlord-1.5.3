using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002BF RID: 703
	public class MissionMultiplayerTeamDeathmatch : MissionMultiplayerGameModeBase
	{
		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x0600286A RID: 10346 RVA: 0x000990C8 File Offset: 0x000972C8
		public override bool IsGameModeHidingAllAgentVisuals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x0600286B RID: 10347 RVA: 0x000990CB File Offset: 0x000972CB
		public override bool IsGameModeUsingOpposingTeams
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600286C RID: 10348 RVA: 0x000990CE File Offset: 0x000972CE
		public override MultiplayerGameType GetMissionType()
		{
			return MultiplayerGameType.TeamDeathmatch;
		}

		// Token: 0x0600286D RID: 10349 RVA: 0x000990D1 File Offset: 0x000972D1
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionScoreboardComponent = base.Mission.GetMissionBehavior<MissionScoreboardComponent>();
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000990EC File Offset: 0x000972EC
		public override void AfterStart()
		{
			string strValue = MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			string strValue2 = MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(strValue);
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(strValue2);
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			Banner banner = new Banner(@object.Banner, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint);
			Banner banner2 = new Banner(object2.Banner, multiplayerBattleColors.DefenderColors.BannerBackgroundColorUint, multiplayerBattleColors.DefenderColors.BannerForegroundColorUint);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint, banner, true, false, true);
			base.Mission.Teams.Add(BattleSideEnum.Defender, multiplayerBattleColors.DefenderColors.BannerBackgroundColorUint, multiplayerBattleColors.DefenderColors.BannerForegroundColorUint, banner2, true, false, true);
		}

		// Token: 0x0600286F RID: 10351 RVA: 0x000991C9 File Offset: 0x000973C9
		protected override void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			networkPeer.AddComponent<TeamDeathmatchMissionRepresentative>();
		}

		// Token: 0x06002870 RID: 10352 RVA: 0x000991D2 File Offset: 0x000973D2
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			base.ChangeCurrentGoldForPeer(networkPeer.GetComponent<MissionPeer>(), 120);
			this.GameModeBaseClient.OnGoldAmountChangedForRepresentative(networkPeer.GetComponent<TeamDeathmatchMissionRepresentative>(), 120);
		}

		// Token: 0x06002871 RID: 10353 RVA: 0x000991F5 File Offset: 0x000973F5
		public override void OnPeerChangedTeam(NetworkCommunicator peer, Team oldTeam, Team newTeam)
		{
			if (oldTeam != null && oldTeam != newTeam && oldTeam.Side != BattleSideEnum.None)
			{
				base.ChangeCurrentGoldForPeer(peer.GetComponent<MissionPeer>(), 100);
			}
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x00099218 File Offset: 0x00097418
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (blow.DamageType != DamageTypes.Invalid && (agentState == AgentState.Unconscious || agentState == AgentState.Killed) && affectedAgent.IsHuman)
			{
				if (affectorAgent != null)
				{
					if (affectorAgent.IsEnemyOf(affectedAgent))
					{
						this._missionScoreboardComponent.ChangeTeamScore(affectorAgent.Team, this.GetScoreForKill(affectedAgent));
					}
					else
					{
						this._missionScoreboardComponent.ChangeTeamScore(affectedAgent.Team, -this.GetScoreForKill(affectedAgent));
					}
				}
				MissionPeer missionPeer = affectedAgent.MissionPeer;
				if (missionPeer != null)
				{
					int num = 100;
					if (affectorAgent != affectedAgent)
					{
						List<MissionPeer>[] array = new List<MissionPeer>[2];
						for (int i = 0; i < array.Length; i++)
						{
							array[i] = new List<MissionPeer>();
						}
						foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
						{
							MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
							if (component != null && component.Team != null && component.Team.Side != BattleSideEnum.None)
							{
								array[(int)component.Team.Side].Add(component);
							}
						}
						int num2 = array[1].Count - array[0].Count;
						BattleSideEnum battleSideEnum = ((num2 == 0) ? BattleSideEnum.None : ((num2 < 0) ? BattleSideEnum.Attacker : BattleSideEnum.Defender));
						if (battleSideEnum != BattleSideEnum.None && battleSideEnum == missionPeer.Team.Side)
						{
							num2 = MathF.Abs(num2);
							int count = array[(int)battleSideEnum].Count;
							if (count > 0)
							{
								int num3 = num * num2 / 10 / count * 10;
								num += num3;
							}
						}
					}
					base.ChangeCurrentGoldForPeer(missionPeer, missionPeer.Representative.Gold + num);
				}
				bool flag = ((affectorAgent != null) ? affectorAgent.Team : null) != null && affectedAgent.Team != null && affectorAgent.Team.Side == affectedAgent.Team.Side;
				MultiplayerClassDivisions.MPHeroClass mpheroClassForCharacter = MultiplayerClassDivisions.GetMPHeroClassForCharacter(affectedAgent.Character);
				Agent.Hitter assistingHitter = affectedAgent.GetAssistingHitter((affectorAgent != null) ? affectorAgent.MissionPeer : null);
				if (((affectorAgent != null) ? affectorAgent.MissionPeer : null) != null && affectorAgent != affectedAgent && !affectorAgent.IsFriendOf(affectedAgent))
				{
					TeamDeathmatchMissionRepresentative teamDeathmatchMissionRepresentative = affectorAgent.MissionPeer.Representative as TeamDeathmatchMissionRepresentative;
					int goldGainsFromKillDataAndUpdateFlags = teamDeathmatchMissionRepresentative.GetGoldGainsFromKillDataAndUpdateFlags(MPPerkObject.GetPerkHandler(affectorAgent.MissionPeer), MPPerkObject.GetPerkHandler((assistingHitter != null) ? assistingHitter.HitterPeer : null), mpheroClassForCharacter, false, blow.IsMissile, flag);
					base.ChangeCurrentGoldForPeer(affectorAgent.MissionPeer, teamDeathmatchMissionRepresentative.Gold + goldGainsFromKillDataAndUpdateFlags);
				}
				if (((assistingHitter != null) ? assistingHitter.HitterPeer : null) != null && !assistingHitter.IsFriendlyHit)
				{
					TeamDeathmatchMissionRepresentative teamDeathmatchMissionRepresentative2 = assistingHitter.HitterPeer.Representative as TeamDeathmatchMissionRepresentative;
					int goldGainsFromKillDataAndUpdateFlags2 = teamDeathmatchMissionRepresentative2.GetGoldGainsFromKillDataAndUpdateFlags(MPPerkObject.GetPerkHandler((affectorAgent != null) ? affectorAgent.MissionPeer : null), MPPerkObject.GetPerkHandler(assistingHitter.HitterPeer), mpheroClassForCharacter, true, blow.IsMissile, flag);
					base.ChangeCurrentGoldForPeer(assistingHitter.HitterPeer, teamDeathmatchMissionRepresentative2.Gold + goldGainsFromKillDataAndUpdateFlags2);
				}
				if (((missionPeer != null) ? missionPeer.Team : null) != null)
				{
					MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(missionPeer);
					IEnumerable<ValueTuple<MissionPeer, int>> enumerable = ((perkHandler != null) ? perkHandler.GetTeamGoldRewardsOnDeath() : null);
					if (enumerable != null)
					{
						foreach (ValueTuple<MissionPeer, int> valueTuple in enumerable)
						{
							MissionPeer item = valueTuple.Item1;
							int item2 = valueTuple.Item2;
							TeamDeathmatchMissionRepresentative teamDeathmatchMissionRepresentative3;
							if ((teamDeathmatchMissionRepresentative3 = ((item != null) ? item.Representative : null) as TeamDeathmatchMissionRepresentative) != null)
							{
								int goldGainsFromAllyDeathReward = teamDeathmatchMissionRepresentative3.GetGoldGainsFromAllyDeathReward(item2);
								if (goldGainsFromAllyDeathReward > 0)
								{
									base.ChangeCurrentGoldForPeer(item, teamDeathmatchMissionRepresentative3.Gold + goldGainsFromAllyDeathReward);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x00099594 File Offset: 0x00097794
		public override bool CheckForMatchEnd()
		{
			int minScoreToWinMatch = MultiplayerOptions.OptionType.MinScoreToWinMatch.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			return this._missionScoreboardComponent.Sides.Any<MissionScoreboardComponent.MissionScoreboardSide>((MissionScoreboardComponent.MissionScoreboardSide side) => side.SideScore >= minScoreToWinMatch);
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x000995D4 File Offset: 0x000977D4
		public override Team GetWinnerTeam()
		{
			int intValue = MultiplayerOptions.OptionType.MinScoreToWinMatch.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
			Team team = null;
			MissionScoreboardComponent.MissionScoreboardSide[] sides = this._missionScoreboardComponent.Sides;
			if (sides[1].SideScore < intValue && sides[0].SideScore >= intValue)
			{
				team = base.Mission.Teams.Defender;
			}
			if (sides[0].SideScore < intValue && sides[1].SideScore >= intValue)
			{
				team = base.Mission.Teams.Attacker;
			}
			return team;
		}

		// Token: 0x04000F7A RID: 3962
		public const int MaxScoreToEndMatch = 1023000;

		// Token: 0x04000F7B RID: 3963
		private const int FirstSpawnGold = 120;

		// Token: 0x04000F7C RID: 3964
		private const int RespawnGold = 100;

		// Token: 0x04000F7D RID: 3965
		private MissionScoreboardComponent _missionScoreboardComponent;
	}
}
