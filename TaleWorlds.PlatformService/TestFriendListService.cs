using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaleWorlds.Diamond.AccessProvider.Test;
using TaleWorlds.Localization;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.PlatformService
{
	// Token: 0x02000012 RID: 18
	public class TestFriendListService : IFriendListService
	{
		// Token: 0x060000A4 RID: 164 RVA: 0x0000270C File Offset: 0x0000090C
		public TestFriendListService(string userName, PlayerId myPlayerId)
		{
			this._userName = userName;
			this._playerId = myPlayerId;
			this._testUserNames = new Dictionary<PlayerId, string>();
			this._testUserPlayerIds = new Dictionary<string, PlayerId>();
			this._testUserNames.Add(this._playerId, this._userName);
			this._testUserPlayerIds.Add(this._userName, this._playerId);
			for (int i = 1; i <= 12; i++)
			{
				string text = "TestPlayer" + i;
				PlayerId playerIdFromUserName = TestLoginAccessProvider.GetPlayerIdFromUserName(text);
				if (!this._testUserNames.ContainsKey(playerIdFromUserName))
				{
					this._testUserNames.Add(playerIdFromUserName, text);
					this._testUserPlayerIds.Add(text, playerIdFromUserName);
				}
			}
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000027BE File Offset: 0x000009BE
		string IFriendListService.GetServiceCodeName()
		{
			return "Test";
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000027C5 File Offset: 0x000009C5
		TextObject IFriendListService.GetServiceLocalizedName()
		{
			return new TextObject("{=!}Test", null);
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x000027D2 File Offset: 0x000009D2
		FriendListServiceType IFriendListService.GetFriendListServiceType()
		{
			return FriendListServiceType.Test;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x000027D5 File Offset: 0x000009D5
		Task<bool> IFriendListService.GetUserOnlineStatus(PlayerId providedId)
		{
			if (this._testUserNames.ContainsKey(providedId))
			{
				return Task.FromResult<bool>(true);
			}
			return Task.FromResult<bool>(false);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x000027F2 File Offset: 0x000009F2
		Task<bool> IFriendListService.IsPlayingThisGame(PlayerId providedId)
		{
			if (this._testUserNames.ContainsKey(providedId))
			{
				return Task.FromResult<bool>(true);
			}
			return Task.FromResult<bool>(false);
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x060000AA RID: 170 RVA: 0x0000280F File Offset: 0x00000A0F
		bool IFriendListService.InGameStatusFetchable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x060000AB RID: 171 RVA: 0x00002812 File Offset: 0x00000A12
		bool IFriendListService.AllowsFriendOperations
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x060000AC RID: 172 RVA: 0x00002815 File Offset: 0x00000A15
		bool IFriendListService.CanInvitePlayersToPlatformSession
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x060000AD RID: 173 RVA: 0x00002818 File Offset: 0x00000A18
		bool IFriendListService.IncludeInAllFriends
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x0000281B File Offset: 0x00000A1B
		IEnumerable<PlayerId> IFriendListService.GetAllFriends()
		{
			List<string> list = new List<string>();
			if (this._userName == "TestPlayer1" || this._userName == "TestPlayer2" || this._userName == "TestPlayer3" || this._userName == "TestPlayer4" || this._userName == "TestPlayer5" || this._userName == "TestPlayer6")
			{
				list.Add("TestPlayer1");
				list.Add("TestPlayer2");
				list.Add("TestPlayer3");
				list.Add("TestPlayer4");
				list.Add("TestPlayer5");
				list.Add("TestPlayer6");
			}
			else if (this._userName == "TestPlayer7" || this._userName == "TestPlayer8" || this._userName == "TestPlayer9" || this._userName == "TestPlayer10" || this._userName == "TestPlayer11" || this._userName == "TestPlayer12")
			{
				list.Add("TestPlayer7");
				list.Add("TestPlayer8");
				list.Add("TestPlayer9");
				list.Add("TestPlayer10");
				list.Add("TestPlayer11");
				list.Add("TestPlayer12");
			}
			else
			{
				list.Add("TestPlayer1");
				list.Add("TestPlayer2");
				list.Add("TestPlayer3");
				list.Add("TestPlayer4");
				list.Add("TestPlayer5");
				list.Add("TestPlayer6");
				list.Add("TestPlayer7");
				list.Add("TestPlayer8");
				list.Add("TestPlayer9");
				list.Add("TestPlayer10");
				list.Add("TestPlayer11");
				list.Add("TestPlayer12");
			}
			foreach (string text in list)
			{
				if (this._userName != text)
				{
					yield return TestLoginAccessProvider.GetPlayerIdFromUserName(text);
				}
			}
			List<string>.Enumerator enumerator = default(List<string>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x0000282B File Offset: 0x00000A2B
		IEnumerable<PlayerId> IFriendListService.GetPendingRequests()
		{
			return null;
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000282E File Offset: 0x00000A2E
		IEnumerable<PlayerId> IFriendListService.GetReceivedRequests()
		{
			return null;
		}

		// Token: 0x14000011 RID: 17
		// (add) Token: 0x060000B1 RID: 177 RVA: 0x00002834 File Offset: 0x00000A34
		// (remove) Token: 0x060000B2 RID: 178 RVA: 0x0000286C File Offset: 0x00000A6C
		public event Action<PlayerId> OnUserStatusChanged;

		// Token: 0x14000012 RID: 18
		// (add) Token: 0x060000B3 RID: 179 RVA: 0x000028A4 File Offset: 0x00000AA4
		// (remove) Token: 0x060000B4 RID: 180 RVA: 0x000028DC File Offset: 0x00000ADC
		public event Action<PlayerId> OnFriendRemoved;

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x060000B5 RID: 181 RVA: 0x00002914 File Offset: 0x00000B14
		// (remove) Token: 0x060000B6 RID: 182 RVA: 0x0000294C File Offset: 0x00000B4C
		public event Action OnFriendListChanged;

		// Token: 0x060000B7 RID: 183 RVA: 0x00002984 File Offset: 0x00000B84
		private void Dummy()
		{
			if (this.OnUserStatusChanged != null)
			{
				this.OnUserStatusChanged(default(PlayerId));
			}
			if (this.OnFriendRemoved != null)
			{
				this.OnFriendRemoved(default(PlayerId));
			}
			if (this.OnFriendListChanged != null)
			{
				this.OnFriendListChanged();
			}
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x000029DC File Offset: 0x00000BDC
		Task<string> IFriendListService.GetUserName(PlayerId providedId)
		{
			string text = "-";
			string text2;
			if (this._testUserNames.TryGetValue(providedId, out text2))
			{
				text = text2;
			}
			return Task.FromResult<string>(text);
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x00002A08 File Offset: 0x00000C08
		Task<PlayerId> IFriendListService.GetUserWithName(string name)
		{
			PlayerId playerId = default(PlayerId);
			PlayerId playerId2;
			if (this._testUserPlayerIds.TryGetValue(name, out playerId2))
			{
				playerId = playerId2;
			}
			return Task.FromResult<PlayerId>(playerId);
		}

		// Token: 0x04000031 RID: 49
		private string _userName;

		// Token: 0x04000032 RID: 50
		private PlayerId _playerId;

		// Token: 0x04000033 RID: 51
		private Dictionary<PlayerId, string> _testUserNames;

		// Token: 0x04000034 RID: 52
		private Dictionary<string, PlayerId> _testUserPlayerIds;
	}
}
