using System;
using SandBox.Missions;
using TaleWorlds.Engine;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects
{
	// Token: 0x02000034 RID: 52
	public class CheckpointArea : VolumeBox
	{
		// Token: 0x17000025 RID: 37
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000CA35 File Offset: 0x0000AC35
		// (set) Token: 0x060001EB RID: 491 RVA: 0x0000CA3D File Offset: 0x0000AC3D
		[EditorVisibleScriptComponentVariable(false)]
		public GameEntity SpawnPoint { get; private set; }

		// Token: 0x060001EC RID: 492 RVA: 0x0000CA48 File Offset: 0x0000AC48
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

		// Token: 0x060001ED RID: 493 RVA: 0x0000CAC4 File Offset: 0x0000ACC4
		protected override void OnTick(float dt)
		{
			if (this._checkpointMissionLogic != null)
			{
				Agent main = Agent.Main;
				if (main != null && main.IsActive() && base.IsPointIn(Agent.Main.Position))
				{
					this._checkpointMissionLogic.OnCheckpointUsed(this.UniqueId);
				}
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000CB04 File Offset: 0x0000AD04
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick;
		}

		// Token: 0x040000B2 RID: 178
		public const string CheckpointSpawnPointTag = "sp_checkpoint";

		// Token: 0x040000B3 RID: 179
		public int UniqueId;

		// Token: 0x040000B5 RID: 181
		[EditorVisibleScriptComponentVariable(false)]
		private CheckpointMissionLogic _checkpointMissionLogic;
	}
}
