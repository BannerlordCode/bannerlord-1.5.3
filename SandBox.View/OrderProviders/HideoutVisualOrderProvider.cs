using System;
using SandBox.Missions.MissionLogics.Hideout;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View.VisualOrders.Orders;
using TaleWorlds.MountAndBlade.View.VisualOrders.Orders.ToggleOrders;
using TaleWorlds.MountAndBlade.View.VisualOrders.OrderSets;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.FormOrders;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.MovementOrders;
using TaleWorlds.MountAndBlade.ViewModelCollection.Order.Visual.Default.Orders.ToggleOrders;

namespace SandBox.View.OrderProviders
{
	// Token: 0x02000011 RID: 17
	internal class HideoutVisualOrderProvider : VisualOrderProvider
	{
		// Token: 0x06000075 RID: 117 RVA: 0x00004448 File Offset: 0x00002648
		public override bool IsAvailable()
		{
			Mission mission = Mission.Current;
			return mission != null && mission.HasMissionBehavior<HideoutMissionController>();
		}

		// Token: 0x06000076 RID: 118 RVA: 0x0000445A File Offset: 0x0000265A
		public override MBReadOnlyList<VisualOrderSet> GetOrders()
		{
			if (BannerlordConfig.OrderLayoutType == 1)
			{
				return this.GetLegacyOrders();
			}
			return this.GetDefaultOrders();
		}

