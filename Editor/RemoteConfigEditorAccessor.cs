using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using Newtonsoft.Json.Linq;

/*===============================================================
Project:	Remote Variables
Developer:	Marci San Diego
Company:	Personal - marcisandiego@gmail.com
Date:		21/04/2020 14:22
===============================================================*/

namespace MSD.Modules.RemoteVariables.Editor
{
	internal static class RemoteConfigEditorAccessor
	{
		private static readonly Lazy<Assembly> s_assemblyLazyLoader = new Lazy<Assembly>(() => {
			return AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "Unity.RemoteConfig.Editor");
		});

		private static readonly Lazy<object> s_dataStoreLazyLoader = new Lazy<object>(() => {
			string path = Path.Combine("Assets/Editor/RemoteConfig/Data", "RemoteConfigDataStoreAsset.asset");
			string typeName = "Unity.RemoteConfig.Editor.RemoteConfigDataStore";
			Type type = s_assemblyLazyLoader.Value.GetType(typeName);

			object obj = AssetDatabase.LoadAssetAtPath(path, type);
			return Convert.ChangeType(obj, type) ?? null;
		});

		private static readonly Lazy<MethodInfo> s_getWindowMethod = new Lazy<MethodInfo>(() => {
			string typeName = "Unity.RemoteConfig.Editor.RemoteConfigWindow";
			Type type = s_assemblyLazyLoader.Value.GetType(typeName);
			return type.GetMethod("GetWindow", BindingFlags.Static | BindingFlags.Public);
		});

		private static readonly Lazy<PropertyInfo> s_environmentsField = new Lazy<PropertyInfo>(() => {
			string typeName = "Unity.RemoteConfig.Editor.RemoteConfigDataStore";
			Type type = s_assemblyLazyLoader.Value.GetType(typeName);
			return type.GetProperty("environments", BindingFlags.Instance | BindingFlags.Public);
		});

		public static bool IsServiceAvailable => DataStore != null;

		private static object DataStore => s_dataStoreLazyLoader.Value;

		private static MethodInfo GetWindowMethod => s_getWindowMethod.Value;

		private static PropertyInfo EnvironmentsField => s_environmentsField.Value;

		public static void GetEditorWindow() => GetWindowMethod.Invoke(null, null);

		public static string[] GetEnvironmentNames()
		{
			return GetEnvironments().Select(env => env["name"].Value<string>()).ToArray();
		}

		public static (string, string) GetEnvironmentInfoForName(string environmentName)
		{
			JObject environmentObj = (JObject)GetEnvironments().FirstOrDefault(e => e["name"].Value<string>() == environmentName);
			return (environmentObj["name"].Value<string>(), environmentObj["id"].Value<string>());
		}

		private static JArray GetEnvironments() => EnvironmentsField != null ? EnvironmentsField.GetValue(DataStore) as JArray : new JArray();
	}
}
