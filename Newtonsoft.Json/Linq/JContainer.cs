using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Linq
{
	// Token: 0x020000B9 RID: 185
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JContainer : JToken, IList<JToken>, ICollection<JToken>, IEnumerable<JToken>, IEnumerable, ITypedList, IBindingList, ICollection, IList, INotifyCollectionChanged
	{
		// Token: 0x060009AE RID: 2478 RVA: 0x00027FB8 File Offset: 0x000261B8
		internal async Task ReadTokenFromAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings options, CancellationToken cancellationToken = default(CancellationToken))
		{
			ValidationUtils.ArgumentNotNull(reader, "reader");
			int startDepth = reader.Depth;
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
				throw JsonReaderException.Create(reader, "Error reading {0} from JsonReader.".FormatWith(CultureInfo.InvariantCulture, base.GetType().Name));
			}
			await this.ReadContentFromAsync(reader, options, cancellationToken).ConfigureAwait(false);
			if (reader.Depth > startDepth)
			{
				throw JsonReaderException.Create(reader, "Unexpected end of content while loading {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType().Name));
			}
		}

		// Token: 0x060009AF RID: 2479 RVA: 0x00028014 File Offset: 0x00026214
		private async Task ReadContentFromAsync(JsonReader reader, [Nullable(2)] JsonLoadSettings settings, CancellationToken cancellationToken = default(CancellationToken))
		{
			IJsonLineInfo lineInfo = reader as IJsonLineInfo;
			JContainer parent = this;
			ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter;
			do
			{
				JProperty jproperty = parent as JProperty;
				if (jproperty != null && jproperty.Value != null)
				{
					if (parent == this)
					{
						break;
					}
					parent = parent.Parent;
				}
				switch (reader.TokenType)
				{
				case JsonToken.None:
					goto IL_0393;
				case JsonToken.StartObject:
				{
					JObject jobject = new JObject();
					jobject.SetLineInfo(lineInfo, settings);
					parent.Add(jobject);
					parent = jobject;
					goto IL_0393;
				}
				case JsonToken.StartArray:
				{
					JArray jarray = new JArray();
					jarray.SetLineInfo(lineInfo, settings);
					parent.Add(jarray);
					parent = jarray;
					goto IL_0393;
				}
				case JsonToken.StartConstructor:
				{
					JConstructor jconstructor = new JConstructor(reader.Value.ToString());
					jconstructor.SetLineInfo(lineInfo, settings);
					parent.Add(jconstructor);
					parent = jconstructor;
					goto IL_0393;
				}
				case JsonToken.PropertyName:
				{
					JProperty jproperty2 = JContainer.ReadProperty(reader, settings, lineInfo, parent);
					if (jproperty2 != null)
					{
						parent = jproperty2;
						goto IL_0393;
					}
					await reader.SkipAsync(default(CancellationToken)).ConfigureAwait(false);
					goto IL_0393;
				}
				case JsonToken.Comment:
					if (settings != null && settings.CommentHandling == CommentHandling.Load)
					{
						JValue jvalue = JValue.CreateComment(reader.Value.ToString());
						jvalue.SetLineInfo(lineInfo, settings);
						parent.Add(jvalue);
						goto IL_0393;
					}
					goto IL_0393;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					JValue jvalue = new JValue(reader.Value);
					jvalue.SetLineInfo(lineInfo, settings);
					parent.Add(jvalue);
					goto IL_0393;
				}
				case JsonToken.Null:
				{
					JValue jvalue = JValue.CreateNull();
					jvalue.SetLineInfo(lineInfo, settings);
					parent.Add(jvalue);
					goto IL_0393;
				}
				case JsonToken.Undefined:
				{
					JValue jvalue = JValue.CreateUndefined();
					jvalue.SetLineInfo(lineInfo, settings);
					parent.Add(jvalue);
					goto IL_0393;
				}
				case JsonToken.EndObject:
					if (parent == this)
					{
						goto Block_6;
					}
					parent = parent.Parent;
					goto IL_0393;
				case JsonToken.EndArray:
					if (parent == this)
					{
						goto Block_5;
					}
					parent = parent.Parent;
					goto IL_0393;
				case JsonToken.EndConstructor:
					if (parent == this)
					{
						goto Block_7;
					}
					parent = parent.Parent;
					goto IL_0393;
				}
				goto Block_4;
				IL_0393:
				configuredTaskAwaiter = reader.ReadAsync(cancellationToken).ConfigureAwait(false).GetAwaiter();
				if (!configuredTaskAwaiter.IsCompleted)
				{
					await configuredTaskAwaiter;
					ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter configuredTaskAwaiter2;
					configuredTaskAwaiter = configuredTaskAwaiter2;
					configuredTaskAwaiter2 = default(ConfiguredTaskAwaitable<bool>.ConfiguredTaskAwaiter);
				}
			}
			while (configuredTaskAwaiter.GetResult());
			return;
			Block_4:
			goto IL_036E;
			Block_5:
			Block_6:
			Block_7:
			return;
			IL_036E:
			throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.InvariantCulture, reader.TokenType));
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060009B0 RID: 2480 RVA: 0x0002806F File Offset: 0x0002626F
		// (remove) Token: 0x060009B1 RID: 2481 RVA: 0x00028088 File Offset: 0x00026288
		public event ListChangedEventHandler ListChanged
		{
			add
			{
				this._listChanged = (ListChangedEventHandler)Delegate.Combine(this._listChanged, value);
			}
			remove
			{
				this._listChanged = (ListChangedEventHandler)Delegate.Remove(this._listChanged, value);
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x060009B2 RID: 2482 RVA: 0x000280A1 File Offset: 0x000262A1
		// (remove) Token: 0x060009B3 RID: 2483 RVA: 0x000280BA File Offset: 0x000262BA
		public event AddingNewEventHandler AddingNew
		{
			add
			{
				this._addingNew = (AddingNewEventHandler)Delegate.Combine(this._addingNew, value);
			}
			remove
			{
				this._addingNew = (AddingNewEventHandler)Delegate.Remove(this._addingNew, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x060009B4 RID: 2484 RVA: 0x000280D3 File Offset: 0x000262D3
		// (remove) Token: 0x060009B5 RID: 2485 RVA: 0x000280EC File Offset: 0x000262EC
		[Nullable(2)]
		public event NotifyCollectionChangedEventHandler CollectionChanged
		{
			[NullableContext(2)]
			add
			{
				this._collectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Combine(this._collectionChanged, value);
			}
			[NullableContext(2)]
			remove
			{
				this._collectionChanged = (NotifyCollectionChangedEventHandler)Delegate.Remove(this._collectionChanged, value);
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060009B6 RID: 2486
		protected abstract IList<JToken> ChildrenTokens { get; }

		// Token: 0x060009B7 RID: 2487 RVA: 0x00028105 File Offset: 0x00026305
		internal JContainer()
		{
		}

		// Token: 0x060009B8 RID: 2488 RVA: 0x00028110 File Offset: 0x00026310
		internal JContainer(JContainer other, [Nullable(2)] JsonCloneSettings settings)
			: this()
		{
			ValidationUtils.ArgumentNotNull(other, "other");
			bool flag = settings == null || settings.CopyAnnotations;
			if (flag)
			{
				base.CopyAnnotations(this, other);
			}
			int num = 0;
			foreach (JToken jtoken in ((IEnumerable<JToken>)other))
			{
				this.TryAddInternal(num, jtoken, false, flag);
				num++;
			}
		}

		// Token: 0x060009B9 RID: 2489 RVA: 0x0002818C File Offset: 0x0002638C
		internal void CheckReentrancy()
		{
			if (this._busy)
			{
				throw new InvalidOperationException("Cannot change {0} during a collection change event.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
		}

		// Token: 0x060009BA RID: 2490 RVA: 0x000281B1 File Offset: 0x000263B1
		internal virtual IList<JToken> CreateChildrenCollection()
		{
			return new List<JToken>();
		}

		// Token: 0x060009BB RID: 2491 RVA: 0x000281B8 File Offset: 0x000263B8
		protected virtual void OnAddingNew(AddingNewEventArgs e)
		{
			AddingNewEventHandler addingNew = this._addingNew;
			if (addingNew == null)
			{
				return;
			}
			addingNew(this, e);
		}

		// Token: 0x060009BC RID: 2492 RVA: 0x000281CC File Offset: 0x000263CC
		protected virtual void OnListChanged(ListChangedEventArgs e)
		{
			ListChangedEventHandler listChanged = this._listChanged;
			if (listChanged != null)
			{
				this._busy = true;
				try
				{
					listChanged(this, e);
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x060009BD RID: 2493 RVA: 0x0002820C File Offset: 0x0002640C
		protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			NotifyCollectionChangedEventHandler collectionChanged = this._collectionChanged;
			if (collectionChanged != null)
			{
				this._busy = true;
				try
				{
					collectionChanged(this, e);
				}
				finally
				{
					this._busy = false;
				}
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060009BE RID: 2494 RVA: 0x0002824C File Offset: 0x0002644C
		public override bool HasValues
		{
			get
			{
				return this.ChildrenTokens.Count > 0;
			}
		}

		// Token: 0x060009BF RID: 2495 RVA: 0x0002825C File Offset: 0x0002645C
		internal bool ContentsEqual(JContainer container)
		{
			if (container == this)
			{
				return true;
			}
			IList<JToken> childrenTokens = this.ChildrenTokens;
			IList<JToken> childrenTokens2 = container.ChildrenTokens;
			if (childrenTokens.Count != childrenTokens2.Count)
			{
				return false;
			}
			for (int i = 0; i < childrenTokens.Count; i++)
			{
				if (!childrenTokens[i].DeepEquals(childrenTokens2[i]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060009C0 RID: 2496 RVA: 0x000282B8 File Offset: 0x000264B8
		[Nullable(2)]
		public override JToken First
		{
			[NullableContext(2)]
			get
			{
				IList<JToken> childrenTokens = this.ChildrenTokens;
				if (childrenTokens.Count <= 0)
				{
					return null;
				}
				return childrenTokens[0];
			}
		}

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x060009C1 RID: 2497 RVA: 0x000282E0 File Offset: 0x000264E0
		[Nullable(2)]
		public override JToken Last
		{
			[NullableContext(2)]
			get
			{
				IList<JToken> childrenTokens = this.ChildrenTokens;
				int count = childrenTokens.Count;
				if (count <= 0)
				{
					return null;
				}
				return childrenTokens[count - 1];
			}
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x0002830A File Offset: 0x0002650A
		[return: Nullable(new byte[] { 0, 1 })]
		public override JEnumerable<JToken> Children()
		{
			return new JEnumerable<JToken>(this.ChildrenTokens);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x00028317 File Offset: 0x00026517
		[NullableContext(2)]
		[return: Nullable(new byte[] { 1, 2 })]
		public override IEnumerable<T> Values<T>()
		{
			return this.ChildrenTokens.Convert<JToken, T>();
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x00028324 File Offset: 0x00026524
		public IEnumerable<JToken> Descendants()
		{
			return this.GetDescendants(false);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x0002832D File Offset: 0x0002652D
		public IEnumerable<JToken> DescendantsAndSelf()
		{
			return this.GetDescendants(true);
		}

		// Token: 0x060009C6 RID: 2502 RVA: 0x00028336 File Offset: 0x00026536
		internal IEnumerable<JToken> GetDescendants(bool self)
		{
			if (self)
			{
				yield return this;
			}
			foreach (JToken o in this.ChildrenTokens)
			{
				yield return o;
				JContainer jcontainer = o as JContainer;
				if (jcontainer != null)
				{
					foreach (JToken jtoken in jcontainer.Descendants())
					{
						yield return jtoken;
					}
					IEnumerator<JToken> enumerator2 = null;
				}
				o = null;
			}
			IEnumerator<JToken> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060009C7 RID: 2503 RVA: 0x0002834D File Offset: 0x0002654D
		[NullableContext(2)]
		internal bool IsMultiContent([NotNullWhen(true)] object content)
		{
			return content is IEnumerable && !(content is string) && !(content is JToken) && !(content is byte[]);
		}

		// Token: 0x060009C8 RID: 2504 RVA: 0x00028378 File Offset: 0x00026578
		internal JToken EnsureParentToken([Nullable(2)] JToken item, bool skipParentCheck, bool copyAnnotations)
		{
			if (item == null)
			{
				return JValue.CreateNull();
			}
			if (skipParentCheck)
			{
				return item;
			}
			if (item.Parent != null || item == this || (item.HasValues && base.Root == item))
			{
				JsonCloneSettings jsonCloneSettings = (copyAnnotations ? null : JsonCloneSettings.SkipCopyAnnotations);
				item = item.CloneToken(jsonCloneSettings);
			}
			return item;
		}

		// Token: 0x060009C9 RID: 2505
		[NullableContext(2)]
		internal abstract int IndexOfItem(JToken item);

		// Token: 0x060009CA RID: 2506 RVA: 0x000283C8 File Offset: 0x000265C8
		[NullableContext(2)]
		internal virtual bool InsertItem(int index, JToken item, bool skipParentCheck, bool copyAnnotations)
		{
			IList<JToken> childrenTokens = this.ChildrenTokens;
			if (index > childrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index must be within the bounds of the List.");
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item, skipParentCheck, copyAnnotations);
			JToken jtoken = ((index == 0) ? null : childrenTokens[index - 1]);
			JToken jtoken2 = ((index == childrenTokens.Count) ? null : childrenTokens[index]);
			this.ValidateToken(item, null);
			item.Parent = this;
			item.Previous = jtoken;
			if (jtoken != null)
			{
				jtoken.Next = item;
			}
			item.Next = jtoken2;
			if (jtoken2 != null)
			{
				jtoken2.Previous = item;
			}
			childrenTokens.Insert(index, item);
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemAdded, index));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
			}
			return true;
		}

		// Token: 0x060009CB RID: 2507 RVA: 0x00028490 File Offset: 0x00026690
		internal virtual void RemoveItemAt(int index)
		{
			IList<JToken> childrenTokens = this.ChildrenTokens;
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= childrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			this.CheckReentrancy();
			JToken jtoken = childrenTokens[index];
			JToken jtoken2 = ((index == 0) ? null : childrenTokens[index - 1]);
			JToken jtoken3 = ((index == childrenTokens.Count - 1) ? null : childrenTokens[index + 1]);
			if (jtoken2 != null)
			{
				jtoken2.Next = jtoken3;
			}
			if (jtoken3 != null)
			{
				jtoken3.Previous = jtoken2;
			}
			jtoken.Parent = null;
			jtoken.Previous = null;
			jtoken.Next = null;
			childrenTokens.RemoveAt(index);
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemDeleted, index));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, jtoken, index));
			}
		}

		// Token: 0x060009CC RID: 2508 RVA: 0x00028564 File Offset: 0x00026764
		[NullableContext(2)]
		internal virtual bool RemoveItem(JToken item)
		{
			if (item != null)
			{
				int num = this.IndexOfItem(item);
				if (num >= 0)
				{
					this.RemoveItemAt(num);
					return true;
				}
			}
			return false;
		}

		// Token: 0x060009CD RID: 2509 RVA: 0x0002858A File Offset: 0x0002678A
		internal virtual JToken GetItem(int index)
		{
			return this.ChildrenTokens[index];
		}

		// Token: 0x060009CE RID: 2510 RVA: 0x00028598 File Offset: 0x00026798
		[NullableContext(2)]
		internal virtual void SetItem(int index, JToken item)
		{
			IList<JToken> childrenTokens = this.ChildrenTokens;
			if (index < 0)
			{
				throw new ArgumentOutOfRangeException("index", "Index is less than 0.");
			}
			if (index >= childrenTokens.Count)
			{
				throw new ArgumentOutOfRangeException("index", "Index is equal to or greater than Count.");
			}
			JToken jtoken = childrenTokens[index];
			if (JContainer.IsTokenUnchanged(jtoken, item))
			{
				return;
			}
			this.CheckReentrancy();
			item = this.EnsureParentToken(item, false, true);
			this.ValidateToken(item, jtoken);
			JToken jtoken2 = ((index == 0) ? null : childrenTokens[index - 1]);
			JToken jtoken3 = ((index == childrenTokens.Count - 1) ? null : childrenTokens[index + 1]);
			item.Parent = this;
			item.Previous = jtoken2;
			if (jtoken2 != null)
			{
				jtoken2.Next = item;
			}
			item.Next = jtoken3;
			if (jtoken3 != null)
			{
				jtoken3.Previous = item;
			}
			childrenTokens[index] = item;
			jtoken.Parent = null;
			jtoken.Previous = null;
			jtoken.Next = null;
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.ItemChanged, index));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, item, jtoken, index));
			}
		}

		// Token: 0x060009CF RID: 2511 RVA: 0x000286A0 File Offset: 0x000268A0
		internal virtual void ClearItems()
		{
			this.CheckReentrancy();
			IList<JToken> childrenTokens = this.ChildrenTokens;
			foreach (JToken jtoken in childrenTokens)
			{
				jtoken.Parent = null;
				jtoken.Previous = null;
				jtoken.Next = null;
			}
			childrenTokens.Clear();
			if (this._listChanged != null)
			{
				this.OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
			}
			if (this._collectionChanged != null)
			{
				this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}
		}

		// Token: 0x060009D0 RID: 2512 RVA: 0x00028730 File Offset: 0x00026930
		internal virtual void ReplaceItem(JToken existing, JToken replacement)
		{
			if (existing == null || existing.Parent != this)
			{
				return;
			}
			int num = this.IndexOfItem(existing);
			this.SetItem(num, replacement);
		}

		// Token: 0x060009D1 RID: 2513 RVA: 0x0002875A File Offset: 0x0002695A
		[NullableContext(2)]
		internal virtual bool ContainsItem(JToken item)
		{
			return this.IndexOfItem(item) != -1;
		}

		// Token: 0x060009D2 RID: 2514 RVA: 0x0002876C File Offset: 0x0002696C
		internal virtual void CopyItemsTo(Array array, int arrayIndex)
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
			if (this.Count > array.Length - arrayIndex)
			{
				throw new ArgumentException("The number of elements in the source JObject is greater than the available space from arrayIndex to the end of the destination array.");
			}
			int num = 0;
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				array.SetValue(jtoken, arrayIndex + num);
				num++;
			}
		}

		// Token: 0x060009D3 RID: 2515 RVA: 0x00028818 File Offset: 0x00026A18
		internal static bool IsTokenUnchanged(JToken currentValue, [Nullable(2)] JToken newValue)
		{
			JValue jvalue = currentValue as JValue;
			if (jvalue == null)
			{
				return false;
			}
			if (newValue == null)
			{
				return jvalue.Type == JTokenType.Null;
			}
			return jvalue.Equals(newValue);
		}

		// Token: 0x060009D4 RID: 2516 RVA: 0x00028846 File Offset: 0x00026A46
		internal virtual void ValidateToken(JToken o, [Nullable(2)] JToken existing)
		{
			ValidationUtils.ArgumentNotNull(o, "o");
			if (o.Type == JTokenType.Property)
			{
				throw new ArgumentException("Can not add {0} to {1}.".FormatWith(CultureInfo.InvariantCulture, o.GetType(), base.GetType()));
			}
		}

		// Token: 0x060009D5 RID: 2517 RVA: 0x0002887D File Offset: 0x00026A7D
		[NullableContext(2)]
		public virtual void Add(object content)
		{
			this.TryAddInternal(this.ChildrenTokens.Count, content, false, true);
		}

		// Token: 0x060009D6 RID: 2518 RVA: 0x00028894 File Offset: 0x00026A94
		[NullableContext(2)]
		internal bool TryAdd(object content)
		{
			return this.TryAddInternal(this.ChildrenTokens.Count, content, false, true);
		}

		// Token: 0x060009D7 RID: 2519 RVA: 0x000288AA File Offset: 0x00026AAA
		internal void AddAndSkipParentCheck(JToken token)
		{
			this.TryAddInternal(this.ChildrenTokens.Count, token, true, true);
		}

		// Token: 0x060009D8 RID: 2520 RVA: 0x000288C1 File Offset: 0x00026AC1
		[NullableContext(2)]
		public void AddFirst(object content)
		{
			this.TryAddInternal(0, content, false, true);
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x000288D0 File Offset: 0x00026AD0
		[NullableContext(2)]
		internal bool TryAddInternal(int index, object content, bool skipParentCheck, bool copyAnnotations)
		{
			if (this.IsMultiContent(content))
			{
				IEnumerable enumerable = (IEnumerable)content;
				int num = index;
				foreach (object obj in enumerable)
				{
					this.TryAddInternal(num, obj, skipParentCheck, copyAnnotations);
					num++;
				}
				return true;
			}
			JToken jtoken = JContainer.CreateFromContent(content);
			return this.InsertItem(index, jtoken, skipParentCheck, copyAnnotations);
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x00028950 File Offset: 0x00026B50
		internal static JToken CreateFromContent([Nullable(2)] object content)
		{
			JToken jtoken = content as JToken;
			if (jtoken != null)
			{
				return jtoken;
			}
			return new JValue(content);
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0002896F File Offset: 0x00026B6F
		public JsonWriter CreateWriter()
		{
			return new JTokenWriter(this);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x00028977 File Offset: 0x00026B77
		public void ReplaceAll(object content)
		{
			this.ClearItems();
			this.Add(content);
		}

		// Token: 0x060009DD RID: 2525 RVA: 0x00028986 File Offset: 0x00026B86
		public void RemoveAll()
		{
			this.ClearItems();
		}

		// Token: 0x060009DE RID: 2526
		internal abstract void MergeItem(object content, [Nullable(2)] JsonMergeSettings settings);

		// Token: 0x060009DF RID: 2527 RVA: 0x0002898E File Offset: 0x00026B8E
		[NullableContext(2)]
		public void Merge(object content)
		{
			if (content == null)
			{
				return;
			}
			this.ValidateContent(content);
			this.MergeItem(content, null);
		}

		// Token: 0x060009E0 RID: 2528 RVA: 0x000289A3 File Offset: 0x00026BA3
		[NullableContext(2)]
		public void Merge(object content, JsonMergeSettings settings)
		{
			if (content == null)
			{
				return;
			}
			this.ValidateContent(content);
			this.MergeItem(content, settings);
		}

		// Token: 0x060009E1 RID: 2529 RVA: 0x000289B8 File Offset: 0x00026BB8
		private void ValidateContent(object content)
		{
			if (content.GetType().IsSubclassOf(typeof(JToken)))
			{
				return;
			}
			if (this.IsMultiContent(content))
			{
				return;
			}
			throw new ArgumentException("Could not determine JSON object type for type {0}.".FormatWith(CultureInfo.InvariantCulture, content.GetType()), "content");
		}

		// Token: 0x060009E2 RID: 2530 RVA: 0x00028A08 File Offset: 0x00026C08
		internal void ReadTokenFrom(JsonReader reader, [Nullable(2)] JsonLoadSettings options)
		{
			int depth = reader.Depth;
			if (!reader.Read())
			{
				throw JsonReaderException.Create(reader, "Error reading {0} from JsonReader.".FormatWith(CultureInfo.InvariantCulture, base.GetType().Name));
			}
			this.ReadContentFrom(reader, options);
			if (reader.Depth > depth)
			{
				throw JsonReaderException.Create(reader, "Unexpected end of content while loading {0}.".FormatWith(CultureInfo.InvariantCulture, base.GetType().Name));
			}
		}

		// Token: 0x060009E3 RID: 2531 RVA: 0x00028A78 File Offset: 0x00026C78
		internal void ReadContentFrom(JsonReader r, [Nullable(2)] JsonLoadSettings settings)
		{
			ValidationUtils.ArgumentNotNull(r, "r");
			IJsonLineInfo jsonLineInfo = r as IJsonLineInfo;
			JContainer jcontainer = this;
			for (;;)
			{
				JProperty jproperty = jcontainer as JProperty;
				if (jproperty != null && jproperty.Value != null)
				{
					if (jcontainer == this)
					{
						break;
					}
					jcontainer = jcontainer.Parent;
				}
				switch (r.TokenType)
				{
				case JsonToken.None:
					goto IL_01F2;
				case JsonToken.StartObject:
				{
					JObject jobject = new JObject();
					jobject.SetLineInfo(jsonLineInfo, settings);
					jcontainer.Add(jobject);
					jcontainer = jobject;
					goto IL_01F2;
				}
				case JsonToken.StartArray:
				{
					JArray jarray = new JArray();
					jarray.SetLineInfo(jsonLineInfo, settings);
					jcontainer.Add(jarray);
					jcontainer = jarray;
					goto IL_01F2;
				}
				case JsonToken.StartConstructor:
				{
					JConstructor jconstructor = new JConstructor(r.Value.ToString());
					jconstructor.SetLineInfo(jsonLineInfo, settings);
					jcontainer.Add(jconstructor);
					jcontainer = jconstructor;
					goto IL_01F2;
				}
				case JsonToken.PropertyName:
				{
					JProperty jproperty2 = JContainer.ReadProperty(r, settings, jsonLineInfo, jcontainer);
					if (jproperty2 != null)
					{
						jcontainer = jproperty2;
						goto IL_01F2;
					}
					r.Skip();
					goto IL_01F2;
				}
				case JsonToken.Comment:
					if (settings != null && settings.CommentHandling == CommentHandling.Load)
					{
						JValue jvalue = JValue.CreateComment(r.Value.ToString());
						jvalue.SetLineInfo(jsonLineInfo, settings);
						jcontainer.Add(jvalue);
						goto IL_01F2;
					}
					goto IL_01F2;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Date:
				case JsonToken.Bytes:
				{
					JValue jvalue = new JValue(r.Value);
					jvalue.SetLineInfo(jsonLineInfo, settings);
					jcontainer.Add(jvalue);
					goto IL_01F2;
				}
				case JsonToken.Null:
				{
					JValue jvalue = JValue.CreateNull();
					jvalue.SetLineInfo(jsonLineInfo, settings);
					jcontainer.Add(jvalue);
					goto IL_01F2;
				}
				case JsonToken.Undefined:
				{
					JValue jvalue = JValue.CreateUndefined();
					jvalue.SetLineInfo(jsonLineInfo, settings);
					jcontainer.Add(jvalue);
					goto IL_01F2;
				}
				case JsonToken.EndObject:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_01F2;
				case JsonToken.EndArray:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_01F2;
				case JsonToken.EndConstructor:
					if (jcontainer == this)
					{
						return;
					}
					jcontainer = jcontainer.Parent;
					goto IL_01F2;
				}
				goto Block_4;
				IL_01F2:
				if (!r.Read())
				{
					return;
				}
			}
			return;
			Block_4:
			throw new InvalidOperationException("The JsonReader should not be on a token of type {0}.".FormatWith(CultureInfo.InvariantCulture, r.TokenType));
		}

		// Token: 0x060009E4 RID: 2532 RVA: 0x00028C84 File Offset: 0x00026E84
		[NullableContext(2)]
		private static JProperty ReadProperty([Nullable(1)] JsonReader r, JsonLoadSettings settings, IJsonLineInfo lineInfo, [Nullable(1)] JContainer parent)
		{
			DuplicatePropertyNameHandling duplicatePropertyNameHandling = ((settings != null) ? settings.DuplicatePropertyNameHandling : DuplicatePropertyNameHandling.Replace);
			JObject jobject = (JObject)parent;
			string text = r.Value.ToString();
			JProperty jproperty = jobject.Property(text, StringComparison.Ordinal);
			if (jproperty != null)
			{
				if (duplicatePropertyNameHandling == DuplicatePropertyNameHandling.Ignore)
				{
					return null;
				}
				if (duplicatePropertyNameHandling == DuplicatePropertyNameHandling.Error)
				{
					throw JsonReaderException.Create(r, "Property with the name '{0}' already exists in the current JSON object.".FormatWith(CultureInfo.InvariantCulture, text));
				}
			}
			JProperty jproperty2 = new JProperty(text);
			jproperty2.SetLineInfo(lineInfo, settings);
			if (jproperty == null)
			{
				parent.Add(jproperty2);
			}
			else
			{
				jproperty.Replace(jproperty2);
			}
			return jproperty2;
		}

		// Token: 0x060009E5 RID: 2533 RVA: 0x00028D00 File Offset: 0x00026F00
		internal int ContentsHashCode()
		{
			int num = 0;
			foreach (JToken jtoken in this.ChildrenTokens)
			{
				num ^= jtoken.GetDeepHashCode();
			}
			return num;
		}

		// Token: 0x060009E6 RID: 2534 RVA: 0x00028D54 File Offset: 0x00026F54
		string ITypedList.GetListName(PropertyDescriptor[] listAccessors)
		{
			return string.Empty;
		}

		// Token: 0x060009E7 RID: 2535 RVA: 0x00028D5B File Offset: 0x00026F5B
		PropertyDescriptorCollection ITypedList.GetItemProperties(PropertyDescriptor[] listAccessors)
		{
			ICustomTypeDescriptor customTypeDescriptor = this.First as ICustomTypeDescriptor;
			return ((customTypeDescriptor != null) ? customTypeDescriptor.GetProperties() : null) ?? new PropertyDescriptorCollection(CollectionUtils.ArrayEmpty<PropertyDescriptor>());
		}

		// Token: 0x060009E8 RID: 2536 RVA: 0x00028D82 File Offset: 0x00026F82
		int IList<JToken>.IndexOf(JToken item)
		{
			return this.IndexOfItem(item);
		}

		// Token: 0x060009E9 RID: 2537 RVA: 0x00028D8B File Offset: 0x00026F8B
		void IList<JToken>.Insert(int index, JToken item)
		{
			this.InsertItem(index, item, false, true);
		}

		// Token: 0x060009EA RID: 2538 RVA: 0x00028D98 File Offset: 0x00026F98
		void IList<JToken>.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x170001C7 RID: 455
		JToken IList<JToken>.this[int index]
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

		// Token: 0x060009ED RID: 2541 RVA: 0x00028DB4 File Offset: 0x00026FB4
		void ICollection<JToken>.Add(JToken item)
		{
			this.Add(item);
		}

		// Token: 0x060009EE RID: 2542 RVA: 0x00028DBD File Offset: 0x00026FBD
		void ICollection<JToken>.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x060009EF RID: 2543 RVA: 0x00028DC5 File Offset: 0x00026FC5
		bool ICollection<JToken>.Contains(JToken item)
		{
			return this.ContainsItem(item);
		}

		// Token: 0x060009F0 RID: 2544 RVA: 0x00028DCE File Offset: 0x00026FCE
		void ICollection<JToken>.CopyTo(JToken[] array, int arrayIndex)
		{
			this.CopyItemsTo(array, arrayIndex);
		}

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x060009F1 RID: 2545 RVA: 0x00028DD8 File Offset: 0x00026FD8
		bool ICollection<JToken>.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060009F2 RID: 2546 RVA: 0x00028DDB File Offset: 0x00026FDB
		bool ICollection<JToken>.Remove(JToken item)
		{
			return this.RemoveItem(item);
		}

		// Token: 0x060009F3 RID: 2547 RVA: 0x00028DE4 File Offset: 0x00026FE4
		[NullableContext(2)]
		private JToken EnsureValue(object value)
		{
			if (value == null)
			{
				return null;
			}
			JToken jtoken = value as JToken;
			if (jtoken != null)
			{
				return jtoken;
			}
			throw new ArgumentException("Argument is not a JToken.");
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00028E0C File Offset: 0x0002700C
		[NullableContext(2)]
		int IList.Add(object value)
		{
			this.Add(this.EnsureValue(value));
			return this.Count - 1;
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00028E23 File Offset: 0x00027023
		void IList.Clear()
		{
			this.ClearItems();
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00028E2B File Offset: 0x0002702B
		[NullableContext(2)]
		bool IList.Contains(object value)
		{
			return this.ContainsItem(this.EnsureValue(value));
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00028E3A File Offset: 0x0002703A
		[NullableContext(2)]
		int IList.IndexOf(object value)
		{
			return this.IndexOfItem(this.EnsureValue(value));
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x00028E49 File Offset: 0x00027049
		[NullableContext(2)]
		void IList.Insert(int index, object value)
		{
			this.InsertItem(index, this.EnsureValue(value), false, false);
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x060009F9 RID: 2553 RVA: 0x00028E5C File Offset: 0x0002705C
		bool IList.IsFixedSize
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x060009FA RID: 2554 RVA: 0x00028E5F File Offset: 0x0002705F
		bool IList.IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x00028E62 File Offset: 0x00027062
		[NullableContext(2)]
		void IList.Remove(object value)
		{
			this.RemoveItem(this.EnsureValue(value));
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00028E72 File Offset: 0x00027072
		void IList.RemoveAt(int index)
		{
			this.RemoveItemAt(index);
		}

		// Token: 0x170001CB RID: 459
		[Nullable(2)]
		object IList.this[int index]
		{
			[NullableContext(2)]
			get
			{
				return this.GetItem(index);
			}
			[NullableContext(2)]
			set
			{
				this.SetItem(index, this.EnsureValue(value));
			}
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00028E94 File Offset: 0x00027094
		void ICollection.CopyTo(Array array, int index)
		{
			this.CopyItemsTo(array, index);
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x06000A00 RID: 2560 RVA: 0x00028E9E File Offset: 0x0002709E
		public int Count
		{
			get
			{
				return this.ChildrenTokens.Count;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x06000A01 RID: 2561 RVA: 0x00028EAB File Offset: 0x000270AB
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000A02 RID: 2562 RVA: 0x00028EAE File Offset: 0x000270AE
		object ICollection.SyncRoot
		{
			get
			{
				if (this._syncRoot == null)
				{
					Interlocked.CompareExchange(ref this._syncRoot, new object(), null);
				}
				return this._syncRoot;
			}
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00028ED0 File Offset: 0x000270D0
		void IBindingList.AddIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x00028ED4 File Offset: 0x000270D4
		object IBindingList.AddNew()
		{
			AddingNewEventArgs e = new AddingNewEventArgs();
			this.OnAddingNew(e);
			if (e.NewObject == null)
			{
				throw new JsonException("Could not determine new value to add to '{0}'.".FormatWith(CultureInfo.InvariantCulture, base.GetType()));
			}
			JToken jtoken = e.NewObject as JToken;
			if (jtoken == null)
			{
				throw new JsonException("New item to be added to collection must be compatible with {0}.".FormatWith(CultureInfo.InvariantCulture, typeof(JToken)));
			}
			this.Add(jtoken);
			return jtoken;
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000A05 RID: 2565 RVA: 0x00028F47 File Offset: 0x00027147
		bool IBindingList.AllowEdit
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x06000A06 RID: 2566 RVA: 0x00028F4A File Offset: 0x0002714A
		bool IBindingList.AllowNew
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x00028F4D File Offset: 0x0002714D
		bool IBindingList.AllowRemove
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00028F50 File Offset: 0x00027150
		void IBindingList.ApplySort(PropertyDescriptor property, ListSortDirection direction)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00028F57 File Offset: 0x00027157
		int IBindingList.Find(PropertyDescriptor property, object key)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000A0A RID: 2570 RVA: 0x00028F5E File Offset: 0x0002715E
		bool IBindingList.IsSorted
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x00028F61 File Offset: 0x00027161
		void IBindingList.RemoveIndex(PropertyDescriptor property)
		{
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00028F63 File Offset: 0x00027163
		void IBindingList.RemoveSort()
		{
			throw new NotSupportedException();
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000A0D RID: 2573 RVA: 0x00028F6A File Offset: 0x0002716A
		ListSortDirection IBindingList.SortDirection
		{
			get
			{
				return ListSortDirection.Ascending;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000A0E RID: 2574 RVA: 0x00028F6D File Offset: 0x0002716D
		[Nullable(2)]
		PropertyDescriptor IBindingList.SortProperty
		{
			[NullableContext(2)]
			get
			{
				return null;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000A0F RID: 2575 RVA: 0x00028F70 File Offset: 0x00027170
		bool IBindingList.SupportsChangeNotification
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000A10 RID: 2576 RVA: 0x00028F73 File Offset: 0x00027173
		bool IBindingList.SupportsSearching
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x00028F76 File Offset: 0x00027176
		bool IBindingList.SupportsSorting
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000A12 RID: 2578 RVA: 0x00028F7C File Offset: 0x0002717C
		internal static void MergeEnumerableContent(JContainer target, IEnumerable content, [Nullable(2)] JsonMergeSettings settings)
		{
			switch ((settings != null) ? settings.MergeArrayHandling : MergeArrayHandling.Concat)
			{
			case MergeArrayHandling.Concat:
			{
				using (IEnumerator enumerator = content.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						target.Add(JContainer.CreateFromContent(obj));
					}
					return;
				}
				break;
			}
			case MergeArrayHandling.Union:
				break;
			case MergeArrayHandling.Replace:
				goto IL_00BC;
			case MergeArrayHandling.Merge:
				goto IL_0108;
			default:
				goto IL_019E;
			}
			HashSet<JToken> hashSet = new HashSet<JToken>(target, JToken.EqualityComparer);
			using (IEnumerator enumerator = content.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj2 = enumerator.Current;
					JToken jtoken = JContainer.CreateFromContent(obj2);
					if (hashSet.Add(jtoken))
					{
						target.Add(jtoken);
					}
				}
				return;
			}
			IL_00BC:
			if (target == content)
			{
				return;
			}
			target.ClearItems();
			using (IEnumerator enumerator = content.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj3 = enumerator.Current;
					target.Add(JContainer.CreateFromContent(obj3));
				}
				return;
			}
			IL_0108:
			int num = 0;
			using (IEnumerator enumerator = content.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object obj4 = enumerator.Current;
					if (num < target.Count)
					{
						JContainer jcontainer = target[num] as JContainer;
						if (jcontainer != null)
						{
							jcontainer.Merge(obj4, settings);
						}
						else if (obj4 != null)
						{
							JToken jtoken2 = JContainer.CreateFromContent(obj4);
							if (jtoken2.Type != JTokenType.Null)
							{
								target[num] = jtoken2;
							}
						}
					}
					else
					{
						target.Add(JContainer.CreateFromContent(obj4));
					}
					num++;
				}
				return;
			}
			IL_019E:
			throw new ArgumentOutOfRangeException("settings", "Unexpected merge array handling when merging JSON.");
		}

		// Token: 0x04000378 RID: 888
		[Nullable(2)]
		internal ListChangedEventHandler _listChanged;

		// Token: 0x04000379 RID: 889
		[Nullable(2)]
		internal AddingNewEventHandler _addingNew;

		// Token: 0x0400037A RID: 890
		[Nullable(2)]
		internal NotifyCollectionChangedEventHandler _collectionChanged;

		// Token: 0x0400037B RID: 891
		[Nullable(2)]
		private object _syncRoot;

		// Token: 0x0400037C RID: 892
		private bool _busy;
	}
}
