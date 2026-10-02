using System;
using TaleWorlds.Library;

namespace TaleWorlds.Engine
{
	// Token: 0x0200002E RID: 46
	[ApplicationInterfaceBase]
	internal interface IPhysicsMaterial
	{
		// Token: 0x06000505 RID: 1285
		[EngineMethod("get_index_with_name", false, null, false)]
		PhysicsMaterial GetIndexWithName(string materialName);

		// Token: 0x06000506 RID: 1286
		[EngineMethod("get_material_count", false, null, false)]
		int GetMaterialCount();

		// Token: 0x06000507 RID: 1287
		[EngineMethod("get_material_name_at_index", false, null, false)]
		string GetMaterialNameAtIndex(int index);

		// Token: 0x06000508 RID: 1288
		[EngineMethod("get_material_flags_at_index", false, null, false)]
		PhysicsMaterialFlags GetFlagsAtIndex(int index);

		// Token: 0x06000509 RID: 1289
		[EngineMethod("get_restitution_at_index", false, null, false)]
		float GetRestitutionAtIndex(int index);

		// Token: 0x0600050A RID: 1290
		[EngineMethod("get_dynamic_friction_at_index", false, null, false)]
		float GetDynamicFrictionAtIndex(int index);

		// Token: 0x0600050B RID: 1291
		[EngineMethod("get_static_friction_at_index", false, null, false)]
		float GetStaticFrictionAtIndex(int index);

		// Token: 0x0600050C RID: 1292
		[EngineMethod("get_linear_damping_at_index", false, null, false)]
		float GetLinearDampingAtIndex(int index);

		// Token: 0x0600050D RID: 1293
		[EngineMethod("get_angular_damping_at_index", false, null, false)]
		float GetAngularDampingAtIndex(int index);
	}
}
