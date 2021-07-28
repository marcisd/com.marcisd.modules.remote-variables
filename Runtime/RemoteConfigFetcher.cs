using System;
using Unity.RemoteConfig;
using UnityEngine;
using System.Threading.Tasks;

#if UNITY_EDITOR
using UnityEditor;
#endif

/*===============================================================
Project:	Remote Variables
Developer:	Marci San Diego
Company:	Personal - marcisandiego@gmail.com
Date:		21/04/2020 10:47
===============================================================*/

namespace MSD.Modules.RemoteVariables
{
	using UObject = UnityEngine.Object;

	public class RemoteConfigFetcher : ScriptableConfig<RemoteConfigFetcher>
	{
		private struct UserAttributes { }

		private struct AppAttributes { }

		[SerializeField] private bool _shouldFetchOnAppStart = true;

		[SerializeField, HideInInspector]
		private string _environmentName = string.Empty;

		[SerializeField, HideInInspector]
		private string _environmentId = string.Empty;

		private TaskCompletionSource<bool> _tcs;

		public string EnvironmentName {
			get => _environmentName;
			private set => _environmentName = value;
		}

		public string EnvironmentId {
			get => _environmentId;
			private set => _environmentId = value;
		}

		[field: NonSerialized]
		public string AssignmentId { get; private set; }

		[field: NonSerialized]
		public ConfigOrigin ConfigOrigin { get; private set; }

		[field: NonSerialized]
		public ConfigRequestStatus ConfigRequestStatus { get; private set; }

		public void Fetch()
		{
			_ = FetchAsync();
		}

		public async Task FetchAsync()
		{
			Debugger.Log("[Remote Config] Start fetch!");
			ConfigManager.SetEnvironmentID(EnvironmentId);
			ConfigManager.FetchConfigs(new UserAttributes(), new AppAttributes());
			_tcs = new TaskCompletionSource<bool>();
			await _tcs.Task.ConfigureAwait(false);
		}

		internal void SetEnvironment(string name, string id)
		{
			EnvironmentName = name;
			EnvironmentId = id;
		}

		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
		private static void Bootstrap()
		{
			ConfigManager.FetchCompleted += Instance.FetchCompleted;

			if (Instance._shouldFetchOnAppStart) {
				Instance.Fetch();
			}
		}

		private void FetchCompleted(ConfigResponse configResponse)
		{
			ConfigOrigin = configResponse.requestOrigin;
			ConfigRequestStatus = configResponse.status;

			if (configResponse.status.HasFlag(ConfigRequestStatus.Success)) {
				EnvironmentId = ConfigManager.appConfig.environmentID;
				AssignmentId = ConfigManager.appConfig.assignmentID;
				Debugger.Log("[Remote Config] Fetch completed successfully!");
				_tcs.SetResult(true);
			} else {
				Debugger.LogWarning("[Remote Config] Fetch completed with status: " + configResponse.status);
				_tcs.SetResult(false);
			}
		}

#if UNITY_EDITOR

		[MenuItem("MSD/Config/Remote Config")]
		private static void ShowConfig()
		{
			Selection.objects = new UObject[] { Instance };
		}

#endif
	}
}
