using System;
using System.Linq;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.FactionBanVote
{
	// Token: 0x020000A2 RID: 162
	public class MultiplayerFactionBanVM : ViewModel
	{
		// Token: 0x06000FC7 RID: 4039 RVA: 0x00031400 File Offset: 0x0002F600
		public MultiplayerFactionBanVM()
		{
			this.SelectTitle = "SELECT FACTION";
			this.BanTitle = "BAN FACTION";
			this._banList = new MBBindingList<MultiplayerFactionBanVoteVM>();
			foreach (BasicCultureObject basicCultureObject in MultiplayerClassDivisions.AvailableCultures)
			{
				this._banList.Add(new MultiplayerFactionBanVoteVM(basicCultureObject, new Action<MultiplayerFactionBanVoteVM>(this.OnBanFaction)));
			}
			this._selectList = new MBBindingList<MultiplayerFactionBanVoteVM>();
			foreach (BasicCultureObject basicCultureObject2 in MultiplayerClassDivisions.AvailableCultures)
			{
				this._selectList.Add(new MultiplayerFactionBanVoteVM(basicCultureObject2, new Action<MultiplayerFactionBanVoteVM>(this.OnSelectFaction)));
			}
			foreach (MultiplayerFactionBanVoteVM multiplayerFactionBanVoteVM in this._selectList)
			{
				if (multiplayerFactionBanVoteVM.IsEnabled)
				{
					multiplayerFactionBanVoteVM.IsSelected = true;
					break;
				}
			}
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x00031530 File Offset: 0x0002F730
		public override void RefreshValues()
		{
			base.RefreshValues();
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00031538 File Offset: 0x0002F738
		public override void OnFinalize()
		{
			base.OnFinalize();
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00031540 File Offset: 0x0002F740
		private void OnSelectFaction(MultiplayerFactionBanVoteVM vote)
		{
			MultiplayerFactionBanVM.VoteForCulture(CultureVoteTypes.Select, vote.Culture);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x0003154E File Offset: 0x0002F74E
		private void OnBanFaction(MultiplayerFactionBanVoteVM vote)
		{
			MultiplayerFactionBanVM.VoteForCulture(CultureVoteTypes.Ban, vote.Culture);
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x0003155C File Offset: 0x0002F75C
		private void Refresh()
		{
			foreach (MultiplayerFactionBanVoteVM multiplayerFactionBanVoteVM in this._banList)
			{
				multiplayerFactionBanVoteVM.IsSelected = false;
				multiplayerFactionBanVoteVM.IsEnabled = false;
			}
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			bool flag = false;
			foreach (MultiplayerFactionBanVoteVM multiplayerFactionBanVoteVM2 in this._selectList)
			{
				if (flag)
				{
					multiplayerFactionBanVoteVM2.IsSelected = true;
					flag = false;
					break;
				}
				if (component.VotedForBan == multiplayerFactionBanVoteVM2.Culture)
				{
					multiplayerFactionBanVoteVM2.IsEnabled = false;
					if (multiplayerFactionBanVoteVM2.IsSelected)
					{
						multiplayerFactionBanVoteVM2.IsSelected = false;
						flag = true;
					}
				}
			}
			if (flag)
			{
				MultiplayerFactionBanVoteVM multiplayerFactionBanVoteVM3 = this._selectList.FirstOrDefault<MultiplayerFactionBanVoteVM>((MultiplayerFactionBanVoteVM s) => s.IsEnabled);
				if (multiplayerFactionBanVoteVM3 != null)
				{
					multiplayerFactionBanVoteVM3.IsSelected = true;
				}
			}
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x00031660 File Offset: 0x0002F860
		private static void VoteForCulture(CultureVoteTypes voteType, BasicCultureObject culture)
		{
			MissionPeer component = GameNetwork.MyPeer.GetComponent<MissionPeer>();
			if (GameNetwork.IsServer)
			{
				component.HandleVoteChange(voteType, culture);
				return;
			}
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new CultureVoteClient(voteType, culture));
				GameNetwork.EndModuleEventAsClient();
			}
		}

		// Token: 0x17000535 RID: 1333
		// (get) Token: 0x06000FCE RID: 4046 RVA: 0x000316A5 File Offset: 0x0002F8A5
		// (set) Token: 0x06000FCF RID: 4047 RVA: 0x000316AD File Offset: 0x0002F8AD
		[DataSourceProperty]
		public MBBindingList<MultiplayerFactionBanVoteVM> SelectList
		{
			get
			{
				return this._selectList;
			}
			set
			{
				if (value != this._selectList)
				{
					this._selectList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MultiplayerFactionBanVoteVM>>(value, "SelectList");
				}
			}
		}

		// Token: 0x17000536 RID: 1334
		// (get) Token: 0x06000FD0 RID: 4048 RVA: 0x000316CB File Offset: 0x0002F8CB
		// (set) Token: 0x06000FD1 RID: 4049 RVA: 0x000316D3 File Offset: 0x0002F8D3
		[DataSourceProperty]
		public MBBindingList<MultiplayerFactionBanVoteVM> BanList
		{
			get
			{
				return this._banList;
			}
			set
			{
				if (value != this._banList)
				{
					this._banList = value;
					base.OnPropertyChangedWithValue<MBBindingList<MultiplayerFactionBanVoteVM>>(value, "BanList");
				}
			}
		}

		// Token: 0x17000537 RID: 1335
		// (get) Token: 0x06000FD2 RID: 4050 RVA: 0x000316F1 File Offset: 0x0002F8F1
		// (set) Token: 0x06000FD3 RID: 4051 RVA: 0x000316F9 File Offset: 0x0002F8F9
		[DataSourceProperty]
		public string SelectTitle
		{
			get
			{
				return this._selectTitle;
			}
			set
			{
				if (value != this._selectTitle)
				{
					this._selectTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "SelectTitle");
				}
			}
		}

		// Token: 0x17000538 RID: 1336
		// (get) Token: 0x06000FD4 RID: 4052 RVA: 0x0003171C File Offset: 0x0002F91C
		// (set) Token: 0x06000FD5 RID: 4053 RVA: 0x00031724 File Offset: 0x0002F924
		[DataSourceProperty]
		public string BanTitle
		{
			get
			{
				return this._banTitle;
			}
			set
			{
				if (value != this._banTitle)
				{
					this._banTitle = value;
					base.OnPropertyChangedWithValue<string>(value, "BanTitle");
				}
			}
		}

		// Token: 0x0400075B RID: 1883
		private MBBindingList<MultiplayerFactionBanVoteVM> _banList;

		// Token: 0x0400075C RID: 1884
		private MBBindingList<MultiplayerFactionBanVoteVM> _selectList;

		// Token: 0x0400075D RID: 1885
		private string _selectTitle;

		// Token: 0x0400075E RID: 1886
		private string _banTitle;
	}
}
