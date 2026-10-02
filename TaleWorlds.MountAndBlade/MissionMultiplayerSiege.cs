using System;
using System.Collections.Generic;
using System.Linq;
using NetworkMessages.FromServer;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.MissionRepresentatives;
using TaleWorlds.MountAndBlade.Missions.Multiplayer;
using TaleWorlds.MountAndBlade.Objects;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002BE RID: 702
	public class MissionMultiplayerSiege : MissionMultiplayerGameModeBase, IAnalyticsFlagInfo, IMissionBehavior
	{
		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06002846 RID: 10310 RVA: 0x00097681 File Offset: 0x00095881
		public override bool IsGameModeHidingAllAgentVisuals
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002847 RID: 10311 RVA: 0x00097684 File Offset: 0x00095884
		public override bool IsGameModeUsingOpposingTeams
		{
			get
			{
				return true;
			}
		}

		// Token: 0x14000062 RID: 98
		// (add) Token: 0x06002848 RID: 10312 RVA: 0x00097688 File Offset: 0x00095888
		// (remove) Token: 0x06002849 RID: 10313 RVA: 0x000976C0 File Offset: 0x000958C0
		public event MissionMultiplayerSiege.OnDestructableComponentDestroyedDelegate OnDestructableComponentDestroyed;

		// Token: 0x14000063 RID: 99
		// (add) Token: 0x0600284A RID: 10314 RVA: 0x000976F8 File Offset: 0x000958F8
		// (remove) Token: 0x0600284B RID: 10315 RVA: 0x00097730 File Offset: 0x00095930
		public event MissionMultiplayerSiege.OnObjectiveGoldGainedDelegate OnObjectiveGoldGained;

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x0600284C RID: 10316 RVA: 0x00097765 File Offset: 0x00095965
		// (set) Token: 0x0600284D RID: 10317 RVA: 0x0009776D File Offset: 0x0009596D
		public MBReadOnlyList<FlagCapturePoint> AllCapturePoints { get; private set; }

		// Token: 0x0600284E RID: 10318 RVA: 0x00097778 File Offset: 0x00095978
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._objectiveSystem = new MissionMultiplayerSiege.ObjectiveSystem();
			this._childDestructableComponents = new Dictionary<GameEntity, List<DestructableComponent>>();
			this._gameModeSiegeClient = Mission.Current.GetMissionBehavior<MissionMultiplayerSiegeClient>();
			this._warmupComponent = Mission.Current.GetMissionBehavior<MultiplayerWarmupComponent>();
			this._capturePointOwners = new Team[7];
			this._capturePointRemainingMoraleGains = new int[7];
			this._morales = new int[2];
			this._morales[1] = 360;
			this._morales[0] = 360;
			this.AllCapturePoints = Mission.Current.MissionObjects.FindAllWithType<FlagCapturePoint>().ToMBList<FlagCapturePoint>();
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				flagCapturePoint.SetTeamColorsSynched(4284111450U, uint.MaxValue);
				this._capturePointOwners[flagCapturePoint.FlagIndex] = null;
				this._capturePointRemainingMoraleGains[flagCapturePoint.FlagIndex] = 90;
				if (flagCapturePoint.GameEntity.HasTag("keep_capture_point"))
				{
					this._masterFlag = flagCapturePoint;
				}
			}
			foreach (DestructableComponent destructableComponent in Mission.Current.MissionObjects.FindAllWithType<DestructableComponent>())
			{
				if (destructableComponent.BattleSide != BattleSideEnum.None)
				{
					GameEntity gameEntity = GameEntity.CreateFromWeakEntity(destructableComponent.GameEntity.Root);
					if (this._objectiveSystem.RegisterObjective(gameEntity))
					{
						this._childDestructableComponents.Add(gameEntity, new List<DestructableComponent>());
						MissionMultiplayerSiege.GetDestructableCompoenentClosestToTheRoot(gameEntity).OnDestroyed += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnDestroyed);
					}
					this._childDestructableComponents[gameEntity].Add(destructableComponent);
					destructableComponent.OnHitTaken += new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnHitTaken);
				}
			}
			List<RangedSiegeWeapon> list = new List<RangedSiegeWeapon>();
			List<IMoveableSiegeWeapon> list2 = new List<IMoveableSiegeWeapon>();
			foreach (UsableMachine usableMachine in Mission.Current.MissionObjects.FindAllWithType<UsableMachine>())
			{
				RangedSiegeWeapon rangedSiegeWeapon;
				IMoveableSiegeWeapon moveableSiegeWeapon;
				if ((rangedSiegeWeapon = usableMachine as RangedSiegeWeapon) != null)
				{
					list.Add(rangedSiegeWeapon);
					rangedSiegeWeapon.OnAgentLoadsMachine += this.RangedSiegeMachineOnAgentLoadsMachine;
				}
				else if ((moveableSiegeWeapon = usableMachine as IMoveableSiegeWeapon) != null)
				{
					list2.Add(moveableSiegeWeapon);
					this._objectiveSystem.RegisterObjective(GameEntity.CreateFromWeakEntity(usableMachine.GameEntity.Root));
				}
			}
			this._lastReloadingAgentPerRangedSiegeMachine = new ValueTuple<RangedSiegeWeapon, Agent>[list.Count];
			for (int i = 0; i < this._lastReloadingAgentPerRangedSiegeMachine.Length; i++)
			{
				this._lastReloadingAgentPerRangedSiegeMachine[i] = ValueTuple.Create<RangedSiegeWeapon, Agent>(list[i], null);
			}
			this._movingObjectives = new ValueTuple<IMoveableSiegeWeapon, Vec3>[list2.Count];
			for (int j = 0; j < this._movingObjectives.Length; j++)
			{
				SiegeWeapon siegeWeapon = list2[j] as SiegeWeapon;
				this._movingObjectives[j] = ValueTuple.Create<IMoveableSiegeWeapon, Vec3>(list2[j], siegeWeapon.GameEntity.GlobalPosition);
			}
		}

		// Token: 0x0600284F RID: 10319 RVA: 0x00097ABC File Offset: 0x00095CBC
		private static DestructableComponent GetDestructableCompoenentClosestToTheRoot(GameEntity entity)
		{
			DestructableComponent destructableComponent = entity.GetFirstScriptOfType<DestructableComponent>();
			while (destructableComponent == null && entity.ChildCount != 0)
			{
				for (int i = 0; i < entity.ChildCount; i++)
				{
					destructableComponent = MissionMultiplayerSiege.GetDestructableCompoenentClosestToTheRoot(entity.GetChild(i));
					if (destructableComponent != null)
					{
						break;
					}
				}
			}
			return destructableComponent;
		}

		// Token: 0x06002850 RID: 10320 RVA: 0x00097B00 File Offset: 0x00095D00
		private void RangedSiegeMachineOnAgentLoadsMachine(RangedSiegeWeapon siegeWeapon, Agent reloadingAgent)
		{
			for (int i = 0; i < this._lastReloadingAgentPerRangedSiegeMachine.Length; i++)
			{
				if (this._lastReloadingAgentPerRangedSiegeMachine[i].Item1 == siegeWeapon)
				{
					this._lastReloadingAgentPerRangedSiegeMachine[i].Item2 = reloadingAgent;
				}
			}
		}

		// Token: 0x06002851 RID: 10321 RVA: 0x00097B48 File Offset: 0x00095D48
		private void DestructableComponentOnHitTaken(DestructableComponent destructableComponent, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			if (!this.WarmupComponent.IsInWarmup)
			{
				GameEntity gameEntity = GameEntity.CreateFromWeakEntity(destructableComponent.GameEntity.Root);
				BatteringRam batteringRam;
				if ((batteringRam = attackerScriptComponentBehavior as BatteringRam) != null)
				{
					int userCountNotInStruckAction = batteringRam.UserCountNotInStruckAction;
					if (userCountNotInStruckAction <= 0)
					{
						goto IL_0234;
					}
					float num = (float)inflictedDamage / (float)userCountNotInStruckAction;
					using (List<StandingPoint>.Enumerator enumerator = batteringRam.StandingPoints.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							StandingPoint standingPoint = enumerator.Current;
							Agent userAgent = standingPoint.UserAgent;
							if (((userAgent != null) ? userAgent.MissionPeer : null) != null && !userAgent.IsInBeingStruckAction && userAgent.MissionPeer.Team.Side == destructableComponent.BattleSide.GetOppositeSide())
							{
								this._objectiveSystem.AddContributionForObjective(gameEntity, userAgent.MissionPeer, num);
							}
						}
						goto IL_0234;
					}
				}
				bool flag;
				if (attackerAgent == null)
				{
					flag = null != null;
				}
				else
				{
					MissionPeer missionPeer = attackerAgent.MissionPeer;
					flag = ((missionPeer != null) ? missionPeer.Team : null) != null;
				}
				if (flag && attackerAgent.MissionPeer.Team.Side == destructableComponent.BattleSide.GetOppositeSide())
				{
					StandingPoint standingPoint2;
					if (attackerAgent.CurrentlyUsedGameObject != null && (standingPoint2 = attackerAgent.CurrentlyUsedGameObject as StandingPoint) != null)
					{
						RangedSiegeWeapon firstScriptOfTypeInFamily = standingPoint2.GameEntity.GetFirstScriptOfTypeInFamily<RangedSiegeWeapon>();
						if (firstScriptOfTypeInFamily != null)
						{
							for (int i = 0; i < this._lastReloadingAgentPerRangedSiegeMachine.Length; i++)
							{
								if (this._lastReloadingAgentPerRangedSiegeMachine[i].Item1 == firstScriptOfTypeInFamily)
								{
									Agent item = this._lastReloadingAgentPerRangedSiegeMachine[i].Item2;
									if (((item != null) ? item.MissionPeer : null) != null)
									{
										Agent item2 = this._lastReloadingAgentPerRangedSiegeMachine[i].Item2;
										BattleSideEnum? battleSideEnum = ((item2 != null) ? new BattleSideEnum?(item2.MissionPeer.Team.Side) : null);
										BattleSideEnum oppositeSide = destructableComponent.BattleSide.GetOppositeSide();
										if ((battleSideEnum.GetValueOrDefault() == oppositeSide) & (battleSideEnum != null))
										{
											this._objectiveSystem.AddContributionForObjective(gameEntity, this._lastReloadingAgentPerRangedSiegeMachine[i].Item2.MissionPeer, (float)inflictedDamage * 0.33f);
										}
									}
								}
							}
						}
					}
					this._objectiveSystem.AddContributionForObjective(gameEntity, attackerAgent.MissionPeer, (float)inflictedDamage);
				}
				IL_0234:
				if (destructableComponent.IsDestroyed)
				{
					destructableComponent.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnHitTaken);
					this._childDestructableComponents[gameEntity].Remove(destructableComponent);
				}
			}
		}

		// Token: 0x06002852 RID: 10322 RVA: 0x00097DC8 File Offset: 0x00095FC8
		private void DestructableComponentOnDestroyed(DestructableComponent destructableComponent, Agent attackerAgent, in MissionWeapon weapon, ScriptComponentBehavior attackerScriptComponentBehavior, int inflictedDamage)
		{
			GameEntity gameEntity = GameEntity.CreateFromWeakEntity(destructableComponent.GameEntity.Root);
			List<KeyValuePair<MissionPeer, float>> allContributorsForSideAndClear = this._objectiveSystem.GetAllContributorsForSideAndClear(gameEntity, destructableComponent.BattleSide.GetOppositeSide());
			float num = allContributorsForSideAndClear.Sum<KeyValuePair<MissionPeer, float>>((KeyValuePair<MissionPeer, float> ac) => ac.Value);
			List<MissionPeer> list = new List<MissionPeer>();
			foreach (KeyValuePair<MissionPeer, float> keyValuePair in allContributorsForSideAndClear)
			{
				int goldGainsFromObjectiveAssist = (keyValuePair.Key.Representative as SiegeMissionRepresentative).GetGoldGainsFromObjectiveAssist(gameEntity, keyValuePair.Value / num, false);
				if (goldGainsFromObjectiveAssist > 0)
				{
					base.ChangeCurrentGoldForPeer(keyValuePair.Key, keyValuePair.Key.Representative.Gold + goldGainsFromObjectiveAssist);
					list.Add(keyValuePair.Key);
					MissionMultiplayerSiege.OnObjectiveGoldGainedDelegate onObjectiveGoldGained = this.OnObjectiveGoldGained;
					if (onObjectiveGoldGained != null)
					{
						onObjectiveGoldGained(keyValuePair.Key, goldGainsFromObjectiveAssist);
					}
				}
			}
			destructableComponent.OnDestroyed -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnDestroyed);
			foreach (DestructableComponent destructableComponent2 in this._childDestructableComponents[gameEntity])
			{
				destructableComponent2.OnHitTaken -= new DestructableComponent.OnHitTakenAndDestroyedDelegate(this.DestructableComponentOnHitTaken);
			}
			this._childDestructableComponents.Remove(gameEntity);
			MissionMultiplayerSiege.OnDestructableComponentDestroyedDelegate onDestructableComponentDestroyed = this.OnDestructableComponentDestroyed;
			if (onDestructableComponentDestroyed == null)
			{
				return;
			}
			onDestructableComponentDestroyed(destructableComponent, attackerScriptComponentBehavior, list.ToArray());
		}

		// Token: 0x06002853 RID: 10323 RVA: 0x00097F64 File Offset: 0x00096164
		public override MultiplayerGameType GetMissionType()
		{
			return MultiplayerGameType.Siege;
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x00097F67 File Offset: 0x00096167
		public override bool UseRoundController()
		{
			return false;
		}

		// Token: 0x06002855 RID: 10325 RVA: 0x00097F6C File Offset: 0x0009616C
		public override void AfterStart()
		{
			BasicCultureObject @object = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			BasicCultureObject object2 = MBObjectManager.Instance.GetObject<BasicCultureObject>(MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			MultiplayerBattleColors multiplayerBattleColors = MultiplayerBattleColors.CreateWith(@object, object2);
			Banner banner = new Banner(@object.Banner, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint);
			Banner banner2 = new Banner(object2.Banner, multiplayerBattleColors.DefenderColors.BannerBackgroundColorUint, multiplayerBattleColors.DefenderColors.BannerForegroundColorUint);
			base.Mission.Teams.Add(BattleSideEnum.Attacker, multiplayerBattleColors.AttackerColors.BannerBackgroundColorUint, multiplayerBattleColors.AttackerColors.BannerForegroundColorUint, banner, true, false, true);
			base.Mission.Teams.Add(BattleSideEnum.Defender, multiplayerBattleColors.DefenderColors.BannerBackgroundColorUint, multiplayerBattleColors.DefenderColors.BannerForegroundColorUint, banner2, true, false, true);
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				this._capturePointOwners[flagCapturePoint.FlagIndex] = base.Mission.Teams.Defender;
				flagCapturePoint.SetTeamColors(base.Mission.Teams.Defender.Color, base.Mission.Teams.Defender.Color2);
				MissionMultiplayerSiegeClient gameModeSiegeClient = this._gameModeSiegeClient;
				if (gameModeSiegeClient != null)
				{
					gameModeSiegeClient.OnCapturePointOwnerChanged(flagCapturePoint, base.Mission.Teams.Defender);
				}
			}
			if (this._warmupComponent != null)
			{
				this._warmupComponent.OnWarmupEnding += this.OnWarmupEnding;
			}
		}

		// Token: 0x06002856 RID: 10326 RVA: 0x00098118 File Offset: 0x00096318
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (!this._firstTickDone)
			{
				foreach (CastleGate castleGate in Mission.Current.MissionObjects.FindAllWithType<CastleGate>())
				{
					castleGate.OpenDoor();
					foreach (StandingPoint standingPoint in castleGate.StandingPoints)
					{
						standingPoint.SetIsDeactivatedSynched(true);
					}
				}
				this._firstTickDone = true;
			}
			if (this.MissionLobbyComponent.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing && (this.WarmupComponent == null || !this.WarmupComponent.IsInWarmup))
			{
				this.CheckMorales(dt);
				if (this.CheckObjectives(dt))
				{
					this.TickFlags(dt);
					this.TickObjectives(dt);
				}
			}
		}

		// Token: 0x06002857 RID: 10327 RVA: 0x00098204 File Offset: 0x00096404
		private void CheckMorales(float dt)
		{
			this._dtSumCheckMorales += dt;
			if (this._dtSumCheckMorales >= 1f)
			{
				this._dtSumCheckMorales -= 1f;
				int num = MathF.Max(this._morales[1] + this.GetMoraleGain(BattleSideEnum.Attacker), 0);
				int num2 = MBMath.ClampInt(this._morales[0] + this.GetMoraleGain(BattleSideEnum.Defender), 0, 360);
				GameNetwork.BeginBroadcastModuleEvent();
				GameNetwork.WriteMessage(new SiegeMoraleChangeMessage(num, num2, this._capturePointRemainingMoraleGains));
				GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
				MissionMultiplayerSiegeClient gameModeSiegeClient = this._gameModeSiegeClient;
				if (gameModeSiegeClient != null)
				{
					gameModeSiegeClient.OnMoraleChanged(num, num2, this._capturePointRemainingMoraleGains);
				}
				this._morales[1] = num;
				this._morales[0] = num2;
			}
		}

		// Token: 0x06002858 RID: 10328 RVA: 0x000982BD File Offset: 0x000964BD
		public override bool CheckForMatchEnd()
		{
			return this._morales.Any<int>((int morale) => morale == 0);
		}

		// Token: 0x06002859 RID: 10329 RVA: 0x000982EC File Offset: 0x000964EC
		public override Team GetWinnerTeam()
		{
			Team team = null;
			if (this._morales[1] <= 0 && this._morales[0] > 0)
			{
				team = base.Mission.Teams.Defender;
			}
			if (this._morales[0] <= 0 && this._morales[1] > 0)
			{
				team = base.Mission.Teams.Attacker;
			}
			team = team ?? base.Mission.Teams.Defender;
			base.Mission.GetMissionBehavior<MissionScoreboardComponent>().ChangeTeamScore(team, 1);
			return team;
		}

		// Token: 0x0600285A RID: 10330 RVA: 0x00098374 File Offset: 0x00096574
		private int GetMoraleGain(BattleSideEnum side)
		{
			int num = 0;
			bool flag2 = this._masterFlagBestAgent != null && this._masterFlagBestAgent.Team.Side == side;
			if (side == BattleSideEnum.Attacker)
			{
				if (!flag2)
				{
					num += -1;
				}
				using (IEnumerator<FlagCapturePoint> enumerator = this.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint flag) => flag != this._masterFlag && !flag.IsDeactivated && flag.IsFullyRaised && this.GetFlagOwnerTeam(flag).Side == BattleSideEnum.Attacker).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FlagCapturePoint flagCapturePoint = enumerator.Current;
						this._capturePointRemainingMoraleGains[flagCapturePoint.FlagIndex]--;
						num++;
						if (this._capturePointRemainingMoraleGains[flagCapturePoint.FlagIndex] == 0)
						{
							num += 90;
							foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
							{
								MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
								if (component != null)
								{
									Team team = component.Team;
									BattleSideEnum? battleSideEnum = ((team != null) ? new BattleSideEnum?(team.Side) : null);
									if ((battleSideEnum.GetValueOrDefault() == side) & (battleSideEnum != null))
									{
										base.ChangeCurrentGoldForPeer(component, base.GetCurrentGoldForPeer(component) + 35);
									}
								}
							}
							flagCapturePoint.RemovePointAsServer();
							(base.SpawnComponent.SpawnFrameBehavior as SiegeSpawnFrameBehavior).OnFlagDeactivated(flagCapturePoint);
							this._gameModeSiegeClient.OnNumberOfFlagsChanged();
							GameNetwork.BeginBroadcastModuleEvent();
							GameNetwork.WriteMessage(new FlagDominationFlagsRemovedMessage());
							GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
							this.NotificationsComponent.FlagsXRemoved(flagCapturePoint);
						}
					}
					return num;
				}
			}
			if (this._masterFlag.IsFullyRaised)
			{
				if (this.GetFlagOwnerTeam(this._masterFlag).Side == BattleSideEnum.Attacker)
				{
					if (!flag2)
					{
						int num2 = 0;
						for (int i = 0; i < this.AllCapturePoints.Count; i++)
						{
							if (this.AllCapturePoints[i] != this._masterFlag && !this.AllCapturePoints[i].IsDeactivated)
							{
								num2++;
							}
						}
						num += -6 + num2;
					}
				}
				else
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600285B RID: 10331 RVA: 0x000985A8 File Offset: 0x000967A8
		public Team GetFlagOwnerTeam(FlagCapturePoint flag)
		{
			return this._capturePointOwners[flag.FlagIndex];
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x000985B7 File Offset: 0x000967B7
		private bool CheckObjectives(float dt)
		{
			this._dtSumObjectiveCheck += dt;
			if (this._dtSumObjectiveCheck >= 0.25f)
			{
				this._dtSumObjectiveCheck -= 0.25f;
				return true;
			}
			return false;
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000985EC File Offset: 0x000967EC
		private void TickFlags(float dt)
		{
			foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints)
			{
				if (!flagCapturePoint.IsDeactivated)
				{
					Team flagOwnerTeam = this.GetFlagOwnerTeam(flagCapturePoint);
					Agent agent = null;
					float num = float.MaxValue;
					AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, flagCapturePoint.Position.AsVec2, 4f, false);
					while (proximityMapSearchStruct.LastFoundAgent != null)
					{
						Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
						if (!lastFoundAgent.IsMount && lastFoundAgent.IsActive())
						{
							float num2 = lastFoundAgent.Position.DistanceSquared(flagCapturePoint.Position);
							if (num2 <= 16f && num2 < num)
							{
								agent = lastFoundAgent;
								num = num2;
							}
						}
						AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
					}
					if (flagCapturePoint == this._masterFlag)
					{
						this._masterFlagBestAgent = agent;
					}
					CaptureTheFlagFlagDirection captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.None;
					bool isContested = flagCapturePoint.IsContested;
					if (flagOwnerTeam == null)
					{
						if (!isContested && agent != null)
						{
							captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Down;
						}
						else if (agent == null && isContested)
						{
							captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Up;
						}
					}
					else if (agent != null)
					{
						if (agent.Team != flagOwnerTeam && !isContested)
						{
							captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Down;
						}
						else if (agent.Team == flagOwnerTeam && isContested)
						{
							captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Up;
						}
					}
					else if (isContested)
					{
						captureTheFlagFlagDirection = CaptureTheFlagFlagDirection.Up;
					}
					if (captureTheFlagFlagDirection != CaptureTheFlagFlagDirection.None)
					{
						flagCapturePoint.SetMoveFlag(captureTheFlagFlagDirection, 1f);
					}
					bool flag;
					flagCapturePoint.OnAfterTick(agent != null, out flag);
					if (flag)
					{
						Team team = agent.Team;
						uint num3 = ((team != null) ? team.Color : 4284111450U);
						uint num4 = ((team != null) ? team.Color2 : uint.MaxValue);
						flagCapturePoint.SetTeamColorsSynched(num3, num4);
						this._capturePointOwners[flagCapturePoint.FlagIndex] = team;
						GameNetwork.BeginBroadcastModuleEvent();
						GameNetwork.WriteMessage(new FlagDominationCapturePointMessage(flagCapturePoint.FlagIndex, (team != null) ? team.TeamIndex : (-1)));
						GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
						MissionMultiplayerSiegeClient gameModeSiegeClient = this._gameModeSiegeClient;
						if (gameModeSiegeClient != null)
						{
							gameModeSiegeClient.OnCapturePointOwnerChanged(flagCapturePoint, team);
						}
						this.NotificationsComponent.FlagXCapturedByTeamX(flagCapturePoint, agent.Team);
					}
				}
			}
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x00098808 File Offset: 0x00096A08
		private void TickObjectives(float dt)
		{
			for (int i = this._movingObjectives.Length - 1; i >= 0; i--)
			{
				IMoveableSiegeWeapon item = this._movingObjectives[i].Item1;
				if (item != null)
				{
					SiegeWeapon siegeWeapon = item as SiegeWeapon;
					if (siegeWeapon.IsDeactivated || siegeWeapon.IsDestroyed || siegeWeapon.IsDisabled)
					{
						this._movingObjectives[i].Item1 = null;
					}
					else
					{
						if (item.MovementComponent.HasArrivedAtTarget)
						{
							this._movingObjectives[i].Item1 = null;
							GameEntity gameEntity = GameEntity.CreateFromWeakEntity(siegeWeapon.GameEntity.Root);
							List<KeyValuePair<MissionPeer, float>> allContributorsForSideAndClear = this._objectiveSystem.GetAllContributorsForSideAndClear(gameEntity, BattleSideEnum.Attacker);
							float num = allContributorsForSideAndClear.Sum<KeyValuePair<MissionPeer, float>>((KeyValuePair<MissionPeer, float> ac) => ac.Value);
							using (List<KeyValuePair<MissionPeer, float>>.Enumerator enumerator = allContributorsForSideAndClear.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<MissionPeer, float> keyValuePair = enumerator.Current;
									int goldGainsFromObjectiveAssist = (keyValuePair.Key.Representative as SiegeMissionRepresentative).GetGoldGainsFromObjectiveAssist(gameEntity, keyValuePair.Value / num, true);
									if (goldGainsFromObjectiveAssist > 0)
									{
										base.ChangeCurrentGoldForPeer(keyValuePair.Key, keyValuePair.Key.Representative.Gold + goldGainsFromObjectiveAssist);
										MissionMultiplayerSiege.OnObjectiveGoldGainedDelegate onObjectiveGoldGained = this.OnObjectiveGoldGained;
										if (onObjectiveGoldGained != null)
										{
											onObjectiveGoldGained(keyValuePair.Key, goldGainsFromObjectiveAssist);
										}
									}
								}
								goto IL_0231;
							}
						}
						WeakGameEntity gameEntity2 = siegeWeapon.GameEntity;
						Vec3 item2 = this._movingObjectives[i].Item2;
						Vec3 globalPosition = gameEntity2.GlobalPosition;
						float lengthSquared = (globalPosition - item2).LengthSquared;
						if (lengthSquared > 1f)
						{
							this._movingObjectives[i].Item2 = globalPosition;
							foreach (StandingPoint standingPoint in siegeWeapon.StandingPoints)
							{
								Agent userAgent = standingPoint.UserAgent;
								if (((userAgent != null) ? userAgent.MissionPeer : null) != null && userAgent.MissionPeer.Team.Side == siegeWeapon.Side)
								{
									this._objectiveSystem.AddContributionForObjective(GameEntity.CreateFromWeakEntity(gameEntity2.Root), userAgent.MissionPeer, lengthSquared);
								}
							}
						}
					}
				}
				IL_0231:;
			}
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x00098A70 File Offset: 0x00096C70
		private void OnWarmupEnding()
		{
			this.NotificationsComponent.WarmupEnding();
		}

		// Token: 0x06002860 RID: 10336 RVA: 0x00098A80 File Offset: 0x00096C80
		public override bool CheckForWarmupEnd()
		{
			int[] array = new int[2];
			foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
			{
				MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
				if (networkCommunicator.IsSynchronized && ((component != null) ? component.Team : null) != null && component.Team.Side != BattleSideEnum.None)
				{
					array[(int)component.Team.Side]++;
				}
			}
			return array.Sum() >= MultiplayerOptions.OptionType.MaxNumberOfPlayers.GetIntValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
		}

		// Token: 0x06002861 RID: 10337 RVA: 0x00098B24 File Offset: 0x00096D24
		protected override void HandleEarlyNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			networkPeer.AddComponent<SiegeMissionRepresentative>();
		}

		// Token: 0x06002862 RID: 10338 RVA: 0x00098B30 File Offset: 0x00096D30
		protected override void HandleNewClientAfterSynchronized(NetworkCommunicator networkPeer)
		{
			int num = 120;
			if (this._warmupComponent != null && this._warmupComponent.IsInWarmup)
			{
				num = 160;
			}
			base.ChangeCurrentGoldForPeer(networkPeer.GetComponent<MissionPeer>(), num);
			MissionMultiplayerSiegeClient gameModeSiegeClient = this._gameModeSiegeClient;
			if (gameModeSiegeClient != null)
			{
				gameModeSiegeClient.OnGoldAmountChangedForRepresentative(networkPeer.GetComponent<SiegeMissionRepresentative>(), num);
			}
			if (this.AllCapturePoints != null && !networkPeer.IsServerPeer)
			{
				foreach (FlagCapturePoint flagCapturePoint in this.AllCapturePoints.Where<FlagCapturePoint>((FlagCapturePoint cp) => !cp.IsDeactivated))
				{
					GameNetwork.BeginModuleEventAsServer(networkPeer);
					int flagIndex = flagCapturePoint.FlagIndex;
					Team team = this._capturePointOwners[flagCapturePoint.FlagIndex];
					GameNetwork.WriteMessage(new FlagDominationCapturePointMessage(flagIndex, (team != null) ? team.TeamIndex : (-1)));
					GameNetwork.EndModuleEventAsServer();
				}
			}
		}

		// Token: 0x06002863 RID: 10339 RVA: 0x00098C28 File Offset: 0x00096E28
		public override void OnPeerChangedTeam(NetworkCommunicator peer, Team oldTeam, Team newTeam)
		{
			if (this.MissionLobbyComponent.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing && oldTeam != null && oldTeam != newTeam)
			{
				base.ChangeCurrentGoldForPeer(peer.GetComponent<MissionPeer>(), 100);
			}
		}

		// Token: 0x06002864 RID: 10340 RVA: 0x00098C50 File Offset: 0x00096E50
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (this.MissionLobbyComponent.CurrentMultiplayerState == MissionLobbyComponent.MultiplayerGameState.Playing && blow.DamageType != DamageTypes.Invalid && (agentState == AgentState.Unconscious || agentState == AgentState.Killed) && affectedAgent.IsHuman)
			{
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
				if (((affectorAgent != null) ? affectorAgent.MissionPeer : null) != null && affectorAgent != affectedAgent && affectedAgent.Team != affectorAgent.Team)
				{
					SiegeMissionRepresentative siegeMissionRepresentative = affectorAgent.MissionPeer.Representative as SiegeMissionRepresentative;
					int goldGainsFromKillDataAndUpdateFlags = siegeMissionRepresentative.GetGoldGainsFromKillDataAndUpdateFlags(MPPerkObject.GetPerkHandler(affectorAgent.MissionPeer), MPPerkObject.GetPerkHandler((assistingHitter != null) ? assistingHitter.HitterPeer : null), mpheroClassForCharacter, false, blow.IsMissile, flag);
					base.ChangeCurrentGoldForPeer(affectorAgent.MissionPeer, siegeMissionRepresentative.Gold + goldGainsFromKillDataAndUpdateFlags);
				}
				if (((assistingHitter != null) ? assistingHitter.HitterPeer : null) != null && !assistingHitter.IsFriendlyHit)
				{
					SiegeMissionRepresentative siegeMissionRepresentative2 = assistingHitter.HitterPeer.Representative as SiegeMissionRepresentative;
					int goldGainsFromKillDataAndUpdateFlags2 = siegeMissionRepresentative2.GetGoldGainsFromKillDataAndUpdateFlags(MPPerkObject.GetPerkHandler((affectorAgent != null) ? affectorAgent.MissionPeer : null), MPPerkObject.GetPerkHandler(assistingHitter.HitterPeer), mpheroClassForCharacter, true, blow.IsMissile, flag);
					base.ChangeCurrentGoldForPeer(assistingHitter.HitterPeer, siegeMissionRepresentative2.Gold + goldGainsFromKillDataAndUpdateFlags2);
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
							SiegeMissionRepresentative siegeMissionRepresentative3;
							if (item2 > 0 && (siegeMissionRepresentative3 = ((item != null) ? item.Representative : null) as SiegeMissionRepresentative) != null)
							{
								int goldGainsFromAllyDeathReward = siegeMissionRepresentative3.GetGoldGainsFromAllyDeathReward(item2);
								if (goldGainsFromAllyDeathReward > 0)
								{
									base.ChangeCurrentGoldForPeer(item, siegeMissionRepresentative3.Gold + goldGainsFromAllyDeathReward);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06002865 RID: 10341 RVA: 0x00098FA8 File Offset: 0x000971A8
		protected override void HandleNewClientAfterLoadingFinished(NetworkCommunicator networkPeer)
		{
			GameNetwork.BeginBroadcastModuleEvent();
			GameNetwork.WriteMessage(new SiegeMoraleChangeMessage(this._morales[1], this._morales[0], this._capturePointRemainingMoraleGains));
			GameNetwork.EndBroadcastModuleEvent(GameNetwork.EventBroadcastFlags.None, null);
		}

		// Token: 0x06002866 RID: 10342 RVA: 0x00098FD6 File Offset: 0x000971D6
		public override void OnRemoveBehavior()
		{
			base.OnRemoveBehavior();
			if (this._warmupComponent != null)
			{
				this._warmupComponent.OnWarmupEnding -= this.OnWarmupEnding;
			}
		}

		// Token: 0x06002867 RID: 10343 RVA: 0x00099000 File Offset: 0x00097200
		public override void OnClearScene()
		{
			base.OnClearScene();
			foreach (CastleGate castleGate in Mission.Current.MissionObjects.FindAllWithType<CastleGate>())
			{
				foreach (StandingPoint standingPoint in castleGate.StandingPoints)
				{
					standingPoint.SetIsDeactivatedSynched(false);
				}
			}
		}

		// Token: 0x04000F59 RID: 3929
		public const int NumberOfFlagsInGame = 7;

		// Token: 0x04000F5A RID: 3930
		public const int NumberOfFlagsAffectingMoraleInGame = 6;

		// Token: 0x04000F5B RID: 3931
		public const int MaxMorale = 1440;

		// Token: 0x04000F5C RID: 3932
		public const int StartingMorale = 360;

		// Token: 0x04000F5D RID: 3933
		private const int FirstSpawnGold = 120;

		// Token: 0x04000F5E RID: 3934
		private const int FirstSpawnGoldForEarlyJoin = 160;

		// Token: 0x04000F5F RID: 3935
		private const int RespawnGold = 100;

		// Token: 0x04000F60 RID: 3936
		private const float ObjectiveCheckPeriod = 0.25f;

		// Token: 0x04000F61 RID: 3937
		private const float MoraleTickTimeInSeconds = 1f;

		// Token: 0x04000F62 RID: 3938
		public const int MaxMoraleGainPerFlag = 90;

		// Token: 0x04000F63 RID: 3939
		private const int MoraleBoostOnFlagRemoval = 90;

		// Token: 0x04000F64 RID: 3940
		private const int MoraleDecayInTick = -1;

		// Token: 0x04000F65 RID: 3941
		private const int MoraleDecayOnDefenderInTick = -6;

		// Token: 0x04000F66 RID: 3942
		public const int MoraleGainPerFlag = 1;

		// Token: 0x04000F67 RID: 3943
		public const int GoldBonusOnFlagRemoval = 35;

		// Token: 0x04000F68 RID: 3944
		public const string MasterFlagTag = "keep_capture_point";

		// Token: 0x04000F6B RID: 3947
		private int[] _morales;

		// Token: 0x04000F6C RID: 3948
		private Agent _masterFlagBestAgent;

		// Token: 0x04000F6D RID: 3949
		private FlagCapturePoint _masterFlag;

		// Token: 0x04000F6E RID: 3950
		private Team[] _capturePointOwners;

		// Token: 0x04000F6F RID: 3951
		private int[] _capturePointRemainingMoraleGains;

		// Token: 0x04000F71 RID: 3953
		private float _dtSumCheckMorales;

		// Token: 0x04000F72 RID: 3954
		private float _dtSumObjectiveCheck;

		// Token: 0x04000F73 RID: 3955
		private MissionMultiplayerSiege.ObjectiveSystem _objectiveSystem;

		// Token: 0x04000F74 RID: 3956
		private ValueTuple<IMoveableSiegeWeapon, Vec3>[] _movingObjectives;

		// Token: 0x04000F75 RID: 3957
		private ValueTuple<RangedSiegeWeapon, Agent>[] _lastReloadingAgentPerRangedSiegeMachine;

		// Token: 0x04000F76 RID: 3958
		private MissionMultiplayerSiegeClient _gameModeSiegeClient;

		// Token: 0x04000F77 RID: 3959
		private MultiplayerWarmupComponent _warmupComponent;

		// Token: 0x04000F78 RID: 3960
		private Dictionary<GameEntity, List<DestructableComponent>> _childDestructableComponents;

		// Token: 0x04000F79 RID: 3961
		private bool _firstTickDone;

		// Token: 0x020005A6 RID: 1446
		private class ObjectiveSystem
		{
			// Token: 0x06003E9E RID: 16030 RVA: 0x000F8488 File Offset: 0x000F6688
			public ObjectiveSystem()
			{
				this._objectiveContributorMap = new Dictionary<GameEntity, List<MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor>[]>();
			}

			// Token: 0x06003E9F RID: 16031 RVA: 0x000F849C File Offset: 0x000F669C
			public bool RegisterObjective(GameEntity entity)
			{
				if (!this._objectiveContributorMap.ContainsKey(entity))
				{
					this._objectiveContributorMap.Add(entity, new List<MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor>[2]);
					for (int i = 0; i < 2; i++)
					{
						this._objectiveContributorMap[entity][i] = new List<MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor>();
					}
					return true;
				}
				return false;
			}

			// Token: 0x06003EA0 RID: 16032 RVA: 0x000F84EC File Offset: 0x000F66EC
			public void AddContributionForObjective(GameEntity objectiveEntity, MissionPeer contributorPeer, float contribution)
			{
				string text = objectiveEntity.Tags.FirstOrDefault<string>((string x) => x.StartsWith("mp_siege_objective_")) ?? "";
				bool flag = false;
				for (int i = 0; i < 2; i++)
				{
					foreach (MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor objectiveContributor in this._objectiveContributorMap[objectiveEntity][i])
					{
						if (objectiveContributor.Peer == contributorPeer)
						{
							Debug.Print(string.Format("[CONT > {0}] Increased contribution for {1}({2}) by {3}.", new object[]
							{
								text,
								contributorPeer.Name,
								contributorPeer.Team.Side.ToString(),
								contribution
							}), 0, Debug.DebugColor.White, 17179869184UL);
							objectiveContributor.IncreaseAmount(contribution);
							flag = true;
							break;
						}
					}
					if (flag)
					{
						break;
					}
				}
				if (!flag)
				{
					Debug.Print(string.Format("[CONT > {0}] Adding {1} contribution for {2}({3}).", new object[]
					{
						text,
						contribution,
						contributorPeer.Name,
						contributorPeer.Team.Side.ToString()
					}), 0, Debug.DebugColor.White, 17179869184UL);
					this._objectiveContributorMap[objectiveEntity][(int)contributorPeer.Team.Side].Add(new MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor(contributorPeer, contribution));
				}
			}

			// Token: 0x06003EA1 RID: 16033 RVA: 0x000F8674 File Offset: 0x000F6874
			public List<KeyValuePair<MissionPeer, float>> GetAllContributorsForSideAndClear(GameEntity objectiveEntity, BattleSideEnum side)
			{
				List<KeyValuePair<MissionPeer, float>> list = new List<KeyValuePair<MissionPeer, float>>();
				string text = objectiveEntity.Tags.FirstOrDefault<string>((string x) => x.StartsWith("mp_siege_objective_")) ?? "";
				foreach (MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor objectiveContributor in this._objectiveContributorMap[objectiveEntity][(int)side])
				{
					Debug.Print(string.Format("[CONT > {0}] Rewarding {1} contribution for {2}({3}).", new object[]
					{
						text,
						objectiveContributor.Contribution,
						objectiveContributor.Peer.Name,
						side.ToString()
					}), 0, Debug.DebugColor.White, 17179869184UL);
					list.Add(new KeyValuePair<MissionPeer, float>(objectiveContributor.Peer, objectiveContributor.Contribution));
				}
				this._objectiveContributorMap[objectiveEntity][(int)side].Clear();
				return list;
			}

			// Token: 0x04001F1B RID: 7963
			private readonly Dictionary<GameEntity, List<MissionMultiplayerSiege.ObjectiveSystem.ObjectiveContributor>[]> _objectiveContributorMap;

			// Token: 0x020006CE RID: 1742
			private class ObjectiveContributor
			{
				// Token: 0x17000B2F RID: 2863
				// (get) Token: 0x06004310 RID: 17168 RVA: 0x00101487 File Offset: 0x000FF687
				// (set) Token: 0x06004311 RID: 17169 RVA: 0x0010148F File Offset: 0x000FF68F
				public float Contribution { get; private set; }

				// Token: 0x06004312 RID: 17170 RVA: 0x00101498 File Offset: 0x000FF698
				public ObjectiveContributor(MissionPeer peer, float initialContribution)
				{
					this.Peer = peer;
					this.Contribution = initialContribution;
				}

				// Token: 0x06004313 RID: 17171 RVA: 0x001014AE File Offset: 0x000FF6AE
				public void IncreaseAmount(float deltaContribution)
				{
					this.Contribution += deltaContribution;
				}

				// Token: 0x040023D1 RID: 9169
				public readonly MissionPeer Peer;
			}
		}

		// Token: 0x020005A7 RID: 1447
		// (Invoke) Token: 0x06003EA3 RID: 16035
		public delegate void OnDestructableComponentDestroyedDelegate(DestructableComponent destructableComponent, ScriptComponentBehavior attackerScriptComponentBehaviour, MissionPeer[] contributors);

		// Token: 0x020005A8 RID: 1448
		// (Invoke) Token: 0x06003EA7 RID: 16039
		public delegate void OnObjectiveGoldGainedDelegate(MissionPeer peer, int goldGain);
	}
}
