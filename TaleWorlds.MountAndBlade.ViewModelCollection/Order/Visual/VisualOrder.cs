using System;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual
{
	// Token: 0x02000029 RID: 41
	public abstract class VisualOrder
	{
		// Token: 0x170000FD RID: 253
		// (get) Token: 0x06000336 RID: 822 RVA: 0x0000BE7D File Offset: 0x0000A07D
		public string StringId { get; }

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000BE85 File Offset: 0x0000A085
		public string IconId
		{
			get
			{
				return this.GetIconId();
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x0000BE8D File Offset: 0x0000A08D
		public VisualOrder(string stringId)
		{
			this.StringId = stringId;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x0000BE9C File Offset: 0x0000A09C
		protected virtual string GetIconId()
		{
			return this.StringId;
		}

		// Token: 0x0600033A RID: 826
		public abstract TextObject GetName(OrderController orderController);

		// Token: 0x0600033B RID: 827
		public abstract bool IsTargeted();

		// Token: 0x0600033C RID: 828
		public abstract void ExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters);

		// Token: 0x0600033D RID: 829 RVA: 0x0000BEA4 File Offset: 0x0000A0A4
		public virtual void BeforeExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0000BEA6 File Offset: 0x0000A0A6
		public virtual void AfterExecuteOrder(OrderController orderController, VisualOrderExecutionParameters executionParameters)
		{
		}

		// Token: 0x0600033F RID: 831
		protected abstract bool? OnGetFormationHasOrder(Formation formation);

		// Token: 0x06000340 RID: 832 RVA: 0x0000BEA8 File Offset: 0x0000A0A8
		public bool GetFormationHasOrder(Formation formation)
		{
			bool? flag = this.OnGetFormationHasOrder(formation);
			bool flag2 = true;
			return (flag.GetValueOrDefault() == flag2) & (flag != null);
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0000BED1 File Offset: 0x0000A0D1
		public OrderState GetActiveState(OrderController orderController)
		{
			this._lastActiveState = this.GetActiveStateAux(orderController);
			return this._lastActiveState;
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
		private OrderState GetActiveStateAux(OrderController orderController)
		{
			if (orderController.SelectedFormations == null || orderController.SelectedFormations.Count == 0)
			{
				return OrderState.Default;
			}
			int num = orderController.SelectedFormations.Count;
			int num2 = 0;
			MBReadOnlyList<Formation> selectedFormations = orderController.SelectedFormations;
			for (int i = 0; i < selectedFormations.Count; i++)
			{
				Formation formation = selectedFormations[i];
				bool? flag = this.OnGetFormationHasOrder(formation);
				if (flag == null)
				{
					num--;
				}
				else
				{
					bool? flag2 = flag;
					bool flag3 = true;
					if ((flag2.GetValueOrDefault() == flag3) & (flag2 != null))
					{
						num2++;
					}
				}
			}
			if (num2 == 0)
			{
				return OrderState.Default;
			}
			if (num2 < num)
			{
				return OrderState.PartiallyActive;
			}
			if (num2 == num)
			{
				return OrderState.Active;
			}
			return OrderState.Default;
		}

		// Token: 0x04000175 RID: 373
		protected OrderState _lastActiveState;
	}
}
