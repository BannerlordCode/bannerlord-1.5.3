using System;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Source.Missions.Handlers
{
	// Token: 0x020003E2 RID: 994
	public class BasicMissionHandler : MissionLogic
	{
		// Token: 0x17000A1D RID: 2589
		// (get) Token: 0x06003753 RID: 14163 RVA: 0x000E5C23 File Offset: 0x000E3E23
		// (set) Token: 0x06003754 RID: 14164 RVA: 0x000E5C2B File Offset: 0x000E3E2B
		public bool IsWarningWidgetOpened { get; private set; }

		// Token: 0x06003755 RID: 14165 RVA: 0x000E5C34 File Offset: 0x000E3E34
		public override void OnBehaviorInitialize()
		{
			base.OnBehaviorInitialize();
			this.IsWarningWidgetOpened = false;
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x000E5C43 File Offset: 0x000E3E43
		public void CreateWarningWidgetForResult(BattleEndLogic.ExitResult result)
		{
			if (!GameNetwork.IsClient)
			{
				MBCommon.PauseGameEngine();
			}
			this._isSurrender = result == BattleEndLogic.ExitResult.SurrenderSiege;
			InformationManager.ShowInquiry(this._isSurrender ? this.GetSurrenderPopupData() : this.GetRetreatPopUpData(), true, false);
			this.IsWarningWidgetOpened = true;
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x000E5C7F File Offset: 0x000E3E7F
		private void CloseSelectionWidget()
		{
			if (!this.IsWarningWidgetOpened)
			{
				return;
			}
			this.IsWarningWidgetOpened = false;
			if (!GameNetwork.IsClient)
			{
				MBCommon.UnPauseGameEngine();
			}
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x000E5C9D File Offset: 0x000E3E9D
		private void OnEventCancelSelectionWidget()
		{
			this.CloseSelectionWidget();
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x000E5CA8 File Offset: 0x000E3EA8
		private void OnEventAcceptSelectionWidget()
		{
			MissionLogic[] array = base.Mission.MissionLogics.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].OnBattleEnded();
			}
			this.CloseSelectionWidget();
			if (this._isSurrender)
			{
				base.Mission.SurrenderMission();
				return;
			}
			base.Mission.RetreatMission();
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x000E5D04 File Offset: 0x000E3F04
		private InquiryData GetRetreatPopUpData()
		{
			return new InquiryData("", GameTexts.FindText("str_retreat_question", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(this.OnEventAcceptSelectionWidget), new Action(this.OnEventCancelSelectionWidget), "", 0f, null, null, null);
		}

		// Token: 0x0600375B RID: 14171 RVA: 0x000E5D74 File Offset: 0x000E3F74
		private InquiryData GetSurrenderPopupData()
		{
			return new InquiryData(GameTexts.FindText("str_surrender", null).ToString(), GameTexts.FindText("str_surrender_question", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(this.OnEventAcceptSelectionWidget), new Action(this.OnEventCancelSelectionWidget), "", 0f, null, null, null);
		}

		// Token: 0x040017E5 RID: 6117
		private bool _isSurrender;
	}
}
