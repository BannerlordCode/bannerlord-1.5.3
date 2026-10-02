using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000094 RID: 148
	public class MultiplayerScoreboardSideWidget : Widget
	{
		// Token: 0x06000825 RID: 2085 RVA: 0x00017BF4 File Offset: 0x00015DF4
		public MultiplayerScoreboardSideWidget(UIContext context)
			: base(context)
		{
			this._nameColumnItemDescription = new ContainerItemDescription();
			this._nameColumnItemDescription.WidgetIndex = 3;
		}

		// Token: 0x06000826 RID: 2086 RVA: 0x00017C1F File Offset: 0x00015E1F
		private void AvatarColumnWidthRatioUpdated()
		{
			if (this.TitlesListPanel == null)
			{
				return;
			}
			this._nameColumnItemDescription.WidthStretchRatio = this.NameColumnWidthRatio;
			this.TitlesListPanel.AddItemDescription(this._nameColumnItemDescription);
		}

		// Token: 0x06000827 RID: 2087 RVA: 0x00017C4C File Offset: 0x00015E4C
		private void UpdateBackgroundColors()
		{
			if (string.IsNullOrEmpty(this.CultureId))
			{
				return;
			}
			base.Color = this.CultureColor;
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000828 RID: 2088 RVA: 0x00017C68 File Offset: 0x00015E68
		// (set) Token: 0x06000829 RID: 2089 RVA: 0x00017C70 File Offset: 0x00015E70
		public Color CultureColor
		{
			get
			{
				return this._cultureColor;
			}
			set
			{
				if (value != this._cultureColor)
				{
					this._cultureColor = value;
					base.OnPropertyChanged(value, "CultureColor");
					this.UpdateBackgroundColors();
				}
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x0600082A RID: 2090 RVA: 0x00017C99 File Offset: 0x00015E99
		// (set) Token: 0x0600082B RID: 2091 RVA: 0x00017CA1 File Offset: 0x00015EA1
		public string CultureId
		{
			get
			{
				return this._cultureId;
			}
			set
			{
				if (value != this._cultureId)
				{
					this._cultureId = value;
					base.OnPropertyChanged<string>(value, "CultureId");
					this.UpdateBackgroundColors();
				}
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x0600082C RID: 2092 RVA: 0x00017CCA File Offset: 0x00015ECA
		// (set) Token: 0x0600082D RID: 2093 RVA: 0x00017CD2 File Offset: 0x00015ED2
		public bool UseSecondary
		{
			get
			{
				return this._useSecondary;
			}
			set
			{
				if (value != this._useSecondary)
				{
					this._useSecondary = value;
					base.OnPropertyChanged(value, "UseSecondary");
					this.UpdateBackgroundColors();
				}
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x0600082E RID: 2094 RVA: 0x00017CF6 File Offset: 0x00015EF6
		// (set) Token: 0x0600082F RID: 2095 RVA: 0x00017CFE File Offset: 0x00015EFE
		public float NameColumnWidthRatio
		{
			get
			{
				return this._nameColumnWidthRatio;
			}
			set
			{
				if (value != this._nameColumnWidthRatio)
				{
					this._nameColumnWidthRatio = value;
					base.OnPropertyChanged(value, "NameColumnWidthRatio");
					this.AvatarColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000830 RID: 2096 RVA: 0x00017D22 File Offset: 0x00015F22
		// (set) Token: 0x06000831 RID: 2097 RVA: 0x00017D2A File Offset: 0x00015F2A
		public ListPanel TitlesListPanel
		{
			get
			{
				return this._titlesListPanel;
			}
			set
			{
				if (value != this._titlesListPanel)
				{
					this._titlesListPanel = value;
					base.OnPropertyChanged<ListPanel>(value, "TitlesListPanel");
					this.AvatarColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x0400039F RID: 927
		private ContainerItemDescription _nameColumnItemDescription;

		// Token: 0x040003A0 RID: 928
		private float _nameColumnWidthRatio = 1f;

		// Token: 0x040003A1 RID: 929
		private ListPanel _titlesListPanel;

		// Token: 0x040003A2 RID: 930
		private string _cultureId;

		// Token: 0x040003A3 RID: 931
		private Color _cultureColor;

		// Token: 0x040003A4 RID: 932
		private bool _useSecondary;
	}
}
