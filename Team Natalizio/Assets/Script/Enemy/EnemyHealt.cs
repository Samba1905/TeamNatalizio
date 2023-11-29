using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealt : MonoBehaviour
{
    [SerializeField]
    int _currentHealt, _maxHealt;
    Player player;

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
        player = GameObject.FindAnyObjectByType<Player>();
        _currentHealt = _maxHealt;
    }

    void Update() 
    {
        
    }


    public void TakeDamage(int damageAmmount)
    {
        _currentHealt -= damageAmmount;
        player.canDamage = false;

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
