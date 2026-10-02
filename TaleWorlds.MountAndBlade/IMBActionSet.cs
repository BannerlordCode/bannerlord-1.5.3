using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001AA RID: 426
	[ScriptingInterfaceBase]
	internal interface IMBActionSet
	{
		// Token: 0x060016D3 RID: 5843
		[EngineMethod("get_index_with_id", false, null, false)]
		int GetIndexWithID(string id);

		// Token: 0x060016D4 RID: 5844
		[EngineMethod("get_name_with_index", false, null, false)]
		string GetNameWithIndex(int index);

		// Token: 0x060016D5 RID: 5845
		[EngineMethod("get_skeleton_name", false, null, false)]
		string GetSkeletonName(int index);

		// Token: 0x060016D6 RID: 5846
		[EngineMethod("get_number_of_action_sets", false, null, false)]
		int GetNumberOfActionSets();

		// Token: 0x060016D7 RID: 5847
		[EngineMethod("get_number_of_monster_usage_sets", false, null, false)]
		int GetNumberOfMonsterUsageSets();

		// Token: 0x060016D8 RID: 5848
		[EngineMethod("get_animation_name", false, null, false)]
		string GetAnimationName(int index, int actionNo);

		// Token: 0x060016D9 RID: 5849
		[EngineMethod("are_actions_alternatives", false, null, false)]
		bool AreActionsAlternatives(int index, int actionNo1, int actionNo2);

		// Token: 0x060016DA RID: 5850
		[EngineMethod("get_bone_index_with_id", false, null, false)]
		sbyte GetBoneIndexWithId(string actionSetId, string boneId);

		// Token: 0x060016DB RID: 5851
		[EngineMethod("get_bone_has_parent_bone", false, null, false)]
		bool GetBoneHasParentBone(string actionSetId, sbyte boneIndex);
	}
}
