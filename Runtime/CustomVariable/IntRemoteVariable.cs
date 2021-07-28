using System;
using Unity.RemoteConfig;
using UnityEngine;

/*===============================================================
Project:	Remote Variables
Developer:	Marci San Diego
Company:	Personal - marcisandiego@gmail.com
Date:		21/04/2020 18:38
===============================================================*/

namespace MSD.Modules.RemoteVariables
{
	[CreateAssetMenu(menuName = "MSD/Modules/Remote Variables/Int")]
	public class IntRemoteVariable : IntCustomVariable
	{
		[Serializable]
		public class IntRemote : GenericRemoteVariable<int>
		{
			protected override int GetValue(string key)
			{
				return ConfigManager.appConfig.GetInt(key);
			}
		}

		public IntRemote remote;

		protected override bool IsReadonly => true;

		protected override int GetValue() => remote.Value;

		protected override void SetValue(int value) => throw new NotImplementedException();
	}
}
