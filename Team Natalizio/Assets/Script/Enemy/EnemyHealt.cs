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

    private void Start()
    {
        _currentHealt = _maxHealt;
    }

    void Update() 
    {
        
    }


    public void TakeDamage(int damageAmmount)
    {
        _currentHealt -= damageAmmount;

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
