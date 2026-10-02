using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020001B9 RID: 441
	[ScriptingInterfaceBase]
	internal interface IMBVoiceManager
	{
		// Token: 0x06001922 RID: 6434
		[EngineMethod("get_voice_type_index", false, null, false)]
		int GetVoiceTypeIndex(string voiceType);

		// Token: 0x06001923 RID: 6435
		[EngineMethod("get_voice_definition_count_with_monster_sound_and_collision_info_class_name", false, null, false)]
		int GetVoiceDefinitionCountWithMonsterSoundAndCollisionInfoClassName(string className);

		// Token: 0x06001924 RID: 6436
		[EngineMethod("get_voice_definitions_with_monster_sound_and_collision_info_class_name", false, null, false)]
		void GetVoiceDefinitionListWithMonsterSoundAndCollisionInfoClassName(string className, int[] definitionIndices);
	}
}
