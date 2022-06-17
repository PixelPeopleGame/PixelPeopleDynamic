using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupAnimation : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private RectTransform _transform;

    private void Awake()
    {
        _transform = GetComponent<RectTransform>();
    }

    public void PlayAnimation(AnimationType animationType)
    {
        StopAllCoroutines();

        switch (animationType)
        {
            case AnimationType.Show:
                StartCoroutine(nameof(ShowAnimation));
                break;
            case AnimationType.Hide:
                StartCoroutine(nameof(HideAnimation));
                break;
        } 
    }

    private IEnumerator ShowAnimation()
    {
        yield return new WaitForSeconds(0.1f);
        _transform.anchoredPosition = new Vector2(0, _transform.rect.height);
        while(Vector2.Distance(_transform.anchoredPosition, new Vector2(0,0)) > 1)
        {
            _transform.anchoredPosition = Vector2.MoveTowards(_transform.anchoredPosition, new Vector2(0, 0), _speed * Time.deltaTime);
            yield return null;
        }
    }

    private IEnumerator HideAnimation()
    {
        while (Vector2.Distance(_transform.anchoredPosition, new Vector2(0, _transform.rect.height + 1)) > 1)
        {
            _transform.anchoredPosition = Vector2.MoveTowards(_transform.anchoredPosition, new Vector2(0, _transform.rect.height + 1), _speed * Time.deltaTime);
            yield return null;
        }
    }
}

public enum AnimationType
{
    Show,
    Hide
}
