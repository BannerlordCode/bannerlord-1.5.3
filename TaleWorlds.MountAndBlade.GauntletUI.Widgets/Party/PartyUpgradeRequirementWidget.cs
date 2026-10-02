using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Party
{
	// Token: 0x0200006E RID: 110
	public class PartyUpgradeRequirementWidget : Widget
	{
		// Token: 0x06000605 RID: 1541 RVA: 0x00011DE4 File Offset: 0x0000FFE4
		public PartyUpgradeRequirementWidget(UIContext context)
			: base(context)
		{
			this.NormalColor = new Color(1f, 1f, 1f, 1f);
			this.InsufficientColor = new Color(0.753f, 0.071f, 0.098f, 1f);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x00011E40 File Offset: 0x00010040
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._requiresRefresh)
			{
				if (this.RequirementId != null)
				{
					string text = (this.IsPerkRequirement ? "SPGeneral\\Skills\\gui_skills_icon_" : "StdAssets\\ItemIcons\\");
					string text2 = (this.IsPerkRequirement ? "_tiny" : "");
					base.Sprite = base.Context.SpriteData.GetSprite(text + this.RequirementId + text2);
				}
				base.Color = (this.IsSufficient ? this.NormalColor : this.InsufficientColor);
				this._requiresRefresh = false;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000607 RID: 1543 RVA: 0x00011ED4 File Offset: 0x000100D4
		// (set) Token: 0x06000608 RID: 1544 RVA: 0x00011EDC File Offset: 0x000100DC
		[Editor(false)]
		public string RequirementId
		{
			get
			{
				return this._requirementId;
			}
			set
			{
				if (value != this._requirementId)
				{
					this._requirementId = value;
					base.OnPropertyChanged<string>(value, "RequirementId");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x00011F06 File Offset: 0x00010106
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x00011F0E File Offset: 0x0001010E
		[Editor(false)]
		public bool IsSufficient
		{
			get
			{
				return this._isSufficient;
			}
			set
			{
				if (value != this._isSufficient)
				{
					this._isSufficient = value;
					base.OnPropertyChanged(value, "IsSufficient");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x0600060B RID: 1547 RVA: 0x00011F33 File Offset: 0x00010133
		// (set) Token: 0x0600060C RID: 1548 RVA: 0x00011F3B File Offset: 0x0001013B
		[Editor(false)]
		public bool IsPerkRequirement
		{
			get
			{
				return this._isPerkRequirement;
			}
			set
			{
				if (value != this._isPerkRequirement)
				{
					this._isPerkRequirement = value;
					base.OnPropertyChanged(value, "IsPerkRequirement");
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x0600060D RID: 1549 RVA: 0x00011F60 File Offset: 0x00010160
		// (set) Token: 0x0600060E RID: 1550 RVA: 0x00011F68 File Offset: 0x00010168
		public Color NormalColor
		{
			get
			{
				return this._normalColor;
			}
			set
			{
				if (value != this._normalColor)
				{
					this._normalColor = value;
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600060F RID: 1551 RVA: 0x00011F86 File Offset: 0x00010186
		// (set) Token: 0x06000610 RID: 1552 RVA: 0x00011F8E File Offset: 0x0001018E
		public Color InsufficientColor
		{
			get
			{
				return this._insufficientColor;
			}
			set
			{
				if (value != this._insufficientColor)
				{
					this._insufficientColor = value;
					this._requiresRefresh = true;
				}
			}
		}

		// Token: 0x04000292 RID: 658
		private bool _requiresRefresh = true;

		// Token: 0x04000293 RID: 659
		private string _requirementId;

		// Token: 0x04000294 RID: 660
		private bool _isSufficient;

		// Token: 0x04000295 RID: 661
		private bool _isPerkRequirement;

		// Token: 0x04000296 RID: 662
		private Color _normalColor;

		// Token: 0x04000297 RID: 663
		private Color _insufficientColor;
	}
}
