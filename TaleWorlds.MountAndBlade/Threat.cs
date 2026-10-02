using System;
using System.Diagnostics;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000192 RID: 402
	public class Threat
	{
		// Token: 0x0600156E RID: 5486 RVA: 0x0004FDD0 File Offset: 0x0004DFD0
		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x0600156F RID: 5487 RVA: 0x0004FDD8 File Offset: 0x0004DFD8
		public string Name
		{
			get
			{
				if (this.TargetableObject != null)
				{
					return this.TargetableObject.Entity().Name;
				}
				if (this.Agent != null)
				{
					return this.Agent.Name.ToString();
				}
				if (this.Formation != null)
				{
					return this.Formation.ToString();
				}
				Debug.FailedAssert("Invalid threat", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Threat.cs", "Name", 39);
				return "Invalid";
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06001570 RID: 5488 RVA: 0x0004FE4C File Offset: 0x0004E04C
		public Vec3 TargetingPosition
		{
			get
			{
				if (this.TargetableObject != null)
				{
					ValueTuple<Vec3, Vec3> valueTuple = this.TargetableObject.ComputeGlobalPhysicsBoundingBoxMinMax();
					Vec3 item = valueTuple.Item1;
					return (valueTuple.Item2 + item) * 0.5f + this.TargetableObject.GetTargetingOffset();
				}
				if (this.Agent != null)
				{
					return this.Agent.CollisionCapsuleCenter;
				}
				if (this.Formation != null)
				{
					return this.Formation.GetMedianAgent(false, false, this.Formation.GetAveragePositionOfUnits(false, false)).Position;
				}
				Debug.FailedAssert("Invalid threat", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Threat.cs", "TargetingPosition", 64);
				return Vec3.Invalid;
			}
		}

		// Token: 0x06001571 RID: 5489 RVA: 0x0004FEF0 File Offset: 0x0004E0F0
		public ValueTuple<Vec3, Vec3> ComputeGlobalTargetingBoundingBoxMinMax()
		{
			if (this.TargetableObject != null)
			{
				ValueTuple<Vec3, Vec3> valueTuple = this.TargetableObject.ComputeGlobalPhysicsBoundingBoxMinMax();
				Vec3 item = valueTuple.Item1;
				Vec3 item2 = valueTuple.Item2;
				return new ValueTuple<Vec3, Vec3>(item + this.TargetableObject.GetTargetingOffset(), item2 + this.TargetableObject.GetTargetingOffset());
			}
			if (this.Agent != null)
			{
				return this.Agent.CollisionCapsule.GetBoxMinMax();
			}
			if (this.Formation != null)
			{
				Debug.FailedAssert("Nobody should be requesting a bounding box for a formation", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\AI\\Threat.cs", "ComputeGlobalTargetingBoundingBoxMinMax", 83);
				return new ValueTuple<Vec3, Vec3>(Vec3.Invalid, Vec3.Invalid);
			}
			return new ValueTuple<Vec3, Vec3>(Vec3.Invalid, Vec3.Invalid);
		}

		// Token: 0x06001572 RID: 5490 RVA: 0x0004FFA0 File Offset: 0x0004E1A0
		public Vec3 GetGlobalVelocity()
		{
			if (this.TargetableObject != null)
			{
				return this.TargetableObject.GetTargetGlobalVelocity();
			}
			if (this.Agent != null)
			{
				return new Vec3(this.Agent.GetAverageRealGlobalVelocity().AsVec2, 0f, -1f);
			}
			return Vec3.Zero;
		}

		// Token: 0x06001573 RID: 5491 RVA: 0x0004FFF4 File Offset: 0x0004E1F4
		public override bool Equals(object obj)
		{
			Threat threat;
			return (threat = obj as Threat) != null && this.TargetableObject == threat.TargetableObject && this.Formation == threat.Formation;
		}

		// Token: 0x06001574 RID: 5492 RVA: 0x0005002B File Offset: 0x0004E22B
		[Conditional("DEBUG")]
		public void DisplayDebugInfo()
		{
		}

		// Token: 0x040005D7 RID: 1495
		public ITargetable TargetableObject;

		// Token: 0x040005D8 RID: 1496
		public Formation Formation;

		// Token: 0x040005D9 RID: 1497
		public Agent Agent;

		// Token: 0x040005DA RID: 1498
		public float ThreatValue;

		// Token: 0x040005DB RID: 1499
		public bool ForceTarget;
	}
}
