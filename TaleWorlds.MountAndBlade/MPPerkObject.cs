using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Conditions;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031C RID: 796
	public class MPPerkObject : IReadOnlyPerkObject
	{
		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x06002D99 RID: 11673 RVA: 0x000AFBB1 File Offset: 0x000ADDB1
		public TextObject Name
		{
			get
			{
				return new TextObject(this._name, null);
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x06002D9A RID: 11674 RVA: 0x000AFBBF File Offset: 0x000ADDBF
		public TextObject Description
		{
			get
			{
				return new TextObject(this._description, null);
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x06002D9B RID: 11675 RVA: 0x000AFBCD File Offset: 0x000ADDCD
		public bool HasBannerBearer { get; }

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x06002D9C RID: 11676 RVA: 0x000AFBD5 File Offset: 0x000ADDD5
		public List<string> GameModes { get; }

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x06002D9D RID: 11677 RVA: 0x000AFBDD File Offset: 0x000ADDDD
		public int PerkListIndex { get; }

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06002D9E RID: 11678 RVA: 0x000AFBE5 File Offset: 0x000ADDE5
		public string IconId { get; }

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002D9F RID: 11679 RVA: 0x000AFBED File Offset: 0x000ADDED
		public string HeroIdleAnimOverride { get; }

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002DA0 RID: 11680 RVA: 0x000AFBF5 File Offset: 0x000ADDF5
		public string HeroMountIdleAnimOverride { get; }

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002DA1 RID: 11681 RVA: 0x000AFBFD File Offset: 0x000ADDFD
		public string TroopIdleAnimOverride { get; }

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002DA2 RID: 11682 RVA: 0x000AFC05 File Offset: 0x000ADE05
		public string TroopMountIdleAnimOverride { get; }

		// Token: 0x06002DA3 RID: 11683 RVA: 0x000AFC10 File Offset: 0x000ADE10
		public MPPerkObject(MissionPeer peer, string name, string description, List<string> gameModes, int perkListIndex, string iconId, IEnumerable<MPConditionalEffect> conditionalEffects, IEnumerable<MPPerkEffectBase> effects, string heroIdleAnimOverride, string heroMountIdleAnimOverride, string troopIdleAnimOverride, string troopMountIdleAnimOverride)
		{
			this._peer = peer;
			this._name = name;
			this._description = description;
			this.GameModes = gameModes;
			this.PerkListIndex = perkListIndex;
			this.IconId = iconId;
			this._conditionalEffects = new MPConditionalEffect.ConditionalEffectContainer(conditionalEffects);
			this._effects = new List<MPPerkEffectBase>(effects);
			this.HeroIdleAnimOverride = heroIdleAnimOverride;
			this.HeroMountIdleAnimOverride = heroMountIdleAnimOverride;
			this.TroopIdleAnimOverride = troopIdleAnimOverride;
			this.TroopMountIdleAnimOverride = troopMountIdleAnimOverride;
			this._perkEventFlags = MPPerkCondition.PerkEventFlags.None;
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				foreach (MPPerkCondition mpperkCondition in mpconditionalEffect.Conditions)
				{
					this._perkEventFlags |= mpperkCondition.EventFlags;
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect2 in this._conditionalEffects)
			{
				using (List<MPPerkCondition>.Enumerator enumerator2 = mpconditionalEffect2.Conditions.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current is BannerBearerCondition)
						{
							this.HasBannerBearer = true;
						}
					}
				}
			}
		}

		// Token: 0x06002DA4 RID: 11684 RVA: 0x000AFDA0 File Offset: 0x000ADFA0
		private MPPerkObject(XmlNode node)
		{
			this._peer = null;
			this._conditionalEffects = new MPConditionalEffect.ConditionalEffectContainer();
			this._effects = new List<MPPerkEffectBase>();
			this._name = node.Attributes["name"].Value;
			this._description = node.Attributes["description"].Value;
			this.GameModes = new List<string>(node.Attributes["game_mode"].Value.Split(new char[] { ',' }));
			for (int i = 0; i < this.GameModes.Count; i++)
			{
				this.GameModes[i] = this.GameModes[i].Trim();
			}
			this.IconId = node.Attributes["icon"].Value;
			this.PerkListIndex = 0;
			XmlNode xmlNode = node.Attributes["perk_list"];
			if (xmlNode != null)
			{
				this.PerkListIndex = Convert.ToInt32(xmlNode.Value);
				int perkListIndex = this.PerkListIndex;
				this.PerkListIndex = perkListIndex - 1;
			}
			foreach (object obj in node.ChildNodes)
			{
				XmlNode xmlNode2 = (XmlNode)obj;
				if (xmlNode2.NodeType != XmlNodeType.Comment && xmlNode2.NodeType != XmlNodeType.SignificantWhitespace)
				{
					if (xmlNode2.Name == "ConditionalEffect")
					{
						this._conditionalEffects.Add(new MPConditionalEffect(this.GameModes, xmlNode2));
					}
					else if (xmlNode2.Name == "Effect")
					{
						this._effects.Add(MPPerkEffect.CreateFrom(xmlNode2));
					}
					else if (xmlNode2.Name == "OnSpawnEffect")
					{
						this._effects.Add(MPOnSpawnPerkEffect.CreateFrom(xmlNode2));
					}
					else if (xmlNode2.Name == "RandomOnSpawnEffect")
					{
						this._effects.Add(MPRandomOnSpawnPerkEffect.CreateFrom(xmlNode2));
					}
					else
					{
						Debug.FailedAssert("Unknown child element", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\Perks\\MPPerkObject.cs", ".ctor", 750);
					}
				}
			}
			XmlAttribute xmlAttribute = node.Attributes["hero_idle_anim"];
			this.HeroIdleAnimOverride = ((xmlAttribute != null) ? xmlAttribute.Value : null);
			XmlAttribute xmlAttribute2 = node.Attributes["hero_mount_idle_anim"];
			this.HeroMountIdleAnimOverride = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
			XmlAttribute xmlAttribute3 = node.Attributes["troop_idle_anim"];
			this.TroopIdleAnimOverride = ((xmlAttribute3 != null) ? xmlAttribute3.Value : null);
			XmlAttribute xmlAttribute4 = node.Attributes["troop_mount_idle_anim"];
			this.TroopMountIdleAnimOverride = ((xmlAttribute4 != null) ? xmlAttribute4.Value : null);
			this._perkEventFlags = MPPerkCondition.PerkEventFlags.None;
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				foreach (MPPerkCondition mpperkCondition in mpconditionalEffect.Conditions)
				{
					this._perkEventFlags |= mpperkCondition.EventFlags;
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect2 in this._conditionalEffects)
			{
				using (List<MPPerkCondition>.Enumerator enumerator3 = mpconditionalEffect2.Conditions.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						if (enumerator3.Current is BannerBearerCondition)
						{
							this.HasBannerBearer = true;
						}
					}
				}
			}
		}

		// Token: 0x06002DA5 RID: 11685 RVA: 0x000B0188 File Offset: 0x000AE388
		public MPPerkObject Clone(MissionPeer peer)
		{
			return new MPPerkObject(peer, this._name, this._description, this.GameModes, this.PerkListIndex, this.IconId, this._conditionalEffects, this._effects, this.HeroIdleAnimOverride, this.HeroMountIdleAnimOverride, this.TroopIdleAnimOverride, this.TroopMountIdleAnimOverride);
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000B01DD File Offset: 0x000AE3DD
		public void Reset()
		{
			this._conditionalEffects.ResetStates();
		}

		// Token: 0x06002DA7 RID: 11687 RVA: 0x000B01EC File Offset: 0x000AE3EC
		private void OnEvent(bool isWarmup, MPPerkCondition.PerkEventFlags flags)
		{
			if ((flags & this._perkEventFlags) != MPPerkCondition.PerkEventFlags.None)
			{
				foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
				{
					if ((flags & mpconditionalEffect.EventFlags) != MPPerkCondition.PerkEventFlags.None)
					{
						mpconditionalEffect.OnEvent(isWarmup, this._peer, this._conditionalEffects);
					}
				}
			}
		}

		// Token: 0x06002DA8 RID: 11688 RVA: 0x000B0260 File Offset: 0x000AE460
		private void OnEvent(bool isWarmup, Agent agent, MPPerkCondition.PerkEventFlags flags)
		{
			if (((agent != null) ? agent.MissionPeer : null) == null && agent != null)
			{
				MissionPeer owningAgentMissionPeer = agent.OwningAgentMissionPeer;
			}
			if ((flags & this._perkEventFlags) != MPPerkCondition.PerkEventFlags.None)
			{
				foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
				{
					if ((flags & mpconditionalEffect.EventFlags) != MPPerkCondition.PerkEventFlags.None)
					{
						mpconditionalEffect.OnEvent(isWarmup, agent, this._conditionalEffects);
					}
				}
			}
		}

		// Token: 0x06002DA9 RID: 11689 RVA: 0x000B02E8 File Offset: 0x000AE4E8
		private void OnTick(bool isWarmup, int tickCount)
		{
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.IsTickRequired)
				{
					mpconditionalEffect.OnTick(isWarmup, this._peer, tickCount);
				}
			}
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if ((!isWarmup || !mpperkEffectBase.IsDisabledInWarmup) && mpperkEffectBase.IsTickRequired)
				{
					mpperkEffectBase.OnTick(this._peer, tickCount);
				}
			}
		}

		// Token: 0x06002DAA RID: 11690 RVA: 0x000B03A8 File Offset: 0x000AE5A8
		private float GetDamage(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetDamage(attackerWeapon, damageType, isAlternativeAttack);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetDamage(attackerWeapon, damageType, isAlternativeAttack);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DAB RID: 11691 RVA: 0x000B04CC File Offset: 0x000AE6CC
		private float GetMountDamage(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetMountDamage(attackerWeapon, damageType, isAlternativeAttack);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetMountDamage(attackerWeapon, damageType, isAlternativeAttack);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DAC RID: 11692 RVA: 0x000B05F0 File Offset: 0x000AE7F0
		private float GetDamageTaken(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetDamageTaken(attackerWeapon, damageType);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetDamageTaken(attackerWeapon, damageType);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DAD RID: 11693 RVA: 0x000B0710 File Offset: 0x000AE910
		private float GetMountDamageTaken(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetMountDamageTaken(attackerWeapon, damageType);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetMountDamageTaken(attackerWeapon, damageType);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DAE RID: 11694 RVA: 0x000B0830 File Offset: 0x000AEA30
		private float GetSpeedBonusEffectiveness(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetSpeedBonusEffectiveness(agent, attackerWeapon, damageType);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetSpeedBonusEffectiveness(agent, attackerWeapon, damageType);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DAF RID: 11695 RVA: 0x000B0954 File Offset: 0x000AEB54
		private float GetShieldDamage(bool isWarmup, Agent attacker, Agent defender, bool isCorrectSideBlock)
		{
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetShieldDamage(isCorrectSideBlock);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(attacker))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetShieldDamage(isCorrectSideBlock);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DB0 RID: 11696 RVA: 0x000B0A58 File Offset: 0x000AEC58
		private float GetShieldDamageTaken(bool isWarmup, Agent attacker, Agent defender, bool isCorrectSideBlock)
		{
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetShieldDamageTaken(isCorrectSideBlock);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(defender))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetShieldDamageTaken(isCorrectSideBlock);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DB1 RID: 11697 RVA: 0x000B0B5C File Offset: 0x000AED5C
		private float GetRangedAccuracy(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetRangedAccuracy();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetRangedAccuracy();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DB2 RID: 11698 RVA: 0x000B0C78 File Offset: 0x000AEE78
		private float GetThrowingWeaponSpeed(bool isWarmup, Agent agent, WeaponComponentData attackerWeapon)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetThrowingWeaponSpeed(attackerWeapon);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetThrowingWeaponSpeed(attackerWeapon);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DB3 RID: 11699 RVA: 0x000B0D94 File Offset: 0x000AEF94
		private float GetDamageInterruptionThreshold(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetDamageInterruptionThreshold();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetDamageInterruptionThreshold();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DB4 RID: 11700 RVA: 0x000B0EB0 File Offset: 0x000AF0B0
		private float GetMountManeuver(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetMountManeuver();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetMountManeuver();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DB5 RID: 11701 RVA: 0x000B0FCC File Offset: 0x000AF1CC
		private float GetMountSpeed(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetMountSpeed();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetMountSpeed();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DB6 RID: 11702 RVA: 0x000B10E8 File Offset: 0x000AF2E8
		private float GetRangedHeadShotDamage(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetRangedHeadShotDamage();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetRangedHeadShotDamage();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DB7 RID: 11703 RVA: 0x000B1204 File Offset: 0x000AF404
		public int GetExtraTroopCount(bool isWarmup)
		{
			int num = 0;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				IOnSpawnPerkEffect onSpawnPerkEffect = mpperkEffectBase as IOnSpawnPerkEffect;
				if (onSpawnPerkEffect != null && (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup))
				{
					num += onSpawnPerkEffect.GetExtraTroopCount();
				}
			}
			return num;
		}

		// Token: 0x06002DB8 RID: 11704 RVA: 0x000B1274 File Offset: 0x000AF474
		public List<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isWarmup, bool isPlayer, List<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments, bool getAllEquipments = false)
		{
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				IOnSpawnPerkEffect onSpawnPerkEffect = mpperkEffectBase as IOnSpawnPerkEffect;
				if (onSpawnPerkEffect != null && (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup))
				{
					alternativeEquipments = onSpawnPerkEffect.GetAlternativeEquipments(isPlayer, alternativeEquipments, getAllEquipments);
				}
			}
			return alternativeEquipments;
		}

		// Token: 0x06002DB9 RID: 11705 RVA: 0x000B12E4 File Offset: 0x000AF4E4
		private float GetDrivenPropertyBonus(bool isWarmup, Agent agent, DrivenProperty drivenProperty, float baseValue)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetDrivenPropertyBonus(drivenProperty, baseValue);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetDrivenPropertyBonus(drivenProperty, baseValue);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DBA RID: 11706 RVA: 0x000B1404 File Offset: 0x000AF604
		public float GetDrivenPropertyBonusOnSpawn(bool isWarmup, bool isPlayer, DrivenProperty drivenProperty, float baseValue)
		{
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				IOnSpawnPerkEffect onSpawnPerkEffect = mpperkEffectBase as IOnSpawnPerkEffect;
				if (onSpawnPerkEffect != null && (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup))
				{
					num += onSpawnPerkEffect.GetDrivenPropertyBonusOnSpawn(isPlayer, drivenProperty, baseValue);
				}
			}
			return num;
		}

		// Token: 0x06002DBB RID: 11707 RVA: 0x000B147C File Offset: 0x000AF67C
		public float GetHitpoints(bool isWarmup, bool isPlayer)
		{
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				IOnSpawnPerkEffect onSpawnPerkEffect = mpperkEffectBase as IOnSpawnPerkEffect;
				if (onSpawnPerkEffect != null && (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup))
				{
					num += onSpawnPerkEffect.GetHitpoints(isPlayer);
				}
			}
			return num;
		}

		// Token: 0x06002DBC RID: 11708 RVA: 0x000B14F0 File Offset: 0x000AF6F0
		private float GetEncumbrance(bool isWarmup, Agent agent, bool isOnBody)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			float num = 0f;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetEncumbrance(isOnBody);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetEncumbrance(isOnBody);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x000B160C File Offset: 0x000AF80C
		private int GetGoldOnKill(bool isWarmup, Agent agent, float attackerValue, float victimValue)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			int num = 0;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetGoldOnKill(attackerValue, victimValue);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetGoldOnKill(attackerValue, victimValue);
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000B1728 File Offset: 0x000AF928
		private int GetGoldOnAssist(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			int num = 0;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetGoldOnAssist();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetGoldOnAssist();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DBF RID: 11711 RVA: 0x000B1840 File Offset: 0x000AFA40
		private int GetRewardedGoldOnAssist(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			int num = 0;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					num += mpperkEffectBase.GetRewardedGoldOnAssist();
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							num += mpperkEffectBase2.GetRewardedGoldOnAssist();
						}
					}
				}
			}
			return num;
		}

		// Token: 0x06002DC0 RID: 11712 RVA: 0x000B1958 File Offset: 0x000AFB58
		private bool GetIsTeamRewardedOnDeath(bool isWarmup, Agent agent)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if ((!isWarmup || !mpperkEffectBase.IsDisabledInWarmup) && mpperkEffectBase.GetIsTeamRewardedOnDeath())
				{
					return true;
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if ((!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup) && mpperkEffectBase2.GetIsTeamRewardedOnDeath())
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06002DC1 RID: 11713 RVA: 0x000B1A78 File Offset: 0x000AFC78
		private void CalculateRewardedGoldOnDeath(bool isWarmup, Agent agent, List<ValueTuple<MissionPeer, int>> teamMembers)
		{
			Agent agent2;
			if ((agent2 = agent) == null)
			{
				MissionPeer peer = this._peer;
				agent2 = ((peer != null) ? peer.ControlledAgent : null);
			}
			agent = agent2;
			teamMembers.Shuffle<ValueTuple<MissionPeer, int>>();
			foreach (MPPerkEffectBase mpperkEffectBase in this._effects)
			{
				if (!isWarmup || !mpperkEffectBase.IsDisabledInWarmup)
				{
					mpperkEffectBase.CalculateRewardedGoldOnDeath(agent, teamMembers);
				}
			}
			foreach (MPConditionalEffect mpconditionalEffect in this._conditionalEffects)
			{
				if (mpconditionalEffect.Check(agent))
				{
					foreach (MPPerkEffectBase mpperkEffectBase2 in mpconditionalEffect.Effects)
					{
						if (!isWarmup || !mpperkEffectBase2.IsDisabledInWarmup)
						{
							mpperkEffectBase2.CalculateRewardedGoldOnDeath(agent, teamMembers);
						}
					}
				}
			}
		}

		// Token: 0x06002DC2 RID: 11714 RVA: 0x000B1B8C File Offset: 0x000AFD8C
		public static int GetTroopCount(MultiplayerClassDivisions.MPHeroClass heroClass, int botsPerFormation, MPPerkObject.MPOnSpawnPerkHandler onSpawnPerkHandler)
		{
			int num = MathF.Ceiling((float)botsPerFormation * heroClass.TroopMultiplier - 1E-05f);
			if (onSpawnPerkHandler != null)
			{
				num += (int)onSpawnPerkHandler.GetExtraTroopCount();
			}
			return MathF.Max(num, 1);
		}

		// Token: 0x06002DC3 RID: 11715 RVA: 0x000B1BC2 File Offset: 0x000AFDC2
		public static IReadOnlyPerkObject Deserialize(XmlNode node)
		{
			return new MPPerkObject(node);
		}

		// Token: 0x06002DC4 RID: 11716 RVA: 0x000B1BCC File Offset: 0x000AFDCC
		public static MPPerkObject.MPPerkHandler GetPerkHandler(Agent agent)
		{
			object obj;
			if (agent == null)
			{
				obj = null;
			}
			else
			{
				MissionPeer missionPeer = agent.MissionPeer;
				obj = ((missionPeer != null) ? missionPeer.SelectedPerks : null);
			}
			object obj2;
			if ((obj2 = obj) == null)
			{
				if (agent == null)
				{
					obj2 = null;
				}
				else
				{
					MissionPeer owningAgentMissionPeer = agent.OwningAgentMissionPeer;
					obj2 = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.SelectedPerks : null);
				}
			}
			MBReadOnlyList<MPPerkObject> mbreadOnlyList = obj2;
			if (mbreadOnlyList != null && mbreadOnlyList.Count > 0 && !agent.IsMount)
			{
				return new MPPerkObject.MPPerkHandlerInstance(agent);
			}
			return null;
		}

		// Token: 0x06002DC5 RID: 11717 RVA: 0x000B1C2C File Offset: 0x000AFE2C
		public static MPPerkObject.MPPerkHandler GetPerkHandler(MissionPeer peer)
		{
			MBReadOnlyList<MPPerkObject> mbreadOnlyList = ((peer != null) ? peer.SelectedPerks : null) ?? ((peer != null) ? peer.SelectedPerks : null);
			if (mbreadOnlyList != null && mbreadOnlyList.Count > 0)
			{
				return new MPPerkObject.MPPerkHandlerInstance(peer);
			}
			return null;
		}

		// Token: 0x06002DC6 RID: 11718 RVA: 0x000B1C6C File Offset: 0x000AFE6C
		public static MPPerkObject.MPCombatPerkHandler GetCombatPerkHandler(Agent attacker, Agent defender)
		{
			Agent agent = ((attacker != null && attacker.IsMount) ? attacker.RiderAgent : attacker);
			object obj;
			if (agent == null)
			{
				obj = null;
			}
			else
			{
				MissionPeer missionPeer = agent.MissionPeer;
				obj = ((missionPeer != null) ? missionPeer.SelectedPerks : null);
			}
			object obj2;
			if ((obj2 = obj) == null)
			{
				if (agent == null)
				{
					obj2 = null;
				}
				else
				{
					MissionPeer owningAgentMissionPeer = agent.OwningAgentMissionPeer;
					obj2 = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.SelectedPerks : null);
				}
			}
			MBReadOnlyList<MPPerkObject> mbreadOnlyList = obj2;
			Agent agent2 = ((defender != null && defender.IsMount) ? defender.RiderAgent : defender);
			object obj3;
			if (agent2 == null)
			{
				obj3 = null;
			}
			else
			{
				MissionPeer missionPeer2 = agent2.MissionPeer;
				obj3 = ((missionPeer2 != null) ? missionPeer2.SelectedPerks : null);
			}
			object obj4;
			if ((obj4 = obj3) == null)
			{
				if (agent2 == null)
				{
					obj4 = null;
				}
				else
				{
					MissionPeer owningAgentMissionPeer2 = agent2.OwningAgentMissionPeer;
					obj4 = ((owningAgentMissionPeer2 != null) ? owningAgentMissionPeer2.SelectedPerks : null);
				}
			}
			MBReadOnlyList<MPPerkObject> mbreadOnlyList2 = obj4;
			if (attacker != defender && ((mbreadOnlyList != null && mbreadOnlyList.Count > 0) || (mbreadOnlyList2 != null && mbreadOnlyList2.Count > 0)))
			{
				return new MPPerkObject.MPCombatPerkHandlerInstance(attacker, defender);
			}
			return null;
		}

		// Token: 0x06002DC7 RID: 11719 RVA: 0x000B1D32 File Offset: 0x000AFF32
		public static MPPerkObject.MPOnSpawnPerkHandler GetOnSpawnPerkHandler(MissionPeer peer)
		{
			if ((((peer != null) ? peer.SelectedPerks : null) ?? ((peer != null) ? peer.SelectedPerks : null)) != null)
			{
				return new MPPerkObject.MPOnSpawnPerkHandlerInstance(peer);
			}
			return null;
		}

		// Token: 0x06002DC8 RID: 11720 RVA: 0x000B1D5A File Offset: 0x000AFF5A
		public static MPPerkObject.MPOnSpawnPerkHandler GetOnSpawnPerkHandler(IEnumerable<IReadOnlyPerkObject> perks)
		{
			if (perks != null)
			{
				return new MPPerkObject.MPOnSpawnPerkHandlerInstance(perks);
			}
			return null;
		}

		// Token: 0x06002DC9 RID: 11721 RVA: 0x000B1D68 File Offset: 0x000AFF68
		public static void RaiseEventForAllPeers(MPPerkCondition.PerkEventFlags flags)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(networkCommunicator.GetComponent<MissionPeer>());
					if (perkHandler != null)
					{
						perkHandler.OnEvent(flags);
					}
				}
			}
		}

		// Token: 0x06002DCA RID: 11722 RVA: 0x000B1DD0 File Offset: 0x000AFFD0
		public static void RaiseEventForAllPeersOnTeam(Team side, MPPerkCondition.PerkEventFlags flags)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.Team == side)
					{
						MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(component);
						if (perkHandler != null)
						{
							perkHandler.OnEvent(flags);
						}
					}
				}
			}
		}

		// Token: 0x06002DCB RID: 11723 RVA: 0x000B1E48 File Offset: 0x000B0048
		public static void TickAllPeerPerks(int tickCount)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
				{
					MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
					if (component != null && component.Team != null && component.Culture != null && component.Team.Side != BattleSideEnum.None)
					{
						MPPerkObject.MPPerkHandler perkHandler = MPPerkObject.GetPerkHandler(component);
						if (perkHandler != null)
						{
							perkHandler.OnTick(tickCount);
						}
					}
				}
			}
		}

		// Token: 0x06002DCC RID: 11724 RVA: 0x000B1ED4 File Offset: 0x000B00D4
		[CommandLineFunctionality.CommandLineArgumentFunction("raise_event", "mp_perks")]
		public static string RaiseEventForAllPeersCommand(List<string> strings)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				MPPerkCondition.PerkEventFlags perkEventFlags = MPPerkCondition.PerkEventFlags.None;
				using (List<string>.Enumerator enumerator = strings.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MPPerkCondition.PerkEventFlags perkEventFlags2;
						if (Enum.TryParse<MPPerkCondition.PerkEventFlags>(enumerator.Current, true, out perkEventFlags2))
						{
							perkEventFlags |= perkEventFlags2;
						}
					}
				}
				MPPerkObject.RaiseEventForAllPeers(perkEventFlags);
				return "Raised event with flags " + perkEventFlags;
			}
			return "Can't run this command on clients";
		}

		// Token: 0x06002DCD RID: 11725 RVA: 0x000B1F50 File Offset: 0x000B0150
		[CommandLineFunctionality.CommandLineArgumentFunction("tick_perks", "mp_perks")]
		public static string TickAllPeerPerksCommand(List<string> strings)
		{
			if (GameNetwork.IsServerOrRecorder)
			{
				int num;
				if (strings.Count == 0 || !int.TryParse(strings[0], out num))
				{
					num = 1;
				}
				MPPerkObject.TickAllPeerPerks(num);
				return "Peer perks on tick with tick count " + num;
			}
			return "Can't run this command on clients";
		}

		// Token: 0x04001206 RID: 4614
		private readonly MissionPeer _peer;

		// Token: 0x04001207 RID: 4615
		private readonly MPConditionalEffect.ConditionalEffectContainer _conditionalEffects;

		// Token: 0x04001208 RID: 4616
		private readonly MPPerkCondition.PerkEventFlags _perkEventFlags;

		// Token: 0x04001209 RID: 4617
		private readonly string _name;

		// Token: 0x0400120A RID: 4618
		private readonly string _description;

		// Token: 0x0400120B RID: 4619
		private readonly List<MPPerkEffectBase> _effects;

		// Token: 0x02000608 RID: 1544
		private class MPOnSpawnPerkHandlerInstance : MPPerkObject.MPOnSpawnPerkHandler
		{
			// Token: 0x06004031 RID: 16433 RVA: 0x000FAFA5 File Offset: 0x000F91A5
			public MPOnSpawnPerkHandlerInstance(IEnumerable<IReadOnlyPerkObject> perks)
				: base(perks)
			{
			}

			// Token: 0x06004032 RID: 16434 RVA: 0x000FAFAE File Offset: 0x000F91AE
			public MPOnSpawnPerkHandlerInstance(MissionPeer peer)
				: base(peer)
			{
			}
		}

		// Token: 0x02000609 RID: 1545
		private class MPPerkHandlerInstance : MPPerkObject.MPPerkHandler
		{
			// Token: 0x06004033 RID: 16435 RVA: 0x000FAFB7 File Offset: 0x000F91B7
			public MPPerkHandlerInstance(Agent agent)
				: base(agent)
			{
			}

			// Token: 0x06004034 RID: 16436 RVA: 0x000FAFC0 File Offset: 0x000F91C0
			public MPPerkHandlerInstance(MissionPeer peer)
				: base(peer)
			{
			}
		}

		// Token: 0x0200060A RID: 1546
		private class MPCombatPerkHandlerInstance : MPPerkObject.MPCombatPerkHandler
		{
			// Token: 0x06004035 RID: 16437 RVA: 0x000FAFC9 File Offset: 0x000F91C9
			public MPCombatPerkHandlerInstance(Agent attacker, Agent defender)
				: base(attacker, defender)
			{
			}
		}

		// Token: 0x0200060B RID: 1547
		public class MPOnSpawnPerkHandler
		{
			// Token: 0x17000ACB RID: 2763
			// (get) Token: 0x06004036 RID: 16438 RVA: 0x000FAFD4 File Offset: 0x000F91D4
			public bool IsWarmup
			{
				get
				{
					Mission mission = Mission.Current;
					bool? flag;
					if (mission == null)
					{
						flag = null;
					}
					else
					{
						MissionMultiplayerGameModeBase missionBehavior = mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
						if (missionBehavior == null)
						{
							flag = null;
						}
						else
						{
							MultiplayerWarmupComponent warmupComponent = missionBehavior.WarmupComponent;
							flag = ((warmupComponent != null) ? new bool?(warmupComponent.IsInWarmup) : null);
						}
					}
					return flag ?? false;
				}
			}

			// Token: 0x06004037 RID: 16439 RVA: 0x000FB03A File Offset: 0x000F923A
			protected MPOnSpawnPerkHandler(IEnumerable<IReadOnlyPerkObject> perks)
			{
				this._perks = perks;
			}

			// Token: 0x06004038 RID: 16440 RVA: 0x000FB049 File Offset: 0x000F9249
			protected MPOnSpawnPerkHandler(MissionPeer peer)
			{
				this._perks = peer.SelectedPerks;
			}

			// Token: 0x06004039 RID: 16441 RVA: 0x000FB060 File Offset: 0x000F9260
			public float GetExtraTroopCount()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					if (readOnlyPerkObject != null)
					{
						num += (float)readOnlyPerkObject.GetExtraTroopCount(isWarmup);
					}
				}
				return num;
			}

			// Token: 0x0600403A RID: 16442 RVA: 0x000FB0C4 File Offset: 0x000F92C4
			public IEnumerable<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isPlayer)
			{
				List<ValueTuple<EquipmentIndex, EquipmentElement>> list = null;
				bool isWarmup = this.IsWarmup;
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					if (readOnlyPerkObject != null)
					{
						list = readOnlyPerkObject.GetAlternativeEquipments(isWarmup, isPlayer, list, false);
					}
				}
				return list;
			}

			// Token: 0x0600403B RID: 16443 RVA: 0x000FB124 File Offset: 0x000F9324
			public float GetDrivenPropertyBonusOnSpawn(bool isPlayer, DrivenProperty drivenProperty, float baseValue)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					if (readOnlyPerkObject != null)
					{
						num += readOnlyPerkObject.GetDrivenPropertyBonusOnSpawn(isWarmup, isPlayer, drivenProperty, baseValue);
					}
				}
				return num;
			}

			// Token: 0x0600403C RID: 16444 RVA: 0x000FB188 File Offset: 0x000F9388
			public float GetHitpoints(bool isPlayer)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (IReadOnlyPerkObject readOnlyPerkObject in this._perks)
				{
					num += readOnlyPerkObject.GetHitpoints(isWarmup, isPlayer);
				}
				return num;
			}

			// Token: 0x040020B3 RID: 8371
			private IEnumerable<IReadOnlyPerkObject> _perks;
		}

		// Token: 0x0200060C RID: 1548
		public class MPPerkHandler
		{
			// Token: 0x17000ACC RID: 2764
			// (get) Token: 0x0600403D RID: 16445 RVA: 0x000FB1E8 File Offset: 0x000F93E8
			public bool IsWarmup
			{
				get
				{
					Mission mission = Mission.Current;
					bool? flag;
					if (mission == null)
					{
						flag = null;
					}
					else
					{
						MissionMultiplayerGameModeBase missionBehavior = mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
						if (missionBehavior == null)
						{
							flag = null;
						}
						else
						{
							MultiplayerWarmupComponent warmupComponent = missionBehavior.WarmupComponent;
							flag = ((warmupComponent != null) ? new bool?(warmupComponent.IsInWarmup) : null);
						}
					}
					return flag ?? false;
				}
			}

			// Token: 0x0600403E RID: 16446 RVA: 0x000FB250 File Offset: 0x000F9450
			protected MPPerkHandler(Agent agent)
			{
				this._agent = agent;
				Agent agent2 = this._agent;
				object obj;
				if (agent2 == null)
				{
					obj = null;
				}
				else
				{
					MissionPeer missionPeer = agent2.MissionPeer;
					obj = ((missionPeer != null) ? missionPeer.SelectedPerks : null);
				}
				object obj2;
				if ((obj2 = obj) == null)
				{
					Agent agent3 = this._agent;
					if (agent3 == null)
					{
						obj2 = null;
					}
					else
					{
						MissionPeer owningAgentMissionPeer = agent3.OwningAgentMissionPeer;
						obj2 = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.SelectedPerks : null);
					}
				}
				this._perks = obj2 ?? new MBList<MPPerkObject>();
			}

			// Token: 0x0600403F RID: 16447 RVA: 0x000FB2B9 File Offset: 0x000F94B9
			protected MPPerkHandler(MissionPeer peer)
			{
				this._agent = ((peer != null) ? peer.ControlledAgent : null);
				this._perks = ((peer != null) ? peer.SelectedPerks : null) ?? new MBList<MPPerkObject>();
			}

			// Token: 0x06004040 RID: 16448 RVA: 0x000FB2F0 File Offset: 0x000F94F0
			public void OnEvent(MPPerkCondition.PerkEventFlags flags)
			{
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					mpperkObject.OnEvent(isWarmup, flags);
				}
			}

			// Token: 0x06004041 RID: 16449 RVA: 0x000FB34C File Offset: 0x000F954C
			public void OnEvent(Agent agent, MPPerkCondition.PerkEventFlags flags)
			{
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					mpperkObject.OnEvent(isWarmup, agent, flags);
				}
			}

			// Token: 0x06004042 RID: 16450 RVA: 0x000FB3A8 File Offset: 0x000F95A8
			public void OnTick(int tickCount)
			{
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					mpperkObject.OnTick(isWarmup, tickCount);
				}
			}

			// Token: 0x06004043 RID: 16451 RVA: 0x000FB404 File Offset: 0x000F9604
			public float GetDrivenPropertyBonus(DrivenProperty drivenProperty, float baseValue)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetDrivenPropertyBonus(isWarmup, this._agent, drivenProperty, baseValue);
				}
				return num;
			}

			// Token: 0x06004044 RID: 16452 RVA: 0x000FB470 File Offset: 0x000F9670
			public float GetRangedAccuracy()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetRangedAccuracy(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06004045 RID: 16453 RVA: 0x000FB4DC File Offset: 0x000F96DC
			public float GetThrowingWeaponSpeed(WeaponComponentData attackerWeapon)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetThrowingWeaponSpeed(isWarmup, this._agent, attackerWeapon);
				}
				return num;
			}

			// Token: 0x06004046 RID: 16454 RVA: 0x000FB548 File Offset: 0x000F9748
			public float GetDamageInterruptionThreshold()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetDamageInterruptionThreshold(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06004047 RID: 16455 RVA: 0x000FB5B4 File Offset: 0x000F97B4
			public float GetMountManeuver()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetMountManeuver(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06004048 RID: 16456 RVA: 0x000FB620 File Offset: 0x000F9820
			public float GetMountSpeed()
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetMountSpeed(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x06004049 RID: 16457 RVA: 0x000FB68C File Offset: 0x000F988C
			public int GetGoldOnKill(float attackerValue, float victimValue)
			{
				int num = 0;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetGoldOnKill(isWarmup, this._agent, attackerValue, victimValue);
				}
				return num;
			}

			// Token: 0x0600404A RID: 16458 RVA: 0x000FB6F4 File Offset: 0x000F98F4
			public int GetGoldOnAssist()
			{
				int num = 0;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetGoldOnAssist(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x0600404B RID: 16459 RVA: 0x000FB75C File Offset: 0x000F995C
			public int GetRewardedGoldOnAssist()
			{
				int num = 0;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetRewardedGoldOnAssist(isWarmup, this._agent);
				}
				return num;
			}

			// Token: 0x0600404C RID: 16460 RVA: 0x000FB7C4 File Offset: 0x000F99C4
			public bool GetIsTeamRewardedOnDeath()
			{
				bool isWarmup = this.IsWarmup;
				using (List<MPPerkObject>.Enumerator enumerator = this._perks.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.GetIsTeamRewardedOnDeath(isWarmup, this._agent))
						{
							return true;
						}
					}
				}
				return false;
			}

			// Token: 0x0600404D RID: 16461 RVA: 0x000FB82C File Offset: 0x000F9A2C
			public IEnumerable<ValueTuple<MissionPeer, int>> GetTeamGoldRewardsOnDeath()
			{
				if (this.GetIsTeamRewardedOnDeath())
				{
					Agent agent = this._agent;
					MissionPeer missionPeer;
					if ((missionPeer = ((agent != null) ? agent.MissionPeer : null)) == null)
					{
						Agent agent2 = this._agent;
						missionPeer = ((agent2 != null) ? agent2.OwningAgentMissionPeer : null);
					}
					MissionPeer missionPeer2 = missionPeer;
					List<ValueTuple<MissionPeer, int>> list = new List<ValueTuple<MissionPeer, int>>();
					foreach (NetworkCommunicator networkCommunicator in GameNetwork.NetworkPeers)
					{
						MissionPeer component = networkCommunicator.GetComponent<MissionPeer>();
						if (component != missionPeer2 && component.Team == missionPeer2.Team)
						{
							list.Add(new ValueTuple<MissionPeer, int>(component, 0));
						}
					}
					bool isWarmup = this.IsWarmup;
					foreach (MPPerkObject mpperkObject in this._perks)
					{
						mpperkObject.CalculateRewardedGoldOnDeath(isWarmup, this._agent, list);
					}
					return list;
				}
				return null;
			}

			// Token: 0x0600404E RID: 16462 RVA: 0x000FB92C File Offset: 0x000F9B2C
			public float GetEncumbrance(bool isOnBody)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._perks)
				{
					num += mpperkObject.GetEncumbrance(isWarmup, this._agent, isOnBody);
				}
				return num;
			}

			// Token: 0x040020B4 RID: 8372
			private readonly Agent _agent;

			// Token: 0x040020B5 RID: 8373
			private readonly MBReadOnlyList<MPPerkObject> _perks;
		}

		// Token: 0x0200060D RID: 1549
		public class MPCombatPerkHandler
		{
			// Token: 0x17000ACD RID: 2765
			// (get) Token: 0x0600404F RID: 16463 RVA: 0x000FB998 File Offset: 0x000F9B98
			public bool IsWarmup
			{
				get
				{
					Mission mission = Mission.Current;
					bool? flag;
					if (mission == null)
					{
						flag = null;
					}
					else
					{
						MissionMultiplayerGameModeBase missionBehavior = mission.GetMissionBehavior<MissionMultiplayerGameModeBase>();
						if (missionBehavior == null)
						{
							flag = null;
						}
						else
						{
							MultiplayerWarmupComponent warmupComponent = missionBehavior.WarmupComponent;
							flag = ((warmupComponent != null) ? new bool?(warmupComponent.IsInWarmup) : null);
						}
					}
					return flag ?? false;
				}
			}

			// Token: 0x06004050 RID: 16464 RVA: 0x000FBA00 File Offset: 0x000F9C00
			protected MPCombatPerkHandler(Agent attacker, Agent defender)
			{
				this._attacker = attacker;
				this._defender = defender;
				attacker = ((attacker != null && attacker.IsMount) ? attacker.RiderAgent : attacker);
				defender = ((defender != null && defender.IsMount) ? defender.RiderAgent : defender);
				MBList<MPPerkObject> mblist;
				if (attacker == null)
				{
					mblist = null;
				}
				else
				{
					MissionPeer missionPeer = attacker.MissionPeer;
					mblist = ((missionPeer != null) ? missionPeer.SelectedPerks : null);
				}
				MBList<MPPerkObject> mblist2;
				if ((mblist2 = mblist) == null)
				{
					MBList<MPPerkObject> mblist3;
					if (attacker == null)
					{
						mblist3 = null;
					}
					else
					{
						MissionPeer owningAgentMissionPeer = attacker.OwningAgentMissionPeer;
						mblist3 = ((owningAgentMissionPeer != null) ? owningAgentMissionPeer.SelectedPerks : null);
					}
					mblist2 = mblist3 ?? new MBList<MPPerkObject>();
				}
				this._attackerPerks = mblist2;
				MBList<MPPerkObject> mblist4;
				if (defender == null)
				{
					mblist4 = null;
				}
				else
				{
					MissionPeer missionPeer2 = defender.MissionPeer;
					mblist4 = ((missionPeer2 != null) ? missionPeer2.SelectedPerks : null);
				}
				MBList<MPPerkObject> mblist5;
				if ((mblist5 = mblist4) == null)
				{
					MBList<MPPerkObject> mblist6;
					if (defender == null)
					{
						mblist6 = null;
					}
					else
					{
						MissionPeer owningAgentMissionPeer2 = defender.OwningAgentMissionPeer;
						mblist6 = ((owningAgentMissionPeer2 != null) ? owningAgentMissionPeer2.SelectedPerks : null);
					}
					mblist5 = mblist6 ?? new MBList<MPPerkObject>();
				}
				this._defenderPerks = mblist5;
			}

			// Token: 0x06004051 RID: 16465 RVA: 0x000FBAD4 File Offset: 0x000F9CD4
			public float GetDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
			{
				float num = 0f;
				if (this._attackerPerks.Count > 0 && this._defender != null)
				{
					bool isWarmup = this.IsWarmup;
					if (this._defender.IsMount)
					{
						foreach (MPPerkObject mpperkObject in this._attackerPerks)
						{
							num += mpperkObject.GetMountDamage(isWarmup, this._attacker, attackerWeapon, damageType, isAlternativeAttack);
						}
					}
					foreach (MPPerkObject mpperkObject2 in this._attackerPerks)
					{
						num += mpperkObject2.GetDamage(isWarmup, this._attacker, attackerWeapon, damageType, isAlternativeAttack);
					}
				}
				return num;
			}

			// Token: 0x06004052 RID: 16466 RVA: 0x000FBBBC File Offset: 0x000F9DBC
			public float GetDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
			{
				float num = 0f;
				if (this._defenderPerks.Count > 0)
				{
					bool isWarmup = this.IsWarmup;
					if (this._defender.IsMount)
					{
						using (List<MPPerkObject>.Enumerator enumerator = this._defenderPerks.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								MPPerkObject mpperkObject = enumerator.Current;
								num += mpperkObject.GetMountDamageTaken(isWarmup, this._defender, attackerWeapon, damageType);
							}
							return num;
						}
					}
					foreach (MPPerkObject mpperkObject2 in this._defenderPerks)
					{
						num += mpperkObject2.GetDamageTaken(isWarmup, this._defender, attackerWeapon, damageType);
					}
				}
				return num;
			}

			// Token: 0x06004053 RID: 16467 RVA: 0x000FBC98 File Offset: 0x000F9E98
			public float GetSpeedBonusEffectiveness(WeaponComponentData attackerWeapon, DamageTypes damageType)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._attackerPerks)
				{
					num += mpperkObject.GetSpeedBonusEffectiveness(isWarmup, this._attacker, attackerWeapon, damageType);
				}
				return num;
			}

			// Token: 0x06004054 RID: 16468 RVA: 0x000FBD04 File Offset: 0x000F9F04
			public float GetShieldDamage(bool isCorrectSideBlock)
			{
				float num = 0f;
				if (this._defender != null)
				{
					bool isWarmup = this.IsWarmup;
					foreach (MPPerkObject mpperkObject in this._attackerPerks)
					{
						num += mpperkObject.GetShieldDamage(isWarmup, this._attacker, this._defender, isCorrectSideBlock);
					}
				}
				return num;
			}

			// Token: 0x06004055 RID: 16469 RVA: 0x000FBD80 File Offset: 0x000F9F80
			public float GetShieldDamageTaken(bool isCorrectSideBlock)
			{
				float num = 0f;
				bool isWarmup = this.IsWarmup;
				foreach (MPPerkObject mpperkObject in this._defenderPerks)
				{
					num += mpperkObject.GetShieldDamageTaken(isWarmup, this._attacker, this._defender, isCorrectSideBlock);
				}
				return num;
			}

			// Token: 0x06004056 RID: 16470 RVA: 0x000FBDF4 File Offset: 0x000F9FF4
			public float GetRangedHeadShotDamage()
			{
				float num = 0f;
				if (this._attacker != null)
				{
					bool isWarmup = this.IsWarmup;
					foreach (MPPerkObject mpperkObject in this._attackerPerks)
					{
						num += mpperkObject.GetRangedHeadShotDamage(isWarmup, this._attacker);
					}
				}
				return num;
			}

			// Token: 0x040020B6 RID: 8374
			private readonly Agent _attacker;

			// Token: 0x040020B7 RID: 8375
			private readonly Agent _defender;

			// Token: 0x040020B8 RID: 8376
			private readonly MBReadOnlyList<MPPerkObject> _attackerPerks;

			// Token: 0x040020B9 RID: 8377
			private readonly MBReadOnlyList<MPPerkObject> _defenderPerks;
		}
	}
}
