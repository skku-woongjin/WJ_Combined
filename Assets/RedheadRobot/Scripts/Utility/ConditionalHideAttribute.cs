using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class ConditionalHideAttribute : PropertyAttribute {
#if UNITY_EDITOR
	private readonly string propertyToCheck;
	private readonly object compareValue;
#endif

	public ConditionalHideAttribute(string propertyToCheck, object compareValue = null) {
#if UNITY_EDITOR
		this.propertyToCheck = propertyToCheck;
		this.compareValue = compareValue;
#endif
}

#if UNITY_EDITOR
	public bool CheckBehaviourPropertyVisible(MonoBehaviour behaviour, string propertyName) {
		if (string.IsNullOrEmpty(propertyToCheck)) return true;

		SerializedObject serializedObject = new SerializedObject(behaviour);
		SerializedProperty property = serializedObject.FindProperty(propertyName);

		return CheckPropertyVisible(property);
	}

	public bool CheckPropertyVisible(SerializedProperty property) {
		PropertyInfo propertyInfo = FindRelativePropertyInfo(property, propertyToCheck);
		if (propertyInfo != null) {
			bool conditionValue = CheckGetterVisible(propertyInfo, property.serializedObject.targetObject);

			if (compareValue is bool) {
				return conditionValue == (bool) compareValue;
			}

			return conditionValue;
		}

		SerializedProperty conditionProperty = FindRelativeProperty(property, propertyToCheck);
		if (conditionProperty == null) return true;

		bool isBoolMatch = conditionProperty.propertyType == SerializedPropertyType.Boolean && conditionProperty.boolValue;
		string compareStringValue = compareValue != null ? compareValue.ToString().ToUpper() : "NULL";

		if (isBoolMatch && compareStringValue == "FALSE")
			isBoolMatch = false;

		string conditionPropertyStringValue = AsStringValue(conditionProperty).ToUpper();
		bool objectMatch = compareStringValue == conditionPropertyStringValue;
		bool notVisible = !isBoolMatch && !objectMatch;

		return !notVisible;
	}

	public bool CheckGetterVisible(PropertyInfo propertyInfo, object owner) {
		if (propertyInfo.PropertyType != typeof(bool)) return false;

		return (bool)propertyInfo.GetValue(owner, null);
	}

	private PropertyInfo FindRelativePropertyInfo(SerializedProperty property, string toGet) {
		return property.serializedObject.targetObject.GetType().GetProperty(toGet, BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
	}

	private SerializedProperty FindRelativeProperty(SerializedProperty property, string toGet) {
		if (property.depth == 0) return property.serializedObject.FindProperty(toGet);

		string path = property.propertyPath.Replace(".Array.data[", "[");
		string[] elements = path.Split('.');

		SerializedProperty nestedProperty = NestedPropertyOrigin(property, elements);

		if (nestedProperty != null) return nestedProperty.FindPropertyRelative(toGet);

		// if nested property is null = we hit an array property
		string cleanPath = path.Substring(0, path.IndexOf('['));
		SerializedProperty arrayProp = property.serializedObject.FindProperty(cleanPath);

		if (warningsPool.Contains(arrayProp.exposedReferenceValue)) return null;

		UnityEngine.Object target = arrayProp.serializedObject.targetObject;
		string who = string.Format("Property <color=brown>{0}</color> in object <color=brown>{1}</color> caused: ", arrayProp.name,
			target.name);

		Debug.LogWarning(who + "Array fields is not supported by [ConditionalFieldAttribute]", target);
		warningsPool.Add(arrayProp.exposedReferenceValue);

		return null;
	}

	// For [Serialized] types with [Conditional] fields
	private SerializedProperty NestedPropertyOrigin(SerializedProperty property, string[] elements) {
		SerializedProperty parent = null;

		for (int i = 0; i < elements.Length - 1; i++) {
			var element = elements[i];
			int index = -1;
			if (element.Contains("[")) {
				index = Convert.ToInt32(element.Substring(element.IndexOf("[", StringComparison.Ordinal))
					.Replace("[", "").Replace("]", ""));
				element = element.Substring(0, element.IndexOf("[", StringComparison.Ordinal));
			}

			parent = i == 0
				? property.serializedObject.FindProperty(element)
				: parent.FindPropertyRelative(element);

			if (index >= 0) parent = parent.GetArrayElementAtIndex(index);
		}

		return parent;
	}


	private string AsStringValue(SerializedProperty prop) {
		switch (prop.propertyType) {
			case SerializedPropertyType.String:
				return prop.stringValue;

			case SerializedPropertyType.Character:
			case SerializedPropertyType.Integer:
				if (prop.type == "char") return Convert.ToChar(prop.intValue).ToString();
				return prop.intValue.ToString();

			case SerializedPropertyType.ObjectReference:
				return prop.objectReferenceValue != null ? prop.objectReferenceValue.ToString() : "null";

			case SerializedPropertyType.Boolean:
				return prop.boolValue.ToString();

			case SerializedPropertyType.Enum:
				return prop.enumNames[prop.enumValueIndex];

			default:
				return string.Empty;
		}
	}

	//This pool is used to prevent spamming with warning messages
	//One message per property
	private readonly HashSet<object> warningsPool = new HashSet<object>();

	[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
	public class ConditionalHideAttributeDrawer : PropertyDrawer {
		private ConditionalHideAttribute Attribute {
			get { return _attribute ?? (_attribute = attribute as ConditionalHideAttribute); }
		}

		private ConditionalHideAttribute _attribute;

		private bool toShow = true;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			toShow = Attribute.CheckPropertyVisible(property);

			return toShow ? EditorGUI.GetPropertyHeight(property) : 0;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
			if (toShow) EditorGUI.PropertyField(position, property, label, true);
		}
	}
#endif
}