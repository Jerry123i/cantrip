using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[System.Flags]
public enum EnemyGroup {
        FIRE   = 1,
        EARTH  = 2,
        WATER  = 4,
        AIR    = 8,
        AETHER = 16
}

[System.Flags]
public enum EnemyTag {
    RANGED = 1,
    MELEE = 2,
    SUPPORT = 4,
    IMMOVABLE = 8,
    VANILLA = 16,
    FODDER= 32
}

[CreateAssetMenu(fileName = "EnemyTag", menuName = "Enemy Tag", order = 2)]
public class EnemyInfoData : ScriptableObject {

	public string name;

	[EnumFlags]
    public EnemyGroup groups;
	[EnumFlags]
    public EnemyTag tags;

}

public class EnumFlagsAttribute : PropertyAttribute {
	public EnumFlagsAttribute() { }
}

[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
public class EnumFlagsAttributeDrawer : PropertyDrawer {

	public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label) {
		int buttonsIntValue = 0;
		int enumLength = _property.enumNames.Length;
		bool[] buttonPressed = new bool[enumLength];
		float buttonWidth = (_position.width - EditorGUIUtility.labelWidth) / enumLength;

		EditorGUI.LabelField(new Rect(_position.x, _position.y, EditorGUIUtility.labelWidth, _position.height), _label);

		EditorGUI.BeginChangeCheck();

		for (int i = 0; i < enumLength; i++) {

			// Check if the button is/was pressed 
			if ((_property.intValue & (1 << i)) == 1 << i) {
				buttonPressed[i] = true;
			}

			Rect buttonPos = new Rect(_position.x + EditorGUIUtility.labelWidth + buttonWidth * i, _position.y, buttonWidth, _position.height);

			buttonPressed[i] = GUI.Toggle(buttonPos, buttonPressed[i], _property.enumNames[i], "Button");

			if (buttonPressed[i])
				buttonsIntValue += 1 << i;
		}

		if (EditorGUI.EndChangeCheck()) {
			_property.intValue = buttonsIntValue;
		}
	}
}