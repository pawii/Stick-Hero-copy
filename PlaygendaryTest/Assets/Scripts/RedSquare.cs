using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSquare : MonoBehaviour
{
    [SerializeField]
    private States state;

    
    private void Awake()
    {
        Player.MoveNext += OnMoveNext;
    }


    private void OnDestroy()
    {
        Player.MoveNext -= OnMoveNext;
    }


    // дублирование кода
    private void OnMoveNext()
    {
        int newState = (int)state - 1;
        if (newState == 0)
        {
            newState = 3;
        }
        state = (States)newState;

        if(state == States.Front)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
