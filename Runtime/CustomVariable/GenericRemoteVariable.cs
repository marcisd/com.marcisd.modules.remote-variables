using UnityEngine;
using System;
using Unity.RemoteConfig;

/*===============================================================
Project:	Remote Variables
Developer:	Marci San Diego
Company:	Personal - marcisandiego@gmail.com
Date:		21/04/2020 11:27
===============================================================*/

namespace MSD.Modules.RemoteVariables
{
	[Serializable]
	public abstract class GenericRemoteVariable<T> : GenericRemoteVariableBase
	{
		[SerializeField][TextArea(2, 5)]
		private string _key = string.Empty;

		[NonSerialized]
		private T _value = default;

		[field: NonSerialized]
		public string AssignmentId { get; private set; } = string.Empty;

		[field: NonSerialized]
		public new Status Status { get; private set; } = Status.Uninitialized;

		public T Value {
			get {
				if (AssignmentId != ConfigManager.appConfig.assignmentID) {
					AssignmentId = ConfigManager.appConfig.assignmentID;
					if (ConfigManager.appConfig.HasKey(_key)) {
						Debugger.Log("[Remote Config] Fetched value for key: " + _key);
						_value = GetValue(_key);
						Status = Status.Found;
					} else {
						Debugger.LogError("[Remote Config] Key not found in current config!");
						Status = Status.NotFound;
					}
				}
				return _value;
			}
		}

		protected abstract T GetValue(string key);
	}

	public abstract class GenericRemoteVariableBase
	{
		protected static readonly string DEBUG_PREPEND = $"[{nameof(GenericRemoteVariableBase)}]";

		public enum Status
		{
			Uninitialized,
			Found,
			NotFound,
		}
	}
}
