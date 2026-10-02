using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000093 RID: 147
	public class MultiplayerScoreboardScreenWidget : Widget
	{
		// Token: 0x0600081B RID: 2075 RVA: 0x00017B1F File Offset: 0x00015D1F
		public MultiplayerScoreboardScreenWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x00017B28 File Offset: 0x00015D28
		private void UpdateSidesList()
		{
			if (this.SidesList == null)
			{
				return;
			}
			base.SuggestedWidth = (float)(this.IsSingleSide ? this.SingleColumnedWidth : this.DoubleColumnedWidth);
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x0600081D RID: 2077 RVA: 0x00017B50 File Offset: 0x00015D50
		// (set) Token: 0x0600081E RID: 2078 RVA: 0x00017B58 File Offset: 0x00015D58
		[DataSourceProperty]
		public bool IsSingleSide
		{
			get
			{
				return this._isSingleSide;
			}
			set
			{
				if (value != this._isSingleSide)
				{
					this._isSingleSide = value;
					base.OnPropertyChanged(value, "IsSingleSide");
					this.UpdateSidesList();
				}
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x0600081F RID: 2079 RVA: 0x00017B7C File Offset: 0x00015D7C
		// (set) Token: 0x06000820 RID: 2080 RVA: 0x00017B84 File Offset: 0x00015D84
		[DataSourceProperty]
		public int SingleColumnedWidth
		{
			get
			{
				return this._singleColumnedWidth;
			}
			set
			{
				if (value != this._singleColumnedWidth)
				{
					this._singleColumnedWidth = value;
					base.OnPropertyChanged(value, "SingleColumnedWidth");
				}
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000821 RID: 2081 RVA: 0x00017BA2 File Offset: 0x00015DA2
		// (set) Token: 0x06000822 RID: 2082 RVA: 0x00017BAA File Offset: 0x00015DAA
		[DataSourceProperty]
		public int DoubleColumnedWidth
		{
			get
			{
				return this._doubleColumnedWidth;
			}
			set
			{
				if (value != this._doubleColumnedWidth)
				{
					this._doubleColumnedWidth = value;
					base.OnPropertyChanged(value, "DoubleColumnedWidth");
				}
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000823 RID: 2083 RVA: 0x00017BC8 File Offset: 0x00015DC8
		// (set) Token: 0x06000824 RID: 2084 RVA: 0x00017BD0 File Offset: 0x00015DD0
		[DataSourceProperty]
		public ListPanel SidesList
		{
			get
			{
				return this._sidesList;
			}
			set
			{
				if (value != this._sidesList)
				{
					this._sidesList = value;
					base.OnPropertyChanged<ListPanel>(value, "SidesList");
					this.UpdateSidesList();
				}
			}
		}

		// Token: 0x0400039B RID: 923
		private bool _isSingleSide;

		// Token: 0x0400039C RID: 924
		private int _singleColumnedWidth;

		// Token: 0x0400039D RID: 925
		private int _doubleColumnedWidth;

		// Token: 0x0400039E RID: 926
		private ListPanel _sidesList;
	}
}
