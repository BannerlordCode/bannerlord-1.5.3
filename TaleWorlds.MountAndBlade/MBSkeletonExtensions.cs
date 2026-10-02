using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001E1 RID: 481
	public static class MBSkeletonExtensions
	{
		// Token: 0x06001C7E RID: 7294 RVA: 0x00061EA6 File Offset: 0x000600A6
		public static Skeleton CreateWithActionSet(ref AnimationSystemData animationSystemData)
		{
			return MBAPI.IMBSkeletonExtensions.CreateWithActionSet(ref animationSystemData);
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x00061EB3 File Offset: 0x000600B3
		public static float GetSkeletonFaceAnimationTime(Skeleton skeleton)
		{
			return MBAPI.IMBSkeletonExtensions.GetSkeletonFaceAnimationTime(skeleton.Pointer);
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x00061EC5 File Offset: 0x000600C5
		public static void SetSkeletonFaceAnimationTime(Skeleton skeleton, float time)
		{
			MBAPI.IMBSkeletonExtensions.SetSkeletonFaceAnimationTime(skeleton.Pointer, time);
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x00061ED8 File Offset: 0x000600D8
		public static string GetSkeletonFaceAnimationName(Skeleton skeleton)
		{
			return MBAPI.IMBSkeletonExtensions.GetSkeletonFaceAnimationName(skeleton.Pointer);
		}

		// Token: 0x06001C82 RID: 7298 RVA: 0x00061EEC File Offset: 0x000600EC
		public static MatrixFrame GetBoneEntitialFrameAtAnimationProgress(this Skeleton skeleton, sbyte boneIndex, int animationIndex, float progress)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBSkeletonExtensions.GetBoneEntitialFrameAtAnimationProgress(skeleton.Pointer, boneIndex, animationIndex, progress, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06001C83 RID: 7299 RVA: 0x00061F18 File Offset: 0x00060118
		public static MatrixFrame GetBoneEntitialFrame(this Skeleton skeleton, sbyte boneNumber, bool forceToUpdate = false)
		{
			MatrixFrame matrixFrame = default(MatrixFrame);
			MBAPI.IMBSkeletonExtensions.GetBoneEntitialFrame(skeleton.Pointer, boneNumber, false, forceToUpdate, ref matrixFrame);
			return matrixFrame;
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x00061F43 File Offset: 0x00060143
		public static void SetFacialAnimation(this Skeleton skeleton, Agent.FacialAnimChannel channel, string faceAnimation, bool playSound, bool loop)
		{
			MBAPI.IMBSkeletonExtensions.SetFacialAnimationOfChannel(skeleton.Pointer, (int)channel, faceAnimation, playSound, loop);
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x00061F5A File Offset: 0x0006015A
		public static void SetAgentActionChannel(this Skeleton skeleton, int actionChannelNo, in ActionIndexCache actionIndex, float channelParameter = 0f, float blendPeriodOverride = -0.2f, bool forceFaceMorphRestart = true, float blendWithNextActionFactor = 0f)
		{
			MBAPI.IMBSkeletonExtensions.SetAgentActionChannel(skeleton.Pointer, actionChannelNo, actionIndex.Index, channelParameter, blendPeriodOverride, forceFaceMorphRestart, blendWithNextActionFactor);
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x00061F7A File Offset: 0x0006017A
		public static bool DoesActionContinueWithCurrentActionAtChannel(this Skeleton skeleton, int actionChannelNo, in ActionIndexCache actionIndex)
		{
			return MBAPI.IMBSkeletonExtensions.DoesActionContinueWithCurrentActionAtChannel(skeleton.Pointer, actionChannelNo, actionIndex.Index);
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x00061F93 File Offset: 0x00060193
		public static void TickActionChannels(this Skeleton skeleton)
		{
			MBAPI.IMBSkeletonExtensions.TickActionChannels(skeleton.Pointer);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x00061FA8 File Offset: 0x000601A8
		public static void SetAnimationAtChannel(this Skeleton skeleton, string animationName, int channelNo, float animationSpeedMultiplier = 1f, float blendInPeriod = -1f, float startProgress = 0f)
		{
			int indexWithID = MBAPI.IMBAnimation.GetIndexWithID(animationName);
			skeleton.SetAnimationAtChannel(indexWithID, channelNo, animationSpeedMultiplier, blendInPeriod, startProgress);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x00061FCE File Offset: 0x000601CE
		public static void SetAnimationAtChannel(this Skeleton skeleton, int animationIndex, int channelNo, float animationSpeedMultiplier = 1f, float blendInPeriod = -1f, float startProgress = 0f)
		{
			MBAPI.IMBSkeletonExtensions.SetAnimationAtChannel(skeleton.Pointer, animationIndex, channelNo, animationSpeedMultiplier, blendInPeriod, startProgress);
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x00061FE7 File Offset: 0x000601E7
		public static ActionIndexCache GetActionAtChannel(this Skeleton skeleton, int channelNo)
		{
			return new ActionIndexCache(MBAPI.IMBSkeletonExtensions.GetActionAtChannel(skeleton.Pointer, channelNo));
		}
	}
}
