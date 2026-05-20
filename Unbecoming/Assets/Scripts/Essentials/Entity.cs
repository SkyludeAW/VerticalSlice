using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Entity : MonoBehaviour {
    public Hex CurrentHex { get; private set; }

    [SerializeField] private float _moveSpeed = 3.0f;
    private bool _isMoving = false;

    public bool Invulnerable = false;
    public bool IsDead => Hitpoints <= 0;

    #region Attributes
    [SerializeField] private float _maxHitpoints = 1f;
    public float MaxHitpoints => _maxHitpoints;

    [SerializeField] private float _hitpoints = 1f;
    public float Hitpoints => _hitpoints;

    [SerializeField] private float _offense;
    public float Offense => _offense;

    [SerializeField] private float _defense;
    public float Defense => _defense;

    [SerializeField] private int _agility;
    public int Agility => _agility;

    [SerializeField] private int _range;
    public int Range => _range;

    [SerializeField] private float _maxMana;
    public float MaxMana => _maxMana;

    [SerializeField] private float _mana;
    public float Mana => _mana;
    #endregion

    public List<Ability> Abilities = new List<Ability>();

    public void TakeDamage(float damage) {
        if (Invulnerable) return;
        _hitpoints -= damage;

        if (IsDead) {
            Die();
        }
    }

    public void Die() {
        
    }

    public void Heal(float amount) {
        _hitpoints = Mathf.Min(_hitpoints + amount, MaxHitpoints);
    }

    #region Mobility
    public bool MoveTo(Hex destination, MapManager map) {

        if (_isMoving)
            return false;

        List<Hex> path = HexPathfinder.FindPath(
            map,
            CurrentHex,
            destination,
            Agility
        );

        if (path == null)
            return false;

        StartCoroutine(FollowPath(path));

        return true;
    }

    public void SetHex(Hex hex) {

        // Clear previous occupancy
        if (CurrentHex != null) {
            CurrentHex.OccupiedEntity = null;
        }

        CurrentHex = hex;

        if (CurrentHex != null) {
            CurrentHex.OccupiedEntity = this;
        }

        transform.position = hex.ToWorldPosition();
    }

    private IEnumerator FollowPath(List<Hex> path) {

        _isMoving = true;

        // Skip index 0 because it's the current tile
        for (int i = 1; i < path.Count; i++) {

            Hex nextHex = path[i];

            // Update occupancy BEFORE movement
            CurrentHex.OccupiedEntity = null;
            nextHex.OccupiedEntity = this;

            Vector3 targetPosition = nextHex.ToWorldPosition();

            while (
                Vector3.Distance(transform.position, targetPosition) > 0.01f
            ) {

                transform.position = Vector3.MoveTowards(
                    transform.position,
                    targetPosition,
                    _moveSpeed * Time.deltaTime
                );

                yield return null;
            }

            transform.position = targetPosition;

            CurrentHex = nextHex;
        }

        _isMoving = false;
    }
    #endregion

    #region Combat
    public bool Attack(Entity target) {
        bool success = false;

        bool inRange, haveLineOfSight;

        return success;
    }

    public bool UseAbility(Ability ability, Entity target) {
        bool success = false;
        
        return success;
    }
    #endregion
}
