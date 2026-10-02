using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	// Token: 0x02000092 RID: 146
	[NullableContext(1)]
	[Nullable(0)]
	internal abstract class JsonSerializerInternalBase
	{
		// Token: 0x0600074E RID: 1870 RVA: 0x0001D239 File Offset: 0x0001B439
		protected JsonSerializerInternalBase(JsonSerializer serializer)
		{
			ValidationUtils.ArgumentNotNull(serializer, "serializer");
			this.Serializer = serializer;
			this.TraceWriter = serializer.TraceWriter;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001D25F File Offset: 0x0001B45F
		internal BidirectionalDictionary<string, object> DefaultReferenceMappings
		{
			get
			{
				if (this._mappings == null)
				{
					this._mappings = new BidirectionalDictionary<string, object>(EqualityComparer<string>.Default, new JsonSerializerInternalBase.ReferenceEqualsEqualityComparer(), "A different value already has the Id '{0}'.", "A different Id has already been assigned for value '{0}'. This error may be caused by an object being reused multiple times during deserialization and can be fixed with the setting ObjectCreationHandling.Replace.");
				}
				return this._mappings;
			}
		}

		// Token: 0x06000750 RID: 1872 RVA: 0x0001D290 File Offset: 0x0001B490
		protected NullValueHandling ResolvedNullValueHandling([Nullable(2)] JsonObjectContract containerContract, JsonProperty property)
		{
			NullValueHandling? nullValueHandling = property.NullValueHandling;
			if (nullValueHandling != null)
			{
				return nullValueHandling.GetValueOrDefault();
			}
			NullValueHandling? nullValueHandling2 = ((containerContract != null) ? containerContract.ItemNullValueHandling : null);
			if (nullValueHandling2 == null)
			{
				return this.Serializer._nullValueHandling;
			}
			return nullValueHandling2.GetValueOrDefault();
		}

		// Token: 0x06000751 RID: 1873 RVA: 0x0001D2E6 File Offset: 0x0001B4E6
		private ErrorContext GetErrorContext([Nullable(2)] object currentObject, [Nullable(2)] object member, string path, Exception error)
		{
			if (this._currentErrorContext == null)
			{
				this._currentErrorContext = new ErrorContext(currentObject, member, path, error);
			}
			if (this._currentErrorContext.Error != error)
			{
				throw new InvalidOperationException("Current error context error is different to requested error.");
			}
			return this._currentErrorContext;
		}

		// Token: 0x06000752 RID: 1874 RVA: 0x0001D320 File Offset: 0x0001B520
		protected void ClearErrorContext()
		{
			if (this._currentErrorContext == null)
			{
				throw new InvalidOperationException("Could not clear error context. Error context is already null.");
			}
			this._currentErrorContext = null;
		}

		// Token: 0x06000753 RID: 1875 RVA: 0x0001D33C File Offset: 0x0001B53C
		[NullableContext(2)]
		protected bool IsErrorHandled(object currentObject, JsonContract contract, object keyValue, IJsonLineInfo lineInfo, [Nullable(1)] string path, [Nullable(1)] Exception ex)
		{
			ErrorContext errorContext = this.GetErrorContext(currentObject, keyValue, path, ex);
			if (this.TraceWriter != null && this.TraceWriter.LevelFilter >= TraceLevel.Error && !errorContext.Traced)
			{
				errorContext.Traced = true;
				string text = ((base.GetType() == typeof(JsonSerializerInternalWriter)) ? "Error serializing" : "Error deserializing");
				if (contract != null)
				{
					string text2 = text;
					string text3 = " ";
					Type underlyingType = contract.UnderlyingType;
					text = text2 + text3 + ((underlyingType != null) ? underlyingType.ToString() : null);
				}
				text = text + ". " + ex.Message;
				if (!(ex is JsonException))
				{
					text = JsonPosition.FormatMessage(lineInfo, path, text);
				}
				this.TraceWriter.Trace(TraceLevel.Error, text, ex);
			}
			if (contract != null && currentObject != null)
			{
				contract.InvokeOnError(currentObject, this.Serializer.Context, errorContext);
			}
			if (!errorContext.Handled)
			{
				this.Serializer.OnError(new ErrorEventArgs(currentObject, errorContext));
			}
			return errorContext.Handled;
		}

		// Token: 0x040002C1 RID: 705
		[Nullable(2)]
		private ErrorContext _currentErrorContext;

		// Token: 0x040002C2 RID: 706
		[Nullable(new byte[] { 2, 1, 1 })]
		private BidirectionalDictionary<string, object> _mappings;

		// Token: 0x040002C3 RID: 707
		internal readonly JsonSerializer Serializer;

		// Token: 0x040002C4 RID: 708
		[Nullable(2)]
		internal readonly ITraceWriter TraceWriter;

		// Token: 0x040002C5 RID: 709
		[Nullable(2)]
		protected JsonSerializerProxy InternalSerializer;

		// Token: 0x020001A4 RID: 420
		[NullableContext(0)]
		private class ReferenceEqualsEqualityComparer : IEqualityComparer<object>
		{
			// Token: 0x06000F2A RID: 3882 RVA: 0x000429D1 File Offset: 0x00040BD1
			[NullableContext(2)]
			bool IEqualityComparer<object>.Equals(object x, object y)
			{
				return x == y;
			}

			// Token: 0x06000F2B RID: 3883 RVA: 0x000429D7 File Offset: 0x00040BD7
			[NullableContext(1)]
			int IEqualityComparer<object>.GetHashCode(object obj)
			{
				return RuntimeHelpers.GetHashCode(obj);
			}
		}
	}
}
