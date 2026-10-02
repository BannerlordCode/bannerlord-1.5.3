using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security;

namespace System.Runtime.InteropServices.WindowsRuntime
{
	// Token: 0x020009E4 RID: 2532
	[DebuggerDisplay("Count = {Count}")]
	internal sealed class IMapViewToIReadOnlyDictionaryAdapter
	{
		// Token: 0x06006484 RID: 25732 RVA: 0x00156B7E File Offset: 0x00154D7E
		private IMapViewToIReadOnlyDictionaryAdapter()
		{
		}

		// Token: 0x06006485 RID: 25733 RVA: 0x00156B88 File Offset: 0x00154D88
		[SecurityCritical]
		internal V Indexer_Get<K, V>(K key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			IMapView<K, V> mapView = JitHelpers.UnsafeCast<IMapView<K, V>>(this);
			return IMapViewToIReadOnlyDictionaryAdapter.Lookup<K, V>(mapView, key);
		}

		// Token: 0x06006486 RID: 25734 RVA: 0x00156BB8 File Offset: 0x00154DB8
		[SecurityCritical]
		internal IEnumerable<K> Keys<K, V>()
		{
			IMapView<K, V> mapView = JitHelpers.UnsafeCast<IMapView<K, V>>(this);
			IReadOnlyDictionary<K, V> readOnlyDictionary = (IReadOnlyDictionary<K, V>)mapView;
			return new ReadOnlyDictionaryKeyCollection<K, V>(readOnlyDictionary);
		}

		// Token: 0x06006487 RID: 25735 RVA: 0x00156BDC File Offset: 0x00154DDC
		[SecurityCritical]
		internal IEnumerable<V> Values<K, V>()
		{
			IMapView<K, V> mapView = JitHelpers.UnsafeCast<IMapView<K, V>>(this);
			IReadOnlyDictionary<K, V> readOnlyDictionary = (IReadOnlyDictionary<K, V>)mapView;
			return new ReadOnlyDictionaryValueCollection<K, V>(readOnlyDictionary);
		}

		// Token: 0x06006488 RID: 25736 RVA: 0x00156C00 File Offset: 0x00154E00
		[SecurityCritical]
		internal bool ContainsKey<K, V>(K key)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			IMapView<K, V> mapView = JitHelpers.UnsafeCast<IMapView<K, V>>(this);
			return mapView.HasKey(key);
		}

		// Token: 0x06006489 RID: 25737 RVA: 0x00156C30 File Offset: 0x00154E30
		[SecurityCritical]
		internal bool TryGetValue<K, V>(K key, out V value)
		{
			if (key == null)
			{
				throw new ArgumentNullException("key");
			}
			IMapView<K, V> mapView = JitHelpers.UnsafeCast<IMapView<K, V>>(this);
			if (!mapView.HasKey(key))
			{
				value = default(V);
				return false;
			}
			bool flag;
			try
			{
				value = mapView.Lookup(key);
				flag = true;
			}
			catch (Exception ex)
			{
				if (-2147483637 != ex._HResult)
				{
					throw;
				}
				value = default(V);
				flag = false;
			}
			return flag;
		}

		// Token: 0x0600648A RID: 25738 RVA: 0x00156CA8 File Offset: 0x00154EA8
		private static V Lookup<K, V>(IMapView<K, V> _this, K key)
		{
			V v;
			try
			{
				v = _this.Lookup(key);
			}
			catch (Exception ex)
			{
				if (-2147483637 == ex._HResult)
				{
					throw new KeyNotFoundException(Environment.GetResourceString("Arg_KeyNotFound"));
				}
				throw;
			}
			return v;
		}
	}
}
