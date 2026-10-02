using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000343 RID: 835
	public class RandomParticleSpawner : ScriptComponentBehavior
	{
		// Token: 0x06002F07 RID: 12039 RVA: 0x000B6B22 File Offset: 0x000B4D22
		private void InitScript()
		{
			this._timeUntilNextParticleSpawn = this.spawnInterval;
		}

		// Token: 0x06002F08 RID: 12040 RVA: 0x000B6B30 File Offset: 0x000B4D30
		private void CheckSpawnParticle(float dt)
		{
			this._timeUntilNextParticleSpawn -= dt;
			if (this._timeUntilNextParticleSpawn <= 0f)
			{
				int childCount = base.GameEntity.ChildCount;
				if (childCount > 0)
				{
					int num = MBRandom.RandomInt(childCount);
					WeakGameEntity child = base.GameEntity.GetChild(num);
					int componentCount = child.GetComponentCount(TaleWorlds.Engine.GameEntity.ComponentType.ParticleSystemInstanced);
					for (int i = 0; i < componentCount; i++)
					{
						((ParticleSystem)child.GetComponentAtIndex(i, TaleWorlds.Engine.GameEntity.ComponentType.ParticleSystemInstanced)).Restart();
					}
				}
				this._timeUntilNextParticleSpawn += this.spawnInterval;
			}
		}

		// Token: 0x06002F09 RID: 12041 RVA: 0x000B6BC4 File Offset: 0x000B4DC4
		protected internal override void OnInit()
		{
			base.OnInit();
			this.InitScript();
			base.SetScriptComponentToTick(this.GetTickRequirement());
		}

		// Token: 0x06002F0A RID: 12042 RVA: 0x000B6BDE File Offset: 0x000B4DDE
		protected internal override void OnEditorInit()
		{
			base.OnEditorInit();
			this.OnInit();
		}

		// Token: 0x06002F0B RID: 12043 RVA: 0x000B6BEC File Offset: 0x000B4DEC
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			return ScriptComponentBehavior.TickRequirement.Tick | base.GetTickRequirement();
		}

		// Token: 0x06002F0C RID: 12044 RVA: 0x000B6BF6 File Offset: 0x000B4DF6
		protected internal override void OnTick(float dt)
		{
			this.CheckSpawnParticle(dt);
		}

		// Token: 0x06002F0D RID: 12045 RVA: 0x000B6BFF File Offset: 0x000B4DFF
		protected internal override void OnEditorTick(float dt)
		{
			base.OnEditorTick(dt);
			this.CheckSpawnParticle(dt);
		}

		// Token: 0x06002F0E RID: 12046 RVA: 0x000B6C0F File Offset: 0x000B4E0F
		protected internal override bool MovesEntity()
		{
			return true;
		}

		// Token: 0x040012CC RID: 4812
		private float _timeUntilNextParticleSpawn;

		// Token: 0x040012CD RID: 4813
		public float spawnInterval = 3f;
	}
}