		// Token: 0x06000077 RID: 119 RVA: 0x00004474 File Offset: 0x00002674
		private MBReadOnlyList<VisualOrderSet> GetDefaultOrders()
		{
			MBList<VisualOrderSet> mblist = new MBList<VisualOrderSet>();
			GenericVisualOrderSet genericVisualOrderSet = new GenericVisualOrderSet("order_type_movement", new TextObject("{=KiJd6Xik}Movement", null), true, true);
			genericVisualOrderSet.AddOrder(new MoveVisualOrder("order_movement_move"));
			genericVisualOrderSet.AddOrder(new FollowMeVisualOrder("order_movement_follow"));
			genericVisualOrderSet.AddOrder(new ChargeVisualOrder("order_movement_charge"));
			genericVisualOrderSet.AddOrder(new FallbackVisualOrder("order_movement_fallback"));
			genericVisualOrderSet.AddOrder(new StopVisualOrder("order_movement_stop"));
			genericVisualOrderSet.AddOrder(new RetreatVisualOrder("order_movement_retreat"));
			genericVisualOrderSet.AddOrder(new ReturnVisualOrder());
			GenericVisualOrderSet genericVisualOrderSet2 = new GenericVisualOrderSet("order_type_form", new TextObject("{=iBk2wbn3}Form", null), true, true);
			ArrangementVisualOrder arrangementVisualOrder = new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Line, "order_form_line");
			ArrangementVisualOrder arrangementVisualOrder2 = new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.ShieldWall, "order_form_close");
			genericVisualOrderSet2.AddOrder(arrangementVisualOrder);
			genericVisualOrderSet2.AddOrder(arrangementVisualOrder2);
			genericVisualOrderSet2.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Loose, "order_form_loose"));
			genericVisualOrderSet2.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Circle, "order_form_circular"));
			genericVisualOrderSet2.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Square, "order_form_schiltron"));
			genericVisualOrderSet2.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Skein, "order_form_v"));
			genericVisualOrderSet2.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Column, "order_form_column"));
			genericVisualOrderSet2.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Scatter, "order_form_scatter"));
			genericVisualOrderSet2.AddOrder(new ReturnVisualOrder());
			GenericVisualOrderSet genericVisualOrderSet3 = new GenericVisualOrderSet("order_type_toggle", new TextObject("{=0HTNYQz2}Toggle", null), false, false);
			ToggleFacingVisualOrder toggleFacingVisualOrder = new ToggleFacingVisualOrder("order_toggle_facing");
			GenericToggleVisualOrder genericToggleVisualOrder = new GenericToggleVisualOrder("order_toggle_fire", OrderType.FireAtWill, OrderType.HoldFire);
			GenericToggleVisualOrder genericToggleVisualOrder2 = new GenericToggleVisualOrder("order_toggle_mount", OrderType.Mount, OrderType.Dismount);
			GenericToggleVisualOrder genericToggleVisualOrder3 = (GameNetwork.IsMultiplayer ? null : new GenericToggleVisualOrder("order_toggle_ai", OrderType.AIControlOn, OrderType.AIControlOff));
			TransferTroopsVisualOrder transferTroopsVisualOrder = (GameNetwork.IsMultiplayer ? null : new TransferTroopsVisualOrder());
			genericVisualOrderSet3.AddOrder(toggleFacingVisualOrder);
			genericVisualOrderSet3.AddOrder(genericToggleVisualOrder);
			genericVisualOrderSet3.AddOrder(genericToggleVisualOrder2);
			if (genericToggleVisualOrder3 != null)
			{
				genericVisualOrderSet3.AddOrder(genericToggleVisualOrder3);
			}
			if (transferTroopsVisualOrder != null)
			{
				genericVisualOrderSet3.AddOrder(transferTroopsVisualOrder);
			}
			genericVisualOrderSet3.AddOrder(new ReturnVisualOrder());
			mblist.Add(genericVisualOrderSet);
			mblist.Add(genericVisualOrderSet2);
			mblist.Add(genericVisualOrderSet3);
			if (!Input.IsGamepadActive)
			{
				mblist.Add(new SingleVisualOrderSet(genericToggleVisualOrder));
				mblist.Add(new SingleVisualOrderSet(genericToggleVisualOrder2));
				if (genericToggleVisualOrder3 != null)
				{
					mblist.Add(new SingleVisualOrderSet(genericToggleVisualOrder3));
				}
				mblist.Add(new SingleVisualOrderSet(toggleFacingVisualOrder));
				mblist.Add(new SingleVisualOrderSet(arrangementVisualOrder2));
				mblist.Add(new SingleVisualOrderSet(arrangementVisualOrder));
			}
			return mblist;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000046DC File Offset: 0x000028DC
		private MBList<VisualOrderSet> GetLegacyOrders()
		{
			MBList<VisualOrderSet> mblist = new MBList<VisualOrderSet>();
			GenericVisualOrderSet genericVisualOrderSet = new GenericVisualOrderSet("order_type_movement", new TextObject("{=KiJd6Xik}Movement", null), true, false);
			genericVisualOrderSet.AddOrder(new MoveVisualOrder("order_movement_move"));
			genericVisualOrderSet.AddOrder(new FollowMeVisualOrder("order_movement_follow"));
			genericVisualOrderSet.AddOrder(new ChargeVisualOrder("order_movement_charge"));
			genericVisualOrderSet.AddOrder(new FallbackVisualOrder("order_movement_fallback"));
			genericVisualOrderSet.AddOrder(new StopVisualOrder("order_movement_stop"));
			genericVisualOrderSet.AddOrder(new RetreatVisualOrder("order_movement_retreat"));
			genericVisualOrderSet.AddOrder(new ReturnVisualOrder());
			GenericVisualOrderSet genericVisualOrderSet2 = new GenericVisualOrderSet("order_type_facing", new TextObject("{=psynaDsM}Facing", null), true, false);
			SingleVisualOrder singleVisualOrder = new SingleVisualOrder("order_toggle_facing", new TextObject("{=MH9Pi3ao}Face Direction", null), OrderType.LookAtDirection, false, true);
			SingleVisualOrder singleVisualOrder2 = new SingleVisualOrder("order_toggle_facing_active", new TextObject("{=u8j8nN5U}Face Enemy", null), OrderType.LookAtEnemy, false, false);
			genericVisualOrderSet2.AddOrder(singleVisualOrder);
			genericVisualOrderSet2.AddOrder(singleVisualOrder2);
			GenericVisualOrderSet genericVisualOrderSet3 = new GenericVisualOrderSet("order_type_form", new TextObject("{=iBk2wbn3}Form", null), true, true);
			ArrangementVisualOrder arrangementVisualOrder = new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Line, "order_form_line");
			ArrangementVisualOrder arrangementVisualOrder2 = new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.ShieldWall, "order_form_close");
			genericVisualOrderSet3.AddOrder(arrangementVisualOrder);
			genericVisualOrderSet3.AddOrder(arrangementVisualOrder2);
			genericVisualOrderSet3.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Loose, "order_form_loose"));
			genericVisualOrderSet3.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Circle, "order_form_circular"));
			genericVisualOrderSet3.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Square, "order_form_schiltron"));
			genericVisualOrderSet3.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Skein, "order_form_v"));
			genericVisualOrderSet3.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Column, "order_form_column"));
			genericVisualOrderSet3.AddOrder(new ArrangementVisualOrder(ArrangementOrder.ArrangementOrderEnum.Scatter, "order_form_scatter"));
			genericVisualOrderSet3.AddOrder(new ReturnVisualOrder());
			mblist.Add(genericVisualOrderSet);
			mblist.Add(genericVisualOrderSet2);
			mblist.Add(genericVisualOrderSet3);
			GenericToggleVisualOrder genericToggleVisualOrder = new GenericToggleVisualOrder("order_toggle_fire", OrderType.FireAtWill, OrderType.HoldFire);
			GenericToggleVisualOrder genericToggleVisualOrder2 = new GenericToggleVisualOrder("order_toggle_mount", OrderType.Mount, OrderType.Dismount);
			GenericToggleVisualOrder genericToggleVisualOrder3 = (GameNetwork.IsMultiplayer ? null : new GenericToggleVisualOrder("order_toggle_ai", OrderType.AIControlOn, OrderType.AIControlOff));
			TransferTroopsVisualOrder transferTroopsVisualOrder = (GameNetwork.IsMultiplayer ? null : new TransferTroopsVisualOrder());
			if (!Input.IsGamepadActive)
			{
				mblist.Add(new SingleVisualOrderSet(genericToggleVisualOrder));
				mblist.Add(new SingleVisualOrderSet(genericToggleVisualOrder2));
				if (genericToggleVisualOrder3 != null)
				{
					mblist.Add(new SingleVisualOrderSet(genericToggleVisualOrder3));
				}
				if (transferTroopsVisualOrder != null)
				{
					mblist.Add(new SingleVisualOrderSet(transferTroopsVisualOrder));
				}
				mblist.Add(new SingleVisualOrderSet(new ReturnVisualOrder()));
			}
			return mblist;
		}
	}
}
