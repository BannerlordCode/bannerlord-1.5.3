using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer.Lobby.Clan
{
	// Token: 0x020000B3 RID: 179
	public class MultiplayerLobbyClanMemberRankVisualBrushWidget : BrushWidget
	{
		// Token: 0x06000972 RID: 2418 RVA: 0x0001ACD3 File Offset: 0x00018ED3
		public MultiplayerLobbyClanMemberRankVisualBrushWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x06000973 RID: 2419 RVA: 0x0001ACE4 File Offset: 0x00018EE4
		private void UpdateTypeVisual()
		{
			if (this.Type == 0)
			{
				this.SetState("Member");
				return;
			}
			if (this.Type == 1)
			{
				this.SetState("Officer");
				return;
			}
			if (this.Type == 2)
			{
				this.SetState("Leader");
				return;
			}
			Debug.FailedAssert("This member type is not defined in widget", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.GauntletUI.Widgets\\Multiplayer\\Lobby\\Clan\\MultiplayerLobbyClanMemberRankVisualBrushWidget.cs", "UpdateTypeVisual", 28);
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06000974 RID: 2420 RVA: 0x0001AD45 File Offset: 0x00018F45
		// (set) Token: 0x06000975 RID: 2421 RVA: 0x0001AD4D File Offset: 0x00018F4D
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
					this.UpdateTypeVisual();
				}
			}
		}

		// Token: 0x04000441 RID: 1089
		private int _type = -1;
	}
}
