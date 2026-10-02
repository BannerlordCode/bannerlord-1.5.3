using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002C RID: 44
	public class DamageEffect : MPCombatPerkEffect
	{
		// Token: 0x060001E9 RID: 489 RVA: 0x00008D3B File Offset: 0x00006F3B
		protected DamageEffect()
		{
		}

		// Token: 0x060001EA RID: 490 RVA: 0x00008D44 File Offset: 0x00006F44
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\DamageEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x00008DA9 File Offset: 0x00006FA9
		public override float GetDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			if (!base.IsSatisfied(attackerWeapon, damageType))
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x0400007C RID: 124
		protected static string StringType = "DamageDealt";

		// Token: 0x0400007D RID: 125
		private float _value;
	}
}
