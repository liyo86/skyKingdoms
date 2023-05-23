using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DOTDialogAnimator : MonoBehaviour
{
    [SerializeField]
    private Image dialogImage;
    private float fadeInTime = 1f;

    private void Start()
    {
        dialogImage.gameObject.SetActive(false);
    }

    public void ShowDialogBox()
    {
        dialogImage.gameObject.SetActive(true);
        dialogImage.color = new Color(dialogImage.color.r, dialogImage.color.g, dialogImage.color.b, 0f);
        dialogImage.DOFade(1f, fadeInTime);
    }

    public void HideDialogBox()
    {
        dialogImage.DOFade(0f, fadeInTime).OnComplete(() => {
            dialogImage.gameObject.SetActive(false);
        });
    }
}
