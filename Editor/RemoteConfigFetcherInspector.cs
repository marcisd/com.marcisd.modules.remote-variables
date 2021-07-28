using System;
using UnityEditor;
using UnityEngine;

/*===============================================================
Project:	Remote Variables
Developer:	Marci San Diego
Company:	Personal - marcisandiego@gmail.com
Date:		21/04/2020 14:22
===============================================================*/

namespace MSD.Modules.RemoteVariables.Editor
{
	[CustomEditor(typeof(RemoteConfigFetcher))]
	public class RemoteConfigFetcherInspector : UnityEditor.Editor 
	{
		private RemoteConfigFetcher Target => target as RemoteConfigFetcher;
		
		public override void OnInspectorGUI()
		{
			using (new EditorGUI.DisabledScope(EditorApplication.isPlaying)) {
				base.OnInspectorGUI();

				if (RemoteConfigEditorAccessor.IsServiceAvailable) {
					DrawRemoteConfigEnvironmentEditor();
				} else {
					EditorGUILayout.HelpBox("Unity's Remote Config is not set up yet.", MessageType.Error);
					if (GUILayout.Button("Open Remote Config Window")) {
						RemoteConfigEditorAccessor.GetEditorWindow();
					}
				}
			}

			DrawRemoteConfigInfo();
		}

		private void DrawRemoteConfigEnvironmentEditor()
		{
			using (EditorGUI.ChangeCheckScope changeCheck = new EditorGUI.ChangeCheckScope()) {
				string[] environmentNames = RemoteConfigEditorAccessor.GetEnvironmentNames();
				int selectedIndex = Array.FindIndex(environmentNames, (env) => env == Target.EnvironmentName);
				selectedIndex = selectedIndex < 0 ? 0 : selectedIndex;
				selectedIndex = EditorGUILayout.Popup("Environment", selectedIndex, environmentNames);

				if (environmentNames.Length > 0) {
					(string, string) environmentInfo = RemoteConfigEditorAccessor.GetEnvironmentInfoForName(environmentNames[selectedIndex]);
					Target.SetEnvironment(environmentInfo.Item1, environmentInfo.Item2);
				}

				if (changeCheck.changed) {
					EditorUtility.SetDirty(target);
				}
			}
		}

		private void DrawRemoteConfigInfo()
		{
			EditorGUILayout.Space();
			EditorGUILayout.LabelField("Information", EditorStyles.boldLabel);
			EditorGUILayout.LabelField("Environment Name", Target.EnvironmentName);
			EditorGUILayout.LabelField("Environment ID", Target.EnvironmentId);
			EditorGUILayout.LabelField("Assignment ID", Target.AssignmentId);
			EditorGUILayout.LabelField("Origin", Target.ConfigOrigin.ToString());
			EditorGUILayout.LabelField("Request Status", Target.ConfigRequestStatus.ToString());
		}
	}
}
