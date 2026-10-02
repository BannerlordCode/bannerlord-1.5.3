using System;
using System.Collections.Generic;
using System.Linq;
using SandBox.GauntletUI.Tutorial;
using SandBox.ViewModelCollection.Tutorial;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace StoryMode.GauntletUI.Tutorial
{
	// Token: 0x02000032 RID: 50
	[Tutorial("OrderTutorial1TutorialStep2")]
	public class OrderTutorialStep2 : TutorialItemBase
	{
		// Token: 0x060000F8 RID: 248 RVA: 0x00003BA1 File Offset: 0x00001DA1
		public OrderTutorialStep2()
		{
			base.Placement = TutorialItemVM.ItemPlacements.TopRight;
			base.HighlightedVisualElementID = "";
			base.MouseRequired = false;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x00003BC4 File Offset: 0x00001DC4
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
			return this._hasPlayerOrderedCharge;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x00003C3C File Offset: 0x00001E3C
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

		// Token: 0x060000FB RID: 251 RVA: 0x00003C9D File Offset: 0x00001E9D
		private void OnPlayerOrdered(OrderType orderType, IEnumerable<Formation> appliedFormations, OrderController orderController, params object[] delegateParams)
		{
			this._hasPlayerOrderedCharge = this._hasPlayerOrderedCharge || (orderType == OrderType.Charge && appliedFormations.Any<Formation>());
		}

		// Token: 0x060000FC RID: 252 RVA: 0x00003CBD File Offset: 0x00001EBD
		public override TutorialContexts GetTutorialsRelevantContext()
		{
			return TutorialContexts.Mission;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00003CC0 File Offset: 0x00001EC0
		public override bool IsConditionsMetForActivation()
		{
			return TutorialHelper.CurrentContext == TutorialContexts.Mission && TutorialHelper.IsPlayerInABattleMission && Mission.Current.Mode != MissionMode.Deployment && TutorialHelper.IsOrderingAvailable;
		}

		// Token: 0x0400003B RID: 59
		private bool _hasPlayerOrderedCharge;

		// Token: 0x0400003C RID: 60
		private bool _registeredToOrderEvent;
	}
}
