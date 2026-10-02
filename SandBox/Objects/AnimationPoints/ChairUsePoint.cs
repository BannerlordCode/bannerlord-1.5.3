using System;
using System.Collections.Generic;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace SandBox.Objects.AnimationPoints
{
	// Token: 0x02000053 RID: 83
	public class ChairUsePoint : AnimationPoint
	{
		// Token: 0x06000336 RID: 822 RVA: 0x00012C58 File Offset: 0x00010E58
		protected override void SetActionCodes()
		{
			base.SetActionCodes();
			this._loopAction = ActionIndexCache.Create(this.LoopStartAction);
			this._pairLoopAction = ActionIndexCache.Create(this.PairLoopStartAction);
			this._nearTableLoopAction = ActionIndexCache.Create(this.NearTableLoopAction);
			this._nearTablePairLoopAction = ActionIndexCache.Create(this.NearTablePairLoopAction);
			this._drinkLoopAction = ActionIndexCache.Create(this.DrinkLoopAction);
			this._drinkPairLoopAction = ActionIndexCache.Create(this.DrinkPairLoopAction);
			this._eatLoopAction = ActionIndexCache.Create(this.EatLoopAction);
			this._eatPairLoopAction = ActionIndexCache.Create(this.EatPairLoopAction);
			this.SetChairAction(this.GetRandomChairAction());
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00012D00 File Offset: 0x00010F00
		protected override bool ShouldUpdateOnEditorVariableChanged(string variableName)
		{
			return base.ShouldUpdateOnEditorVariableChanged(variableName) || variableName == "NearTable" || variableName == "Drink" || variableName == "Eat" || variableName == "NearTableLoopAction" || variableName == "DrinkLoopAction" || variableName == "EatLoopAction";
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00012D64 File Offset: 0x00010F64
		public override void OnUse(Agent userAgent, sbyte agentBoneIndex)
		{
			ChairUsePoint.ChairAction chairAction = (base.CanAgentUseItem(userAgent) ? this.GetRandomChairAction() : ChairUsePoint.ChairAction.None);
			this.SetChairAction(chairAction);
			base.OnUse(userAgent, agentBoneIndex);
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00012D94 File Offset: 0x00010F94
		private ChairUsePoint.ChairAction GetRandomChairAction()
		{
			List<ChairUsePoint.ChairAction> list = new List<ChairUsePoint.ChairAction> { ChairUsePoint.ChairAction.None };
			if (this.NearTable && this._nearTableLoopAction != ActionIndexCache.act_none)
			{
				list.Add(ChairUsePoint.ChairAction.LeanOnTable);
			}
			if (this.Drink && this._drinkLoopAction != ActionIndexCache.act_none)
			{
				list.Add(ChairUsePoint.ChairAction.Drink);
			}
			if (this.Eat && this._eatLoopAction != ActionIndexCache.act_none)
			{
				list.Add(ChairUsePoint.ChairAction.Eat);
			}
			return list[new Random().Next(list.Count)];
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00012E28 File Offset: 0x00011028
		private void SetChairAction(ChairUsePoint.ChairAction chairAction)
		{
			switch (chairAction)
			{
			case ChairUsePoint.ChairAction.None:
				this.LoopStartActionCode = this._loopAction;
				this.PairLoopStartActionCode = this._pairLoopAction;
				base.SelectedRightHandItem = this.RightHandItem;
				base.SelectedLeftHandItem = this.LeftHandItem;
				return;
			case ChairUsePoint.ChairAction.LeanOnTable:
				this.LoopStartActionCode = this._nearTableLoopAction;
				this.PairLoopStartActionCode = this._nearTablePairLoopAction;
				base.SelectedRightHandItem = string.Empty;
				base.SelectedLeftHandItem = string.Empty;
				return;
			case ChairUsePoint.ChairAction.Drink:
				this.LoopStartActionCode = this._drinkLoopAction;
				this.PairLoopStartActionCode = this._drinkPairLoopAction;
				base.SelectedRightHandItem = this.DrinkRightHandItem;
				base.SelectedLeftHandItem = this.DrinkLeftHandItem;
				return;
			case ChairUsePoint.ChairAction.Eat:
				this.LoopStartActionCode = this._eatLoopAction;
				this.PairLoopStartActionCode = this._eatPairLoopAction;
				base.SelectedRightHandItem = this.EatRightHandItem;
				base.SelectedLeftHandItem = this.EatLeftHandItem;
				return;
			default:
				return;
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00012F10 File Offset: 0x00011110
		protected override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (base.UserAgent != null && !base.UserAgent.IsAIControlled && base.UserAgent.EventControlFlags.HasAnyFlag(Agent.EventControlFlag.Crouch | Agent.EventControlFlag.Stand))
			{
				base.UserAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
			}
		}

		// Token: 0x0400017F RID: 383
		public bool NearTable;

		// Token: 0x04000180 RID: 384
		public string NearTableLoopAction = "";

		// Token: 0x04000181 RID: 385
		public string NearTablePairLoopAction = "";

		// Token: 0x04000182 RID: 386
		public bool Drink;

		// Token: 0x04000183 RID: 387
		public string DrinkLoopAction = "";

		// Token: 0x04000184 RID: 388
		public string DrinkPairLoopAction = "";

		// Token: 0x04000185 RID: 389
		public string DrinkRightHandItem = "";

		// Token: 0x04000186 RID: 390
		public string DrinkLeftHandItem = "";

		// Token: 0x04000187 RID: 391
		public bool Eat;

		// Token: 0x04000188 RID: 392
		public string EatLoopAction = "";

		// Token: 0x04000189 RID: 393
		public string EatPairLoopAction = "";

		// Token: 0x0400018A RID: 394
		public string EatRightHandItem = "";

		// Token: 0x0400018B RID: 395
		public string EatLeftHandItem = "";

		// Token: 0x0400018C RID: 396
		private ActionIndexCache _loopAction;

		// Token: 0x0400018D RID: 397
		private ActionIndexCache _pairLoopAction;

		// Token: 0x0400018E RID: 398
		private ActionIndexCache _nearTableLoopAction;

		// Token: 0x0400018F RID: 399
		private ActionIndexCache _nearTablePairLoopAction;

		// Token: 0x04000190 RID: 400
		private ActionIndexCache _drinkLoopAction;

		// Token: 0x04000191 RID: 401
		private ActionIndexCache _drinkPairLoopAction;

		// Token: 0x04000192 RID: 402
		private ActionIndexCache _eatLoopAction;

		// Token: 0x04000193 RID: 403
		private ActionIndexCache _eatPairLoopAction;

		// Token: 0x02000162 RID: 354
		private enum ChairAction
		{
			// Token: 0x040006D3 RID: 1747
			None,
			// Token: 0x040006D4 RID: 1748
			LeanOnTable,
			// Token: 0x040006D5 RID: 1749
			Drink,
			// Token: 0x040006D6 RID: 1750
			Eat
		}
	}
}
