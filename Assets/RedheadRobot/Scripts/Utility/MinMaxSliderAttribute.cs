using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
public class MinMaxSliderAttribute : PropertyAttribute {
	public readonly float min;
	public readonly float max;

	public MinMaxSliderAttribute() : this(0, 1) { }

	public MinMaxSliderAttribute(float min, float max) {
		this.min = min;
		this.max = max;
	}
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
class MinMaxSliderDrawer : PropertyDrawer {
	private const string vectorMinName = "x";
	private const string vectorMaxName = "y";
	private const float floatFieldWidth = 30f;
	private const float spacing = 2f;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
		if (property.propertyType == SerializedPropertyType.Vector2) {
			float objectSpacing = MinMaxSliderDrawer.spacing * EditorGUIUtility.pixelsPerPoint;

			Vector2 range = property.vector2Value;
			float min = range.x;
			float max = range.y;

			MinMaxSliderAttribute attr = attribute as MinMaxSliderAttribute;

			EditorGUI.PrefixLabel(position, label);

			Rect sliderPos = position;
			sliderPos.x += EditorGUIUtility.labelWidth + floatFieldWidth + objectSpacing;
			sliderPos.width -= EditorGUIUtility.labelWidth + (floatFieldWidth + objectSpacing) * 2;

			EditorGUI.BeginChangeCheck();
			EditorGUI.MinMaxSlider(sliderPos, ref min, ref max, attr.min, attr.max);
			if (EditorGUI.EndChangeCheck()) {
				range.x = min;
				range.y = max;
				property.vector2Value = range;
			}

			Rect minPos = position;
			minPos.x += EditorGUIUtility.labelWidth;
			minPos.width = floatFieldWidth;

			EditorGUI.showMixedValue = property.FindPropertyRelative(vectorMinName).hasMultipleDifferentValues;
			EditorGUI.BeginChangeCheck();
			min = EditorGUI.FloatField(minPos, min);
			if (EditorGUI.EndChangeCheck()) {
				range.x = Mathf.Max(min, attr.min);
				property.vector2Value = range;
			}

			Rect maxPos = position;
			maxPos.x += maxPos.width - floatFieldWidth;
			maxPos.width = floatFieldWidth;

			EditorGUI.showMixedValue = property.FindPropertyRelative(vectorMaxName).hasMultipleDifferentValues;
			EditorGUI.BeginChangeCheck();
			max = EditorGUI.FloatField(maxPos, max);
			if (EditorGUI.EndChangeCheck()) {
				range.y = Mathf.Min(max, attr.max);
				property.vector2Value = range;
			}

			EditorGUI.showMixedValue = false;
		}
		else {
			EditorGUI.LabelField(position, label, new GUIContent("Vector2 support only"));
		}
	}
}
#endif
