using System;
using TaleWorlds.Core;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x020002E6 RID: 742
	public class MPPerksAgentComponent : AgentComponent
	{
		// Token: 0x06002B3A RID: 11066 RVA: 0x000A6E6C File Offset: 0x000A506C
		public MPPerksAgentComponent(Agent agent)
			: base(agent)
		{
			this.Agent.OnAgentHealthChanged += this.OnHealthChanged;
			if (this.Agent.HasMount)
			{
				this.Agent.MountAgent.OnAgentHealthChanged += this.OnMountHealthChanged;
			}
		}

		// Token: 0x06002B3B RID: 11067 RVA: 0x000A6EC0 File Offset: 0x000A50C0
		public override void OnMount(Agent mount)
		{
			mount.OnAgentHealthChanged += this.OnMountHealthChanged;
			mount.UpdateAgentProperties();
			MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(this.Agent);
			if (perkHandler == null)
			{
				return;
			}
			perkHandler.OnEvent(this.Agent, MPPerkCondition.PerkEventFlags.MountChange);
		}

		// Token: 0x06002B3C RID: 11068 RVA: 0x000A6EFA File Offset: 0x000A50FA
		public override void OnDismount(Agent mount)
		{
			mount.OnAgentHealthChanged -= this.OnMountHealthChanged;
			mount.UpdateAgentProperties();
			MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(this.Agent);
			if (perkHandler == null)
			{
				return;
			}
			perkHandler.OnEvent(this.Agent, MPPerkCondition.PerkEventFlags.MountChange);
		}

		// Token: 0x06002B3D RID: 11069 RVA: 0x000A6F34 File Offset: 0x000A5134
		public override void OnItemPickup(SpawnedItemEntity item)
		{
			if (!item.WeaponCopy.IsEmpty && item.WeaponCopy.Item.ItemType == ItemObject.ItemTypeEnum.Banner)
			{
				MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(this.Agent);
				if (perkHandler == null)
				{
					return;
				}
				perkHandler.OnEvent(MPPerkCondition.PerkEventFlags.BannerPickUp);
			}
		}

		// Token: 0x06002B3E RID: 11070 RVA: 0x000A6F7F File Offset: 0x000A517F
		public override void OnWeaponDrop(MissionWeapon droppedWeapon)
		{
			if (!droppedWeapon.IsEmpty && droppedWeapon.Item.ItemType == ItemObject.ItemTypeEnum.Banner)
			{
				MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(this.Agent);
				if (perkHandler == null)
				{
					return;
				}
				perkHandler.OnEvent(MPPerkCondition.PerkEventFlags.BannerDrop);
			}
		}

		// Token: 0x06002B3F RID: 11071 RVA: 0x000A6FB4 File Offset: 0x000A51B4
		public override void OnAgentRemoved()
		{
			if (this.Agent.HasMount)
			{
				this.Agent.MountAgent.OnAgentHealthChanged -= this.OnMountHealthChanged;
				this.Agent.MountAgent.UpdateAgentProperties();
			}
		}

		// Token: 0x06002B40 RID: 11072 RVA: 0x000A6FEF File Offset: 0x000A51EF
		private void OnHealthChanged(Agent agent, float oldHealth, float newHealth)
		{
			MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(this.Agent);
			if (perkHandler == null)
			{
				return;
			}
			perkHandler.OnEvent(agent, MPPerkCondition.PerkEventFlags.HealthChange);
		}

		// Token: 0x06002B41 RID: 11073 RVA: 0x000A7008 File Offset: 0x000A5208
		private void OnMountHealthChanged(Agent agent, float oldHealth, float newHealth)
		{
			if (!this.Agent.IsActive() || this.Agent.MountAgent != agent)
			{
				agent.OnAgentHealthChanged -= this.OnMountHealthChanged;
				return;
			}
			MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(this.Agent);
			if (perkHandler == null)
			{
				return;
			}
			perkHandler.OnEvent(this.Agent, MPPerkCondition.PerkEventFlags.MountHealthChange);
		}
	}
}
