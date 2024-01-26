using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Rich.Base.Editor.Concrete.CodeGenerationRich.Code.ContextList
{
  [CustomPropertyDrawer(typeof(ReorderableContextList))]
  public class ReorderableDelegateDrawer : PropertyDrawer
  {
    private ReorderableList list;

    GUIStyle headerBackground = "RL Header";

    [HideInInspector] public bool mShowList = true;

    private ReorderableList getList(SerializedProperty property)
    {
      if (list == null)
      {
        list = new ReorderableList(property.serializedObject, property, true, true, true, true);

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
          if (!mShowList)
            return;

          var element = list.serializedProperty.GetArrayElementAtIndex(index);
          rect.y += 2;
//        EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width * .4f, EditorGUIUtility.singleLineHeight), "Context");
//        EditorGUI.PropertyField(
//          new Rect(rect.x + rect.width * .4f, rect.y, rect.width * .6f, EditorGUIUtility.singleLineHeight),
//          element.FindPropertyRelative("Context"), GUIContent.none);
          EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("Context"), GUIContent.none);
        };

        list.elementHeightCallback = (index) =>
        {
          if (!mShowList)
            return 0;

          return EditorGUIUtility.singleLineHeight + 5;
        };
      }

      return list;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      if (!mShowList)
        return EditorGUIUtility.singleLineHeight + 5;

      if (list == null)
        list = getList(property.FindPropertyRelative("List"));

      if (list == null)
        return 0;
      else
        return list.GetHeight();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      if (list == null)
      {
        var listProperty = property.FindPropertyRelative("List");

        list = getList(listProperty);
      }

      if (list != null)
      {
        list.drawHeaderCallback = rect =>
        {
          rect.x += 1;
          mShowList = EditorGUI.Foldout(rect, mShowList, property.name, true);
        };

        list.displayAdd = mShowList;
        list.displayRemove = mShowList;

        if (mShowList)
        {
          list.DoList(position);
        }
        else
        {
          if (UnityEngine.Event.current.type == EventType.Repaint)
          {
            headerBackground.Draw(position, false, false, false, false);
          }

          position.x += 5;
          position.y += 1;
          mShowList = EditorGUI.Foldout(position, mShowList, property.name, true);
        }
      }
    }
  }
}