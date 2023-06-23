using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ProjectileType
{
    Simple,
    Quadratic
}

public class SimpleProjectile : MonoBehaviour
{
    public ProjectileType projectileType; // Select the projectile type in the inspector
    public float speed = 10f;
    private Vector3 targetPosition;
    [SerializeField] private Vector3 offset;
    [HideInInspector] public GameObject handPosition;
    private bool release = false;

    private Animator animator;
    private string releaseTriggerName;
    [SerializeField] private bool lookAt;

    [SerializeField, ShowInInspectorIf("projectileType", ProjectileType.Quadratic)]
    private float acceleration = 1f;

    [SerializeField, ShowInInspectorIf("projectileType", ProjectileType.Quadratic)]
    private float maxSpeed = 20f;

    private void Update()
    {
        if (lookAt)
            transform.LookAt(targetPosition);
        if (targetPosition != Vector3.zero && release)
        {
            if (projectileType == ProjectileType.Simple)
                MoveTowardsTargetSimple();
            else if (projectileType == ProjectileType.Quadratic)
                MoveTowardsTargetQuadratic();
        }
        else
        {
            transform.position = handPosition.transform.position + offset;
        }
    }

    public void SetAnimator(Animator anim, string triggerName)
    {
        animator = anim;
        releaseTriggerName = triggerName;
    }

    private void MoveTowardsTargetSimple()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            Destroy(gameObject);
        }
    }

    private void MoveTowardsTargetQuadratic()
    {
        float currentSpeed = Mathf.Clamp(speed + (acceleration * Time.deltaTime), 0f, maxSpeed);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);

        if (transform.position == targetPosition)
        {
            Destroy(gameObject);
        }
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    public void ReleaseProjectile()
    {
        release = true;
    }

    public void TriggerAnimation()
    {
        animator.SetTrigger(releaseTriggerName);
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ShowInInspectorIfAttribute))]
public class ShowInInspectorIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowInInspectorIfAttribute conditionalAttribute = attribute as ShowInInspectorIfAttribute;

        SerializedProperty conditionProperty = property.serializedObject.FindProperty(conditionalAttribute.conditionFieldName);
        if (conditionProperty == null)
        {
            EditorGUI.PropertyField(position, property, label);
            return;
        }

        bool showProperty = conditionProperty.enumValueIndex == (int)GetEnumValueByName(conditionProperty.enumNames, conditionalAttribute.enumValueName);
        if (showProperty)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }

    private object GetEnumValueByName(string[] enumNames, ProjectileType enumValueName)
    {
        for (int i = 0; i < enumNames.Length; i++)
        {
            if (enumNames[i] == enumValueName.ToString())
            {
                return i;
            }
        }
        return null;
    }
}
#endif

[System.AttributeUsage(System.AttributeTargets.Field)]
public class ShowInInspectorIfAttribute : PropertyAttribute
{
    public string conditionFieldName;
    public ProjectileType enumValueName;

    public ShowInInspectorIfAttribute(string conditionFieldName, ProjectileType enumValueName)
    {
        this.conditionFieldName = conditionFieldName;
        this.enumValueName = enumValueName;
    }
}
