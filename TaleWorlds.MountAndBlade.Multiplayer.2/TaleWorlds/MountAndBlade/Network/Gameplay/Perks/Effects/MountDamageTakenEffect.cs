using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x02000038 RID: 56
	public class MountDamageTakenEffect : MPCombatPerkEffect
	{
		// Token: 0x0600021D RID: 541 RVA: 0x000099B9 File Offset: 0x00007BB9
		protected MountDamageTakenEffect()
		{
		}

		// Token: 0x0600021E RID: 542 RVA: 0x000099C4 File Offset: 0x00007BC4
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\MountDamageTakenEffect.cs", "Deserialize", 22);
			}
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00009A29 File Offset: 0x00007C29
		public override float GetMountDamageTaken(WeaponComponentData attackerWeapon, DamageTypes damageType)
		{
			if (!base.IsSatisfied(attackerWeapon, damageType))
			{
				return 0f;
			}
			return this._value;
		}

		// Token: 0x0400009C RID: 156
		protected static string StringType = "MountDamageTaken";

		// Token: 0x0400009D RID: 157
		private float _value;
	}
}
