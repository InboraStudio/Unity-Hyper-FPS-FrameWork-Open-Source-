using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using System.Threading.Tasks;
using System;

namespace Inbora.FPSFramework
{
    [AddComponentMenu("Inbora/FPS Framework/Weapons/Throwable")]
    [RequireComponent(typeof(ItemInput))]
    public class Throwable : Weapon
    {
        public string itemName = "Throwable";
        public Rigidbody throwableItem;
        public Transform throwPoint;
        public Pickable replacement;
        public float minimumTriggerTime = 0.75f;
        public float throwForce = 25;
        public string triggerStateName = "Trigger";

        [Space]
        [Header("Cooking (For Explosives Only)")]
        public bool canCook = true;

        private float cookingTimer = 0;
        private float maxCookingTime = 0;


        private void Awake()
        {
            Setup(itemName, replacement);
        }

        private void Start()
        {
            itemInput.throwAction.performed += ctx =>
            {
                if (throwableItem.TryGetComponent<Explosive>(out Explosive explosive))
                {
                    maxCookingTime = explosive.delay;
                }

                cookingTimer = 0;
                if (Animator) Animator.SetTrigger("Trigger");
            };

            itemInput.throwAction.canceled += ctx =>
            {
                PerformThrow();
            };
        }

        private void Update()
        {
            if (itemInput.dropInput) Drop(Vector3.down * Inventory.dropForce, Vector3.up * Inventory.dropForce * 3);

            if(itemInput.throwAction.IsPressed() && canCook)
            {
                cookingTimer += Time.deltaTime;
            }

            if (cookingTimer >= maxCookingTime) PerformThrow();
        }

        private void FixedUpdate()
        {
            //Reset all bool, to stops animation looping.
            Animator.SetBool("Trigger", false);
            Animator.SetBool("Throw", false);
        }

        public void Throw()
        {
            //Do throw logic
            Rigidbody newThrowable = Instantiate(throwableItem, throwPoint.position, throwPoint.rotation);
            newThrowable.AddForce(throwPoint.forward * throwForce, ForceMode.VelocityChange);

            if(newThrowable.TryGetComponent<Explosive>(out Explosive explosive))
            {
                explosive.source = Actor;
                explosive.delay = maxCookingTime - cookingTimer;
            }
        }

        public async void PerformThrow()
        {
            //Stops animation overlapping.
            if (Animator && !Animator.GetCurrentAnimatorStateInfo(0).IsName(triggerStateName)) return;
            float currentTime = 0;

            //Wait until the triggering time is reached and then apply throw.
            while (currentTime < minimumTriggerTime && cookingTimer < minimumTriggerTime)
            {
                currentTime += Time.deltaTime;

                await Task.Yield();
            }

            if(Animator)
            Animator.SetTrigger("Throw");
        }
    }
}