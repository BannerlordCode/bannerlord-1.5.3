using System;
using System.Collections.Generic;
using TaleWorlds.Localization;

namespace TaleWorlds.Core
{
	// Token: 0x02000079 RID: 121
	public static class GameTexts
	{
		// Token: 0x06000841 RID: 2113 RVA: 0x0001B8AC File Offset: 0x00019AAC
		public static void Initialize(GameTextManager gameTextManager)
		{
			GameTexts._gameTextManager = gameTextManager;
			GameTexts.InitializeGlobalTags();
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x0001B8B9 File Offset: 0x00019AB9
		public static TextObject FindText(string id, string variation = null)
		{
			return GameTexts._gameTextManager.FindText(id, variation);
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x0001B8C7 File Offset: 0x00019AC7
		public static bool TryGetText(string id, out TextObject textObject, string variation = null)
		{
			return GameTexts._gameTextManager.TryGetText(id, variation, out textObject);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x0001B8D6 File Offset: 0x00019AD6
		public static IEnumerable<TextObject> FindAllTextVariations(string id)
		{
			return GameTexts._gameTextManager.FindAllTextVariations(id);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0001B8E3 File Offset: 0x00019AE3
		public static void SetVariable(string variableName, string content)
		{
			MBTextManager.SetTextVariable(variableName, content, false);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x0001B8ED File Offset: 0x00019AED
		public static void SetVariable(string variableName, float content)
		{
			MBTextManager.SetTextVariable(variableName, content, 2);
		}

		// Token: 0x06000847 RID: 2119 RVA: 0x0001B8F7 File Offset: 0x00019AF7
		public static void SetVariable(string variableName, int content)
		{
			MBTextManager.SetTextVariable(variableName, content);
		}

		// Token: 0x06000848 RID: 2120 RVA: 0x0001B900 File Offset: 0x00019B00
		public static void SetVariable(string variableName, TextObject content)
		{
			MBTextManager.SetTextVariable(variableName, content, false);
		}

		// Token: 0x06000849 RID: 2121 RVA: 0x0001B90A File Offset: 0x00019B0A
		public static void ClearInstance()
		{
			GameTexts._gameTextManager = null;
		}

		// Token: 0x0600084A RID: 2122 RVA: 0x0001B912 File Offset: 0x00019B12
		public static GameTexts.GameTextHelper AddGameTextWithVariation(string id)
		{
			return new GameTexts.GameTextHelper(id);
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x0001B91A File Offset: 0x00019B1A
		private static void InitializeGlobalTags()
		{
			GameTexts.SetVariable("newline", "\n");
		}

		// Token: 0x04000426 RID: 1062
		private static GameTextManager _gameTextManager;

		// Token: 0x0200011D RID: 285
		public class GameTextHelper
		{
			// Token: 0x06000C13 RID: 3091 RVA: 0x00026983 File Offset: 0x00024B83
			public GameTextHelper(string id)
			{
				this._id = id;
			}

			// Token: 0x06000C14 RID: 3092 RVA: 0x00026992 File Offset: 0x00024B92
			public GameTexts.GameTextHelper Variation(string text, params object[] propertiesAndWeights)
			{
				GameTexts._gameTextManager.AddGameText(this._id).AddVariation(text, propertiesAndWeights);
				return this;
			}

			// Token: 0x06000C15 RID: 3093 RVA: 0x000269AC File Offset: 0x00024BAC
			public static TextObject MergeTextObjectsWithComma(List<TextObject> textObjects, bool includeAnd)
			{
				return GameTexts.GameTextHelper.MergeTextObjectsWithSymbol(textObjects, new TextObject("{=kfdxjIad}, ", null), includeAnd ? new TextObject("{=eob9goyW} and ", null) : null);
			}

			// Token: 0x06000C16 RID: 3094 RVA: 0x000269D0 File Offset: 0x00024BD0
			public static TextObject MergeTextObjectsWithSymbol(List<TextObject> textObjects, TextObject symbol, TextObject lastSymbol = null)
			{
				int count = textObjects.Count;
				TextObject textObject;
				if (count == 0)
				{
					textObject = TextObject.GetEmpty();
				}
				else if (count == 1)
				{
					textObject = textObjects[0];
				}
				else
				{
					string text = "{=!}";
					for (int i = 0; i < textObjects.Count - 2; i++)
					{
						text = string.Concat(new object[] { text, "{VAR_", i, "}{SYMBOL}" });
					}
					text = string.Concat(new object[]
					{
						text,
						"{VAR_",
						textObjects.Count - 2,
						"}{LAST_SYMBOL}{VAR_",
						textObjects.Count - 1,
						"}"
					});
					textObject = new TextObject(text, null);
					for (int j = 0; j < textObjects.Count; j++)
					{
						textObject.SetTextVariable("VAR_" + j, textObjects[j]);
					}
					textObject.SetTextVariable("SYMBOL", symbol);
					textObject.SetTextVariable("LAST_SYMBOL", lastSymbol ?? symbol);
				}
				return textObject;
			}

			// Token: 0x040007B5 RID: 1973
			private string _id;
		}
	}
}
