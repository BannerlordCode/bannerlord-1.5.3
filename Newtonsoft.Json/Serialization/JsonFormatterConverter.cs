using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x0200008B RID: 139
	[NullableContext(1)]
	[Nullable(0)]
	internal class JsonFormatterConverter : IFormatterConverter
	{
		// Token: 0x060006D3 RID: 1747 RVA: 0x0001C81D File Offset: 0x0001AA1D
		public JsonFormatterConverter(JsonSerializerInternalReader reader, JsonISerializableContract contract, [Nullable(2)] JsonProperty member)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			ValidationUtils.ArgumentNotNull(contract, "contract");
			this._reader = reader;
			this._contract = contract;
			this._member = member;
		}

		// Token: 0x060006D4 RID: 1748 RVA: 0x0001C850 File Offset: 0x0001AA50
		private T GetTokenValue<[Nullable(2)] T>(object value)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			return (T)((object)global::System.Convert.ChangeType(((JValue)value).Value, typeof(T), CultureInfo.InvariantCulture));
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0001C884 File Offset: 0x0001AA84
		public object Convert(object value, Type type)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JToken jtoken = value as JToken;
			if (jtoken == null)
			{
				throw new ArgumentException("Value is not a JToken.", "value");
			}
			return this._reader.CreateISerializableItem(jtoken, type, this._contract, this._member);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0001C8D0 File Offset: 0x0001AAD0
		public object Convert(object value, TypeCode typeCode)
		{
			ValidationUtils.ArgumentNotNull(value, "value");
			JValue jvalue = value as JValue;
			return global::System.Convert.ChangeType((jvalue != null) ? jvalue.Value : value, typeCode, CultureInfo.InvariantCulture);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x0001C906 File Offset: 0x0001AB06
		public bool ToBoolean(object value)
		{
			return this.GetTokenValue<bool>(value);
		}

		// Token: 0x060006D8 RID: 1752 RVA: 0x0001C90F File Offset: 0x0001AB0F
		public byte ToByte(object value)
		{
			return this.GetTokenValue<byte>(value);
		}

		// Token: 0x060006D9 RID: 1753 RVA: 0x0001C918 File Offset: 0x0001AB18
		public char ToChar(object value)
		{
			return this.GetTokenValue<char>(value);
		}

		// Token: 0x060006DA RID: 1754 RVA: 0x0001C921 File Offset: 0x0001AB21
		public DateTime ToDateTime(object value)
		{
			return this.GetTokenValue<DateTime>(value);
		}

		// Token: 0x060006DB RID: 1755 RVA: 0x0001C92A File Offset: 0x0001AB2A
		public decimal ToDecimal(object value)
		{
			return this.GetTokenValue<decimal>(value);
		}

		// Token: 0x060006DC RID: 1756 RVA: 0x0001C933 File Offset: 0x0001AB33
		public double ToDouble(object value)
		{
			return this.GetTokenValue<double>(value);
		}

		// Token: 0x060006DD RID: 1757 RVA: 0x0001C93C File Offset: 0x0001AB3C
		public short ToInt16(object value)
		{
			return this.GetTokenValue<short>(value);
		}

		// Token: 0x060006DE RID: 1758 RVA: 0x0001C945 File Offset: 0x0001AB45
		public int ToInt32(object value)
		{
			return this.GetTokenValue<int>(value);
		}

		// Token: 0x060006DF RID: 1759 RVA: 0x0001C94E File Offset: 0x0001AB4E
		public long ToInt64(object value)
		{
			return this.GetTokenValue<long>(value);
		}

		// Token: 0x060006E0 RID: 1760 RVA: 0x0001C957 File Offset: 0x0001AB57
		public sbyte ToSByte(object value)
		{
			return this.GetTokenValue<sbyte>(value);
		}

		// Token: 0x060006E1 RID: 1761 RVA: 0x0001C960 File Offset: 0x0001AB60
		public float ToSingle(object value)
		{
			return this.GetTokenValue<float>(value);
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x0001C969 File Offset: 0x0001AB69
		public string ToString(object value)
		{
			return this.GetTokenValue<string>(value);
		}

		// Token: 0x060006E3 RID: 1763 RVA: 0x0001C972 File Offset: 0x0001AB72
		public ushort ToUInt16(object value)
		{
			return this.GetTokenValue<ushort>(value);
		}

		// Token: 0x060006E4 RID: 1764 RVA: 0x0001C97B File Offset: 0x0001AB7B
		public uint ToUInt32(object value)
		{
			return this.GetTokenValue<uint>(value);
		}

		// Token: 0x060006E5 RID: 1765 RVA: 0x0001C984 File Offset: 0x0001AB84
		public ulong ToUInt64(object value)
		{
			return this.GetTokenValue<ulong>(value);
		}

		// Token: 0x0400028B RID: 651
		private readonly JsonSerializerInternalReader _reader;

		// Token: 0x0400028C RID: 652
		private readonly JsonISerializableContract _contract;

		// Token: 0x0400028D RID: 653
		[Nullable(2)]
		private readonly JsonProperty _member;
	}
}
