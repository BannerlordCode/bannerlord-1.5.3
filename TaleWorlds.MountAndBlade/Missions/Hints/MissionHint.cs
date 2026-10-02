using System;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Missions.Hints
{
	// Token: 0x020003F8 RID: 1016
	public class MissionHint
	{
		// Token: 0x06003815 RID: 14357 RVA: 0x000E8FA4 File Offset: 0x000E71A4
		public MissionHint(TextObject description)
		{
			this.Description = description;
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x000E8FB3 File Offset: 0x000E71B3
		public static MissionHint CreateWithKeyAndAction(TextObject actionText, string hotKeyId)
		{
			TextObject textObject = GameTexts.FindText("str_key_action", null).CopyTextObject();
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(hotKeyId, 1f));
			textObject.SetTextVariable("ACTION", actionText);
			return new MissionHint(textObject);
		}

		// Token: 0x04001831 RID: 6193
		public readonly TextObject Description;
	}
}
