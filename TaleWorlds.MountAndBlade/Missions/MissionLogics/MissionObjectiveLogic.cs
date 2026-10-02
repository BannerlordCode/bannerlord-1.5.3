using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions.Objectives;

namespace TaleWorlds.MountAndBlade.Missions.MissionLogics
{
	// Token: 0x020003F5 RID: 1013
	public class MissionObjectiveLogic : MissionLogic
	{
		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x060037EC RID: 14316 RVA: 0x000E7DFB File Offset: 0x000E5FFB
		public MissionObjective CurrentObjective
		{
			get
			{
				return this._currentObjective;
			}
		}

		// Token: 0x060037EE RID: 14318 RVA: 0x000E7E0C File Offset: 0x000E600C
		public void StartObjective(MissionObjective objective)
		{
			if (objective == null || objective.IsStarted || objective.IsCompleted)
			{
				Debug.FailedAssert("Trying to start an invalid mission objective.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Missions\\MissionLogics\\MissionObjectiveLogic.cs", "StartObjective", 20);
				return;
			}
			this.CompleteCurrentObjective();
			this._currentObjective = objective;
			if (this._currentObjective != null && this._currentObjective.GetIsActivationRequirementsMet())
			{
				this.StartObjectiveAux();
			}
		}

		// Token: 0x060037EF RID: 14319 RVA: 0x000E7E6B File Offset: 0x000E606B
		private void StartObjectiveAux()
		{
			Debug.Print("Mission: Start objective: " + this._currentObjective.UniqueId, 0, Debug.DebugColor.White, 17592186044416UL);
			this._currentObjective.Start();
		}

		// Token: 0x060037F0 RID: 14320 RVA: 0x000E7EA0 File Offset: 0x000E60A0
		public void CompleteCurrentObjective()
		{
			if (this._currentObjective == null)
			{
				return;
			}
			Debug.Print("Mission: Complete objective: " + this._currentObjective.UniqueId, 0, Debug.DebugColor.White, 17592186044416UL);
			this._currentObjective.Complete();
			this._currentObjective = null;
		}

		// Token: 0x060037F1 RID: 14321 RVA: 0x000E7EF0 File Offset: 0x000E60F0
		public override void OnMissionTick(float dt)
		{
			base.OnMissionTick(dt);
			if (this._currentObjective != null && !this._currentObjective.IsStarted && this._currentObjective.GetIsActivationRequirementsMet())
			{
				this.StartObjectiveAux();
			}
			MissionObjective currentObjective = this._currentObjective;
			if (currentObjective != null)
			{
				currentObjective.Tick(dt);
			}
			if (this._currentObjective != null && this._currentObjective.IsStarted && this._currentObjective.GetIsCompletionRequirementsMet())
			{
				this.CompleteCurrentObjective();
			}
		}

		// Token: 0x0400182B RID: 6187
		private MissionObjective _currentObjective;
	}
}
