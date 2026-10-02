using System;
using System.Collections.Generic;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x02000316 RID: 790
	public abstract class MPOnSpawnPerkEffectBase : MPPerkEffectBase, IOnSpawnPerkEffect
	{
		// Token: 0x06002D59 RID: 11609 RVA: 0x000AF59C File Offset: 0x000AD79C
		protected override void Deserialize(XmlNode node)
		{
			string text;
			if (node == null)
			{
				text = null;
			}
			else
			{
				XmlAttributeCollection attributes = node.Attributes;
				if (attributes == null)
				{
					text = null;
				}
				else
				{
					XmlAttribute xmlAttribute = attributes["is_disabled_in_warmup"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			base.IsDisabledInWarmup = ((text2 != null) ? text2.ToLower() : null) == "true";
			string text3;
			if (node == null)
			{
				text3 = null;
			}
			else
			{
				XmlAttributeCollection attributes2 = node.Attributes;
				if (attributes2 == null)
				{
					text3 = null;
				}
				else
				{
					XmlAttribute xmlAttribute2 = attributes2["target"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			this.EffectTarget = MPOnSpawnPerkEffectBase.Target.Any;
			if (text4 != null && !Enum.TryParse<MPOnSpawnPerkEffectBase.Target>(text4, true, out this.EffectTarget))
			{
				this.EffectTarget = MPOnSpawnPerkEffectBase.Target.Any;
				Debug.FailedAssert("provided 'target' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade\\Network\\Gameplay\\Perks\\MPOnSpawnPerkEffectBase.cs", "Deserialize", 38);
			}
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x000AF64F File Offset: 0x000AD84F
		public virtual float GetTroopCountMultiplier()
		{
			return 0f;
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x000AF656 File Offset: 0x000AD856
		public virtual int GetExtraTroopCount()
		{
			return 0;
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x000AF659 File Offset: 0x000AD859
		public virtual List<ValueTuple<EquipmentIndex, EquipmentElement>> GetAlternativeEquipments(bool isPlayer, List<ValueTuple<EquipmentIndex, EquipmentElement>> alternativeEquipments, bool getAll = false)
		{
			return alternativeEquipments;
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x000AF65C File Offset: 0x000AD85C
		public virtual float GetDrivenPropertyBonusOnSpawn(bool isPlayer, DrivenProperty drivenProperty, float baseValue)
		{
			return 0f;
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x000AF663 File Offset: 0x000AD863
		public virtual float GetHitpoints(bool isPlayer)
		{
			return 0f;
		}

		// Token: 0x04001202 RID: 4610
		protected MPOnSpawnPerkEffectBase.Target EffectTarget;

		// Token: 0x02000604 RID: 1540
		protected enum Target
		{
			// Token: 0x040020A0 RID: 8352
			Player,
			// Token: 0x040020A1 RID: 8353
			Troops,
			// Token: 0x040020A2 RID: 8354
			Any
		}
	}
}
