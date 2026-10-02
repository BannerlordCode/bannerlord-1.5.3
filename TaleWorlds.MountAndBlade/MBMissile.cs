using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001D4 RID: 468
	public abstract class MBMissile
	{
		// Token: 0x06001C28 RID: 7208 RVA: 0x0006158A File Offset: 0x0005F78A
		protected MBMissile(Mission mission)
		{
			this._mission = mission;
		}

		// Token: 0x170005B2 RID: 1458
		// (get) Token: 0x06001C29 RID: 7209 RVA: 0x00061599 File Offset: 0x0005F799
		// (set) Token: 0x06001C2A RID: 7210 RVA: 0x000615A1 File Offset: 0x0005F7A1
		public int Index { get; set; }

		// Token: 0x06001C2B RID: 7211 RVA: 0x000615AA File Offset: 0x0005F7AA
		public Vec3 GetPosition()
		{
			return MBAPI.IMBMission.GetPositionOfMissile(this._mission.Pointer, this.Index);
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x000615C7 File Offset: 0x0005F7C7
		public Vec3 GetOldPosition()
		{
			return MBAPI.IMBMission.GetOldPositionOfMissile(this._mission.Pointer, this.Index);
		}

		// Token: 0x06001C2D RID: 7213 RVA: 0x000615E4 File Offset: 0x0005F7E4
		public Vec3 GetVelocity()
		{
			return MBAPI.IMBMission.GetVelocityOfMissile(this._mission.Pointer, this.Index);
		}

		// Token: 0x06001C2E RID: 7214 RVA: 0x00061601 File Offset: 0x0005F801
		public void SetVelocity(in Vec3 velocity)
		{
			MBAPI.IMBMission.SetVelocityOfMissile(this._mission.Pointer, this.Index, in velocity);
		}

		// Token: 0x06001C2F RID: 7215 RVA: 0x0006161F File Offset: 0x0005F81F
		public bool GetHasRigidBody()
		{
			return MBAPI.IMBMission.GetMissileHasRigidBody(this._mission.Pointer, this.Index);
		}

		// Token: 0x0400092F RID: 2351
		private readonly Mission _mission;
	}
}
