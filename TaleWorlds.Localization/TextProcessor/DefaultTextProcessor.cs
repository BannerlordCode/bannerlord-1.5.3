using System;
using System.Globalization;
using System.Text;

namespace TaleWorlds.Localization.TextProcessor
{
	// Token: 0x02000025 RID: 37
	public class DefaultTextProcessor : LanguageSpecificTextProcessor
	{
		// Token: 0x060000E9 RID: 233 RVA: 0x00005247 File Offset: 0x00003447
		public override void ProcessToken(string sourceText, ref int cursorPos, string token, StringBuilder outputString)
		{
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000EA RID: 234 RVA: 0x00005249 File Offset: 0x00003449
		public override CultureInfo CultureInfoForLanguage
		{
			get
			{
				return CultureInfo.InvariantCulture;
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00005250 File Offset: 0x00003450
		public override void ClearTemporaryData()
		{
		}
	}
}
