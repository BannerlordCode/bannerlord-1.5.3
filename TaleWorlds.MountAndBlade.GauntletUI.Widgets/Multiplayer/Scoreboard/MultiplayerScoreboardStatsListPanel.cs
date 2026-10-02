using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000095 RID: 149
	public class MultiplayerScoreboardStatsListPanel : ListPanel
	{
		// Token: 0x06000832 RID: 2098 RVA: 0x00017D50 File Offset: 0x00015F50
		public MultiplayerScoreboardStatsListPanel(UIContext context)
			: base(context)
		{
			this._nameColumnItemDescription = new ContainerItemDescription
			{
				WidgetId = "name"
			};
			this._scoreColumnItemDescription = new ContainerItemDescription
			{
				WidgetId = "score"
			};
			this._soldiersColumnItemDescription = new ContainerItemDescription
			{
				WidgetId = "soldiers"
			};
		}

		// Token: 0x06000833 RID: 2099 RVA: 0x00017DC7 File Offset: 0x00015FC7
		private void NameColumnWidthRatioUpdated()
		{
			this._nameColumnItemDescription.WidthStretchRatio = this.NameColumnWidthRatio;
			base.AddItemDescription(this._nameColumnItemDescription);
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x06000834 RID: 2100 RVA: 0x00017DEC File Offset: 0x00015FEC
		private void ScoreColumnWidthRatioUpdated()
		{
			this._scoreColumnItemDescription.WidthStretchRatio = this.ScoreColumnWidthRatio;
			base.AddItemDescription(this._scoreColumnItemDescription);
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x06000835 RID: 2101 RVA: 0x00017E11 File Offset: 0x00016011
		private void SoldiersColumnWidthRatioUpdated()
		{
			this._soldiersColumnItemDescription.WidthStretchRatio = this.SoldiersColumnWidthRatio;
			base.AddItemDescription(this._soldiersColumnItemDescription);
			base.SetMeasureAndLayoutDirty();
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000836 RID: 2102 RVA: 0x00017E36 File Offset: 0x00016036
		// (set) Token: 0x06000837 RID: 2103 RVA: 0x00017E3E File Offset: 0x0001603E
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
					this.NameColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000838 RID: 2104 RVA: 0x00017E62 File Offset: 0x00016062
		// (set) Token: 0x06000839 RID: 2105 RVA: 0x00017E6A File Offset: 0x0001606A
		public float ScoreColumnWidthRatio
		{
			get
			{
				return this._scoreColumnWidthRatio;
			}
			set
			{
				if (value != this._scoreColumnWidthRatio)
				{
					this._scoreColumnWidthRatio = value;
					base.OnPropertyChanged(value, "ScoreColumnWidthRatio");
					this.ScoreColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x0600083A RID: 2106 RVA: 0x00017E8E File Offset: 0x0001608E
		// (set) Token: 0x0600083B RID: 2107 RVA: 0x00017E96 File Offset: 0x00016096
		public float SoldiersColumnWidthRatio
		{
			get
			{
				return this._soldiersColumnWidthRatio;
			}
			set
			{
				if (value != this._soldiersColumnWidthRatio)
				{
					this._soldiersColumnWidthRatio = value;
					base.OnPropertyChanged(value, "SoldiersColumnWidthRatio");
					this.SoldiersColumnWidthRatioUpdated();
				}
			}
		}

		// Token: 0x040003A5 RID: 933
		private ContainerItemDescription _nameColumnItemDescription;

		// Token: 0x040003A6 RID: 934
		private ContainerItemDescription _scoreColumnItemDescription;

		// Token: 0x040003A7 RID: 935
		private ContainerItemDescription _soldiersColumnItemDescription;

		// Token: 0x040003A8 RID: 936
		private const string _nameColumnWidgetID = "name";

		// Token: 0x040003A9 RID: 937
		private const string _scoreColumnWidgetID = "score";

		// Token: 0x040003AA RID: 938
		private const string _soldiersColumnWidgetID = "soldiers";

		// Token: 0x040003AB RID: 939
		private float _nameColumnWidthRatio = 1f;

		// Token: 0x040003AC RID: 940
		private float _scoreColumnWidthRatio = 1f;

		// Token: 0x040003AD RID: 941
		private float _soldiersColumnWidthRatio = 1f;
	}
}
