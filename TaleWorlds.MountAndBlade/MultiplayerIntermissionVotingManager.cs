using System;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200030F RID: 783
	public class MultiplayerIntermissionVotingManager
	{
		// Token: 0x1700085E RID: 2142
		// (get) Token: 0x06002CF5 RID: 11509 RVA: 0x000ACE78 File Offset: 0x000AB078
		public static MultiplayerIntermissionVotingManager Instance
		{
			get
			{
				MultiplayerIntermissionVotingManager multiplayerIntermissionVotingManager;
				if ((multiplayerIntermissionVotingManager = MultiplayerIntermissionVotingManager._instance) == null)
				{
					multiplayerIntermissionVotingManager = (MultiplayerIntermissionVotingManager._instance = new MultiplayerIntermissionVotingManager());
				}
				return multiplayerIntermissionVotingManager;
			}
		}

		// Token: 0x1700085F RID: 2143
		// (get) Token: 0x06002CF6 RID: 11510 RVA: 0x000ACE8E File Offset: 0x000AB08E
		// (set) Token: 0x06002CF7 RID: 11511 RVA: 0x000ACE96 File Offset: 0x000AB096
		public List<IntermissionVoteItem> MapVoteItems { get; private set; }

		// Token: 0x17000860 RID: 2144
		// (get) Token: 0x06002CF8 RID: 11512 RVA: 0x000ACE9F File Offset: 0x000AB09F
		// (set) Token: 0x06002CF9 RID: 11513 RVA: 0x000ACEA7 File Offset: 0x000AB0A7
		public List<IntermissionVoteItem> CultureVoteItems { get; private set; }

		// Token: 0x17000861 RID: 2145
		// (get) Token: 0x06002CFA RID: 11514 RVA: 0x000ACEB0 File Offset: 0x000AB0B0
		// (set) Token: 0x06002CFB RID: 11515 RVA: 0x000ACEB8 File Offset: 0x000AB0B8
		public List<CustomGameUsableMap> UsableMaps { get; private set; }

		// Token: 0x14000093 RID: 147
		// (add) Token: 0x06002CFC RID: 11516 RVA: 0x000ACEC4 File Offset: 0x000AB0C4
		// (remove) Token: 0x06002CFD RID: 11517 RVA: 0x000ACEFC File Offset: 0x000AB0FC
		public event MultiplayerIntermissionVotingManager.MapItemAddedDelegate OnMapItemAdded;

		// Token: 0x14000094 RID: 148
		// (add) Token: 0x06002CFE RID: 11518 RVA: 0x000ACF34 File Offset: 0x000AB134
		// (remove) Token: 0x06002CFF RID: 11519 RVA: 0x000ACF6C File Offset: 0x000AB16C
		public event MultiplayerIntermissionVotingManager.CultureItemAddedDelegate OnCultureItemAdded;

		// Token: 0x14000095 RID: 149
		// (add) Token: 0x06002D00 RID: 11520 RVA: 0x000ACFA4 File Offset: 0x000AB1A4
		// (remove) Token: 0x06002D01 RID: 11521 RVA: 0x000ACFDC File Offset: 0x000AB1DC
		public event MultiplayerIntermissionVotingManager.MapItemVoteCountChangedDelegate OnMapItemVoteCountChanged;

		// Token: 0x14000096 RID: 150
		// (add) Token: 0x06002D02 RID: 11522 RVA: 0x000AD014 File Offset: 0x000AB214
		// (remove) Token: 0x06002D03 RID: 11523 RVA: 0x000AD04C File Offset: 0x000AB24C
		public event MultiplayerIntermissionVotingManager.CultureItemVoteCountChangedDelegate OnCultureItemVoteCountChanged;

		// Token: 0x06002D04 RID: 11524 RVA: 0x000AD084 File Offset: 0x000AB284
		public MultiplayerIntermissionVotingManager()
		{
			this.MapVoteItems = new List<IntermissionVoteItem>();
			this.CultureVoteItems = new List<IntermissionVoteItem>();
			this.UsableMaps = new List<CustomGameUsableMap>();
			this._votesOfPlayers = new Dictionary<PlayerId, List<string>>();
			this.IsMapVoteEnabled = true;
			this.IsCultureVoteEnabled = true;
			this.IsDisableMapVoteOverride = false;
			this.IsDisableCultureVoteOverride = false;
			this.IsMapSelectedByAdmin = false;
		}

		// Token: 0x06002D05 RID: 11525 RVA: 0x000AD0E8 File Offset: 0x000AB2E8
		public void AddMapItem(string mapID)
		{
			if (!this.MapVoteItems.ContainsItem(mapID))
			{
				IntermissionVoteItem intermissionVoteItem = this.MapVoteItems.Add(mapID);
				MultiplayerIntermissionVotingManager.MapItemAddedDelegate onMapItemAdded = this.OnMapItemAdded;
				if (onMapItemAdded != null)
				{
					onMapItemAdded(intermissionVoteItem.Id);
				}
				this.SortVotesAndPickBest();
			}
		}

		// Token: 0x06002D06 RID: 11526 RVA: 0x000AD12D File Offset: 0x000AB32D
		public void AddUsableMap(CustomGameUsableMap usableMap)
		{
			this.UsableMaps.Add(usableMap);
		}

		// Token: 0x06002D07 RID: 11527 RVA: 0x000AD13C File Offset: 0x000AB33C
		public List<string> GetUsableMaps(string gameType)
		{
			List<string> list = new List<string>();
			for (int i = 0; i < this.UsableMaps.Count; i++)
			{
				if (this.UsableMaps[i].IsCompatibleWithAllGameTypes || this.UsableMaps[i].CompatibleGameTypes.Contains(gameType))
				{
					list.Add(this.UsableMaps[i].Map);
				}
			}
			return list;
		}

		// Token: 0x06002D08 RID: 11528 RVA: 0x000AD1AC File Offset: 0x000AB3AC
		public void AddCultureItem(string cultureID)
		{
			if (!this.CultureVoteItems.ContainsItem(cultureID))
			{
				IntermissionVoteItem intermissionVoteItem = this.CultureVoteItems.Add(cultureID);
				MultiplayerIntermissionVotingManager.CultureItemAddedDelegate onCultureItemAdded = this.OnCultureItemAdded;
				if (onCultureItemAdded != null)
				{
					onCultureItemAdded(intermissionVoteItem.Id);
				}
				this.SortVotesAndPickBest();
			}
		}

		// Token: 0x06002D09 RID: 11529 RVA: 0x000AD1F4 File Offset: 0x000AB3F4
		public void AddVote(PlayerId voterID, string itemID, int voteCount)
		{
			if (this.MapVoteItems.ContainsItem(itemID))
			{
				IntermissionVoteItem item = this.MapVoteItems.GetItem(itemID);
				item.IncreaseVoteCount(voteCount);
				MultiplayerIntermissionVotingManager.MapItemVoteCountChangedDelegate onMapItemVoteCountChanged = this.OnMapItemVoteCountChanged;
				if (onMapItemVoteCountChanged != null)
				{
					onMapItemVoteCountChanged(item.Index, item.VoteCount);
				}
			}
			else if (this.CultureVoteItems.ContainsItem(itemID))
			{
				IntermissionVoteItem item2 = this.CultureVoteItems.GetItem(itemID);
				item2.IncreaseVoteCount(voteCount);
				MultiplayerIntermissionVotingManager.CultureItemVoteCountChangedDelegate onCultureItemVoteCountChanged = this.OnCultureItemVoteCountChanged;
				if (onCultureItemVoteCountChanged != null)
				{
					onCultureItemVoteCountChanged(item2.Index, item2.VoteCount);
				}
			}
			else
			{
				Debug.FailedAssert("Item with ID does not exist.", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\MultiplayerIntermissionVotingManager.cs", "AddVote", 120);
			}
			if (!this._votesOfPlayers.ContainsKey(voterID))
			{
				this._votesOfPlayers.Add(voterID, new List<string>());
			}
			if (voteCount == 1)
			{
				this._votesOfPlayers[voterID].Add(itemID);
			}
			else if (voteCount == -1)
			{
				this._votesOfPlayers[voterID].Remove(itemID);
			}
			this.SortVotesAndPickBest();
		}

		// Token: 0x06002D0A RID: 11530 RVA: 0x000AD2ED File Offset: 0x000AB4ED
		public void SetVotesOfMap(int mapItemIndex, int voteCount)
		{
			this.MapVoteItems[mapItemIndex].SetVoteCount(voteCount);
			MultiplayerIntermissionVotingManager.MapItemVoteCountChangedDelegate onMapItemVoteCountChanged = this.OnMapItemVoteCountChanged;
			if (onMapItemVoteCountChanged == null)
			{
				return;
			}
			onMapItemVoteCountChanged(mapItemIndex, voteCount);
		}

		// Token: 0x06002D0B RID: 11531 RVA: 0x000AD313 File Offset: 0x000AB513
		public void SetVotesOfCulture(int cultureItemIndex, int voteCount)
		{
			this.CultureVoteItems[cultureItemIndex].SetVoteCount(voteCount);
			MultiplayerIntermissionVotingManager.CultureItemVoteCountChangedDelegate onCultureItemVoteCountChanged = this.OnCultureItemVoteCountChanged;
			if (onCultureItemVoteCountChanged == null)
			{
				return;
			}
			onCultureItemVoteCountChanged(cultureItemIndex, voteCount);
		}

		// Token: 0x06002D0C RID: 11532 RVA: 0x000AD33C File Offset: 0x000AB53C
		public void ClearVotes()
		{
			foreach (IntermissionVoteItem intermissionVoteItem in this.MapVoteItems)
			{
				intermissionVoteItem.SetVoteCount(0);
				MultiplayerIntermissionVotingManager.MapItemVoteCountChangedDelegate onMapItemVoteCountChanged = this.OnMapItemVoteCountChanged;
				if (onMapItemVoteCountChanged != null)
				{
					onMapItemVoteCountChanged(intermissionVoteItem.Index, intermissionVoteItem.VoteCount);
				}
			}
			foreach (IntermissionVoteItem intermissionVoteItem2 in this.CultureVoteItems)
			{
				intermissionVoteItem2.SetVoteCount(0);
				MultiplayerIntermissionVotingManager.CultureItemVoteCountChangedDelegate onCultureItemVoteCountChanged = this.OnCultureItemVoteCountChanged;
				if (onCultureItemVoteCountChanged != null)
				{
					onCultureItemVoteCountChanged(intermissionVoteItem2.Index, intermissionVoteItem2.VoteCount);
				}
			}
			this._votesOfPlayers.Clear();
		}

		// Token: 0x06002D0D RID: 11533 RVA: 0x000AD418 File Offset: 0x000AB618
		public void ClearItems()
		{
			this.MapVoteItems.Clear();
			this.CultureVoteItems.Clear();
			this._votesOfPlayers.Clear();
		}

		// Token: 0x06002D0E RID: 11534 RVA: 0x000AD43B File Offset: 0x000AB63B
		public bool IsCultureItem(string itemID)
		{
			return this.CultureVoteItems.ContainsItem(itemID);
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x000AD449 File Offset: 0x000AB649
		public bool IsMapItem(string itemID)
		{
			return this.MapVoteItems.ContainsItem(itemID);
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x000AD458 File Offset: 0x000AB658
		public void HandlePlayerDisconnect(PlayerId playerID)
		{
			if (this._votesOfPlayers.ContainsKey(playerID))
			{
				foreach (string text in this._votesOfPlayers[playerID].ToList<string>())
				{
					this.AddVote(playerID, text, -1);
				}
				this._votesOfPlayers.Remove(playerID);
			}
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x000AD4D4 File Offset: 0x000AB6D4
		public void SelectRandomCultures(MultiplayerOptions.MultiplayerOptionsAccessMode accessMode)
		{
			string[] array = new string[] { "khuzait", "aserai", "battania", "vlandia", "sturgia", "empire" };
			Random random = new Random();
			string text = array[random.Next(0, array.Length)];
			string text2 = array[random.Next(0, array.Length)];
			MultiplayerOptions.OptionType.CultureTeam1.SetValue(text, accessMode);
			MultiplayerOptions.OptionType.CultureTeam2.SetValue(text2, accessMode);
		}

		// Token: 0x06002D12 RID: 11538 RVA: 0x000AD54A File Offset: 0x000AB74A
		public bool IsPeerVotedForItem(NetworkCommunicator peer, string itemID)
		{
			return this._votesOfPlayers.ContainsKey(peer.VirtualPlayer.Id) && this._votesOfPlayers[peer.VirtualPlayer.Id].Contains(itemID);
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x000AD584 File Offset: 0x000AB784
		public void SortVotesAndPickBest()
		{
			if (GameNetwork.IsServer)
			{
				if (this.IsMapVoteEnabled)
				{
					List<IntermissionVoteItem> list = this.MapVoteItems.ToList<IntermissionVoteItem>();
					if (list.Count > 1)
					{
						list.Sort((IntermissionVoteItem m1, IntermissionVoteItem m2) => -m1.VoteCount.CompareTo(m2.VoteCount));
						string text = list[0].Id;
						if (list[0].VoteCount <= 0)
						{
							Random random = new Random();
							text = list[random.Next(0, list.Count)].Id;
						}
						MultiplayerOptions.OptionType.Map.SetValue(text, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					}
					else if (list.Count == 1)
					{
						MultiplayerOptions.OptionType.Map.SetValue(list[0].Id, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					}
				}
				if (this.IsCultureVoteEnabled)
				{
					List<IntermissionVoteItem> list2 = this.CultureVoteItems.ToList<IntermissionVoteItem>();
					if (list2.Count > 2)
					{
						list2.Sort((IntermissionVoteItem c1, IntermissionVoteItem c2) => -c1.VoteCount.CompareTo(c2.VoteCount));
						string id = list2[0].Id;
						string text2 = list2[1].Id;
						if (list2[0].VoteCount > 0)
						{
							if (10 * list2[0].VoteCount >= 7 * list2.Select<IntermissionVoteItem, int>((IntermissionVoteItem item) => item.VoteCount).Sum())
							{
								text2 = list2[0].Id;
							}
							MultiplayerOptions.OptionType.CultureTeam1.SetValue(id, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
							MultiplayerOptions.OptionType.CultureTeam2.SetValue(text2, MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
							return;
						}
						this.SelectRandomCultures(MultiplayerOptions.MultiplayerOptionsAccessMode.CurrentMapOptions);
					}
				}
			}
		}

		// Token: 0x040011C0 RID: 4544
		public const int MaxAllowedMapCount = 100;

		// Token: 0x040011C1 RID: 4545
		private static MultiplayerIntermissionVotingManager _instance;

		// Token: 0x040011C2 RID: 4546
		public bool IsAutomatedBattleSwitchingEnabled;

		// Token: 0x040011C3 RID: 4547
		public bool IsMapVoteEnabled;

		// Token: 0x040011C4 RID: 4548
		public bool IsCultureVoteEnabled;

		// Token: 0x040011C5 RID: 4549
		public bool IsDisableMapVoteOverride;

		// Token: 0x040011C6 RID: 4550
		public bool IsDisableCultureVoteOverride;

		// Token: 0x040011C7 RID: 4551
		public bool IsMapSelectedByAdmin;

		// Token: 0x040011C8 RID: 4552
		public string InitialGameType;

		// Token: 0x040011CC RID: 4556
		private readonly Dictionary<PlayerId, List<string>> _votesOfPlayers;

		// Token: 0x040011CD RID: 4557
		public MultiplayerIntermissionState CurrentVoteState;

		// Token: 0x020005F5 RID: 1525
		// (Invoke) Token: 0x06003FF2 RID: 16370
		public delegate void MapItemAddedDelegate(string mapId);

		// Token: 0x020005F6 RID: 1526
		// (Invoke) Token: 0x06003FF6 RID: 16374
		public delegate void CultureItemAddedDelegate(string cultureId);

		// Token: 0x020005F7 RID: 1527
		// (Invoke) Token: 0x06003FFA RID: 16378
		public delegate void MapItemVoteCountChangedDelegate(int mapItemIndex, int voteCount);

		// Token: 0x020005F8 RID: 1528
		// (Invoke) Token: 0x06003FFE RID: 16382
		public delegate void CultureItemVoteCountChangedDelegate(int cultureItemIndex, int voteCount);
	}
}
