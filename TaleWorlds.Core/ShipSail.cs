using System;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x020000CE RID: 206
	public class ShipSail
	{
		// Token: 0x06000B1A RID: 2842 RVA: 0x0002400A File Offset: 0x0002220A
		public ShipSail(SailType type, float forceMultiplier, float leftRotationLimit, float rightRotationLimit, float rotationRate)
		{
			this.Type = type;
			this.ForceMultiplier = forceMultiplier;
			this.LeftRotationLimit = leftRotationLimit;
			this.RightRotationLimit = rightRotationLimit;
			this.RotationRate = rotationRate;
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00024038 File Offset: 0x00022238
		public bool NearlyEquals(ShipSail otherShipSail)
		{
			return this.Type == otherShipSail.Type && this.ForceMultiplier.ApproximatelyEqualsTo(otherShipSail.ForceMultiplier, 1E-05f) && this.LeftRotationLimit.ApproximatelyEqualsTo(otherShipSail.LeftRotationLimit, 1E-05f) && this.RightRotationLimit.ApproximatelyEqualsTo(otherShipSail.RightRotationLimit, 1E-05f) && this.RotationRate.ApproximatelyEqualsTo(otherShipSail.RotationRate, 1E-05f);
		}

		// Token: 0x04000624 RID: 1572
		public readonly SailType Type;

		// Token: 0x04000625 RID: 1573
		public readonly float ForceMultiplier;

		// Token: 0x04000626 RID: 1574
		public readonly float LeftRotationLimit;

		// Token: 0x04000627 RID: 1575
		public readonly float RightRotationLimit;

		// Token: 0x04000628 RID: 1576
		public readonly float RotationRate;
	}
}
