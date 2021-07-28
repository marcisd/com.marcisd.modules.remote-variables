using System.Reflection;
using UnityEditor;
using UnityEngine;
using MSD.Editor;

/*===============================================================
Project:	Poop Deck
Developer:	Marci San Diego
Company:	David Morgan Education - marcianosd@dm-ed.com
Date:		21/04/2020 18:40
===============================================================*/

namespace MSD.Modules.RemoteVariables.Editor
{
	[CustomPropertyDrawer(typeof(GenericRemoteVariableBase), true)]
	public class GenericRemoteVariableDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			SerializedProperty keyProp = property.FindPropertyRelative("_key");
			float keyHeight = EditorGUI.GetPropertyHeight(keyProp);

			Rect keyRect = new Rect(position) {
				height = keyHeight,
			};

			Rect labelRect = new Rect(position) {
				y = keyRect.yMax + EditorGUIUtility.standardVerticalSpacing,
				height = EditorGUIUtility.singleLineHeight,
			};

			Rect selectableLabelRect = new Rect(position) {
				x = keyRect.xMin + EditorGUIUtility.labelWidth + EditorGUIUtility.standardVerticalSpacing,
				y = keyRect.yMax + EditorGUIUtility.standardVerticalSpacing,
				width = keyRect.width - EditorGUIUtility.labelWidth,
				height = EditorGUIUtility.singleLineHeight,
			};

			Rect buttonPos = new Rect(keyRect) {
				y = labelRect.y + labelRect.height + EditorGUIUtility.standardVerticalSpacing,
				height = EditorGUIUtility.singleLineHeight,
			};

			using (new EditorGUI.DisabledScope(Application.isPlaying)) {
				EditorGUI.PropertyField(keyRect, keyProp);
			}

			EditorGUI.LabelField(labelRect, "Runtime Value");
			if (Application.isPlaying) {

				PropertyInfo valueInfo = fieldInfo.FieldType.GetProperty("Value");
				PropertyInfo statusInfo = fieldInfo.FieldType.GetProperty("Status");

				object instance = property.GetObjectInstance();

				string valueDisplay = valueInfo.GetValue(instance).ToString();
				GenericRemoteVariableBase.Status status = (GenericRemoteVariableBase.Status)(statusInfo.GetValue(instance));

				if (status == GenericRemoteVariableBase.Status.Found) {
					EditorGUI.SelectableLabel(selectableLabelRect, valueDisplay);
				} else {
					EditorGUI.LabelField(selectableLabelRect, status.ToString());
				}
			} else {
				EditorGUI.LabelField(selectableLabelRect, string.Empty);
			}

			if (GUI.Button(buttonPos, "Open Remote Config Window")) {
				RemoteConfigEditorAccessor.GetEditorWindow();
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			SerializedProperty keyProp = property.FindPropertyRelative("_key");
			float height = EditorGUI.GetPropertyHeight(keyProp);
			height += EditorGUIUtility.singleLineHeight * 2;
			return height;
		}
	}
}
