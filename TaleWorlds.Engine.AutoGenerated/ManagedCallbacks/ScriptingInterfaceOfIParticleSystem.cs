using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace ManagedCallbacks
{
	// Token: 0x0200001E RID: 30
	internal class ScriptingInterfaceOfIParticleSystem : IParticleSystem
	{
		// Token: 0x060003B6 RID: 950 RVA: 0x00014F60 File Offset: 0x00013160
		public ParticleSystem CreateParticleSystemAttachedToBone(int runtimeId, UIntPtr skeletonPtr, sbyte boneIndex, ref MatrixFrame boneLocalFrame)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIParticleSystem.call_CreateParticleSystemAttachedToBoneDelegate(runtimeId, skeletonPtr, boneIndex, ref boneLocalFrame);
			ParticleSystem particleSystem = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				particleSystem = new ParticleSystem(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return particleSystem;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00014FB0 File Offset: 0x000131B0
		public ParticleSystem CreateParticleSystemAttachedToEntity(int runtimeId, UIntPtr entityPtr, ref MatrixFrame boneLocalFrame)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIParticleSystem.call_CreateParticleSystemAttachedToEntityDelegate(runtimeId, entityPtr, ref boneLocalFrame);
			ParticleSystem particleSystem = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				particleSystem = new ParticleSystem(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return particleSystem;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00014FFC File Offset: 0x000131FC
		public void GetLocalFrame(UIntPtr pointer, ref MatrixFrame frame)
		{
			ScriptingInterfaceOfIParticleSystem.call_GetLocalFrameDelegate(pointer, ref frame);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001500C File Offset: 0x0001320C
		public int GetRuntimeIdByName(string particleSystemName)
		{
			byte[] array = null;
			if (particleSystemName != null)
			{
				int byteCount = ScriptingInterfaceOfIParticleSystem._utf8.GetByteCount(particleSystemName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIParticleSystem._utf8.GetBytes(particleSystemName, 0, particleSystemName.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIParticleSystem.call_GetRuntimeIdByNameDelegate(array);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00015066 File Offset: 0x00013266
		public bool HasAliveParticles(UIntPtr pointer)
		{
			return ScriptingInterfaceOfIParticleSystem.call_HasAliveParticlesDelegate(pointer);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00015073 File Offset: 0x00013273
		public void Restart(UIntPtr psysPointer)
		{
			ScriptingInterfaceOfIParticleSystem.call_RestartDelegate(psysPointer);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00015080 File Offset: 0x00013280
		public void SetDontRemoveFromEntity(UIntPtr pointer, bool value)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetDontRemoveFromEntityDelegate(pointer, value);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001508E File Offset: 0x0001328E
		public void SetEnable(UIntPtr psysPointer, bool enable)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetEnableDelegate(psysPointer, enable);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x0001509C File Offset: 0x0001329C
		public void SetLocalFrame(UIntPtr pointer, in MatrixFrame newFrame)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetLocalFrameDelegate(pointer, in newFrame);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000150AC File Offset: 0x000132AC
		public void SetParticleEffectByName(UIntPtr pointer, string effectName)
		{
			byte[] array = null;
			if (effectName != null)
			{
				int byteCount = ScriptingInterfaceOfIParticleSystem._utf8.GetByteCount(effectName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIParticleSystem._utf8.GetBytes(effectName, 0, effectName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIParticleSystem.call_SetParticleEffectByNameDelegate(pointer, array);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00015107 File Offset: 0x00013307
		public void SetPreviousGlobalFrame(UIntPtr pointer, in MatrixFrame newFrame)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetPreviousGlobalFrameDelegate(pointer, in newFrame);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00015115 File Offset: 0x00013315
		public void SetRuntimeEmissionRateMultiplier(UIntPtr pointer, float multiplier)
		{
			ScriptingInterfaceOfIParticleSystem.call_SetRuntimeEmissionRateMultiplierDelegate(pointer, multiplier);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00015137 File Offset: 0x00013337
		void IParticleSystem.SetLocalFrame(UIntPtr pointer, in MatrixFrame newFrame)
		{
			this.SetLocalFrame(pointer, in newFrame);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00015141 File Offset: 0x00013341
		void IParticleSystem.SetPreviousGlobalFrame(UIntPtr pointer, in MatrixFrame newFrame)
		{
			this.SetPreviousGlobalFrame(pointer, in newFrame);
		}

		// Token: 0x04000326 RID: 806
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x04000327 RID: 807
		public static ScriptingInterfaceOfIParticleSystem.CreateParticleSystemAttachedToBoneDelegate call_CreateParticleSystemAttachedToBoneDelegate;

		// Token: 0x04000328 RID: 808
		public static ScriptingInterfaceOfIParticleSystem.CreateParticleSystemAttachedToEntityDelegate call_CreateParticleSystemAttachedToEntityDelegate;

		// Token: 0x04000329 RID: 809
		public static ScriptingInterfaceOfIParticleSystem.GetLocalFrameDelegate call_GetLocalFrameDelegate;

		// Token: 0x0400032A RID: 810
		public static ScriptingInterfaceOfIParticleSystem.GetRuntimeIdByNameDelegate call_GetRuntimeIdByNameDelegate;

		// Token: 0x0400032B RID: 811
		public static ScriptingInterfaceOfIParticleSystem.HasAliveParticlesDelegate call_HasAliveParticlesDelegate;

		// Token: 0x0400032C RID: 812
		public static ScriptingInterfaceOfIParticleSystem.RestartDelegate call_RestartDelegate;

		// Token: 0x0400032D RID: 813
		public static ScriptingInterfaceOfIParticleSystem.SetDontRemoveFromEntityDelegate call_SetDontRemoveFromEntityDelegate;

		// Token: 0x0400032E RID: 814
		public static ScriptingInterfaceOfIParticleSystem.SetEnableDelegate call_SetEnableDelegate;

		// Token: 0x0400032F RID: 815
		public static ScriptingInterfaceOfIParticleSystem.SetLocalFrameDelegate call_SetLocalFrameDelegate;

		// Token: 0x04000330 RID: 816
		public static ScriptingInterfaceOfIParticleSystem.SetParticleEffectByNameDelegate call_SetParticleEffectByNameDelegate;

		// Token: 0x04000331 RID: 817
		public static ScriptingInterfaceOfIParticleSystem.SetPreviousGlobalFrameDelegate call_SetPreviousGlobalFrameDelegate;

		// Token: 0x04000332 RID: 818
		public static ScriptingInterfaceOfIParticleSystem.SetRuntimeEmissionRateMultiplierDelegate call_SetRuntimeEmissionRateMultiplierDelegate;

		// Token: 0x02000398 RID: 920
		// (Invoke) Token: 0x0600148B RID: 5259
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateParticleSystemAttachedToBoneDelegate(int runtimeId, UIntPtr skeletonPtr, sbyte boneIndex, ref MatrixFrame boneLocalFrame);

		// Token: 0x02000399 RID: 921
		// (Invoke) Token: 0x0600148F RID: 5263
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateParticleSystemAttachedToEntityDelegate(int runtimeId, UIntPtr entityPtr, ref MatrixFrame boneLocalFrame);

		// Token: 0x0200039A RID: 922
		// (Invoke) Token: 0x06001493 RID: 5267
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetLocalFrameDelegate(UIntPtr pointer, ref MatrixFrame frame);

		// Token: 0x0200039B RID: 923
		// (Invoke) Token: 0x06001497 RID: 5271
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetRuntimeIdByNameDelegate(byte[] particleSystemName);

		// Token: 0x0200039C RID: 924
		// (Invoke) Token: 0x0600149B RID: 5275
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool HasAliveParticlesDelegate(UIntPtr pointer);

		// Token: 0x0200039D RID: 925
		// (Invoke) Token: 0x0600149F RID: 5279
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void RestartDelegate(UIntPtr psysPointer);

		// Token: 0x0200039E RID: 926
		// (Invoke) Token: 0x060014A3 RID: 5283
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetDontRemoveFromEntityDelegate(UIntPtr pointer, [MarshalAs(UnmanagedType.U1)] bool value);

		// Token: 0x0200039F RID: 927
		// (Invoke) Token: 0x060014A7 RID: 5287
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetEnableDelegate(UIntPtr psysPointer, [MarshalAs(UnmanagedType.U1)] bool enable);

		// Token: 0x020003A0 RID: 928
		// (Invoke) Token: 0x060014AB RID: 5291
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetLocalFrameDelegate(UIntPtr pointer, in MatrixFrame newFrame);

		// Token: 0x020003A1 RID: 929
		// (Invoke) Token: 0x060014AF RID: 5295
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetParticleEffectByNameDelegate(UIntPtr pointer, byte[] effectName);

		// Token: 0x020003A2 RID: 930
		// (Invoke) Token: 0x060014B3 RID: 5299
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetPreviousGlobalFrameDelegate(UIntPtr pointer, in MatrixFrame newFrame);

		// Token: 0x020003A3 RID: 931
		// (Invoke) Token: 0x060014B7 RID: 5303
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetRuntimeEmissionRateMultiplierDelegate(UIntPtr pointer, float multiplier);
	}
}
