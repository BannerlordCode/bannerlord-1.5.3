using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Scoreboard
{
	// Token: 0x02000096 RID: 150
	public class MultiplayerScoreboardStatsParentWidget : Widget
	{
		// Token: 0x0600083C RID: 2108 RVA: 0x00017EBA File Offset: 0x000160BA
		public MultiplayerScoreboardStatsParentWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00017EC4 File Offset: 0x000160C4
		private void RefreshActiveState()
		{
			float num = (this.IsActive ? this.ActiveAlpha : this.InactiveAlpha);
			List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
			for (int i = 0; i < allChildrenRecursive.Count; i++)
			{
				RichTextWidget richTextWidget;
				TextWidget textWidget;
				if ((richTextWidget = allChildrenRecursive[i] as RichTextWidget) != null)
				{
					richTextWidget.SetAlpha(num);
				}
				else if ((textWidget = allChildrenRecursive[i] as TextWidget) != null)
				{
					textWidget.SetAlpha(num);
				}
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x0600083E RID: 2110 RVA: 0x00017F33 File Offset: 0x00016133
		// (set) Token: 0x0600083F RID: 2111 RVA: 0x00017F3B File Offset: 0x0001613B
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChanged(value, "IsActive");
					this.RefreshActiveState();
				}
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000840 RID: 2112 RVA: 0x00017F5F File Offset: 0x0001615F
		// (set) Token: 0x06000841 RID: 2113 RVA: 0x00017F6A File Offset: 0x0001616A
		public bool IsInactive
		{
			get
			{
				return !this.IsActive;
			}
			set
			{
				if (value == this.IsActive)
				{
					this.IsActive = !value;
					base.OnPropertyChanged(value, "IsInactive");
				}
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000842 RID: 2114 RVA: 0x00017F8B File Offset: 0x0001618B
		// (set) Token: 0x06000843 RID: 2115 RVA: 0x00017F93 File Offset: 0x00016193
		public float ActiveAlpha
		{
			get
			{
				return this._activeAlpha;
			}
			set
			{
				if (value != this._activeAlpha)
				{
					this._activeAlpha = value;
					base.OnPropertyChanged(value, "ActiveAlpha");
				}
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000844 RID: 2116 RVA: 0x00017FB1 File Offset: 0x000161B1
		// (set) Token: 0x06000845 RID: 2117 RVA: 0x00017FB9 File Offset: 0x000161B9
		public float InactiveAlpha
		{
			get
			{
				return this._inactiveAlpha;
			}
			set
			{
				if (value != this._inactiveAlpha)
				{
					this._inactiveAlpha = value;
					base.OnPropertyChanged(value, "InactiveAlpha");
				}
			}
		}

		// Token: 0x040003AE RID: 942
		private bool _isActive;

		// Token: 0x040003AF RID: 943
		private float _activeAlpha;

		// Token: 0x040003B0 RID: 944
		private float _inactiveAlpha;
	}
}
