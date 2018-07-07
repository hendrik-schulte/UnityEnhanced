#if UNITY_EDITOR
using UE.Common;
using UE.Common.SubjectNerd.Utilities;
#if UE_Photon
using UE.PUNNetworking;
#endif
using UnityEditor;
using UnityEngine;

namespace UE.Instancing
{
    /// <summary>
    /// This property drawer takes care of displaying a key property field in case the respective
    /// InstanciableSO has instancing enabled. Only works for properties that implement
    /// <see cref="IInstanceReference"/>
    /// </summary>
    public abstract class InstanceReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var instanceProp = GetInstanciableProperty(property);
            var instanciable = Instanciable(property);
            var instanced = instanciable != null && instanciable.Instanced;
            var instanceReference = GetIInstanceReference(property);

            EditorGUI.BeginProperty(position, GUIContent.none, property);
            EditorGUI.BeginChangeCheck();

            if (instanced)
            {
                //Drawing the background                
                EditorGUI.DrawRect(position.Offset(0, -20, 0, 0),
                    EditorUtil.EditorBackgroundColor + new Color(.03f, .03f, .03f));

                EditorGUI.PropertyField(position.GetLine(1), instanceProp, GetMainLabel(property));

                DrawKeyProperty(position, property, instanceReference);
            }
            else //instancing not enabled: only draw instanciable1111
                EditorGUI.PropertyField(position, instanceProp, GetMainLabel(property));


            if (EditorGUI.EndChangeCheck())
                property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        /// <summary>
        /// Draw the key property.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="instanceReference"></param>
        private void DrawKeyProperty(Rect position, SerializedProperty property, IInstanceReference instanceReference)
        {
            EditorGUI.indentLevel++;

            var key = GetKeyProperty(property);
            int keyLine;

            if (key.objectReferenceValue == null)
            {
                keyLine = 4;
                EditorGUI.HelpBox(position.GetLines(2, 2),
                    InstanceReference.WARNING_INSTANCE_KEY_EMPTY,
                    MessageType.Error);
            }
#if UE_Photon
            else if (instanceReference != null && !instanceReference.HasValidNetworkingKey)
            {
                keyLine = 5;
                EditorGUI.HelpBox(position.GetLines(2, 3),
                    PhotonSync.WARNING_INSTANCE_KEY_WRONG,
                    MessageType.Error);
            }
#endif
            else keyLine = 2;

            EditorGUI.PropertyField(position.GetLine(keyLine), key, new GUIContent {text = "Instance Key"});
            EditorGUI.indentLevel--;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var instanciable = Instanciable(property);

            var key = GetKeyProperty(property);

            if (instanciable == null || !instanciable.Instanced)
                return base.GetPropertyHeight(property, label);
            if (key.objectReferenceValue == null)
                return EditorUtil.PropertyHeight(4);
#if UE_Photon
            var instanceReference = GetIInstanceReference(property);
            if (instanceReference != null && !instanceReference.HasValidNetworkingKey)
                return EditorUtil.PropertyHeight(5);
#endif
            return EditorUtil.PropertyHeight(2);
        }

        /// <summary>
        /// Creates the label for the instance reference.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        protected GUIContent GetMainLabel(SerializedProperty property)
        {
            //Take the name and tooltip of the parent property rather than the generalized one.
            var hastooltip = property.HasAttribute<TooltipAttribute>();

            var customTooltip = hastooltip ? property.GetAttribute<TooltipAttribute>().tooltip : "";

            var instanceLabel = new GUIContent
            {
                text = property.displayName,
                tooltip = customTooltip //label.tooltip
            };

            return instanceLabel;
        }

        protected SerializedProperty GetInstanciableProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative(InstanciablePropertyName);
        }

        protected abstract string InstanciablePropertyName { get; }

        protected virtual IInstanciable Instanciable(SerializedProperty property)
        {
            return GetInstanciableProperty(property).objectReferenceValue as IInstanciable;
        }

        protected SerializedProperty GetKeyProperty(SerializedProperty property)
        {
            return property.FindPropertyRelative("key");
        }

        protected IInstanceReference GetIInstanceReference(SerializedProperty property)
        {
            return property.GetValue<object>() as IInstanceReference;
        }
    }
}
#endif