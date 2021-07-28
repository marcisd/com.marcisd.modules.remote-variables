using System;
using Unity.RemoteConfig;
using UnityEngine;

/*===============================================================
Project:	Remote Variables
Developer:	Marci San Diego
Company:	Personal - marcisandiego@gmail.com
Date:		21/04/2020 13:34
===============================================================*/

namespace MSD.Modules.RemoteVariables
{
	[CreateAssetMenu(menuName = "MSD/Modules/Remote Variables/Float")]
	public class FloatRemoteVariable : FloatCustomVariable
	{
		[Serializable]
		public class FloatRemote : GenericRemoteVariable<float>
		{
			protected override float GetValue(string key)
			{
				return ConfigManager.appConfig.GetFloat(key);
			}
		}

		public FloatRemote remote;

		protected override bool IsReadonly => true;

		protected override float GetValue() => remote.Value;

		protected override void SetValue(float value) => throw new NotImplementedException();
	}
}
