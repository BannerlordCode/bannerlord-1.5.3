using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.Library.Http;
using TaleWorlds.ServiceDiscovery.Client;

namespace TaleWorlds.Diamond.ClientApplication
{
	// Token: 0x02000042 RID: 66
	public class DiamondClientApplication
	{
		// Token: 0x17000062 RID: 98
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x0000590C File Offset: 0x00003B0C
		// (set) Token: 0x060001C4 RID: 452 RVA: 0x00005914 File Offset: 0x00003B14
		public ApplicationVersion ApplicationVersion { get; private set; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000591D File Offset: 0x00003B1D
		public ParameterContainer Parameters
		{
			get
			{
				return this._parameters;
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x060001C6 RID: 454 RVA: 0x00005925 File Offset: 0x00003B25
		// (set) Token: 0x060001C7 RID: 455 RVA: 0x0000592D File Offset: 0x00003B2D
		public IReadOnlyDictionary<string, string> ProxyAddressMap { get; private set; }

		// Token: 0x060001C8 RID: 456 RVA: 0x00005938 File Offset: 0x00003B38
		public DiamondClientApplication(ApplicationVersion applicationVersion, ParameterContainer parameters)
		{
			this.ApplicationVersion = applicationVersion;
			this._parameters = parameters;
			this._clientApplicationObjects = new Dictionary<string, DiamondClientApplicationObject>();
			this._clientObjects = new Dictionary<string, IClient>();
			this.ProxyAddressMap = new Dictionary<string, string>();
			ServicePointManager.DefaultConnectionLimit = 1000;
			ServicePointManager.Expect100Continue = false;
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000598A File Offset: 0x00003B8A
		public DiamondClientApplication(ApplicationVersion applicationVersion)
			: this(applicationVersion, new ParameterContainer())
		{
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00005998 File Offset: 0x00003B98
		public object GetObject(string name)
		{
			DiamondClientApplicationObject diamondClientApplicationObject;
			this._clientApplicationObjects.TryGetValue(name, out diamondClientApplicationObject);
			return diamondClientApplicationObject;
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000059B5 File Offset: 0x00003BB5
		public void AddObject(string name, DiamondClientApplicationObject applicationObject)
		{
			this._clientApplicationObjects.Add(name, applicationObject);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000059C4 File Offset: 0x00003BC4
		public void Initialize(ClientApplicationConfiguration applicationConfiguration)
		{
			this._parameters = applicationConfiguration.Parameters;
			foreach (string text in applicationConfiguration.Clients)
			{
				this.CreateClient(text, applicationConfiguration.SessionProviderType);
			}
		}

		// Token: 0x060001CD RID: 461 RVA: 0x00005A04 File Offset: 0x00003C04
		private void CreateClient(string clientConfiguration, SessionProviderType sessionProviderType)
		{
			Type type = DiamondClientApplication.FindType(clientConfiguration);
			object obj = this.CreateClientSessionProvider(clientConfiguration, sessionProviderType, this._parameters);
			IClient client = (IClient)Activator.CreateInstance(type, new object[] { this, obj });
			this._clientObjects.Add(clientConfiguration, client);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x00005A4C File Offset: 0x00003C4C
		public string ResolveClientAddress(string clientName, ParameterContainer parameters)
		{
			string text;
			parameters.TryGetParameter(clientName + ".Address", out text);
			if (ServiceAddress.IsServiceAddress(text))
			{
				string text2;
				parameters.TryGetParameter(clientName + ".ServiceDiscovery.Address", out text2);
				ServiceAddressManager.ResolveAddress(text2, ref text);
			}
			string text3 = clientName + ".Proxy.";
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			foreach (KeyValuePair<string, string> keyValuePair in parameters.Iterator)
			{
				if (keyValuePair.Key.StartsWith(text3) && keyValuePair.Key.Length > text3.Length)
				{
					dictionary[keyValuePair.Key.Substring(text3.Length)] = keyValuePair.Value;
				}
			}
			this.ProxyAddressMap = dictionary;
			string text4;
			if (dictionary.TryGetValue(text, out text4))
			{
				text = text4;
			}
			return text;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x00005B3C File Offset: 0x00003D3C
		public void ReplaceParameters(ParameterContainer parameters)
		{
			this._parameters = parameters;
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x00005B48 File Offset: 0x00003D48
		public object CreateClientSessionProvider(string clientName, SessionProviderType sessionProviderType, ParameterContainer parameters)
		{
			if (sessionProviderType != SessionProviderType.Rest && sessionProviderType != SessionProviderType.ThreadedRest)
			{
				throw new NotImplementedException("Other session provider types are not supported yet.");
			}
			string text = this.ResolveClientAddress(clientName, parameters);
			string text2;
			IHttpDriver httpDriver;
			if (parameters.TryGetParameter(clientName + ".HttpDriver", out text2))
			{
				httpDriver = HttpDriverManager.GetHttpDriver(text2);
			}
			else
			{
				httpDriver = HttpDriverManager.GetDefaultHttpDriver();
			}
			if (sessionProviderType == SessionProviderType.Rest)
			{
				return new GenericRestSessionProvider(text, httpDriver);
			}
			return new ThreadedRestSessionFactory(text, httpDriver, parameters);
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00005BAC File Offset: 0x00003DAC
		private static Assembly[] GetDiamondAssemblies()
		{
			List<Assembly> list = new List<Assembly>();
			Assembly assembly = typeof(PeerId).Assembly;
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			list.Add(assembly);
			foreach (Assembly assembly2 in assemblies)
			{
				AssemblyName[] referencedAssembliesSafe = assembly2.GetReferencedAssembliesSafe();
				for (int j = 0; j < referencedAssembliesSafe.Length; j++)
				{
					if (referencedAssembliesSafe[j].ToString() == assembly.GetName().ToString())
					{
						list.Add(assembly2);
						break;
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x00005C3C File Offset: 0x00003E3C
		private static Type FindType(string name)
		{
			Assembly[] diamondAssemblies = DiamondClientApplication.GetDiamondAssemblies();
			Type type = null;
			Assembly[] array = diamondAssemblies;
			for (int i = 0; i < array.Length; i++)
			{
				foreach (Type type2 in array[i].GetTypesSafe(null))
				{
					if (type2.Name == name)
					{
						type = type2;
					}
				}
			}
			return type;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00005CB8 File Offset: 0x00003EB8
		public T GetClient<T>(string name) where T : class, IClient
		{
			IClient client;
			if (this._clientObjects.TryGetValue(name, out client))
			{
				return client as T;
			}
			return default(T);
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x00005CEC File Offset: 0x00003EEC
		public void Update()
		{
			foreach (IClient client in this._clientObjects.Values)
			{
			}
		}

		// Token: 0x040000A5 RID: 165
		private ParameterContainer _parameters;

		// Token: 0x040000A6 RID: 166
		private Dictionary<string, DiamondClientApplicationObject> _clientApplicationObjects;

		// Token: 0x040000A7 RID: 167
		private Dictionary<string, IClient> _clientObjects;
	}
}
