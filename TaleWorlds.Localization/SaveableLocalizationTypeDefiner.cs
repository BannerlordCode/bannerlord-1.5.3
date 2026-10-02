using System;
using System.Collections.Generic;
using TaleWorlds.SaveSystem;

namespace TaleWorlds.Localization
{
	// Token: 0x0200000A RID: 10
	public class SaveableLocalizationTypeDefiner : SaveableTypeDefiner
	{
		// Token: 0x0600008E RID: 142 RVA: 0x0000448B File Offset: 0x0000268B
		public SaveableLocalizationTypeDefiner()
			: base(20000)
		{
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00004498 File Offset: 0x00002698
		protected override void DefineClassTypes()
		{
			base.AddClassDefinition(typeof(TextObject), 1, null);
		}

		// Token: 0x06000090 RID: 144 RVA: 0x000044AC File Offset: 0x000026AC
		protected override void DefineContainerDefinitions()
		{
			base.ConstructContainerDefinition(typeof(Dictionary<string, TextObject>));
		}
	}
}
