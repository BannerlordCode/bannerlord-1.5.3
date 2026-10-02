using System;
using System.Collections.Generic;
using System.Xml;
using Newtonsoft.Json;
using TaleWorlds.Library;

namespace TaleWorlds.Core
{
	// Token: 0x02000022 RID: 34
	[JsonConverter(typeof(BodyPropertiesJsonConverter))]
	[Serializable]
	public struct BodyProperties
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600018E RID: 398 RVA: 0x00006A89 File Offset: 0x00004C89
		public StaticBodyProperties StaticProperties
		{
			get
			{
				return this._staticBodyProperties;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600018F RID: 399 RVA: 0x00006A91 File Offset: 0x00004C91
		public DynamicBodyProperties DynamicProperties
		{
			get
			{
				return this._dynamicBodyProperties;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000190 RID: 400 RVA: 0x00006A99 File Offset: 0x00004C99
		public float Age
		{
			get
			{
				return this._dynamicBodyProperties.Age;
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x06000191 RID: 401 RVA: 0x00006AA6 File Offset: 0x00004CA6
		public float Weight
		{
			get
			{
				return this._dynamicBodyProperties.Weight;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x06000192 RID: 402 RVA: 0x00006AB3 File Offset: 0x00004CB3
		public float Build
		{
			get
			{
				return this._dynamicBodyProperties.Build;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x06000193 RID: 403 RVA: 0x00006AC0 File Offset: 0x00004CC0
		public ulong KeyPart1
		{
			get
			{
				return this._staticBodyProperties.KeyPart1;
			}
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000194 RID: 404 RVA: 0x00006ADC File Offset: 0x00004CDC
		public ulong KeyPart2
		{
			get
			{
				return this._staticBodyProperties.KeyPart2;
			}
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x06000195 RID: 405 RVA: 0x00006AF8 File Offset: 0x00004CF8
		public ulong KeyPart3
		{
			get
			{
				return this._staticBodyProperties.KeyPart3;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x06000196 RID: 406 RVA: 0x00006B14 File Offset: 0x00004D14
		public ulong KeyPart4
		{
			get
			{
				return this._staticBodyProperties.KeyPart4;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000197 RID: 407 RVA: 0x00006B30 File Offset: 0x00004D30
		public ulong KeyPart5
		{
			get
			{
				return this._staticBodyProperties.KeyPart5;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x06000198 RID: 408 RVA: 0x00006B4C File Offset: 0x00004D4C
		public ulong KeyPart6
		{
			get
			{
				return this._staticBodyProperties.KeyPart6;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x06000199 RID: 409 RVA: 0x00006B68 File Offset: 0x00004D68
		public ulong KeyPart7
		{
			get
			{
				return this._staticBodyProperties.KeyPart7;
			}
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x0600019A RID: 410 RVA: 0x00006B84 File Offset: 0x00004D84
		public ulong KeyPart8
		{
			get
			{
				return this._staticBodyProperties.KeyPart8;
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00006B9F File Offset: 0x00004D9F
		public BodyProperties(DynamicBodyProperties dynamicBodyProperties, StaticBodyProperties staticBodyProperties)
		{
			this._dynamicBodyProperties = dynamicBodyProperties;
			this._staticBodyProperties = staticBodyProperties;
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00006BB0 File Offset: 0x00004DB0
		public static bool FromXmlNode(XmlNode node, out BodyProperties bodyProperties)
		{
			float num = 30f;
			float num2 = 0.5f;
			float num3 = 0.5f;
			if (node.Attributes["age"] != null)
			{
				float.TryParse(node.Attributes["age"].Value, out num);
			}
			if (node.Attributes["weight"] != null)
			{
				float.TryParse(node.Attributes["weight"].Value, out num2);
			}
			if (node.Attributes["build"] != null)
			{
				float.TryParse(node.Attributes["build"].Value, out num3);
			}
			DynamicBodyProperties dynamicBodyProperties = new DynamicBodyProperties(num, num2, num3);
			StaticBodyProperties staticBodyProperties;
			if (StaticBodyProperties.FromXmlNode(node, out staticBodyProperties))
			{
				bodyProperties = new BodyProperties(dynamicBodyProperties, staticBodyProperties);
				return true;
			}
			bodyProperties = default(BodyProperties);
			return false;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x00006C8C File Offset: 0x00004E8C
		public static bool FromString(string keyValue, out BodyProperties bodyProperties)
		{
			if (keyValue.StartsWith("<BodyProperties ", StringComparison.InvariantCultureIgnoreCase) || keyValue.StartsWith("<BodyPropertiesMax ", StringComparison.InvariantCultureIgnoreCase))
			{
				XmlDocument xmlDocument = new XmlDocument();
				try
				{
					xmlDocument.LoadXml(keyValue);
				}
				catch (XmlException)
				{
					bodyProperties = default(BodyProperties);
					return false;
				}
				if (xmlDocument.FirstChild.Name.Equals("BodyProperties", StringComparison.InvariantCultureIgnoreCase) || xmlDocument.FirstChild.Name.Equals("BodyPropertiesMax", StringComparison.InvariantCultureIgnoreCase))
				{
					BodyProperties.FromXmlNode(xmlDocument.FirstChild, out bodyProperties);
					float num = 20f;
					float num2 = 0f;
					float num3 = 0f;
					if (xmlDocument.FirstChild.Attributes["age"] != null)
					{
						float.TryParse(xmlDocument.FirstChild.Attributes["age"].Value, out num);
					}
					if (xmlDocument.FirstChild.Attributes["weight"] != null)
					{
						float.TryParse(xmlDocument.FirstChild.Attributes["weight"].Value, out num2);
					}
					if (xmlDocument.FirstChild.Attributes["build"] != null)
					{
						float.TryParse(xmlDocument.FirstChild.Attributes["build"].Value, out num3);
					}
					bodyProperties = new BodyProperties(new DynamicBodyProperties(num, num2, num3), bodyProperties.StaticProperties);
					return true;
				}
				bodyProperties = default(BodyProperties);
				return false;
			}
			Debug.FailedAssert("unknown body properties format:\n" + keyValue, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.Core\\BodyProperties.cs", "FromString", 148);
			bodyProperties = default(BodyProperties);
			return false;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x00006E34 File Offset: 0x00005034
		public static BodyProperties GetRandomBodyProperties(int race, bool isFemale, BodyProperties bodyPropertiesMin, BodyProperties bodyPropertiesMax, int hairCoverType, int seed, string hairTags, string beardTags, string tattooTags, float variationAmount = 0f)
		{
			variationAmount = MathF.Max(variationAmount, 0f);
			return FaceGen.GetRandomBodyProperties(race, isFemale, bodyPropertiesMin, bodyPropertiesMax, hairCoverType, seed, hairTags, beardTags, tattooTags, variationAmount);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x00006E64 File Offset: 0x00005064
		public static bool operator ==(BodyProperties a, BodyProperties b)
		{
			return a == b || (a != null && b != null && a._staticBodyProperties == b._staticBodyProperties && a._dynamicBodyProperties == b._dynamicBodyProperties);
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x00006EB9 File Offset: 0x000050B9
		public static bool operator !=(BodyProperties a, BodyProperties b)
		{
			return !(a == b);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x00006EC8 File Offset: 0x000050C8
		public override string ToString()
		{
			MBStringBuilder mbstringBuilder = default(MBStringBuilder);
			mbstringBuilder.Initialize(150, "ToString");
			mbstringBuilder.Append<string>("<BodyProperties version=\"4\" ");
			mbstringBuilder.Append<string>(this._dynamicBodyProperties.ToString() + " ");
			mbstringBuilder.Append<string>(this._staticBodyProperties.ToString());
			mbstringBuilder.Append<string>(" />");
			return mbstringBuilder.ToStringAndRelease();
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x00006F54 File Offset: 0x00005154
		public override bool Equals(object obj)
		{
			if (!(obj is BodyProperties))
			{
				return false;
			}
			BodyProperties bodyProperties = (BodyProperties)obj;
			return EqualityComparer<DynamicBodyProperties>.Default.Equals(this._dynamicBodyProperties, bodyProperties._dynamicBodyProperties) && EqualityComparer<StaticBodyProperties>.Default.Equals(this._staticBodyProperties, bodyProperties._staticBodyProperties);
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x00006FA2 File Offset: 0x000051A2
		public override int GetHashCode()
		{
			return (2041866711 * -1521134295 + EqualityComparer<DynamicBodyProperties>.Default.GetHashCode(this._dynamicBodyProperties)) * -1521134295 + EqualityComparer<StaticBodyProperties>.Default.GetHashCode(this._staticBodyProperties);
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x00006FD8 File Offset: 0x000051D8
		public BodyProperties ClampForMultiplayer()
		{
			float num = MathF.Clamp(this.DynamicProperties.Age, 22f, 128f);
			DynamicBodyProperties dynamicBodyProperties = new DynamicBodyProperties(num, 0.5f, 0.5f);
			StaticBodyProperties staticProperties = this.StaticProperties;
			StaticBodyProperties staticBodyProperties = this.ClampHeightMultiplierFaceKey(in staticProperties);
			return new BodyProperties(dynamicBodyProperties, staticBodyProperties);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x00007028 File Offset: 0x00005228
		private StaticBodyProperties ClampHeightMultiplierFaceKey(in StaticBodyProperties staticBodyProperties)
		{
			StaticBodyProperties staticBodyProperties2 = staticBodyProperties;
			ulong keyPart = staticBodyProperties2.KeyPart8;
			float num = (float)BodyProperties.GetBitsValueFromKey(in keyPart, 19, 6) / 63f;
			if (num < 0.25f || num > 0.75f)
			{
				num = 0.5f;
				int num2 = (int)(num * 63f);
				ulong num3 = BodyProperties.SetBits(in keyPart, 19, 6, num2);
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart2 = staticBodyProperties2.KeyPart1;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart3 = staticBodyProperties2.KeyPart2;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart4 = staticBodyProperties2.KeyPart3;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart5 = staticBodyProperties2.KeyPart4;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart6 = staticBodyProperties2.KeyPart5;
				staticBodyProperties2 = staticBodyProperties;
				ulong keyPart7 = staticBodyProperties2.KeyPart6;
				ulong num4 = num3;
				staticBodyProperties2 = staticBodyProperties;
				return new StaticBodyProperties(keyPart2, keyPart3, keyPart4, keyPart5, keyPart6, keyPart7, num4, staticBodyProperties2.KeyPart8);
			}
			return staticBodyProperties;
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x000070F8 File Offset: 0x000052F8
		private static ulong SetBits(in ulong ipart7, int startBit, int numBits, int inewValue)
		{
			ulong num = ipart7;
			ulong num2 = MathF.PowTwo64(numBits) - 1UL << startBit;
			return (num & ~num2) | (ulong)((ulong)((long)inewValue) << startBit);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x00007124 File Offset: 0x00005324
		private static int GetBitsValueFromKey(in ulong part, int startBit, int numBits)
		{
			ulong num = part >> startBit;
			ulong num2 = MathF.PowTwo64(numBits) - 1UL;
			return (int)(num & num2);
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060001A8 RID: 424 RVA: 0x00007148 File Offset: 0x00005348
		public static BodyProperties Default
		{
			get
			{
				return new BodyProperties(new DynamicBodyProperties(20f, 0f, 0f), default(StaticBodyProperties));
			}
		}

		// Token: 0x04000178 RID: 376
		private readonly DynamicBodyProperties _dynamicBodyProperties;

		// Token: 0x04000179 RID: 377
		private readonly StaticBodyProperties _staticBodyProperties;

		// Token: 0x0400017A RID: 378
		private const float DefaultAge = 30f;

		// Token: 0x0400017B RID: 379
		private const float DefaultWeight = 0.5f;

		// Token: 0x0400017C RID: 380
		private const float DefaultBuild = 0.5f;
	}
}
