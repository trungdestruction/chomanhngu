using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace UI
{
    public class TapTapTap : MonoBehaviour
    {
        [SerializeField] private Transform hand;
        //[SerializeField] private List<GameObject> listTapTapTap = new List<GameObject>();


        private Tween handTween;
        private float lastTimeChangeTap = 0;
        private int tapActiveIndex = 0;

        private void OnEnable()
        {
            PlayHandTween();
            //HideAllTap();
            tapActiveIndex = 0;
        }

        private void OnDisable()
        {
            handTween?.Kill();
            handTween = null;
        }

        void PlayHandTween()
        {
            Sequence sequence = DOTween.Sequence();
            hand.localScale = Vector3.one;
            sequence.Append(hand.DOScale(1.2f, 0.35f).SetEase(Ease.Linear));
            sequence.Append(hand.DOScale(1f, 0.35f).SetEase(Ease.Linear));
            sequence.SetLoops(-1);

            handTween = sequence;
        }

        //void HideAllTap()
        //{
        //    foreach (var tap in listTapTapTap)
        //    {
        //        tap.SetActive(false);
        //    }
        //}

        //private void Update()
        //{
        //    if (Time.time - lastTimeChangeTap < 0.35f)
        //        return;

        //    lastTimeChangeTap = Time.time;
        //    for (int i = 0; i < listTapTapTap.Count; i++)
        //    {
        //        listTapTapTap[i].SetActive(i == tapActiveIndex);
        //    }
            
        //    tapActiveIndex++;
        //    if (tapActiveIndex >= listTapTapTap.Count)
        //        tapActiveIndex = 0;
        //}
    }
}