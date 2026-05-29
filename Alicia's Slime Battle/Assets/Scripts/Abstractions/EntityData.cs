using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This ScriptableObject contains data/attributes of some specific entity
 */
[CreateAssetMenu(fileName = "EntityData", menuName = "ScriptableObjects/Entity Data")]
public class EntityData : ScriptableObject
{
    // 这里是每个 entity（中文注释中我有时将以“个体”表示）都具有 最大生命值、速度、攻击力、护甲、击退抗性 的基础变量
    [SerializeField] public float maxHealth;
    [SerializeField] public float speed;
    [SerializeField] public float attackDamage;
    [SerializeField] public float armor;
    [SerializeField] public float attackKnockback;
    [SerializeField, Range(0f, 1f)] public float knockbackResistance;

    // Defines what type of entity this is; WHY THE F**K ISN'T HASHSET SUPPORTED WITH SERIALZEFIELD, AND WHY DOES UNITY NOT ALLOW MULTIPLE TAGS
    [SerializeField] public List<EntityAttribute> attributes;
}
