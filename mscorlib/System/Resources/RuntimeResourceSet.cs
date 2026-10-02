using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;

namespace System.Resources
{
	// Token: 0x0200039D RID: 925
	internal sealed class RuntimeResourceSet : ResourceSet, IEnumerable
	{
		// Token: 0x06002D91 RID: 11665 RVA: 0x000AE640 File Offset: 0x000AC840
		[SecurityCritical]
		internal RuntimeResourceSet(string fileName)
			: base(false)
		{
			this._resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.Default);
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			this._defaultReader = new ResourceReader(stream, this._resCache);
			this.Reader = this._defaultReader;
		}

		// Token: 0x06002D92 RID: 11666 RVA: 0x000AE68C File Offset: 0x000AC88C
		[SecurityCritical]
		internal RuntimeResourceSet(Stream stream)
			: base(false)
		{
			this._resCache = new Dictionary<string, ResourceLocator>(FastResourceComparer.Default);
			this._defaultReader = new ResourceReader(stream, this._resCache);
			this.Reader = this._defaultReader;
		}

		// Token: 0x06002D93 RID: 11667 RVA: 0x000AE6C4 File Offset: 0x000AC8C4
		protected override void Dispose(bool disposing)
		{
			if (this.Reader == null)
			{
				return;
			}
			if (disposing)
			{
				IResourceReader reader = this.Reader;
				lock (reader)
				{
					this._resCache = null;
					if (this._defaultReader != null)
					{
						this._defaultReader.Close();
						this._defaultReader = null;
					}
					this._caseInsensitiveTable = null;
					base.Dispose(disposing);
					return;
				}
			}
			this._resCache = null;
			this._caseInsensitiveTable = null;
			this._defaultReader = null;
			base.Dispose(disposing);
		}

