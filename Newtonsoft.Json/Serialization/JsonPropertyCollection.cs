using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000091 RID: 145
	[NullableContext(1)]
	[Nullable(new byte[] { 0, 1, 1 })]
	public class JsonPropertyCollection : KeyedCollection<string, JsonProperty>
	{
		// Token: 0x06000748 RID: 1864 RVA: 0x0001D038 File Offset: 0x0001B238
		public JsonPropertyCollection(Type type)
			: base(StringComparer.Ordinal)
		{
			ValidationUtils.ArgumentNotNull(type, "type");
			this._type = type;
			this._list = (List<JsonProperty>)base.Items;
		}

		// Token: 0x06000749 RID: 1865 RVA: 0x0001D068 File Offset: 0x0001B268
		protected override string GetKeyForItem(JsonProperty item)
		{
			return item.PropertyName;
		}

		// Token: 0x0600074A RID: 1866 RVA: 0x0001D070 File Offset: 0x0001B270
		public void AddProperty(JsonProperty property)
		{
			if (base.Contains(property.PropertyName))
			{
				if (property.Ignored)
				{
					return;
				}
				JsonProperty jsonProperty = base[property.PropertyName];
				bool flag = true;
				if (jsonProperty.Ignored)
				{
					base.Remove(jsonProperty);
					flag = false;
				}
				else if (property.DeclaringType != null && jsonProperty.DeclaringType != null)
				{
					if (property.DeclaringType.IsSubclassOf(jsonProperty.DeclaringType) || (jsonProperty.DeclaringType.IsInterface() && property.DeclaringType.ImplementInterface(jsonProperty.DeclaringType)))
					{
						base.Remove(jsonProperty);
						flag = false;
					}
					if (jsonProperty.DeclaringType.IsSubclassOf(property.DeclaringType) || (property.DeclaringType.IsInterface() && jsonProperty.DeclaringType.ImplementInterface(property.DeclaringType)))
					{
						return;
					}
					if (this._type.ImplementInterface(jsonProperty.DeclaringType) && this._type.ImplementInterface(property.DeclaringType))
					{
						return;
					}
				}
				if (flag)
				{
					throw new JsonSerializationException("A member with the name '{0}' already exists on '{1}'. Use the JsonPropertyAttribute to specify another name.".FormatWith(CultureInfo.InvariantCulture, property.PropertyName, this._type));
				}
			}
			base.Add(property);
		}

		// Token: 0x0600074B RID: 1867 RVA: 0x0001D1A4 File Offset: 0x0001B3A4
		[return: Nullable(2)]
		public JsonProperty GetClosestMatchProperty(string propertyName)
		{
			JsonProperty jsonProperty = this.GetProperty(propertyName, StringComparison.Ordinal);
			if (jsonProperty == null)
			{
				jsonProperty = this.GetProperty(propertyName, StringComparison.OrdinalIgnoreCase);
			}
			return jsonProperty;
		}

		// Token: 0x0600074C RID: 1868 RVA: 0x0001D1C7 File Offset: 0x0001B3C7
		private bool TryGetProperty(string key, [Nullable(2)] [NotNullWhen(true)] out JsonProperty item)
		{
			if (base.Dictionary == null)
			{
				item = null;
				return false;
			}
			return base.Dictionary.TryGetValue(key, out item);
		}

		// Token: 0x0600074D RID: 1869 RVA: 0x0001D1E4 File Offset: 0x0001B3E4
		[return: Nullable(2)]
		public JsonProperty GetProperty(string propertyName, StringComparison comparisonType)
		{
			if (comparisonType != StringComparison.Ordinal)
			{
				for (int i = 0; i < this._list.Count; i++)
				{
					JsonProperty jsonProperty = this._list[i];
					if (string.Equals(propertyName, jsonProperty.PropertyName, comparisonType))
					{
						return jsonProperty;
					}
				}
				return null;
			}
			JsonProperty jsonProperty2;
			if (this.TryGetProperty(propertyName, out jsonProperty2))
			{
				return jsonProperty2;
			}
			return null;
		}

		// Token: 0x040002BF RID: 703
		private readonly Type _type;

		// Token: 0x040002C0 RID: 704
		private readonly List<JsonProperty> _list;
	}
}
