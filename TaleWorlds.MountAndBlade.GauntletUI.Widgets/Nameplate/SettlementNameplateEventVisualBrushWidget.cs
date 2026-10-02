using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Nameplate
{
	// Token: 0x02000080 RID: 128
	public class SettlementNameplateEventVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000739 RID: 1849 RVA: 0x00015448 File Offset: 0x00013648
		public SettlementNameplateEventVisualBrushWidget(UIContext context)
			: base(context)
		{
			base.EventManager.AddLateUpdateAction(this, new Action<float>(this.LateUpdateAction), 1);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00015471 File Offset: 0x00013671
		private void LateUpdateAction(float dt)
		{
			if (!this._determinedVisual)
			{
				this.RegisterBrushStatesOfWidget();
				this.UpdateVisual(this.Type);
				this._determinedVisual = true;
			}
		}

		// Token: 0x0600073B RID: 1851 RVA: 0x00015494 File Offset: 0x00013694
		private void UpdateVisual(int type)
		{
			switch (type)
			{
			case 0:
				this.SetState("Tournament");
				break;
			case 1:
				this.SetState("AvailableIssue");
				break;
			case 2:
				this.SetState("ActiveQuest");
				break;
			case 3:
				this.SetState("ActiveStoryQuest");
				break;
			case 4:
				this.SetState("TrackedIssue");
				break;
			case 5:
				this.SetState("TrackedStoryQuest");
				break;
			case 6:
				this.SetState(this.AdditionalParameters);
				base.MarginLeft = 2f;
				base.MarginRight = 2f;
				break;
			}
			Brush brush = base.Brush;
			Sprite sprite;
			if (brush == null)
			{
				sprite = null;
			}
			else
			{
				Style style = brush.GetStyle(base.CurrentState);
				if (style == null)
				{
					sprite = null;
				}
				else
				{
					StyleLayer layer = style.GetLayer(0);
					sprite = ((layer != null) ? layer.Sprite : null);
				}
			}
			Sprite sprite2 = sprite;
			if (sprite2 != null)
			{
				base.SuggestedWidth = base.SuggestedHeight / (float)sprite2.Height * (float)sprite2.Width;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x00015586 File Offset: 0x00013786
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0001558E File Offset: 0x0001378E
		[Editor(false)]
		public int Type
		{
			get
			{
				return this._type;
			}
			set
			{
				if (this._type != value)
				{
					this._type = value;
					base.OnPropertyChanged(value, "Type");
				}
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x000155AC File Offset: 0x000137AC
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x000155B4 File Offset: 0x000137B4
		[Editor(false)]
		public string AdditionalParameters
		{
			get
			{
				return this._additionalParameters;
			}
			set
			{
				if (this._additionalParameters != value)
				{
					this._additionalParameters = value;
					base.OnPropertyChanged<string>(value, "AdditionalParameters");
				}
			}
		}

		// Token: 0x04000321 RID: 801
		private bool _determinedVisual;

		// Token: 0x04000322 RID: 802
		private int _type = -1;

		// Token: 0x04000323 RID: 803
		private string _additionalParameters;
	}
}
