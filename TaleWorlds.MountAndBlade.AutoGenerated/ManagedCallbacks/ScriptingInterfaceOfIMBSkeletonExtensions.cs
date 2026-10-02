using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200001E RID: 30
	internal class ScriptingInterfaceOfIMBSkeletonExtensions : IMBSkeletonExtensions
	{
		// Token: 0x06000340 RID: 832 RVA: 0x0000D2F8 File Offset: 0x0000B4F8
		public Skeleton CreateAgentSkeleton(string skeletonName, bool isHumanoid, int actionSetIndex, string monsterUsageSetName, ref AnimationSystemData animationSystemData)
		{
			byte[] array = null;
			if (skeletonName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetByteCount(skeletonName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetBytes(skeletonName, 0, skeletonName.Length, array, 0);
				array[byteCount] = 0;
			}
			byte[] array2 = null;
			if (monsterUsageSetName != null)
			{
				int byteCount2 = ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetByteCount(monsterUsageSetName);
				array2 = ((byteCount2 < 1024) ? CallbackStringBufferManager.StringBuffer1 : new byte[byteCount2 + 1]);
				ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetBytes(monsterUsageSetName, 0, monsterUsageSetName.Length, array2, 0);
				array2[byteCount2] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMBSkeletonExtensions.call_CreateAgentSkeletonDelegate(array, isHumanoid, actionSetIndex, array2, ref animationSystemData);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000D3D8 File Offset: 0x0000B5D8
		public Skeleton CreateSimpleSkeleton(string skeletonName)
		{
			byte[] array = null;
			if (skeletonName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetByteCount(skeletonName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetBytes(skeletonName, 0, skeletonName.Length, array, 0);
				array[byteCount] = 0;
			}
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMBSkeletonExtensions.call_CreateSimpleSkeletonDelegate(array);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000D464 File Offset: 0x0000B664
		public Skeleton CreateWithActionSet(ref AnimationSystemData animationSystemData)
		{
			NativeObjectPointer nativeObjectPointer = ScriptingInterfaceOfIMBSkeletonExtensions.call_CreateWithActionSetDelegate(ref animationSystemData);
			Skeleton skeleton = null;
			if (nativeObjectPointer.Pointer != UIntPtr.Zero)
			{
				skeleton = new Skeleton(nativeObjectPointer.Pointer);
				LibraryApplicationInterface.IManaged.DecreaseReferenceCount(nativeObjectPointer.Pointer);
			}
			return skeleton;
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000D4AE File Offset: 0x0000B6AE
		public bool DoesActionContinueWithCurrentActionAtChannel(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBSkeletonExtensions.call_DoesActionContinueWithCurrentActionAtChannelDelegate(skeletonPointer, actionChannelNo, actionIndex);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000D4BD File Offset: 0x0000B6BD
		public int GetActionAtChannel(UIntPtr skeletonPointer, int channelNo)
		{
			return ScriptingInterfaceOfIMBSkeletonExtensions.call_GetActionAtChannelDelegate(skeletonPointer, channelNo);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000D4CB File Offset: 0x0000B6CB
		public void GetBoneEntitialFrame(UIntPtr skeletonPointer, sbyte bone, bool useBoneMapping, bool forceToUpdate, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_GetBoneEntitialFrameDelegate(skeletonPointer, bone, useBoneMapping, forceToUpdate, ref outFrame);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0000D4DE File Offset: 0x0000B6DE
		public void GetBoneEntitialFrameAtAnimationProgress(UIntPtr skeletonPointer, sbyte boneIndex, int animationIndex, float progress, ref MatrixFrame outFrame)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_GetBoneEntitialFrameAtAnimationProgressDelegate(skeletonPointer, boneIndex, animationIndex, progress, ref outFrame);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x0000D4F1 File Offset: 0x0000B6F1
		public string GetSkeletonFaceAnimationName(UIntPtr entityId)
		{
			if (ScriptingInterfaceOfIMBSkeletonExtensions.call_GetSkeletonFaceAnimationNameDelegate(entityId) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x06000348 RID: 840 RVA: 0x0000D508 File Offset: 0x0000B708
		public float GetSkeletonFaceAnimationTime(UIntPtr entityId)
		{
			return ScriptingInterfaceOfIMBSkeletonExtensions.call_GetSkeletonFaceAnimationTimeDelegate(entityId);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x0000D515 File Offset: 0x0000B715
		public void SetAgentActionChannel(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex, float channelParameter, float blendPeriodOverride, bool forceFaceMorphRestart, float blendWithNextActionFactor)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_SetAgentActionChannelDelegate(skeletonPointer, actionChannelNo, actionIndex, channelParameter, blendPeriodOverride, forceFaceMorphRestart, blendWithNextActionFactor);
		}

		// Token: 0x0600034A RID: 842 RVA: 0x0000D52C File Offset: 0x0000B72C
		public void SetAnimationAtChannel(UIntPtr skeletonPointer, int animationIndex, int channelNo, float animationSpeedMultiplier, float blendInPeriod, float startProgress)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_SetAnimationAtChannelDelegate(skeletonPointer, animationIndex, channelNo, animationSpeedMultiplier, blendInPeriod, startProgress);
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0000D544 File Offset: 0x0000B744
		public void SetFacialAnimationOfChannel(UIntPtr skeletonPointer, int channel, string facialAnimationName, bool playSound, bool loop)
		{
			byte[] array = null;
			if (facialAnimationName != null)
			{
				int byteCount = ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetByteCount(facialAnimationName);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBSkeletonExtensions._utf8.GetBytes(facialAnimationName, 0, facialAnimationName.Length, array, 0);
				array[byteCount] = 0;
			}
			ScriptingInterfaceOfIMBSkeletonExtensions.call_SetFacialAnimationOfChannelDelegate(skeletonPointer, channel, array, playSound, loop);
		}

		// Token: 0x0600034C RID: 844 RVA: 0x0000D5A4 File Offset: 0x0000B7A4
		public void SetSkeletonFaceAnimationTime(UIntPtr entityId, float time)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_SetSkeletonFaceAnimationTimeDelegate(entityId, time);
		}

		// Token: 0x0600034D RID: 845 RVA: 0x0000D5B2 File Offset: 0x0000B7B2
		public void TickActionChannels(UIntPtr skeletonPointer)
		{
			ScriptingInterfaceOfIMBSkeletonExtensions.call_TickActionChannelsDelegate(skeletonPointer);
		}

		// Token: 0x040002B4 RID: 692
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x040002B5 RID: 693
		public static ScriptingInterfaceOfIMBSkeletonExtensions.CreateAgentSkeletonDelegate call_CreateAgentSkeletonDelegate;

		// Token: 0x040002B6 RID: 694
		public static ScriptingInterfaceOfIMBSkeletonExtensions.CreateSimpleSkeletonDelegate call_CreateSimpleSkeletonDelegate;

		// Token: 0x040002B7 RID: 695
		public static ScriptingInterfaceOfIMBSkeletonExtensions.CreateWithActionSetDelegate call_CreateWithActionSetDelegate;

		// Token: 0x040002B8 RID: 696
		public static ScriptingInterfaceOfIMBSkeletonExtensions.DoesActionContinueWithCurrentActionAtChannelDelegate call_DoesActionContinueWithCurrentActionAtChannelDelegate;

		// Token: 0x040002B9 RID: 697
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetActionAtChannelDelegate call_GetActionAtChannelDelegate;

		// Token: 0x040002BA RID: 698
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetBoneEntitialFrameDelegate call_GetBoneEntitialFrameDelegate;

		// Token: 0x040002BB RID: 699
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetBoneEntitialFrameAtAnimationProgressDelegate call_GetBoneEntitialFrameAtAnimationProgressDelegate;

		// Token: 0x040002BC RID: 700
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetSkeletonFaceAnimationNameDelegate call_GetSkeletonFaceAnimationNameDelegate;

		// Token: 0x040002BD RID: 701
		public static ScriptingInterfaceOfIMBSkeletonExtensions.GetSkeletonFaceAnimationTimeDelegate call_GetSkeletonFaceAnimationTimeDelegate;

		// Token: 0x040002BE RID: 702
		public static ScriptingInterfaceOfIMBSkeletonExtensions.SetAgentActionChannelDelegate call_SetAgentActionChannelDelegate;

		// Token: 0x040002BF RID: 703
		public static ScriptingInterfaceOfIMBSkeletonExtensions.SetAnimationAtChannelDelegate call_SetAnimationAtChannelDelegate;

		// Token: 0x040002C0 RID: 704
		public static ScriptingInterfaceOfIMBSkeletonExtensions.SetFacialAnimationOfChannelDelegate call_SetFacialAnimationOfChannelDelegate;

		// Token: 0x040002C1 RID: 705
		public static ScriptingInterfaceOfIMBSkeletonExtensions.SetSkeletonFaceAnimationTimeDelegate call_SetSkeletonFaceAnimationTimeDelegate;

		// Token: 0x040002C2 RID: 706
		public static ScriptingInterfaceOfIMBSkeletonExtensions.TickActionChannelsDelegate call_TickActionChannelsDelegate;

		// Token: 0x02000310 RID: 784
		// (Invoke) Token: 0x06000F30 RID: 3888
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateAgentSkeletonDelegate(byte[] skeletonName, [MarshalAs(UnmanagedType.U1)] bool isHumanoid, int actionSetIndex, byte[] monsterUsageSetName, ref AnimationSystemData animationSystemData);

		// Token: 0x02000311 RID: 785
		// (Invoke) Token: 0x06000F34 RID: 3892
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateSimpleSkeletonDelegate(byte[] skeletonName);

		// Token: 0x02000312 RID: 786
		// (Invoke) Token: 0x06000F38 RID: 3896
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate NativeObjectPointer CreateWithActionSetDelegate(ref AnimationSystemData animationSystemData);

		// Token: 0x02000313 RID: 787
		// (Invoke) Token: 0x06000F3C RID: 3900
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool DoesActionContinueWithCurrentActionAtChannelDelegate(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex);

		// Token: 0x02000314 RID: 788
		// (Invoke) Token: 0x06000F40 RID: 3904
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetActionAtChannelDelegate(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x02000315 RID: 789
		// (Invoke) Token: 0x06000F44 RID: 3908
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameDelegate(UIntPtr skeletonPointer, sbyte bone, [MarshalAs(UnmanagedType.U1)] bool useBoneMapping, [MarshalAs(UnmanagedType.U1)] bool forceToUpdate, ref MatrixFrame outFrame);

		// Token: 0x02000316 RID: 790
		// (Invoke) Token: 0x06000F48 RID: 3912
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void GetBoneEntitialFrameAtAnimationProgressDelegate(UIntPtr skeletonPointer, sbyte boneIndex, int animationIndex, float progress, ref MatrixFrame outFrame);

		// Token: 0x02000317 RID: 791
		// (Invoke) Token: 0x06000F4C RID: 3916
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetSkeletonFaceAnimationNameDelegate(UIntPtr entityId);

		// Token: 0x02000318 RID: 792
		// (Invoke) Token: 0x06000F50 RID: 3920
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetSkeletonFaceAnimationTimeDelegate(UIntPtr entityId);

		// Token: 0x02000319 RID: 793
		// (Invoke) Token: 0x06000F54 RID: 3924
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAgentActionChannelDelegate(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex, float channelParameter, float blendPeriodOverride, [MarshalAs(UnmanagedType.U1)] bool forceFaceMorphRestart, float blendWithNextActionFactor);

		// Token: 0x0200031A RID: 794
		// (Invoke) Token: 0x06000F58 RID: 3928
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetAnimationAtChannelDelegate(UIntPtr skeletonPointer, int animationIndex, int channelNo, float animationSpeedMultiplier, float blendInPeriod, float startProgress);

		// Token: 0x0200031B RID: 795
		// (Invoke) Token: 0x06000F5C RID: 3932
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetFacialAnimationOfChannelDelegate(UIntPtr skeletonPointer, int channel, byte[] facialAnimationName, [MarshalAs(UnmanagedType.U1)] bool playSound, [MarshalAs(UnmanagedType.U1)] bool loop);

		// Token: 0x0200031C RID: 796
		// (Invoke) Token: 0x06000F60 RID: 3936
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void SetSkeletonFaceAnimationTimeDelegate(UIntPtr entityId, float time);

		// Token: 0x0200031D RID: 797
		// (Invoke) Token: 0x06000F64 RID: 3940
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void TickActionChannelsDelegate(UIntPtr skeletonPointer);
	}
}
