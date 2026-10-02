using System;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace SandBox.View.Map.Navigation
{
	// Token: 0x02000069 RID: 105
	public abstract class MapNavigationElementBase : INavigationElement
	{
		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000480 RID: 1152 RVA: 0x00024F71 File Offset: 0x00023171
		public NavigationPermissionItem Permission
		{
			get
			{
				return this.GetPermission();
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000481 RID: 1153 RVA: 0x00024F79 File Offset: 0x00023179
		public TextObject Tooltip
		{
			get
			{
				return this.GetTooltip();
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000482 RID: 1154 RVA: 0x00024F81 File Offset: 0x00023181
		public TextObject AlertTooltip
		{
			get
			{
				return this.GetAlertTooltip();
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000483 RID: 1155
		public abstract bool IsActive { get; }

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000484 RID: 1156
		public abstract bool IsLockingNavigation { get; }

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000485 RID: 1157
		public abstract bool HasAlert { get; }

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000486 RID: 1158
		public abstract string StringId { get; }

		// Token: 0x06000487 RID: 1159
		public abstract void OpenView();

		// Token: 0x06000488 RID: 1160
		public abstract void OpenView(params object[] parameters);

		// Token: 0x06000489 RID: 1161
		public abstract void GoToLink();

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600048A RID: 1162 RVA: 0x00024F89 File Offset: 0x00023189
		protected Game _game
		{
			get
			{
				return Game.Current;
			}
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x00024F90 File Offset: 0x00023190
		public MapNavigationElementBase(MapNavigationHandler handler)
		{
			this._handler = handler;
			this._viewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
		}

		// Token: 0x0600048C RID: 1164
		protected abstract NavigationPermissionItem GetPermission();

		// Token: 0x0600048D RID: 1165
		protected abstract TextObject GetTooltip();

		// Token: 0x0600048E RID: 1166
		protected abstract TextObject GetAlertTooltip();

		// Token: 0x04000236 RID: 566
		protected readonly MapNavigationHandler _handler;

		// Token: 0x04000237 RID: 567
		protected readonly IViewDataTracker _viewDataTracker;
	}
}
