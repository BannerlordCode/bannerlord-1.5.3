using System;
using System.Collections.Generic;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020002C2 RID: 706
	internal struct AttributeAsn
	{
		// Token: 0x0600251C RID: 9500 RVA: 0x000870EE File Offset: 0x000852EE
		internal static AttributeAsn Decode(ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			return AttributeAsn.Decode(Asn1Tag.Sequence, encoded, ruleSet);
		}

		// Token: 0x0600251D RID: 9501 RVA: 0x000870FC File Offset: 0x000852FC
		internal static AttributeAsn Decode(Asn1Tag expectedTag, ReadOnlyMemory<byte> encoded, AsnEncodingRules ruleSet)
		{
			AttributeAsn attributeAsn2;
			try
			{
				AsnValueReader asnValueReader = new AsnValueReader(encoded.Span, ruleSet);
				AttributeAsn attributeAsn;
				AttributeAsn.DecodeCore(ref asnValueReader, expectedTag, encoded, out attributeAsn);
				asnValueReader.ThrowIfNotEmpty();
				attributeAsn2 = attributeAsn;
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
			return attributeAsn2;
		}

		// Token: 0x0600251E RID: 9502 RVA: 0x0008714C File Offset: 0x0008534C
		internal static void Decode(ref AsnValueReader reader, ReadOnlyMemory<byte> rebind, out AttributeAsn decoded)
		{
			AttributeAsn.Decode(ref reader, Asn1Tag.Sequence, rebind, out decoded);
		}

		// Token: 0x0600251F RID: 9503 RVA: 0x0008715C File Offset: 0x0008535C
		internal static void Decode(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out AttributeAsn decoded)
		{
			try
			{
				AttributeAsn.DecodeCore(ref reader, expectedTag, rebind, out decoded);
			}
			catch (InvalidOperationException ex)
			{
				throw new CryptographicException("ASN1 corrupted data.", ex);
			}
		}

		// Token: 0x06002520 RID: 9504 RVA: 0x00087194 File Offset: 0x00085394
		private static void DecodeCore(ref AsnValueReader reader, Asn1Tag expectedTag, ReadOnlyMemory<byte> rebind, out AttributeAsn decoded)
		{
			decoded = default(AttributeAsn);
			AsnValueReader asnValueReader = reader.ReadSequence(new Asn1Tag?(expectedTag));
			ReadOnlySpan<byte> span = rebind.Span;
			decoded.AttrType = asnValueReader.ReadObjectIdentifier();
			AsnValueReader asnValueReader2 = asnValueReader.ReadSetOf();
			List<ReadOnlyMemory<byte>> list = new List<ReadOnlyMemory<byte>>();
			while (asnValueReader2.HasData)
			{
				ReadOnlySpan<byte> readOnlySpan = asnValueReader2.ReadEncodedValue();
				int num;
				ReadOnlyMemory<byte> readOnlyMemory = (span.Overlaps(readOnlySpan, out num) ? rebind.Slice(num, readOnlySpan.Length) : readOnlySpan.ToArray());
				list.Add(readOnlyMemory);
			}
			decoded.AttrValues = list.ToArray();
			asnValueReader.ThrowIfNotEmpty();
		}

		// Token: 0x04000DF5 RID: 3573
		internal byte[] AttrType;

		// Token: 0x04000DF6 RID: 3574
		internal ReadOnlyMemory<byte>[] AttrValues;
	}
}
