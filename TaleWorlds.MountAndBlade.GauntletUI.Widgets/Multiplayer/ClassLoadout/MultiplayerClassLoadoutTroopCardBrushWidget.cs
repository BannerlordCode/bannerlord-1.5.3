using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.ClassLoadout
{
	// Token: 0x020000CF RID: 207
	public class MultiplayerClassLoadoutTroopCardBrushWidget : BrushWidget
	{
		// Token: 0x06000ACC RID: 2764 RVA: 0x0001E662 File Offset: 0x0001C862
		public MultiplayerClassLoadoutTroopCardBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000ACD RID: 2765 RVA: 0x0001E66C File Offset: 0x0001C86C
		private void OnCultureIDUpdated()
		{
			if (this.CultureID != null)
			{
				this.SetState(this.CultureID);
				BrushWidget border = this.Border;
				if (border != null)
				{
					border.SetState(this.CultureID);
				}
				BrushWidget classBorder = this.ClassBorder;
				if (classBorder != null)
				{
					classBorder.SetState(this.CultureID);
				}
				BrushWidget classFrame = this.ClassFrame;
				if (classFrame == null)
				{
					return;
				}
				classFrame.SetState(this.CultureID);
			}
		}

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x06000ACE RID: 2766 RVA: 0x0001E6D1 File Offset: 0x0001C8D1
		// (set) Token: 0x06000ACF RID: 2767 RVA: 0x0001E6D9 File Offset: 0x0001C8D9
		[Editor(false)]
		public string CultureID
		{
			get
			{
				return this._cultureID;
			}
			set
			{
				if (value != this._cultureID)
				{
					this._cultureID = value;
					base.OnPropertyChanged<string>(value, "CultureID");
					this.OnCultureIDUpdated();
				}
			}
		}

		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x06000AD0 RID: 2768 RVA: 0x0001E702 File Offset: 0x0001C902
		// (set) Token: 0x06000AD1 RID: 2769 RVA: 0x0001E70A File Offset: 0x0001C90A
		[Editor(false)]
		public BrushWidget Border
		{
			get
			{
				return this._border;
			}
			set
			{
				if (value != this._border)
				{
					this._border = value;
					base.OnPropertyChanged<BrushWidget>(value, "Border");
					this.OnCultureIDUpdated();
				}
			}
		}

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x06000AD2 RID: 2770 RVA: 0x0001E72E File Offset: 0x0001C92E
		// (set) Token: 0x06000AD3 RID: 2771 RVA: 0x0001E736 File Offset: 0x0001C936
		[Editor(false)]
		public BrushWidget ClassBorder
		{
			get
			{
				return this._classBorder;
			}
			set
			{
				if (value != this._classBorder)
				{
					this._classBorder = value;
					base.OnPropertyChanged<BrushWidget>(value, "ClassBorder");
					this.OnCultureIDUpdated();
				}
			}
		}

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x06000AD4 RID: 2772 RVA: 0x0001E75A File Offset: 0x0001C95A
		// (set) Token: 0x06000AD5 RID: 2773 RVA: 0x0001E762 File Offset: 0x0001C962
		[Editor(false)]
		public BrushWidget ClassFrame
		{
			get
			{
				return this._classFrame;
			}
			set
			{
				if (value != this._classFrame)
				{
					this._classFrame = value;
					base.OnPropertyChanged<BrushWidget>(value, "ClassFrame");
					base.OnPropertyChanged<BrushWidget>(value, "ClassFrame");
				}
			}
		}

		// Token: 0x040004EB RID: 1259
		private string _cultureID;

		// Token: 0x040004EC RID: 1260
		private BrushWidget _border;

		// Token: 0x040004ED RID: 1261
		private BrushWidget _classBorder;

		// Token: 0x040004EE RID: 1262
		private BrushWidget _classFrame;
	}
}
