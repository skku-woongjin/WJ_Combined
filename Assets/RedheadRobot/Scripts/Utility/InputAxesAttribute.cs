using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class InputAxesAttribute : PropertyAttribute {
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(InputAxesAttribute))]
public class InputAxesDrawer : PropertyDrawer {
	public string[] ReadInputAxes() {
		UnityEngine.Object inputManager = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/InputManager.asset")[0];
		SerializedObject obj = new SerializedObject(inputManager);
		SerializedProperty axisArray = obj.FindProperty("m_Axes");

		if (axisArray.arraySize == 0) {
			Debug.LogError("No Input axes found!");
			return new string[] {};
		}

		string[] availableAxes = new string[axisArray.arraySize];

		for (int i = 0; i < axisArray.arraySize; ++i) {
			SerializedProperty axis = axisArray.GetArrayElementAtIndex(i);

			availableAxes[i] = axis.FindPropertyRelative("m_Name").stringValue;
		}

		return availableAxes;
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		ConditionalHideAttribute conditionalHideAttribute;
		if (!HasAttribute(property, out conditionalHideAttribute)) return EditorGUI.GetPropertyHeight(property);

		if (conditionalHideAttribute != null)
			return conditionalHideAttribute.CheckPropertyVisible(property) ? EditorGUI.GetPropertyHeight(property) : 0;

		Debug.LogError("Something went wrong when combining this attribute with a conditional hide attribute");
		return EditorGUI.GetPropertyHeight(property);
	}

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		InputAxesAttribute inputAxesAttribute = attribute as InputAxesAttribute;

		if (inputAxesAttribute == null) {
			Debug.LogError("Something went wrong when trying to draw this property attribute");
			return;
		}

		ConditionalHideAttribute conditionalHideAttribute;
		if (HasAttribute(property, out conditionalHideAttribute)) {
			if (conditionalHideAttribute == null) {
				Debug.LogError("Something went wrong when combining this attribute with a conditional hide attribute");
				return;
			}

			if (conditionalHideAttribute.CheckPropertyVisible(property) == false) {
				return;
			}
		}

		string[] inputList = ReadInputAxes();

		if (property.propertyType == SerializedPropertyType.String) {
			int index = Mathf.Max(0, Array.IndexOf(inputList, property.stringValue));
			index = EditorGUI.Popup(position, property.displayName, index, ReadInputAxes());

			property.stringValue = inputList[index];
		}
		else {
			base.OnGUI(position, property, label);
		}
	}

	private bool HasAttribute<T>(SerializedProperty property, out T foundAttribute) where T : Attribute {
		Type parentType = property.serializedObject.targetObject.GetType();
		System.Reflection.FieldInfo parentField = parentType.GetField(property.propertyPath, 
			System.Reflection.BindingFlags.NonPublic | 
			System.Reflection.BindingFlags.Public | 
			System.Reflection.BindingFlags.Instance);

		if (parentField == null) {
			foundAttribute = null;
			return false;
		}
		
		var attributes = parentField.GetCustomAttributes(typeof(T), false);

		for (int i = 0; i < attributes.Length; i++) {
			if (attributes[i].GetType() != typeof(T)) continue;

			foundAttribute = (T)attributes[i];
			return true;
		}

		foundAttribute = null;
		return false;
	}
}
#endif