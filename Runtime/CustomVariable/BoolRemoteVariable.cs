using System;
using Unity.RemoteConfig;
using UnityEngine;

/*===============================================================
Project:	Remote Variables
Developer:	Marci San Diego
Company:	Personal - marcisandiego@gmail.com
Date:		21/04/2020 18:39
===============================================================*/

namespace MSD.Modules.RemoteVariables
{
	[CreateAssetMenu(menuName = "MSD/Modules/Remote Variables/Bool")]
	public class BoolRemoteVariable : BoolCustomVariable
	{
		[Serializable]
		public class BoolRemote : GenericRemoteVariable<bool>
		{
			protected override bool GetValue(string key)
			{
				return ConfigManager.appConfig.GetBool(key);
			}
		}

		public BoolRemote remote;

		protected override bool IsReadonly => true;

		protected override bool GetValue() => remote.Value;

		protected override void SetValue(bool value) => throw new NotImplementedException();
	}
}
