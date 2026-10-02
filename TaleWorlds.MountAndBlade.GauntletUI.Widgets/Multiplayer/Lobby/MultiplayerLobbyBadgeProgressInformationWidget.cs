using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby
{
	// Token: 0x0200009F RID: 159
	public class MultiplayerLobbyBadgeProgressInformationWidget : Widget
	{
		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000898 RID: 2200 RVA: 0x00018D76 File Offset: 0x00016F76
		// (set) Token: 0x06000899 RID: 2201 RVA: 0x00018D7E File Offset: 0x00016F7E
		public float CenterBadgeSize { get; set; } = 200f;

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x0600089A RID: 2202 RVA: 0x00018D87 File Offset: 0x00016F87
		// (set) Token: 0x0600089B RID: 2203 RVA: 0x00018D8F File Offset: 0x00016F8F
		public float OuterBadgeBaseSize { get; set; } = 175f;

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x0600089C RID: 2204 RVA: 0x00018D98 File Offset: 0x00016F98
		// (set) Token: 0x0600089D RID: 2205 RVA: 0x00018DA0 File Offset: 0x00016FA0
		public float SizeDecayFromCenterPerElement { get; set; } = 25f;

		// Token: 0x0600089E RID: 2206 RVA: 0x00018DA9 File Offset: 0x00016FA9
		public MultiplayerLobbyBadgeProgressInformationWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600089F RID: 2207 RVA: 0x00018DD3 File Offset: 0x00016FD3
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this.ActiveBadgesList != null)
			{
				this.ArrangeChildrenSizes();
			}
		}

		// Token: 0x060008A0 RID: 2208 RVA: 0x00018DEC File Offset: 0x00016FEC
		private void ArrangeChildrenSizes()
		{
			this.ActiveBadgesList.IsVisible = this.ShownBadgeCount > 0;
			int centerIndex = this.ShownBadgeCount / 2;
			int currentIndex = 0;
			this.ActiveBadgesList.ApplyActionToAllChildrenRecursive(delegate(Widget widget)
			{
				MultiplayerPlayerBadgeVisualWidget multiplayerPlayerBadgeVisualWidget;
				if ((multiplayerPlayerBadgeVisualWidget = widget as MultiplayerPlayerBadgeVisualWidget) != null)
				{
					float num = this.CenterBadgeSize;
					if (currentIndex != centerIndex)
					{
						float num2 = (float)MathF.Abs(currentIndex - centerIndex);
						num = this.OuterBadgeBaseSize - this.SizeDecayFromCenterPerElement * num2;
					}
					multiplayerPlayerBadgeVisualWidget.SetForcedSize(num, num);
					int currentIndex2 = currentIndex;
					currentIndex = currentIndex2 + 1;
				}
			});
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00018E46 File Offset: 0x00017046
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x00018E4E File Offset: 0x0001704E
		[Editor(false)]
		public int ShownBadgeCount
		{
			get
			{
				return this._shownBadgeCount;
			}
			set
			{
				if (value != this._shownBadgeCount)
				{
					this._shownBadgeCount = value;
					base.OnPropertyChanged(value, "ShownBadgeCount");
				}
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x060008A3 RID: 2211 RVA: 0x00018E6C File Offset: 0x0001706C
		// (set) Token: 0x060008A4 RID: 2212 RVA: 0x00018E74 File Offset: 0x00017074
		[Editor(false)]
		public ListPanel ActiveBadgesList
		{
			get
			{
				return this._activeBadgesList;
			}
			set
			{
				if (value != this._activeBadgesList)
				{
					this._activeBadgesList = value;
					base.OnPropertyChanged<ListPanel>(value, "ActiveBadgesList");
				}
			}
		}

		// Token: 0x040003D8 RID: 984
		private int _shownBadgeCount;

		// Token: 0x040003D9 RID: 985
		private ListPanel _activeBadgesList;
	}
}
