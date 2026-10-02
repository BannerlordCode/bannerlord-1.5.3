using System;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000232 RID: 562
	public static class GameKeyTextExtensions
	{
		// Token: 0x06002141 RID: 8513 RVA: 0x00075759 File Offset: 0x00073959
		public static TextObject GetHotKeyGameText(this GameTextManager gameTextManager, string categoryName, string hotKeyId)
		{
			return gameTextManager.GetHotKeyGameTextFromKeyID(HotKeyManager.GetHotKeyId(categoryName, hotKeyId));
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x00075768 File Offset: 0x00073968
		public static TextObject GetHotKeyGameText(this GameTextManager gameTextManager, string categoryName, int gameKeyId)
		{
			return gameTextManager.GetHotKeyGameTextFromKeyID(HotKeyManager.GetHotKeyId(categoryName, gameKeyId));
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00075777 File Offset: 0x00073977
		public static TextObject GetHotKeyGameTextFromKeyID(this GameTextManager gameTextManager, string keyId)
		{
			return gameTextManager.FindText("str_game_key_text", keyId.ToLower());
		}
	}
}
