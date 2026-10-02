using System;
using System.Collections.Generic;
using Jose.jwe;
using Jose.netstandard1_4;

namespace Jose
{
	// Token: 0x0200000E RID: 14
	public class JwtSettings
	{
		// Token: 0x06000039 RID: 57 RVA: 0x00002DA0 File Offset: 0x00000FA0
		public JwtSettings RegisterJwa(JweAlgorithm alg, IKeyManagement impl)
		{
			this.keyAlgorithms[alg] = impl;
			return this;
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00002DB0 File Offset: 0x00000FB0
		public JwtSettings RegisterJwaAlias(string alias, JweAlgorithm alg)
		{
			this.keyAlgorithmsAliases[alias] = alg;
			return this;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00002DC0 File Offset: 0x00000FC0
		public JwtSettings RegisterJwe(JweEncryption alg, IJweAlgorithm impl)
		{
			this.encAlgorithms[alg] = impl;
			return this;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002DD0 File Offset: 0x00000FD0
		public JwtSettings RegisterJweAlias(string alias, JweEncryption alg)
		{
			this.encAlgorithmsAliases[alias] = alg;
			return this;
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002DE0 File Offset: 0x00000FE0
		public JwtSettings RegisterCompression(JweCompression alg, ICompression impl)
		{
			this.compressionAlgorithms[alg] = impl;
			return this;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002DF0 File Offset: 0x00000FF0
		public JwtSettings RegisterCompressionAlias(string alias, JweCompression alg)
		{
			this.compressionAlgorithmsAliases[alias] = alg;
			return this;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002E00 File Offset: 0x00001000
		public JwtSettings RegisterJws(JwsAlgorithm alg, IJwsAlgorithm impl)
		{
			this.jwsAlgorithms[alg] = impl;
			return this;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002E10 File Offset: 0x00001010
		public JwtSettings RegisterJwsAlias(string alias, JwsAlgorithm alg)
		{
			this.jwsAlgorithmsAliases[alias] = alg;
			return this;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00002E20 File Offset: 0x00001020
		public JwtSettings RegisterMapper(IJsonMapper mapper)
		{
			this.jsMapper = mapper;
			return this;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000042 RID: 66 RVA: 0x00002E2A File Offset: 0x0000102A
		// (set) Token: 0x06000043 RID: 67 RVA: 0x00002E32 File Offset: 0x00001032
		public IJsonMapper JsonMapper
		{
			get
			{
				return this.jsMapper;
			}
			set
			{
				this.jsMapper = value;
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00002E3C File Offset: 0x0000103C
		public IJwsAlgorithm Jws(JwsAlgorithm alg)
		{
			IJwsAlgorithm jwsAlgorithm;
			if (!this.jwsAlgorithms.TryGetValue(alg, out jwsAlgorithm))
			{
				return null;
			}
			return jwsAlgorithm;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00002E5C File Offset: 0x0000105C
		public string JwsHeaderValue(JwsAlgorithm algorithm)
		{
			return this.jwsAlgorithmsHeaderValue[algorithm];
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00002E6C File Offset: 0x0000106C
		public JwsAlgorithm JwsAlgorithmFromHeader(string headerValue)
		{
			foreach (KeyValuePair<JwsAlgorithm, string> keyValuePair in this.jwsAlgorithmsHeaderValue)
			{
				if (keyValuePair.Value.Equals(headerValue))
				{
					return keyValuePair.Key;
				}
			}
			JwsAlgorithm jwsAlgorithm;
			if (this.jwsAlgorithmsAliases.TryGetValue(headerValue, out jwsAlgorithm))
			{
				return jwsAlgorithm;
			}
			throw new InvalidAlgorithmException(string.Format("JWS algorithm is not supported: {0}", headerValue));
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00002EF8 File Offset: 0x000010F8
		public IJweAlgorithm Jwe(JweEncryption alg)
		{
			IJweAlgorithm jweAlgorithm;
			if (!this.encAlgorithms.TryGetValue(alg, out jweAlgorithm))
			{
				return null;
			}
			return jweAlgorithm;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00002F18 File Offset: 0x00001118
		public string JweHeaderValue(JweEncryption algorithm)
		{
			return this.encAlgorithmsHeaderValue[algorithm];
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00002F28 File Offset: 0x00001128
		public JweEncryption JweAlgorithmFromHeader(string headerValue)
		{
			foreach (KeyValuePair<JweEncryption, string> keyValuePair in this.encAlgorithmsHeaderValue)
			{
				if (keyValuePair.Value.Equals(headerValue))
				{
					return keyValuePair.Key;
				}
			}
			JweEncryption jweEncryption;
			if (this.encAlgorithmsAliases.TryGetValue(headerValue, out jweEncryption))
			{
				return jweEncryption;
			}
			throw new InvalidAlgorithmException(string.Format("JWE algorithm is not supported: {0}", headerValue));
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002FB4 File Offset: 0x000011B4
		public IKeyManagement Jwa(JweAlgorithm alg)
		{
			IKeyManagement keyManagement;
			if (!this.keyAlgorithms.TryGetValue(alg, out keyManagement))
			{
				return null;
			}
			return keyManagement;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002FD4 File Offset: 0x000011D4
		public string JwaHeaderValue(JweAlgorithm alg)
		{
			return this.keyAlgorithmsHeaderValue[alg];
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002FE4 File Offset: 0x000011E4
		public JweAlgorithm JwaAlgorithmFromHeader(string headerValue)
		{
			foreach (KeyValuePair<JweAlgorithm, string> keyValuePair in this.keyAlgorithmsHeaderValue)
			{
				if (keyValuePair.Value.Equals(headerValue))
				{
					return keyValuePair.Key;
				}
			}
			JweAlgorithm jweAlgorithm;
			if (this.keyAlgorithmsAliases.TryGetValue(headerValue, out jweAlgorithm))
			{
				return jweAlgorithm;
			}
			throw new InvalidAlgorithmException(string.Format("JWA algorithm is not supported: {0}.", headerValue));
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00003070 File Offset: 0x00001270
		public ICompression Compression(JweCompression alg)
		{
			ICompression compression;
			if (!this.compressionAlgorithms.TryGetValue(alg, out compression))
			{
				return null;
			}
			return compression;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00003090 File Offset: 0x00001290
		public ICompression Compression(string alg)
		{
			return this.Compression(this.CompressionAlgFromHeader(alg));
		}

		// Token: 0x0600004F RID: 79 RVA: 0x0000309F File Offset: 0x0000129F
		public string CompressionHeader(JweCompression value)
		{
			return this.jweCompressionHeaderValue[value];
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000030B0 File Offset: 0x000012B0
		public JweCompression CompressionAlgFromHeader(string header)
		{
			foreach (KeyValuePair<JweCompression, string> keyValuePair in this.jweCompressionHeaderValue)
			{
				if (keyValuePair.Value.Equals(header))
				{
					return keyValuePair.Key;
				}
			}
			JweCompression jweCompression;
			if (this.compressionAlgorithmsAliases.TryGetValue(header, out jweCompression))
			{
				return jweCompression;
			}
			throw new InvalidAlgorithmException(string.Format("Compression algorithm is not supported: {0}.", header));
		}

		// Token: 0x04000040 RID: 64
		private Dictionary<JwsAlgorithm, IJwsAlgorithm> jwsAlgorithms = new Dictionary<JwsAlgorithm, IJwsAlgorithm>
		{
			{
				JwsAlgorithm.none,
				new Plaintext()
			},
			{
				JwsAlgorithm.HS256,
				new HmacUsingSha("SHA256")
			},
			{
				JwsAlgorithm.HS384,
				new HmacUsingSha("SHA384")
			},
			{
				JwsAlgorithm.HS512,
				new HmacUsingSha("SHA512")
			},
			{
				JwsAlgorithm.RS256,
				new RsaUsingSha("SHA256")
			},
			{
				JwsAlgorithm.RS384,
				new RsaUsingSha("SHA384")
			},
			{
				JwsAlgorithm.RS512,
				new RsaUsingSha("SHA512")
			},
			{
				JwsAlgorithm.PS256,
				new RsaPssUsingSha(32)
			},
			{
				JwsAlgorithm.PS384,
				new RsaPssUsingSha(48)
			},
			{
				JwsAlgorithm.PS512,
				new RsaPssUsingSha(64)
			},
			{
				JwsAlgorithm.ES256,
				new EcdsaUsingSha(256)
			},
			{
				JwsAlgorithm.ES384,
				new EcdsaUsingSha(384)
			},
			{
				JwsAlgorithm.ES512,
				new EcdsaUsingSha(521)
			}
		};

		// Token: 0x04000041 RID: 65
		private Dictionary<JwsAlgorithm, string> jwsAlgorithmsHeaderValue = new Dictionary<JwsAlgorithm, string>
		{
			{
				JwsAlgorithm.none,
				"none"
			},
			{
				JwsAlgorithm.HS256,
				"HS256"
			},
			{
				JwsAlgorithm.HS384,
				"HS384"
			},
			{
				JwsAlgorithm.HS512,
				"HS512"
			},
			{
				JwsAlgorithm.RS256,
				"RS256"
			},
			{
				JwsAlgorithm.RS384,
				"RS384"
			},
			{
				JwsAlgorithm.RS512,
				"RS512"
			},
			{
				JwsAlgorithm.ES256,
				"ES256"
			},
			{
				JwsAlgorithm.ES384,
				"ES384"
			},
			{
				JwsAlgorithm.ES512,
				"ES512"
			},
			{
				JwsAlgorithm.PS256,
				"PS256"
			},
			{
				JwsAlgorithm.PS384,
				"PS384"
			},
			{
				JwsAlgorithm.PS512,
				"PS512"
			}
		};

		// Token: 0x04000042 RID: 66
		private Dictionary<string, JwsAlgorithm> jwsAlgorithmsAliases = new Dictionary<string, JwsAlgorithm>();

		// Token: 0x04000043 RID: 67
		private Dictionary<JweEncryption, IJweAlgorithm> encAlgorithms = new Dictionary<JweEncryption, IJweAlgorithm>
		{
			{
				JweEncryption.A128CBC_HS256,
				new AesCbcHmacEncryption(new HmacUsingSha("SHA256"), 256)
			},
			{
				JweEncryption.A192CBC_HS384,
				new AesCbcHmacEncryption(new HmacUsingSha("SHA384"), 384)
			},
			{
				JweEncryption.A256CBC_HS512,
				new AesCbcHmacEncryption(new HmacUsingSha("SHA512"), 512)
			},
			{
				JweEncryption.A128GCM,
				new AesGcmEncryption(128)
			},
			{
				JweEncryption.A192GCM,
				new AesGcmEncryption(192)
			},
			{
				JweEncryption.A256GCM,
				new AesGcmEncryption(256)
			}
		};

		// Token: 0x04000044 RID: 68
		private Dictionary<JweEncryption, string> encAlgorithmsHeaderValue = new Dictionary<JweEncryption, string>
		{
			{
				JweEncryption.A128CBC_HS256,
				"A128CBC-HS256"
			},
			{
				JweEncryption.A192CBC_HS384,
				"A192CBC-HS384"
			},
			{
				JweEncryption.A256CBC_HS512,
				"A256CBC-HS512"
			},
			{
				JweEncryption.A128GCM,
				"A128GCM"
			},
			{
				JweEncryption.A192GCM,
				"A192GCM"
			},
			{
				JweEncryption.A256GCM,
				"A256GCM"
			}
		};

		// Token: 0x04000045 RID: 69
		private Dictionary<string, JweEncryption> encAlgorithmsAliases = new Dictionary<string, JweEncryption>();

		// Token: 0x04000046 RID: 70
		private Dictionary<JweAlgorithm, IKeyManagement> keyAlgorithms = new Dictionary<JweAlgorithm, IKeyManagement>
		{
			{
				JweAlgorithm.RSA_OAEP,
				new RsaKeyManagement(true)
			},
			{
				JweAlgorithm.RSA_OAEP_256,
				new RsaOaep256KeyManagement()
			},
			{
				JweAlgorithm.RSA1_5,
				new RsaKeyManagement(false)
			},
			{
				JweAlgorithm.DIR,
				new DirectKeyManagement()
			},
			{
				JweAlgorithm.A128KW,
				new AesKeyWrapManagement(128)
			},
			{
				JweAlgorithm.A192KW,
				new AesKeyWrapManagement(192)
			},
			{
				JweAlgorithm.A256KW,
				new AesKeyWrapManagement(256)
			},
			{
				JweAlgorithm.ECDH_ES,
				new EcdhKeyManagement(true)
			},
			{
				JweAlgorithm.ECDH_ES_A128KW,
				new EcdhKeyManagementWithAesKeyWrap(128, new AesKeyWrapManagement(128))
			},
			{
				JweAlgorithm.ECDH_ES_A192KW,
				new EcdhKeyManagementWithAesKeyWrap(192, new AesKeyWrapManagement(192))
			},
			{
				JweAlgorithm.ECDH_ES_A256KW,
				new EcdhKeyManagementWithAesKeyWrap(256, new AesKeyWrapManagement(256))
			},
			{
				JweAlgorithm.PBES2_HS256_A128KW,
				new Pbse2HmacShaKeyManagementWithAesKeyWrap(128, new AesKeyWrapManagement(128))
			},
			{
				JweAlgorithm.PBES2_HS384_A192KW,
				new Pbse2HmacShaKeyManagementWithAesKeyWrap(192, new AesKeyWrapManagement(192))
			},
			{
				JweAlgorithm.PBES2_HS512_A256KW,
				new Pbse2HmacShaKeyManagementWithAesKeyWrap(256, new AesKeyWrapManagement(256))
			},
			{
				JweAlgorithm.A128GCMKW,
				new AesGcmKeyWrapManagement(128)
			},
			{
				JweAlgorithm.A192GCMKW,
				new AesGcmKeyWrapManagement(192)
			},
			{
				JweAlgorithm.A256GCMKW,
				new AesGcmKeyWrapManagement(256)
			}
		};

		// Token: 0x04000047 RID: 71
		private Dictionary<JweAlgorithm, string> keyAlgorithmsHeaderValue = new Dictionary<JweAlgorithm, string>
		{
			{
				JweAlgorithm.RSA1_5,
				"RSA1_5"
			},
			{
				JweAlgorithm.RSA_OAEP,
				"RSA-OAEP"
			},
			{
				JweAlgorithm.RSA_OAEP_256,
				"RSA-OAEP-256"
			},
			{
				JweAlgorithm.DIR,
				"dir"
			},
			{
				JweAlgorithm.A128KW,
				"A128KW"
			},
			{
				JweAlgorithm.A256KW,
				"A256KW"
			},
			{
				JweAlgorithm.A192KW,
				"A192KW"
			},
			{
				JweAlgorithm.ECDH_ES,
				"ECDH-ES"
			},
			{
				JweAlgorithm.ECDH_ES_A128KW,
				"ECDH-ES+A128KW"
			},
			{
				JweAlgorithm.ECDH_ES_A192KW,
				"ECDH-ES+A192KW"
			},
			{
				JweAlgorithm.ECDH_ES_A256KW,
				"ECDH-ES+A256KW"
			},
			{
				JweAlgorithm.PBES2_HS256_A128KW,
				"PBES2-HS256+A128KW"
			},
			{
				JweAlgorithm.PBES2_HS384_A192KW,
				"PBES2-HS384+A192KW"
			},
			{
				JweAlgorithm.PBES2_HS512_A256KW,
				"PBES2-HS512+A256KW"
			},
			{
				JweAlgorithm.A128GCMKW,
				"A128GCMKW"
			},
			{
				JweAlgorithm.A192GCMKW,
				"A192GCMKW"
			},
			{
				JweAlgorithm.A256GCMKW,
				"A256GCMKW"
			}
		};

		// Token: 0x04000048 RID: 72
		private Dictionary<string, JweAlgorithm> keyAlgorithmsAliases = new Dictionary<string, JweAlgorithm>();

		// Token: 0x04000049 RID: 73
		private Dictionary<JweCompression, ICompression> compressionAlgorithms = new Dictionary<JweCompression, ICompression> { 
		{
			JweCompression.DEF,
			new DeflateCompression()
		} };

		// Token: 0x0400004A RID: 74
		private Dictionary<JweCompression, string> jweCompressionHeaderValue = new Dictionary<JweCompression, string> { 
		{
			JweCompression.DEF,
			"DEF"
		} };

		// Token: 0x0400004B RID: 75
		private Dictionary<string, JweCompression> compressionAlgorithmsAliases = new Dictionary<string, JweCompression>();

		// Token: 0x0400004C RID: 76
		private IJsonMapper jsMapper = new NewtonsoftMapper();
	}
}
