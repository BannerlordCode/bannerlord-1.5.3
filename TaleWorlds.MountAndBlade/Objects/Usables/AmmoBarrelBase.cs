using System;
using System.Linq;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade.Objects.Usables
{
	// Token: 0x020003AF RID: 943
	public abstract class AmmoBarrelBase : UsableMachine
	{
		// Token: 0x170009FD RID: 2557
		// (get) Token: 0x060035DC RID: 13788 RVA: 0x000DE331 File Offset: 0x000DC531
		private int PickupSoundFromBarrelCache
		{
			get
			{
				if (this._pickupSoundFromBarrel == -1)
				{
					this._pickupSoundFromBarrel = this.GetSoundEvent();
				}
				return this._pickupSoundFromBarrel;
			}
		}

		// Token: 0x060035DD RID: 13789 RVA: 0x000DE34E File Offset: 0x000DC54E
		public AmmoBarrelBase()
		{
			this._requiredWeaponClasses = this.GetRequiredWeaponClasses();
		}

		// Token: 0x060035DE RID: 13790 RVA: 0x000DE370 File Offset: 0x000DC570
		protected internal override void OnInit()
		{
			base.OnInit();
			foreach (StandingPoint standingPoint in base.StandingPoints)
			{
				(standingPoint as StandingPointWithWeaponRequirement).InitRequiredWeaponClasses(this._requiredWeaponClasses);
			}
			base.SetScriptComponentToTick(this.GetTickRequirement());
			this.MakeVisibilityCheck = false;
			this._isVisible = base.GameEntity.IsVisibleIncludeParents();
		}

		// Token: 0x060035DF RID: 13791
		protected abstract WeaponClass[] GetRequiredWeaponClasses();

		// Token: 0x060035E0 RID: 13792 RVA: 0x000DE3F8 File Offset: 0x000DC5F8
		public override void OnDeploymentFinished()
		{
			if (base.StandingPoints != null)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					standingPoint.LockUserFrames = false;
				}
			}
		}

		// Token: 0x060035E1 RID: 13793
		protected abstract int GetSoundEvent();

		// Token: 0x060035E2 RID: 13794 RVA: 0x000DE454 File Offset: 0x000DC654
		public override TextObject GetActionTextForStandingPoint(UsableMissionObject usableGameObject)
		{
			TextObject textObject = new TextObject("{=bNYm3K6b}{KEY} Pick Up", null);
			textObject.SetTextVariable("KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f));
			return textObject;
		}

		// Token: 0x060035E3 RID: 13795
		public abstract override TextObject GetDescriptionText(WeakGameEntity gameEntity);

		// Token: 0x060035E4 RID: 13796 RVA: 0x000DE483 File Offset: 0x000DC683
		public override ScriptComponentBehavior.TickRequirement GetTickRequirement()
		{
			if (GameNetwork.IsClientOrReplay)
			{
				return base.GetTickRequirement();
			}
			return ScriptComponentBehavior.TickRequirement.Tick | ScriptComponentBehavior.TickRequirement.TickParallel | base.GetTickRequirement();
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x000DE49B File Offset: 0x000DC69B
		protected internal override void OnTickParallel(float dt)
		{
			this.TickAux(true);
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x000DE4A4 File Offset: 0x000DC6A4
		protected internal override void OnTick(float dt)
		{
			base.OnTick(dt);
			if (this._needsSingleThreadTickOnce)
			{
				this._needsSingleThreadTickOnce = false;
				this.TickAux(false);
			}
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x000DE4C4 File Offset: 0x000DC6C4
		private void TickAux(bool isParallel)
		{
			if (this._isVisible && !GameNetwork.IsClientOrReplay)
			{
				foreach (StandingPoint standingPoint in base.StandingPoints)
				{
					if (standingPoint.HasUser)
					{
						Agent userAgent = standingPoint.UserAgent;
						ActionIndexCache currentAction = userAgent.GetCurrentAction(0);
						ActionIndexCache currentAction2 = userAgent.GetCurrentAction(1);
						if (!(currentAction2 == ActionIndexCache.act_none) || (!(currentAction == ActionIndexCache.act_pickup_down_begin) && !(currentAction == ActionIndexCache.act_pickup_down_begin_left_stance)))
						{
							if (currentAction2 == ActionIndexCache.act_none && (currentAction == ActionIndexCache.act_pickup_down_end || currentAction == ActionIndexCache.act_pickup_down_end_left_stance))
							{
								if (isParallel)
								{
									this._needsSingleThreadTickOnce = true;
								}
								else
								{
									for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
									{
										if (!userAgent.Equipment[equipmentIndex].IsEmpty && this._requiredWeaponClasses.Contains(userAgent.Equipment[equipmentIndex].CurrentUsageItem.WeaponClass) && userAgent.Equipment[equipmentIndex].Amount < userAgent.Equipment[equipmentIndex].ModifiedMaxAmount)
										{
											userAgent.SetWeaponAmountInSlot(equipmentIndex, userAgent.Equipment[equipmentIndex].ModifiedMaxAmount, true);
											Mission.Current.MakeSoundOnlyOnRelatedPeer(this.PickupSoundFromBarrelCache, userAgent.Position, userAgent.Index);
										}
									}
									userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
								}
							}
							else
							{
								if (!(currentAction2 != ActionIndexCache.act_none))
								{
									Agent agent = userAgent;
									int num = 0;
									ActionIndexCache actionIndexCache = (userAgent.GetIsLeftStance() ? ActionIndexCache.act_pickup_down_begin_left_stance : ActionIndexCache.act_pickup_down_begin);
									if (agent.SetActionChannel(num, in actionIndexCache, false, (AnimFlags)0UL, 0f, 1f, -0.2f, 0.4f, 0f, false, -0.2f, 0, true))
									{
										continue;
									}
								}
								if (isParallel)
								{
									this._needsSingleThreadTickOnce = true;
								}
								else
								{
									userAgent.StopUsingGameObject(true, Agent.StopUsingGameObjectFlags.AutoAttachAfterStoppingUsingGameObject);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x000DE6F8 File Offset: 0x000DC8F8
		public override OrderType GetOrder(BattleSideEnum side)
		{
			return OrderType.None;
		}

		// Token: 0x040016F6 RID: 5878
		private readonly WeaponClass[] _requiredWeaponClasses;

		// Token: 0x040016F7 RID: 5879
		private int _pickupSoundFromBarrel = -1;

		// Token: 0x040016F8 RID: 5880
		private bool _isVisible = true;

		// Token: 0x040016F9 RID: 5881
		private bool _needsSingleThreadTickOnce;
	}
}
