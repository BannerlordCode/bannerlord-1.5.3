using System;
using TaleWorlds.Core.ViewModelCollection.Generic;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.Lobby.Friends;
using TaleWorlds.PlatformService;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection
{
	// Token: 0x02000013 RID: 19
	public static class MultiplayerPlayerContextMenuHelper
	{
		// Token: 0x0600010E RID: 270 RVA: 0x00005888 File Offset: 0x00003A88
		public static void AddLobbyViewProfileOptions(MPLobbyPlayerBaseVM player, MBBindingList<StringPairItemWithActionVM> contextMenuOptions)
		{
			contextMenuOptions.Add(new StringPairItemWithActionVM(new Action<object>(MultiplayerPlayerContextMenuHelper.ExecuteViewProfile), new TextObject("{=bjJkW9dO}View Profile", null).ToString(), "ViewProfile", player));
			MultiplayerPlayerContextMenuHelper.AddPlatformProfileCardOption(new Action<object>(MultiplayerPlayerContextMenuHelper.ExecuteViewPlatformProfileCardLobby), player, player.ProvidedID, contextMenuOptions);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x000058DB File Offset: 0x00003ADB
		public static void AddMissionViewProfileOptions(MPPlayerVM player, MBBindingList<StringPairItemWithActionVM> contextMenuOptions)
		{
			MultiplayerPlayerContextMenuHelper.AddPlatformProfileCardOption(new Action<object>(MultiplayerPlayerContextMenuHelper.ExecuteViewPlatformProfileCardMission), player, player.Peer.Peer.Id, contextMenuOptions);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00005900 File Offset: 0x00003B00
		private static void AddPlatformProfileCardOption(Action<object> onExecuted, object target, PlayerId playerId, MBBindingList<StringPairItemWithActionVM> contextMenuOptions)
		{
			if (!PlatformServices.Instance.IsPlayerProfileCardAvailable(NetworkMain.GameClient.PlayerID))
			{
				return;
			}
			if (!PlatformServices.Instance.IsPlayerProfileCardAvailable(playerId))
			{
				return;
			}
			if (!playerId.ProvidedType.SupportsPlayerCard())
			{
				return;
			}
			TextObject empty = TextObject.GetEmpty();
			Debug.FailedAssert("Platform profile is supported but \"Show Profile\" text is not defined!", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection\\MultiplayerPlayerContextMenuHelper.cs", "AddPlatformProfileCardOption", 51);
			if (!empty.IsEmpty())
			{
				contextMenuOptions.Add(new StringPairItemWithActionVM(onExecuted, empty.ToString(), "ViewProfile", target));
			}
		}

		// Token: 0x06000111 RID: 273 RVA: 0x0000597D File Offset: 0x00003B7D
		private static void ExecuteViewProfile(object playerObj)
		{
			(playerObj as MPLobbyPlayerBaseVM).ExecuteShowProfile();
		}

		// Token: 0x06000112 RID: 274 RVA: 0x0000598A File Offset: 0x00003B8A
		private static void ExecuteViewPlatformProfileCardLobby(object playerObj)
		{
			PlatformServices.Instance.ShowPlayerProfileCard((playerObj as MPLobbyPlayerBaseVM).ProvidedID);
		}

		// Token: 0x06000113 RID: 275 RVA: 0x000059A1 File Offset: 0x00003BA1
		private static void ExecuteViewPlatformProfileCardMission(object playerObj)
		{
			PlatformServices.Instance.ShowPlayerProfileCard((playerObj as MPPlayerVM).Peer.Peer.Id);
		}
	}
}
