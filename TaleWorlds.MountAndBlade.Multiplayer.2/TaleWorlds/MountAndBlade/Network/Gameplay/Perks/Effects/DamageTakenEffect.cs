using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002E RID: 46
	public class DamageTakenEffect : MPCombatPerkEffect
	{
		// Token: 0x060001F1 RID: 497 RVA: 0x00008E90 File Offset: 0x00007090
		protected DamageTakenEffect()
		{
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x00008E98 File Offset: 0x00007098
		protected override void Deserialize(XmlNode node)
		{
			base.Deserialize(node);
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
					XmlAttribute xmlAttribute = attributes["value"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			if (text2 == null || !float.TryParse(text2, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DamageTakenEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x00008EFD File Offset: 0x000070FD
		public override float GetDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			if (!base.IsSatisfied(attackerWeapon, damageType))
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x04000080 RID: 128
		protected static string StringType = "DamageTaken";

		// Token: 0x04000081 RID: 129
		private float _value;
	}
}
