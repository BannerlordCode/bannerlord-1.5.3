using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TaleWorlds.Library;
using TaleWorlds.ObjectSystem;

namespace TaleWorlds.CampaignSystem.Encyclopedia
{
	// Token: 0x0200017F RID: 383
	public class EncyclopediaManager
	{
		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x06001BFD RID: 7165 RVA: 0x00090A2C File Offset: 0x0008EC2C
		// (set) Token: 0x06001BFE RID: 7166 RVA: 0x00090A34 File Offset: 0x0008EC34
		public IViewDataTracker ViewDataTracker { get; private set; }

		// Token: 0x06001BFF RID: 7167 RVA: 0x00090A40 File Offset: 0x0008EC40
		public void CreateEncyclopediaPages()
		{
			this._pages = new Dictionary<Type, EncyclopediaPage>();
			this.ViewDataTracker = Campaign.Current.GetCampaignBehavior<IViewDataTracker>();
			List<Type> list = new List<Type>();
			List<Assembly> list2 = new List<Assembly>();
			Assembly assembly = typeof(EncyclopediaModelBase).Assembly;
			list2.Add(assembly);
			foreach (Assembly assembly2 in AppDomain.CurrentDomain.GetAssemblies())
			{
				AssemblyName[] referencedAssembliesSafe = assembly2.GetReferencedAssembliesSafe();
				for (int j = 0; j < referencedAssembliesSafe.Length; j++)
				{
					if (referencedAssembliesSafe[j].ToString() == assembly.GetName().ToString())
					{
						list2.Add(assembly2);
						break;
					}
				}
			}
			foreach (Assembly assembly3 in list2)
			{
				list.AddRange(assembly3.GetTypesSafe(null));
			}
			foreach (Type type in list)
			{
				if (typeof(EncyclopediaPage).IsAssignableFrom(type))
				{
					object[] array = type.GetCustomAttributesSafe(typeof(OverrideEncyclopediaModel), false);
					for (int i = 0; i < array.Length; i++)
					{
						OverrideEncyclopediaModel overrideEncyclopediaModel = array[i] as OverrideEncyclopediaModel;
						if (overrideEncyclopediaModel != null)
						{
							EncyclopediaPage encyclopediaPage = Activator.CreateInstance(type) as EncyclopediaPage;
							foreach (Type type2 in overrideEncyclopediaModel.PageTargetTypes)
							{
								this._pages.Add(type2, encyclopediaPage);
							}
						}
					}
				}
			}
			foreach (Type type3 in list)
			{
				if (typeof(EncyclopediaPage).IsAssignableFrom(type3))
				{
					object[] array = type3.GetCustomAttributesSafe(typeof(EncyclopediaModel), false);
					for (int i = 0; i < array.Length; i++)
					{
						EncyclopediaModel encyclopediaModel = array[i] as EncyclopediaModel;
						if (encyclopediaModel != null)
						{
							EncyclopediaPage encyclopediaPage2 = Activator.CreateInstance(type3) as EncyclopediaPage;
							foreach (Type type4 in encyclopediaModel.PageTargetTypes)
							{
								if (!this._pages.ContainsKey(type4))
								{
									this._pages.Add(type4, encyclopediaPage2);
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x00090CDC File Offset: 0x0008EEDC
		public IEnumerable<EncyclopediaPage> GetEncyclopediaPages()
		{
			return this._pages.Values.Distinct<EncyclopediaPage>();
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x00090CEE File Offset: 0x0008EEEE
		public EncyclopediaPage GetPageOf(Type type)
		{
			return this._pages[type];
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x00090CFC File Offset: 0x0008EEFC
		public string GetIdentifier(Type type)
		{
			return this._pages[type].GetIdentifier(type);
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x00090D10 File Offset: 0x0008EF10
		public void GoToLink(string pageType, string stringID)
		{
			if (this._executeLink == null || string.IsNullOrEmpty(pageType))
			{
				return;
			}
			if (pageType == "Home" || pageType == "LastPage")
			{
				this._executeLink(pageType, null);
				return;
			}
			if (pageType == "ListPage")
			{
				EncyclopediaPage encyclopediaPage = Campaign.Current.EncyclopediaManager.GetEncyclopediaPages().FirstOrDefault<EncyclopediaPage>((EncyclopediaPage e) => e.HasIdentifier(stringID));
				this._executeLink(pageType, encyclopediaPage);
				return;
			}
			EncyclopediaPage encyclopediaPage2 = Campaign.Current.EncyclopediaManager.GetEncyclopediaPages().FirstOrDefault<EncyclopediaPage>((EncyclopediaPage e) => e.HasIdentifier(pageType));
			MBObjectBase @object = encyclopediaPage2.GetObject(pageType, stringID);
			if (encyclopediaPage2 != null && encyclopediaPage2.IsValidEncyclopediaItem(@object))
			{
				this._executeLink(pageType, @object);
			}
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x00090E1C File Offset: 0x0008F01C
		public void GoToLink(string link)
		{
			int num = link.IndexOf('-');
			if (num > 0)
			{
				string text = link.Substring(0, num);
				string text2 = link.Substring(num + 1);
				this.GoToLink(text, text2);
				return;
			}
			Debug.FailedAssert("Failed to resolve encyclopedia link: " + link, "C:\\BuildAgent\\work\\mb3\\Source\\Bannerlord\\TaleWorlds.CampaignSystem\\Encyclopedia\\EncyclopediaManager.cs", "GoToLink", 165);
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x00090E71 File Offset: 0x0008F071
		public void SetLinkCallback(Action<string, object> ExecuteLink)
		{
			this._executeLink = ExecuteLink;
		}

		// Token: 0x04000958 RID: 2392
		private Dictionary<Type, EncyclopediaPage> _pages;

		// Token: 0x0400095A RID: 2394
		public const string HOME_ID = "Home";

		// Token: 0x0400095B RID: 2395
		public const string LIST_PAGE_ID = "ListPage";

		// Token: 0x0400095C RID: 2396
		public const string LAST_PAGE_ID = "LastPage";

		// Token: 0x0400095D RID: 2397
		private Action<string, object> _executeLink;
	}
}
