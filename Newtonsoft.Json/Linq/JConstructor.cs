using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B8 RID: 184
	[NullableContext(1)]
	[Nullable(0)]
	public class JConstructor : JContainer
	{
		// Token: 0x06000997 RID: 2455 RVA: 0x00027C24 File Offset: 0x00025E24
		public override async Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			await writer.WriteStartConstructorAsync(this._name ?? string.Empty, cancellationToken).ConfigureAwait(false);
			for (int i = 0; i < this._values.Count; i++)
			{
				await this._values[i].WriteToAsync(writer, cancellationToken, converters).ConfigureAwait(false);
			}
			await writer.WriteEndConstructorAsync(cancellationToken).ConfigureAwait(false);
		}

		// Token: 0x06000998 RID: 2456 RVA: 0x00027C7F File Offset: 0x00025E7F
		public new static Task<JConstructor> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JConstructor.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00027C8C File Offset: 0x00025E8C
		public new static async Task<JConstructor> LoadAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
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
					throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor c = new JConstructor((string)reader.Value);
			c.SetLineInfo(reader as IJsonLineInfo, settings);
			await c.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return c;
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600099A RID: 2458 RVA: 0x00027CDF File Offset: 0x00025EDF
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00027CE7 File Offset: 0x00025EE7
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._values.IndexOfReference(item);
		}

		// Token: 0x0600099C RID: 2460 RVA: 0x00027CFC File Offset: 0x00025EFC
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JConstructor jconstructor = content as JConstructor;
			if (jconstructor == null)
			{
				return;
			}
			if (jconstructor.Name != null)
			{
				this.Name = jconstructor.Name;
			}
			JContainer.MergeEnumerableContent(this, jconstructor, settings);
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600099D RID: 2461 RVA: 0x00027D30 File Offset: 0x00025F30
		// (set) Token: 0x0600099E RID: 2462 RVA: 0x00027D38 File Offset: 0x00025F38
		[Nullable(2)]
		public string Name
		{
			[NullableContext(2)]
			get
			{
				return this._name;
			}
			[NullableContext(2)]
			set
			{
				this._name = value;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600099F RID: 2463 RVA: 0x00027D41 File Offset: 0x00025F41
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Constructor;
			}
		}

		// Token: 0x060009A0 RID: 2464 RVA: 0x00027D44 File Offset: 0x00025F44
		public JConstructor()
		{
		}

		// Token: 0x060009A1 RID: 2465 RVA: 0x00027D57 File Offset: 0x00025F57
		public JConstructor(JConstructor other)
			: base(other, null)
		{
			this._name = other.Name;
		}

		// Token: 0x060009A2 RID: 2466 RVA: 0x00027D78 File Offset: 0x00025F78
		internal JConstructor(JConstructor other, [Nullable(2)] JsonCloneSettings settings)
			: base(other, settings)
		{
			this._name = other.Name;
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00027D99 File Offset: 0x00025F99
		public JConstructor(string name, params object[] content)
			: this(name, content)
		{
		}

		// Token: 0x060009A4 RID: 2468 RVA: 0x00027DA3 File Offset: 0x00025FA3
		public JConstructor(string name, object content)
			: this(name)
		{
			this.Add(content);
		}

		// Token: 0x060009A5 RID: 2469 RVA: 0x00027DB3 File Offset: 0x00025FB3
		public JConstructor(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException("Constructor name cannot be empty.", "name");
			}
			this._name = name;
		}

		// Token: 0x060009A6 RID: 2470 RVA: 0x00027DF4 File Offset: 0x00025FF4
		internal override bool DeepEquals(JToken node)
		{
			JConstructor jconstructor = node as JConstructor;
			return jconstructor != null && this._name == jconstructor.Name && base.ContentsEqual(jconstructor);
		}

		// Token: 0x060009A7 RID: 2471 RVA: 0x00027E27 File Offset: 0x00026027
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings = null)
		{
			return new JConstructor(this, settings);
		}

		// Token: 0x060009A8 RID: 2472 RVA: 0x00027E30 File Offset: 0x00026030
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartConstructor(this._name);
			int count = this._values.Count;
			for (int i = 0; i < count; i++)
			{
				this._values[i].WriteTo(writer, converters);
			}
			writer.WriteEndConstructor();
		}

		// Token: 0x170001C2 RID: 450
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (key is int)
				{
					int num = (int)key;
					return this.GetItem(num);
				}
				throw new ArgumentException("Accessed JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				if (key is int)
				{
					int num = (int)key;
					this.SetItem(num, value);
					return;
				}
				throw new ArgumentException("Set JConstructor values with invalid key value: {0}. Argument position index expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
			}
		}

		// Token: 0x060009AB RID: 2475 RVA: 0x00027F14 File Offset: 0x00026114
		internal override int GetDeepHashCode()
		{
			string name = this._name;
			return ((name != null) ? name.GetHashCode() : 0) ^ base.ContentsHashCode();
		}

		// Token: 0x060009AC RID: 2476 RVA: 0x00027F2F File Offset: 0x0002612F
		public new static JConstructor Load(JsonReader reader)
		{
			return JConstructor.Load(reader, null);
		}

		// Token: 0x060009AD RID: 2477 RVA: 0x00027F38 File Offset: 0x00026138
		public new static JConstructor Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartConstructor)
			{
				throw JsonReaderException.Create(reader, "Error reading JConstructor from JsonReader. Current JsonReader item is not a constructor: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JConstructor jconstructor = new JConstructor((string)reader.Value);
			jconstructor.SetLineInfo(reader as IJsonLineInfo, settings);
			jconstructor.ReadTokenFrom(reader, settings);
			return jconstructor;
		}

		// Token: 0x04000376 RID: 886
		[Nullable(2)]
		private string _name;

		// Token: 0x04000377 RID: 887
		private readonly List<JToken> _values = new List<JToken>();
	}
}
