using System;
using System.Collections.Generic;
using TaleWorlds.GauntletUI;
using TaleWorlds.GauntletUI.BaseTypes;

namespace TaleWorlds.MountAndBlade.GauntletUI.Widgets.Tournament
{
	// Token: 0x02000051 RID: 81
	public class TournamentMatchWidget : Widget
	{
		// Token: 0x06000475 RID: 1141 RVA: 0x0000E4AA File Offset: 0x0000C6AA
		public TournamentMatchWidget(UIContext context)
			: base(context)
		{
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x0000E4B3 File Offset: 0x0000C6B3
		// (set) Token: 0x06000477 RID: 1143 RVA: 0x0000E4BC File Offset: 0x0000C6BC
		[Editor(false)]
		public int State
		{
			get
			{
				return this._state;
			}
			set
			{
				if (this._state != value)
				{
					this._state = value;
					List<Widget> allChildrenRecursive = base.GetAllChildrenRecursive(null);
					for (int i = 0; i < allChildrenRecursive.Count; i++)
					{
						TournamentParticipantBrushWidget tournamentParticipantBrushWidget;
						if ((tournamentParticipantBrushWidget = allChildrenRecursive[i] as TournamentParticipantBrushWidget) != null)
						{
							tournamentParticipantBrushWidget.MatchState = this.State;
						}
					}
				}
			}
		}

		// Token: 0x040001E4 RID: 484
		private int _state;
	}
}