		// Token: 0x06002D94 RID: 11668 RVA: 0x000AE758 File Offset: 0x000AC958
		public override IDictionaryEnumerator GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x000AE760 File Offset: 0x000AC960
		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumeratorHelper();
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x000AE768 File Offset: 0x000AC968
		private IDictionaryEnumerator GetEnumeratorHelper()
		{
			IResourceReader reader = this.Reader;
			if (reader == null || this._resCache == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			return reader.GetEnumerator();
		}

		// Token: 0x06002D97 RID: 11671 RVA: 0x000AE7A0 File Offset: 0x000AC9A0
		public override string GetString(string key)
		{
			object @object = this.GetObject(key, false, true);
			return (string)@object;
		}

		// Token: 0x06002D98 RID: 11672 RVA: 0x000AE7C0 File Offset: 0x000AC9C0
		public override string GetString(string key, bool ignoreCase)
		{
			object @object = this.GetObject(key, ignoreCase, true);
			return (string)@object;
		}

		// Token: 0x06002D99 RID: 11673 RVA: 0x000AE7DD File Offset: 0x000AC9DD
		public override object GetObject(string key)
		{
			return this.GetObject(key, false, false);
		}

		// Token: 0x06002D9A RID: 11674 RVA: 0x000AE7E8 File Offset: 0x000AC9E8
		public override object GetObject(string key, bool ignoreCase)
		{
			return this.GetObject(key, ignoreCase, false);
		}

		// Token: 0x06002D9B RID: 11675 RVA: 0x000AE7F4 File Offset: 0x000AC9F4
		private object GetObject(string key, bool ignoreCase, bool isString)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			if (this.Reader == null || this._resCache == null)
			{
				throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
			}
			object obj = null;
			IResourceReader reader = this.Reader;
			object obj3;
			lock (reader)
			{
				if (this.Reader == null)
				{
					throw new ObjectDisposedException(null, Environment.GetResourceString("ObjectDisposed_ResourceSet"));
				}
				ResourceLocator resourceLocator;
				if (this._defaultReader != null)
				{
					int num = -1;
					if (this._resCache.TryGetValue(key, out resourceLocator))
					{
						obj = resourceLocator.Value;
						num = resourceLocator.DataPosition;
					}
					if (num == -1 && obj == null)
					{
						num = this._defaultReader.FindPosForResource(key);
					}
					if (num != -1 && obj == null)
					{
						ResourceTypeCode resourceTypeCode;
						if (isString)
						{
							obj = this._defaultReader.LoadString(num);
							resourceTypeCode = ResourceTypeCode.String;
						}
						else
						{
							obj = this._defaultReader.LoadObject(num, out resourceTypeCode);
						}
						resourceLocator = new ResourceLocator(num, ResourceLocator.CanCache(resourceTypeCode) ? obj : null);
						Dictionary<string, ResourceLocator> resCache = this._resCache;
						lock (resCache)
						{
							this._resCache[key] = resourceLocator;
						}
					}
					if (obj != null || !ignoreCase)
					{
						return obj;
					}
				}
				if (!this._haveReadFromReader)
				{
					if (ignoreCase && this._caseInsensitiveTable == null)
					{
						this._caseInsensitiveTable = new Dictionary<string, ResourceLocator>(StringComparer.OrdinalIgnoreCase);
					}
					if (this._defaultReader == null)
					{
						IDictionaryEnumerator enumerator = this.Reader.GetEnumerator();
						while (enumerator.MoveNext())
						{
							DictionaryEntry entry = enumerator.Entry;
							string text = (string)entry.Key;
							ResourceLocator resourceLocator2 = new ResourceLocator(-1, entry.Value);
							this._resCache.Add(text, resourceLocator2);
							if (ignoreCase)
							{
								this._caseInsensitiveTable.Add(text, resourceLocator2);
							}
						}
						if (!ignoreCase)
						{
							this.Reader.Close();
						}
					}
					else
					{
						ResourceReader.ResourceEnumerator enumeratorInternal = this._defaultReader.GetEnumeratorInternal();
						while (enumeratorInternal.MoveNext())
						{
							string text2 = (string)enumeratorInternal.Key;
							int dataPosition = enumeratorInternal.DataPosition;
							ResourceLocator resourceLocator3 = new ResourceLocator(dataPosition, null);
							this._caseInsensitiveTable.Add(text2, resourceLocator3);
						}
					}
					this._haveReadFromReader = true;
				}
				object obj2 = null;
				bool flag3 = false;
				bool flag4 = false;
				if (this._defaultReader != null && this._resCache.TryGetValue(key, out resourceLocator))
				{
					flag3 = true;
					obj2 = this.ResolveResourceLocator(resourceLocator, key, this._resCache, flag4);
				}
				if (!flag3 && ignoreCase && this._caseInsensitiveTable.TryGetValue(key, out resourceLocator))
				{
					flag4 = true;
					obj2 = this.ResolveResourceLocator(resourceLocator, key, this._resCache, flag4);
				}
				obj3 = obj2;
			}
			return obj3;
		}

		// Token: 0x06002D9C RID: 11676 RVA: 0x000AEABC File Offset: 0x000ACCBC
		private object ResolveResourceLocator(ResourceLocator resLocation, string key, Dictionary<string, ResourceLocator> copyOfCache, bool keyInWrongCase)
		{
			object obj = resLocation.Value;
			if (obj == null)
			{
				IResourceReader reader = this.Reader;
				ResourceTypeCode resourceTypeCode;
				lock (reader)
				{
					obj = this._defaultReader.LoadObject(resLocation.DataPosition, out resourceTypeCode);
				}
				if (!keyInWrongCase && ResourceLocator.CanCache(resourceTypeCode))
				{
					resLocation.Value = obj;
					copyOfCache[key] = resLocation;
				}
			}
			return obj;
		}

		// Token: 0x0400128E RID: 4750
		internal const int Version = 2;

		// Token: 0x0400128F RID: 4751
		private Dictionary<string, ResourceLocator> _resCache;

		// Token: 0x04001290 RID: 4752
		private ResourceReader _defaultReader;

		// Token: 0x04001291 RID: 4753
		private Dictionary<string, ResourceLocator> _caseInsensitiveTable;

		// Token: 0x04001292 RID: 4754
		private bool _haveReadFromReader;
	}
}
