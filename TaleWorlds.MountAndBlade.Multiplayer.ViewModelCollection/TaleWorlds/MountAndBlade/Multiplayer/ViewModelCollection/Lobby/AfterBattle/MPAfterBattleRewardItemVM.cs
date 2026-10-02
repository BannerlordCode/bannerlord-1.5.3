using System;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.AfterBattle
{
	// Token: 0x02000088 RID: 136
	public abstract class MPAfterBattleRewardItemVM : ViewModel
	{
		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x00029818 File Offset: 0x00027A18
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x00029820 File Offset: 0x00027A20
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (value != this._type)
				{
					this._type = value;
					base.OnPropertyChangedWithValue(value, "Type");
				}
			}
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06000D78 RID: 3448 RVA: 0x0002983E File Offset: 0x00027A3E
		// (set) Token: 0x06000D79 RID: 3449 RVA: 0x00029846 File Offset: 0x00027A46
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				if (value != this._name)
				{
					this._name = value;
					base.OnPropertyChangedWithValue<string>(value, "Name");
				}
			}
		}

		// Token: 0x04000625 RID: 1573
		private int _type;

		// Token: 0x04000626 RID: 1574
		private string _name;

		// Token: 0x02000184 RID: 388
		public enum RewardType
		{
			// Token: 0x04000A9A RID: 2714
			Loot,
			// Token: 0x04000A9B RID: 2715
			Badge
		}
	}
}
