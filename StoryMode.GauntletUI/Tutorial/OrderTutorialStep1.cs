using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000031 RID: 49
	[Tutorial("OrderTutorial1TutorialStep1")]
	public class OrderTutorialStep1 : TutorialItemBase
	{
		// Token: 0x060000F2 RID: 242 RVA: 0x00003A5C File Offset: 0x00001C5C
		public OrderTutorialStep1()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "";
			base.MouseRequired = false;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00003A80 File Offset: 0x00001C80
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
			return this._hasPlayerOrderedFollowMe;
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00003AF8 File Offset: 0x00001CF8
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

		// Token: 0x060000F5 RID: 245 RVA: 0x00003B59 File Offset: 0x00001D59
		private void OnPlayerOrdered(OrderType orderType, IEnumerable<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			this._hasPlayerOrderedFollowMe = this._hasPlayerOrderedFollowMe || (orderType == OrderType.FollowMe && appliedFormations.Any<Formation>());
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00003B79 File Offset: 0x00001D79
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00003B7C File Offset: 0x00001D7C
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.CurrentContext == TutorialContexts.Mission && TutorialHelper.IsPlayerInABattleMission && Mission.Current.Mode != MissionMode.Deployment && TutorialHelper.IsOrderingAvailable;
		}

		// Token: 0x04000039 RID: 57
		private bool _hasPlayerOrderedFollowMe;

		// Token: 0x0400003A RID: 58
		private bool _registeredToOrderEvent;
	}
}
