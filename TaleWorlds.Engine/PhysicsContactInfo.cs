using System;
using TaleWorlds.DotNet;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200007B RID: 123
	[EngineStruct("rglPhysics_contact_info", false, null)]
	public struct PhysicsContactInfo
	{
		// Token: 0x04000172 RID: 370
		public Vec3 Position;

		// Token: 0x04000173 RID: 371
		public Vec3 Normal;

		// Token: 0x04000174 RID: 372
		public float Penetration;

		// Token: 0x04000175 RID: 373
		public Vec3 Impulse;

		// Token: 0x04000176 RID: 374
		[CustomEngineStructMemberData("physics_material0_index")]
		public PhysicsMaterial PhysicsMaterial0;

		// Token: 0x04000177 RID: 375
		[CustomEngineStructMemberData("physics_material1_index")]
		public PhysicsMaterial PhysicsMaterial1;
	}
}
