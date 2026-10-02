using System;
using System.Collections.Generic;

namespace TaleWorlds.TwoDimension
{
	// Token: 0x0200001F RID: 31
	public interface ILanguage
	{
		// Token: 0x06000130 RID: 304
		IEnumerable<char> GetForbiddenStartOfLineCharacters();

		// Token: 0x06000131 RID: 305
		bool IsCharacterForbiddenAtStartOfLine(char character);

		// Token: 0x06000132 RID: 306
		IEnumerable<char> GetForbiddenEndOfLineCharacters();

		// Token: 0x06000133 RID: 307
		bool IsCharacterForbiddenAtEndOfLine(char character);

		// Token: 0x06000134 RID: 308
		string GetLanguageID();

		// Token: 0x06000135 RID: 309
		string GetDefaultFontName();

		// Token: 0x06000136 RID: 310
		Font GetDefaultFont();

		// Token: 0x06000137 RID: 311
		char GetLineSeperatorChar();

		// Token: 0x06000138 RID: 312
		bool DoesLanguageRequireSpaceForNewline();

		// Token: 0x06000139 RID: 313
		bool FontMapHasKey(string keyFontName);

		// Token: 0x0600013A RID: 314
		Font GetMappedFont(string keyFontName);
	}
}
