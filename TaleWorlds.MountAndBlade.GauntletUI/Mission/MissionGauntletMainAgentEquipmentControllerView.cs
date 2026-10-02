using System;
using NetworkMessages.FromClient;
using TaleWorlds.Core;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.MountAndBlade.ViewModelCollection;
using TaleWorlds.MountAndBlade.ViewModelCollection.HUD;

namespace TaleWorlds.MountAndBlade.GauntletUI.Mission
{
	// Token: 0x02000034 RID: 52
	[OverrideView(typeof(MissionMainAgentEquipmentControllerView))]
	public class MissionGauntletMainAgentEquipmentControllerView : MissionView
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000243 RID: 579 RVA: 0x0000D588 File Offset: 0x0000B788
		// (remove) Token: 0x06000244 RID: 580 RVA: 0x0000D5C0 File Offset: 0x0000B7C0
		public event Action<bool> OnEquipmentDropInteractionViewToggled;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000245 RID: 581 RVA: 0x0000D5F8 File Offset: 0x0000B7F8
		// (remove) Token: 0x06000246 RID: 582 RVA: 0x0000D630 File Offset: 0x0000B830
		public event Action<bool> OnEquipmentEquipInteractionViewToggled;

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x06000247 RID: 583 RVA: 0x0000D665 File Offset: 0x0000B865
		private bool IsDisplayingADialog
		{
			get
			{
				IMissionScreen missionScreenAsInterface = this._missionScreenAsInterface;
				return missionScreenAsInterface != null && missionScreenAsInterface.GetDisplayDialog();
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x06000248 RID: 584 RVA: 0x0000D678 File Offset: 0x0000B878
		// (set) Token: 0x06000249 RID: 585 RVA: 0x0000D680 File Offset: 0x0000B880
		private bool EquipHoldHandled
		{
			get
			{
				return this._equipHoldHandled;
			}
			set
			{
				this._equipHoldHandled = value;
				if (this._equipHoldHandled)
				{
					MissionScreen missionScreen = base.MissionScreen;
					if (missionScreen == null)
					{
						return;
					}
					missionScreen.RegisterRadialMenuObject<MissionGauntletMainAgentEquipmentControllerView>(this);
					return;
				}
				else
				{
					MissionScreen missionScreen2 = base.MissionScreen;
					if (missionScreen2 == null)
					{
						return;
					}
					missionScreen2.UnregisterRadialMenuObject(this);
					return;
				}
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600024A RID: 586 RVA: 0x0000D6B4 File Offset: 0x0000B8B4
		// (set) Token: 0x0600024B RID: 587 RVA: 0x0000D6BC File Offset: 0x0000B8BC
		private bool DropHoldHandled
		{
			get
			{
				return this._dropHoldHandled;
			}
			set
			{
				this._dropHoldHandled = value;
				if (this._dropHoldHandled)
				{
					MissionScreen missionScreen = base.MissionScreen;
					if (missionScreen == null)
					{
						return;
					}
					missionScreen.RegisterRadialMenuObject<MissionGauntletMainAgentEquipmentControllerView>(this);
					return;
				}
				else
				{
					MissionScreen missionScreen2 = base.MissionScreen;
					if (missionScreen2 == null)
					{
						return;
					}
					missionScreen2.UnregisterRadialMenuObject(this);
					return;
				}
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000D6F0 File Offset: 0x0000B8F0
		public MissionGauntletMainAgentEquipmentControllerView()
		{
			this._missionScreenAsInterface = base.MissionScreen;
			this.EquipHoldHandled = false;
			this.DropHoldHandled = false;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000D714 File Offset: 0x0000B914
		public override void OnMissionScreenInitialize()
		{
			base.OnMissionScreenInitialize();
			this._gauntletLayer = new GauntletLayer("MissionEquipmentController", this.ViewOrderPriority, false);
			this._dataSource = new MissionMainAgentEquipmentControllerVM(new Action<EquipmentIndex>(this.OnDropEquipment), new Action<SpawnedItemEntity, EquipmentIndex>(this.OnEquipItem));
			this._gauntletLayer.LoadMovie("MainAgentEquipmentController", this._dataSource);
			this._gauntletLayer.InputRestrictions.SetInputRestrictions(false, InputUsageMask.Invalid);
			base.MissionScreen.AddLayer(this._gauntletLayer);
			base.Mission.OnMainAgentChanged += this.OnMainAgentChanged;
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000D7B4 File Offset: 0x0000B9B4
		public override void OnMissionScreenFinalize()
		{
			base.OnMissionScreenFinalize();
			base.Mission.OnMainAgentChanged -= this.OnMainAgentChanged;
			base.MissionScreen.RemoveLayer(this._gauntletLayer);
			this._gauntletLayer = null;
			this._dataSource.OnFinalize();
			this._dataSource = null;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000D808 File Offset: 0x0000BA08
		public override void OnMissionScreenTick(float dt)
		{
			base.OnMissionScreenTick(dt);
			if (this.IsMainAgentAvailable() && base.Mission.IsMainAgentItemInteractionEnabled)
			{
				this.DropWeaponTick(dt);
				this.EquipWeaponTick(dt);
				return;
			}
			this._prevDropKeyDown = false;
			this._prevEquipKeyDown = false;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000D844 File Offset: 0x0000BA44
		public override void OnFocusGained(Agent agent, IFocusable focusableObject, bool isInteractable)
		{
			base.OnFocusGained(agent, focusableObject, isInteractable);
			UsableMissionObject usableMissionObject;
			SpawnedItemEntity spawnedItemEntity;
			if ((usableMissionObject = focusableObject as UsableMissionObject) != null && (spawnedItemEntity = usableMissionObject as SpawnedItemEntity) != null)
			{
				this._isCurrentFocusedItemInteractable = isInteractable;
				if (!spawnedItemEntity.WeaponCopy.IsEmpty)
				{
					this._isFocusedOnEquipment = true;
					this._focusedWeaponItem = spawnedItemEntity;
					this._dataSource.SetCurrentFocusedWeaponEntity(this._focusedWeaponItem);
				}
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000D8A4 File Offset: 0x0000BAA4
		public override void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			base.OnFocusLost(agent, focusableObject);
			this._isCurrentFocusedItemInteractable = false;
			this._isFocusedOnEquipment = false;
			this._focusedWeaponItem = null;
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.SetCurrentFocusedWeaponEntity(this._focusedWeaponItem);
			}
			if (this.EquipHoldHandled)
			{
				this.EquipHoldHandled = false;
				this._equipHoldTime = 0f;
				MissionMainAgentEquipmentControllerVM dataSource2 = this._dataSource;
				if (dataSource2 != null)
				{
					dataSource2.OnCancelEquipController();
				}
				Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
				if (onEquipmentEquipInteractionViewToggled != null)
				{
					onEquipmentEquipInteractionViewToggled(false);
				}
				this._equipmentWasInFocusFirstFrameOfEquipDown = false;
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0000D92C File Offset: 0x0000BB2C
		private void OnMainAgentChanged(Agent oldAgent)
		{
			if (base.Mission.MainAgent == null)
			{
				if (this.EquipHoldHandled)
				{
					this.EquipHoldHandled = false;
					Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
					if (onEquipmentEquipInteractionViewToggled != null)
					{
						onEquipmentEquipInteractionViewToggled(false);
					}
				}
				this._equipHoldTime = 0f;
				this._dataSource.OnCancelEquipController();
				if (this.DropHoldHandled)
				{
					Action<bool> onEquipmentDropInteractionViewToggled = this.OnEquipmentDropInteractionViewToggled;
					if (onEquipmentDropInteractionViewToggled != null)
					{
						onEquipmentDropInteractionViewToggled(false);
					}
					this.DropHoldHandled = false;
				}
				this._dropHoldTime = 0f;
				this._dataSource.OnCancelDropController();
			}
		}

		// Token: 0x06000253 RID: 595 RVA: 0x0000D9B4 File Offset: 0x0000BBB4
		private void EquipWeaponTick(float dt)
		{
			if (base.MissionScreen.SceneLayer.Input.IsGameKeyDown(13) && !this._prevDropKeyDown && !this.IsDisplayingADialog && this.IsMainAgentAvailable() && !base.MissionScreen.Mission.IsOrderMenuOpen)
			{
				if (!this._firstFrameOfEquipDownHandled)
				{
					this._equipmentWasInFocusFirstFrameOfEquipDown = this._isFocusedOnEquipment;
					this._firstFrameOfEquipDownHandled = true;
				}
				if (this._equipmentWasInFocusFirstFrameOfEquipDown)
				{
					this._equipHoldTime += dt;
					if (this._equipHoldTime > 0.5f && !this.EquipHoldHandled && this._isFocusedOnEquipment && this._isCurrentFocusedItemInteractable)
					{
						this.HandleOpeningHoldEquip();
						this.EquipHoldHandled = true;
					}
				}
				this._prevEquipKeyDown = true;
				return;
			}
			if (this._prevEquipKeyDown && !base.MissionScreen.SceneLayer.Input.IsGameKeyDown(13))
			{
				if (this._equipmentWasInFocusFirstFrameOfEquipDown)
				{
					if (this._equipHoldTime < 0.5f)
					{
						if (this._focusedWeaponItem != null)
						{
							Agent main = Agent.Main;
							if (main != null && main.CanQuickPickUp(this._focusedWeaponItem))
							{
								this.HandleQuickReleaseEquip();
							}
						}
					}
					else
					{
						this.HandleClosingHoldEquip();
					}
				}
				if (this.EquipHoldHandled)
				{
					this.EquipHoldHandled = false;
				}
				this._equipHoldTime = 0f;
				this._firstFrameOfEquipDownHandled = false;
				this._prevEquipKeyDown = false;
			}
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000DB08 File Offset: 0x0000BD08
		private void DropWeaponTick(float dt)
		{
			if (base.MissionScreen.SceneLayer.Input.IsGameKeyDown(22) && !this._prevEquipKeyDown && !this.IsDisplayingADialog && this.IsMainAgentAvailable() && this.IsMainAgentHasAtLeastOneItem() && !base.MissionScreen.Mission.IsOrderMenuOpen)
			{
				this._dropHoldTime += dt;
				if (this._dropHoldTime > 0.5f && !this.DropHoldHandled)
				{
					this.HandleOpeningHoldDrop();
					this.DropHoldHandled = true;
				}
				this._prevDropKeyDown = true;
				return;
			}
			if (this._prevDropKeyDown && !base.MissionScreen.SceneLayer.Input.IsGameKeyDown(22))
			{
				if (this._dropHoldTime < 0.5f)
				{
					this.HandleQuickReleaseDrop();
				}
				else
				{
					this.HandleClosingHoldDrop();
				}
				this.DropHoldHandled = false;
				this._dropHoldTime = 0f;
				this._prevDropKeyDown = false;
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000DBED File Offset: 0x0000BDED
		private void HandleOpeningHoldEquip()
		{
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnEquipControllerToggle(true);
			}
			Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
			if (onEquipmentEquipInteractionViewToggled == null)
			{
				return;
			}
			onEquipmentEquipInteractionViewToggled(true);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0000DC12 File Offset: 0x0000BE12
		private void HandleClosingHoldEquip()
		{
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnEquipControllerToggle(false);
			}
			Action<bool> onEquipmentEquipInteractionViewToggled = this.OnEquipmentEquipInteractionViewToggled;
			if (onEquipmentEquipInteractionViewToggled == null)
			{
				return;
			}
			onEquipmentEquipInteractionViewToggled(false);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0000DC37 File Offset: 0x0000BE37
		private void HandleQuickReleaseEquip()
		{
			this.OnEquipItem(this._focusedWeaponItem, EquipmentIndex.None);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000DC46 File Offset: 0x0000BE46
		private void HandleOpeningHoldDrop()
		{
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnDropControllerToggle(true);
			}
			Action<bool> onEquipmentDropInteractionViewToggled = this.OnEquipmentDropInteractionViewToggled;
			if (onEquipmentDropInteractionViewToggled == null)
			{
				return;
			}
			onEquipmentDropInteractionViewToggled(true);
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000DC6B File Offset: 0x0000BE6B
		private void HandleClosingHoldDrop()
		{
			MissionMainAgentEquipmentControllerVM dataSource = this._dataSource;
			if (dataSource != null)
			{
				dataSource.OnDropControllerToggle(false);
			}
			Action<bool> onEquipmentDropInteractionViewToggled = this.OnEquipmentDropInteractionViewToggled;
			if (onEquipmentDropInteractionViewToggled == null)
			{
				return;
			}
			onEquipmentDropInteractionViewToggled(false);
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000DC90 File Offset: 0x0000BE90
		private void HandleQuickReleaseDrop()
		{
			this.OnDropEquipment(EquipmentIndex.None);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000DC9C File Offset: 0x0000BE9C
		private void OnEquipItem(SpawnedItemEntity itemToEquip, EquipmentIndex indexToEquipItTo)
		{
			if (itemToEquip.GameEntity.IsValid)
			{
				Agent main = Agent.Main;
				if (main == null)
				{
					return;
				}
				main.HandleStartUsingAction(itemToEquip, (int)indexToEquipItTo);
			}
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000DCCC File Offset: 0x0000BECC
		private void OnDropEquipment(EquipmentIndex indexToDrop)
		{
			if (GameNetwork.IsClient)
			{
				GameNetwork.BeginModuleEventAsClient();
				GameNetwork.WriteMessage(new DropWeapon(base.Input.IsGameKeyDown(10), indexToDrop));
				GameNetwork.EndModuleEventAsClient();
				return;
			}
			Agent.Main.HandleDropWeapon(base.Input.IsGameKeyDown(10), indexToDrop);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000DD1B File Offset: 0x0000BF1B
		private bool IsMainAgentAvailable()
		{
			Agent main = Agent.Main;
			return main != null && main.IsActive();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000DD30 File Offset: 0x0000BF30
		private bool IsMainAgentHasAtLeastOneItem()
		{
			for (EquipmentIndex equipmentIndex = EquipmentIndex.WeaponItemBeginSlot; equipmentIndex < EquipmentIndex.NumAllWeaponSlots; equipmentIndex++)
			{
				if (!Agent.Main.Equipment[equipmentIndex].IsEmpty)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000DD66 File Offset: 0x0000BF66
		public override void OnPhotoModeActivated()
		{
			base.OnPhotoModeActivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 0f;
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000DD8B File Offset: 0x0000BF8B
		public override void OnPhotoModeDeactivated()
		{
			base.OnPhotoModeDeactivated();
			if (this._gauntletLayer != null)
			{
				this._gauntletLayer.UIContext.ContextAlpha = 1f;
			}
		}

		// Token: 0x04000129 RID: 297
		private const float _minHoldTime = 0.5f;

		// Token: 0x0400012C RID: 300
		private readonly IMissionScreen _missionScreenAsInterface;

		// Token: 0x0400012D RID: 301
		private bool _equipmentWasInFocusFirstFrameOfEquipDown;

		// Token: 0x0400012E RID: 302
		private bool _firstFrameOfEquipDownHandled;

		// Token: 0x0400012F RID: 303
		private bool _equipHoldHandled;

		// Token: 0x04000130 RID: 304
		private bool _isFocusedOnEquipment;

		// Token: 0x04000131 RID: 305
		private float _equipHoldTime;

		// Token: 0x04000132 RID: 306
		private bool _prevEquipKeyDown;

		// Token: 0x04000133 RID: 307
		private SpawnedItemEntity _focusedWeaponItem;

		// Token: 0x04000134 RID: 308
		private bool _dropHoldHandled;

		// Token: 0x04000135 RID: 309
		private float _dropHoldTime;

		// Token: 0x04000136 RID: 310
		private bool _prevDropKeyDown;

		// Token: 0x04000137 RID: 311
		private bool _isCurrentFocusedItemInteractable;

		// Token: 0x04000138 RID: 312
		private GauntletLayer _gauntletLayer;

		// Token: 0x04000139 RID: 313
		private MissionMainAgentEquipmentControllerVM _dataSource;
	}
}
