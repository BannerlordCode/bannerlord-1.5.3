using System;
using System.Collections.Generic;
using System.Text;
using Jose.jwe;

namespace Jose
{
	// Token: 0x02000008 RID: 8
	public static class JWT
	{
		// Token: 0x17000005 RID: 5
		// (get) Token: 0x0600000F RID: 15 RVA: 0x0000245C File Offset: 0x0000065C
		public static JwtSettings DefaultSettings
		{
			get
			{
				return JWT.defaultSettings;
			}
		}

		// Token: 0x17000006 RID: 6
		// (set) Token: 0x06000010 RID: 16 RVA: 0x00002463 File Offset: 0x00000663
		[Obsolete("Custom JsonMappers should be set in DefaultSettings")]
		public static IJsonMapper JsonMapper
		{
			set
			{
				JWT.defaultSettings.RegisterMapper(value);
			}
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000247D File Offset: 0x0000067D
		public static IDictionary<string, object> Headers(string token, JwtSettings settings = null)
		{
			return JWT.Headers<IDictionary<string, object>>(token, settings);
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002488 File Offset: 0x00000688
		public static T Headers<T>(string token, JwtSettings settings = null)
		{
			Compact.Iterator iterator = Compact.Iterate(token);
			return JWT.GetSettings(settings).JsonMapper.Parse<T>(Encoding.UTF8.GetString(iterator.Next(true)));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x000024C0 File Offset: 0x000006C0
		public static string Payload(string token, bool b64 = true)
		{
			byte[] array = JWT.PayloadBytes(token, b64);
			return Encoding.UTF8.GetString(array);
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024E0 File Offset: 0x000006E0
		public static byte[] PayloadBytes(string token, bool b64 = true)
		{
			Compact.Iterator iterator = Compact.Iterate(token);
			if (iterator.Count < 3)
			{
				throw new JoseException("The given token doesn't follow JWT format and must contains at least three parts.");
			}
			if (iterator.Count > 3)
			{
				throw new JoseException("Getting payload for encrypted tokens is not supported. Please use Jose.JWT.Decode() method instead.");
			}
			iterator.Next(false);
			return iterator.Next(b64);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x0000251E File Offset: 0x0000071E
		public static T Payload<T>(string token, JwtSettings settings = null)
		{
			return JWT.GetSettings(settings).JsonMapper.Parse<T>(JWT.Payload(token, true));
		}

		// Token: 0x06000017 RID: 23 RVA: 0x00002537 File Offset: 0x00000737
		public static string Encode(object payload, object key, JweAlgorithm alg, JweEncryption enc, JweCompression? compression = null, IDictionary<string, object> extraHeaders = null, JwtSettings settings = null)
		{
			return JWT.Encode(JWT.GetSettings(settings).JsonMapper.Serialize(payload), key, alg, enc, compression, extraHeaders, settings);
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002559 File Offset: 0x00000759
		public static string Encode(string payload, object key, JweAlgorithm alg, JweEncryption enc, JweCompression? compression = null, IDictionary<string, object> extraHeaders = null, JwtSettings settings = null)
		{
			Ensure.IsNotEmpty(payload, "Payload expected to be not empty, whitespace or null.", Array.Empty<object>());
			return JWT.EncodeBytes(Encoding.UTF8.GetBytes(payload), key, alg, enc, compression, extraHeaders, settings);
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002584 File Offset: 0x00000784
		public static string EncodeBytes(byte[] payload, object key, JweAlgorithm alg, JweEncryption enc, JweCompression? compression = null, IDictionary<string, object> extraHeaders = null, JwtSettings settings = null)
		{
			if (payload == null)
			{
				throw new ArgumentNullException("payload");
			}
			JwtSettings settings2 = JWT.GetSettings(settings);
			IKeyManagement keyManagement = settings2.Jwa(alg);
			IJweAlgorithm jweAlgorithm = settings2.Jwe(enc);
			if (keyManagement == null)
			{
				throw new JoseException(string.Format("Unsupported JWA algorithm requested: {0}", alg));
			}
			if (jweAlgorithm == null)
			{
				throw new JoseException(string.Format("Unsupported JWE algorithm requested: {0}", enc));
			}
			IDictionary<string, object> dictionary = new Dictionary<string, object>
			{
				{
					"alg",
					settings2.JwaHeaderValue(alg)
				},
				{
					"enc",
					settings2.JweHeaderValue(enc)
				}
			};
			Dictionaries.Append<string, object>(dictionary, extraHeaders);
			byte[][] array = keyManagement.WrapNewKey(jweAlgorithm.KeySize, key, dictionary);
			byte[] array2 = array[0];
			byte[] array3 = array[1];
			if (compression != null)
			{
				dictionary["zip"] = settings2.CompressionHeader(compression.Value);
				payload = settings2.Compression(compression.Value).Compress(payload);
			}
			byte[] bytes = Encoding.UTF8.GetBytes(settings2.JsonMapper.Serialize(dictionary));
			byte[] bytes2 = Encoding.UTF8.GetBytes(Compact.Serialize(new byte[][] { bytes }));
			byte[][] array4 = jweAlgorithm.Encrypt(bytes2, payload, array2);
			return Compact.Serialize(new byte[][]
			{
				bytes,
				array3,
				array4[0],
				array4[1],
				array4[2]
			});
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000026CF File Offset: 0x000008CF
		public static string Encode(object payload, object key, JwsAlgorithm algorithm, IDictionary<string, object> extraHeaders = null, JwtSettings settings = null, JwtOptions options = null)
		{
			return JWT.Encode(JWT.GetSettings(settings).JsonMapper.Serialize(payload), key, algorithm, extraHeaders, settings, options);
		}

		// Token: 0x0600001B RID: 27 RVA: 0x000026EF File Offset: 0x000008EF
		public static string Encode(string payload, object key, JwsAlgorithm algorithm, IDictionary<string, object> extraHeaders = null, JwtSettings settings = null, JwtOptions options = null)
		{
			Ensure.IsNotEmpty(payload, "Payload expected to be not empty, whitespace or null.", Array.Empty<object>());
			return JWT.EncodeBytes(Encoding.UTF8.GetBytes(payload), key, algorithm, extraHeaders, settings, options);
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002718 File Offset: 0x00000918
		public static string EncodeBytes(byte[] payload, object key, JwsAlgorithm algorithm, IDictionary<string, object> extraHeaders = null, JwtSettings settings = null, JwtOptions options = null)
		{
			if (payload == null)
			{
				throw new ArgumentNullException("payload");
			}
			JwtSettings settings2 = JWT.GetSettings(settings);
			JwtOptions jwtOptions = options ?? JwtOptions.Default;
			Dictionary<string, object> dictionary = new Dictionary<string, object> { 
			{
				"alg",
				settings2.JwsHeaderValue(algorithm)
			} };
			if (extraHeaders == null)
			{
				extraHeaders = new Dictionary<string, object> { { "typ", "JWT" } };
			}
			if (!jwtOptions.EncodePayload)
			{
				dictionary["b64"] = false;
				dictionary["crit"] = Collections.Union(new string[] { "b64" }, Dictionaries.Get<string, object>(extraHeaders, "crit"));
			}
			Dictionaries.Append<string, object>(dictionary, extraHeaders);
			byte[] bytes = Encoding.UTF8.GetBytes(settings2.JsonMapper.Serialize(dictionary));
			IJwsAlgorithm jwsAlgorithm = settings2.Jws(algorithm);
			if (jwsAlgorithm == null)
			{
				throw new JoseException(string.Format("Unsupported JWS algorithm requested: {0}", algorithm));
			}
			byte[] array = jwsAlgorithm.Sign(JWT.securedInput(bytes, payload, jwtOptions.EncodePayload), key);
			byte[] array2 = (jwtOptions.DetachPayload ? new byte[0] : payload);
			if (!jwtOptions.EncodePayload)
			{
				return Compact.Serialize(bytes, Encoding.UTF8.GetString(array2), new byte[][] { array });
			}
			return Compact.Serialize(new byte[][] { bytes, array2, array });
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002860 File Offset: 0x00000A60
		public static string Decode(string token, object key, JweAlgorithm alg, JweEncryption enc, JwtSettings settings = null)
		{
			return JWT.Decode(token, key, null, new JweAlgorithm?(alg), new JweEncryption?(enc), settings, null);
		}

		// Token: 0x0600001E RID: 30 RVA: 0x0000288C File Offset: 0x00000A8C
		public static byte[] DecodeBytes(string token, object key, JweAlgorithm alg, JweEncryption enc, JwtSettings settings = null)
		{
			return JWT.DecodeBytes(token, key, null, new JweAlgorithm?(alg), new JweEncryption?(enc), settings, null);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000028B8 File Offset: 0x00000AB8
		public static string Decode(string token, object key, JwsAlgorithm alg, JwtSettings settings = null, string payload = null)
		{
			return JWT.Decode(token, key, new JwsAlgorithm?(alg), null, null, settings, payload);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000028E8 File Offset: 0x00000AE8
		public static byte[] DecodeBytes(string token, object key, JwsAlgorithm alg, JwtSettings settings = null, byte[] payload = null)
		{
			return JWT.DecodeBytes(token, key, new JwsAlgorithm?(alg), null, null, settings, payload);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002918 File Offset: 0x00000B18
		public static string Decode(string token, object key = null, JwtSettings settings = null, string payload = null)
		{
			return JWT.Decode(token, key, null, null, null, settings, payload);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000294C File Offset: 0x00000B4C
		public static byte[] DecodeBytes(string token, object key = null, JwtSettings settings = null, byte[] payload = null)
		{
			return JWT.DecodeBytes(token, key, null, null, null, settings, payload);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x0000297D File Offset: 0x00000B7D
		public static T Decode<T>(string token, object key, JweAlgorithm alg, JweEncryption enc, JwtSettings settings = null)
		{
			return JWT.GetSettings(settings).JsonMapper.Parse<T>(JWT.Decode(token, key, alg, enc, settings));
		}

		// Token: 0x06000024 RID: 36 RVA: 0x0000299B File Offset: 0x00000B9B
		public static T Decode<T>(string token, object key, JwsAlgorithm alg, JwtSettings settings = null)
		{
			return JWT.GetSettings(settings).JsonMapper.Parse<T>(JWT.Decode(token, key, alg, settings, null));
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000029B7 File Offset: 0x00000BB7
		public static T Decode<T>(string token, object key = null, JwtSettings settings = null)
		{
			return JWT.GetSettings(settings).JsonMapper.Parse<T>(JWT.Decode(token, key, settings, null));
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000029D4 File Offset: 0x00000BD4
		private static byte[] DecodeBytes(string token, object key = null, JwsAlgorithm? expectedJwsAlg = null, JweAlgorithm? expectedJweAlg = null, JweEncryption? expectedJweEnc = null, JwtSettings settings = null, byte[] payload = null)
		{
			Ensure.IsNotEmpty(token, "Incoming token expected to be in compact serialization form, not empty, whitespace or null.", Array.Empty<object>());
			Compact.Iterator iterator = Compact.Iterate(token);
			if (iterator.Count == 5)
			{
				return JWT.DecryptBytes(iterator, key, expectedJweAlg, expectedJweEnc, settings);
			}
			JwtSettings settings2 = JWT.GetSettings(settings);
			byte[] array = iterator.Next(true);
			Dictionary<string, object> dictionary = settings2.JsonMapper.Parse<Dictionary<string, object>>(Encoding.UTF8.GetString(array));
			bool flag = true;
			object obj;
			if (dictionary.TryGetValue("b64", out obj))
			{
				flag = (bool)obj;
			}
			byte[] array2 = iterator.Next(flag);
			byte[] array3 = iterator.Next(true);
			byte[] array4 = payload ?? array2;
			string text = (string)dictionary["alg"];
			JwsAlgorithm jwsAlgorithm = settings2.JwsAlgorithmFromHeader(text);
			if (expectedJwsAlg != null && expectedJwsAlg != jwsAlgorithm)
			{
				throw new InvalidAlgorithmException("The algorithm type passed to the Decode method did not match the algorithm type in the header.");
			}
			IJwsAlgorithm jwsAlgorithm2 = settings2.Jws(jwsAlgorithm);
			if (jwsAlgorithm2 == null)
			{
				throw new JoseException(string.Format("Unsupported JWS algorithm requested: {0}", text));
			}
			if (!jwsAlgorithm2.Verify(array3, JWT.securedInput(array, array4, flag), key))
			{
				throw new IntegrityException("Invalid signature.");
			}
			return array4;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002AF4 File Offset: 0x00000CF4
		private static string Decode(string token, object key = null, JwsAlgorithm? jwsAlg = null, JweAlgorithm? jweAlg = null, JweEncryption? jweEnc = null, JwtSettings settings = null, string payload = null)
		{
			byte[] array = ((payload != null) ? Encoding.UTF8.GetBytes(payload) : null);
			byte[] array2 = JWT.DecodeBytes(token, key, jwsAlg, jweAlg, jweEnc, settings, array);
			return Encoding.UTF8.GetString(array2);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002B30 File Offset: 0x00000D30
		private static byte[] DecryptBytes(Compact.Iterator parts, object key, JweAlgorithm? jweAlg, JweEncryption? jweEnc, JwtSettings settings = null)
		{
			byte[] array = parts.Next(true);
			byte[] array2 = parts.Next(true);
			byte[] array3 = parts.Next(true);
			byte[] array4 = parts.Next(true);
			byte[] array5 = parts.Next(true);
			JwtSettings settings2 = JWT.GetSettings(settings);
			IDictionary<string, object> dictionary = settings2.JsonMapper.Parse<Dictionary<string, object>>(Encoding.UTF8.GetString(array));
			JweAlgorithm jweAlgorithm = settings2.JwaAlgorithmFromHeader((string)dictionary["alg"]);
			JweEncryption jweEncryption = settings2.JweAlgorithmFromHeader((string)dictionary["enc"]);
			IKeyManagement keyManagement = settings2.Jwa(jweAlgorithm);
			IJweAlgorithm jweAlgorithm2 = settings2.Jwe(jweEncryption);
			if (keyManagement == null)
			{
				throw new JoseException(string.Format("Unsupported JWA algorithm requested: {0}", jweAlgorithm));
			}
			if (jweAlgorithm2 == null)
			{
				throw new JoseException(string.Format("Unsupported JWE algorithm requested: {0}", jweEncryption));
			}
			if (jweAlg != null && jweAlg.Value != jweAlgorithm)
			{
				throw new InvalidAlgorithmException("The algorithm type passed to the Decrypt method did not match the algorithm type in the header.");
			}
			if (jweEnc != null && jweEnc.Value != jweEncryption)
			{
				throw new InvalidAlgorithmException("The encryption type passed to the Decrypt method did not match the encryption type in the header.");
			}
			byte[] array6 = keyManagement.Unwrap(array2, key, jweAlgorithm2.KeySize, dictionary);
			byte[] bytes = Encoding.UTF8.GetBytes(Compact.Serialize(new byte[][] { array }));
			byte[] array7 = jweAlgorithm2.Decrypt(bytes, array6, array3, array4, array5);
			if (dictionary.ContainsKey("zip"))
			{
				array7 = settings2.Compression((string)dictionary["zip"]).Decompress(array7);
			}
			return array7;
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002CB7 File Offset: 0x00000EB7
		private static JwtSettings GetSettings(JwtSettings settings)
		{
			return settings ?? JWT.defaultSettings;
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002CC4 File Offset: 0x00000EC4
		private static byte[] securedInput(byte[] header, byte[] payload, bool b64)
		{
			if (!b64)
			{
				return Arrays.Concat(new byte[][]
				{
					Encoding.UTF8.GetBytes(Compact.Serialize(new byte[][] { header })),
					Encoding.UTF8.GetBytes("."),
					payload
				});
			}
			return Encoding.UTF8.GetBytes(Compact.Serialize(new byte[][] { header, payload }));
		}

		// Token: 0x0400003C RID: 60
		private static JwtSettings defaultSettings = new JwtSettings();
	}
}
