using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AD RID: 429
	[ScriptingInterfaceBase]
	internal interface IMBAnimation
	{
		// Token: 0x06001819 RID: 6169
		[EngineMethod("get_id_with_index", false, null, false)]
		string GetIDWithIndex(int index);

		// Token: 0x0600181A RID: 6170
		[EngineMethod("get_index_with_id", false, null, false)]
		int GetIndexWithID(string id);

		// Token: 0x0600181B RID: 6171
		[EngineMethod("get_displacement_vector", false, null, false)]
		Vec3 GetDisplacementVector(int actionSetNo, int actionIndex);

		// Token: 0x0600181C RID: 6172
		[EngineMethod("check_animation_clip_exists", false, null, false)]
		bool CheckAnimationClipExists(int actionSetNo, int actionIndex);

		// Token: 0x0600181D RID: 6173
		[EngineMethod("prefetch_animation_clip", false, null, false)]
		void PrefetchAnimationClip(int actionSetNo, int actionIndex);

		// Token: 0x0600181E RID: 6174
		[EngineMethod("get_animation_index_of_action_code", false, null, true)]
		int AnimationIndexOfActionCode(int actionSetNo, int actionIndex);

		// Token: 0x0600181F RID: 6175
		[EngineMethod("get_animation_flags", false, null, false)]
		AnimFlags GetAnimationFlags(int actionSetNo, int actionIndex);

		// Token: 0x06001820 RID: 6176
		[EngineMethod("get_action_type", false, null, true)]
		Agent.ActionCodeType GetActionType(int actionIndex);

		// Token: 0x06001821 RID: 6177
		[EngineMethod("get_animation_duration", false, null, false)]
		float GetAnimationDuration(int animationIndex);

		// Token: 0x06001822 RID: 6178
		[EngineMethod("get_animation_parameter1", false, null, false)]
		float GetAnimationParameter1(int animationIndex);

		// Token: 0x06001823 RID: 6179
		[EngineMethod("get_animation_parameter2", false, null, false)]
		float GetAnimationParameter2(int animationIndex);

		// Token: 0x06001824 RID: 6180
		[EngineMethod("get_animation_parameter3", false, null, false)]
		float GetAnimationParameter3(int animationIndex);

		// Token: 0x06001825 RID: 6181
		[EngineMethod("get_action_animation_duration", false, null, false)]
		float GetActionAnimationDuration(int actionSetNo, int actionIndex);

		// Token: 0x06001826 RID: 6182
		[EngineMethod("get_animation_name", false, null, false)]
		string GetAnimationName(int actionSetNo, int actionIndex);

		// Token: 0x06001827 RID: 6183
		[EngineMethod("get_animation_continue_to_action", false, null, false)]
		int GetAnimationContinueToAction(int actionSetNo, int actionIndex);

		// Token: 0x06001828 RID: 6184
		[EngineMethod("get_animation_blend_in_period", false, null, false)]
		float GetAnimationBlendInPeriod(int animationIndex);

		// Token: 0x06001829 RID: 6185
		[EngineMethod("get_action_blend_out_start_progress", false, null, false)]
		float GetActionBlendOutStartProgress(int actionSetNo, int actionIndex);

		// Token: 0x0600182A RID: 6186
		[EngineMethod("get_animation_blends_with_action_index", false, null, false)]
		int GetAnimationBlendsWithActionIndex(int animationIndex);

		// Token: 0x0600182B RID: 6187
		[EngineMethod("get_animation_displacement_at_progress", false, null, true)]
		Vec3 GetAnimationDisplacementAtProgress(int animationIndex, float progress);

		// Token: 0x0600182C RID: 6188
		[EngineMethod("get_action_code_with_name", false, null, false)]
		int GetActionCodeWithName(string name);

		// Token: 0x0600182D RID: 6189
		[EngineMethod("get_action_name_with_code", false, null, false)]
		string GetActionNameWithCode(int index);

		// Token: 0x0600182E RID: 6190
		[EngineMethod("get_num_action_codes", false, null, false)]
		int GetNumActionCodes();

		// Token: 0x0600182F RID: 6191
		[EngineMethod("get_num_animations", false, null, false)]
		int GetNumAnimations();

		// Token: 0x06001830 RID: 6192
		[EngineMethod("is_any_animation_loading_from_disk", false, null, false)]
		bool IsAnyAnimationLoadingFromDisk();
	}
}
