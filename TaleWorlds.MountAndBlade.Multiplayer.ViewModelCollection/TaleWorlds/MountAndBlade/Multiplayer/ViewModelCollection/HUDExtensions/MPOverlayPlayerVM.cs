using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection.HUDExtensions
{
	// Token: 0x02000097 RID: 151
	public class MPOverlayPlayerVM : MPPlayerVM
	{
		// Token: 0x06000F35 RID: 3893 RVA: 0x0002F35B File Offset: 0x0002D55B
		public static bool IsExcludedHeaderId(string headerId)
		{
			return MPOverlayPlayerVM.ExcludedHeaderIds.Contains(headerId);
		}

		// Token: 0x06000F36 RID: 3894 RVA: 0x0002F368 File Offset: 0x0002D568
		public MPOverlayPlayerVM(MissionPeer peer, Action<MPOverlayPlayerVM> onSelected)
			: base(peer)
		{
			this._onSelected = onSelected;
			this.Stats = new MBBindingList<MPOverlayStatVM>();
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x0002F38E File Offset: 0x0002D58E
		public override void ExecuteSelectPlayer()
		{
			Action<MPOverlayPlayerVM> onSelected = this._onSelected;
			if (onSelected == null)
			{
				return;
			}
			onSelected(this);
		}

		// Token: 0x06000F38 RID: 3896 RVA: 0x0002F3A4 File Offset: 0x0002D5A4
		public void RebuildStats(MissionScoreboardComponent.ScoreboardHeader[] headers)
		{
			this.Stats.Clear();
			this._statIds.Clear();
			if (headers == null)
			{
				return;
			}
			foreach (MissionScoreboardComponent.ScoreboardHeader scoreboardHeader in headers)
			{
				if (!string.IsNullOrEmpty(scoreboardHeader.Id) && !MPOverlayPlayerVM.IsExcludedHeaderId(scoreboardHeader.Id))
				{
					Collection<MPOverlayStatVM> stats = this.Stats;
					string id = scoreboardHeader.Id;
					TextObject name = scoreboardHeader.Name;
					stats.Add(new MPOverlayStatVM(id, ((name != null) ? name.ToString() : null) ?? string.Empty, scoreboardHeader.GetValueOf(base.Peer)));
					this._statIds.Add(scoreboardHeader.Id);
				}
			}
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x0002F44C File Offset: 0x0002D64C
		public void RefreshStats(MissionScoreboardComponent.ScoreboardHeader[] headers)
		{
			if (headers == null || base.Peer == null)
			{
				return;
			}
			if (this.Stats.Count != this._statIds.Count)
			{
				this.RebuildStats(headers);
				return;
			}
			int num = 0;
			foreach (MissionScoreboardComponent.ScoreboardHeader scoreboardHeader in headers)
			{
				if (!string.IsNullOrEmpty(scoreboardHeader.Id) && !MPOverlayPlayerVM.IsExcludedHeaderId(scoreboardHeader.Id))
				{
					if (num >= this.Stats.Count || this._statIds[num] != scoreboardHeader.Id)
					{
						this.RebuildStats(headers);
						return;
					}
					this.Stats[num].Refresh(scoreboardHeader.GetValueOf(base.Peer));
					num++;
				}
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06000F3A RID: 3898 RVA: 0x0002F508 File Offset: 0x0002D708
		// (set) Token: 0x06000F3B RID: 3899 RVA: 0x0002F510 File Offset: 0x0002D710
		[DataSourceProperty]
		public MBBindingList<MPOverlayStatVM> Stats
		{
			get
			{
				return this._stats;
			}
			set
			{
				if (value != this._stats)
				{
					this._stats = value;
					base.OnPropertyChangedWithValue<MBBindingList<MPOverlayStatVM>>(value, "Stats");
				}
			}
		}

		// Token: 0x04000710 RID: 1808
		private readonly Action<MPOverlayPlayerVM> _onSelected;

		// Token: 0x04000711 RID: 1809
		private static readonly HashSet<string> ExcludedHeaderIds = new HashSet<string> { "avatar", "badge", "name", "score", "ping" };

		// Token: 0x04000712 RID: 1810
		private readonly List<string> _statIds = new List<string>();

		// Token: 0x04000713 RID: 1811
		private MBBindingList<MPOverlayStatVM> _stats;
	}
}
