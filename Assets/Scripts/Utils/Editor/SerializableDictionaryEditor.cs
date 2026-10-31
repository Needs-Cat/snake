using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomPropertyDrawer(typeof(SerializableDictionary<,>), true)]
public class SerializableDictionaryEditor : PropertyDrawer
{
	private ReorderableList m_list = null;

	private void InitialiseList(SerializedProperty property)
	{
		SerializedProperty keysProp = property.FindPropertyRelative("m_keys");
		SerializedProperty valuesProp = property.FindPropertyRelative("m_values");

		m_list = new ReorderableList(property.serializedObject, keysProp, false, true, true, true)
		{
			drawHeaderCallback = (rect) => DrawHeader(rect, property),
			drawElementCallback = (rect, index, isActive, isFocused) => DrawElement(rect, index, isActive, isFocused, keysProp, valuesProp),
			onAddCallback = (_) => AddPressed(keysProp, valuesProp),
			onRemoveCallback = (list) => RemovePressed(list, keysProp, valuesProp),
			multiSelect = false
		};
	}
	
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		if (m_list == null)
		{
			InitialiseList(property);
		}

		m_list!.DoList(position);
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
	{
		if (m_list == null)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		return m_list.GetHeight();
	}
	
#region ReorderableListCallbacks

	private void DrawHeader(Rect rect, SerializedProperty property)
	{
		EditorGUI.LabelField(rect, property.displayName);
	}
	
	private void DrawElement(Rect rect, int index, bool isActive, bool isFocused, SerializedProperty keysProp, SerializedProperty valuesProp)
	{
		float halfWidth = rect.width * 0.5f;
		Rect keyRect = new(rect.x, rect.y + EditorGUIUtility.standardVerticalSpacing, halfWidth, rect.height - EditorGUIUtility.standardVerticalSpacing);
		Rect valueRect = new(keyRect.x + halfWidth, keyRect.y, halfWidth, keyRect.height);

		SerializedProperty keyProp = keysProp.GetArrayElementAtIndex(index);
		SerializedProperty valueProp = valuesProp.GetArrayElementAtIndex(index);

		EditorGUI.PropertyField(keyRect, keyProp, GUIContent.none);
		EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
	}

	private void AddPressed(SerializedProperty keysProp, SerializedProperty valuesProp)
	{
		keysProp.arraySize++;
		valuesProp.arraySize++;
	}

	private void RemovePressed(ReorderableList list, SerializedProperty keysProp, SerializedProperty valuesProp)
	{
		if (list.index < 0 || list.index >= keysProp.arraySize)
		{
			return;
		}

		// Safety catch
		if (keysProp.arraySize != valuesProp.arraySize)
		{
			return;
		}
		
		keysProp.DeleteArrayElementAtIndex(list.index);
		valuesProp.DeleteArrayElementAtIndex(list.index);
	}
	
#endregion
	
}
