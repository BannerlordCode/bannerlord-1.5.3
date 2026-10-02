using System;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Multiplayer
{
	// Token: 0x0200008E RID: 142
	public class MultiplayerPollProgressionWidget : Widget
	{
		// Token: 0x060007E0 RID: 2016 RVA: 0x00017004 File Offset: 0x00015204
		public MultiplayerPollProgressionWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x0001700D File Offset: 0x0001520D
		protected override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00017016 File Offset: 0x00015216
		// (set) Token: 0x060007E3 RID: 2019 RVA: 0x0001701E File Offset: 0x0001521E
		public bool HasOngoingPoll
		{
			get
			{
				return this._hasOngoingPoll;
			}
			set
			{
				if (value != this._hasOngoingPoll)
				{
					this._hasOngoingPoll = value;
					base.OnPropertyChanged(value, "HasOngoingPoll");
					ListPanel pollExtension = this.PollExtension;
					if (pollExtension == null)
					{
						return;
					}
					pollExtension.SetState(value ? "Active" : "Inactive");
				}
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x060007E4 RID: 2020 RVA: 0x0001705B File Offset: 0x0001525B
		// (set) Token: 0x060007E5 RID: 2021 RVA: 0x00017063 File Offset: 0x00015263
		[Editor(false)]
		public ListPanel PollExtension
		{
			get
			{
				return this._pollExtension;
			}
			set
			{
				if (value != this._pollExtension)
				{
					this._pollExtension = value;
					base.OnPropertyChanged<ListPanel>(value, "PollExtension");
					this._pollExtension.SetState("Inactive");
				}
			}
		}

		// Token: 0x04000373 RID: 883
		private const string _activeState = "Active";

		// Token: 0x04000374 RID: 884
		private const string _inactiveState = "Inactive";

		// Token: 0x04000375 RID: 885
		private bool _hasOngoingPoll;

		// Token: 0x04000376 RID: 886
		private ListPanel _pollExtension;
	}
}
