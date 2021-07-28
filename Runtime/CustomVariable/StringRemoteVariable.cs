using System;
using Unity.RemoteConfig;
using UnityEngine;

/*===============================================================
Project:	Remote Variables
Developer:	Marci San Diego
Company:	Personal - marcisandiego@gmail.com
Date:		21/04/2020 18:40
===============================================================*/

namespace MSD.Modules.RemoteVariables
{
	[CreateAssetMenu(menuName = "MSD/Modules/Remote Variables/String")]
	public class StringRemoteVariable : StringCustomVariable
	{
		[Serializable]
		public class StringRemote : GenericRemoteVariable<string>
		{
			protected override string GetValue(string key)
			{
				return ConfigManager.appConfig.GetString(key);
			}
		}

		public StringRemote remote;

		protected override bool IsReadonly => true;

		protected override string GetValue() => remote.Value;

		protected override void SetValue(string value) => throw new NotImplementedException();
	}
}
