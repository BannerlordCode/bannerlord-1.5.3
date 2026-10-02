using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020002B4 RID: 692
	internal class AsnReader
	{
		// Token: 0x060024B7 RID: 9399 RVA: 0x00084FE4 File Offset: 0x000831E4
		public bool TryReadPrimitiveBitString(out int unusedBitCount, out ReadOnlyMemory<byte> value, Asn1Tag? expectedTag)
		{
			ReadOnlySpan<byte> readOnlySpan;
			int num;
			bool flag = AsnDecoder.TryReadPrimitiveBitString(this._data.Span, this.RuleSet, out unusedBitCount, out readOnlySpan, out num, expectedTag);
			if (flag)
			{
				value = AsnDecoder.Slice(this._data, readOnlySpan);
				this._data = this._data.Slice(num);
			}
			else
			{
				value = default(ReadOnlyMemory<byte>);
			}
			return flag;
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x00085040 File Offset: 0x00083240
		public bool TryReadBitString(Span<byte> destination, out int unusedBitCount, out int bytesWritten, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadBitString(this._data.Span, destination, this.RuleSet, out unusedBitCount, out num, out bytesWritten, expectedTag);
			if (flag)
			{
				this._data = this._data.Slice(num);
			}
			return flag;
		}

		// Token: 0x060024B9 RID: 9401 RVA: 0x00085084 File Offset: 0x00083284
		public byte[] ReadBitString(out int unusedBitCount, Asn1Tag? expectedTag)
		{
			int num;
			byte[] array = AsnDecoder.ReadBitString(this._data.Span, this.RuleSet, out unusedBitCount, out num, expectedTag);
			this._data = this._data.Slice(num);
			return array;
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060024BA RID: 9402 RVA: 0x000850BF File Offset: 0x000832BF
		public AsnEncodingRules RuleSet
		{
			get
			{
				return this._ruleSet;
			}
		}

		// Token: 0x17000498 RID: 1176
		// (get) Token: 0x060024BB RID: 9403 RVA: 0x000850C7 File Offset: 0x000832C7
		public bool HasData
		{
			get
			{
				return !this._data.IsEmpty;
			}
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000850D7 File Offset: 0x000832D7
		public AsnReader(ReadOnlyMemory<byte> data, AsnEncodingRules ruleSet, AsnReaderOptions options)
		{
			AsnDecoder.CheckEncodingRules(ruleSet);
			this._data = data;
			this._ruleSet = ruleSet;
			this._options = options;
		}

		// Token: 0x060024BD RID: 9405 RVA: 0x000850FC File Offset: 0x000832FC
		public AsnReader(ReadOnlyMemory<byte> data, AsnEncodingRules ruleSet)
			: this(data, ruleSet, default(AsnReaderOptions))
		{
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x0008511A File Offset: 0x0008331A
		public void ThrowIfNotEmpty()
		{
			if (this.HasData)
			{
				throw new InvalidOperationException("The last expected value has been read, but the reader still has pending data. This value may be from a newer schema, or is corrupt.");
			}
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x00085130 File Offset: 0x00083330
		public Asn1Tag PeekTag()
		{
			int num;
			return Asn1Tag.Decode(this._data.Span, out num);
		}

		// Token: 0x060024C0 RID: 9408 RVA: 0x00085150 File Offset: 0x00083350
		public ReadOnlyMemory<byte> PeekEncodedValue()
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadEncodedValue(this._data.Span, this.RuleSet, out num, out num2, out num3);
			return this._data.Slice(0, num3);
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x00085188 File Offset: 0x00083388
		public ReadOnlyMemory<byte> PeekContentBytes()
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadEncodedValue(this._data.Span, this.RuleSet, out num, out num2, out num3);
			return this._data.Slice(num, num2);
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000851C0 File Offset: 0x000833C0
		public ReadOnlyMemory<byte> ReadEncodedValue()
		{
			ReadOnlyMemory<byte> readOnlyMemory = this.PeekEncodedValue();
			this._data = this._data.Slice(readOnlyMemory.Length);
			return readOnlyMemory;
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000851ED File Offset: 0x000833ED
		private AsnReader CloneAtSlice(int start, int length)
		{
			return new AsnReader(this._data.Slice(start, length), this.RuleSet, this._options);
		}

		// Token: 0x060024C4 RID: 9412 RVA: 0x00085210 File Offset: 0x00083410
		public ReadOnlyMemory<byte> ReadIntegerBytes(Asn1Tag? expectedTag)
		{
			int num;
			ReadOnlySpan<byte> readOnlySpan = AsnDecoder.ReadIntegerBytes(this._data.Span, this.RuleSet, out num, expectedTag);
			ReadOnlyMemory<byte> readOnlyMemory = AsnDecoder.Slice(this._data, readOnlySpan);
			this._data = this._data.Slice(num);
			return readOnlyMemory;
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x00085258 File Offset: 0x00083458
		public bool TryReadInt32(out int value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadInt32(this._data.Span, this.RuleSet, out value, out num, expectedTag);
			this._data = this._data.Slice(num);
			return flag;
		}

		// Token: 0x060024C6 RID: 9414 RVA: 0x00085294 File Offset: 0x00083494
		public bool TryReadUInt32(out uint value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadUInt32(this._data.Span, this.RuleSet, out value, out num, expectedTag);
			this._data = this._data.Slice(num);
			return flag;
		}

		// Token: 0x060024C7 RID: 9415 RVA: 0x000852D0 File Offset: 0x000834D0
		public bool TryReadInt64(out long value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadInt64(this._data.Span, this.RuleSet, out value, out num, expectedTag);
			this._data = this._data.Slice(num);
			return flag;
		}

		// Token: 0x060024C8 RID: 9416 RVA: 0x0008530C File Offset: 0x0008350C
		public bool TryReadUInt64(out ulong value, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadUInt64(this._data.Span, this.RuleSet, out value, out num, expectedTag);
			this._data = this._data.Slice(num);
			return flag;
		}

		// Token: 0x060024C9 RID: 9417 RVA: 0x00085348 File Offset: 0x00083548
		public void ReadNull(Asn1Tag? expectedTag)
		{
			int num;
			AsnDecoder.ReadNull(this._data.Span, this.RuleSet, out num, expectedTag);
			this._data = this._data.Slice(num);
		}

		// Token: 0x060024CA RID: 9418 RVA: 0x00085380 File Offset: 0x00083580
		public bool TryReadOctetString(Span<byte> destination, out int bytesWritten, Asn1Tag? expectedTag)
		{
			int num;
			bool flag = AsnDecoder.TryReadOctetString(this._data.Span, destination, this.RuleSet, out num, out bytesWritten, expectedTag);
			if (flag)
			{
				this._data = this._data.Slice(num);
			}
			return flag;
		}

		// Token: 0x060024CB RID: 9419 RVA: 0x000853C0 File Offset: 0x000835C0
		public byte[] ReadOctetString(Asn1Tag? expectedTag)
		{
			int num;
			byte[] array = AsnDecoder.ReadOctetString(this._data.Span, this.RuleSet, out num, expectedTag);
			this._data = this._data.Slice(num);
			return array;
		}

		// Token: 0x060024CC RID: 9420 RVA: 0x000853FC File Offset: 0x000835FC
		public bool TryReadPrimitiveOctetString(out ReadOnlyMemory<byte> contents, Asn1Tag? expectedTag)
		{
			ReadOnlySpan<byte> readOnlySpan;
			int num;
			bool flag = AsnDecoder.TryReadPrimitiveOctetString(this._data.Span, this.RuleSet, out readOnlySpan, out num, expectedTag);
			if (flag)
			{
				contents = AsnDecoder.Slice(this._data, readOnlySpan);
				this._data = this._data.Slice(num);
			}
			else
			{
				contents = default(ReadOnlyMemory<byte>);
			}
			return flag;
		}

		// Token: 0x060024CD RID: 9421 RVA: 0x00085458 File Offset: 0x00083658
		public byte[] ReadObjectIdentifier(Asn1Tag? expectedTag)
		{
			int num;
			byte[] array = AsnDecoder.ReadObjectIdentifier(this._data.Span, this.RuleSet, out num, expectedTag);
			this._data = this._data.Slice(num);
			return array;
		}

		// Token: 0x060024CE RID: 9422 RVA: 0x00085494 File Offset: 0x00083694
		public AsnReader ReadSequence(Asn1Tag? expectedTag)
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadSequence(this._data.Span, this.RuleSet, out num, out num2, out num3, expectedTag);
			AsnReader asnReader = this.CloneAtSlice(num, num2);
			this._data = this._data.Slice(num3);
			return asnReader;
		}

		// Token: 0x060024CF RID: 9423 RVA: 0x000854DC File Offset: 0x000836DC
		public AsnReader ReadSetOf(Asn1Tag? expectedTag)
		{
			return this.ReadSetOf(this._options.SkipSetSortOrderVerification, expectedTag);
		}

		// Token: 0x060024D0 RID: 9424 RVA: 0x00085500 File Offset: 0x00083700
		public AsnReader ReadSetOf(bool skipSortOrderValidation, Asn1Tag? expectedTag)
		{
			int num;
			int num2;
			int num3;
			AsnDecoder.ReadSetOf(this._data.Span, this.RuleSet, out num, out num2, out num3, skipSortOrderValidation, expectedTag);
			AsnReader asnReader = this.CloneAtSlice(num, num2);
			this._data = this._data.Slice(num3);
			return asnReader;
		}

		// Token: 0x04000DBE RID: 3518
		internal const int MaxCERSegmentSize = 1000;

		// Token: 0x04000DBF RID: 3519
		private ReadOnlyMemory<byte> _data;

		// Token: 0x04000DC0 RID: 3520
		private readonly AsnReaderOptions _options;

		// Token: 0x04000DC1 RID: 3521
		private AsnEncodingRules _ruleSet;
	}
}
