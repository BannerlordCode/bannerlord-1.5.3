using System;

namespace SandBox.GauntletUI.Tutorial
{
	// Token: 0x02000017 RID: 23
	public class TutorialAttribute : Attribute
	{
		// Token: 0x0600013E RID: 318 RVA: 0x0000A5AD File Offset: 0x000087AD
		public TutorialAttribute(string tutorialIdentifier)
		{
			this.TutorialIdentifier = tutorialIdentifier;
		}

		// Token: 0x0400006D RID: 109
		public readonly string TutorialIdentifier;
	}
}
