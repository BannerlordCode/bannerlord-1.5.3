using System;

namespace TaleWorlds.MountAndBlade.View.MissionViews.Singleplayer
{
	// Token: 0x02000091 RID: 145
	public class MissionConversationView : MissionView
	{
		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000562 RID: 1378 RVA: 0x000276B4 File Offset: 0x000258B4
		public static MissionConversationView Current
		{
			get
			{
				return Mission.Current.GetMissionBehavior<MissionConversationView>();
			}
		}
	}
}
