using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.HUD
{
	// Token: 0x020000C9 RID: 201
	public class MultiplayerTeamStatsSideRowWidget : ButtonWidget
	{
		// Token: 0x06000AA7 RID: 2727 RVA: 0x0001DF3D File Offset: 0x0001C13D
		public MultiplayerTeamStatsSideRowWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x06000AA8 RID: 2728 RVA: 0x0001DF46 File Offset: 0x0001C146
		// (set) Token: 0x06000AA9 RID: 2729 RVA: 0x0001DF4E File Offset: 0x0001C14E
		[Editor(false)]
		public bool IsFollowed
		{
			get
			{
				return this._isFollowed;
			}
			set
			{
				if (this._isFollowed != value)
				{
					this._isFollowed = value;
					base.OnPropertyChanged(value, "IsFollowed");
					base.IsSelected = value;
				}
			}
		}

		// Token: 0x040004DD RID: 1245
		private bool _isFollowed;
	}
}
