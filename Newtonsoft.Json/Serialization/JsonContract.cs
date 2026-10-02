using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000088 RID: 136
	[NullableContext(1)]
	[Nullable(0)]
	public abstract class JsonContract
	{
		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x0001BEA3 File Offset: 0x0001A0A3
		public Type UnderlyingType { get; }

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060006A2 RID: 1698 RVA: 0x0001BEAB File Offset: 0x0001A0AB
		// (set) Token: 0x060006A3 RID: 1699 RVA: 0x0001BEB4 File Offset: 0x0001A0B4
		public Type CreatedType
		{
			get
			{
				return this._createdType;
			}
			set
			{
				ValidationUtils.ArgumentNotNull(value, "value");
				this._createdType = value;
				this.IsSealed = this._createdType.IsSealed();
				this.IsInstantiable = !this._createdType.IsInterface() && !this._createdType.IsAbstract();
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060006A4 RID: 1700 RVA: 0x0001BF08 File Offset: 0x0001A108
		// (set) Token: 0x060006A5 RID: 1701 RVA: 0x0001BF10 File Offset: 0x0001A110
		public bool? IsReference { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060006A6 RID: 1702 RVA: 0x0001BF19 File Offset: 0x0001A119
		// (set) Token: 0x060006A7 RID: 1703 RVA: 0x0001BF21 File Offset: 0x0001A121
		[Nullable(2)]
		public JsonConverter Converter
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			set;
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060006A8 RID: 1704 RVA: 0x0001BF2A File Offset: 0x0001A12A
		// (set) Token: 0x060006A9 RID: 1705 RVA: 0x0001BF32 File Offset: 0x0001A132
		[Nullable(2)]
		public JsonConverter InternalConverter
		{
			[NullableContext(2)]
			get;
			[NullableContext(2)]
			internal set;
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060006AA RID: 1706 RVA: 0x0001BF3B File Offset: 0x0001A13B
		public IList<SerializationCallback> OnDeserializedCallbacks
		{
			get
			{
				if (this._onDeserializedCallbacks == null)
				{
					this._onDeserializedCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializedCallbacks;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x0001BF56 File Offset: 0x0001A156
		public IList<SerializationCallback> OnDeserializingCallbacks
		{
			get
			{
				if (this._onDeserializingCallbacks == null)
				{
					this._onDeserializingCallbacks = new List<SerializationCallback>();
				}
				return this._onDeserializingCallbacks;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060006AC RID: 1708 RVA: 0x0001BF71 File Offset: 0x0001A171
		public IList<SerializationCallback> OnSerializedCallbacks
		{
			get
			{
				if (this._onSerializedCallbacks == null)
				{
					this._onSerializedCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializedCallbacks;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060006AD RID: 1709 RVA: 0x0001BF8C File Offset: 0x0001A18C
		public IList<SerializationCallback> OnSerializingCallbacks
		{
			get
			{
				if (this._onSerializingCallbacks == null)
				{
					this._onSerializingCallbacks = new List<SerializationCallback>();
				}
				return this._onSerializingCallbacks;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060006AE RID: 1710 RVA: 0x0001BFA7 File Offset: 0x0001A1A7
		public IList<SerializationErrorCallback> OnErrorCallbacks
		{
			get
			{
				if (this._onErrorCallbacks == null)
				{
					this._onErrorCallbacks = new List<SerializationErrorCallback>();
				}
				return this._onErrorCallbacks;
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060006AF RID: 1711 RVA: 0x0001BFC2 File Offset: 0x0001A1C2
		// (set) Token: 0x060006B0 RID: 1712 RVA: 0x0001BFCA File Offset: 0x0001A1CA
		[Nullable(new byte[] { 2, 1 })]
		public Func<object> DefaultCreator
		{
			[return: Nullable(new byte[] { 2, 1 })]
			get;
			[param: Nullable(new byte[] { 2, 1 })]
			set;
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060006B1 RID: 1713 RVA: 0x0001BFD3 File Offset: 0x0001A1D3
		// (set) Token: 0x060006B2 RID: 1714 RVA: 0x0001BFDB File Offset: 0x0001A1DB
		public bool DefaultCreatorNonPublic { get; set; }

		// Token: 0x060006B3 RID: 1715 RVA: 0x0001BFE4 File Offset: 0x0001A1E4
		internal JsonContract(Type underlyingType)
		{
			ValidationUtils.ArgumentNotNull(underlyingType, "underlyingType");
			this.UnderlyingType = underlyingType;
			underlyingType = ReflectionUtils.EnsureNotByRefType(underlyingType);
			this.IsNullable = ReflectionUtils.IsNullable(underlyingType);
			this.NonNullableUnderlyingType = ((this.IsNullable && ReflectionUtils.IsNullableType(underlyingType)) ? Nullable.GetUnderlyingType(underlyingType) : underlyingType);
			this._createdType = (this.CreatedType = this.NonNullableUnderlyingType);
			this.IsConvertable = ConvertUtils.IsConvertible(this.NonNullableUnderlyingType);
			this.IsEnum = this.NonNullableUnderlyingType.IsEnum();
			this.InternalReadType = ReadType.Read;
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0001C07C File Offset: 0x0001A27C
		internal void InvokeOnSerializing(object o, StreamingContext context)
		{
			if (this._onSerializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0001C0D8 File Offset: 0x0001A2D8
		internal void InvokeOnSerialized(object o, StreamingContext context)
		{
			if (this._onSerializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onSerializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x0001C134 File Offset: 0x0001A334
		internal void InvokeOnDeserializing(object o, StreamingContext context)
		{
			if (this._onDeserializingCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializingCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006B7 RID: 1719 RVA: 0x0001C190 File Offset: 0x0001A390
		internal void InvokeOnDeserialized(object o, StreamingContext context)
		{
			if (this._onDeserializedCallbacks != null)
			{
				foreach (SerializationCallback serializationCallback in this._onDeserializedCallbacks)
				{
					serializationCallback(o, context);
				}
			}
		}

		// Token: 0x060006B8 RID: 1720 RVA: 0x0001C1EC File Offset: 0x0001A3EC
		internal void InvokeOnError(object o, StreamingContext context, ErrorContext errorContext)
		{
			if (this._onErrorCallbacks != null)
			{
				foreach (SerializationErrorCallback serializationErrorCallback in this._onErrorCallbacks)
				{
					serializationErrorCallback(o, context, errorContext);
				}
			}
		}

		// Token: 0x060006B9 RID: 1721 RVA: 0x0001C248 File Offset: 0x0001A448
		internal static SerializationCallback CreateSerializationCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context)
			{
				callbackMethodInfo.Invoke(o, new object[] { context });
			};
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x0001C261 File Offset: 0x0001A461
		internal static SerializationErrorCallback CreateSerializationErrorCallback(MethodInfo callbackMethodInfo)
		{
			return delegate(object o, StreamingContext context, ErrorContext econtext)
			{
				callbackMethodInfo.Invoke(o, new object[] { context, econtext });
			};
		}

		// Token: 0x04000265 RID: 613
		internal bool IsNullable;

		// Token: 0x04000266 RID: 614
		internal bool IsConvertable;

		// Token: 0x04000267 RID: 615
		internal bool IsEnum;

		// Token: 0x04000268 RID: 616
		internal Type NonNullableUnderlyingType;

		// Token: 0x04000269 RID: 617
		internal ReadType InternalReadType;

		// Token: 0x0400026A RID: 618
		internal JsonContractType ContractType;

		// Token: 0x0400026B RID: 619
		internal bool IsReadOnlyOrFixedSize;

		// Token: 0x0400026C RID: 620
		internal bool IsSealed;

		// Token: 0x0400026D RID: 621
		internal bool IsInstantiable;

		// Token: 0x0400026E RID: 622
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onDeserializedCallbacks;

		// Token: 0x0400026F RID: 623
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onDeserializingCallbacks;

		// Token: 0x04000270 RID: 624
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onSerializedCallbacks;

		// Token: 0x04000271 RID: 625
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationCallback> _onSerializingCallbacks;

		// Token: 0x04000272 RID: 626
		[Nullable(new byte[] { 2, 1 })]
		private List<SerializationErrorCallback> _onErrorCallbacks;

		// Token: 0x04000273 RID: 627
		private Type _createdType;
	}
}
