using System;
using System.Collections.Generic;
using System.Globalization;

namespace TaleWorlds.Library
{
	// Token: 0x02000076 RID: 118
	public class ParameterContainer
	{
		// Token: 0x06000439 RID: 1081 RVA: 0x0000EF42 File Offset: 0x0000D142
		public ParameterContainer()
		{
			this._parameters = new Dictionary<string, string>();
		}

		// Token: 0x0600043A RID: 1082 RVA: 0x0000EF55 File Offset: 0x0000D155
		public void AddParameter(string key, string value, bool overwriteIfExists)
		{
			if (this._parameters.ContainsKey(key))
			{
				if (overwriteIfExists)
				{
					this._parameters[key] = value;
					return;
				}
			}
			else
			{
				this._parameters.Add(key, value);
			}
		}

		// Token: 0x0600043B RID: 1083 RVA: 0x0000EF84 File Offset: 0x0000D184
		public void AddParameterConcurrent(string key, string value, bool overwriteIfExists)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(this._parameters);
			if (dictionary.ContainsKey(key))
			{
				if (overwriteIfExists)
				{
					dictionary[key] = value;
				}
			}
			else
			{
				dictionary.Add(key, value);
			}
			this._parameters = dictionary;
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
		public void AddParametersConcurrent(IEnumerable<KeyValuePair<string, string>> parameters, bool overwriteIfExists)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>(this._parameters);
			foreach (KeyValuePair<string, string> keyValuePair in parameters)
			{
				if (dictionary.ContainsKey(keyValuePair.Key))
				{
					if (overwriteIfExists)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				else
				{
					dictionary.Add(keyValuePair.Key, keyValuePair.Value);
				}
			}
			this._parameters = dictionary;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x0000F054 File Offset: 0x0000D254
		public void ClearParameters()
		{
			this._parameters = new Dictionary<string, string>();
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x0000F061 File Offset: 0x0000D261
		public bool TryGetParameter(string key, out string outValue)
		{
			return this._parameters.TryGetValue(key, out outValue);
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x0000F070 File Offset: 0x0000D270
		public bool TryGetParameterAsBool(string key, out bool outValue)
		{
			outValue = false;
			string text;
			if (this.TryGetParameter(key, out text))
			{
				outValue = text == "true" || text == "True";
				return true;
			}
			return false;
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x0000F0AC File Offset: 0x0000D2AC
		public bool TryGetParameterAsInt(string key, out int outValue)
		{
			outValue = 0;
			string text;
			if (this.TryGetParameter(key, out text))
			{
				outValue = Convert.ToInt32(text);
				return true;
			}
			return false;
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x0000F0D4 File Offset: 0x0000D2D4
		public bool TryGetParameterAsUInt16(string key, out ushort outValue)
		{
			outValue = 0;
			string text;
			if (this.TryGetParameter(key, out text))
			{
				outValue = Convert.ToUInt16(text);
				return true;
			}
			return false;
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x0000F0FC File Offset: 0x0000D2FC
		public bool TryGetParameterAsFloat(string key, out float outValue)
		{
			outValue = 0f;
			string text;
			if (this.TryGetParameter(key, out text))
			{
				outValue = Convert.ToSingle(text, CultureInfo.InvariantCulture);
				return true;
			}
			return false;
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x0000F12C File Offset: 0x0000D32C
		public bool TryGetParameterAsByte(string key, out byte outValue)
		{
			outValue = 0;
			string text;
			if (this.TryGetParameter(key, out text))
			{
				outValue = Convert.ToByte(text);
				return true;
			}
			return false;
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x0000F154 File Offset: 0x0000D354
		public bool TryGetParameterAsSByte(string key, out sbyte outValue)
		{
			outValue = 0;
			string text;
			if (this.TryGetParameter(key, out text))
			{
				outValue = Convert.ToSByte(text);
				return true;
			}
			return false;
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x0000F17C File Offset: 0x0000D37C
		public bool TryGetParameterAsVec3(string key, out Vec3 outValue)
		{
			outValue = default(Vec3);
			string text;
			if (this.TryGetParameter(key, out text))
			{
				string[] array = text.Split(new char[] { ';' });
				float num = Convert.ToSingle(array[0], CultureInfo.InvariantCulture);
				float num2 = Convert.ToSingle(array[1], CultureInfo.InvariantCulture);
				float num3 = Convert.ToSingle(array[2], CultureInfo.InvariantCulture);
				outValue = new Vec3(num, num2, num3, -1f);
				return true;
			}
			return false;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0000F1EC File Offset: 0x0000D3EC
		public bool TryGetParameterAsVec2(string key, out Vec2 outValue)
		{
			outValue = default(Vec2);
			string text;
			if (this.TryGetParameter(key, out text))
			{
				string[] array = text.Split(new char[] { ';' });
				float num = Convert.ToSingle(array[0], CultureInfo.InvariantCulture);
				float num2 = Convert.ToSingle(array[1], CultureInfo.InvariantCulture);
				outValue = new Vec2(num, num2);
				return true;
			}
			return false;
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x0000F247 File Offset: 0x0000D447
		public string GetParameter(string key)
		{
			return this._parameters[key];
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x06000448 RID: 1096 RVA: 0x0000F255 File Offset: 0x0000D455
		public IEnumerable<KeyValuePair<string, string>> Iterator
		{
			get
			{
				return this._parameters;
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x0000F260 File Offset: 0x0000D460
		public ParameterContainer Clone()
		{
			ParameterContainer parameterContainer = new ParameterContainer();
			foreach (KeyValuePair<string, string> keyValuePair in this._parameters)
			{
				parameterContainer._parameters.Add(keyValuePair.Key, keyValuePair.Value);
			}
			return parameterContainer;
		}

		// Token: 0x04000150 RID: 336
		private Dictionary<string, string> _parameters;
	}
}
