using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x02000089 RID: 137
	public class MultiplayerEndOfRoundPanelBrushWidget : BrushWidget
	{
		// Token: 0x060007BB RID: 1979 RVA: 0x00016B0F File Offset: 0x00014D0F
		public MultiplayerEndOfRoundPanelBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007BC RID: 1980 RVA: 0x00016B18 File Offset: 0x00014D18
		private void IsShownUpdated()
		{
			if (this.IsShown)
			{
				string text = (this.IsRoundWinner ? "Victory" : "Defeat");
				base.EventFired(text, Array.Empty<object>());
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00016B4E File Offset: 0x00014D4E
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x00016B56 File Offset: 0x00014D56
		[DataSourceProperty]
		public bool IsShown
		{
			get
			{
				return this._isShown;
			}
			set
			{
				if (value != this._isShown)
				{
					this._isShown = value;
					base.OnPropertyChanged(value, "IsShown");
					this.IsShownUpdated();
				}
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00016B7A File Offset: 0x00014D7A
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x00016B82 File Offset: 0x00014D82
		[DataSourceProperty]
		public bool IsRoundWinner
		{
			get
			{
				return this._isRoundWinner;
			}
			set
			{
				if (value != this._isRoundWinner)
				{
					this._isRoundWinner = value;
					base.OnPropertyChanged(value, "IsRoundWinner");
				}
			}
		}

		// Token: 0x04000363 RID: 867
		private bool _isShown;

		// Token: 0x04000364 RID: 868
		private bool _isRoundWinner;
	}
}
