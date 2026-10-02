using System;
using SandBox.Missions;
using TaleWorlds.Engine;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.Usables
{
	// Token: 0x0200004A RID: 74
	public class CheckpointUsePoint : UsableMachine
	{
		// Token: 0x1700004A RID: 74
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000FDF3 File Offset: 0x0000DFF3
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000FDFB File Offset: 0x0000DFFB
		[EditorVisibleScriptComponentVariable(false)]
		public GameEntity SpawnPoint { get; private set; }

		// Token: 0x060002BC RID: 700 RVA: 0x0000FE04 File Offset: 0x0000E004
		protected override void OnInit()
		{
			base.OnInit();
			base.SetScriptComponentToTick(ScriptComponentBehavior.TickRequirement.Tick);
		}

		// Token: 0x060002BD RID: 701 RVA: 0x0000FE14 File Offset: 0x0000E014
		public override void AfterMissionStart()
		{
			this._checkpointMissionLogic = Mission.Current.GetMissionBehavior<CheckpointMissionLogic>();
			foreach (WeakGameEntity weakGameEntity in base.GameEntity.GetChildren())
			{
				if (weakGameEntity.HasTag("sp_checkpoint"))
				{
					this.SpawnPoint = TaleWorlds.Engine.GameEntity.CreateFromWeakEntity(weakGameEntity);
					break;
				}
			}
		}

		// Token: 0x060002BE RID: 702 RVA: 0x0000FE90 File Offset: 0x0000E090
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._checkpointMissionLogic != null)
			{
				Agent main = Agent.Main;
				if (main != null && main.IsActive())
				{
					for (int i = 0; i < base.StandingPoints.Count; i++)
					{
						if (base.StandingPoints[i].HasUser)
						{
							this._checkpointMissionLogic.OnCheckpointUsed(this.UniqueId);
						}
					}
				}
			}
		}

		// Token: 0x060002BF RID: 703 RVA: 0x0000FEF9 File Offset: 0x0000E0F9
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			return new TextObject("{=G2IaEr2Z}Use", null);
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x0000FF06 File Offset: 0x0000E106
		public override TextObject GetDescriptionText(WeakGameEntity gameEntity)
		{
			return new TextObject("{=eO7p1Q3C}Checkpoint", null);
		}

		// Token: 0x0400012E RID: 302
		public const string CheckpointSpawnPointTag = "sp_checkpoint";

		// Token: 0x0400012F RID: 303
		public int UniqueId;

		// Token: 0x04000131 RID: 305
		[EditorVisibleScriptComponentVariable(false)]
		private CheckpointMissionLogic _checkpointMissionLogic;
	}
}
