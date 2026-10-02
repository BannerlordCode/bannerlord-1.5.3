using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000037 RID: 55
	public class MountDamageEffect : MPCombatPerkEffect
	{
		// Token: 0x06000219 RID: 537 RVA: 0x00009925 File Offset: 0x00007B25
		protected MountDamageEffect()
		{
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00009930 File Offset: 0x00007B30
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\MountDamageEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00009995 File Offset: 0x00007B95
		public override float GetMountDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			if (!base.IsSatisfied(attackerWeapon, damageType))
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x0400009A RID: 154
		protected static string StringType = "MountDamage";

		// Token: 0x0400009B RID: 155
		private float _value;
	}
}
