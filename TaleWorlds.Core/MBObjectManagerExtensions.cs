using System;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.Core
{
	// Token: 0x020000B2 RID: 178
	public static class MBObjectManagerExtensions
	{
		// Token: 0x06000959 RID: 2393 RVA: 0x0001E7B4 File Offset: 0x0001C9B4
		public static void LoadXML(this MBObjectManager objectManager, string id, bool skipXmlFilterForEditor = false)
		{
			Game game = Game.Current;
			bool flag = false;
			string text = "";
			if (game != null)
			{
				flag = game.GameType.IsDevelopment;
				text = game.GameType.GameTypeStringId;
			}
			objectManager.LoadXML(id, flag, text, skipXmlFilterForEditor);
		}
	}
}
