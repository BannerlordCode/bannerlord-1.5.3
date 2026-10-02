using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000033 RID: 51
	[Tutorial("OrderTutorial2Tutorial")]
	public class OrderHideoutTutorial : TutorialItemBase
	{
		// Token: 0x060000FE RID: 254 RVA: 0x00003CE5 File Offset: 0x00001EE5
		public OrderHideoutTutorial()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "";
			base.MouseRequired = false;
		}

		// Token: 0x060000FF RID: 255 RVA: 0x00003D08 File Offset: 0x00001F08
		public override bool IsConditionsMetForCompletion()
		{
			if (!this._registeredToOrderEvent)
			{
				Mission mission = Mission.Current;
				bool flag;
				if (mission == null)
				{
					flag = null != null;
				}
				else
				{
					Team playerTeam = mission.PlayerTeam;
					flag = ((playerTeam != null) ? playerTeam.PlayerOrderController : null) != null;
				}
				if (flag)
				{
					Mission mission2 = Mission.Current;
					if (mission2 != null && mission2.Mode == MissionMode.Battle)
					{
						Mission.Current.PlayerTeam.PlayerOrderController.OnOrderIssued += new OnOrderIssuedDelegate(this.OnPlayerOrdered);
						this._registeredToOrderEvent = true;
					}
				}
			}
			return this._hasPlayerOrderedFollowme;
		}

		// Token: 0x06000100 RID: 256 RVA: 0x00003D80 File Offset: 0x00001F80
		public override void OnDeactivate()
		{
			base.OnDeactivate();
			if (this._registeredToOrderEvent)
			{
				Mission mission = Mission.Current;
				bool flag;
				if (mission == null)
				{
					flag = null != null;
				}
				else
				{
					Team playerTeam = mission.PlayerTeam;
					flag = ((playerTeam != null) ? playerTeam.PlayerOrderController : null) != null;
				}
				if (flag)
				{
					Mission.Current.PlayerTeam.PlayerOrderController.OnOrderIssued -= new OnOrderIssuedDelegate(this.OnPlayerOrdered);
				}
			}
			this._registeredToOrderEvent = false;
		}

		// Token: 0x06000101 RID: 257 RVA: 0x00003DE1 File Offset: 0x00001FE1
		private void OnPlayerOrdered(OrderType orderType, IEnumerable<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			this._hasPlayerOrderedFollowme = this._hasPlayerOrderedFollowme || (orderType == OrderType.FollowMe && appliedFormations.Any<Formation>());
		}

		// Token: 0x06000102 RID: 258 RVA: 0x00003E01 File Offset: 0x00002001
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x06000103 RID: 259 RVA: 0x00003E04 File Offset: 0x00002004
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.CurrentContext == TutorialContexts.Mission && TutorialHelper.IsPlayerInAHideoutBattleMission && TutorialHelper.IsOrderingAvailable;
		}

		// Token: 0x0400003D RID: 61
		private bool _hasPlayerOrderedFollowme;

		// Token: 0x0400003E RID: 62
		private bool _registeredToOrderEvent;
	}
}
