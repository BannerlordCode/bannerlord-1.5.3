using System;
using System.Collections.Generic;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.GauntletUI.Mission;
using TaleWorlds.MountAndBlade.Multiplayer.View.MissionViews;
using TaleWorlds.MountAndBlade.Multiplayer.ViewModelCollection;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.ViewModelCollection.EscapeMenu;

namespace TaleWorlds.MountAndBlade.Multiplayer.GauntletUI.Mission
{
	// Token: 0x02000017 RID: 23
	[OverrideView(typeof(MissionMultiplayerPracticeEscapeMenu))]
	public class MissionGauntletMultiplayerPracticeEscapeMenu : MissionGauntletEscapeMenuBase
	{
		// Token: 0x0600010D RID: 269 RVA: 0x00007108 File Offset: 0x00005308
		public MissionGauntletMultiplayerPracticeEscapeMenu()
			: base("MultiplayerEscapeMenu")
		{
		}

		// Token: 0x0600010E RID: 270 RVA: 0x00007115 File Offset: 0x00005315
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this.DataSource = new MPEscapeMenuVM(null, null);
		}

		// Token: 0x0600010F RID: 271 RVA: 0x0000712A File Offset: 0x0000532A
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			this.DataSource.Tick(dt);
		}

		// Token: 0x06000110 RID: 272 RVA: 0x00007140 File Offset: 0x00005340
		protected override List<EscapeMenuItemVM> GetEscapeMenuItems()
		{
			List<EscapeMenuItemVM> list = new List<EscapeMenuItemVM>();
			list.Add(new EscapeMenuItemVM(new TextObject("{=e139gKZc}Return to the Game", null), delegate(object o)
			{
				base.OnEscapeMenuToggled(false);
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			list.Add(new EscapeMenuItemVM(new TextObject("{=EXqcmGy4}Return to Lobby", null), delegate(object o)
			{
				base.OnEscapeMenuToggled(false);
				base.Mission.EndMission();
			}, null, () => new Tuple<bool, TextObject>(false, null), false));
			return list;
		}
	}
}
