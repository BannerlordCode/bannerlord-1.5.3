using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.TwoDimension;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Quest
{
	// Token: 0x0200005E RID: 94
	public class QuestMarkerBrushWidget : BrushWidget
	{
		// Token: 0x0600052E RID: 1326 RVA: 0x0000FD23 File Offset: 0x0000DF23
		public QuestMarkerBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0000FD2C File Offset: 0x0000DF2C
		private void UpdateMarkerState(int type)
		{
			string text;
			switch (type)
			{
			case 0:
				text = "None";
				goto IL_006D;
			case 1:
				text = "AvailableIssue";
				goto IL_006D;
			case 2:
				text = "ActiveIssue";
				goto IL_006D;
			case 3:
			case 5:
			case 6:
			case 7:
				break;
			case 4:
				text = "ActiveStoryQuest";
				goto IL_006D;
			case 8:
				text = "TrackedIssue";
				goto IL_006D;
			default:
				if (type == 16)
				{
					text = "TrackedStoryQuest";
					goto IL_006D;
				}
				break;
			}
			text = "None";
			IL_006D:
			if (text != null)
			{
				this.SetState(text);
				Sprite sprite = base.Brush.GetLayer(text).Sprite;
				if (sprite != null)
				{
					float num = base.SuggestedHeight / (float)sprite.Height;
					base.SuggestedWidth = (float)sprite.Width * num;
				}
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000530 RID: 1328 RVA: 0x0000FDE3 File Offset: 0x0000DFE3
		// (set) Token: 0x06000531 RID: 1329 RVA: 0x0000FDEB File Offset: 0x0000DFEB
		public int QuestMarkerType
		{
			get
			{
				return this._questMarkerType;
			}
			set
			{
				if (value != this._questMarkerType)
				{
					this._questMarkerType = value;
					this.UpdateMarkerState(this._questMarkerType);
				}
			}
		}

		// Token: 0x04000236 RID: 566
		private int _questMarkerType;
	}
}
