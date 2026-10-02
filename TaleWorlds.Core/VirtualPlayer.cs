using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.PlayerServices;

namespace TaleWorlds.Core
{
	// Token: 0x020000DE RID: 222
	public class VirtualPlayer
	{
		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06000B57 RID: 2903 RVA: 0x00024E82 File Offset: 0x00023082
		public static Dictionary<Type, object> PeerComponents
		{
			get
			{
				return VirtualPlayer._peerComponents;
			}
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00024E89 File Offset: 0x00023089
		static VirtualPlayer()
		{
			VirtualPlayer.FindPeerComponents();
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00024E9C File Offset: 0x0002309C
		private static void FindPeerComponents()
		{
			Debug.Print("Searching Peer Components", 0, Debug.DebugColor.White, 17592186044416UL);
			VirtualPlayer._peerComponentIds = new Dictionary<Type, uint>();
			VirtualPlayer._peerComponentTypes = new Dictionary<uint, Type>();
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			List<Type> list = new List<Type>();
			foreach (Assembly assembly in assemblies)
			{
				if (VirtualPlayer.CheckAssemblyForPeerComponent(assembly))
				{
					List<Type> typesSafe = assembly.GetTypesSafe(null);
					list.AddRange(typesSafe.Where<Type>((Type q) => typeof(PeerComponent).IsAssignableFrom(q) && typeof(PeerComponent) != q));
				}
			}
			foreach (Type type in list)
			{
				uint djb = (uint)Common.GetDJB2(type.Name);
				VirtualPlayer._peerComponentIds.Add(type, djb);
				VirtualPlayer._peerComponentTypes.Add(djb, type);
			}
			Debug.Print("Found " + list.Count + " peer components", 0, Debug.DebugColor.White, 17592186044416UL);
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00024FC4 File Offset: 0x000231C4
		private static bool CheckAssemblyForPeerComponent(Assembly assembly)
		{
			Assembly assembly2 = Assembly.GetAssembly(typeof(PeerComponent));
			if (assembly == assembly2)
			{
				return true;
			}
			AssemblyName[] referencedAssembliesSafe = assembly.GetReferencedAssembliesSafe();
			for (int i = 0; i < referencedAssembliesSafe.Length; i++)
			{
				if (referencedAssembliesSafe[i].FullName == assembly2.FullName)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00025019 File Offset: 0x00023219
		private static void EnsurePeerTypeList<T>() where T : PeerComponent
		{
			if (!VirtualPlayer._peerComponents.ContainsKey(typeof(T)))
			{
				VirtualPlayer._peerComponents.Add(typeof(T), new List<T>());
			}
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x0002504C File Offset: 0x0002324C
		private static void EnsurePeerTypeList(Type type)
		{
			if (!VirtualPlayer._peerComponents.ContainsKey(type))
			{
				IList list = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { type })) as IList;
				VirtualPlayer._peerComponents.Add(type, list);
			}
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00025096 File Offset: 0x00023296
		public static List<T> Peers<T>() where T : PeerComponent
		{
			VirtualPlayer.EnsurePeerTypeList<T>();
			return VirtualPlayer._peerComponents[typeof(T)] as List<T>;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x000250B6 File Offset: 0x000232B6
		public static void Reset()
		{
			VirtualPlayer._peerComponents = new Dictionary<Type, object>();
		}

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x06000B5F RID: 2911 RVA: 0x000250C2 File Offset: 0x000232C2
		// (set) Token: 0x06000B60 RID: 2912 RVA: 0x000250DD File Offset: 0x000232DD
		public string BannerCode
		{
			get
			{
				if (this._bannerCode == null)
				{
					this._bannerCode = "11.8.1.4345.4345.770.774.1.0.0.133.7.5.512.512.784.769.1.0.0";
				}
				return this._bannerCode;
			}
			set
			{
				this._bannerCode = value;
			}
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x000250E6 File Offset: 0x000232E6
		// (set) Token: 0x06000B62 RID: 2914 RVA: 0x000250EE File Offset: 0x000232EE
		public BodyProperties BodyProperties { get; set; }

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x000250F7 File Offset: 0x000232F7
		// (set) Token: 0x06000B64 RID: 2916 RVA: 0x000250FF File Offset: 0x000232FF
		public int Race { get; set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x06000B65 RID: 2917 RVA: 0x00025108 File Offset: 0x00023308
		// (set) Token: 0x06000B66 RID: 2918 RVA: 0x00025110 File Offset: 0x00023310
		public bool IsFemale { get; set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000B67 RID: 2919 RVA: 0x00025119 File Offset: 0x00023319
		// (set) Token: 0x06000B68 RID: 2920 RVA: 0x00025121 File Offset: 0x00023321
		public PlayerId Id { get; set; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0002512A File Offset: 0x0002332A
		// (set) Token: 0x06000B6A RID: 2922 RVA: 0x00025132 File Offset: 0x00023332
		public int Index { get; private set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06000B6B RID: 2923 RVA: 0x0002513B File Offset: 0x0002333B
		public bool IsMine
		{
			get
			{
				return MBNetwork.MyPeer == this;
			}
		}

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06000B6C RID: 2924 RVA: 0x00025145 File Offset: 0x00023345
		// (set) Token: 0x06000B6D RID: 2925 RVA: 0x0002514D File Offset: 0x0002334D
		public string UserName { get; private set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06000B6E RID: 2926 RVA: 0x00025156 File Offset: 0x00023356
		// (set) Token: 0x06000B6F RID: 2927 RVA: 0x0002515E File Offset: 0x0002335E
		public int ChosenBadgeIndex { get; set; }

		// Token: 0x06000B70 RID: 2928 RVA: 0x00025167 File Offset: 0x00023367
		public VirtualPlayer(int index, string name, PlayerId playerID, ICommunicator communicator)
		{
			this._peerEntitySystem = new EntitySystem<PeerComponent>();
			this.UserName = name;
			this.Index = index;
			this.Id = playerID;
			this.Communicator = communicator;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00025198 File Offset: 0x00023398
		public T AddComponent<T>() where T : PeerComponent, new()
		{
			T t = this._peerEntitySystem.AddComponent<T>();
			t.Peer = this;
			t.TypeId = VirtualPlayer._peerComponentIds[typeof(T)];
			VirtualPlayer.EnsurePeerTypeList<T>();
			(VirtualPlayer._peerComponents[typeof(T)] as List<T>).Add(t);
			this.Communicator.OnAddComponent(t);
			t.Initialize();
			return t;
		}

		// Token: 0x06000B72 RID: 2930 RVA: 0x00025220 File Offset: 0x00023420
		public PeerComponent AddComponent(Type peerComponentType)
		{
			PeerComponent peerComponent = this._peerEntitySystem.AddComponent(peerComponentType);
			peerComponent.Peer = this;
			peerComponent.TypeId = VirtualPlayer._peerComponentIds[peerComponentType];
			VirtualPlayer.EnsurePeerTypeList(peerComponentType);
			(VirtualPlayer._peerComponents[peerComponentType] as IList).Add(peerComponent);
			this.Communicator.OnAddComponent(peerComponent);
			peerComponent.Initialize();
			return peerComponent;
		}

		// Token: 0x06000B73 RID: 2931 RVA: 0x00025282 File Offset: 0x00023482
		public PeerComponent AddComponent(uint componentId)
		{
			return this.AddComponent(VirtualPlayer._peerComponentTypes[componentId]);
		}

		// Token: 0x06000B74 RID: 2932 RVA: 0x00025295 File Offset: 0x00023495
		public PeerComponent GetComponent(uint componentId)
		{
			return this.GetComponent(VirtualPlayer._peerComponentTypes[componentId]);
		}

		// Token: 0x06000B75 RID: 2933 RVA: 0x000252A8 File Offset: 0x000234A8
		public T GetComponent<T>() where T : PeerComponent
		{
			return this._peerEntitySystem.GetComponent<T>();
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x000252B5 File Offset: 0x000234B5
		public PeerComponent GetComponent(Type peerComponentType)
		{
			return this._peerEntitySystem.GetComponent(peerComponentType);
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x000252C4 File Offset: 0x000234C4
		public void RemoveComponent<T>(bool synched = true) where T : PeerComponent
		{
			T component = this._peerEntitySystem.GetComponent<T>();
			if (component != null)
			{
				this._peerEntitySystem.RemoveComponent(component);
				(VirtualPlayer._peerComponents[typeof(T)] as List<T>).Remove(component);
				if (synched)
				{
					this.Communicator.OnRemoveComponent(component);
				}
			}
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0002532A File Offset: 0x0002352A
		public void RemoveComponent(PeerComponent component)
		{
			this._peerEntitySystem.RemoveComponent(component);
			(VirtualPlayer._peerComponents[component.GetType()] as IList).Remove(component);
			this.Communicator.OnRemoveComponent(component);
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0002535F File Offset: 0x0002355F
		public void OnDisconnect()
		{
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x00025364 File Offset: 0x00023564
		public void SynchronizeComponentsTo(VirtualPlayer peer)
		{
			foreach (PeerComponent peerComponent in this._peerEntitySystem.Components)
			{
				this.Communicator.OnSynchronizeComponentTo(peer, peerComponent);
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x000253C4 File Offset: 0x000235C4
		public void UpdateIndexForReconnectingPlayer(int playerIndex)
		{
			this.Index = playerIndex;
		}

		// Token: 0x04000686 RID: 1670
		private const string DefaultPlayerBannerCode = "11.8.1.4345.4345.770.774.1.0.0.133.7.5.512.512.784.769.1.0.0";

		// Token: 0x04000687 RID: 1671
		private static Dictionary<Type, object> _peerComponents = new Dictionary<Type, object>();

		// Token: 0x04000688 RID: 1672
		private static Dictionary<Type, uint> _peerComponentIds;

		// Token: 0x04000689 RID: 1673
		private static Dictionary<uint, Type> _peerComponentTypes;

		// Token: 0x0400068A RID: 1674
		private string _bannerCode;

		// Token: 0x0400068F RID: 1679
		public readonly ICommunicator Communicator;

		// Token: 0x04000690 RID: 1680
		private EntitySystem<PeerComponent> _peerEntitySystem;

		// Token: 0x04000694 RID: 1684
		public Dictionary<int, List<int>> UsedCosmetics;
	}
}
