using System;
using System.Text;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.View.Tableaus
{
	// Token: 0x02000039 RID: 57
	internal static class ThumbnailDebugUtility
	{
		// Token: 0x06000217 RID: 535 RVA: 0x0000EC18 File Offset: 0x0000CE18
		internal static string CreateDebugIdFrom(string renderId, string typeId, string additionalInfo = "")
		{
			string text = Common.CreateNanoIdFrom(renderId);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("uit");
			stringBuilder.Append('_');
			stringBuilder.Append(typeId);
			stringBuilder.Append('_');
			stringBuilder.Append(text);
			stringBuilder.Append('_');
			stringBuilder.Append(additionalInfo);
			string text2 = stringBuilder.ToString();
			if (text2.Length > 127)
			{
				text2 = text2.Substring(0, 127);
			}
			return text2;
		}
	}
}
