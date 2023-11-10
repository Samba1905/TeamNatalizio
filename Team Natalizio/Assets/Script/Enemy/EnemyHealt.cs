using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealt : MonoBehaviour
{
    [SerializeField]
    int _currentHealt, _maxHealt;

    public int HealtPoints
    {
        get
        {
            return _currentHealt;
        }
        private set
        {
            if(value > _maxHealt)
            {
                _currentHealt = _maxHealt;
            }
            else
            {
                _currentHealt = value;
            }
        }
    }

    public void TakeDamage(int damageAmmount)
    {
        HealtPoints -= damageAmmount;
        if(HealtPoints <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        gameObject.SetActive(false);
    }
}
