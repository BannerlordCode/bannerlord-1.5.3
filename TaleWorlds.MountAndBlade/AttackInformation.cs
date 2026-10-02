using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.ComponentInterfaces;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200019C RID: 412
	public struct AttackInformation
	{
		// Token: 0x060015AE RID: 5550 RVA: 0x00050A98 File Offset: 0x0004EC98
		public AttackInformation(Agent attackerAgent, Agent victimAgent, WeakGameEntity hitObject, in AttackCollisionData attackCollisionData, in MissionWeapon attackerWeapon)
		{
			this.AttackerAgent = attackerAgent;
			this.VictimAgent = victimAgent;
			this.IsAttackerAgentNull = attackerAgent == null;
			this.IsVictimAgentNull = victimAgent == null;
			this.ArmorAmountFloat = 0f;
			AttackCollisionData attackCollisionData2;
			if (!this.IsVictimAgentNull)
			{
				attackCollisionData2 = attackCollisionData;
				this.ArmorAmountFloat = victimAgent.GetBaseArmorEffectivenessForBodyPart(attackCollisionData2.VictimHitBodyPart);
			}
			this.ShieldOnBack = null;
			if (!this.IsVictimAgentNull && (victimAgent.GetAgentFlags() & AgentFlag.CanWieldWeapon) != AgentFlag.None)
			{
				EquipmentIndex offhandWieldedItemIndex = victimAgent.GetOffhandWieldedItemIndex();
				for (int i = 0; i < 4; i++)
				{
					WeaponComponentData currentUsageItem = victimAgent.Equipment[i].CurrentUsageItem;
					if (i != (int)offhandWieldedItemIndex && currentUsageItem != null && currentUsageItem.IsShield)
					{
						this.ShieldOnBack = currentUsageItem;
						break;
					}
				}
			}
			this.AttackerWeapon = attackerWeapon;
			this.VictimShield = MissionWeapon.Invalid;
			this.VictimMainHandWeapon = MissionWeapon.Invalid;
			if (!this.IsVictimAgentNull && (victimAgent.GetAgentFlags() & AgentFlag.CanWieldWeapon) != AgentFlag.None)
			{
				EquipmentIndex offhandWieldedItemIndex2 = victimAgent.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex2 != EquipmentIndex.None)
				{
					this.VictimShield = victimAgent.Equipment[offhandWieldedItemIndex2];
				}
				EquipmentIndex primaryWieldedItemIndex = victimAgent.GetPrimaryWieldedItemIndex();
				if (primaryWieldedItemIndex != EquipmentIndex.None)
				{
					this.VictimMainHandWeapon = victimAgent.Equipment[primaryWieldedItemIndex];
				}
			}
			this.AttackerAgentMountMovementDirection = default(Vec2);
			if (!this.IsAttackerAgentNull && attackerAgent.HasMount)
			{
				this.AttackerAgentMountMovementDirection = attackerAgent.MountAgent.GetMovementDirection();
			}
			this.VictimAgentMountMovementDirection = default(Vec2);
			if (!this.IsVictimAgentNull && victimAgent.HasMount)
			{
				this.VictimAgentMountMovementDirection = victimAgent.MountAgent.GetMovementDirection();
			}
			this.IsVictimAgentSameWithAttackerAgent = !this.IsAttackerAgentNull && attackerAgent == victimAgent;
			MissionWeapon missionWeapon = attackerWeapon;
			int num;
			if (missionWeapon.IsEmpty || this.IsAttackerAgentNull || !attackerAgent.IsHuman)
			{
				num = -1;
			}
			else
			{
				Monster monster = attackerAgent.Monster;
				missionWeapon = attackerWeapon;
				num = (int)monster.GetBoneToAttachForItemFlags(missionWeapon.Item.ItemFlags);
			}
			this.WeaponAttachBoneIndex = num;
			DestructableComponent destructableComponent = (hitObject.IsValid ? hitObject.GetFirstScriptOfTypeInFamily<DestructableComponent>() : null);
			this.HitObjectDestructibleComponent = destructableComponent;
			bool flag;
			if (!this.IsAttackerAgentNull)
			{
				if (!this.IsVictimAgentSameWithAttackerAgent && (this.IsVictimAgentNull || !victimAgent.IsFriendOf(attackerAgent)))
				{
					if (destructableComponent != null)
					{
						BattleSideEnum battleSide = destructableComponent.BattleSide;
						Team team = attackerAgent.Team;
						BattleSideEnum? battleSideEnum = ((team != null) ? new BattleSideEnum?(team.Side) : null);
						flag = (battleSide == battleSideEnum.GetValueOrDefault()) & (battleSideEnum != null);
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					flag = true;
				}
			}
			else
			{
				flag = false;
			}
			this.IsFriendlyFire = flag;
			this.OffHandItem = default(MissionWeapon);
			if (!this.IsAttackerAgentNull && (attackerAgent.GetAgentFlags() & AgentFlag.CanWieldWeapon) != AgentFlag.None)
			{
				EquipmentIndex offhandWieldedItemIndex3 = attackerAgent.GetOffhandWieldedItemIndex();
				if (offhandWieldedItemIndex3 != EquipmentIndex.None)
				{
					this.OffHandItem = attackerAgent.Equipment[offhandWieldedItemIndex3];
				}
			}
			attackCollisionData2 = attackCollisionData;
			this.IsHeadShot = attackCollisionData2.VictimHitBodyPart == BoneBodyPartType.Head;
			this.VictimAgentAbsorbedDamageRatio = 0f;
			this.DamageMultiplierOfBone = 0f;
			this.VictimMovementDirectionAsAngle = 0f;
			this.VictimAgentScale = 0f;
			this.VictimAgentHealth = 0f;
			this.VictimAgentMaxHealth = 0f;
			this.VictimAgentWeight = 0f;
			this.VictimAgentTotalEncumbrance = 0f;
			this.CombatDifficultyMultiplier = 1f;
			this.VictimHitPointRate = 0f;
			this.VictimAgentFlags = AgentFlag.CanAttack;
			this.VictimAgentAIStateFlags = Agent.AIStateFlag.Alarmed;
			this.IsVictimAgentLeftStance = false;
			this.DoesVictimHaveMountAgent = false;
			this.IsVictimAgentMine = false;
			this.DoesVictimHaveRiderAgent = false;
			this.IsVictimAgentRiderAgentMine = false;
			this.IsVictimAgentMount = false;
			this.IsVictimAgentHuman = false;
			this.IsVictimRiderAgentSameAsAttackerAgent = false;
			this.IsVictimPlayer = false;
			this.VictimAgentCharacter = null;
			this.VictimRiderAgentCharacter = null;
			this.VictimBattleEnvironment = BattleEnvironment.None;
			this.VictimAgentMovementVelocity = default(Vec2);
			this.VictimAgentPosition = default(Vec3);
			this.VictimAgentMovementDirection = default(Vec2);
			this.VictimAgentVelocity = default(Vec3);
			this.VictimCaptainCharacter = null;
			this.VictimAgentOrigin = null;
			this.VictimRiderAgentOrigin = null;
			this.VictimFormation = null;
			if (!this.IsVictimAgentNull)
			{
				this.IsVictimAgentMount = victimAgent.IsMount;
				this.IsVictimAgentMine = victimAgent.IsMine;
				this.IsVictimAgentHuman = victimAgent.IsHuman;
				this.IsVictimAgentLeftStance = victimAgent.GetIsLeftStance();
				this.DoesVictimHaveMountAgent = victimAgent.HasMount;
				this.DoesVictimHaveRiderAgent = victimAgent.RiderAgent != null;
				this.IsVictimRiderAgentSameAsAttackerAgent = this.DoesVictimHaveRiderAgent && victimAgent.RiderAgent == attackerAgent;
				this.IsVictimPlayer = victimAgent.IsPlayerControlled;
				this.VictimAgentAbsorbedDamageRatio = victimAgent.Monster.AbsorbedDamageRatio;
				AgentApplyDamageModel agentApplyDamageModel = MissionGameModels.Current.AgentApplyDamageModel;
				attackCollisionData2 = attackCollisionData;
				BoneBodyPartType boneBodyPartType;
				if (attackCollisionData2.CollisionBoneIndex == -1)
				{
					boneBodyPartType = BoneBodyPartType.None;
				}
				else
				{
					MBAgentVisuals agentVisuals = victimAgent.AgentVisuals;
					attackCollisionData2 = attackCollisionData;
					boneBodyPartType = agentVisuals.GetBoneTypeData(attackCollisionData2.CollisionBoneIndex).BodyPartType;
				}
				attackCollisionData2 = attackCollisionData;
				DamageTypes damageType = (DamageTypes)attackCollisionData2.DamageType;
				bool isVictimAgentHuman = this.IsVictimAgentHuman;
				attackCollisionData2 = attackCollisionData;
				this.DamageMultiplierOfBone = agentApplyDamageModel.GetDamageMultiplierForBodyPart(boneBodyPartType, damageType, isVictimAgentHuman, attackCollisionData2.IsMissile);
				this.VictimMovementDirectionAsAngle = victimAgent.MovementDirectionAsAngle;
				this.VictimAgentScale = victimAgent.AgentScale;
				this.VictimAgentHealth = victimAgent.Health;
				this.VictimAgentMaxHealth = victimAgent.HealthLimit;
				this.VictimAgentWeight = (victimAgent.IsMount ? victimAgent.SpawnEquipment[EquipmentIndex.ArmorItemEndSlot].Weight : ((float)victimAgent.Monster.Weight));
				this.VictimAgentTotalEncumbrance = victimAgent.GetTotalEncumbrance();
				this.CombatDifficultyMultiplier = Mission.Current.GetDamageMultiplierOfCombatDifficulty(victimAgent, attackerAgent);
				this.VictimHitPointRate = victimAgent.Health / victimAgent.HealthLimit;
				this.VictimAgentPosition = victimAgent.Position;
				this.VictimAgentMovementDirection = victimAgent.GetMovementDirection();
				this.VictimAgentVelocity = victimAgent.Velocity;
				this.VictimAgentMovementVelocity = victimAgent.MovementVelocity;
				this.VictimAgentFlags = victimAgent.GetAgentFlags();
				this.VictimAgentAIStateFlags = victimAgent.AIStateFlags;
				this.VictimAgentCharacter = victimAgent.Character;
				this.VictimBattleEnvironment = victimAgent.CurrentBattleEnvironment;
				this.VictimAgentOrigin = victimAgent.Origin;
				if (this.DoesVictimHaveRiderAgent)
				{
					Agent riderAgent = victimAgent.RiderAgent;
					this.IsVictimAgentRiderAgentMine = riderAgent.IsMine;
					this.VictimRiderAgentCharacter = riderAgent.Character;
					this.VictimRiderAgentOrigin = riderAgent.Origin;
					Formation formation = riderAgent.Formation;
					Agent agent = ((formation != null) ? formation.Captain : null);
					this.VictimCaptainCharacter = ((riderAgent != agent) ? ((agent != null) ? agent.Character : null) : null);
					this.VictimFormation = riderAgent.Formation;
				}
				else
				{
					Formation formation2 = victimAgent.Formation;
					Agent agent2 = ((formation2 != null) ? formation2.Captain : null);
					this.VictimCaptainCharacter = ((victimAgent != agent2) ? ((agent2 != null) ? agent2.Character : null) : null);
					this.VictimFormation = victimAgent.Formation;
				}
			}
			this.AttackerMovementDirectionAsAngle = 0f;
			this.AttackerAgentMountChargeDamageProperty = 0f;
			this.DoesAttackerHaveMountAgent = false;
			this.IsAttackerAgentMine = false;
			this.DoesAttackerHaveRiderAgent = false;
			this.IsAttackerAgentRiderAgentMine = false;
			this.IsAttackerAgentMount = false;
			this.IsAttackerAgentHuman = false;
			this.IsAttackerAgentActive = false;
			this.IsAttackerAgentDoingPassiveAttack = false;
			this.IsAttackerPlayer = false;
			this.AttackerAgentMovementVelocity = default(Vec2);
			this.AttackerAgentCharacter = null;
			this.AttackerRiderAgentCharacter = null;
			this.AttackerBattleEnvironment = BattleEnvironment.None;
			this.AttackerAgentMonster = null;
			this.AttackerAgentOrigin = null;
			this.AttackerRiderAgentOrigin = null;
			this.AttackerAgentPosition = default(Vec3);
			this.AttackerAgentMovementDirection = default(Vec2);
			this.AttackerAgentVelocity = default(Vec3);
			this.AttackerAgentCurrentWeaponOffset = default(Vec3);
			this.IsAttackerAIControlled = false;
			this.AttackerCaptainCharacter = null;
			this.AttackerFormation = null;
			this.AttackerHitPointRate = 0f;
			if (!this.IsAttackerAgentNull)
			{
				this.DoesAttackerHaveMountAgent = attackerAgent.HasMount;
				this.IsAttackerAgentMine = attackerAgent.IsMine;
				this.IsAttackerAgentMount = attackerAgent.IsMount;
				this.IsAttackerAgentHuman = attackerAgent.IsHuman;
				this.IsAttackerAgentActive = attackerAgent.IsActive();
				this.IsAttackerAgentDoingPassiveAttack = attackerAgent.IsDoingPassiveAttack;
				this.DoesAttackerHaveRiderAgent = attackerAgent.RiderAgent != null;
				this.IsAttackerAIControlled = attackerAgent.IsAIControlled;
				this.IsAttackerPlayer = attackerAgent.IsPlayerControlled;
				this.AttackerMovementDirectionAsAngle = attackerAgent.MovementDirectionAsAngle;
				this.AttackerAgentMountChargeDamageProperty = attackerAgent.GetAgentDrivenPropertyValue(DrivenProperty.MountChargeDamage);
				this.AttackerHitPointRate = attackerAgent.Health / attackerAgent.HealthLimit;
				this.AttackerAgentPosition = attackerAgent.Position;
				this.AttackerAgentMovementDirection = attackerAgent.GetMovementDirection();
				this.AttackerAgentVelocity = attackerAgent.Velocity;
				this.AttackerAgentMovementVelocity = attackerAgent.MovementVelocity;
				if (this.IsAttackerAgentActive)
				{
					this.AttackerAgentCurrentWeaponOffset = attackerAgent.GetCurWeaponOffset();
				}
				this.AttackerAgentCharacter = attackerAgent.Character;
				this.AttackerBattleEnvironment = attackerAgent.CurrentBattleEnvironment;
				this.AttackerAgentMonster = this.AttackerAgent.Monster;
				this.AttackerAgentOrigin = attackerAgent.Origin;
				if (this.DoesAttackerHaveRiderAgent)
				{
					Agent riderAgent2 = attackerAgent.RiderAgent;
					this.IsAttackerAgentRiderAgentMine = riderAgent2.IsMine;
					this.AttackerRiderAgentCharacter = riderAgent2.Character;
					this.AttackerRiderAgentOrigin = riderAgent2.Origin;
					Formation formation3 = riderAgent2.Formation;
					Agent agent3 = ((formation3 != null) ? formation3.Captain : null);
					this.AttackerCaptainCharacter = ((riderAgent2 != agent3) ? ((agent3 != null) ? agent3.Character : null) : null);
					this.AttackerFormation = riderAgent2.Formation;
				}
				else
				{
					Formation formation4 = attackerAgent.Formation;
					Agent agent4 = ((formation4 != null) ? formation4.Captain : null);
					this.AttackerCaptainCharacter = ((attackerAgent != agent4) ? ((agent4 != null) ? agent4.Character : null) : null);
					this.AttackerFormation = attackerAgent.Formation;
				}
			}
			this.CanGiveDamageToAgentShield = true;
			if (!this.IsVictimAgentSameWithAttackerAgent)
			{
				Mission mission = Mission.Current;
				missionWeapon = attackerWeapon;
				this.CanGiveDamageToAgentShield = mission.CanGiveDamageToAgentShield(attackerAgent, missionWeapon.CurrentUsageItem, victimAgent);
			}
		}

		// Token: 0x060015AF RID: 5551 RVA: 0x00051414 File Offset: 0x0004F614
		public AttackInformation(Agent attackerAgent, Agent victimAgent, float armorAmountFloat, WeaponComponentData shieldOnBack, AgentFlag victimAgentFlags, Agent.AIStateFlag victimAgentAIStateFlags, float victimAgentAbsorbedDamageRatio, float damageMultiplierOfBone, float combatDifficultyMultiplier, MissionWeapon attackerWeapon, MissionWeapon victimMainHandWeapon, MissionWeapon victimShield, bool canGiveDamageToAgentShield, bool isVictimAgentLeftStance, bool isFriendlyFire, bool doesAttackerHaveMountAgent, bool doesVictimHaveMountAgent, Vec2 attackerAgentMovementVelocity, Vec2 attackerAgentMountMovementDirection, float attackerMovementDirectionAsAngle, Vec2 victimAgentMovementVelocity, Vec2 victimAgentMountMovementDirection, float victimMovementDirectionAsAngle, bool isVictimAgentSameWithAttackerAgent, bool isAttackerAgentMine, bool doesAttackerHaveRiderAgent, bool isAttackerAgentRiderAgentMine, bool isAttackerAgentMount, bool isVictimAgentMine, bool doesVictimHaveRiderAgent, bool isVictimAgentRiderAgentMine, bool isVictimAgentMount, bool isAttackerAgentNull, bool isAttackerAIControlled, BasicCharacterObject attackerAgentCharacter, BasicCharacterObject attackerRiderAgentCharacter, BattleEnvironment attackerBattleEnvironment, Monster attackerAgentMonster, IAgentOriginBase attackerAgentOrigin, IAgentOriginBase attackerRiderAgentOrigin, BasicCharacterObject victimAgentCharacter, BasicCharacterObject victimRiderAgentCharacter, BattleEnvironment victimBattleEnvironment, IAgentOriginBase victimAgentOrigin, IAgentOriginBase victimRiderAgentOrigin, Vec3 attackerAgentPosition, Vec2 attackerAgentMovementDirection, Vec3 attackerAgentVelocity, float attackerAgentMountChargeDamageProperty, Vec3 attackerAgentCurrentWeaponOffset, bool isAttackerAgentHuman, bool isAttackerAgentActive, bool isAttackerAgentDoingPassiveAttack, bool isVictimAgentNull, float victimAgentScale, float victimAgentHealth, float victimAgentMaxHealth, float victimAgentWeight, float victimAgentTotalEncumbrance, bool isVictimAgentHuman, Vec3 victimAgentPosition, Vec2 victimAgentMovementDirection, Vec3 victimAgentVelocity, int weaponAttachBoneIndex, MissionWeapon offHandItem, bool isHeadShot, bool isVictimRiderAgentSameAsAttackerAgent, bool isAttackerPlayer, bool isVictimPlayer, DestructableComponent hitObjectDestructibleComponent)
		{
			this.AttackerAgent = attackerAgent;
			this.VictimAgent = victimAgent;
			this.ArmorAmountFloat = armorAmountFloat;
			this.ShieldOnBack = shieldOnBack;
			this.VictimAgentFlags = victimAgentFlags;
			this.VictimAgentAIStateFlags = victimAgentAIStateFlags;
			this.VictimAgentAbsorbedDamageRatio = victimAgentAbsorbedDamageRatio;
			this.DamageMultiplierOfBone = damageMultiplierOfBone;
			this.CombatDifficultyMultiplier = combatDifficultyMultiplier;
			this.AttackerWeapon = attackerWeapon;
			this.VictimMainHandWeapon = victimMainHandWeapon;
			this.VictimShield = victimShield;
			this.CanGiveDamageToAgentShield = canGiveDamageToAgentShield;
			this.IsVictimAgentLeftStance = isVictimAgentLeftStance;
			this.IsFriendlyFire = isFriendlyFire;
			this.DoesAttackerHaveMountAgent = doesAttackerHaveMountAgent;
			this.DoesVictimHaveMountAgent = doesVictimHaveMountAgent;
			this.AttackerAgentMovementVelocity = attackerAgentMovementVelocity;
			this.AttackerAgentMountMovementDirection = attackerAgentMountMovementDirection;
			this.AttackerMovementDirectionAsAngle = attackerMovementDirectionAsAngle;
			this.VictimAgentMovementVelocity = victimAgentMovementVelocity;
			this.VictimAgentMountMovementDirection = victimAgentMountMovementDirection;
			this.VictimMovementDirectionAsAngle = victimMovementDirectionAsAngle;
			this.IsVictimAgentSameWithAttackerAgent = isVictimAgentSameWithAttackerAgent;
			this.IsAttackerAgentMine = isAttackerAgentMine;
			this.DoesAttackerHaveRiderAgent = doesAttackerHaveRiderAgent;
			this.IsAttackerAgentRiderAgentMine = isAttackerAgentRiderAgentMine;
			this.IsAttackerAgentMount = isAttackerAgentMount;
			this.IsVictimAgentMine = isVictimAgentMine;
			this.DoesVictimHaveRiderAgent = doesVictimHaveRiderAgent;
			this.IsVictimAgentRiderAgentMine = isVictimAgentRiderAgentMine;
			this.IsVictimAgentMount = isVictimAgentMount;
			this.IsAttackerAgentNull = isAttackerAgentNull;
			this.IsAttackerAIControlled = isAttackerAIControlled;
			this.AttackerAgentCharacter = attackerAgentCharacter;
			this.AttackerBattleEnvironment = attackerBattleEnvironment;
			this.AttackerRiderAgentCharacter = attackerRiderAgentCharacter;
			this.AttackerAgentMonster = attackerAgentMonster;
			this.AttackerAgentOrigin = attackerAgentOrigin;
			this.AttackerRiderAgentOrigin = attackerRiderAgentOrigin;
			this.VictimAgentCharacter = victimAgentCharacter;
			this.VictimBattleEnvironment = victimBattleEnvironment;
			this.VictimRiderAgentCharacter = victimRiderAgentCharacter;
			this.VictimAgentOrigin = victimAgentOrigin;
			this.VictimRiderAgentOrigin = victimRiderAgentOrigin;
			this.AttackerAgentPosition = attackerAgentPosition;
			this.AttackerAgentMovementDirection = attackerAgentMovementDirection;
			this.AttackerAgentVelocity = attackerAgentVelocity;
			this.AttackerAgentMountChargeDamageProperty = attackerAgentMountChargeDamageProperty;
			this.AttackerAgentCurrentWeaponOffset = attackerAgentCurrentWeaponOffset;
			this.IsAttackerAgentHuman = isAttackerAgentHuman;
			this.IsAttackerAgentActive = isAttackerAgentActive;
			this.IsAttackerAgentDoingPassiveAttack = isAttackerAgentDoingPassiveAttack;
			this.VictimAgentScale = victimAgentScale;
			this.IsVictimAgentNull = isVictimAgentNull;
			this.VictimAgentHealth = victimAgentHealth;
			this.VictimAgentMaxHealth = victimAgentMaxHealth;
			this.VictimAgentWeight = victimAgentWeight;
			this.VictimAgentTotalEncumbrance = victimAgentTotalEncumbrance;
			this.IsVictimAgentHuman = isVictimAgentHuman;
			this.VictimAgentPosition = victimAgentPosition;
			this.VictimAgentMovementDirection = victimAgentMovementDirection;
			this.VictimAgentVelocity = victimAgentVelocity;
			this.WeaponAttachBoneIndex = weaponAttachBoneIndex;
			this.OffHandItem = offHandItem;
			this.IsHeadShot = isHeadShot;
			this.IsVictimRiderAgentSameAsAttackerAgent = isVictimRiderAgentSameAsAttackerAgent;
			this.AttackerCaptainCharacter = null;
			this.VictimCaptainCharacter = null;
			this.VictimFormation = null;
			this.AttackerFormation = null;
			this.AttackerHitPointRate = 1f;
			this.VictimHitPointRate = 1f;
			this.IsAttackerPlayer = isAttackerPlayer;
			this.IsVictimPlayer = isVictimPlayer;
			this.HitObjectDestructibleComponent = hitObjectDestructibleComponent;
		}

		// Token: 0x04000666 RID: 1638
		public Agent AttackerAgent;

		// Token: 0x04000667 RID: 1639
		public Agent VictimAgent;

		// Token: 0x04000668 RID: 1640
		public float ArmorAmountFloat;

		// Token: 0x04000669 RID: 1641
		public WeaponComponentData ShieldOnBack;

		// Token: 0x0400066A RID: 1642
		public AgentFlag VictimAgentFlags;

		// Token: 0x0400066B RID: 1643
		public Agent.AIStateFlag VictimAgentAIStateFlags;

		// Token: 0x0400066C RID: 1644
		public float VictimAgentAbsorbedDamageRatio;

		// Token: 0x0400066D RID: 1645
		public float DamageMultiplierOfBone;

		// Token: 0x0400066E RID: 1646
		public float CombatDifficultyMultiplier;

		// Token: 0x0400066F RID: 1647
		public MissionWeapon AttackerWeapon;

		// Token: 0x04000670 RID: 1648
		public MissionWeapon VictimMainHandWeapon;

		// Token: 0x04000671 RID: 1649
		public MissionWeapon VictimShield;

		// Token: 0x04000672 RID: 1650
		public bool CanGiveDamageToAgentShield;

		// Token: 0x04000673 RID: 1651
		public bool IsVictimAgentLeftStance;

		// Token: 0x04000674 RID: 1652
		public bool IsFriendlyFire;

		// Token: 0x04000675 RID: 1653
		public bool DoesAttackerHaveMountAgent;

		// Token: 0x04000676 RID: 1654
		public bool DoesVictimHaveMountAgent;

		// Token: 0x04000677 RID: 1655
		public Vec2 AttackerAgentMovementVelocity;

		// Token: 0x04000678 RID: 1656
		public Vec2 AttackerAgentMountMovementDirection;

		// Token: 0x04000679 RID: 1657
		public float AttackerMovementDirectionAsAngle;

		// Token: 0x0400067A RID: 1658
		public Vec2 VictimAgentMovementVelocity;

		// Token: 0x0400067B RID: 1659
		public Vec2 VictimAgentMountMovementDirection;

		// Token: 0x0400067C RID: 1660
		public float VictimMovementDirectionAsAngle;

		// Token: 0x0400067D RID: 1661
		public bool IsVictimAgentSameWithAttackerAgent;

		// Token: 0x0400067E RID: 1662
		public bool IsAttackerAgentMine;

		// Token: 0x0400067F RID: 1663
		public bool DoesAttackerHaveRiderAgent;

		// Token: 0x04000680 RID: 1664
		public bool IsAttackerAgentRiderAgentMine;

		// Token: 0x04000681 RID: 1665
		public bool IsAttackerAgentMount;

		// Token: 0x04000682 RID: 1666
		public bool IsVictimAgentMine;

		// Token: 0x04000683 RID: 1667
		public bool DoesVictimHaveRiderAgent;

		// Token: 0x04000684 RID: 1668
		public bool IsVictimAgentRiderAgentMine;

		// Token: 0x04000685 RID: 1669
		public bool IsVictimAgentMount;

		// Token: 0x04000686 RID: 1670
		public bool IsAttackerAgentNull;

		// Token: 0x04000687 RID: 1671
		public bool IsAttackerAIControlled;

		// Token: 0x04000688 RID: 1672
		public BasicCharacterObject AttackerAgentCharacter;

		// Token: 0x04000689 RID: 1673
		public BasicCharacterObject AttackerRiderAgentCharacter;

		// Token: 0x0400068A RID: 1674
		public Monster AttackerAgentMonster;

		// Token: 0x0400068B RID: 1675
		public IAgentOriginBase AttackerAgentOrigin;

		// Token: 0x0400068C RID: 1676
		public IAgentOriginBase AttackerRiderAgentOrigin;

		// Token: 0x0400068D RID: 1677
		public BasicCharacterObject VictimAgentCharacter;

		// Token: 0x0400068E RID: 1678
		public BasicCharacterObject VictimRiderAgentCharacter;

		// Token: 0x0400068F RID: 1679
		public IAgentOriginBase VictimAgentOrigin;

		// Token: 0x04000690 RID: 1680
		public IAgentOriginBase VictimRiderAgentOrigin;

		// Token: 0x04000691 RID: 1681
		public BattleEnvironment AttackerBattleEnvironment;

		// Token: 0x04000692 RID: 1682
		public BattleEnvironment VictimBattleEnvironment;

		// Token: 0x04000693 RID: 1683
		public Vec3 AttackerAgentPosition;

		// Token: 0x04000694 RID: 1684
		public Vec2 AttackerAgentMovementDirection;

		// Token: 0x04000695 RID: 1685
		public Vec3 AttackerAgentVelocity;

		// Token: 0x04000696 RID: 1686
		public Vec3 VictimAgentPosition;

		// Token: 0x04000697 RID: 1687
		public float AttackerAgentMountChargeDamageProperty;

		// Token: 0x04000698 RID: 1688
		public Vec3 VictimAgentVelocity;

		// Token: 0x04000699 RID: 1689
		public Vec2 VictimAgentMovementDirection;

		// Token: 0x0400069A RID: 1690
		public Vec3 AttackerAgentCurrentWeaponOffset;

		// Token: 0x0400069B RID: 1691
		public bool IsAttackerAgentHuman;

		// Token: 0x0400069C RID: 1692
		public bool IsAttackerAgentActive;

		// Token: 0x0400069D RID: 1693
		public bool IsAttackerAgentDoingPassiveAttack;

		// Token: 0x0400069E RID: 1694
		public bool IsVictimAgentNull;

		// Token: 0x0400069F RID: 1695
		public float VictimAgentScale;

		// Token: 0x040006A0 RID: 1696
		public float VictimAgentWeight;

		// Token: 0x040006A1 RID: 1697
		public float VictimAgentHealth;

		// Token: 0x040006A2 RID: 1698
		public float VictimAgentMaxHealth;

		// Token: 0x040006A3 RID: 1699
		public float VictimAgentTotalEncumbrance;

		// Token: 0x040006A4 RID: 1700
		public bool IsVictimAgentHuman;

		// Token: 0x040006A5 RID: 1701
		public int WeaponAttachBoneIndex;

		// Token: 0x040006A6 RID: 1702
		public MissionWeapon OffHandItem;

		// Token: 0x040006A7 RID: 1703
		public bool IsHeadShot;

		// Token: 0x040006A8 RID: 1704
		public bool IsVictimRiderAgentSameAsAttackerAgent;

		// Token: 0x040006A9 RID: 1705
		public BasicCharacterObject AttackerCaptainCharacter;

		// Token: 0x040006AA RID: 1706
		public BasicCharacterObject VictimCaptainCharacter;

		// Token: 0x040006AB RID: 1707
		public Formation AttackerFormation;

		// Token: 0x040006AC RID: 1708
		public Formation VictimFormation;

		// Token: 0x040006AD RID: 1709
		public float AttackerHitPointRate;

		// Token: 0x040006AE RID: 1710
		public float VictimHitPointRate;

		// Token: 0x040006AF RID: 1711
		public bool IsAttackerPlayer;

		// Token: 0x040006B0 RID: 1712
		public bool IsVictimPlayer;

		// Token: 0x040006B1 RID: 1713
		public DestructableComponent HitObjectDestructibleComponent;
	}
}
