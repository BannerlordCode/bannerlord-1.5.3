using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Security.Cryptography;

namespace Jose
{
	// Token: 0x0200001D RID: 29
	public class EcdhKeyManagement : IKeyManagement
	{
		// Token: 0x0600007F RID: 127 RVA: 0x00004529 File Offset: 0x00002729
		public EcdhKeyManagement(bool isDirectAgreement)
		{
			this.algIdHeader = (isDirectAgreement ? "enc" : "alg");
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00004548 File Offset: 0x00002748
		public virtual byte[][] WrapNewKey(int cekSizeBits, object key, IDictionary<string, object> header)
		{
			byte[] array = this.NewKey(cekSizeBits, key, header);
			byte[] array2 = this.Wrap(array, key);
			return new byte[][] { array, array2 };
		}

		// Token: 0x06000081 RID: 129 RVA: 0x00004578 File Offset: 0x00002778
		private byte[] NewKey(int keyLength, object key, IDictionary<string, object> header)
		{
			CngKey cngKey = Ensure.Type<CngKey>(key, "EcdhKeyManagement alg expects key to be of CngKey type.", Array.Empty<object>());
			EccKey eccKey = EccKey.Generate(cngKey);
			IDictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary["kty"] = "EC";
			dictionary["x"] = Base64Url.Encode(eccKey.X);
			dictionary["y"] = Base64Url.Encode(eccKey.Y);
			dictionary["crv"] = this.Curve(cngKey);
			header["epk"] = dictionary;
			return this.DeriveKey(header, keyLength, cngKey, eccKey.Key);
		}

		// Token: 0x06000082 RID: 130 RVA: 0x0000460C File Offset: 0x0000280C
		public virtual byte[] Wrap(byte[] cek, object key)
		{
			return Arrays.Empty;
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004614 File Offset: 0x00002814
		public virtual byte[] Unwrap(byte[] encryptedCek, object key, int cekSizeBits, IDictionary<string, object> header)
		{
			CngKey cngKey = Ensure.Type<CngKey>(key, "EcdhKeyManagement alg expects key to be of CngKey type.", Array.Empty<object>());
			Ensure.Contains(header, new string[] { "epk" }, "EcdhKeyManagement algorithm expects 'epk' key param in JWT header, but was not found", Array.Empty<object>());
			Ensure.Contains(header, new string[] { this.algIdHeader }, "EcdhKeyManagement algorithm expects 'enc' header to be present in JWT header, but was not found", Array.Empty<object>());
			IDictionary<string, object> dictionary = (IDictionary<string, object>)header["epk"];
			Ensure.Contains(dictionary, new string[] { "x", "y", "crv" }, "EcdhKeyManagement algorithm expects 'epk' key to contain 'x','y' and 'crv' fields.", Array.Empty<object>());
			byte[] array = Base64Url.Decode((string)dictionary["x"]);
			byte[] array2 = Base64Url.Decode((string)dictionary["y"]);
			CngKey cngKey2 = EccKey.New(array, array2, null, 4);
			return this.DeriveKey(header, cekSizeBits, cngKey2, cngKey);
		}

		// Token: 0x06000084 RID: 132 RVA: 0x000046F4 File Offset: 0x000028F4
		private byte[] DeriveKey(IDictionary<string, object> header, int cekSizeBits, CngKey externalPublicKey, CngKey privateKey)
		{
			byte[] bytes = Encoding.UTF8.GetBytes((string)header[this.algIdHeader]);
			byte[] array = (header.ContainsKey("apv") ? Base64Url.Decode((string)header["apv"]) : Arrays.Empty);
			byte[] array2 = (header.ContainsKey("apu") ? Base64Url.Decode((string)header["apu"]) : Arrays.Empty);
			byte[] array3 = Arrays.Concat(new byte[][]
			{
				Arrays.IntToBytes(bytes.Length),
				bytes
			});
			byte[] array4 = Arrays.Concat(new byte[][]
			{
				Arrays.IntToBytes(array2.Length),
				array2
			});
			byte[] array5 = Arrays.Concat(new byte[][]
			{
				Arrays.IntToBytes(array.Length),
				array
			});
			byte[] array6 = Arrays.IntToBytes(cekSizeBits);
			return ConcatKDF.DeriveKey(externalPublicKey, privateKey, cekSizeBits, array3, array5, array4, array6);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x000047DC File Offset: 0x000029DC
		private string Curve(CngKey key)
		{
			if (key.Algorithm == CngAlgorithm.ECDiffieHellmanP256)
			{
				return "P-256";
			}
			if (key.Algorithm == CngAlgorithm.ECDiffieHellmanP384)
			{
				return "P-384";
			}
			if (key.Algorithm == CngAlgorithm.ECDiffieHellmanP521)
			{
				return "P-521";
			}
			throw new ArgumentException("Unknown curve type " + key.Algorithm);
		}

		// Token: 0x04000050 RID: 80
		private string algIdHeader;
	}
}
