using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TaleWorlds.PlayerServices.Avatar
{
	// Token: 0x0200000F RID: 15
	public class SteamAvatarService : ApiAvatarServiceBase
	{
		// Token: 0x06000076 RID: 118 RVA: 0x00003148 File Offset: 0x00001348
		protected override async Task FetchAvatars()
		{
			await Task.Delay(3000);
			List<ValueTuple<ulong, AvatarData>> waitingAccounts = base.WaitingAccounts;
			lock (waitingAccounts)
			{
				if (base.WaitingAccounts.Count < 1)
				{
					return;
				}
				if (base.WaitingAccounts.Count <= 100)
				{
					base.InProgressAccounts = base.WaitingAccounts;
					base.WaitingAccounts = new List<ValueTuple<ulong, AvatarData>>();
				}
				else
				{
					base.InProgressAccounts = base.WaitingAccounts.GetRange(0, 100);
					base.WaitingAccounts.RemoveRange(0, 100);
				}
			}
			string text = "http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key=820D6EC50E6AAE61E460EA207D8966F7&steamids=" + string.Join<ulong>(",", base.InProgressAccounts.Select<ValueTuple<ulong, AvatarData>, ulong>(([TupleElementNames(new string[] { "accountId", "avatarData" })] ValueTuple<ulong, AvatarData> a) => a.Item1));
			SteamAvatarService.SteamPlayers steamPlayers = null;
			try
			{
				SteamAvatarService.GetPlayerSummariesResult getPlayerSummariesResult = JsonConvert.DeserializeObject<SteamAvatarService.GetPlayerSummariesResult>(await new TimeoutWebClient().DownloadStringTaskAsync(text));
				bool flag2;
				if (getPlayerSummariesResult == null)
				{
					flag2 = null != null;
				}
				else
				{
					SteamAvatarService.SteamPlayers response = getPlayerSummariesResult.response;
					flag2 = ((response != null) ? response.players : null) != null;
				}
				if (flag2 && getPlayerSummariesResult.response.players.Length != 0)
				{
					steamPlayers = getPlayerSummariesResult.response;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
			if (steamPlayers == null || steamPlayers.players.Length < 1)
			{
				foreach (ValueTuple<ulong, AvatarData> valueTuple in base.InProgressAccounts)
				{
					valueTuple.Item2.SetFailed();
				}
			}
			else
			{
				List<Task> list = new List<Task>();
				foreach (ValueTuple<ulong, AvatarData> valueTuple2 in base.InProgressAccounts)
				{
					ulong item = valueTuple2.Item1;
					AvatarData item2 = valueTuple2.Item2;
					string text2 = string.Concat(item);
					string text3 = null;
					foreach (SteamAvatarService.SteamPlayerSummary steamPlayerSummary in steamPlayers.players)
					{
						if (steamPlayerSummary.steamid == text2)
						{
							text3 = steamPlayerSummary.avatarfull;
							break;
						}
					}
					if (!string.IsNullOrWhiteSpace(text3))
					{
						list.Add(this.UpdateAvatarImageData(item, text3, item2));
					}
					else
					{
						item2.SetFailed();
					}
				}
				if (list.Count > 0)
				{
					await Task.WhenAll(list);
				}
			}
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00003190 File Offset: 0x00001390
		private async Task UpdateAvatarImageData(ulong accountId, string avatarUrl, AvatarData avatarData)
		{
			if (!string.IsNullOrWhiteSpace(avatarUrl))
			{
				byte[] array = await new TimeoutWebClient().DownloadDataTaskAsync(avatarUrl);
				if (array != null && array.Length != 0)
				{
					avatarData.SetImageData(array);
					Dictionary<ulong, AvatarData> avatarImageCache = base.AvatarImageCache;
					lock (avatarImageCache)
					{
						base.AvatarImageCache[accountId] = avatarData;
					}
				}
			}
		}

		// Token: 0x0400002F RID: 47
		private const int FetchTaskWaitTime = 3000;

		// Token: 0x04000030 RID: 48
		private const string SteamWebApiKey = "820D6EC50E6AAE61E460EA207D8966F7";

		// Token: 0x04000031 RID: 49
		private const int MaxAccountsPerRequest = 100;

		// Token: 0x02000015 RID: 21
		private class GetPlayerSummariesResult
		{
			// Token: 0x1700001D RID: 29
			// (get) Token: 0x06000083 RID: 131 RVA: 0x0000355E File Offset: 0x0000175E
			// (set) Token: 0x06000084 RID: 132 RVA: 0x00003566 File Offset: 0x00001766
			public SteamAvatarService.SteamPlayers response { get; set; }
		}

		// Token: 0x02000016 RID: 22
		private class SteamPlayers
		{
			// Token: 0x1700001E RID: 30
			// (get) Token: 0x06000086 RID: 134 RVA: 0x00003577 File Offset: 0x00001777
			// (set) Token: 0x06000087 RID: 135 RVA: 0x0000357F File Offset: 0x0000177F
			public SteamAvatarService.SteamPlayerSummary[] players { get; set; }
		}

		// Token: 0x02000017 RID: 23
		private class SteamPlayerSummary
		{
			// Token: 0x1700001F RID: 31
			// (get) Token: 0x06000089 RID: 137 RVA: 0x00003590 File Offset: 0x00001790
			// (set) Token: 0x0600008A RID: 138 RVA: 0x00003598 File Offset: 0x00001798
			public string avatar { get; set; }

			// Token: 0x17000020 RID: 32
			// (get) Token: 0x0600008B RID: 139 RVA: 0x000035A1 File Offset: 0x000017A1
			// (set) Token: 0x0600008C RID: 140 RVA: 0x000035A9 File Offset: 0x000017A9
			public string avatarfull { get; set; }

			// Token: 0x17000021 RID: 33
			// (get) Token: 0x0600008D RID: 141 RVA: 0x000035B2 File Offset: 0x000017B2
			// (set) Token: 0x0600008E RID: 142 RVA: 0x000035BA File Offset: 0x000017BA
			public string avatarmedium { get; set; }

			// Token: 0x17000022 RID: 34
			// (get) Token: 0x0600008F RID: 143 RVA: 0x000035C3 File Offset: 0x000017C3
			// (set) Token: 0x06000090 RID: 144 RVA: 0x000035CB File Offset: 0x000017CB
			public int communityvisibilitystate { get; set; }

			// Token: 0x17000023 RID: 35
			// (get) Token: 0x06000091 RID: 145 RVA: 0x000035D4 File Offset: 0x000017D4
			// (set) Token: 0x06000092 RID: 146 RVA: 0x000035DC File Offset: 0x000017DC
			public int lastlogoff { get; set; }

			// Token: 0x17000024 RID: 36
			// (get) Token: 0x06000093 RID: 147 RVA: 0x000035E5 File Offset: 0x000017E5
			// (set) Token: 0x06000094 RID: 148 RVA: 0x000035ED File Offset: 0x000017ED
			public string personaname { get; set; }

			// Token: 0x17000025 RID: 37
			// (get) Token: 0x06000095 RID: 149 RVA: 0x000035F6 File Offset: 0x000017F6
			// (set) Token: 0x06000096 RID: 150 RVA: 0x000035FE File Offset: 0x000017FE
			public int personastate { get; set; }

			// Token: 0x17000026 RID: 38
			// (get) Token: 0x06000097 RID: 151 RVA: 0x00003607 File Offset: 0x00001807
			// (set) Token: 0x06000098 RID: 152 RVA: 0x0000360F File Offset: 0x0000180F
			public int personastateflags { get; set; }

			// Token: 0x17000027 RID: 39
			// (get) Token: 0x06000099 RID: 153 RVA: 0x00003618 File Offset: 0x00001818
			// (set) Token: 0x0600009A RID: 154 RVA: 0x00003620 File Offset: 0x00001820
			public string primaryclanid { get; set; }

			// Token: 0x17000028 RID: 40
			// (get) Token: 0x0600009B RID: 155 RVA: 0x00003629 File Offset: 0x00001829
			// (set) Token: 0x0600009C RID: 156 RVA: 0x00003631 File Offset: 0x00001831
			public int profilestate { get; set; }

			// Token: 0x17000029 RID: 41
			// (get) Token: 0x0600009D RID: 157 RVA: 0x0000363A File Offset: 0x0000183A
			// (set) Token: 0x0600009E RID: 158 RVA: 0x00003642 File Offset: 0x00001842
			public string profileurl { get; set; }

			// Token: 0x1700002A RID: 42
			// (get) Token: 0x0600009F RID: 159 RVA: 0x0000364B File Offset: 0x0000184B
			// (set) Token: 0x060000A0 RID: 160 RVA: 0x00003653 File Offset: 0x00001853
			public string realname { get; set; }

			// Token: 0x1700002B RID: 43
			// (get) Token: 0x060000A1 RID: 161 RVA: 0x0000365C File Offset: 0x0000185C
			// (set) Token: 0x060000A2 RID: 162 RVA: 0x00003664 File Offset: 0x00001864
			public string steamid { get; set; }

			// Token: 0x1700002C RID: 44
			// (get) Token: 0x060000A3 RID: 163 RVA: 0x0000366D File Offset: 0x0000186D
			// (set) Token: 0x060000A4 RID: 164 RVA: 0x00003675 File Offset: 0x00001875
			public int timecreated { get; set; }
		}
	}
}
