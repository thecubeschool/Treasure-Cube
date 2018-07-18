using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackscreenAnimator : MonoBehaviour {

    private Animator anim;

    private void Start() {
        anim = gameObject.GetComponent<Animator>();
    }

    public void BSFadeOut() {
        StartCoroutine(BSFadeOutEn());
    }

    public void BSFadeIn() {
        StartCoroutine(BSFadeInEn());
    }

    public void BSFadeInOut() {
        StartCoroutine(BSFadeInOutEn());
    }

    private IEnumerator BSFadeOutEn() {
        anim.SetBool("fadeOut", true);
        anim.SetBool("fadeIn", false);
        anim.SetBool("fadeInOut", false);
        yield return new WaitForEndOfFrame();
        anim.SetBool("fadeOut", false);
    }

    private IEnumerator BSFadeInEn() {
        anim.SetBool("fadeOut", false);
        anim.SetBool("fadeIn", true);
        anim.SetBool("fadeInOut", false);
        yield return new WaitForEndOfFrame();
        anim.SetBool("fadeIn", false);
    }

    private IEnumerator BSFadeInOutEn() {
        anim.SetBool("fadeOut", false);
        anim.SetBool("fadeIn", false);
        anim.SetBool("fadeInOut", true);
        yield return new WaitForEndOfFrame();
        anim.SetBool("fadeInOut", false);
    }
}
