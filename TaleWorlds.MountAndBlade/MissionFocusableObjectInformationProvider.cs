using System;
using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000272 RID: 626
	public class MissionFocusableObjectInformationProvider
	{
		// Token: 0x06002359 RID: 9049 RVA: 0x0007CC76 File Offset: 0x0007AE76
		public MissionFocusableObjectInformationProvider()
		{
			this._getFocusableObjectTextsCallbacks = new List<GetFocusableObjectInteractionTextsDelegate>();
			this._getDefaultFocusableObjectTextsCallbacks = new List<GetFocusableObjectInteractionTextsDelegate>();
			this.AddDefaultInteractionTexts();
		}

		// Token: 0x0600235A RID: 9050 RVA: 0x0007CC9C File Offset: 0x0007AE9C
		private void AddDefaultInteractionTexts()
		{
			this._getDefaultFocusableObjectTextsCallbacks.Add(new GetFocusableObjectInteractionTextsDelegate(this.GetSpawnedItemEntityTexts));
			this._getDefaultFocusableObjectTextsCallbacks.Add(new GetFocusableObjectInteractionTextsDelegate(this.GetUsableMissionObjectTexts));
			this._getDefaultFocusableObjectTextsCallbacks.Add(new GetFocusableObjectInteractionTextsDelegate(this.GetDestructibleComponentTexts));
			this._getDefaultFocusableObjectTextsCallbacks.Add(new GetFocusableObjectInteractionTextsDelegate(this.GetUsableMachineTexts));
			this._getDefaultFocusableObjectTextsCallbacks.Add(new GetFocusableObjectInteractionTextsDelegate(this.GetHumanAgentTexts));
			this._getDefaultFocusableObjectTextsCallbacks.Add(new GetFocusableObjectInteractionTextsDelegate(this.GetMountTexts));
			this._getDefaultFocusableObjectTextsCallbacks.Add(new GetFocusableObjectInteractionTextsDelegate(this.GetGenericAgentTexts));
		}

		// Token: 0x0600235B RID: 9051 RVA: 0x0007CD4A File Offset: 0x0007AF4A
		public void OnFinalize()
		{
			this._getFocusableObjectTextsCallbacks.Clear();
			this._getDefaultFocusableObjectTextsCallbacks.Clear();
			this._getFocusableObjectTextsCallbacks = null;
			this._getDefaultFocusableObjectTextsCallbacks = null;
		}

		// Token: 0x0600235C RID: 9052 RVA: 0x0007CD70 File Offset: 0x0007AF70
		public void AddInfoCallback(GetFocusableObjectInteractionTextsDelegate callback)
		{
			this._getFocusableObjectTextsCallbacks.Add(callback);
		}

		// Token: 0x0600235D RID: 9053 RVA: 0x0007CD7E File Offset: 0x0007AF7E
		public void RemoveInfoCallback(GetFocusableObjectInteractionTextsDelegate callback)
		{
			this._getFocusableObjectTextsCallbacks.Remove(callback);
		}

		// Token: 0x0600235E RID: 9054 RVA: 0x0007CD90 File Offset: 0x0007AF90
		public void GetInteractionTexts(Agent requesterAgent, IFocusable focusable, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			for (int i = this._getFocusableObjectTextsCallbacks.Count - 1; i >= 0; i--)
			{
				this._getFocusableObjectTextsCallbacks[i](requesterAgent, focusable, isInteractable, out focusableObjectInformation);
				if (focusableObjectInformation.IsActive)
				{
					return;
				}
			}
			for (int j = 0; j < this._getDefaultFocusableObjectTextsCallbacks.Count; j++)
			{
				this._getDefaultFocusableObjectTextsCallbacks[j](requesterAgent, focusable, isInteractable, out focusableObjectInformation);
				if (focusableObjectInformation.IsActive)
				{
					return;
				}
			}
			focusableObjectInformation.IsActive = false;
			focusableObjectInformation.PrimaryInteractionText = TextObject.GetEmpty();
			focusableObjectInformation.SecondaryInteractionText = TextObject.GetEmpty();
		}

		// Token: 0x0600235F RID: 9055 RVA: 0x0007CE34 File Offset: 0x0007B034
		private void GetSpawnedItemEntityTexts(Agent requesterAgent, IFocusable focusableObject, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			SpawnedItemEntity spawnedItemEntity;
			if ((spawnedItemEntity = focusableObject as SpawnedItemEntity) == null || spawnedItemEntity.IsDeactivated || spawnedItemEntity.IsDisabled)
			{
				focusableObjectInformation.IsActive = false;
				return;
			}
			EquipmentIndex equipmentIndex;
			ItemObject weaponToReplaceOnQuickAction = Agent.Main.GetWeaponToReplaceOnQuickAction(spawnedItemEntity, out equipmentIndex);
			bool flag = Agent.Main.CanQuickPickUp(spawnedItemEntity);
			bool flag2 = equipmentIndex != EquipmentIndex.None && !Agent.Main.Equipment[equipmentIndex].IsEmpty && Agent.Main.Equipment[equipmentIndex].IsAnyConsumable() && Agent.Main.Equipment[equipmentIndex].Amount < Agent.Main.Equipment[equipmentIndex].ModifiedMaxAmount;
			TextObject actionMessage = spawnedItemEntity.GetActionMessage(weaponToReplaceOnQuickAction, flag2);
			TextObject descriptionMessage = spawnedItemEntity.GetDescriptionMessage(flag2);
			if (TextObject.IsNullOrEmpty(actionMessage) || TextObject.IsNullOrEmpty(descriptionMessage))
			{
				focusableObjectInformation.IsActive = false;
				return;
			}
			if (!isInteractable)
			{
				focusableObjectInformation.PrimaryInteractionText = spawnedItemEntity.GetInfoTextForBeingNotInteractable(Agent.Main);
				focusableObjectInformation.SecondaryInteractionText = this.GetItemNameText(descriptionMessage, this.GetWeaponSpecificText(spawnedItemEntity));
				focusableObjectInformation.IsActive = true;
				return;
			}
			MBTextManager.SetTextVariable("USE_KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f), false);
			if (!flag)
			{
				focusableObjectInformation.PrimaryInteractionText = GameTexts.FindText("str_hold_key_action", null);
				focusableObjectInformation.PrimaryInteractionText.SetTextVariable("KEY", GameTexts.FindText("str_ui_agent_interaction_use", null).ToString());
				focusableObjectInformation.PrimaryInteractionText.SetTextVariable("ACTION", GameTexts.FindText("str_select_item_to_replace", null).ToString());
				focusableObjectInformation.SecondaryInteractionText = this.GetItemNameText(descriptionMessage, this.GetWeaponSpecificText(spawnedItemEntity));
				focusableObjectInformation.IsActive = true;
				return;
			}
			Agent main = Agent.Main;
			if (main != null && main.CanInteractableWeaponBePickedUp(spawnedItemEntity))
			{
				TextObject textObject = GameTexts.FindText("str_key_action", null);
				textObject.SetTextVariable("KEY", GameTexts.FindText("str_ui_agent_interaction_use", null).ToString());
				textObject.SetTextVariable("ACTION", actionMessage.ToString());
				TextObject textObject2 = GameTexts.FindText("str_hold_key_action", null);
				textObject2.SetTextVariable("KEY", GameTexts.FindText("str_ui_agent_interaction_use", null).ToString());
				textObject2.SetTextVariable("ACTION", GameTexts.FindText("str_select_item_to_replace", null).ToString());
				focusableObjectInformation.PrimaryInteractionText = GameTexts.FindText("str_string_newline_string", null);
				focusableObjectInformation.PrimaryInteractionText.SetTextVariable("STR1", textObject.ToString());
				focusableObjectInformation.PrimaryInteractionText.SetTextVariable("STR2", textObject2.ToString());
				focusableObjectInformation.SecondaryInteractionText = this.GetItemNameText(descriptionMessage, this.GetWeaponSpecificText(spawnedItemEntity));
				focusableObjectInformation.IsActive = true;
				return;
			}
			focusableObjectInformation.PrimaryInteractionText = GameTexts.FindText("str_key_action", null);
			focusableObjectInformation.PrimaryInteractionText.SetTextVariable("KEY", GameTexts.FindText("str_ui_agent_interaction_use", null).ToString());
			focusableObjectInformation.PrimaryInteractionText.SetTextVariable("ACTION", actionMessage.ToString());
			focusableObjectInformation.SecondaryInteractionText = this.GetItemNameText(descriptionMessage, this.GetWeaponSpecificText(spawnedItemEntity));
			focusableObjectInformation.IsActive = true;
		}

		// Token: 0x06002360 RID: 9056 RVA: 0x0007D16C File Offset: 0x0007B36C
		private TextObject GetItemNameText(TextObject descriptionMessage, TextObject weaponSpecificText)
		{
			TextObject textObject;
			if (!TextObject.IsNullOrEmpty(weaponSpecificText))
			{
				textObject = GameTexts.FindText("str_STR1_space_STR2", null);
				textObject.SetTextVariable("STR1", descriptionMessage.ToString());
				textObject.SetTextVariable("STR2", weaponSpecificText.ToString());
			}
			else
			{
				textObject = descriptionMessage;
			}
			return textObject;
		}

		// Token: 0x06002361 RID: 9057 RVA: 0x0007D1B8 File Offset: 0x0007B3B8
		private TextObject GetWeaponSpecificText(SpawnedItemEntity spawnedItem)
		{
			MissionWeapon weaponCopy = spawnedItem.WeaponCopy;
			WeaponComponentData currentUsageItem = weaponCopy.CurrentUsageItem;
			if (currentUsageItem != null && currentUsageItem.IsShield)
			{
				TextObject textObject = GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null);
				textObject.SetTextVariable("LEFT", (int)weaponCopy.HitPoints);
				textObject.SetTextVariable("RIGHT", (int)weaponCopy.ModifiedMaxHitPoints);
				return textObject;
			}
			WeaponComponentData currentUsageItem2 = weaponCopy.CurrentUsageItem;
			if (currentUsageItem2 != null && currentUsageItem2.IsAmmo && weaponCopy.ModifiedMaxAmount > 1 && !spawnedItem.IsStuckMissile())
			{
				TextObject textObject2 = GameTexts.FindText("str_LEFT_over_RIGHT_in_paranthesis", null);
				textObject2.SetTextVariable("LEFT", (int)weaponCopy.Amount);
				textObject2.SetTextVariable("RIGHT", (int)weaponCopy.ModifiedMaxAmount);
				return textObject2;
			}
			return TextObject.GetEmpty();
		}

		// Token: 0x06002362 RID: 9058 RVA: 0x0007D274 File Offset: 0x0007B474
		private void GetUsableMissionObjectTexts(Agent requesterAgent, IFocusable focusableObject, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			UsableMissionObject usableMissionObject;
			if ((usableMissionObject = focusableObject as UsableMissionObject) == null)
			{
				focusableObjectInformation.IsActive = false;
				return;
			}
			if (!TextObject.IsNullOrEmpty(usableMissionObject.ActionMessage) || !TextObject.IsNullOrEmpty(usableMissionObject.DescriptionMessage))
			{
				focusableObjectInformation.PrimaryInteractionText = usableMissionObject.DescriptionMessage;
				focusableObjectInformation.SecondaryInteractionText = (isInteractable ? usableMissionObject.ActionMessage : TextObject.GetEmpty());
				focusableObjectInformation.IsActive = true;
				return;
			}
			UsableMachine usableMachineFromPoint = this.GetUsableMachineFromPoint(usableMissionObject);
			if (usableMachineFromPoint != null)
			{
				focusableObjectInformation.PrimaryInteractionText = usableMachineFromPoint.GetDescriptionText(usableMissionObject.GameEntity);
				focusableObjectInformation.SecondaryInteractionText = (isInteractable ? (usableMachineFromPoint.GetActionTextForStandingPoint(usableMissionObject) ?? TextObject.GetEmpty()) : TextObject.GetEmpty());
				focusableObjectInformation.IsActive = true;
				return;
			}
			focusableObjectInformation.IsActive = false;
		}

		// Token: 0x06002363 RID: 9059 RVA: 0x0007D33C File Offset: 0x0007B53C
		private UsableMachine GetUsableMachineFromPoint(UsableMissionObject standingPoint)
		{
			WeakGameEntity weakGameEntity = standingPoint.GameEntity;
			while (weakGameEntity.IsValid && !weakGameEntity.HasScriptOfType<UsableMachine>())
			{
				weakGameEntity = weakGameEntity.Parent;
			}
			UsableMachine usableMachine = null;
			if (weakGameEntity.IsValid)
			{
				usableMachine = weakGameEntity.GetFirstScriptOfType<UsableMachine>();
			}
			return usableMachine;
		}

		// Token: 0x06002364 RID: 9060 RVA: 0x0007D380 File Offset: 0x0007B580
		private void GetDestructibleComponentTexts(Agent requesterAgent, IFocusable focusableObject, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			DestructableComponent destructableComponent;
			if ((destructableComponent = focusableObject as DestructableComponent) == null)
			{
				focusableObjectInformation.IsActive = false;
				return;
			}
			TextObject descriptionText = destructableComponent.GetDescriptionText(destructableComponent.GameEntity);
			bool flag = !TextObject.IsNullOrEmpty(descriptionText);
			focusableObjectInformation.PrimaryInteractionText = (flag ? descriptionText : TextObject.GetEmpty());
			focusableObjectInformation.SecondaryInteractionText = TextObject.GetEmpty();
			focusableObjectInformation.IsActive = flag;
		}

		// Token: 0x06002365 RID: 9061 RVA: 0x0007D3E8 File Offset: 0x0007B5E8
		private void GetUsableMachineTexts(Agent requesterAgent, IFocusable focusableObject, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			UsableMachine usableMachine;
			if ((usableMachine = focusableObject as UsableMachine) == null)
			{
				focusableObjectInformation.IsActive = false;
				return;
			}
			focusableObjectInformation.PrimaryInteractionText = usableMachine.GetDescriptionText(usableMachine.GameEntity) ?? TextObject.GetEmpty();
			focusableObjectInformation.SecondaryInteractionText = TextObject.GetEmpty();
			focusableObjectInformation.IsActive = true;
		}

		// Token: 0x06002366 RID: 9062 RVA: 0x0007D440 File Offset: 0x0007B640
		private void GetHumanAgentTexts(Agent requesterAgent, IFocusable focusableObject, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			Agent agent;
			if ((agent = focusableObject as Agent) == null || !agent.IsHuman)
			{
				focusableObjectInformation.IsActive = false;
				return;
			}
			Mission mission = Mission.Current;
			if (isInteractable && (mission.Mode == MissionMode.StartUp || mission.Mode == MissionMode.Duel || mission.Mode == MissionMode.Battle || mission.Mode == MissionMode.Stealth) && agent.IsHuman)
			{
				MBTextManager.SetTextVariable("USE_KEY", HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f), false);
				if (agent.IsActive())
				{
					if (mission.Mode == MissionMode.Duel || mission.Mode == MissionMode.Battle)
					{
						focusableObjectInformation.PrimaryInteractionText = agent.NameTextObject;
						focusableObjectInformation.IsActive = true;
						return;
					}
					if (agent.IsEnemyOf(requesterAgent))
					{
						focusableObjectInformation.PrimaryInteractionText = agent.NameTextObject;
						focusableObjectInformation.IsActive = true;
						return;
					}
					if (!Mission.Current.IsAgentInteractionAllowed())
					{
						focusableObjectInformation.IsActive = false;
						return;
					}
					focusableObjectInformation.PrimaryInteractionText = agent.NameTextObject;
					if (this.CanInteractWithAgent(requesterAgent, agent))
					{
						focusableObjectInformation.SecondaryInteractionText = GameTexts.FindText("str_key_action", null);
						focusableObjectInformation.SecondaryInteractionText.SetTextVariable("KEY", GameTexts.FindText("str_ui_agent_interaction_use", null));
						focusableObjectInformation.SecondaryInteractionText.SetTextVariable("ACTION", GameTexts.FindText("str_ui_talk", null));
					}
					focusableObjectInformation.IsActive = true;
					return;
				}
				else if (mission.Mode != MissionMode.Battle && mission.Mode != MissionMode.Duel)
				{
					focusableObjectInformation.PrimaryInteractionText = agent.NameTextObject;
					focusableObjectInformation.SecondaryInteractionText = GameTexts.FindText("str_key_action", null);
					focusableObjectInformation.SecondaryInteractionText.SetTextVariable("KEY", GameTexts.FindText("str_ui_agent_interaction_use", null));
					focusableObjectInformation.SecondaryInteractionText.SetTextVariable("ACTION", GameTexts.FindText("str_ui_search", null));
					focusableObjectInformation.IsActive = true;
					return;
				}
			}
			focusableObjectInformation.IsActive = false;
		}

		// Token: 0x06002367 RID: 9063 RVA: 0x0007D61D File Offset: 0x0007B81D
		private bool CanInteractWithAgent(Agent requesterAgent, Agent focusedAgent)
		{
			return requesterAgent.CanInteractWithAgent(focusedAgent, -1000f) || requesterAgent.IsEnemyOf(focusedAgent);
		}

		// Token: 0x06002368 RID: 9064 RVA: 0x0007D638 File Offset: 0x0007B838
		private void GetMountTexts(Agent requesterAgent, IFocusable focusableObject, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			Agent agent;
			if ((agent = focusableObject as Agent) == null || !agent.IsMount)
			{
				focusableObjectInformation.IsActive = false;
				return;
			}
			if (agent.IsActive() && agent.IsMount)
			{
				string keyHyperlinkText = HyperlinkTexts.GetKeyHyperlinkText(HotKeyManager.GetHotKeyId("CombatHotKeyCategory", 13), 1f);
				focusableObjectInformation.PrimaryInteractionText = agent.NameTextObject;
				if (agent.RiderAgent == null)
				{
					if ((float)requesterAgent.Character.GetSkillValue(DefaultSkills.Riding) < agent.GetAgentDrivenPropertyValue(DrivenProperty.MountDifficulty))
					{
						focusableObjectInformation.SecondaryInteractionText = GameTexts.FindText("str_ui_riding_skill_not_adequate_to_mount", null);
					}
					else if ((requesterAgent.GetAgentFlags() & AgentFlag.CanRide) > AgentFlag.None)
					{
						focusableObjectInformation.SecondaryInteractionText = GameTexts.FindText("str_key_action", null);
						focusableObjectInformation.SecondaryInteractionText.SetTextVariable("KEY", keyHyperlinkText);
						focusableObjectInformation.SecondaryInteractionText.SetTextVariable("ACTION", GameTexts.FindText("str_ui_mount", null));
					}
				}
				else if (agent.RiderAgent == requesterAgent)
				{
					focusableObjectInformation.SecondaryInteractionText = GameTexts.FindText("str_key_action", null);
					focusableObjectInformation.SecondaryInteractionText.SetTextVariable("KEY", keyHyperlinkText);
					focusableObjectInformation.SecondaryInteractionText.SetTextVariable("ACTION", GameTexts.FindText("str_ui_dismount", null));
				}
				focusableObjectInformation.IsActive = true;
				return;
			}
			focusableObjectInformation.IsActive = false;
		}

		// Token: 0x06002369 RID: 9065 RVA: 0x0007D794 File Offset: 0x0007B994
		private void GetGenericAgentTexts(Agent requesterAgent, IFocusable focusableObject, bool isInteractable, out FocusableObjectInformation focusableObjectInformation)
		{
			focusableObjectInformation = default(FocusableObjectInformation);
			Agent agent;
			if ((agent = focusableObject as Agent) == null || agent.IsHuman || agent.IsMount)
			{
				focusableObjectInformation.IsActive = false;
				return;
			}
			if (agent.IsActive() && !agent.IsMount && !agent.IsHuman)
			{
				AgentComponent agentComponent = null;
				foreach (AgentComponent agentComponent2 in agent.Components)
				{
					if (agentComponent2 is IFocusable)
					{
						agentComponent = agentComponent2;
						break;
					}
				}
				if (agentComponent != null)
				{
					focusableObjectInformation.PrimaryInteractionText = agent.NameTextObject;
					if (isInteractable)
					{
						focusableObjectInformation.SecondaryInteractionText = ((IFocusable)agentComponent).GetDescriptionText(WeakGameEntity.Invalid);
					}
					else
					{
						focusableObjectInformation.SecondaryInteractionText = ((IFocusable)agentComponent).GetInfoTextForBeingNotInteractable(requesterAgent);
					}
					focusableObjectInformation.IsActive = true;
					return;
				}
			}
			focusableObjectInformation.IsActive = false;
		}

		// Token: 0x04000D90 RID: 3472
		private List<GetFocusableObjectInteractionTextsDelegate> _getFocusableObjectTextsCallbacks;

		// Token: 0x04000D91 RID: 3473
		private List<GetFocusableObjectInteractionTextsDelegate> _getDefaultFocusableObjectTextsCallbacks;
	}
}
