using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.GauntletUI.ExtraWidgets;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x02000063 RID: 99
	public class PartyFormationDropdownWidget : DropdownWidget
	{
		// Token: 0x06000557 RID: 1367 RVA: 0x0001026B File Offset: 0x0000E46B
		public PartyFormationDropdownWidget(UIContext context)
			: base(context)
		{
			base.DoNotHandleDropdownListPanel = true;
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001027B File Offset: 0x0000E47B
		private void ListStateChangerUpdated()
		{
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001027D File Offset: 0x0000E47D
		private void SeperatorStateChangerUpdated()
		{
		}

		// Token: 0x0600055A RID: 1370 RVA: 0x00010280 File Offset: 0x0000E480
		protected override void OpenPanel()
		{
			base.ListPanel.IsVisible = true;
			this.SeperatorStateChanger.IsVisible = true;
			this.ListStateChanger.Delay = this.SeperatorStateChanger.VisualDefinition.TransitionDuration;
			this.ListStateChanger.State = "Opened";
			this.ListStateChanger.Start();
			this.SeperatorStateChanger.Delay = 0f;
			this.SeperatorStateChanger.State = "Opened";
			this.SeperatorStateChanger.Start();
			base.Context.TwoDimensionContext.PlaySound("dropdown");
		}

		// Token: 0x0600055B RID: 1371 RVA: 0x0001031C File Offset: 0x0000E51C
		protected override void ClosePanel()
		{
			this.ListStateChanger.Delay = 0f;
			this.ListStateChanger.State = "Closed";
			this.ListStateChanger.Start();
			this.SeperatorStateChanger.Delay = this.ListStateChanger.TargetWidget.VisualDefinition.TransitionDuration;
			this.SeperatorStateChanger.State = "Closed";
			this.SeperatorStateChanger.Start();
			base.Context.TwoDimensionContext.PlaySound("dropdown");
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600055C RID: 1372 RVA: 0x000103A4 File Offset: 0x0000E5A4
		// (set) Token: 0x0600055D RID: 1373 RVA: 0x000103AC File Offset: 0x0000E5AC
		[Editor(false)]
		public DelayedStateChanger SeperatorStateChanger
		{
			get
			{
				return this._seperatorStateChanger;
			}
			set
			{
				if (this._seperatorStateChanger != value)
				{
					this._seperatorStateChanger = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "SeperatorStateChanger");
					this.SeperatorStateChangerUpdated();
				}
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x0600055E RID: 1374 RVA: 0x000103D0 File Offset: 0x0000E5D0
		// (set) Token: 0x0600055F RID: 1375 RVA: 0x000103D8 File Offset: 0x0000E5D8
		[Editor(false)]
		public DelayedStateChanger ListStateChanger
		{
			get
			{
				return this._listStateChanger;
			}
			set
			{
				if (this._listStateChanger != value)
				{
					this._listStateChanger = value;
					base.OnPropertyChanged<DelayedStateChanger>(value, "ListStateChanger");
					this.ListStateChangerUpdated();
				}
			}
		}

		// Token: 0x04000248 RID: 584
		private DelayedStateChanger _seperatorStateChanger;

		// Token: 0x04000249 RID: 585
		private DelayedStateChanger _listStateChanger;
	}
}
