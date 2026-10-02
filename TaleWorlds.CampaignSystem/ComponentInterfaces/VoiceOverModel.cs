using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.CampaignSystem.ComponentInterfaces
{
	// Token: 0x02000202 RID: 514
	public abstract class VoiceOverModel : MBGameModel<VoiceOverModel>
	{
		// Token: 0x0600201A RID: 8218
		public abstract string GetSoundPathForCharacter(CharacterObject character, VoiceObject voiceObject);

		// Token: 0x0600201B RID: 8219
		public abstract string GetAccentClass(CultureObject culture, bool isHighClass);
	}
}
