using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000020 RID: 32
	internal class ScriptingInterfaceOfIPhysicsMaterial : IPhysicsMaterial
	{
		// Token: 0x060003D7 RID: 983 RVA: 0x0001526E File Offset: 0x0001346E
		public float GetAngularDampingAtIndex(int index)
		{
			return ScriptingInterfaceOfIPhysicsMaterial.call_GetAngularDampingAtIndexDelegate(index);
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x0001527B File Offset: 0x0001347B
		public float GetDynamicFrictionAtIndex(int index)
		{
			return ScriptingInterfaceOfIPhysicsMaterial.call_GetDynamicFrictionAtIndexDelegate(index);
		}

		// Token: 0x060003D9 RID: 985 RVA: 0x00015288 File Offset: 0x00013488
		public PhysicsMaterialFlags GetFlagsAtIndex(int index)
		{
			return ScriptingInterfaceOfIPhysicsMaterial.call_GetFlagsAtIndexDelegate(index);
		}

		// Token: 0x060003DA RID: 986 RVA: 0x00015298 File Offset: 0x00013498
		public PhysicsMaterial GetIndexWithName(string materialName)
		{
			byte[] array = null;
			if (materialName != null)
			{
				int byteCount = ScriptingInterfaceOfIPhysicsMaterial._utf8.GetByteCount(materialName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIPhysicsMaterial._utf8.GetBytes(materialName, 0, materialName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIPhysicsMaterial.call_GetIndexWithNameDelegate(array);
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000152F2 File Offset: 0x000134F2
		public float GetLinearDampingAtIndex(int index)
		{
			return ScriptingInterfaceOfIPhysicsMaterial.call_GetLinearDampingAtIndexDelegate(index);
		}

		// Token: 0x060003DC RID: 988 RVA: 0x000152FF File Offset: 0x000134FF
		public int GetMaterialCount()
		{
			return ScriptingInterfaceOfIPhysicsMaterial.call_GetMaterialCountDelegate();
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001530B File Offset: 0x0001350B
		public string GetMaterialNameAtIndex(int index)
		{
			if (ScriptingInterfaceOfIPhysicsMaterial.call_GetMaterialNameAtIndexDelegate(index) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060003DE RID: 990 RVA: 0x00015322 File Offset: 0x00013522
		public float GetRestitutionAtIndex(int index)
		{
			return ScriptingInterfaceOfIPhysicsMaterial.call_GetRestitutionAtIndexDelegate(index);
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001532F File Offset: 0x0001352F
		public float GetStaticFrictionAtIndex(int index)
		{
			return ScriptingInterfaceOfIPhysicsMaterial.call_GetStaticFrictionAtIndexDelegate(index);
		}

		// Token: 0x04000343 RID: 835
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000344 RID: 836
		public static ScriptingInterfaceOfIPhysicsMaterial.GetAngularDampingAtIndexDelegate call_GetAngularDampingAtIndexDelegate;

		// Token: 0x04000345 RID: 837
		public static ScriptingInterfaceOfIPhysicsMaterial.GetDynamicFrictionAtIndexDelegate call_GetDynamicFrictionAtIndexDelegate;

		// Token: 0x04000346 RID: 838
		public static ScriptingInterfaceOfIPhysicsMaterial.GetFlagsAtIndexDelegate call_GetFlagsAtIndexDelegate;

		// Token: 0x04000347 RID: 839
		public static ScriptingInterfaceOfIPhysicsMaterial.GetIndexWithNameDelegate call_GetIndexWithNameDelegate;

		// Token: 0x04000348 RID: 840
		public static ScriptingInterfaceOfIPhysicsMaterial.GetLinearDampingAtIndexDelegate call_GetLinearDampingAtIndexDelegate;

		// Token: 0x04000349 RID: 841
		public static ScriptingInterfaceOfIPhysicsMaterial.GetMaterialCountDelegate call_GetMaterialCountDelegate;

		// Token: 0x0400034A RID: 842
		public static ScriptingInterfaceOfIPhysicsMaterial.GetMaterialNameAtIndexDelegate call_GetMaterialNameAtIndexDelegate;

		// Token: 0x0400034B RID: 843
		public static ScriptingInterfaceOfIPhysicsMaterial.GetRestitutionAtIndexDelegate call_GetRestitutionAtIndexDelegate;

		// Token: 0x0400034C RID: 844
		public static ScriptingInterfaceOfIPhysicsMaterial.GetStaticFrictionAtIndexDelegate call_GetStaticFrictionAtIndexDelegate;

		// Token: 0x020003B3 RID: 947
		// (Invoke) Token: 0x060014F7 RID: 5367
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAngularDampingAtIndexDelegate(int index);

		// Token: 0x020003B4 RID: 948
		// (Invoke) Token: 0x060014FB RID: 5371
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetDynamicFrictionAtIndexDelegate(int index);

		// Token: 0x020003B5 RID: 949
		// (Invoke) Token: 0x060014FF RID: 5375
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate PhysicsMaterialFlags GetFlagsAtIndexDelegate(int index);

		// Token: 0x020003B6 RID: 950
		// (Invoke) Token: 0x06001503 RID: 5379
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate PhysicsMaterial GetIndexWithNameDelegate(byte[] materialName);

		// Token: 0x020003B7 RID: 951
		// (Invoke) Token: 0x06001507 RID: 5383
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetLinearDampingAtIndexDelegate(int index);

		// Token: 0x020003B8 RID: 952
		// (Invoke) Token: 0x0600150B RID: 5387
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMaterialCountDelegate();

		// Token: 0x020003B9 RID: 953
		// (Invoke) Token: 0x0600150F RID: 5391
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetMaterialNameAtIndexDelegate(int index);

		// Token: 0x020003BA RID: 954
		// (Invoke) Token: 0x06001513 RID: 5395
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetRestitutionAtIndexDelegate(int index);

		// Token: 0x020003BB RID: 955
		// (Invoke) Token: 0x06001517 RID: 5399
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetStaticFrictionAtIndexDelegate(int index);
	}
}
