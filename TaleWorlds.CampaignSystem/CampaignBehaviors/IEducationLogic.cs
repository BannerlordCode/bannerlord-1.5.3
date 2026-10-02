using System;
using System.Collections.Generic;
using TaleWorlds.CampaignSystem.MapNotificationTypes;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.CampaignBehaviors
{
	// Token: 0x0200041E RID: 1054
	public interface IEducationLogic
	{
		// Token: 0x06004337 RID: 17207
		void Finalize(Hero child, List<string> chosenOptions);

		// Token: 0x06004338 RID: 17208
		void GetOptionProperties(Hero child, string optionKey, List<string> previousChoices, out TextObject optionTitle, out TextObject description, out TextObject effect, out ValueTuple<CharacterAttribute, int>[] attributes, out ValueTuple<SkillObject, int>[] skills, out ValueTuple<SkillObject, int>[] focusPoints, out EducationCampaignBehavior.EducationCharacterProperties[] characterProperties);

		// Token: 0x06004339 RID: 17209
		void GetPageProperties(Hero child, List<string> previousChoices, out TextObject title, out TextObject description, out TextObject instruction, out EducationCampaignBehavior.EducationCharacterProperties[] defaultProperties, out string[] availableOptions);

		// Token: 0x0600433A RID: 17210
		void GetStageProperties(Hero child, out int pageCount);

		// Token: 0x0600433B RID: 17211
		bool IsValidEducationNotification(EducationMapNotification educationMapNotification);
	}
}
