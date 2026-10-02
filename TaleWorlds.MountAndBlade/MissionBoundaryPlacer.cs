using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000293 RID: 659
	public class MissionBoundaryPlacer : MissionLogic
	{
		// Token: 0x060024D6 RID: 9430 RVA: 0x00086404 File Offset: 0x00084604
		public override void EarlyStart()
		{
			this.AddMissionBoundaries();
		}

		// Token: 0x060024D7 RID: 9431 RVA: 0x0008640C File Offset: 0x0008460C
		public void AddMissionBoundaries()
		{
			MBList<Vec2> softBoundaryPoints = MBSceneUtilities.GetSoftBoundaryPoints(base.Mission.Scene);
			if (softBoundaryPoints.Count == 0)
			{
				Vec3 vec;
				Vec3 vec2;
				base.Mission.Scene.GetBoundingBox(out vec, out vec2);
				float num = MathF.Min(2f, vec2.x - vec.x);
				float num2 = MathF.Min(2f, vec2.y - vec.y);
				List<Vec2> list = new List<Vec2>
				{
					new Vec2(vec.x + num, vec.y + num2),
					new Vec2(vec2.x - num, vec.y + num2),
					new Vec2(vec2.x - num, vec2.y - num2),
					new Vec2(vec.x + num, vec2.y - num2)
				};
				softBoundaryPoints.AddRange(list);
			}
			base.Mission.Boundaries.Add("walk_area", softBoundaryPoints);
		}
	}
}
