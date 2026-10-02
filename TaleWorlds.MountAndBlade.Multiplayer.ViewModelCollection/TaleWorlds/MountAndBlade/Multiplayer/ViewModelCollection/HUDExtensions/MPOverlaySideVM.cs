using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000098 RID: 152
	public class MPOverlaySideVM : ViewModel
	{
		// Token: 0x06000F3D RID: 3901 RVA: 0x0002F583 File Offset: 0x0002D783
		public MPOverlaySideVM()
		{
			this.Players = new MBBindingList<MPOverlayPlayerVM>();
			this.StatHeaders = new MBBindingList<MPOverlayStatVM>();
		}

		// Token: 0x06000F3E RID: 3902 RVA: 0x0002F5B8 File Offset: 0x0002D7B8
		public void ApplyPlayers(List<MPOverlayPlayerVM> desired)
		{
			this._staleScratch.Clear();
			for (int i = 0; i < this.Players.Count; i++)
			{
				MPOverlayPlayerVM mpoverlayPlayerVM = this.Players[i];
				if (!desired.Contains(mpoverlayPlayerVM))
				{
					this._staleScratch.Add(mpoverlayPlayerVM);
				}
			}
			for (int j = 0; j < this._staleScratch.Count; j++)
			{
				this.Players.Remove(this._staleScratch[j]);
			}
			this._staleScratch.Clear();
			for (int k = 0; k < desired.Count; k++)
			{
				MPOverlayPlayerVM mpoverlayPlayerVM2 = desired[k];
				if (!this.Players.Contains(mpoverlayPlayerVM2))
				{
					this.Players.Add(mpoverlayPlayerVM2);
				}
			}
			this.Players.Sort(MPOverlaySideVM.PlayerComparer);
		}

		// Token: 0x06000F3F RID: 3903 RVA: 0x0002F687 File Offset: 0x0002D887
		public void SetFollowedPeer(MissionPeer followedPeer)
		{
			if (this._followedPeer == followedPeer)
			{
				return;
			}
			this._followedPeer = followedPeer;
			this.FollowedPlayerToken++;
		}

		// Token: 0x06000F40 RID: 3904 RVA: 0x0002F6A8 File Offset: 0x0002D8A8
		public void RefreshStatHeaders(MissionScoreboardComponent.ScoreboardHeader[] headers)
		{
			if (headers == null)
			{
				return;
			}
			if (!this.HasHeaderShapeChanged(headers))
			{
				return;
			}
			this.StatHeaders.Clear();
			this._headerIds.Clear();
			foreach (MissionScoreboardComponent.ScoreboardHeader scoreboardHeader in headers)
			{
				if (!string.IsNullOrEmpty(scoreboardHeader.Id) && !MPOverlayPlayerVM.IsExcludedHeaderId(scoreboardHeader.Id))
				{
					Collection<MPOverlayStatVM> statHeaders = this.StatHeaders;
					string id = scoreboardHeader.Id;
					TextObject name = scoreboardHeader.Name;
					statHeaders.Add(new MPOverlayStatVM(id, ((name != null) ? name.ToString() : null) ?? string.Empty, string.Empty));
					this._headerIds.Add(scoreboardHeader.Id);
				}
			}
		}

		// Token: 0x06000F41 RID: 3905 RVA: 0x0002F750 File Offset: 0x0002D950
		private bool HasHeaderShapeChanged(MissionScoreboardComponent.ScoreboardHeader[] headers)
		{
			int num = 0;
			foreach (MissionScoreboardComponent.ScoreboardHeader scoreboardHeader in headers)
			{
				if (!string.IsNullOrEmpty(scoreboardHeader.Id) && !MPOverlayPlayerVM.IsExcludedHeaderId(scoreboardHeader.Id))
				{
					if (num >= this._headerIds.Count || this._headerIds[num] != scoreboardHeader.Id)
					{
						return true;
					}
					num++;
				}
			}
			return num != this._headerIds.Count;
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x0002F7CD File Offset: 0x0002D9CD
		// (set) Token: 0x06000F43 RID: 3907 RVA: 0x0002F7D5 File Offset: 0x0002D9D5
		[DataSourceProperty]
		public MBBindingList<MPOverlayPlayerVM> Players
		{
			get
			{
				return this._players;
			}
			set
			{
				if (value != this._players)
				{
					this._players = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPOverlayPlayerVM>>(value, "Players");
				}
			}
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06000F44 RID: 3908 RVA: 0x0002F7F3 File Offset: 0x0002D9F3
		// (set) Token: 0x06000F45 RID: 3909 RVA: 0x0002F7FB File Offset: 0x0002D9FB
		[DataSourceProperty]
		public MBBindingList<MPOverlayStatVM> StatHeaders
		{
			get
			{
				return this._statHeaders;
			}
			set
			{
				if (value != this._statHeaders)
				{
					this._statHeaders = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPOverlayStatVM>>(value, "StatHeaders");
				}
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06000F46 RID: 3910 RVA: 0x0002F819 File Offset: 0x0002DA19
		// (set) Token: 0x06000F47 RID: 3911 RVA: 0x0002F821 File Offset: 0x0002DA21
		[DataSourceProperty]
		public string OverflowText
		{
			get
			{
				return this._overflowText;
			}
			set
			{
				if (value != this._overflowText)
				{
					this._overflowText = value;
					base.OnPropertyChangedWithValue<string>(value, "OverflowText");
				}
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06000F48 RID: 3912 RVA: 0x0002F844 File Offset: 0x0002DA44
		// (set) Token: 0x06000F49 RID: 3913 RVA: 0x0002F84C File Offset: 0x0002DA4C
		[DataSourceProperty]
		public int FollowedPlayerToken
		{
			get
			{
				return this._followedPlayerToken;
			}
			set
			{
				if (value != this._followedPlayerToken)
				{
					this._followedPlayerToken = value;
					base.OnPropertyChangedWithValue(value, "FollowedPlayerToken");
				}
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0002F86A File Offset: 0x0002DA6A
		// (set) Token: 0x06000F4B RID: 3915 RVA: 0x0002F872 File Offset: 0x0002DA72
		[DataSourceProperty]
		public bool ShowOverflow
		{
			get
			{
				return this._showOverflow;
			}
			set
			{
				if (value != this._showOverflow)
				{
					this._showOverflow = value;
					base.OnPropertyChangedWithValue(value, "ShowOverflow");
				}
			}
		}

		// Token: 0x04000714 RID: 1812
		private static readonly MPOverlaySideVM.OverlayPlayerComparer PlayerComparer = new MPOverlaySideVM.OverlayPlayerComparer();

		// Token: 0x04000715 RID: 1813
		private readonly List<string> _headerIds = new List<string>();

		// Token: 0x04000716 RID: 1814
		private readonly List<MPOverlayPlayerVM> _staleScratch = new List<MPOverlayPlayerVM>();

		// Token: 0x04000717 RID: 1815
		private MissionPeer _followedPeer;

		// Token: 0x04000718 RID: 1816
		private MBBindingList<MPOverlayPlayerVM> _players;

		// Token: 0x04000719 RID: 1817
		private MBBindingList<MPOverlayStatVM> _statHeaders;

		// Token: 0x0400071A RID: 1818
		private string _overflowText;

		// Token: 0x0400071B RID: 1819
		private bool _showOverflow;

		// Token: 0x0400071C RID: 1820
		private int _followedPlayerToken;

		// Token: 0x0200018A RID: 394
		private class OverlayPlayerComparer : IComparer<MPOverlayPlayerVM>
		{
			// Token: 0x06001373 RID: 4979 RVA: 0x0003E334 File Offset: 0x0003C534
			public int Compare(MPOverlayPlayerVM left, MPOverlayPlayerVM right)
			{
				int? num;
				if (left == null)
				{
					num = null;
				}
				else
				{
					MissionPeer peer = left.Peer;
					num = ((peer != null) ? new int?(peer.Score) : null);
				}
				int num2 = num ?? 0;
				int? num3;
				if (right == null)
				{
					num3 = null;
				}
				else
				{
					MissionPeer peer2 = right.Peer;
					num3 = ((peer2 != null) ? new int?(peer2.Score) : null);
				}
				int num4 = num3 ?? 0;
				if (num2 != num4)
				{
					if (num2 <= num4)
					{
						return 1;
					}
					return -1;
				}
				else
				{
					int? num5;
					if (left == null)
					{
						num5 = null;
					}
					else
					{
						MissionPeer peer3 = left.Peer;
						if (peer3 == null)
						{
							num5 = null;
						}
						else
						{
							VirtualPlayer peer4 = peer3.Peer;
							num5 = ((peer4 != null) ? new int?(peer4.Index) : null);
						}
					}
					int num6 = num5 ?? int.MaxValue;
					int? num7;
					if (right == null)
					{
						num7 = null;
					}
					else
					{
						MissionPeer peer5 = right.Peer;
						if (peer5 == null)
						{
							num7 = null;
						}
						else
						{
							VirtualPlayer peer6 = peer5.Peer;
							num7 = ((peer6 != null) ? new int?(peer6.Index) : null);
						}
					}
					int num8 = num7 ?? int.MaxValue;
					if (num6 == num8)
					{
						return 0;
					}
					if (num6 >= num8)
					{
						return 1;
					}
					return -1;
				}
			}
		}
	}
}
