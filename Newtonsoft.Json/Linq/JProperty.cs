using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BC RID: 188
	[NullableContext(1)]
	[Nullable(0)]
	public class JProperty : JContainer
	{
		// Token: 0x06000A62 RID: 2658 RVA: 0x00029D30 File Offset: 0x00027F30
		public override Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			Task task = writer.WritePropertyNameAsync(this._name, cancellationToken);
			if (task.IsCompletedSuccessfully())
			{
				return this.WriteValueAsync(writer, cancellationToken, converters);
			}
			return this.WriteToAsync(task, writer, cancellationToken, converters);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00029D68 File Offset: 0x00027F68
		private async Task WriteToAsync(Task task, JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await task.ConfigureAwait(false);
			await this.WriteValueAsync(writer, cancellationToken, converters).ConfigureAwait(false);
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00029DCC File Offset: 0x00027FCC
		private Task WriteValueAsync(JsonWriter writer, CancellationToken cancellationToken, JsonConverter[] converters)
		{
			JToken value = this.Value;
			if (value == null)
			{
				return writer.WriteNullAsync(cancellationToken);
			}
			return value.WriteToAsync(writer, cancellationToken, converters);
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00029DF4 File Offset: 0x00027FF4
		public new static Task<JProperty> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JProperty.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00029E00 File Offset: 0x00028000
		public new static async Task<JProperty> LoadAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
		{
			if (reader.TokenType == JsonToken.None)
			{
				ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter = reader.ReadAsync(cancellationToken).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
				if (!configuredTaskAwaiter.GetResult())
				{
					throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty p = new JProperty((string)reader.Value);
			p.SetLineInfo(reader as IJsonLineInfo, settings);
			await p.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return p;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x00029E53 File Offset: 0x00028053
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._content;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x00029E5B File Offset: 0x0002805B
		public string Name
		{
			[DebuggerStepThrough]
			get
			{
				return this._name;
			}
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x06000A69 RID: 2665 RVA: 0x00029E63 File Offset: 0x00028063
		// (set) Token: 0x06000A6A RID: 2666 RVA: 0x00029E70 File Offset: 0x00028070
		public new JToken Value
		{
			[DebuggerStepThrough]
			get
			{
				return this._content._token;
			}
			set
			{
				base.CheckReentrancy();
				JToken jtoken = value ?? JValue.CreateNull();
				if (this._content._token == null)
				{
					this.InsertItem(0, jtoken, false, true);
					return;
				}
				this.SetItem(0, jtoken);
			}
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x00029EAF File Offset: 0x000280AF
		public JProperty(JProperty other)
			: base(other, null)
		{
			this._name = other.Name;
		}

		// Token: 0x06000A6C RID: 2668 RVA: 0x00029ED0 File Offset: 0x000280D0
		internal JProperty(JProperty other, [Nullable(2)] JsonCloneSettings settings)
			: base(other, settings)
		{
			this._name = other.Name;
		}

		// Token: 0x06000A6D RID: 2669 RVA: 0x00029EF1 File Offset: 0x000280F1
		internal override JToken GetItem(int index)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			return this.Value;
		}

		// Token: 0x06000A6E RID: 2670 RVA: 0x00029F04 File Offset: 0x00028104
		[NullableContext(2)]
		internal override void SetItem(int index, JToken item)
		{
			if (index != 0)
			{
				throw new ArgumentOutOfRangeException();
			}
			if (JContainer.IsTokenUnchanged(this.Value, item))
			{
				return;
			}
			JObject jobject = (JObject)base.Parent;
			if (jobject != null)
			{
				jobject.InternalPropertyChanging(this);
			}
			base.SetItem(0, item);
			JObject jobject2 = (JObject)base.Parent;
			if (jobject2 == null)
			{
				return;
			}
			jobject2.InternalPropertyChanged(this);
		}

		// Token: 0x06000A6F RID: 2671 RVA: 0x00029F5E File Offset: 0x0002815E
		[NullableContext(2)]
		internal override bool RemoveItem(JToken item)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A70 RID: 2672 RVA: 0x00029F7E File Offset: 0x0002817E
		internal override void RemoveItemAt(int index)
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A71 RID: 2673 RVA: 0x00029F9E File Offset: 0x0002819E
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._content.IndexOf(item);
		}

		// Token: 0x06000A72 RID: 2674 RVA: 0x00029FB4 File Offset: 0x000281B4
		[NullableContext(2)]
		internal override bool InsertItem(int index, JToken item, bool skipParentCheck, bool copyAnnotations)
		{
			if (item != null && item.Type == JTokenType.Comment)
			{
				return false;
			}
			if (this.Value != null)
			{
				throw new JsonException("{0} cannot have multiple values.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
			}
			return base.InsertItem(0, item, false, copyAnnotations);
		}

		// Token: 0x06000A73 RID: 2675 RVA: 0x0002A001 File Offset: 0x00028201
		[NullableContext(2)]
		internal override bool ContainsItem(JToken item)
		{
			return this.Value == item;
		}

		// Token: 0x06000A74 RID: 2676 RVA: 0x0002A00C File Offset: 0x0002820C
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JProperty jproperty = content as JProperty;
			JToken jtoken = ((jproperty != null) ? jproperty.Value : null);
			if (jtoken != null && jtoken.Type != JTokenType.Null)
			{
				this.Value = jtoken;
			}
		}

		// Token: 0x06000A75 RID: 2677 RVA: 0x0002A040 File Offset: 0x00028240
		internal override void ClearItems()
		{
			throw new JsonException("Cannot add or remove items from {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JProperty)));
		}

		// Token: 0x06000A76 RID: 2678 RVA: 0x0002A060 File Offset: 0x00028260
		internal override bool DeepEquals(JToken node)
		{
			JProperty jproperty = node as JProperty;
			return jproperty != null && this._name == jproperty.Name && base.ContentsEqual(jproperty);
		}

		// Token: 0x06000A77 RID: 2679 RVA: 0x0002A093 File Offset: 0x00028293
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings)
		{
			return new JProperty(this, settings);
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x06000A78 RID: 2680 RVA: 0x0002A09C File Offset: 0x0002829C
		public override JTokenType Type
		{
			[DebuggerStepThrough]
			get
			{
				return JTokenType.Property;
			}
		}

		// Token: 0x06000A79 RID: 2681 RVA: 0x0002A09F File Offset: 0x0002829F
		internal JProperty(string name)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
		}

		// Token: 0x06000A7A RID: 2682 RVA: 0x0002A0C4 File Offset: 0x000282C4
		public JProperty(string name, params object[] content)
			: this(name, content)
		{
		}

		// Token: 0x06000A7B RID: 2683 RVA: 0x0002A0D0 File Offset: 0x000282D0
		public JProperty(string name, [Nullable(2)] object content)
		{
			ValidationUtils.ArgumentNotNull(name, "name");
			this._name = name;
			this.Value = (base.IsMultiContent(content) ? new JArray(content) : JContainer.CreateFromContent(content));
		}

		// Token: 0x06000A7C RID: 2684 RVA: 0x0002A120 File Offset: 0x00028320
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WritePropertyName(this._name);
			JToken value = this.Value;
			if (value != null)
			{
				value.WriteTo(writer, converters);
				return;
			}
			writer.WriteNull();
		}

		// Token: 0x06000A7D RID: 2685 RVA: 0x0002A152 File Offset: 0x00028352
		internal override int GetDeepHashCode()
		{
			int hashCode = this._name.GetHashCode();
			JToken value = this.Value;
			return hashCode ^ ((value != null) ? value.GetDeepHashCode() : 0);
		}

		// Token: 0x06000A7E RID: 2686 RVA: 0x0002A172 File Offset: 0x00028372
		public new static JProperty Load(JsonReader reader)
		{
			return JProperty.Load(reader, null);
		}

		// Token: 0x06000A7F RID: 2687 RVA: 0x0002A17C File Offset: 0x0002837C
		public new static JProperty Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.PropertyName)
			{
				throw JsonReaderException.Create(reader, "Error reading JProperty from JsonReader. Current JsonReader item is not a property: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JProperty jproperty = new JProperty((string)reader.Value);
			jproperty.SetLineInfo(reader as IJsonLineInfo, settings);
			jproperty.ReadTokenFrom(reader, settings);
			return jproperty;
		}

		// Token: 0x04000382 RID: 898
		private readonly JProperty.JPropertyList _content = new JProperty.JPropertyList();

		// Token: 0x04000383 RID: 899
		private readonly string _name;

		// Token: 0x020001C7 RID: 455
		[Nullable(0)]
		private class JPropertyList : IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
		{
			// Token: 0x06000F9E RID: 3998 RVA: 0x00044A9E File Offset: 0x00042C9E
			public IEnumerator<JToken> GetEnumerator()
			{
				if (this._token != null)
				{
					yield return this._token;
				}
				yield break;
			}

			// Token: 0x06000F9F RID: 3999 RVA: 0x00044AAD File Offset: 0x00042CAD
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			// Token: 0x06000FA0 RID: 4000 RVA: 0x00044AB5 File Offset: 0x00042CB5
			public void Add(JToken item)
			{
				this._token = item;
			}

			// Token: 0x06000FA1 RID: 4001 RVA: 0x00044ABE File Offset: 0x00042CBE
			public void Clear()
			{
				this._token = null;
			}

			// Token: 0x06000FA2 RID: 4002 RVA: 0x00044AC7 File Offset: 0x00042CC7
			public bool Contains(JToken item)
			{
				return this._token == item;
			}

			// Token: 0x06000FA3 RID: 4003 RVA: 0x00044AD2 File Offset: 0x00042CD2
			public void CopyTo(JToken[] array, int arrayIndex)
			{
				if (this._token != null)
				{
					array[arrayIndex] = this._token;
				}
			}

			// Token: 0x06000FA4 RID: 4004 RVA: 0x00044AE5 File Offset: 0x00042CE5
			public bool Remove(JToken item)
			{
				if (this._token == item)
				{
					this._token = null;
					return true;
				}
				return false;
			}

			// Token: 0x170002A0 RID: 672
			// (get) Token: 0x06000FA5 RID: 4005 RVA: 0x00044AFA File Offset: 0x00042CFA
			public int Count
			{
				get
				{
					if (this._token == null)
					{
						return 0;
					}
					return 1;
				}
			}

			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x06000FA6 RID: 4006 RVA: 0x00044B07 File Offset: 0x00042D07
			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			// Token: 0x06000FA7 RID: 4007 RVA: 0x00044B0A File Offset: 0x00042D0A
			public int IndexOf(JToken item)
			{
				if (this._token != item)
				{
					return -1;
				}
				return 0;
			}

			// Token: 0x06000FA8 RID: 4008 RVA: 0x00044B18 File Offset: 0x00042D18
			public void Insert(int index, JToken item)
			{
				if (index == 0)
				{
					this._token = item;
				}
			}

			// Token: 0x06000FA9 RID: 4009 RVA: 0x00044B24 File Offset: 0x00042D24
			public void RemoveAt(int index)
			{
				if (index == 0)
				{
					this._token = null;
				}
			}

			// Token: 0x170002A2 RID: 674
			public JToken this[int index]
			{
				get
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					return this._token;
				}
				set
				{
					if (index != 0)
					{
						throw new IndexOutOfRangeException();
					}
					this._token = value;
				}
			}

			// Token: 0x040007C5 RID: 1989
			[Nullable(2)]
			internal JToken _token;
		}
	}
}
