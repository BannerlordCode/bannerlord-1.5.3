using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B7 RID: 183
	[NullableContext(1)]
	[Nullable(0)]
	public class JArray : JContainer, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable
	{
		// Token: 0x06000973 RID: 2419 RVA: 0x00027820 File Offset: 0x00025A20
		public override async Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await writer.WriteStartArrayAsync(cancellationToken).ConfigureAwait(false);
			for (int i = 0; i < this._values.Count; i++)
			{
				await this._values[i].WriteToAsync(writer, cancellationToken, converters).ConfigureAwait(false);
			}
			await writer.WriteEndArrayAsync(cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x06000974 RID: 2420 RVA: 0x0002787B File Offset: 0x00025A7B
		public new static Task<JArray> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JArray.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000975 RID: 2421 RVA: 0x00027888 File Offset: 0x00025A88
		public new static async Task<JArray> LoadAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
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
					throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray a = new JArray();
			a.SetLineInfo(reader as IJsonLineInfo, settings);
			await a.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return a;
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000976 RID: 2422 RVA: 0x000278DB File Offset: 0x00025ADB
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x000278E3 File Offset: 0x00025AE3
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Array;
			}
		}

		// Token: 0x06000978 RID: 2424 RVA: 0x000278E6 File Offset: 0x00025AE6
		public JArray()
		{
		}

		// Token: 0x06000979 RID: 2425 RVA: 0x000278F9 File Offset: 0x00025AF9
		public JArray(JArray other)
			: base(other, null)
		{
		}

		// Token: 0x0600097A RID: 2426 RVA: 0x0002790E File Offset: 0x00025B0E
		internal JArray(JArray other, [Nullable(2)] JsonCloneSettings settings)
			: base(other, settings)
		{
		}

		// Token: 0x0600097B RID: 2427 RVA: 0x00027923 File Offset: 0x00025B23
		public JArray(params object[] content)
			: this(content)
		{
		}

		// Token: 0x0600097C RID: 2428 RVA: 0x0002792C File Offset: 0x00025B2C
		public JArray(object content)
		{
			this.Add(content);
		}

		// Token: 0x0600097D RID: 2429 RVA: 0x00027948 File Offset: 0x00025B48
		internal override bool DeepEquals(JToken node)
		{
			JArray jarray = node as JArray;
			return jarray != null && base.ContentsEqual(jarray);
		}

		// Token: 0x0600097E RID: 2430 RVA: 0x00027968 File Offset: 0x00025B68
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings = null)
		{
			return new JArray(this, settings);
		}

		// Token: 0x0600097F RID: 2431 RVA: 0x00027971 File Offset: 0x00025B71
		public new static JArray Load(JsonReader reader)
		{
			return JArray.Load(reader, null);
		}

		// Token: 0x06000980 RID: 2432 RVA: 0x0002797C File Offset: 0x00025B7C
		public new static JArray Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartArray)
			{
				throw JsonReaderException.Create(reader, "Error reading JArray from JsonReader. Current JsonReader item is not an array: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JArray jarray = new JArray();
			jarray.SetLineInfo(reader as IJsonLineInfo, settings);
			jarray.ReadTokenFrom(reader, settings);
			return jarray;
		}

		// Token: 0x06000981 RID: 2433 RVA: 0x000279F0 File Offset: 0x00025BF0
		public new static JArray Parse(string json)
		{
			return JArray.Parse(json, null);
		}

		// Token: 0x06000982 RID: 2434 RVA: 0x000279FC File Offset: 0x00025BFC
		public new static JArray Parse(string json, [Nullable(2)] JsonLoadSettings settings)
		{
			JArray jarray2;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JArray jarray = JArray.Load(jsonReader, settings);
				while (jsonReader.Read())
				{
				}
				jarray2 = jarray;
			}
			return jarray2;
		}

		// Token: 0x06000983 RID: 2435 RVA: 0x00027A44 File Offset: 0x00025C44
		public new static JArray FromObject(object o)
		{
			return JArray.FromObject(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x06000984 RID: 2436 RVA: 0x00027A54 File Offset: 0x00025C54
		public new static JArray FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Array)
			{
				throw new ArgumentException("Object serialized to {0}. JArray instance expected.".FormatWith(CultureInfo.InvariantCulture, jtoken.Type));
			}
			return (JArray)jtoken;
		}

		// Token: 0x06000985 RID: 2437 RVA: 0x00027A98 File Offset: 0x00025C98
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartArray();
			for (int i = 0; i < this._values.Count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndArray();
		}

		// Token: 0x170001BC RID: 444
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Accessed JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this.GetItem((int)key);
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (!(key is int))
				{
					throw new ArgumentException("Set JArray values with invalid key value: {0}. Int32 array index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this.SetItem((int)key, value);
			}
		}

		// Token: 0x170001BD RID: 445
		public JToken this[int index]
		{
			get
			{
				return this.GetItem(index);
			}
			set
			{
				this.SetItem(index, value);
			}
		}

		// Token: 0x0600098A RID: 2442 RVA: 0x00027B66 File Offset: 0x00025D66
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._values.IndexOfReference(item);
		}

		// Token: 0x0600098B RID: 2443 RVA: 0x00027B7C File Offset: 0x00025D7C
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			IEnumerable enumerable = ((base.IsMultiContent(content) || content is JArray) ? ((IEnumerable)content) : null);
			if (enumerable == null)
			{
				return;
			}
			JContainer.MergeEnumerableContent(this, enumerable, settings);
		}

		// Token: 0x0600098C RID: 2444 RVA: 0x00027BB0 File Offset: 0x00025DB0
		public int IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x0600098D RID: 2445 RVA: 0x00027BB9 File Offset: 0x00025DB9
		public void Insert(int index, JToken item)
		{
			this.InsertItem(index, item, false, true);
		}

		// Token: 0x0600098E RID: 2446 RVA: 0x00027BC6 File Offset: 0x00025DC6
		public void RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x0600098F RID: 2447 RVA: 0x00027BD0 File Offset: 0x00025DD0
		public IEnumerator<JToken> GetEnumerator()
		{
			return this.Children().GetEnumerator();
		}

		// Token: 0x06000990 RID: 2448 RVA: 0x00027BEB File Offset: 0x00025DEB
		public void Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x06000991 RID: 2449 RVA: 0x00027BF4 File Offset: 0x00025DF4
		public void Clear()
		{
			this.ClearItems();
		}

		// Token: 0x06000992 RID: 2450 RVA: 0x00027BFC File Offset: 0x00025DFC
		public bool Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x06000993 RID: 2451 RVA: 0x00027C05 File Offset: 0x00025E05
		public void CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000994 RID: 2452 RVA: 0x00027C0F File Offset: 0x00025E0F
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000995 RID: 2453 RVA: 0x00027C12 File Offset: 0x00025E12
		public bool Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x06000996 RID: 2454 RVA: 0x00027C1B File Offset: 0x00025E1B
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x04000375 RID: 885
		private readonly List<JToken> _values = new List<JToken>();
	}
}
