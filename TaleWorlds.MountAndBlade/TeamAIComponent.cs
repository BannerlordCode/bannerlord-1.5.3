using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000189 RID: 393
	public abstract class TeamAIComponent
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06001519 RID: 5401 RVA: 0x0004D7F8 File Offset: 0x0004B9F8
		public MBReadOnlyList<StrategicArea> StrategicAreas
		{
			get
			{
				return this._strategicAreas;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600151A RID: 5402 RVA: 0x0004D800 File Offset: 0x0004BA00
		public bool HasStrategicAreas
		{
			get
			{
				return !this._strategicAreas.IsEmpty<StrategicArea>();
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x0600151B RID: 5403 RVA: 0x0004D810 File Offset: 0x0004BA10
		// (set) Token: 0x0600151C RID: 5404 RVA: 0x0004D818 File Offset: 0x0004BA18
		public bool IsDefenseApplicable { get; private set; }

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x0600151D RID: 5405 RVA: 0x0004D821 File Offset: 0x0004BA21
		// (set) Token: 0x0600151E RID: 5406 RVA: 0x0004D829 File Offset: 0x0004BA29
		public bool GetIsFirstTacticChosen { get; private set; }

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x0600151F RID: 5407 RVA: 0x0004D832 File Offset: 0x0004BA32
		// (set) Token: 0x06001520 RID: 5408 RVA: 0x0004D83A File Offset: 0x0004BA3A
		private protected TacticComponent CurrentTactic
		{
			protected get
			{
				return this._currentTactic;
			}
			private set
			{
				TacticComponent currentTactic = this._currentTactic;
				if (currentTactic != null)
				{
					currentTactic.OnCancel();
				}
				this._currentTactic = value;
				if (this._currentTactic != null)
				{
					this._currentTactic.OnApply();
					this._currentTactic.TickOccasionally();
				}
			}
		}

		// Token: 0x06001521 RID: 5409 RVA: 0x0004D874 File Offset: 0x0004BA74
		protected TeamAIComponent(Mission currentMission, Team currentTeam, float thinkTimerTime, float applyTimerTime)
		{
			this.Mission = currentMission;
			this.Team = currentTeam;
			this._thinkTimer = new Timer(this.Mission.CurrentTime, thinkTimerTime, true);
			this._applyTimer = new Timer(this.Mission.CurrentTime, applyTimerTime, true);
			this._occasionalTickTime = applyTimerTime;
			this._availableTactics = new List<TacticComponent>();
			this.TacticalPositions = currentMission.ActiveMissionObjects.FindAllWithType<TacticalPosition>().ToList<TacticalPosition>();
			this.TacticalRegions = currentMission.ActiveMissionObjects.FindAllWithType<TacticalRegion>().ToList<TacticalRegion>();
			this._strategicAreas = (from amo in currentMission.ActiveMissionObjects.Where<MissionObject>(delegate(MissionObject amo)
				{
					StrategicArea strategicArea;
					return (strategicArea = amo as StrategicArea) != null && strategicArea.IsActive && strategicArea.IsUsableBy(this.Team.Side);
				})
				select amo as StrategicArea).ToMBList<StrategicArea>();
		}

		// Token: 0x06001522 RID: 5410 RVA: 0x0004D94B File Offset: 0x0004BB4B
		public void AddStrategicArea(StrategicArea strategicArea)
		{
			this._strategicAreas.Add(strategicArea);
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x0004D959 File Offset: 0x0004BB59
		public void RemoveStrategicArea(StrategicArea strategicArea)
		{
			if (this.Team.DetachmentManager.ContainsDetachment(strategicArea))
			{
				this.Team.DetachmentManager.DestroyDetachment(strategicArea);
			}
			this._strategicAreas.Remove(strategicArea);
		}

		// Token: 0x06001524 RID: 5412 RVA: 0x0004D98C File Offset: 0x0004BB8C
		public void RemoveAllStrategicAreas()
		{
			foreach (StrategicArea strategicArea in this._strategicAreas)
			{
				if (this.Team.DetachmentManager.ContainsDetachment(strategicArea))
				{
					this.Team.DetachmentManager.DestroyDetachment(strategicArea);
				}
			}
			this._strategicAreas.Clear();
		}

		// Token: 0x06001525 RID: 5413 RVA: 0x0004DA08 File Offset: 0x0004BC08
		public void AddTacticOption(TacticComponent tacticOption)
		{
			this._availableTactics.Add(tacticOption);
		}

		// Token: 0x06001526 RID: 5414 RVA: 0x0004DA18 File Offset: 0x0004BC18
		public void RemoveTacticOption(Type tacticType)
		{
			this._availableTactics.RemoveAll((TacticComponent at) => tacticType == at.GetType());
		}

		// Token: 0x06001527 RID: 5415 RVA: 0x0004DA4A File Offset: 0x0004BC4A
		public void ClearTacticOptions()
		{
			this._availableTactics.Clear();
		}

		// Token: 0x06001528 RID: 5416 RVA: 0x0004DA57 File Offset: 0x0004BC57
		[Conditional("DEBUG")]
		public void AssertTeam(Team team)
		{
		}

		// Token: 0x06001529 RID: 5417 RVA: 0x0004DA59 File Offset: 0x0004BC59
		public void NotifyTacticalDecision(in TacticalDecision decision)
		{
			TeamAIComponent.TacticalDecisionDelegate onNotifyTacticalDecision = this.OnNotifyTacticalDecision;
			if (onNotifyTacticalDecision == null)
			{
				return;
			}
			onNotifyTacticalDecision(in decision);
		}

		// Token: 0x0600152A RID: 5418 RVA: 0x0004DA6C File Offset: 0x0004BC6C
		public virtual void OnDeploymentFinished()
		{
		}

		// Token: 0x0600152B RID: 5419 RVA: 0x0004DA6E File Offset: 0x0004BC6E
		public virtual void OnFormationFrameChanged(Agent agent, bool isFrameEnabled, WorldPosition frame)
		{
		}

		// Token: 0x0600152C RID: 5420 RVA: 0x0004DA70 File Offset: 0x0004BC70
		public virtual void OnMissionEnded()
		{
			MBDebug.Print("Mission end received by teamAI", 0, Debug.DebugColor.White, 17592186044416UL);
			foreach (Formation formation in this.Team.FormationsIncludingSpecialAndEmpty)
			{
				if (formation.CountOfUnits > 0)
				{
					foreach (UsableMachine usableMachine in formation.GetUsedMachines().ToList<UsableMachine>())
					{
						formation.StopUsingMachine(usableMachine, false);
					}
				}
			}
		}

		// Token: 0x0600152D RID: 5421 RVA: 0x0004DB28 File Offset: 0x0004BD28
		public void ResetTacticalPositions()
		{
			this.TacticalPositions = this.Mission.ActiveMissionObjects.FindAllWithType<TacticalPosition>().ToList<TacticalPosition>();
			this.TacticalRegions = this.Mission.ActiveMissionObjects.FindAllWithType<TacticalRegion>().ToList<TacticalRegion>();
		}

		// Token: 0x0600152E RID: 5422 RVA: 0x0004DB60 File Offset: 0x0004BD60
		public void ResetTactic(bool keepCurrentTactic = true)
		{
			if (!keepCurrentTactic)
			{
				this.CurrentTactic = null;
			}
			this._thinkTimer.Reset(this.Mission.CurrentTime);
			this._applyTimer.Reset(this.Mission.CurrentTime);
			this.MakeDecision();
			this.TickOccasionally();
		}

		// Token: 0x0600152F RID: 5423 RVA: 0x0004DBB0 File Offset: 0x0004BDB0
		protected internal virtual void Tick(float dt)
		{
			if (this.Team.BodyGuardFormation != null && this.Team.BodyGuardFormation.CountOfUnits > 0 && (this.Team.GeneralsFormation == null || this.Team.GeneralsFormation.CountOfUnits == 0))
			{
				this.Team.BodyGuardFormation.AI.ResetBehaviorWeights();
				this.Team.BodyGuardFormation.AI.SetBehaviorWeight<BehaviorCharge>(1f);
			}
			if (this._nextTacticChooseTime.IsPast)
			{
				this.MakeDecision();
				this._nextTacticChooseTime = MissionTime.SecondsFromNow(5f);
			}
			if (this._nextOccasionalTickTime.IsPast)
			{
				this.TickOccasionally();
				this._nextOccasionalTickTime = MissionTime.SecondsFromNow(this._occasionalTickTime);
			}
		}

		// Token: 0x06001530 RID: 5424 RVA: 0x0004DC74 File Offset: 0x0004BE74
		public void CheckIsDefenseApplicable()
		{
			if (this.Team.Side != BattleSideEnum.Defender)
			{
				this.IsDefenseApplicable = false;
				return;
			}
			int memberCount = this.Team.QuerySystem.MemberCount;
			float maxUnderRangedAttackRatio = this.Team.QuerySystem.MaxUnderRangedAttackRatio;
			float num = (float)memberCount * maxUnderRangedAttackRatio;
			int deathByRangedCount = this.Team.QuerySystem.DeathByRangedCount;
			int deathCount = this.Team.QuerySystem.DeathCount;
			float num2 = MBMath.ClampFloat((num + (float)deathByRangedCount) / (float)(memberCount + deathCount), 0.05f, 1f);
			int enemyUnitCount = this.Team.QuerySystem.EnemyUnitCount;
			float num3 = 0f;
			int num4 = 0;
			int num5 = 0;
			foreach (Team team in this.Mission.Teams)
			{
				if (this.Team.IsEnemyOf(team))
				{
					TeamQuerySystem querySystem = team.QuerySystem;
					num4 += querySystem.DeathByRangedCount;
					num5 += querySystem.DeathCount;
					num3 += ((enemyUnitCount == 0) ? 0f : (querySystem.MaxUnderRangedAttackRatio * ((float)querySystem.MemberCount / (float)((enemyUnitCount > 0) ? enemyUnitCount : 1))));
				}
			}
			float num6 = (float)enemyUnitCount * num3;
			int num7 = enemyUnitCount + num5;
			float num8 = MBMath.ClampFloat((num6 + (float)num4) / (float)((num7 > 0) ? num7 : 1), 0.05f, 1f);
			float num9 = MathF.Pow(num2 / num8, 3f * (this.Team.QuerySystem.EnemyRangedRatio + this.Team.QuerySystem.EnemyRangedCavalryRatio));
			this.IsDefenseApplicable = num9 <= 1.5f;
		}

		// Token: 0x06001531 RID: 5425 RVA: 0x0004DE2C File Offset: 0x0004C02C
		public void OnTacticAppliedForFirstTime()
		{
			this.GetIsFirstTacticChosen = false;
		}

		// Token: 0x06001532 RID: 5426 RVA: 0x0004DE38 File Offset: 0x0004C038
		private void MakeDecision()
		{
			List<TacticComponent> availableTactics = this._availableTactics;
			if ((this.Mission.CurrentState != Mission.State.Continuing && availableTactics.Count == 0) || !this.Team.HasAnyFormationsIncludingSpecialThatIsNotEmpty())
			{
				return;
			}
			bool flag = true;
			foreach (Team team in this.Mission.Teams)
			{
				if (team.IsEnemyOf(this.Team) && team.HasAnyFormationsIncludingSpecialThatIsNotEmpty())
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				if (this.Mission.MissionEnded)
				{
					return;
				}
				if (!(this.CurrentTactic is TacticCharge))
				{
					foreach (TacticComponent tacticComponent in availableTactics)
					{
						if (tacticComponent is TacticCharge)
						{
							if (this.CurrentTactic == null)
							{
								this.GetIsFirstTacticChosen = true;
							}
							this.CurrentTactic = tacticComponent;
							break;
						}
					}
					if (!(this.CurrentTactic is TacticCharge))
					{
						if (this.CurrentTactic == null)
						{
							this.GetIsFirstTacticChosen = true;
						}
						this.CurrentTactic = availableTactics.FirstOrDefault<TacticComponent>();
					}
				}
			}
			this.CheckIsDefenseApplicable();
			TacticComponent tacticComponent2 = availableTactics.MaxBy<TacticComponent, float>((TacticComponent to) => to.GetTacticWeight() * ((to == this._currentTactic) ? 1.5f : 1f));
			bool flag2 = false;
			if (this.CurrentTactic == null)
			{
				flag2 = true;
			}
			else if (this.CurrentTactic != tacticComponent2)
			{
				if (!this.CurrentTactic.ResetTacticalPositions())
				{
					flag2 = true;
				}
				else
				{
					float tacticWeight = tacticComponent2.GetTacticWeight();
					float num = this.CurrentTactic.GetTacticWeight() * 1.5f;
					if (tacticWeight > num)
					{
						flag2 = true;
					}
				}
			}
			if (flag2)
			{
				if (this.CurrentTactic == null)
				{
					this.GetIsFirstTacticChosen = true;
				}
				this.CurrentTactic = tacticComponent2;
				if (Mission.Current.MainAgent != null && this.Team.GeneralAgent != null && this.Team.IsPlayerTeam && this.Team.IsPlayerSergeant)
				{
					string name = tacticComponent2.GetType().Name;
					MBInformationManager.AddQuickInformation(GameTexts.FindText("str_team_ai_tactic_text", name), 4000, this.Team.GeneralAgent.Character, null, "");
				}
			}
		}

		// Token: 0x06001533 RID: 5427 RVA: 0x0004E060 File Offset: 0x0004C260
		public virtual void TickOccasionally()
		{
			if (Mission.Current.AllowAiTicking && this.Team.HasBots)
			{
				TacticComponent currentTactic = this.CurrentTactic;
				if (currentTactic == null)
				{
					return;
				}
				currentTactic.TickOccasionally();
			}
		}

		// Token: 0x06001534 RID: 5428 RVA: 0x0004E08B File Offset: 0x0004C28B
		public bool IsCurrentTactic(TacticComponent tactic)
		{
			return tactic == this.CurrentTactic;
		}

		// Token: 0x06001535 RID: 5429 RVA: 0x0004E098 File Offset: 0x0004C298
		[Conditional("DEBUG")]
		protected virtual void DebugTick(float dt)
		{
			if (!MBDebug.IsDisplayingHighLevelAI)
			{
				return;
			}
			TacticComponent currentTactic = this.CurrentTactic;
			if (Input.DebugInput.IsHotKeyPressed("UsableMachineAiBaseHotkeyRetreatScriptActive"))
			{
				TeamAIComponent._retreatScriptActive = true;
			}
			else if (Input.DebugInput.IsHotKeyPressed("UsableMachineAiBaseHotkeyRetreatScriptPassive"))
			{
				TeamAIComponent._retreatScriptActive = false;
			}
			bool retreatScriptActive = TeamAIComponent._retreatScriptActive;
		}

		// Token: 0x06001536 RID: 5430
		public abstract void OnUnitAddedToFormationForTheFirstTime(Formation formation);

		// Token: 0x06001537 RID: 5431 RVA: 0x0004E0EA File Offset: 0x0004C2EA
		protected internal virtual void CreateMissionSpecificBehaviors()
		{
		}

		// Token: 0x06001538 RID: 5432 RVA: 0x0004E0EC File Offset: 0x0004C2EC
		protected internal virtual void InitializeDetachments(Mission mission)
		{
			DeploymentHandler missionBehavior = this.Mission.GetMissionBehavior<DeploymentHandler>();
			if (missionBehavior == null)
			{
				return;
			}
			missionBehavior.InitializeDeploymentPoints();
		}

		// Token: 0x040005A8 RID: 1448
		public TeamAIComponent.TacticalDecisionDelegate OnNotifyTacticalDecision;

		// Token: 0x040005A9 RID: 1449
		public const int BattleTokenForceSize = 10;

		// Token: 0x040005AA RID: 1450
		private readonly List<TacticComponent> _availableTactics;

		// Token: 0x040005AB RID: 1451
		private static bool _retreatScriptActive;

		// Token: 0x040005AC RID: 1452
		protected readonly Mission Mission;

		// Token: 0x040005AD RID: 1453
		protected readonly Team Team;

		// Token: 0x040005AE RID: 1454
		private readonly Timer _thinkTimer;

		// Token: 0x040005AF RID: 1455
		private readonly Timer _applyTimer;

		// Token: 0x040005B0 RID: 1456
		private TacticComponent _currentTactic;

		// Token: 0x040005B1 RID: 1457
		public List<TacticalPosition> TacticalPositions;

		// Token: 0x040005B2 RID: 1458
		public List<TacticalRegion> TacticalRegions;

		// Token: 0x040005B3 RID: 1459
		private readonly MBList<StrategicArea> _strategicAreas;

		// Token: 0x040005B4 RID: 1460
		private readonly float _occasionalTickTime;

		// Token: 0x040005B5 RID: 1461
		private MissionTime _nextTacticChooseTime;

		// Token: 0x040005B6 RID: 1462
		private MissionTime _nextOccasionalTickTime;

		// Token: 0x020004E0 RID: 1248
		protected class TacticOption
		{
			// Token: 0x17000A52 RID: 2642
			// (get) Token: 0x06003BCB RID: 15307 RVA: 0x000F192D File Offset: 0x000EFB2D
			// (set) Token: 0x06003BCC RID: 15308 RVA: 0x000F1935 File Offset: 0x000EFB35
			public string Id { get; private set; }

			// Token: 0x17000A53 RID: 2643
			// (get) Token: 0x06003BCD RID: 15309 RVA: 0x000F193E File Offset: 0x000EFB3E
			// (set) Token: 0x06003BCE RID: 15310 RVA: 0x000F1946 File Offset: 0x000EFB46
			public Lazy<TacticComponent> Tactic { get; private set; }

			// Token: 0x17000A54 RID: 2644
			// (get) Token: 0x06003BCF RID: 15311 RVA: 0x000F194F File Offset: 0x000EFB4F
			// (set) Token: 0x06003BD0 RID: 15312 RVA: 0x000F1957 File Offset: 0x000EFB57
			public float Weight { get; set; }

			// Token: 0x06003BD1 RID: 15313 RVA: 0x000F1960 File Offset: 0x000EFB60
			public TacticOption(string id, Lazy<TacticComponent> tactic, float weight)
			{
				this.Id = id;
				this.Tactic = tactic;
				this.Weight = weight;
			}
		}

		// Token: 0x020004E1 RID: 1249
		// (Invoke) Token: 0x06003BD3 RID: 15315
		public delegate void TacticalDecisionDelegate(in TacticalDecision decision);
	}
}
