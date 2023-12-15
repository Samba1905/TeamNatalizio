using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealt : MonoBehaviour
{
    Player player;
    Animator anim;
    [SerializeField]
    int _currentHealt, _maxHealt;
    bool _isDeath;
    public bool IsDeath { get { return _isDeath; } }
    

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
        anim = GetComponentInParent<Animator>();
        _currentHealt = _maxHealt;
    }

    public void TakeDamage(int damageAmmount)
    {
        _currentHealt -= damageAmmount;
        player.canDamage = false;

        if(HealtPoints <= 0)
        {
            anim.SetBool("Death", true);
            _isDeath = true;
            //Invoke("Die", 3f);
        }
    }



    public void Die()
    {
        gameObject.SetActive(false);
    }
}
