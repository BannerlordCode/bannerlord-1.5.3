using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002D7 RID: 727
	public abstract class SpawnFrameBehaviorBase
	{
		// Token: 0x06002A20 RID: 10784 RVA: 0x0009F101 File Offset: 0x0009D301
		public virtual void Initialize()
		{
			this.SpawnPoints = Mission.Current.Scene.FindEntitiesWithTag("spawnpoint");
		}

		// Token: 0x06002A21 RID: 10785
		public abstract MatrixFrame GetSpawnFrame(Team team, bool hasMount, bool isInitialSpawn);

		// Token: 0x06002A22 RID: 10786 RVA: 0x0009F120 File Offset: 0x0009D320
		protected MatrixFrame GetSpawnFrameFromSpawnPoints(IList<GameEntity> spawnPointsList, Team team, bool hasMount)
		{
			float num = float.MinValue;
			int num2 = -1;
			for (int i = 0; i < spawnPointsList.Count; i++)
			{
				float num3 = MBRandom.RandomFloat * 0.2f;
				float num4 = 0f;
				if (hasMount && spawnPointsList[i].HasTag("exclude_mounted"))
				{
					num3 -= 1000f;
				}
				if (!hasMount && spawnPointsList[i].HasTag("exclude_footmen"))
				{
					num3 -= 1000f;
				}
				SpawnFrameBehaviorBase.WeightCache weightCache = SpawnFrameBehaviorBase.WeightCache.CreateDecreasingCache();
				SpawnFrameBehaviorBase.WeightCache weightCache2 = SpawnFrameBehaviorBase.WeightCache.CreateDecreasingCache();
				foreach (Agent agent in Mission.Current.Agents)
				{
					if (!agent.IsMount)
					{
						float length = (agent.Position - spawnPointsList[i].GlobalPosition).Length;
						float num6;
						if (team == null || agent.Team.IsEnemyOf(team))
						{
							float num5 = 3.75f - length * 0.125f;
							num6 = MathF.Tanh(num5 * num5) * -2f + 3.1f - length * 0.0125f - 1f / ((length + 0.0001f) * 0.05f);
						}
						else
						{
							float num7 = 1.8f - length * 0.1f;
							num6 = -MathF.Tanh(num7 * num7) + 1.7f - length * 0.01f - 1f / ((length + 0.0001f) * 0.1f);
						}
						float num9;
						if (num6 >= 0f)
						{
							float num8;
							if (weightCache.CheckAndInsertNewValueIfLower(num6, out num8))
							{
								num4 -= num8;
							}
						}
						else if (weightCache2.CheckAndInsertNewValueIfLower(num6, out num9))
						{
							num4 -= num9;
						}
					}
				}
				if (num4 > 0f)
				{
					num4 /= (float)Mission.Current.Agents.Count;
				}
				num3 += num4;
				if (num3 > num)
				{
					num = num3;
					num2 = i;
				}
			}
			MatrixFrame globalFrame = spawnPointsList[num2].GetGlobalFrame();
			globalFrame.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
			return globalFrame;
		}

		// Token: 0x06002A23 RID: 10787 RVA: 0x0009F344 File Offset: 0x0009D544
		public void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
		{
		}

		// Token: 0x04001027 RID: 4135
		private const string ExcludeMountedTag = "exclude_mounted";

		// Token: 0x04001028 RID: 4136
		private const string ExcludeFootmenTag = "exclude_footmen";

		// Token: 0x04001029 RID: 4137
		protected const string SpawnPointTag = "spawnpoint";

		// Token: 0x0400102A RID: 4138
		public IEnumerable<GameEntity> SpawnPoints;

		// Token: 0x020005C0 RID: 1472
		private struct WeightCache
		{
			// Token: 0x17000AA5 RID: 2725
			private float this[int index]
			{
				get
				{
					switch (index)
					{
					case 0:
						return this._value1;
					case 1:
						return this._value2;
					case 2:
						return this._value3;
					default:
						throw new ArgumentOutOfRangeException();
					}
				}
				set
				{
					switch (index)
					{
					case 0:
						this._value1 = value;
						return;
					case 1:
						this._value2 = value;
						return;
					case 2:
						this._value3 = value;
						return;
					default:
						return;
					}
				}
			}

			// Token: 0x06003F0C RID: 16140 RVA: 0x000F929D File Offset: 0x000F749D
			private WeightCache(float value1, float value2, float value3)
			{
				this._value1 = value1;
				this._value2 = value2;
				this._value3 = value3;
			}

			// Token: 0x06003F0D RID: 16141 RVA: 0x000F92B4 File Offset: 0x000F74B4
			public static SpawnFrameBehaviorBase.WeightCache CreateDecreasingCache()
			{
				return new SpawnFrameBehaviorBase.WeightCache(float.NaN, float.NaN, float.NaN);
			}

			// Token: 0x06003F0E RID: 16142 RVA: 0x000F92CC File Offset: 0x000F74CC
			public bool CheckAndInsertNewValueIfLower(float value, out float valueDifference)
			{
				int num = 0;
				for (int i = 1; i < 3; i++)
				{
					if (this[i] > this[num])
					{
						num = i;
					}
				}
				if (float.IsNaN(this[num]) || value < this[num])
				{
					valueDifference = (float.IsNaN(this[num]) ? MathF.Abs(value) : (this[num] - value));
					this[num] = value;
					return true;
				}
				valueDifference = float.NaN;
				return false;
			}

			// Token: 0x04001F83 RID: 8067
			private const int Length = 3;

			// Token: 0x04001F84 RID: 8068
			private float _value1;

			// Token: 0x04001F85 RID: 8069
			private float _value2;

			// Token: 0x04001F86 RID: 8070
			private float _value3;
		}
	}
}
