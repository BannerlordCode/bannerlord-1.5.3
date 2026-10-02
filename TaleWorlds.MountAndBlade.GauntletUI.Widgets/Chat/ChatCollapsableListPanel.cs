using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Chat
{
	// Token: 0x0200017C RID: 380
	public class ChatCollapsableListPanel : ListPanel
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x060013D4 RID: 5076 RVA: 0x000360B2 File Offset: 0x000342B2
		// (set) Token: 0x060013D5 RID: 5077 RVA: 0x000360BA File Offset: 0x000342BA
		public bool IsLinesVisible { get; private set; }

		// Token: 0x060013D6 RID: 5078 RVA: 0x000360C3 File Offset: 0x000342C3
		public ChatCollapsableListPanel(UIContext context)
			: base(context)
		{
			this.RefreshAlphaValues(this.Alpha);
		}

		// Token: 0x060013D7 RID: 5079 RVA: 0x000360D8 File Offset: 0x000342D8
		private void ToggleLines(bool isVisible)
		{
			for (int i = 0; i < base.ChildCount; i++)
			{
				base.GetChild(i).IsVisible = i == 0 || isVisible;
			}
			this.IsLinesVisible = isVisible;
		}

		// Token: 0x060013D8 RID: 5080 RVA: 0x0003610F File Offset: 0x0003430F
		protected override void OnMousePressed()
		{
			base.OnMousePressed();
			this.ToggleLines(!this.IsLinesVisible);
		}

		// Token: 0x060013D9 RID: 5081 RVA: 0x00036126 File Offset: 0x00034326
		protected override bool OnPreviewMousePressed()
		{
			return base.OnPreviewMousePressed();
		}

		// Token: 0x060013DA RID: 5082 RVA: 0x0003612E File Offset: 0x0003432E
		protected override void OnChildAdded(Widget child)
		{
			base.OnChildAdded(child);
			this.RefreshAlphaValues(this.Alpha);
			this.ToggleLines(true);
		}

		// Token: 0x060013DB RID: 5083 RVA: 0x0003614A File Offset: 0x0003434A
		private void RefreshAlphaValues(float newAlpha)
		{
			this.SetGlobalAlphaRecursively(newAlpha);
			if (newAlpha > 0f)
			{
				ChatLogWidget parentChatLogWidget = this.ParentChatLogWidget;
				if (parentChatLogWidget == null)
				{
					return;
				}
				parentChatLogWidget.RegisterMultiLineElement(this);
				return;
			}
			else
			{
				ChatLogWidget parentChatLogWidget2 = this.ParentChatLogWidget;
				if (parentChatLogWidget2 == null)
				{
					return;
				}
				parentChatLogWidget2.RemoveMultiLineElement(this);
				return;
			}
		}

		// Token: 0x060013DC RID: 5084 RVA: 0x00036180 File Offset: 0x00034380
		private void UpdateColorValuesOfChildren(Widget widget, Color newColor)
		{
			foreach (Widget widget2 in widget.Children)
			{
				BrushWidget brushWidget;
				if ((brushWidget = widget2 as BrushWidget) != null)
				{
					brushWidget.Brush.FontColor = newColor;
				}
				else
				{
					widget2.Color = newColor;
				}
				this.UpdateColorValuesOfChildren(widget2, newColor);
			}
		}

		// Token: 0x060013DD RID: 5085 RVA: 0x000361F4 File Offset: 0x000343F4
		private void RefreshColorValues(Color newColor)
		{
			this.UpdateColorValuesOfChildren(this, newColor);
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x060013DE RID: 5086 RVA: 0x000361FE File Offset: 0x000343FE
		// (set) Token: 0x060013DF RID: 5087 RVA: 0x00036206 File Offset: 0x00034406
		[DataSourceProperty]
		public float Alpha
		{
			get
			{
				return this._alpha;
			}
			set
			{
				if (value != this._alpha)
				{
					this._alpha = value;
					base.OnPropertyChanged(value, "Alpha");
					this.RefreshAlphaValues(value);
				}
			}
		}

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x060013E0 RID: 5088 RVA: 0x0003622B File Offset: 0x0003442B
		// (set) Token: 0x060013E1 RID: 5089 RVA: 0x00036233 File Offset: 0x00034433
		[DataSourceProperty]
		public Color LineColor
		{
			get
			{
				return this._lineColor;
			}
			set
			{
				if (value != this._lineColor)
				{
					this._lineColor = value;
					base.OnPropertyChanged(value, "LineColor");
					this.RefreshColorValues(value);
				}
			}
		}

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x060013E2 RID: 5090 RVA: 0x0003625D File Offset: 0x0003445D
		// (set) Token: 0x060013E3 RID: 5091 RVA: 0x00036265 File Offset: 0x00034465
		[DataSourceProperty]
		public ChatLogWidget ParentChatLogWidget
		{
			get
			{
				return this._parentChatLogWidget;
			}
			set
			{
				if (value != this._parentChatLogWidget)
				{
					this._parentChatLogWidget = value;
					base.OnPropertyChanged<ChatLogWidget>(value, "ParentChatLogWidget");
				}
			}
		}

		// Token: 0x04000904 RID: 2308
		private float _alpha;

		// Token: 0x04000905 RID: 2309
		private Color _lineColor;

		// Token: 0x04000906 RID: 2310
		private ChatLogWidget _parentChatLogWidget;
	}
}
