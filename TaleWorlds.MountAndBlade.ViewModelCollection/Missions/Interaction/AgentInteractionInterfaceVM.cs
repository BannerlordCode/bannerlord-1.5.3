using System;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Missions.Hints;
using TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction.InteractionItems;

namespace TaleWorlds.MountAndBlade.ViewModelCollection.Missions.Interaction
{
	// Token: 0x02000040 RID: 64
	public class AgentInteractionInterfaceVM : ViewModel
	{
		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06000580 RID: 1408 RVA: 0x00014F35 File Offset: 0x00013135
		private bool IsPlayerActive
		{
			get
			{
				Agent main = Agent.Main;
				return main != null && main.Health > 0f;
			}
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x00014F50 File Offset: 0x00013150
		public AgentInteractionInterfaceVM(Mission mission)
		{
			this._mission = mission;
			this.IsActive = false;
			this.PrimaryInteractionMessages = new MBBindingList<MissionPrimaryInteractionItemVM>
			{
				new MissionPrimaryInteractionItemVM(),
				new MissionPrimaryInteractionItemVM()
			};
			this.SecondaryInteractionMessages = new MBBindingList<MissionInteractionItemBaseVM>();
			this.ForcedInteractionMessages = new MBBindingList<MissionPrimaryInteractionItemVM>
			{
				new MissionPrimaryInteractionItemVM(),
				new MissionPrimaryInteractionItemVM()
			};
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x00014FC0 File Offset: 0x000131C0
		public override void RefreshValues()
		{
			base.RefreshValues();
			this.PrimaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM p)
			{
				p.RefreshValues();
			});
			this.SecondaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionInteractionItemBaseVM p)
			{
				p.RefreshValues();
			});
			this.ForcedInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM p)
			{
				p.RefreshValues();
			});
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x00015054 File Offset: 0x00013254
		public override void OnFinalize()
		{
			base.OnFinalize();
			this.PrimaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM p)
			{
				p.OnFinalize();
			});
			this.SecondaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionInteractionItemBaseVM p)
			{
				p.OnFinalize();
			});
			this.ForcedInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM p)
			{
				p.OnFinalize();
			});
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x000150E8 File Offset: 0x000132E8
		internal void Tick(float dt)
		{
			Agent agent;
			if ((agent = this._currentFocusedObject as Agent) != null)
			{
				this.ResetFocus();
				this.OnFocusGained(Agent.Main, agent, agent.IsActive() || Agent.Main.CanInteractWithAgent(agent, -1000f));
			}
			Agent agent2;
			if (this.IsActive && this._mission.Mode == MissionMode.StartUp && (agent2 = this._currentFocusedObject as Agent) != null && agent2.IsEnemyOf(this._mission.MainAgent))
			{
				this.IsActive = false;
			}
			this.HasSecondaryMessages = this.SecondaryInteractionMessages.Count > 0;
			this.SecondaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionInteractionItemBaseVM s)
			{
				s.RefreshValues();
			});
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x000151AD File Offset: 0x000133AD
		internal void CheckAndClearFocusedAgent(Agent agent)
		{
			if (this._currentFocusedObject != null && this._currentFocusedObject as Agent == agent)
			{
				this.IsActive = false;
				this.ResetFocus();
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x000151D2 File Offset: 0x000133D2
		public void OnFocusedHealthChanged(IFocusable focusable, float healthPercentage, bool hideHealthbarWhenFull)
		{
			this.SetHealth(healthPercentage, hideHealthbarWhenFull);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x000151DC File Offset: 0x000133DC
		internal void OnFocusGained(Agent mainAgent, IFocusable focusableObject, bool isInteractable)
		{
			if (this.IsPlayerActive && (this._currentFocusedObject != focusableObject || this._currentObjectInteractable != isInteractable))
			{
				this.ResetFocus();
				this._currentFocusedObject = focusableObject;
				this._currentObjectInteractable = isInteractable;
				Agent agent;
				UsableMissionObject usableMissionObject;
				if ((agent = focusableObject as Agent) != null)
				{
					if (agent.IsHuman)
					{
						this.SetHumanAgent(mainAgent, agent, isInteractable);
						return;
					}
					if (agent.IsMount)
					{
						this.SetMount(mainAgent, agent, isInteractable);
						return;
					}
					this.SetGenericAgent(mainAgent, agent, isInteractable);
					return;
				}
				else if ((usableMissionObject = focusableObject as UsableMissionObject) != null)
				{
					SpawnedItemEntity spawnedItemEntity;
					if ((spawnedItemEntity = usableMissionObject as SpawnedItemEntity) != null)
					{
						bool flag = Agent.Main.CanQuickPickUp(spawnedItemEntity);
						this.SetItem(spawnedItemEntity, flag, isInteractable);
						return;
					}
					this.SetUsableMissionObject(usableMissionObject, isInteractable);
					return;
				}
				else
				{
					UsableMachine usableMachine;
					if ((usableMachine = focusableObject as UsableMachine) != null)
					{
						this.SetUsableMachine(usableMachine, isInteractable);
						return;
					}
					DestructableComponent destructableComponent;
					if ((destructableComponent = focusableObject as DestructableComponent) != null)
					{
						this.SetDestructibleComponent(destructableComponent, false);
					}
				}
			}
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x000152B1 File Offset: 0x000134B1
		internal void OnFocusLost(Agent agent, IFocusable focusableObject)
		{
			this.ResetFocus();
			this.IsActive = false;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x000152C0 File Offset: 0x000134C0
		internal void OnAgentInteraction(Agent userAgent, Agent agent, sbyte agentBoneIndex)
		{
			if (this._mission.Mode == MissionMode.Stealth && agent.IsHuman && agent.IsActive() && !agent.IsEnemyOf(userAgent))
			{
				this.SetHumanAgent(userAgent, agent, true);
			}
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x000152F2 File Offset: 0x000134F2
		private void GetInteractionTexts(Agent requesterAgent, IFocusable focusable, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			focusableObjectInformation.IsActive = false;
			Mission mission = this._mission;
			if (mission == null)
			{
				return;
			}
			MissionFocusableObjectInformationProvider focusableObjectInformationProvider = mission.FocusableObjectInformationProvider;
			if (focusableObjectInformationProvider == null)
			{
				return;
			}
			focusableObjectInformationProvider.GetInteractionTexts(requesterAgent, focusable, isInteractable, out focusableObjectInformation);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x00015323 File Offset: 0x00013523
		private void SetItem(SpawnedItemEntity item, bool canQuickPickup, bool isInteractable)
		{
			this.SetInteractionMessages(Agent.Main, item, isInteractable);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x00015332 File Offset: 0x00013532
		private void SetUsableMissionObject(UsableMissionObject usableObject, bool isInteractable)
		{
			this.SetInteractionMessages(Agent.Main, usableObject, isInteractable);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x00015344 File Offset: 0x00013544
		private void SetUsableMachine(UsableMachine machine, bool isInteractable)
		{
			this.SetInteractionMessages(Agent.Main, machine, isInteractable);
			if (machine.DestructionComponent != null)
			{
				this.TargetHealth = (int)(100f * machine.DestructionComponent.HitPoint / machine.DestructionComponent.MaxHitPoint);
				this.ShowHealthBar = true;
			}
		}

		// Token: 0x0600058E RID: 1422 RVA: 0x00015391 File Offset: 0x00013591
		private void SetDestructibleComponent(DestructableComponent machine, bool isInteractable)
		{
			this.SetInteractionMessages(Agent.Main, machine, isInteractable);
			this.TargetHealth = (int)(100f * machine.HitPoint / machine.MaxHitPoint);
			this.ShowHealthBar = machine.HitPoint < machine.MaxHitPoint;
		}

		// Token: 0x0600058F RID: 1423 RVA: 0x000153CE File Offset: 0x000135CE
		private void SetHumanAgent(Agent requesterAgent, Agent focusedAgent, bool isInteractable)
		{
			this.SetInteractionMessages(requesterAgent, focusedAgent, isInteractable);
		}

		// Token: 0x06000590 RID: 1424 RVA: 0x000153D9 File Offset: 0x000135D9
		private void SetMount(Agent agent, Agent focusedAgent, bool isInteractable)
		{
			this.SetInteractionMessages(agent, focusedAgent, isInteractable);
			if (focusedAgent.IsActive() && focusedAgent.IsMount && focusedAgent.RiderAgent == null)
			{
				this.ShowHealthBar = false;
			}
		}

		// Token: 0x06000591 RID: 1425 RVA: 0x00015403 File Offset: 0x00013603
		private void SetGenericAgent(Agent agent, Agent focusedAgent, bool isInteractable)
		{
			if (focusedAgent.IsActive() && !focusedAgent.IsMount && !focusedAgent.IsHuman)
			{
				this.SetInteractionMessages(agent, focusedAgent, isInteractable);
			}
		}

		// Token: 0x06000592 RID: 1426 RVA: 0x00015428 File Offset: 0x00013628
		private void SetInteractionMessages(Agent requesterAgent, IFocusable focusableObject, bool isInteractable)
		{
			FocusableObjectInformation focusableObjectInformation;
			this.GetInteractionTexts(requesterAgent, focusableObject, isInteractable, out focusableObjectInformation);
			this.IsActive = focusableObjectInformation.IsActive;
			this.PrimaryInteractionMessages[0].SetData(focusableObjectInformation.PrimaryInteractionText, false);
			this.PrimaryInteractionMessages[1].SetData(focusableObjectInformation.SecondaryInteractionText, false);
			this.PrimaryInteractionMessages[1].FocusTypeString = ((focusableObject != null) ? focusableObject.FocusableObjectType.ToString() : null) ?? FocusableObjectType.None.ToString();
		}

		// Token: 0x06000593 RID: 1427 RVA: 0x000154BC File Offset: 0x000136BC
		public void OnActiveMissionHintChanged(MissionHint previousHint, MissionHint newHint)
		{
			if (previousHint != null && newHint == null)
			{
				for (int i = this.SecondaryInteractionMessages.Count - 1; i >= 0; i--)
				{
					MissionHintInteractionItemVM missionHintInteractionItemVM;
					if ((missionHintInteractionItemVM = this.SecondaryInteractionMessages[i] as MissionHintInteractionItemVM) != null && missionHintInteractionItemVM.Hint == previousHint)
					{
						this.SecondaryInteractionMessages.RemoveAt(i);
					}
				}
			}
			if (newHint != null)
			{
				this.SecondaryInteractionMessages.Add(new MissionHintInteractionItemVM(newHint));
			}
			this.HasSecondaryMessages = this.SecondaryInteractionMessages.Count > 0;
		}

		// Token: 0x06000594 RID: 1428 RVA: 0x00015539 File Offset: 0x00013739
		public void AddSecondaryMessage(MissionInteractionItemBaseVM message)
		{
			if (this.HasSecondaryInteractionMessage(message))
			{
				Debug.FailedAssert("Trying to add the same interaction message twice", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.ViewModelCollection\\Missions\\Interaction\\MissionAgentInteractionInterfaceVM.cs", "AddSecondaryMessage", 264);
				return;
			}
			this.SecondaryInteractionMessages.Add(message);
			message.IsDisplayed = true;
		}

		// Token: 0x06000595 RID: 1429 RVA: 0x00015571 File Offset: 0x00013771
		public bool RemoveSecondaryMessage(MissionInteractionItemBaseVM message)
		{
			message.IsDisplayed = false;
			return this.SecondaryInteractionMessages.Remove(message);
		}

		// Token: 0x06000596 RID: 1430 RVA: 0x00015586 File Offset: 0x00013786
		public bool HasSecondaryInteractionMessage(MissionInteractionItemBaseVM message)
		{
			return message.IsDisplayed;
		}

		// Token: 0x06000597 RID: 1431 RVA: 0x0001558E File Offset: 0x0001378E
		private void SetHealth(float healthPercentage, bool hideHealthBarWhenFull)
		{
			this.TargetHealth = (int)(100f * healthPercentage);
			if (hideHealthBarWhenFull)
			{
				this.ShowHealthBar = this.TargetHealth < 100;
				return;
			}
			this.ShowHealthBar = true;
		}

		// Token: 0x06000598 RID: 1432 RVA: 0x000155B9 File Offset: 0x000137B9
		public void ResetFocus()
		{
			this._currentFocusedObject = null;
			this.ShowHealthBar = false;
			this.PrimaryInteractionMessages[0].ResetData();
			this.PrimaryInteractionMessages[1].ResetData();
		}

		// Token: 0x06000599 RID: 1433 RVA: 0x000155EB File Offset: 0x000137EB
		public void SetForcedInteractionTexts(TextObject text1, bool isDisabled1, TextObject text2, bool isDisabled2)
		{
			this.HasForcedMessages = true;
			this.ForcedInteractionMessages[0].SetData(text1, isDisabled1);
			this.ForcedInteractionMessages[1].SetData(text2, isDisabled2);
		}

		// Token: 0x0600059A RID: 1434 RVA: 0x0001561B File Offset: 0x0001381B
		public void ClearForcedInteractionTexts()
		{
			this.ForcedInteractionMessages[0].ResetData();
			this.ForcedInteractionMessages[1].ResetData();
			this.HasForcedMessages = false;
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600059B RID: 1435 RVA: 0x00015646 File Offset: 0x00013846
		// (set) Token: 0x0600059C RID: 1436 RVA: 0x0001564E File Offset: 0x0001384E
		[DataSourceProperty]
		public int TargetHealth
		{
			get
			{
				return this._targetHealth;
			}
			set
			{
				if (value != this._targetHealth)
				{
					this._targetHealth = value;
					base.OnPropertyChangedWithValue(value, "TargetHealth");
				}
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x0600059D RID: 1437 RVA: 0x0001566C File Offset: 0x0001386C
		// (set) Token: 0x0600059E RID: 1438 RVA: 0x00015674 File Offset: 0x00013874
		[DataSourceProperty]
		public bool ShowHealthBar
		{
			get
			{
				return this._showHealthBar;
			}
			set
			{
				if (value != this._showHealthBar)
				{
					this._showHealthBar = value;
					base.OnPropertyChangedWithValue(value, "ShowHealthBar");
				}
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x0600059F RID: 1439 RVA: 0x00015692 File Offset: 0x00013892
		// (set) Token: 0x060005A0 RID: 1440 RVA: 0x0001569A File Offset: 0x0001389A
		[DataSourceProperty]
		public MBBindingList<MissionPrimaryInteractionItemVM> PrimaryInteractionMessages
		{
			get
			{
				return this._primaryInteractionMessages;
			}
			set
			{
				if (this._primaryInteractionMessages != value)
				{
					this._primaryInteractionMessages = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionPrimaryInteractionItemVM>>(value, "PrimaryInteractionMessages");
				}
			}
		}

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060005A1 RID: 1441 RVA: 0x000156B8 File Offset: 0x000138B8
		// (set) Token: 0x060005A2 RID: 1442 RVA: 0x000156C0 File Offset: 0x000138C0
		[DataSourceProperty]
		public MBBindingList<MissionInteractionItemBaseVM> SecondaryInteractionMessages
		{
			get
			{
				return this._secondaryInteractionMessages;
			}
			set
			{
				if (this._secondaryInteractionMessages != value)
				{
					this._secondaryInteractionMessages = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionInteractionItemBaseVM>>(value, "SecondaryInteractionMessages");
				}
			}
		}

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x060005A3 RID: 1443 RVA: 0x000156DE File Offset: 0x000138DE
		// (set) Token: 0x060005A4 RID: 1444 RVA: 0x000156E6 File Offset: 0x000138E6
		[DataSourceProperty]
		public string BackgroundColor
		{
			get
			{
				return this._backgroundColor;
			}
			set
			{
				if (this._backgroundColor != value)
				{
					this._backgroundColor = value;
					base.OnPropertyChangedWithValue<string>(value, "BackgroundColor");
				}
			}
		}

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x060005A5 RID: 1445 RVA: 0x00015709 File Offset: 0x00013909
		// (set) Token: 0x060005A6 RID: 1446 RVA: 0x00015711 File Offset: 0x00013911
		[DataSourceProperty]
		public string TextColor
		{
			get
			{
				return this._textColor;
			}
			set
			{
				if (this._textColor != value)
				{
					this._textColor = value;
					base.OnPropertyChangedWithValue<string>(value, "TextColor");
				}
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x060005A7 RID: 1447 RVA: 0x00015734 File Offset: 0x00013934
		// (set) Token: 0x060005A8 RID: 1448 RVA: 0x0001573C File Offset: 0x0001393C
		[DataSourceProperty]
		public bool IsActive
		{
			get
			{
				return this._isActive;
			}
			set
			{
				if (value != this._isActive)
				{
					this._isActive = value;
					base.OnPropertyChangedWithValue(value, "IsActive");
					if (!value)
					{
						this.ShowHealthBar = false;
						this.PrimaryInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM x)
						{
							x.ResetData();
						});
					}
				}
			}
		}

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x060005A9 RID: 1449 RVA: 0x00015799 File Offset: 0x00013999
		// (set) Token: 0x060005AA RID: 1450 RVA: 0x000157A1 File Offset: 0x000139A1
		[DataSourceProperty]
		public bool HasSecondaryMessages
		{
			get
			{
				return this._hasSecondaryMessages;
			}
			set
			{
				if (value != this._hasSecondaryMessages)
				{
					this._hasSecondaryMessages = value;
					base.OnPropertyChangedWithValue(value, "HasSecondaryMessages");
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060005AB RID: 1451 RVA: 0x000157BF File Offset: 0x000139BF
		// (set) Token: 0x060005AC RID: 1452 RVA: 0x000157C7 File Offset: 0x000139C7
		[DataSourceProperty]
		public bool DisplayInteractionText
		{
			get
			{
				return this._displayInteractionText;
			}
			set
			{
				if (value != this._displayInteractionText)
				{
					this._displayInteractionText = value;
					base.OnPropertyChangedWithValue(value, "DisplayInteractionText");
				}
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060005AD RID: 1453 RVA: 0x000157E5 File Offset: 0x000139E5
		// (set) Token: 0x060005AE RID: 1454 RVA: 0x000157ED File Offset: 0x000139ED
		[DataSourceProperty]
		public MBBindingList<MissionPrimaryInteractionItemVM> ForcedInteractionMessages
		{
			get
			{
				return this._forcedInteractionMessages;
			}
			set
			{
				if (this._forcedInteractionMessages != value)
				{
					this._forcedInteractionMessages = value;
					base.OnPropertyChangedWithValue<MBBindingList<MissionPrimaryInteractionItemVM>>(value, "ForcedInteractionMessages");
				}
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060005AF RID: 1455 RVA: 0x0001580B File Offset: 0x00013A0B
		// (set) Token: 0x060005B0 RID: 1456 RVA: 0x00015814 File Offset: 0x00013A14
		[DataSourceProperty]
		public bool HasForcedMessages
		{
			get
			{
				return this._hasForcedMessages;
			}
			set
			{
				if (this._hasForcedMessages != value)
				{
					this._hasForcedMessages = value;
					base.OnPropertyChangedWithValue(value, "HasForcedMessages");
					if (!value)
					{
						this.ForcedInteractionMessages.ApplyActionOnAllItems(delegate(MissionPrimaryInteractionItemVM x)
						{
							x.ResetData();
						});
					}
				}
			}
		}

		// Token: 0x060005B1 RID: 1457 RVA: 0x0001586C File Offset: 0x00013A6C
		private string GetWeaponSpecificText(SpawnedItemEntity spawnedItem)
		{
			MissionWeapon weaponCopy = spawnedItem.WeaponCopy;
			WeaponComponentData currentUsageItem = weaponCopy.CurrentUsageItem;
			if (currentUsageItem != null && currentUsageItem.IsShield)
			{
				MBTextManager.SetTextVariable("LEFT", (int)weaponCopy.HitPoints);
				MBTextManager.SetTextVariable("RIGHT", (int)weaponCopy.ModifiedMaxHitPoints);
				return GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null).ToString();
			}
			WeaponComponentData currentUsageItem2 = weaponCopy.CurrentUsageItem;
			if (currentUsageItem2 != null && currentUsageItem2.IsAmmo && weaponCopy.ModifiedMaxAmount > 1 && !spawnedItem.IsStuckMissile())
			{
				MBTextManager.SetTextVariable("LEFT", (int)weaponCopy.Amount);
				MBTextManager.SetTextVariable("RIGHT", (int)weaponCopy.ModifiedMaxAmount);
				return GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null).ToString();
			}
			return "";
		}

		// Token: 0x04000284 RID: 644
		private readonly Mission _mission;

		// Token: 0x04000285 RID: 645
		private bool _currentObjectInteractable;

		// Token: 0x04000286 RID: 646
		private IFocusable _currentFocusedObject;

		// Token: 0x04000287 RID: 647
		private bool _isActive;

		// Token: 0x04000288 RID: 648
		private bool _hasSecondaryMessages;

		// Token: 0x04000289 RID: 649
		private MBBindingList<MissionPrimaryInteractionItemVM> _primaryInteractionMessages;

		// Token: 0x0400028A RID: 650
		private MBBindingList<MissionInteractionItemBaseVM> _secondaryInteractionMessages;

		// Token: 0x0400028B RID: 651
		private int _targetHealth;

		// Token: 0x0400028C RID: 652
		private bool _showHealthBar;

		// Token: 0x0400028D RID: 653
		private string _backgroundColor;

		// Token: 0x0400028E RID: 654
		private string _textColor;

		// Token: 0x0400028F RID: 655
		private bool _displayInteractionText;

		// Token: 0x04000290 RID: 656
		private bool _hasForcedMessages;

		// Token: 0x04000291 RID: 657
		private MBBindingList<MissionPrimaryInteractionItemVM> _forcedInteractionMessages;
	}
}
