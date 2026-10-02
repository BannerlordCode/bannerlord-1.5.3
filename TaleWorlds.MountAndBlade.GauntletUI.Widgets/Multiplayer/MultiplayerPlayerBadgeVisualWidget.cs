using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008D RID: 141
	public class MultiplayerPlayerBadgeVisualWidget : Widget
	{
		// Token: 0x060007DA RID: 2010 RVA: 0x00016F52 File Offset: 0x00015152
		public MultiplayerPlayerBadgeVisualWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00016F5B File Offset: 0x0001515B
		private void UpdateVisual(string badgeId)
		{
			if (badgeId == "badge_official_server_admin")
			{
				badgeId = "badge_taleworlds_dev";
			}
			base.Sprite = base.Context.SpriteData.GetSprite("MPPlayerBadges\\" + badgeId);
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00016F92 File Offset: 0x00015192
		protected override void OnLateUpdate(float dt)
		{
			base.OnLateUpdate(dt);
			if (this._hasForcedSize)
			{
				base.SuggestedWidth = this._forcedWidth;
				base.SuggestedHeight = this._forcedHeight;
			}
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00016FBB File Offset: 0x000151BB
		public void SetForcedSize(float width, float height)
		{
			this._forcedWidth = width;
			this._forcedHeight = height;
			this._hasForcedSize = true;
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x060007DE RID: 2014 RVA: 0x00016FD2 File Offset: 0x000151D2
		// (set) Token: 0x060007DF RID: 2015 RVA: 0x00016FDA File Offset: 0x000151DA
		public string BadgeId
		{
			get
			{
				return this._badgeId;
			}
			set
			{
				if (value != this._badgeId)
				{
					this._badgeId = value;
					base.OnPropertyChanged<string>(value, "BadgeId");
					this.UpdateVisual(value);
				}
			}
		}

		// Token: 0x0400036F RID: 879
		private float _forcedWidth;

		// Token: 0x04000370 RID: 880
		private float _forcedHeight;

		// Token: 0x04000371 RID: 881
		private bool _hasForcedSize;

		// Token: 0x04000372 RID: 882
		private string _badgeId;
	}
}
