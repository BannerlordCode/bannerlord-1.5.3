using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000045 RID: 69
	public class ThrowingWeaponSpeedEffect : MPPerkEffect
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000AA7B File Offset: 0x00008C7B
		protected ThrowingWeaponSpeedEffect()
		{
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000AA84 File Offset: 0x00008C84
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\ThrowingWeaponSpeedEffect.cs", "Deserialize", 24);
			}
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0000AB28 File Offset: 0x00008D28
		public override float GetThrowingWeaponSpeed(WeaponComponentData attackerWeapon)
		{
			if (attackerWeapon == null || WeaponComponentData.GetItemTypeFromWeaponClass(attackerWeapon.WeaponClass) != ItemObject.ItemTypeEnum.Thrown)
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x040000BB RID: 187
		protected static string StringType = "ThrowingWeaponSpeed";

		// Token: 0x040000BC RID: 188
		private float _value;
	}
}
