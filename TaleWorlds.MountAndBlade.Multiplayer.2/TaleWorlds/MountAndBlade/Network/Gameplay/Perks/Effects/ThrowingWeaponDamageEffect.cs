using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000044 RID: 68
	public class ThrowingWeaponDamageEffect : MPPerkEffect
	{
		// Token: 0x06000253 RID: 595 RVA: 0x0000A99F File Offset: 0x00008B9F
		protected ThrowingWeaponDamageEffect()
		{
		}

		// Token: 0x06000254 RID: 596 RVA: 0x0000A9A8 File Offset: 0x00008BA8
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
					XmlAttribute xmlAttribute2 = attributes2["value"];
					text3 = ((xmlAttribute2 != null) ? xmlAttribute2.Value : null);
				}
			}
			string text4 = text3;
			if (text4 == null || !float.TryParse(text4, out this._value))
			{
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\ThrowingWeaponDamageEffect.cs", "Deserialize", 24);
			}
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0000AA4C File Offset: 0x00008C4C
		public override float GetDamage(WeaponComponentData attackerWeapon, DamageTypes damageType, bool isAlternativeAttack)
		{
			if (isAlternativeAttack || attackerWeapon == null || WeaponComponentData.GetItemTypeFromWeaponClass(attackerWeapon.WeaponClass) != ItemObject.ItemTypeEnum.Thrown)
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x040000B9 RID: 185
		protected static string StringType = "ThrowingWeaponDamage";

		// Token: 0x040000BA RID: 186
		private float _value;
	}
}
