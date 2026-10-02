using System;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.Diamond.MultiplayerBadges;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.AfterBattle
{
	// Token: 0x02000085 RID: 133
	public class MPAfterBattleBadgeRewardItemVM : MPAfterBattleRewardItemVM
	{
		// Token: 0x06000D33 RID: 3379 RVA: 0x00028B8E File Offset: 0x00026D8E
		public MPAfterBattleBadgeRewardItemVM(Badge badge)
		{
			base.Type = 1;
			base.Name = badge.Name.ToString();
			this.BadgeID = badge.StringId;
			this.RefreshValues();
		}

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x06000D34 RID: 3380 RVA: 0x00028BC0 File Offset: 0x00026DC0
		// (set) Token: 0x06000D35 RID: 3381 RVA: 0x00028BC8 File Offset: 0x00026DC8
		[DataSourceProperty]
		public string BadgeID
		{
			get
			{
				return this._badgeID;
			}
			set
			{
				if (value != this._badgeID)
				{
					this._badgeID = value;
					base.OnPropertyChangedWithValue<string>(value, "BadgeID");
				}
			}
		}

		// Token: 0x040005F6 RID: 1526
		private string _badgeID;
	}
}
