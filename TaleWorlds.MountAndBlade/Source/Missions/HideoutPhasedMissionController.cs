using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions
{
	// Token: 0x020003DE RID: 990
	public class HideoutPhasedMissionController : MissionLogic
	{
		// Token: 0x17000A1B RID: 2587
		// (get) Token: 0x0600373B RID: 14139 RVA: 0x000E56AF File Offset: 0x000E38AF
		public override MissionBehaviorType BehaviorType
		{
			get
			{
				return MissionBehaviorType.Logic;
			}
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x000E56B4 File Offset: 0x000E38B4
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (!this._isNewlyPopulatedFormationGivenOrder)
			{
				foreach (Team team in base.Mission.Teams)
				{
					if (team.Side == BattleSideEnum.Defender)
					{
						foreach (Formation formation in team.FormationsIncludingSpecialAndEmpty)
						{
							if (formation.CountOfUnits > 0)
							{
								formation.SetMovementOrder(MovementOrder.MovementOrderMove(formation.CachedMedianPosition));
								this._isNewlyPopulatedFormationGivenOrder = true;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x000E577C File Offset: 0x000E397C
		protected override void OnEndMission()
		{
			base.Mission.AreOrderGesturesEnabled_AdditionalCondition -= this.AreOrderGesturesEnabled_AdditionalCondition;
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x000E5795 File Offset: 0x000E3995
		public override void OnBehaviorInitialize()
		{
			this.ReadySpawnPointLogic();
			base.Mission.AreOrderGesturesEnabled_AdditionalCondition += this.AreOrderGesturesEnabled_AdditionalCondition;
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x000E57B4 File Offset: 0x000E39B4
		public override void AfterStart()
		{
			base.AfterStart();
			DefaultBattleMissionAgentSpawnLogic missionBehavior = base.Mission.GetMissionBehavior<DefaultBattleMissionAgentSpawnLogic>();
			if (missionBehavior != null && this.IsPhasingInitialized)
			{
				missionBehavior.AddPhaseChangeAction(BattleSideEnum.Defender, new DefaultBattleMissionAgentSpawnLogic.OnPhaseChangedDelegate(this.OnPhaseChanged));
			}
		}

		// Token: 0x17000A1C RID: 2588
		// (get) Token: 0x06003740 RID: 14144 RVA: 0x000E57F1 File Offset: 0x000E39F1
		private bool IsPhasingInitialized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x000E57F4 File Offset: 0x000E39F4
		private void ReadySpawnPointLogic()
		{
			List<WeakGameEntity> list = Mission.Current.GetActiveEntitiesWithScriptComponentOfType<HideoutSpawnPointGroup>().ToList<WeakGameEntity>();
			if (list.Count == 0)
			{
				return;
			}
			HideoutSpawnPointGroup[] array = new HideoutSpawnPointGroup[list.Count];
			foreach (WeakGameEntity weakGameEntity in list)
			{
				HideoutSpawnPointGroup firstScriptOfType = weakGameEntity.GetFirstScriptOfType<HideoutSpawnPointGroup>();
				array[firstScriptOfType.PhaseNumber - 1] = firstScriptOfType;
			}
			List<HideoutSpawnPointGroup> list2 = array.ToList<HideoutSpawnPointGroup>();
			list2.RemoveAt(0);
			for (int i = 0; i < 3; i++)
			{
				list2.RemoveAt(MBRandom.RandomInt(list2.Count));
			}
			this._spawnPointFrames = new Stack<MatrixFrame[]>();
			for (int j = 0; j < array.Length; j++)
			{
				if (!list2.Contains(array[j]))
				{
					this._spawnPointFrames.Push(array[j].GetSpawnPointFrames());
					Debug.Print("Spawn " + array[j].PhaseNumber + " is active.", 0, Debug.DebugColor.Green, 64UL);
				}
				array[j].RemoveWithAllChildren();
			}
			this.CreateSpawnPoints();
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x000E591C File Offset: 0x000E3B1C
		private void CreateSpawnPoints()
		{
			MatrixFrame[] array = this._spawnPointFrames.Pop();
			this._spawnPoints = new GameEntity[array.Length];
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].IsIdentity)
				{
					this._spawnPoints[i] = GameEntity.CreateEmpty(base.Mission.Scene, true, true, true);
					this._spawnPoints[i].SetGlobalFrame(in array[i], true);
					this._spawnPoints[i].AddTag("defender_" + ((FormationClass)i).GetName().ToLower());
				}
			}
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x000E59B4 File Offset: 0x000E3BB4
		private void OnPhaseChanged()
		{
			if (this._spawnPointFrames.Count == 0)
			{
				Debug.FailedAssert("No position left.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\HideoutPhasedMissionController.cs", "OnPhaseChanged", 142);
				return;
			}
			for (int i = 0; i < this._spawnPoints.Length; i++)
			{
				if (!(this._spawnPoints[i] == null))
				{
					this._spawnPoints[i].Remove(78);
				}
			}
			this.CreateSpawnPoints();
			this._isNewlyPopulatedFormationGivenOrder = false;
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x000E5A27 File Offset: 0x000E3C27
		private bool AreOrderGesturesEnabled_AdditionalCondition()
		{
			return false;
		}

		// Token: 0x040017DD RID: 6109
		public const int PhaseCount = 4;

		// Token: 0x040017DE RID: 6110
		private GameEntity[] _spawnPoints;

		// Token: 0x040017DF RID: 6111
		private Stack<MatrixFrame[]> _spawnPointFrames;

		// Token: 0x040017E0 RID: 6112
		private bool _isNewlyPopulatedFormationGivenOrder = true;
	}
}
