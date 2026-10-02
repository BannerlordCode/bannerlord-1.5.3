using System;
using System.Collections.Generic;
using System.Linq;
using psai.net;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Sound
{
	// Token: 0x02000085 RID: 133
	public class MusicBattleMissionView : MissionView, IMusicHandler
	{
		// Token: 0x17000093 RID: 147
		// (get) Token: 0x0600052A RID: 1322 RVA: 0x0002616B File Offset: 0x0002436B
		bool IMusicHandler.IsPausable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600052B RID: 1323 RVA: 0x0002616E File Offset: 0x0002436E
		private BattleSideEnum PlayerSide
		{
			get
			{
				Team playerTeam = Mission.Current.PlayerTeam;
				if (playerTeam == null)
				{
					return BattleSideEnum.None;
				}
				return playerTeam.Side;
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x00026185 File Offset: 0x00024385
		public MusicBattleMissionView(bool isSiegeBattle, bool isKeepBattle)
		{
			this._isSiegeBattle = isSiegeBattle;
			this._isKeepBattle = isKeepBattle;
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0002619B File Offset: 0x0002439B
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this._missionAgentSpawnLogic = Mission.Current.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			MBMusicManager.Current.DeactivateCurrentMode();
			MBMusicManager.Current.ActivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerInit(this);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x000261D2 File Offset: 0x000243D2
		public override void OnMissionScreenFinalize()
		{
			MBMusicManager.Current.DeactivateBattleMode();
			MBMusicManager.Current.OnBattleMusicHandlerFinalize();
			base.Mission.PlayerTeam.PlayerOrderController.OnOrderIssued -= new OnOrderIssuedDelegate(this.PlayerOrderControllerOnOrderIssued);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x00026209 File Offset: 0x00024409
		public override void AfterStart()
		{
			this._nextPossibleTimeToIncreaseIntensityForChargeOrder = MissionTime.Now;
			base.Mission.PlayerTeam.PlayerOrderController.OnOrderIssued += new OnOrderIssuedDelegate(this.PlayerOrderControllerOnOrderIssued);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x00026238 File Offset: 0x00024438
		private void PlayerOrderControllerOnOrderIssued(OrderType orderType, IEnumerable<Formation> appliedFormations, OrderController orderController, object[] parameters)
		{
			if ((orderType == OrderType.Charge || orderType == OrderType.ChargeWithTarget) && this._nextPossibleTimeToIncreaseIntensityForChargeOrder.IsPast)
			{
				float currentIntensity = PsaiCore.Instance.GetCurrentIntensity();
				float num = currentIntensity * MusicParameters.PlayerChargeEffectMultiplierOnIntensity - currentIntensity;
				MBMusicManager.Current.ChangeCurrentThemeIntensity(num);
				this._nextPossibleTimeToIncreaseIntensityForChargeOrder = MissionTime.Now + MissionTime.Seconds(60f);
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x00026294 File Offset: 0x00024494
		private void CheckIntensityFall()
		{
			PsaiInfo psaiInfo = PsaiCore.Instance.GetPsaiInfo();
			if (psaiInfo.effectiveThemeId >= 0)
			{
				if (float.IsNaN(psaiInfo.currentIntensity))
				{
					MBMusicManager.Current.ChangeCurrentThemeIntensity(MusicParameters.MinIntensity);
					return;
				}
				if (psaiInfo.currentIntensity < MusicParameters.MinIntensity)
				{
					MBMusicManager.Current.ChangeCurrentThemeIntensity(MusicParameters.MinIntensity - psaiInfo.currentIntensity);
				}
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x000262FC File Offset: 0x000244FC
		public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
			if (this._battleState != MusicBattleMissionView.BattleState.Starting)
			{
				bool flag = affectedAgent.IsMine || (affectedAgent.RiderAgent != null && affectedAgent.RiderAgent.IsMine);
				Team team = affectedAgent.Team;
				BattleSideEnum battleSideEnum = ((team != null) ? team.Side : BattleSideEnum.None);
				bool flag2;
				if (!flag)
				{
					if (battleSideEnum != BattleSideEnum.None)
					{
						Team playerTeam = Mission.Current.PlayerTeam;
						flag2 = ((playerTeam != null) ? playerTeam.Side : BattleSideEnum.None) == battleSideEnum;
					}
					else
					{
						flag2 = false;
					}
				}
				else
				{
					flag2 = true;
				}
				bool flag3 = flag2;
				if (!this._isSiegeBattle && affectedAgent.IsHuman && battleSideEnum != BattleSideEnum.None && this._battleState == MusicBattleMissionView.BattleState.Started && this._startingTroopCounts.Sum() >= MusicParameters.SmallBattleTreshold && MissionTime.Now.ToSeconds > (double)MusicParameters.BattleTurnsOneSideCooldown && this._missionAgentSpawnLogic.NumberOfRemainingTroops == 0)
				{
					int[] array = new int[]
					{
						this._missionAgentSpawnLogic.NumberOfActiveDefenderTroops,
						this._missionAgentSpawnLogic.NumberOfActiveAttackerTroops
					};
					array[(int)battleSideEnum]--;
					MusicTheme musicTheme = MusicTheme.None;
					if (array[0] > 0 && array[1] > 0)
					{
						float num = (float)array[0] / (float)array[1];
						if (num < this._startingBattleRatio * MusicParameters.BattleRatioTresholdOnIntensity)
						{
							musicTheme = MBMusicManager.Current.GetBattleTurnsOneSideTheme(base.Mission.MusicCulture, this.PlayerSide > BattleSideEnum.Defender, this._isPaganBattle);
						}
						else if (num > this._startingBattleRatio / MusicParameters.BattleRatioTresholdOnIntensity)
						{
							musicTheme = MBMusicManager.Current.GetBattleTurnsOneSideTheme(base.Mission.MusicCulture, this.PlayerSide == BattleSideEnum.Defender, this._isPaganBattle);
						}
					}
					if (musicTheme != MusicTheme.None)
					{
						MBMusicManager.Current.StartTheme(musicTheme, PsaiCore.Instance.GetCurrentIntensity(), false);
						this._battleState = MusicBattleMissionView.BattleState.TurnedOneSide;
					}
				}
				if ((affectedAgent.IsHuman && affectedAgent.State != AgentState.Routed) || flag)
				{
					float num2 = (flag3 ? MusicParameters.FriendlyTroopDeadEffectOnIntensity : MusicParameters.EnemyTroopDeadEffectOnIntensity);
					if (flag)
					{
						num2 *= MusicParameters.PlayerTroopDeadEffectMultiplierOnIntensity;
					}
					MBMusicManager.Current.ChangeCurrentThemeIntensity(num2);
				}
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x00026500 File Offset: 0x00024700
		private void CheckForStarting()
		{
			if (this._startingTroopCounts == null)
			{
				int num;
				int num2;
				if (this._isKeepBattle)
				{
					num = Mission.Current.Teams.Defender.ActiveAgents.Count;
					num2 = Mission.Current.Teams.Attacker.ActiveAgents.Count;
				}
				else
				{
					num = this._missionAgentSpawnLogic.GetTotalNumberOfTroopsForSide(BattleSideEnum.Defender);
					num2 = this._missionAgentSpawnLogic.GetTotalNumberOfTroopsForSide(BattleSideEnum.Attacker);
				}
				this._startingTroopCounts = new int[] { num, num2 };
				this._startingBattleRatio = (float)num / (float)num2;
			}
			Agent main = Agent.Main;
			Vec2 vec = ((main != null) ? main.Position.AsVec2 : Vec2.Invalid);
			Team playerTeam = Mission.Current.PlayerTeam;
			bool flag;
			if (playerTeam == null)
			{
				flag = false;
			}
			else
			{
				flag = playerTeam.FormationsIncludingEmpty.Any<Formation>((Formation f) => f.CountOfUnits > 0);
			}
			bool flag2 = flag;
			float num3 = float.MaxValue;
			if (flag2 || vec.IsValid)
			{
				foreach (Formation formation in Mission.Current.PlayerEnemyTeam.FormationsIncludingEmpty)
				{
					if (formation.CountOfUnits > 0)
					{
						float num4 = float.MaxValue;
						if (!flag2 && vec.IsValid)
						{
							num4 = vec.DistanceSquared(formation.CurrentPosition);
						}
						else if (flag2)
						{
							foreach (Formation formation2 in Mission.Current.PlayerTeam.FormationsIncludingEmpty)
							{
								if (formation2.CountOfUnits > 0)
								{
									float num5 = formation2.CurrentPosition.DistanceSquared(formation.CurrentPosition);
									if (num4 > num5)
									{
										num4 = num5;
									}
								}
							}
						}
						if (num3 > num4)
						{
							num3 = num4;
						}
					}
				}
			}
			int num6 = this._startingTroopCounts.Sum();
			bool flag3 = false;
			if (num6 < MusicParameters.SmallBattleTreshold)
			{
				if (num3 < MusicParameters.SmallBattleDistanceTreshold * MusicParameters.SmallBattleDistanceTreshold)
				{
					flag3 = true;
				}
			}
			else if (num6 < MusicParameters.MediumBattleTreshold)
			{
				if (num3 < MusicParameters.MediumBattleDistanceTreshold * MusicParameters.MediumBattleDistanceTreshold)
				{
					flag3 = true;
				}
			}
			else if (num6 < MusicParameters.LargeBattleTreshold)
			{
				if (num3 < MusicParameters.LargeBattleDistanceTreshold * MusicParameters.LargeBattleDistanceTreshold)
				{
					flag3 = true;
				}
			}
			else if (num3 < MusicParameters.MaxBattleDistanceTreshold * MusicParameters.MaxBattleDistanceTreshold)
			{
				flag3 = true;
			}
			flag3 = flag3 || this._isKeepBattle;
			if (flag3)
			{
				float num7 = (float)num6 / 1000f;
				float num8 = MusicParameters.DefaultStartIntensity + num7 * MusicParameters.BattleSizeEffectOnStartIntensity + (MBRandom.RandomFloat - 0.5f) * (MusicParameters.RandomEffectMultiplierOnStartIntensity * 2f);
				MusicTheme musicTheme = (this._isSiegeBattle ? MBMusicManager.Current.GetSiegeTheme(base.Mission.MusicCulture) : MBMusicManager.Current.GetBattleTheme(base.Mission.MusicCulture, num6, out this._isPaganBattle));
				MBMusicManager.Current.StartTheme(musicTheme, num8, false);
				this._battleState = MusicBattleMissionView.BattleState.Started;
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x00026818 File Offset: 0x00024A18
		private void CheckForEnding()
		{
			if (Mission.Current.IsMissionEnding)
			{
				if (Mission.Current.MissionResult != null)
				{
					base.Mission.MusicCulture = Mission.Current.GetMissionBehavior<MissionCombatantsLogic>().GetCultureForPlayerSide();
					MusicTheme battleEndTheme = MBMusicManager.Current.GetBattleEndTheme(base.Mission.MusicCulture, Mission.Current.MissionResult.PlayerVictory);
					MBMusicManager.Current.StartTheme(battleEndTheme, PsaiCore.Instance.GetPsaiInfo().currentIntensity, true);
					this._battleState = MusicBattleMissionView.BattleState.Ending;
					return;
				}
				MBMusicManager.Current.StartTheme(MusicTheme.BattleDefeat, PsaiCore.Instance.GetPsaiInfo().currentIntensity, true);
				this._battleState = MusicBattleMissionView.BattleState.Ending;
			}
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x000268CC File Offset: 0x00024ACC
		void IMusicHandler.OnUpdated(float dt)
		{
			if (this._battleState == MusicBattleMissionView.BattleState.Starting)
			{
				if (base.Mission.MusicCulture == null && Mission.Current.GetMissionBehavior<DeploymentHandler>() == null && (this._isKeepBattle || this._missionAgentSpawnLogic.IsDeploymentOver))
				{
					KeyValuePair<BasicCultureObject, int> keyValuePair = new KeyValuePair<BasicCultureObject, int>(null, -1);
					Dictionary<BasicCultureObject, int> dictionary = new Dictionary<BasicCultureObject, int>();
					foreach (Team team in base.Mission.Teams)
					{
						foreach (Agent agent in team.ActiveAgents)
						{
							BasicCultureObject culture = agent.Character.Culture;
							if (culture != null && culture.IsMainCulture)
							{
								if (!dictionary.ContainsKey(agent.Character.Culture))
								{
									dictionary.Add(agent.Character.Culture, 0);
								}
								Dictionary<BasicCultureObject, int> dictionary2 = dictionary;
								BasicCultureObject culture2 = agent.Character.Culture;
								int num = dictionary2[culture2];
								dictionary2[culture2] = num + 1;
								if (dictionary[agent.Character.Culture] > keyValuePair.Value)
								{
									keyValuePair = new KeyValuePair<BasicCultureObject, int>(agent.Character.Culture, dictionary[agent.Character.Culture]);
								}
							}
						}
					}
					if (keyValuePair.Key != null)
					{
						base.Mission.MusicCulture = keyValuePair.Key;
					}
					else
					{
						base.Mission.MusicCulture = Mission.Current.GetMissionBehavior<MissionCombatantsLogic>().GetCultureForPlayerSide();
					}
				}
				if (base.Mission.MusicCulture != null)
				{
					this.CheckForStarting();
				}
			}
			if (this._battleState == MusicBattleMissionView.BattleState.Started || this._battleState == MusicBattleMissionView.BattleState.TurnedOneSide)
			{
				this.CheckForEnding();
			}
			this.CheckIntensityFall();
		}

		// Token: 0x040002E7 RID: 743
		private const float ChargeOrderIntensityIncreaseCooldownInSeconds = 60f;

		// Token: 0x040002E8 RID: 744
		private MusicBattleMissionView.BattleState _battleState;

		// Token: 0x040002E9 RID: 745
		private DefaultBattleMissionAgentSpawnLogic _missionAgentSpawnLogic;

		// Token: 0x040002EA RID: 746
		private int[] _startingTroopCounts;

		// Token: 0x040002EB RID: 747
		private float _startingBattleRatio;

		// Token: 0x040002EC RID: 748
		private bool _isKeepBattle;

		// Token: 0x040002ED RID: 749
		private bool _isSiegeBattle;

		// Token: 0x040002EE RID: 750
		private bool _isPaganBattle;

		// Token: 0x040002EF RID: 751
		private MissionTime _nextPossibleTimeToIncreaseIntensityForChargeOrder;

		// Token: 0x020000E6 RID: 230
		private enum BattleState
		{
			// Token: 0x04000401 RID: 1025
			Starting,
			// Token: 0x04000402 RID: 1026
			Started,
			// Token: 0x04000403 RID: 1027
			TurnedOneSide,
			// Token: 0x04000404 RID: 1028
			Ending
		}
	}
}
