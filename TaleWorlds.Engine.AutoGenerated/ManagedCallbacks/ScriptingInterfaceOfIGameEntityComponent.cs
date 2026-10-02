using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;

namespace ManagedCallbacks
{
	// Token: 0x02000012 RID: 18
	internal class ScriptingInterfaceOfIGameEntityComponent : IGameEntityComponent
	{
		// Token: 0x0600024E RID: 590 RVA: 0x00011F9C File Offset: 0x0001019C
		public GameEntity GetEntity(GameEntityComponent entityComponent)
		{
			UIntPtr uintPtr = ((entityComponent != null) ? entityComponent.Pointer : UIntPtr.Zero);
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntityComponent.call_GetEntityDelegate(uintPtr);
			GameEntity gameEntity = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				gameEntity = new GameEntity(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return gameEntity;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00011FFD File Offset: 0x000101FD
		public UIntPtr GetEntityPointer(UIntPtr componentPointer)
		{
			return ScriptingInterfaceOfIGameEntityComponent.call_GetEntityPointerDelegate(componentPointer);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0001200C File Offset: 0x0001020C
		public MetaMesh GetFirstMetaMesh(GameEntityComponent entityComponent)
		{
			UIntPtr uintPtr = ((entityComponent != null) ? entityComponent.Pointer : UIntPtr.Zero);
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIGameEntityComponent.call_GetFirstMetaMeshDelegate(uintPtr);
			MetaMesh metaMesh = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				metaMesh = new MetaMesh(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return metaMesh;
		}

		// Token: 0x040001CB RID: 459
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040001CC RID: 460
		public static ScriptingInterfaceOfIGameEntityComponent.GetEntityDelegate call_GetEntityDelegate;

		// Token: 0x040001CD RID: 461
		public static ScriptingInterfaceOfIGameEntityComponent.GetEntityPointerDelegate call_GetEntityPointerDelegate;

		// Token: 0x040001CE RID: 462
		public static ScriptingInterfaceOfIGameEntityComponent.GetFirstMetaMeshDelegate call_GetFirstMetaMeshDelegate;

		// Token: 0x02000249 RID: 585
		// (Invoke) Token: 0x06000F4F RID: 3919
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetEntityDelegate(UIntPtr entityComponent);

		// Token: 0x0200024A RID: 586
		// (Invoke) Token: 0x06000F53 RID: 3923
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate UIntPtr GetEntityPointerDelegate(UIntPtr componentPointer);

		// Token: 0x0200024B RID: 587
		// (Invoke) Token: 0x06000F57 RID: 3927
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer GetFirstMetaMeshDelegate(UIntPtr entityComponent);
	}
}
