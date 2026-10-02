using System;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;

namespace TaleWorlds.MountAndBlade.Network.Gameplay.Perks.Effects
{
	// Token: 0x0200002B RID: 43
	public class ArmorEffect : MPOnSpawnPerkEffect
	{
		// Token: 0x060001E5 RID: 485 RVA: 0x00008C6E File Offset: 0x00006E6E
		protected ArmorEffect()
		{
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x00008C78 File Offset: 0x00006E78
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
				Debug.FailedAssert("provided 'value' is invalid", "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.MountAndBlade.Multiplayer\\Perks\\Effects\\ArmorEffect.cs", "Deserialize", 21);
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x00008CE0 File Offset: 0x00006EE0
		public override float GetDrivenPropertyBonusOnSpawn(bool isPlayer, DrivenProperty drivenProperty, float baseValue)
		{
			if ((drivenProperty == DrivenProperty.ArmorHead || drivenProperty == DrivenProperty.ArmorTorso || drivenProperty == DrivenProperty.ArmorLegs || drivenProperty == DrivenProperty.ArmorArms) && (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Any || (isPlayer ? (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Player) : (this.EffectTarget == MPOnSpawnPerkEffectBase.Target.Troops))))
			{
				return this._value;
			}
			return 0f;
		}

		// Token: 0x0400007A RID: 122
		protected static string StringType = "ArmorOnSpawn";

		// Token: 0x0400007B RID: 123
		private float _value;
	}
}
