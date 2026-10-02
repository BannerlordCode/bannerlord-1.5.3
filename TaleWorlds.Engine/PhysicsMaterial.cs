using System;
using TaleWorlds.DotNet;

namespace TaleWorlds.Engine
{
	// Token: 0x02000079 RID: 121
	[EngineStruct("int", false, null)]
	public readonly struct PhysicsMaterial
	{
		// Token: 0x06000AAF RID: 2735 RVA: 0x0000AED3 File Offset: 0x000090D3
		internal PhysicsMaterial(int index)
		{
			this = default(PhysicsMaterial);
			this.Index = index;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x0000AEE3 File Offset: 0x000090E3
		public bool IsValid
		{
			get
			{
				return this.Index >= 0;
			}
		}

		// Token: 0x06000AB1 RID: 2737 RVA: 0x0000AEF1 File Offset: 0x000090F1
		public PhysicsMaterialFlags GetFlags()
		{
			return PhysicsMaterial.GetFlagsAtIndex(this.Index);
		}

		// Token: 0x06000AB2 RID: 2738 RVA: 0x0000AEFE File Offset: 0x000090FE
		public float GetDynamicFriction()
		{
			return PhysicsMaterial.GetDynamicFrictionAtIndex(this.Index);
		}

		// Token: 0x06000AB3 RID: 2739 RVA: 0x0000AF0B File Offset: 0x0000910B
		public float GetStaticFriction()
		{
			return PhysicsMaterial.GetStaticFrictionAtIndex(this.Index);
		}

		// Token: 0x06000AB4 RID: 2740 RVA: 0x0000AF18 File Offset: 0x00009118
		public float GetRestitution()
		{
			return PhysicsMaterial.GetRestitutionAtIndex(this.Index);
		}

		// Token: 0x06000AB5 RID: 2741 RVA: 0x0000AF25 File Offset: 0x00009125
		public float GetLinearDamping()
		{
			return PhysicsMaterial.GetLinearDampingAtIndex(this.Index);
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0000AF32 File Offset: 0x00009132
		public float GetAngularDamping()
		{
			return PhysicsMaterial.GetAngularDampingAtIndex(this.Index);
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x0000AF3F File Offset: 0x0000913F
		public string Name
		{
			get
			{
				return PhysicsMaterial.GetNameAtIndex(this.Index);
			}
		}

		// Token: 0x06000AB8 RID: 2744 RVA: 0x0000AF4C File Offset: 0x0000914C
		public bool Equals(PhysicsMaterial m)
		{
			return this.Index == m.Index;
		}

		// Token: 0x06000AB9 RID: 2745 RVA: 0x0000AF5C File Offset: 0x0000915C
		public static int GetMaterialCount()
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetMaterialCount();
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x0000AF68 File Offset: 0x00009168
		public static PhysicsMaterial GetFromName(string id)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetIndexWithName(id);
		}

		// Token: 0x06000ABB RID: 2747 RVA: 0x0000AF75 File Offset: 0x00009175
		public static string GetNameAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetMaterialNameAtIndex(index);
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x0000AF82 File Offset: 0x00009182
		public static PhysicsMaterialFlags GetFlagsAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetFlagsAtIndex(index);
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x0000AF8F File Offset: 0x0000918F
		public static float GetRestitutionAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetRestitutionAtIndex(index);
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x0000AF9C File Offset: 0x0000919C
		public static float GetDynamicFrictionAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetDynamicFrictionAtIndex(index);
		}

		// Token: 0x06000ABF RID: 2751 RVA: 0x0000AFA9 File Offset: 0x000091A9
		public static float GetStaticFrictionAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetStaticFrictionAtIndex(index);
		}

		// Token: 0x06000AC0 RID: 2752 RVA: 0x0000AFB6 File Offset: 0x000091B6
		public static float GetLinearDampingAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetLinearDampingAtIndex(index);
		}

		// Token: 0x06000AC1 RID: 2753 RVA: 0x0000AFC3 File Offset: 0x000091C3
		public static float GetAngularDampingAtIndex(int index)
		{
			return EngineApplicationInterface.IPhysicsMaterial.GetAngularDampingAtIndex(index);
		}

		// Token: 0x06000AC2 RID: 2754 RVA: 0x0000AFD0 File Offset: 0x000091D0
		public static PhysicsMaterial GetFromIndex(int index)
		{
			return new PhysicsMaterial(index);
		}

		// Token: 0x0400016C RID: 364
		[CustomEngineStructMemberData("ignoredMember", true)]
		public readonly int Index;

		// Token: 0x0400016D RID: 365
		public static readonly PhysicsMaterial InvalidPhysicsMaterial = new PhysicsMaterial(-1);
	}
}
