using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Missions;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000108 RID: 264
	public class AgentProximityMap
	{
		// Token: 0x06000D68 RID: 3432 RVA: 0x00018040 File Offset: 0x00016240
		public static bool CanSearchRadius(float searchRadius)
		{
			float num = Mission.Current.ProximityMapMaxSearchRadius();
			return searchRadius <= num;
		}

		// Token: 0x06000D69 RID: 3433 RVA: 0x00018060 File Offset: 0x00016260
		public static AgentProximityMap.ProximityMapSearchStruct BeginSearch(Mission mission, Vec2 searchPos, float searchRadius, bool extendRangeByBiggestAgentCollisionPadding = false)
		{
			if (extendRangeByBiggestAgentCollisionPadding)
			{
				searchRadius += mission.GetBiggestAgentCollisionPadding() + 1f;
			}
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = default(AgentProximityMap.ProximityMapSearchStruct);
			float num = mission.ProximityMapMaxSearchRadius();
			proximityMapSearchStruct.LoopAllAgents = searchRadius > num;
			if (proximityMapSearchStruct.LoopAllAgents)
			{
				proximityMapSearchStruct.SearchStructInternal.SearchPos = searchPos;
				proximityMapSearchStruct.SearchStructInternal.SearchDistSq = searchRadius * searchRadius;
				proximityMapSearchStruct.LastAgentLoopIndex = 0;
				proximityMapSearchStruct.LastFoundAgent = null;
				AgentReadOnlyList agents = mission.Agents;
				while (agents.Count > proximityMapSearchStruct.LastAgentLoopIndex)
				{
					Agent agent = agents[proximityMapSearchStruct.LastAgentLoopIndex];
					if (agent.Position.AsVec2.DistanceSquared(searchPos) <= proximityMapSearchStruct.SearchStructInternal.SearchDistSq)
					{
						proximityMapSearchStruct.LastFoundAgent = agent;
						break;
					}
					proximityMapSearchStruct.LastAgentLoopIndex++;
				}
			}
			else
			{
				proximityMapSearchStruct.SearchStructInternal = mission.ProximityMapBeginSearch(searchPos, searchRadius);
				proximityMapSearchStruct.RefreshLastFoundAgent(mission);
			}
			return proximityMapSearchStruct;
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0001814C File Offset: 0x0001634C
		public static void FindNext(Mission mission, ref AgentProximityMap.ProximityMapSearchStruct searchStruct)
		{
			if (searchStruct.LoopAllAgents)
			{
				searchStruct.LastAgentLoopIndex++;
				searchStruct.LastFoundAgent = null;
				AgentReadOnlyList agents = mission.Agents;
				while (agents.Count > searchStruct.LastAgentLoopIndex)
				{
					Agent agent = agents[searchStruct.LastAgentLoopIndex];
					if (agent.Position.AsVec2.DistanceSquared(searchStruct.SearchStructInternal.SearchPos) <= searchStruct.SearchStructInternal.SearchDistSq)
					{
						searchStruct.LastFoundAgent = agent;
						return;
					}
					searchStruct.LastAgentLoopIndex++;
				}
				return;
			}
			mission.ProximityMapFindNext(ref searchStruct.SearchStructInternal);
			searchStruct.RefreshLastFoundAgent(mission);
		}

		// Token: 0x02000431 RID: 1073
		public struct ProximityMapSearchStruct
		{
			// Token: 0x17000A3E RID: 2622
			// (get) Token: 0x060038E4 RID: 14564 RVA: 0x000EA293 File Offset: 0x000E8493
			// (set) Token: 0x060038E5 RID: 14565 RVA: 0x000EA29B File Offset: 0x000E849B
			public Agent LastFoundAgent { get; internal set; }

			// Token: 0x060038E6 RID: 14566 RVA: 0x000EA2A4 File Offset: 0x000E84A4
			internal void RefreshLastFoundAgent(Mission mission)
			{
				this.LastFoundAgent = this.SearchStructInternal.GetCurrentAgent(mission);
			}

			// Token: 0x04001989 RID: 6537
			internal AgentProximityMap.ProximityMapSearchStructInternal SearchStructInternal;

			// Token: 0x0400198A RID: 6538
			internal bool LoopAllAgents;

			// Token: 0x0400198B RID: 6539
			internal int LastAgentLoopIndex;
		}

		// Token: 0x02000432 RID: 1074
		[EngineStruct("Managed_proximity_map_search_struct", false, null)]
		[Serializable]
		internal struct ProximityMapSearchStructInternal
		{
			// Token: 0x060038E7 RID: 14567 RVA: 0x000EA2B8 File Offset: 0x000E84B8
			internal Agent GetCurrentAgent(Mission mission)
			{
				return mission.FindAgentWithIndex(this.CurrentElementIndex);
			}

			// Token: 0x0400198D RID: 6541
			internal int CurrentElementIndex;

			// Token: 0x0400198E RID: 6542
			internal Vec2i Loc;

			// Token: 0x0400198F RID: 6543
			internal Vec2i GridMin;

			// Token: 0x04001990 RID: 6544
			internal Vec2i GridMax;

			// Token: 0x04001991 RID: 6545
			internal Vec2 SearchPos;

			// Token: 0x04001992 RID: 6546
			internal float SearchDistSq;
		}
	}
}
