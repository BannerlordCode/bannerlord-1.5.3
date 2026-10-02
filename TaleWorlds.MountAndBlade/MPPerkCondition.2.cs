using System;
using System.Collections.Generic;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000318 RID: 792
	public abstract class MPPerkCondition<T> : MPPerkCondition where T : MissionMultiplayerGameModeBase
	{
		// Token: 0x17000869 RID: 2153
		// (get) Token: 0x06002D69 RID: 11625 RVA: 0x000AF774 File Offset: 0x000AD974
		protected T GameModeInstance
		{
			get
			{
				Mission mission = Mission.Current;
				if (mission == null)
				{
					return default(T);
				}
				return mission.GetMissionBehavior<T>();
			}
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x000AF79C File Offset: 0x000AD99C
		protected override bool IsGameModesValid(List<string> gameModes)
		{
			if (typeof(MissionMultiplayerFlagDomination).IsAssignableFrom(typeof(T)))
			{
				string text = MultiplayerGameType.Skirmish.ToString();
				string text2 = MultiplayerGameType.Captain.ToString();
				foreach (string text3 in gameModes)
				{
					if (!text3.Equals(text, StringComparison.InvariantCultureIgnoreCase) && !text3.Equals(text2, StringComparison.InvariantCultureIgnoreCase))
					{
						return false;
					}
				}
				return true;
			}
			if (typeof(MissionMultiplayerTeamDeathmatch).IsAssignableFrom(typeof(T)))
			{
				string text4 = MultiplayerGameType.TeamDeathmatch.ToString();
				using (List<string>.Enumerator enumerator = gameModes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.Equals(text4, StringComparison.InvariantCultureIgnoreCase))
						{
							return false;
						}
					}
				}
				return true;
			}
			if (typeof(MissionMultiplayerSiege).IsAssignableFrom(typeof(T)))
			{
				string text5 = MultiplayerGameType.Siege.ToString();
				using (List<string>.Enumerator enumerator = gameModes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.Current.Equals(text5, StringComparison.InvariantCultureIgnoreCase))
						{
							return false;
						}
					}
				}
				return true;
			}
			Debug.FailedAssert("Not implemented game mode check", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\Perks\\MPPerkCondition.cs", "IsGameModesValid", 134);
			return false;
		}
	}
}
