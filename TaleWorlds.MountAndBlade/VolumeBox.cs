using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000389 RID: 905
	public class VolumeBox : MissionObject
	{
		// Token: 0x0600346F RID: 13423 RVA: 0x000D8249 File Offset: 0x000D6449
		protected internal override void OnInit()
		{
		}

		// Token: 0x06003470 RID: 13424 RVA: 0x000D824B File Offset: 0x000D644B
		public void AddToCheckList(Agent agent)
		{
		}

		// Token: 0x06003471 RID: 13425 RVA: 0x000D824D File Offset: 0x000D644D
		public void RemoveFromCheckList(Agent agent)
		{
		}

		// Token: 0x06003472 RID: 13426 RVA: 0x000D824F File Offset: 0x000D644F
		public void SetIsOccupiedDelegate(VolumeBox.VolumeBoxDelegate volumeBoxDelegate)
		{
			this._volumeBoxIsOccupiedDelegate = volumeBoxDelegate;
		}

		// Token: 0x06003473 RID: 13427 RVA: 0x000D8258 File Offset: 0x000D6458
		public bool HasAgentsInAttackerSide()
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			AgentProximityMap.ProximityMapSearchStruct proximityMapSearchStruct = AgentProximityMap.BeginSearch(Mission.Current, globalFrame.origin.AsVec2, globalFrame.rotation.GetScaleVector().AsVec2.Length, false);
			while (proximityMapSearchStruct.LastFoundAgent != null)
			{
				Agent lastFoundAgent = proximityMapSearchStruct.LastFoundAgent;
				if (lastFoundAgent.Team != null && lastFoundAgent.Team.Side == BattleSideEnum.Attacker && this.IsPointIn(lastFoundAgent.Position))
				{
					return true;
				}
				AgentProximityMap.FindNext(Mission.Current, ref proximityMapSearchStruct);
			}
			return false;
		}

		// Token: 0x06003474 RID: 13428 RVA: 0x000D82F4 File Offset: 0x000D64F4
		public bool IsPointIn(Vec3 point)
		{
			MatrixFrame globalFrame = base.GameEntity.GetGlobalFrame();
			Vec3 scaleVector = globalFrame.rotation.GetScaleVector();
			Vec3 vec = new Vec3(1f / scaleVector.x, 1f / scaleVector.y, 1f / scaleVector.z, -1f);
			globalFrame.rotation.ApplyScaleLocal(in vec);
			point = globalFrame.TransformToLocal(in point);
			return MathF.Abs(point.x) <= scaleVector.x / 2f && MathF.Abs(point.y) <= scaleVector.y / 2f && MathF.Abs(point.z) <= scaleVector.z / 2f;
		}

		// Token: 0x0400162A RID: 5674
		private VolumeBox.VolumeBoxDelegate _volumeBoxIsOccupiedDelegate;

		// Token: 0x02000666 RID: 1638
		// (Invoke) Token: 0x0600414B RID: 16715
		public delegate void VolumeBoxDelegate(VolumeBox volumeBox, List<Agent> agentsInVolume);
	}
}
