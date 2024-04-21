//Created by: Ryan King

using UnityEngine;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace HammerElf.Tools.Utilities
{
    //Beginning of a requested script for adding basic animations to an object. Saving for later work.
    public class ApplyLoopAnimation : MonoBehaviour
    {
        [EnumToggleButtons]
        public enum EffectType { NONE, BOB, ROTATE, PULSE }
        public EffectType appliedEffect;

        public bool isRotate, isPulse;

        [BoxGroup("Pulse"), ShowIf("isPulse")]
        public float pulseIntensity = 1, pulseSpeed = 0.1f;
        public Ease pulseEaseMode = Ease.Linear;

        [BoxGroup("Rotate"), ShowIf("isRotate")]
        public float rotationSpeed = 0;

        private Vector3 initialScale;
        private float tempPulseSpeed;

        private void Start()
        {
            initialScale = transform.localScale;
            tempPulseSpeed = pulseSpeed;

            if (isPulse)
            {
                transform.DOScale(transform.localScale + (new Vector3(1, 1, 1) * tempPulseSpeed), pulseSpeed).SetEase(pulseEaseMode);
            }
        }

        private void Update()
        {
            //switch (appliedEffect)
            //{
            //    case EffectType.BOB:
            //        break;
            //case EffectType.ROTATE: 
            if (isRotate)
                transform.Rotate(Vector3.up, rotationSpeed);
            //    break;
            //case EffectType.PULSE:
            if (isPulse)
            {
                if (transform.localScale.x >= initialScale.x + pulseIntensity)
                {
                    transform.DOScale(initialScale, pulseSpeed).SetEase(pulseEaseMode);
                    //tempPulseSpeed = (pulseSpeed * -1) / 100;
                }
                if (transform.localScale.x <= initialScale.x)
                {
                    transform.DOScale(initialScale + (new Vector3(1, 1, 1) * tempPulseSpeed), pulseSpeed).SetEase(pulseEaseMode);
                    //tempPulseSpeed = pulseSpeed / 100;
                }
                //transform.localScale = new Vector3(transform.localScale.x + tempPulseSpeed, transform.localScale.y + tempPulseSpeed, transform.localScale.z + tempPulseSpeed);
            }
            //    break;
            //default:
            //    break;
            //}
        }
    }
}
