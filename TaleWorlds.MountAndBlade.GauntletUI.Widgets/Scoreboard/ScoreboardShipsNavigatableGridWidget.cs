using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Scoreboard
{
	// Token: 0x02000058 RID: 88
	public class ScoreboardShipsNavigatableGridWidget : NavigatableGridWidget
	{
		// Token: 0x060004E9 RID: 1257 RVA: 0x0000F522 File Offset: 0x0000D722
		public ScoreboardShipsNavigatableGridWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0000F52C File Offset: 0x0000D72C
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			ScrollablePanel parentPanel = base.ParentPanel;
			bool flag;
			if (parentPanel == null)
			{
				flag = false;
			}
			else
			{
				ScrollbarWidget activeScrollbar = parentPanel.ActiveScrollbar;
				bool? flag2 = ((activeScrollbar != null) ? new bool?(activeScrollbar.IsVisible) : null);
				bool flag3 = true;
				flag = (flag2.GetValueOrDefault() == flag3) & (flag2 != null);
			}
			if (flag)
			{
				base.HorizontalAlignment = this.OverflowHorizontalAlignment;
				return;
			}
			base.HorizontalAlignment = this.RegularHorizontalAlignment;
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x0000F59B File Offset: 0x0000D79B
		// (set) Token: 0x060004EC RID: 1260 RVA: 0x0000F5A4 File Offset: 0x0000D7A4
		[Editor(false)]
		public HorizontalAlignment RegularHorizontalAlignment
		{
			get
			{
				return this._regularHorizontalAlignment;
			}
			set
			{
				if (this._regularHorizontalAlignment != value)
				{
					this._regularHorizontalAlignment = value;
					switch (value)
					{
					case HorizontalAlignment.Left:
						base.OnPropertyChanged<string>("Left", "RegularHorizontalAlignment");
						return;
					case HorizontalAlignment.Center:
						base.OnPropertyChanged<string>("Center", "RegularHorizontalAlignment");
						return;
					case HorizontalAlignment.Right:
						base.OnPropertyChanged<string>("Right", "RegularHorizontalAlignment");
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x0000F606 File Offset: 0x0000D806
		// (set) Token: 0x060004EE RID: 1262 RVA: 0x0000F610 File Offset: 0x0000D810
		[Editor(false)]
		public HorizontalAlignment OverflowHorizontalAlignment
		{
			get
			{
				return this._overflowHorizontalAlignment;
			}
			set
			{
				if (this._overflowHorizontalAlignment != value)
				{
					this._overflowHorizontalAlignment = value;
					switch (value)
					{
					case HorizontalAlignment.Left:
						base.OnPropertyChanged<string>("Left", "OverflowHorizontalAlignment");
						return;
					case HorizontalAlignment.Center:
						base.OnPropertyChanged<string>("Center", "OverflowHorizontalAlignment");
						return;
					case HorizontalAlignment.Right:
						base.OnPropertyChanged<string>("Right", "OverflowHorizontalAlignment");
						break;
					default:
						return;
					}
				}
			}
		}

		// Token: 0x04000219 RID: 537
		private HorizontalAlignment _regularHorizontalAlignment;

		// Token: 0x0400021A RID: 538
		private HorizontalAlignment _overflowHorizontalAlignment;
	}
}
