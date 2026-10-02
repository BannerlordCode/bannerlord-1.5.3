using System;
using TaleWorlds.Engine;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B5 RID: 437
	[ScriptingInterfaceBase]
	internal interface IMBSkeletonExtensions
	{
		// Token: 0x06001907 RID: 6407
		[EngineMethod("create_agent_skeleton", false, null, false)]
		Skeleton CreateAgentSkeleton(string skeletonName, bool isHumanoid, int actionSetIndex, string monsterUsageSetName, ref AnimationSystemData animationSystemData);

		// Token: 0x06001908 RID: 6408
		[EngineMethod("create_simple_skeleton", false, null, false)]
		Skeleton CreateSimpleSkeleton(string skeletonName);

		// Token: 0x06001909 RID: 6409
		[EngineMethod("create_with_action_set", false, null, false)]
		Skeleton CreateWithActionSet(ref AnimationSystemData animationSystemData);

		// Token: 0x0600190A RID: 6410
		[EngineMethod("get_skeleton_face_animation_time", false, null, false)]
		float GetSkeletonFaceAnimationTime(UIntPtr entityId);

		// Token: 0x0600190B RID: 6411
		[EngineMethod("set_skeleton_face_animation_time", false, null, false)]
		void SetSkeletonFaceAnimationTime(UIntPtr entityId, float time);

		// Token: 0x0600190C RID: 6412
		[EngineMethod("get_skeleton_face_animation_name", false, null, false)]
		string GetSkeletonFaceAnimationName(UIntPtr entityId);

		// Token: 0x0600190D RID: 6413
		[EngineMethod("get_bone_entitial_frame_at_animation_progress", false, null, false)]
		void GetBoneEntitialFrameAtAnimationProgress(UIntPtr skeletonPointer, sbyte boneIndex, int animationIndex, float progress, ref MatrixFrame outFrame);

		// Token: 0x0600190E RID: 6414
		[EngineMethod("get_bone_entitial_frame", false, null, false)]
		void GetBoneEntitialFrame(UIntPtr skeletonPointer, sbyte bone, bool useBoneMapping, bool forceToUpdate, ref MatrixFrame outFrame);

		// Token: 0x0600190F RID: 6415
		[EngineMethod("set_animation_at_channel", false, null, false)]
		void SetAnimationAtChannel(UIntPtr skeletonPointer, int animationIndex, int channelNo, float animationSpeedMultiplier, float blendInPeriod, float startProgress);

		// Token: 0x06001910 RID: 6416
		[EngineMethod("get_action_at_channel", false, null, false)]
		int GetActionAtChannel(UIntPtr skeletonPointer, int channelNo);

		// Token: 0x06001911 RID: 6417
		[EngineMethod("set_facial_animation_of_channel", false, null, false)]
		void SetFacialAnimationOfChannel(UIntPtr skeletonPointer, int channel, string facialAnimationName, bool playSound, bool loop);

		// Token: 0x06001912 RID: 6418
		[EngineMethod("set_agent_action_channel", false, null, false)]
		void SetAgentActionChannel(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex, float channelParameter, float blendPeriodOverride, bool forceFaceMorphRestart, float blendWithNextActionFactor);

		// Token: 0x06001913 RID: 6419
		[EngineMethod("does_action_continue_with_current_action_at_channel", false, null, false)]
		bool DoesActionContinueWithCurrentActionAtChannel(UIntPtr skeletonPointer, int actionChannelNo, int actionIndex);

		// Token: 0x06001914 RID: 6420
		[EngineMethod("tick_action_channels", false, null, false)]
		void TickActionChannels(UIntPtr skeletonPointer);
	}
}
