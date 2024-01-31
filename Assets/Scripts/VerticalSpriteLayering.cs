using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalSpriteLayering : MonoBehaviour
{
    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        sr.sortingOrder = (int) (transform.position.y * -10);
    }

    public void UpdateSortingOrder()
    {
        sr.sortingOrder = (int) (transform.position.y * -10);
    }
}
