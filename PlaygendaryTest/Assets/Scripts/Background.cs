using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;


    private Vector2 pos;


    #region Unity lifecycle

    private void Awake()
    {
        pos = transform.position;

    }


    private void OnDestroy()
    {
        
    }

    #endregion


    #region Private methods

    private IEnumerator Move()
    {
        while (true)
        {
            pos.x += moveSpeed * Time.deltaTime;
            
            yield return null;
        }
    }

    #endregion
}
