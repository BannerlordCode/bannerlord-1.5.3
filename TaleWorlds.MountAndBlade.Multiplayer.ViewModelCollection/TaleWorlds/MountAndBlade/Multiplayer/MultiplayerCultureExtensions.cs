using System;
using TaleWorlds.Core;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.MountAndBlade.Multiplayer
{
	// Token: 0x02000004 RID: 4
	public static class MultiplayerCultureExtensions
	{
		// Token: 0x06000003 RID: 3 RVA: 0x00002058 File Offset: 0x00000258
		internal static BasicCultureObject GetCulture(this MissionScoreboardComponent.MissionScoreboardSide side)
		{
			if (side == null)
			{
				return null;
			}
			string text = ((side.Side == BattleSideEnum.Attacker) ? MultiplayerOptions.OptionType.CultureTeam1.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions) : MultiplayerOptions.OptionType.CultureTeam2.GetStrValue(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions));
			if (!string.IsNullOrEmpty(text))
			{
				return MBObjectManager.Instance.GetObject<BasicCultureObject>(text);
			}
			return null;
		}
	}
}
