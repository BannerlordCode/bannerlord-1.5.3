using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Xml;
using TaleWorlds.MountAndBlade.Network.Gameplay.Perks;

namespace TaleWorlds.MountAndBlade
{
	// Token: 0x0200031E RID: 798
	public abstract class MPRandomOnSpawnPerkEffect : MPOnSpawnPerkEffectBase
	{
		// Token: 0x06002DD8 RID: 11736 RVA: 0x000B2670 File Offset: 0x000B0870
		static MPRandomOnSpawnPerkEffect()
		{
			foreach (Type type in from t in PerkAssemblyCollection.GetPerkAssemblyTypes()
				where t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(MPRandomOnSpawnPerkEffect))
				select t)
			{
				FieldInfo field = type.GetField("StringType", BindingFlags.Static | BindingFlags.NonPublic);
				string text = (string)((field != null) ? field.GetValue(null) : null);
				MPRandomOnSpawnPerkEffect.Registered.Add(text, type);
			}
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x000B2700 File Offset: 0x000B0900
		public static MPRandomOnSpawnPerkEffect CreateFrom(XmlNode node)
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
					XmlAttribute xmlAttribute = attributes["type"];
					text = ((xmlAttribute != null) ? xmlAttribute.Value : null);
				}
			}
			string text2 = text;
			MPRandomOnSpawnPerkEffect mprandomOnSpawnPerkEffect = (MPRandomOnSpawnPerkEffect)Activator.CreateInstance(MPRandomOnSpawnPerkEffect.Registered[text2], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, null, CultureInfo.InvariantCulture);
			mprandomOnSpawnPerkEffect.Deserialize(node);
			return mprandomOnSpawnPerkEffect;
		}

		// Token: 0x0400121A RID: 4634
		protected static Dictionary<string, Type> Registered = new Dictionary<string, Type>();
	}
}
