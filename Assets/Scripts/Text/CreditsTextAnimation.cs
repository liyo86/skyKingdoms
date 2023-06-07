using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CreditsTextAnimation : MonoBehaviour
{
    public float delay = 1f;
    public float fadeInDuration = 1f;
    public float fadeOutDuration = 1f;
    public float moveDistance = 200f;
    public float moveDuration = 2f;

    private void Start()
    {
        AnimateCreditTexts();
    }

    private void AnimateCreditTexts()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            child.GetComponent<CanvasGroup>().alpha = 0f;
            child.transform.localPosition -= new Vector3(0f, moveDistance, 0f);
        }

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            float delayTime = i * (fadeInDuration + moveDuration + fadeOutDuration) + delay;

            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(delayTime);
            sequence.AppendCallback(() => child.gameObject.SetActive(true));
            sequence.Append(child.GetComponent<CanvasGroup>().DOFade(1f, fadeInDuration));
            sequence.Append(child.transform.DOLocalMoveY(child.transform.localPosition.y + moveDistance, moveDuration));
            sequence.Append(child.GetComponent<CanvasGroup>().DOFade(0f, fadeOutDuration));
            
            if (i == transform.childCount - 1)
            {
                sequence.AppendCallback(() => StartCoroutine(BackToMenu()));
            }
        }
    }

    private IEnumerator BackToMenu()
    {
        yield return new WaitForSeconds(5f);
        
        SceneManager.LoadScene("Menu_game");
    }
}
