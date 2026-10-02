using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using TaleWorlds.DotNet;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace ManagedCallbacks
{
	// Token: 0x0200000C RID: 12
	internal class ScriptingInterfaceOfIMBAnimation : IMBAnimation
	{
		// Token: 0x060001E0 RID: 480 RVA: 0x0000ACEA File Offset: 0x00008EEA
		public int AnimationIndexOfActionCode(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_AnimationIndexOfActionCodeDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001E1 RID: 481 RVA: 0x0000ACF8 File Offset: 0x00008EF8
		public bool CheckAnimationClipExists(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_CheckAnimationClipExistsDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000AD06 File Offset: 0x00008F06
		public float GetActionAnimationDuration(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetActionAnimationDurationDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001E3 RID: 483 RVA: 0x0000AD14 File Offset: 0x00008F14
		public float GetActionBlendOutStartProgress(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetActionBlendOutStartProgressDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000AD24 File Offset: 0x00008F24
		public int GetActionCodeWithName(string name)
		{
			byte[] array = null;
			if (name != null)
			{
				int byteCount = ScriptingInterfaceOfIMBAnimation._utf8.GetByteCount(name);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBAnimation._utf8.GetBytes(name, 0, name.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBAnimation.call_GetActionCodeWithNameDelegate(array);
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000AD7E File Offset: 0x00008F7E
		public string GetActionNameWithCode(int index)
		{
			if (ScriptingInterfaceOfIMBAnimation.call_GetActionNameWithCodeDelegate(index) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0000AD95 File Offset: 0x00008F95
		public Agent.ActionCodeType GetActionType(int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetActionTypeDelegate(actionIndex);
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000ADA2 File Offset: 0x00008FA2
		public float GetAnimationBlendInPeriod(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationBlendInPeriodDelegate(animationIndex);
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000ADAF File Offset: 0x00008FAF
		public int GetAnimationBlendsWithActionIndex(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationBlendsWithActionIndexDelegate(animationIndex);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000ADBC File Offset: 0x00008FBC
		public int GetAnimationContinueToAction(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationContinueToActionDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000ADCA File Offset: 0x00008FCA
		public Vec3 GetAnimationDisplacementAtProgress(int animationIndex, float progress)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationDisplacementAtProgressDelegate(animationIndex, progress);
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000ADD8 File Offset: 0x00008FD8
		public float GetAnimationDuration(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationDurationDelegate(animationIndex);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000ADE5 File Offset: 0x00008FE5
		public AnimFlags GetAnimationFlags(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationFlagsDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000ADF3 File Offset: 0x00008FF3
		public string GetAnimationName(int actionSetNo, int actionIndex)
		{
			if (ScriptingInterfaceOfIMBAnimation.call_GetAnimationNameDelegate(actionSetNo, actionIndex) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000AE0B File Offset: 0x0000900B
		public float GetAnimationParameter1(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationParameter1Delegate(animationIndex);
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000AE18 File Offset: 0x00009018
		public float GetAnimationParameter2(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationParameter2Delegate(animationIndex);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000AE25 File Offset: 0x00009025
		public float GetAnimationParameter3(int animationIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetAnimationParameter3Delegate(animationIndex);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000AE32 File Offset: 0x00009032
		public Vec3 GetDisplacementVector(int actionSetNo, int actionIndex)
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetDisplacementVectorDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000AE40 File Offset: 0x00009040
		public string GetIDWithIndex(int index)
		{
			if (ScriptingInterfaceOfIMBAnimation.call_GetIDWithIndexDelegate(index) != 1)
			{
				return null;
			}
			return Managed.ReturnValueFromEngine;
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000AE58 File Offset: 0x00009058
		public int GetIndexWithID(string id)
		{
			byte[] array = null;
			if (id != null)
			{
				int byteCount = ScriptingInterfaceOfIMBAnimation._utf8.GetByteCount(id);
				array = ((byteCount < 1024) ? CallbackStringBufferManager.StringBuffer0 : new byte[byteCount + 1]);
				ScriptingInterfaceOfIMBAnimation._utf8.GetBytes(id, 0, id.Length, array, 0);
				array[byteCount] = 0;
			}
			return ScriptingInterfaceOfIMBAnimation.call_GetIndexWithIDDelegate(array);
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000AEB2 File Offset: 0x000090B2
		public int GetNumActionCodes()
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetNumActionCodesDelegate();
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000AEBE File Offset: 0x000090BE
		public int GetNumAnimations()
		{
			return ScriptingInterfaceOfIMBAnimation.call_GetNumAnimationsDelegate();
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000AECA File Offset: 0x000090CA
		public bool IsAnyAnimationLoadingFromDisk()
		{
			return ScriptingInterfaceOfIMBAnimation.call_IsAnyAnimationLoadingFromDiskDelegate();
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000AED6 File Offset: 0x000090D6
		public void PrefetchAnimationClip(int actionSetNo, int actionIndex)
		{
			ScriptingInterfaceOfIMBAnimation.call_PrefetchAnimationClipDelegate(actionSetNo, actionIndex);
		}

		// Token: 0x0400016D RID: 365
		private static readonly Encoding _utf8 = Encoding.UTF8;

		// Token: 0x0400016E RID: 366
		public static ScriptingInterfaceOfIMBAnimation.AnimationIndexOfActionCodeDelegate call_AnimationIndexOfActionCodeDelegate;

		// Token: 0x0400016F RID: 367
		public static ScriptingInterfaceOfIMBAnimation.CheckAnimationClipExistsDelegate call_CheckAnimationClipExistsDelegate;

		// Token: 0x04000170 RID: 368
		public static ScriptingInterfaceOfIMBAnimation.GetActionAnimationDurationDelegate call_GetActionAnimationDurationDelegate;

		// Token: 0x04000171 RID: 369
		public static ScriptingInterfaceOfIMBAnimation.GetActionBlendOutStartProgressDelegate call_GetActionBlendOutStartProgressDelegate;

		// Token: 0x04000172 RID: 370
		public static ScriptingInterfaceOfIMBAnimation.GetActionCodeWithNameDelegate call_GetActionCodeWithNameDelegate;

		// Token: 0x04000173 RID: 371
		public static ScriptingInterfaceOfIMBAnimation.GetActionNameWithCodeDelegate call_GetActionNameWithCodeDelegate;

		// Token: 0x04000174 RID: 372
		public static ScriptingInterfaceOfIMBAnimation.GetActionTypeDelegate call_GetActionTypeDelegate;

		// Token: 0x04000175 RID: 373
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationBlendInPeriodDelegate call_GetAnimationBlendInPeriodDelegate;

		// Token: 0x04000176 RID: 374
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationBlendsWithActionIndexDelegate call_GetAnimationBlendsWithActionIndexDelegate;

		// Token: 0x04000177 RID: 375
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationContinueToActionDelegate call_GetAnimationContinueToActionDelegate;

		// Token: 0x04000178 RID: 376
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationDisplacementAtProgressDelegate call_GetAnimationDisplacementAtProgressDelegate;

		// Token: 0x04000179 RID: 377
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationDurationDelegate call_GetAnimationDurationDelegate;

		// Token: 0x0400017A RID: 378
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationFlagsDelegate call_GetAnimationFlagsDelegate;

		// Token: 0x0400017B RID: 379
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationNameDelegate call_GetAnimationNameDelegate;

		// Token: 0x0400017C RID: 380
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationParameter1Delegate call_GetAnimationParameter1Delegate;

		// Token: 0x0400017D RID: 381
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationParameter2Delegate call_GetAnimationParameter2Delegate;

		// Token: 0x0400017E RID: 382
		public static ScriptingInterfaceOfIMBAnimation.GetAnimationParameter3Delegate call_GetAnimationParameter3Delegate;

		// Token: 0x0400017F RID: 383
		public static ScriptingInterfaceOfIMBAnimation.GetDisplacementVectorDelegate call_GetDisplacementVectorDelegate;

		// Token: 0x04000180 RID: 384
		public static ScriptingInterfaceOfIMBAnimation.GetIDWithIndexDelegate call_GetIDWithIndexDelegate;

		// Token: 0x04000181 RID: 385
		public static ScriptingInterfaceOfIMBAnimation.GetIndexWithIDDelegate call_GetIndexWithIDDelegate;

		// Token: 0x04000182 RID: 386
		public static ScriptingInterfaceOfIMBAnimation.GetNumActionCodesDelegate call_GetNumActionCodesDelegate;

		// Token: 0x04000183 RID: 387
		public static ScriptingInterfaceOfIMBAnimation.GetNumAnimationsDelegate call_GetNumAnimationsDelegate;

		// Token: 0x04000184 RID: 388
		public static ScriptingInterfaceOfIMBAnimation.IsAnyAnimationLoadingFromDiskDelegate call_IsAnyAnimationLoadingFromDiskDelegate;

		// Token: 0x04000185 RID: 389
		public static ScriptingInterfaceOfIMBAnimation.PrefetchAnimationClipDelegate call_PrefetchAnimationClipDelegate;

		// Token: 0x020001DB RID: 475
		// (Invoke) Token: 0x06000A5C RID: 2652
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int AnimationIndexOfActionCodeDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001DC RID: 476
		// (Invoke) Token: 0x06000A60 RID: 2656
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool CheckAnimationClipExistsDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001DD RID: 477
		// (Invoke) Token: 0x06000A64 RID: 2660
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetActionAnimationDurationDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001DE RID: 478
		// (Invoke) Token: 0x06000A68 RID: 2664
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetActionBlendOutStartProgressDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001DF RID: 479
		// (Invoke) Token: 0x06000A6C RID: 2668
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetActionCodeWithNameDelegate(byte[] name);

		// Token: 0x020001E0 RID: 480
		// (Invoke) Token: 0x06000A70 RID: 2672
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetActionNameWithCodeDelegate(int index);

		// Token: 0x020001E1 RID: 481
		// (Invoke) Token: 0x06000A74 RID: 2676
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Agent.ActionCodeType GetActionTypeDelegate(int actionIndex);

		// Token: 0x020001E2 RID: 482
		// (Invoke) Token: 0x06000A78 RID: 2680
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationBlendInPeriodDelegate(int animationIndex);

		// Token: 0x020001E3 RID: 483
		// (Invoke) Token: 0x06000A7C RID: 2684
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationBlendsWithActionIndexDelegate(int animationIndex);

		// Token: 0x020001E4 RID: 484
		// (Invoke) Token: 0x06000A80 RID: 2688
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationContinueToActionDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001E5 RID: 485
		// (Invoke) Token: 0x06000A84 RID: 2692
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetAnimationDisplacementAtProgressDelegate(int animationIndex, float progress);

		// Token: 0x020001E6 RID: 486
		// (Invoke) Token: 0x06000A88 RID: 2696
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationDurationDelegate(int animationIndex);

		// Token: 0x020001E7 RID: 487
		// (Invoke) Token: 0x06000A8C RID: 2700
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate AnimFlags GetAnimationFlagsDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001E8 RID: 488
		// (Invoke) Token: 0x06000A90 RID: 2704
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetAnimationNameDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001E9 RID: 489
		// (Invoke) Token: 0x06000A94 RID: 2708
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationParameter1Delegate(int animationIndex);

		// Token: 0x020001EA RID: 490
		// (Invoke) Token: 0x06000A98 RID: 2712
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationParameter2Delegate(int animationIndex);

		// Token: 0x020001EB RID: 491
		// (Invoke) Token: 0x06000A9C RID: 2716
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate float GetAnimationParameter3Delegate(int animationIndex);

		// Token: 0x020001EC RID: 492
		// (Invoke) Token: 0x06000AA0 RID: 2720
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate Vec3 GetDisplacementVectorDelegate(int actionSetNo, int actionIndex);

		// Token: 0x020001ED RID: 493
		// (Invoke) Token: 0x06000AA4 RID: 2724
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetIDWithIndexDelegate(int index);

		// Token: 0x020001EE RID: 494
		// (Invoke) Token: 0x06000AA8 RID: 2728
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetIndexWithIDDelegate(byte[] id);

		// Token: 0x020001EF RID: 495
		// (Invoke) Token: 0x06000AAC RID: 2732
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumActionCodesDelegate();

		// Token: 0x020001F0 RID: 496
		// (Invoke) Token: 0x06000AB0 RID: 2736
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate int GetNumAnimationsDelegate();

		// Token: 0x020001F1 RID: 497
		// (Invoke) Token: 0x06000AB4 RID: 2740
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool IsAnyAnimationLoadingFromDiskDelegate();

		// Token: 0x020001F2 RID: 498
		// (Invoke) Token: 0x06000AB8 RID: 2744
		[UnmanagedFunctionPointer(CallingConvention.StdCall, CharSet = CharSet.Ansi)]
		[SuppressUnmanagedCodeSecurity]
		[MonoNativeFunctionWrapper]
		public delegate void PrefetchAnimationClipDelegate(int actionSetNo, int actionIndex);
	}
}
