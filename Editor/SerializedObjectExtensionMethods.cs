using System;
using UnityEditor;

namespace Kogane
{
    public static class SerializedObjectExtensionMethods
    {
        public static void OnInspectorGUIWithoutScript( this Editor self )
        {
            self.serializedObject.DoDrawDefaultInspectorWithoutScript();
        }

        public static void OnInspectorGUIWithoutScript( this Editor self, Action<SerializedProperty> onGUI )
        {
            self.serializedObject.DoDrawDefaultInspectorWithoutScript( onGUI );
        }

        public static bool DoDrawDefaultInspectorWithoutScript( this SerializedObject self )
        {
            return self.DoDrawDefaultInspectorWithoutScript( null );
        }

        public static bool DoDrawDefaultInspectorWithoutScript( this SerializedObject self, Action<SerializedProperty> onGUI )
        {
            using var scope = new EditorGUI.ChangeCheckScope();

            self.UpdateIfRequiredOrScript();

            var iterator = self.GetIterator();

            for ( var enterChildren = true; iterator.NextVisible( enterChildren ); enterChildren = false )
            {
                var propertyPath = iterator.propertyPath;

                if ( propertyPath == "m_Script" ) continue;

                if ( onGUI == null )
                {
                    EditorGUILayout.PropertyField( iterator, true );
                    continue;
                }

                onGUI( iterator );
            }

            self.ApplyModifiedProperties();

            return scope.changed;
        }
    }
}