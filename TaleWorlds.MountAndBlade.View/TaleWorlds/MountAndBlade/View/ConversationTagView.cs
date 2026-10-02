using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade.View
{
	// Token: 0x02000012 RID: 18
	public class ConversationTagView
	{
		// Token: 0x0600007B RID: 123 RVA: 0x00003F29 File Offset: 0x00002129
		public static string GetSkillMeshName(SkillObject skillEnum, bool isOn = false)
		{
			if (isOn)
			{
				return "skill_icon_" + skillEnum.StringId.ToLower() + "_on";
			}
			return "skill_icon_" + skillEnum.StringId.ToLower() + "_off";
		}
	}
}
