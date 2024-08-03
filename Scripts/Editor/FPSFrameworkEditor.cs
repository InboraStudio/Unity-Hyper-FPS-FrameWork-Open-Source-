using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Inbora.FPSFramework
{
    public static class FPSFrameworkEditor
    {
        [MenuItem(MenuItemPaths.CreateWeaponMotion)]
        private static void CreateWeaponMotionFromMenu()
        {
            CreateWeaponMotion();
        }

        [MenuItem(MenuItemPaths.CreateAttachmentSight)]
        public static void CreateSightAttachment()
        {
            Attachment attachment = new GameObject("Sight Attachment").AddComponent<Attachment>();
            AttachmentSight sight = attachment.gameObject.AddComponent<AttachmentSight>();
            attachment.gameObject.AddComponent<AttachementBehavior>();

            attachment.transform.parent = Selection.activeTransform;
            Selection.activeGameObject = attachment.gameObject;

            Undo.RegisterCreatedObjectUndo(attachment, "Create  " + attachment.name);
        }

        [MenuItem(MenuItemPaths.CreateAttachmentMuzzle)]
        public static void CreateMuzzleAttachment()
        {
            Attachment attachment = new GameObject("Muzzle Attachment").AddComponent<Attachment>();
            AttachmentMuzzle muzzle = attachment.gameObject.AddComponent<AttachmentMuzzle>();
            attachment.gameObject.AddComponent<AttachementBehavior>();

            attachment.transform.parent = Selection.activeTransform;
            Selection.activeGameObject = attachment.gameObject;

            Undo.RegisterCreatedObjectUndo(attachment, "Create  " + attachment.name);
        }

        public static Firearm CreateFirearm(string name)
        {
            Firearm firearm = CreateWeaponMotion(name, typeof(Firearm)).GetComponent<Firearm>();
            firearm.name = name;

            return firearm;
        }

        public static GameObject CreateWeaponMotion(string name = "FPS Motion", Type type = null)
        {
            GameObject holder = null;

            FPSMotion motion = null;

            if (Selection.activeTransform != null && Selection.activeTransform.GetComponent<Weapon>() != null)
            {
                holder = Selection.activeGameObject;
                motion = holder.AddComponent<FPSMotion>();
            }
            else
            {
                holder = new GameObject(name);
                if (type != null) holder.AddComponent(type);
                motion = holder.AddComponent<FPSMotion>();
            }

            Transform transforms = new GameObject("Transforms").transform;

            Transform sway = new GameObject("Sway").transform;
            Transform crouch = new GameObject("Crouch").transform;
            Transform jump = new GameObject("Jumping").transform;
            Transform lowerdPos = new GameObject("Lowerd Pos").transform;
            Transform aimDownSight = new GameObject("Aim Down Sight").transform;
            Transform bobbing = new GameObject("Bobbing").transform;
            Transform obstacleAvoidnace = new GameObject("Obstacle Avoidnace").transform;
            Transform recoil = new GameObject("Recoil").transform;
            Transform idle = new GameObject("Idle").transform;
            Transform sprint = new GameObject("Sprint").transform;
            Transform lean = new GameObject("Lean").transform;
            Transform weapon = new GameObject("Weapon").transform;
            Transform mesh = new GameObject("Mesh").transform;
            GameObject defaultMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);



            if (holder.GetComponent<Firearm>())
            {
                Transform setup = new GameObject("Setup").transform;
                Transform setup_base = new GameObject("Base").transform;
                Transform muzzle = new GameObject("Muzzle").transform;
                Transform casingEjectionPort = new GameObject("Casing Ejection Port").transform;

                Transform attachments = new GameObject("Attachemnts").transform;
                Transform sight = new GameObject("Sight").transform;
                Transform stock = new GameObject("Stock").transform;
                Transform attachment_muzzle = new GameObject("Muzzle").transform;
                Transform magazine = new GameObject("Magazine").transform;
                Transform laser = new GameObject("Laser").transform;

                Firearm firearm = holder.GetComponent<Firearm>();
                firearm._muzzle = muzzle;
                firearm.casingEjectionPort = casingEjectionPort;

                setup.parent = mesh;
                setup_base.parent = setup;
                muzzle.parent = setup_base;
                casingEjectionPort.parent = setup_base;
                attachments.parent = setup;
                defaultMesh.transform.parent = mesh;

                setup.Reset();

                setup_base.Reset();

                attachments.Reset();

                defaultMesh.transform.Reset();

                muzzle.SetPositionAndRotation(new Vector3(0, 0, 0.42f), Quaternion.identity, true);

                casingEjectionPort.SetPositionAndRotation(new Vector3(1.74f, 5.534f, 4.027f), Quaternion.Euler(new Vector3(-2.1f, 0, 0)), true);

                sight.parent = attachments;
                stock.parent = attachments;
                attachment_muzzle.parent = attachments;
                magazine.parent = attachments;
                laser.parent = attachments;
            }

            CameraShaker holderShaker = idle.gameObject.AddComponent<CameraShaker>();

            holderShaker.rotation = Vector3.one;
            holderShaker.position = Vector3.zero;

            holder.transform.parent = Selection.activeTransform;

            transforms.parent = holder.transform;

            sway.parent = transforms;
            crouch.parent = sway;
            jump.parent = crouch;
            lowerdPos.parent = jump;
            aimDownSight.parent = lowerdPos;
            bobbing.parent = aimDownSight;
            obstacleAvoidnace.parent = bobbing;
            recoil.parent = obstacleAvoidnace;
            idle.parent = recoil;
            sprint.parent = idle;
            lean.parent = sprint;
            weapon.parent = lean;
            mesh.parent = weapon;

            holder.transform.Reset();

            transforms.SetPositionAndRotation(new Vector3(0.07599993f, -0.0829999f, 0.2899996f), Quaternion.identity, true);

            sway.Reset();
            crouch.Reset();
            jump.Reset();
            lowerdPos.Reset();
            aimDownSight.Reset();
            bobbing.Reset();
            obstacleAvoidnace.Reset();
            recoil.Reset();

            idle.SetPositionAndRotation(new Vector3(0, -0.057f, 0.023f), Quaternion.identity, true);

            sprint.Reset();
            lean.Reset();
            weapon.Reset();
            mesh.Reset();

            motion.Use_AimDownSight = true;
            motion.Use_Bobbing = true;
            motion.Use_Holding = true;
            motion.Use_Jumping = true;
            motion.Use_Leaning = true;
            motion.Use_LowerdPos = true;
            motion.Use_Movement = true;
            motion.Use_ObstacleAvoidnace = true;
            motion.Use_Sprinting = true;
            motion.Use_Sway = true;
            motion.Use_TacticalSprint = true;
            motion.Use_Crouch = true;

            motion.aimTranform = aimDownSight;
            motion.bobbingTransform = bobbing;
            motion.holder = holderShaker;
            motion.JumpBobTranform = jump;
            motion.leaningTransform = lean;
            motion.lowerdPosTransform = lowerdPos;
            motion.obstacleAvoidnaceTransform = obstacleAvoidnace;
            motion.sprintTransform = sprint;
            motion.swayTransform = sway;
            motion.crouchTransform = crouch;

            aimDownSight.localPosition = new Vector3(0.1f, 0, 0);

            defaultMesh.transform.localPosition = new Vector3(0.01f, -0.2f, 0.42f);
            defaultMesh.transform.localScale = new Vector3(0.15f, 0.4f, 1.5f);

            Undo.RegisterCreatedObjectUndo(holder, "Create  " + holder.name);

            Selection.activeGameObject = holder;
            return holder;
        }

        public static Canvas FindOrCreateCanves()
        {
            Canvas canvas = GameObject.FindObjectOfType<Canvas>();
            CanvasScaler canvasScaler = null;
            GraphicRaycaster graphicRaycaster = null;
            EventSystem eventSystem = GameObject.FindObjectOfType<EventSystem>();
            StandaloneInputModule standaloneInputModule = null;

            if (!canvas)
            {
                canvas = new GameObject("Canvas").AddComponent<Canvas>();
                canvasScaler = canvas.gameObject.AddComponent<CanvasScaler>();
                graphicRaycaster = canvas.gameObject.AddComponent<GraphicRaycaster>();
            }

            if (!eventSystem)
            {
                eventSystem = new GameObject("Event System").AddComponent<EventSystem>();
                standaloneInputModule = eventSystem.gameObject.AddComponent<StandaloneInputModule>();
            }

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            Selection.activeGameObject = canvas.gameObject;

            return canvas;
        }
    }
}