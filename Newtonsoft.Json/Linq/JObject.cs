using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000BB RID: 187
	[NullableContext(1)]
	[Nullable(0)]
	public class JObject : JContainer, IDictionary<string, JToken>, ICollection<KeyValuePair<string, JToken>>, IEnumerable<KeyValuePair<string, JToken>>, IEnumerable, INotifyPropertyChanged, ICustomTypeDescriptor, INotifyPropertyChanging
	{
		// Token: 0x06000A1B RID: 2587 RVA: 0x00029234 File Offset: 0x00027434
		public override Task WriteToAsync(JsonWriter writer, CancellationToken cancellationToken, params JsonConverter[] converters)
		{
			Task task = writer.WriteStartObjectAsync(cancellationToken);
			if (!task.IsCompletedSuccessfully())
			{
				return this.<WriteToAsync>g__AwaitProperties|0_0(task, 0, writer, cancellationToken, converters);
			}
			for (int i = 0; i < this._properties.Count; i++)
			{
				task = this._properties[i].WriteToAsync(writer, cancellationToken, converters);
				if (!task.IsCompletedSuccessfully())
				{
					return this.<WriteToAsync>g__AwaitProperties|0_0(task, i + 1, writer, cancellationToken, converters);
				}
			}
			return writer.WriteEndObjectAsync(cancellationToken);
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x000292A5 File Offset: 0x000274A5
		public new static Task<JObject> LoadAsync(JsonReader reader, CancellationToken cancellationToken = default(CancellationToken))
		{
			return JObject.LoadAsync(reader, null, cancellationToken);
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x000292B0 File Offset: 0x000274B0
		public new static async Task<JObject> LoadAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
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
					throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
				}
			}
			await reader.MoveToContentAsync(cancellationToken).ConfigureAwait(false);
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JObject o = new JObject();
			o.SetLineInfo(reader as IJsonLineInfo, settings);
			await o.ReadTokenFromAsync(reader, settings, cancellationToken).ConfigureAwait(false);
			return o;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000A1E RID: 2590 RVA: 0x00029303 File Offset: 0x00027503
		protected override IList<JToken> ChildrenTokens
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000A1F RID: 2591 RVA: 0x0002930C File Offset: 0x0002750C
		// (remove) Token: 0x06000A20 RID: 2592 RVA: 0x00029344 File Offset: 0x00027544
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event PropertyChangedEventHandler PropertyChanged;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000A21 RID: 2593 RVA: 0x0002937C File Offset: 0x0002757C
		// (remove) Token: 0x06000A22 RID: 2594 RVA: 0x000293B4 File Offset: 0x000275B4
		[Nullable(2)]
		[method: NullableContext(2)]
		[field: Nullable(2)]
		public event PropertyChangingEventHandler PropertyChanging;

		// Token: 0x06000A23 RID: 2595 RVA: 0x000293E9 File Offset: 0x000275E9
		public JObject()
		{
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x000293FC File Offset: 0x000275FC
		public JObject(JObject other)
			: base(other, null)
		{
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x00029411 File Offset: 0x00027611
		internal JObject(JObject other, [Nullable(2)] JsonCloneSettings settings)
			: base(other, settings)
		{
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x00029426 File Offset: 0x00027626
		public JObject(params object[] content)
			: this(content)
		{
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0002942F File Offset: 0x0002762F
		public JObject(object content)
		{
			this.Add(content);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0002944C File Offset: 0x0002764C
		internal override bool DeepEquals(JToken node)
		{
			JObject jobject = node as JObject;
			return jobject != null && this._properties.Compare(jobject._properties);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x00029476 File Offset: 0x00027676
		[NullableContext(2)]
		internal override int IndexOfItem(JToken item)
		{
			if (item == null)
			{
				return -1;
			}
			return this._properties.IndexOfReference(item);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x00029489 File Offset: 0x00027689
		[NullableContext(2)]
		internal override bool InsertItem(int index, JToken item, bool skipParentCheck, bool copyAnnotations)
		{
			return (item == null || item.Type != JTokenType.Comment) && base.InsertItem(index, item, skipParentCheck, copyAnnotations);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000294A4 File Offset: 0x000276A4
		internal override void ValidateToken(JToken o, [Nullable(2)] JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type != JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), base.GetType()));
			}
			JProperty jproperty = (JProperty)o;
			if (existing != null)
			{
				JProperty jproperty2 = (JProperty)existing;
				if (jproperty.Name == jproperty2.Name)
				{
					return;
				}
			}
			if (this._properties.TryGetValue(jproperty.Name, out existing))
			{
				throw new ArgumentException("Can not add property {0} to {1}. Property with the same name already exists on object.".FormatWith(CultureInfo.InvariantCulture, jproperty.Name, base.GetType()));
			}
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x00029544 File Offset: 0x00027744
		internal override void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings)
		{
			JObject jobject = content as JObject;
			if (jobject == null)
			{
				return;
			}
			foreach (KeyValuePair<string, JToken> keyValuePair in jobject)
			{
				JProperty jproperty = this.Property(keyValuePair.Key, (settings != null) ? settings.PropertyNameComparison : StringComparison.Ordinal);
				if (jproperty == null)
				{
					this.Add(keyValuePair.Key, keyValuePair.Value);
				}
				else if (keyValuePair.Value != null)
				{
					JContainer jcontainer = jproperty.Value as JContainer;
					if (jcontainer == null || jcontainer.Type != keyValuePair.Value.Type)
					{
						if (!JObject.IsNull(keyValuePair.Value) || (settings != null && settings.MergeNullValueHandling == MergeNullValueHandling.Merge))
						{
							jproperty.Value = keyValuePair.Value;
						}
					}
					else
					{
						jcontainer.Merge(keyValuePair.Value, settings);
					}
				}
			}
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x00029630 File Offset: 0x00027830
		private static bool IsNull(JToken token)
		{
			if (token.Type == JTokenType.Null)
			{
				return true;
			}
			JValue jvalue = token as JValue;
			return jvalue != null && jvalue.Value == null;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00029660 File Offset: 0x00027860
		internal void InternalPropertyChanged(JProperty childProperty)
		{
			this.OnPropertyChanged(childProperty.Name);
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, this.IndexOfItem(childProperty)));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, childProperty, childProperty, this.IndexOfItem(childProperty)));
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x000296B1 File Offset: 0x000278B1
		internal void InternalPropertyChanging(JProperty childProperty)
		{
			this.OnPropertyChanging(childProperty.Name);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x000296BF File Offset: 0x000278BF
		internal override JToken CloneToken([Nullable(2)] JsonCloneSettings settings)
		{
			return new JObject(this, settings);
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000A31 RID: 2609 RVA: 0x000296C8 File Offset: 0x000278C8
		public override JTokenType Type
		{
			get
			{
				return JTokenType.Object;
			}
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x000296CB File Offset: 0x000278CB
		public IEnumerable<JProperty> Properties()
		{
			return this._properties.Cast<JProperty>();
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x000296D8 File Offset: 0x000278D8
		[return: Nullable(2)]
		public JProperty Property(string name)
		{
			return this.Property(name, StringComparison.Ordinal);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x000296E4 File Offset: 0x000278E4
		[return: Nullable(2)]
		public JProperty Property(string name, StringComparison comparison)
		{
			if (name == null)
			{
				return null;
			}
			JToken jtoken;
			if (this._properties.TryGetValue(name, out jtoken))
			{
				return (JProperty)jtoken;
			}
			if (comparison != StringComparison.Ordinal)
			{
				for (int i = 0; i < this._properties.Count; i++)
				{
					JProperty jproperty = (JProperty)this._properties[i];
					if (string.Equals(jproperty.Name, name, comparison))
					{
						return jproperty;
					}
				}
			}
			return null;
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0002974B File Offset: 0x0002794B
		[return: Nullable(new byte[] { 0, 1 })]
		public JEnumerable<JToken> PropertyValues()
		{
			return new JEnumerable<JToken>(from p in this.Properties()
				select p.Value);
		}

		// Token: 0x170001DB RID: 475
		[Nullable(2)]
		public override JToken this[object key]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Accessed JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				return this[text];
			}
			[param: Nullable(2)]
			set
			{
				ValidationUtils.ArgumentNotNull(key, "key");
				string text = key as string;
				if (text == null)
				{
					throw new ArgumentException("Set JObject values with invalid key value: {0}. Object property name expected.".FormatWith(CultureInfo.InvariantCulture, MiscellaneousUtils.ToString(key)));
				}
				this[text] = value;
			}
		}

		// Token: 0x170001DC RID: 476
		[Nullable(2)]
		public JToken this[string propertyName]
		{
			[return: Nullable(2)]
			get
			{
				ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
				JProperty jproperty = this.Property(propertyName, StringComparison.Ordinal);
				if (jproperty == null)
				{
					return null;
				}
				return jproperty.Value;
			}
			[param: Nullable(2)]
			set
			{
				JProperty jproperty = this.Property(propertyName, StringComparison.Ordinal);
				if (jproperty != null)
				{
					jproperty.Value = value;
					return;
				}
				this.OnPropertyChanging(propertyName);
				this.Add(propertyName, value);
				this.OnPropertyChanged(propertyName);
			}
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0002985F File Offset: 0x00027A5F
		public new static JObject Load(JsonReader reader)
		{
			return JObject.Load(reader, null);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00029868 File Offset: 0x00027A68
		public new static JObject Load(JsonReader reader, [Nullable(2)] JsonLoadSettings settings)
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			if (reader.TokenType == JsonToken.None && !reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader.");
			}
			reader.MoveToContent();
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw JsonReaderException.Create(reader, "Error reading JObject from JsonReader. Current JsonReader item is not an object: {0}".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
			}
			JObject jobject = new JObject();
			jobject.SetLineInfo(reader as IJsonLineInfo, settings);
			jobject.ReadTokenFrom(reader, settings);
			return jobject;
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x000298E7 File Offset: 0x00027AE7
		public new static JObject Parse(string json)
		{
			return JObject.Parse(json, null);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x000298F0 File Offset: 0x00027AF0
		public new static JObject Parse(string json, [Nullable(2)] JsonLoadSettings settings)
		{
			JObject jobject2;
			using (JsonReader jsonReader = new JsonTextReader(new StringReader(json)))
			{
				JObject jobject = JObject.Load(jsonReader, settings);
				while (jsonReader.Read())
				{
				}
				jobject2 = jobject;
			}
			return jobject2;
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00029938 File Offset: 0x00027B38
		public new static JObject FromObject(object o)
		{
			return JObject.FromObject(o, JsonSerializer.CreateDefault());
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00029948 File Offset: 0x00027B48
		public new static JObject FromObject(object o, JsonSerializer jsonSerializer)
		{
			JToken jtoken = JToken.FromObjectInternal(o, jsonSerializer);
			if (jtoken.Type != JTokenType.Object)
			{
				throw new ArgumentException("Object serialized to {0}. JObject instance expected.".FormatWith(CultureInfo.InvariantCulture, jtoken.Type));
			}
			return (JObject)jtoken;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0002998C File Offset: 0x00027B8C
		public override void WriteTo(JsonWriter writer, params JsonConverter[] converters)
		{
			writer.WriteStartObject();
			for (int i = 0; i < this._properties.Count; i++)
			{
				this._properties[i].WriteTo(writer, converters);
			}
			writer.WriteEndObject();
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x000299CE File Offset: 0x00027BCE
		[NullableContext(2)]
		public JToken GetValue(string propertyName)
		{
			return this.GetValue(propertyName, StringComparison.Ordinal);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x000299D8 File Offset: 0x00027BD8
		[NullableContext(2)]
		public JToken GetValue(string propertyName, StringComparison comparison)
		{
			if (propertyName == null)
			{
				return null;
			}
			JProperty jproperty = this.Property(propertyName, comparison);
			if (jproperty == null)
			{
				return null;
			}
			return jproperty.Value;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x000299F2 File Offset: 0x00027BF2
		public bool TryGetValue(string propertyName, StringComparison comparison, [Nullable(2)] [NotNullWhen(true)] out JToken value)
		{
			value = this.GetValue(propertyName, comparison);
			return value != null;
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00029A03 File Offset: 0x00027C03
		public void Add(string propertyName, [Nullable(2)] JToken value)
		{
			this.Add(new JProperty(propertyName, value));
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00029A12 File Offset: 0x00027C12
		public bool ContainsKey(string propertyName)
		{
			ValidationUtils.ArgumentNotNull(propertyName, "propertyName");
			return this._properties.Contains(propertyName);
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000A46 RID: 2630 RVA: 0x00029A2B File Offset: 0x00027C2B
		ICollection<string> IDictionary<string, JToken>.Keys
		{
			get
			{
				return this._properties.Keys;
			}
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00029A38 File Offset: 0x00027C38
		public bool Remove(string propertyName)
		{
			JProperty jproperty = this.Property(propertyName, StringComparison.Ordinal);
			if (jproperty == null)
			{
				return false;
			}
			jproperty.Remove();
			return true;
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00029A5C File Offset: 0x00027C5C
		public bool TryGetValue(string propertyName, [Nullable(2)] [NotNullWhen(true)] out JToken value)
		{
			JProperty jproperty = this.Property(propertyName, StringComparison.Ordinal);
			if (jproperty == null)
			{
				value = null;
				return false;
			}
			value = jproperty.Value;
			return true;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000A49 RID: 2633 RVA: 0x00029A83 File Offset: 0x00027C83
		[Nullable(new byte[] { 1, 2 })]
		ICollection<JToken> IDictionary<string, JToken>.Values
		{
			[return: Nullable(new byte[] { 1, 2 })]
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00029A8A File Offset: 0x00027C8A
		void ICollection<KeyValuePair<string, JToken>>.Add([Nullable(new byte[] { 0, 1, 2 })] KeyValuePair<string, JToken> item)
		{
			this.Add(new JProperty(item.Key, item.Value));
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x00029AA5 File Offset: 0x00027CA5
		void ICollection<KeyValuePair<string, JToken>>.Clear()
		{
			base.RemoveAll();
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00029AB0 File Offset: 0x00027CB0
		bool ICollection<KeyValuePair<string, JToken>>.Contains([Nullable(new byte[] { 0, 1, 2 })] KeyValuePair<string, JToken> item)
		{
			JProperty jproperty = this.Property(item.Key, StringComparison.Ordinal);
			return jproperty != null && jproperty.Value == item.Value;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00029AE0 File Offset: 0x00027CE0
		void ICollection<KeyValuePair<string, JToken>>.CopyTo([Nullable(new byte[] { 1, 0, 1, 2 })] KeyValuePair<string, JToken>[] array, int arrayIndex)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "arrayIndex is less than 0.");
			}
			if (arrayIndex >= array.Length && arrayIndex != 0)
			{
				throw new ArgumentException("arrayIndex is equal to or greater than the length of array.");
			}
			if (base.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken jtoken in this._properties)
			{
				JProperty jproperty = (JProperty)jtoken;
				array[arrayIndex + num] = new KeyValuePair<string, JToken>(jproperty.Name, jproperty.Value);
				num++;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000A4E RID: 2638 RVA: 0x00029B9C File Offset: 0x00027D9C
		bool ICollection<KeyValuePair<string, JToken>>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00029B9F File Offset: 0x00027D9F
		bool ICollection<KeyValuePair<string, JToken>>.Remove([Nullable(new byte[] { 0, 1, 2 })] KeyValuePair<string, JToken> item)
		{
			if (!((ICollection<KeyValuePair<string, JToken>>)this).Contains(item))
			{
				return false;
			}
			((IDictionary<string, JToken>)this).Remove(item.Key);
			return true;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00029BBB File Offset: 0x00027DBB
		internal override int GetDeepHashCode()
		{
			return base.ContentsHashCode();
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00029BC3 File Offset: 0x00027DC3
		[return: Nullable(new byte[] { 1, 0, 1, 2 })]
		public IEnumerator<KeyValuePair<string, JToken>> GetEnumerator()
		{
			foreach (JToken jtoken in this._properties)
			{
				JProperty jproperty = (JProperty)jtoken;
				yield return new KeyValuePair<string, JToken>(jproperty.Name, jproperty.Value);
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00029BD2 File Offset: 0x00027DD2
		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
			if (propertyChanged == null)
			{
				return;
			}
			propertyChanged(this, new PropertyChangedEventArgs(propertyName));
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00029BEB File Offset: 0x00027DEB
		protected virtual void OnPropertyChanging(string propertyName)
		{
			PropertyChangingEventHandler propertyChanging = this.PropertyChanging;
			if (propertyChanging == null)
			{
				return;
			}
			propertyChanging(this, new PropertyChangingEventArgs(propertyName));
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x00029C04 File Offset: 0x00027E04
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return ((ICustomTypeDescriptor)this).GetProperties(null);
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x00029C10 File Offset: 0x00027E10
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties([Nullable(new byte[] { 2, 1 })] Attribute[] attributes)
		{
			PropertyDescriptor[] array = new PropertyDescriptor[base.Count];
			int num = 0;
			foreach (KeyValuePair<string, JToken> keyValuePair in this)
			{
				array[num] = new JPropertyDescriptor(keyValuePair.Key);
				num++;
			}
			return new PropertyDescriptorCollection(array);
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x00029C78 File Offset: 0x00027E78
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return AttributeCollection.Empty;
		}

		// Token: 0x06000A57 RID: 2647 RVA: 0x00029C7F File Offset: 0x00027E7F
		[NullableContext(2)]
		string ICustomTypeDescriptor.GetClassName()
		{
			return null;
		}

		// Token: 0x06000A58 RID: 2648 RVA: 0x00029C82 File Offset: 0x00027E82
		[NullableContext(2)]
		string ICustomTypeDescriptor.GetComponentName()
		{
			return null;
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x00029C85 File Offset: 0x00027E85
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return new TypeConverter();
		}

		// Token: 0x06000A5A RID: 2650 RVA: 0x00029C8C File Offset: 0x00027E8C
		[NullableContext(2)]
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}

		// Token: 0x06000A5B RID: 2651 RVA: 0x00029C8F File Offset: 0x00027E8F
		[NullableContext(2)]
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		// Token: 0x06000A5C RID: 2652 RVA: 0x00029C92 File Offset: 0x00027E92
		[return: Nullable(2)]
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		// Token: 0x06000A5D RID: 2653 RVA: 0x00029C95 File Offset: 0x00027E95
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents([Nullable(new byte[] { 2, 1 })] Attribute[] attributes)
		{
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x06000A5E RID: 2654 RVA: 0x00029C9C File Offset: 0x00027E9C
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return EventDescriptorCollection.Empty;
		}

		// Token: 0x06000A5F RID: 2655 RVA: 0x00029CA3 File Offset: 0x00027EA3
		[NullableContext(2)]
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			if (pd is JPropertyDescriptor)
			{
				return this;
			}
			return null;
		}

		// Token: 0x06000A60 RID: 2656 RVA: 0x00029CB0 File Offset: 0x00027EB0
		protected override DynamicMetaObject GetMetaObject(Expression parameter)
		{
			return new DynamicProxyMetaObject<JObject>(parameter, this, new JObject.JObjectDynamicProxy());
		}

		// Token: 0x06000A61 RID: 2657 RVA: 0x00029CC0 File Offset: 0x00027EC0
		[CompilerGenerated]
		private async Task <WriteToAsync>g__AwaitProperties|0_0(Task task, int i, JsonWriter Writer, CancellationToken CancellationToken, JsonConverter[] Converters)
		{
			await task.ConfigureAwait(false);
			while (i < this._properties.Count)
			{
				await this._properties[i].WriteToAsync(Writer, CancellationToken, Converters).ConfigureAwait(false);
				i++;
			}
			await Writer.WriteEndObjectAsync(CancellationToken).ConfigureAwait(false);
		}

		// Token: 0x0400037F RID: 895
		private readonly JPropertyKeyedCollection _properties = new JPropertyKeyedCollection();

		// Token: 0x020001C2 RID: 450
		[Nullable(new byte[] { 0, 1 })]
		private class JObjectDynamicProxy : DynamicProxy<JObject>
		{
			// Token: 0x06000F8C RID: 3980 RVA: 0x00044446 File Offset: 0x00042646
			public override bool TryGetMember(JObject instance, GetMemberBinder binder, [Nullable(2)] out object result)
			{
				result = instance[binder.Name];
				return true;
			}

			// Token: 0x06000F8D RID: 3981 RVA: 0x00044458 File Offset: 0x00042658
			public override bool TrySetMember(JObject instance, SetMemberBinder binder, object value)
			{
				JToken jtoken = value as JToken;
				if (jtoken == null)
				{
					jtoken = new JValue(value);
				}
				instance[binder.Name] = jtoken;
				return true;
			}

			// Token: 0x06000F8E RID: 3982 RVA: 0x00044484 File Offset: 0x00042684
			public override IEnumerable<string> GetDynamicMemberNames(JObject instance)
			{
				return from p in instance.Properties()
					select p.Name;
			}
		}
	}
}
