using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TaleWorlds.Library;

namespace TaleWorlds.PlatformService
{
	// Token: 0x02000011 RID: 17
	public class PlatformServices
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000024E6 File Offset: 0x000006E6
		public static IPlatformServices Instance
		{
			get
			{
				return PlatformServices._platformServices;
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000024ED File Offset: 0x000006ED
		public static IPlatformInvitationServices InvitationServices
		{
			get
			{
				return PlatformServices._platformServices as IPlatformInvitationServices;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000089 RID: 137 RVA: 0x000024F9 File Offset: 0x000006F9
		// (set) Token: 0x0600008A RID: 138 RVA: 0x00002500 File Offset: 0x00000700
		public static Action<SessionInvitationType> OnSessionInvitationAccepted { get; set; }

		// Token: 0x17000016 RID: 22
		// (get) Token: 0x0600008B RID: 139 RVA: 0x00002508 File Offset: 0x00000708
		// (set) Token: 0x0600008C RID: 140 RVA: 0x0000250F File Offset: 0x0000070F
		public static Action OnPlatformRequestedMultiplayer { get; set; }

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x0600008D RID: 141 RVA: 0x00002517 File Offset: 0x00000717
		// (set) Token: 0x0600008E RID: 142 RVA: 0x0000251E File Offset: 0x0000071E
		public static bool IsPlatformRequestedMultiplayer { get; private set; } = false;

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600008F RID: 143 RVA: 0x00002526 File Offset: 0x00000726
		// (set) Token: 0x06000090 RID: 144 RVA: 0x0000252D File Offset: 0x0000072D
		public static SessionInvitationType SessionInvitationType { get; private set; } = SessionInvitationType.None;

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000091 RID: 145 RVA: 0x00002535 File Offset: 0x00000735
		// (set) Token: 0x06000092 RID: 146 RVA: 0x0000253C File Offset: 0x0000073C
		public static bool IsPlatformRequestedContinueGame { get; private set; }

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000093 RID: 147 RVA: 0x00002544 File Offset: 0x00000744
		public static string ProviderName
		{
			get
			{
				return PlatformServices._platformServices.ProviderName;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000094 RID: 148 RVA: 0x00002550 File Offset: 0x00000750
		public static string UserId
		{
			get
			{
				return PlatformServices._platformServices.UserId;
			}
		}

		// Token: 0x06000096 RID: 150 RVA: 0x00002574 File Offset: 0x00000774
		public static void Setup(IPlatformServices platformServices)
		{
			PlatformServices._platformServices = platformServices;
		}

		// Token: 0x06000097 RID: 151 RVA: 0x0000257C File Offset: 0x0000077C
		public static bool Initialize(IFriendListService[] additionalFriendListServices)
		{
			return PlatformServices._platformServices.Initialize(additionalFriendListServices);
		}

		// Token: 0x06000098 RID: 152 RVA: 0x00002589 File Offset: 0x00000789
		public static void Terminate()
		{
			PlatformServices._platformServices.Terminate();
		}

		// Token: 0x06000099 RID: 153 RVA: 0x00002595 File Offset: 0x00000795
		public static void ConnectionStateChanged(bool isAuthenticated)
		{
			Action<bool> onConnectionStateChanged = PlatformServices.OnConnectionStateChanged;
			if (onConnectionStateChanged == null)
			{
				return;
			}
			onConnectionStateChanged(isAuthenticated);
		}

		// Token: 0x0600009A RID: 154 RVA: 0x000025A7 File Offset: 0x000007A7
		public static void MultiplayerGameStateChanged(bool isPlaying)
		{
			Action<bool> onMultiplayerGameStateChanged = PlatformServices.OnMultiplayerGameStateChanged;
			if (onMultiplayerGameStateChanged == null)
			{
				return;
			}
			onMultiplayerGameStateChanged(isPlaying);
		}

		// Token: 0x0600009B RID: 155 RVA: 0x000025B9 File Offset: 0x000007B9
		public static void LobbyClientStateChanged(bool atLobby, bool isPartyLeaderOrSolo)
		{
			Action<bool, bool> onLobbyClientStateChanged = PlatformServices.OnLobbyClientStateChanged;
			if (onLobbyClientStateChanged == null)
			{
				return;
			}
			onLobbyClientStateChanged(atLobby, isPartyLeaderOrSolo);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x000025CC File Offset: 0x000007CC
		public static void FireOnSessionInvitationAccepted(SessionInvitationType sessionInvitationType)
		{
			PlatformServices.SessionInvitationType = sessionInvitationType;
			if (PlatformServices.OnSessionInvitationAccepted != null)
			{
				Delegate[] invocationList = PlatformServices.OnSessionInvitationAccepted.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					Action<SessionInvitationType> action;
					if ((action = invocationList[i] as Action<SessionInvitationType>) != null)
					{
						action(sessionInvitationType);
					}
				}
			}
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00002614 File Offset: 0x00000814
		public static void FireOnPlatformRequestedMultiplayer()
		{
			PlatformServices.IsPlatformRequestedMultiplayer = true;
			if (PlatformServices.OnPlatformRequestedMultiplayer != null)
			{
				Delegate[] invocationList = PlatformServices.OnPlatformRequestedMultiplayer.GetInvocationList();
				for (int i = 0; i < invocationList.Length; i++)
				{
					Action action;
					if ((action = invocationList[i] as Action) != null)
					{
						action();
					}
				}
			}
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00002659 File Offset: 0x00000859
		public static void OnSessionInvitationHandled()
		{
			PlatformServices.SessionInvitationType = SessionInvitationType.None;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00002661 File Offset: 0x00000861
		public static void OnPlatformMultiplayerRequestHandled()
		{
			PlatformServices.IsPlatformRequestedMultiplayer = false;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00002669 File Offset: 0x00000869
		public static void SetIsPlatformRequestedContinueGame(bool isRequested)
		{
			PlatformServices.IsPlatformRequestedContinueGame = true;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00002674 File Offset: 0x00000874
		public static async Task<string> FilterString(string content, string defaultContent)
		{
			TaskAwaiter<bool> taskAwaiter = PlatformServices.Instance.VerifyString(content).GetAwaiter();
			if (!taskAwaiter.IsCompleted)
			{
				await taskAwaiter;
				TaskAwaiter<bool> taskAwaiter2;
				taskAwaiter = taskAwaiter2;
				taskAwaiter2 = default(TaskAwaiter<bool>);
			}
			string text;
			if (!taskAwaiter.GetResult())
			{
				text = defaultContent;
			}
			else
			{
				text = content;
			}
			return text;
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000026C4 File Offset: 0x000008C4
		[CommandLineFunctionality.CommandLineArgumentFunction("trigger_invitation", "platform_services")]
		public static string TriggerInvitation(List<string> strings)
		{
			SessionInvitationType sessionInvitationType;
			if (strings.Count == 0 || !Enum.TryParse<SessionInvitationType>(strings[0], out sessionInvitationType))
			{
				sessionInvitationType = SessionInvitationType.Multiplayer;
			}
			PlatformServices.FireOnSessionInvitationAccepted(sessionInvitationType);
			return "Triggered invitation with " + sessionInvitationType;
		}

		// Token: 0x04000028 RID: 40
		private static IPlatformServices _platformServices = new NullPlatformServices();

		// Token: 0x04000029 RID: 41
		public static Action<bool> OnConnectionStateChanged;

		// Token: 0x0400002A RID: 42
		public static Action<bool> OnMultiplayerGameStateChanged;

		// Token: 0x0400002B RID: 43
		public static Action<bool, bool> OnLobbyClientStateChanged;
	}
}
