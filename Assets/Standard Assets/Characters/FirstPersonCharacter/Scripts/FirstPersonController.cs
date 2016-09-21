using System;
using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        private Camera m_Camera;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private AudioSource m_AudioSource;
        private bool isCrouched = false;
        public Coroutine CrouchingCoroutine;
        public Coroutine DeCrouchingCoroutine;
        public int rotationSpeed;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
        }
        // Coroutine for Smooth Crouch_Decrouch
        IEnumerator CO_Crouching()
        {
            while (true)
            {
                m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, 1.0f, Time.deltaTime*9);
                if (m_CharacterController.height <= 1.05f)
                {
                    m_CharacterController.height = 1.0f;
                    yield break;
                }
                yield return null;
            }
        }
        IEnumerator CO_DeCrouching()
        {
            while (true)
            {
                m_CharacterController.height = Mathf.Lerp(m_CharacterController.height, 1.8f, Time.deltaTime*5);
                if (m_CharacterController.height >= 1.75f)
                {
                    m_CharacterController.height = 1.8f;
                    yield break;
                }
                yield return null;
            }
        }
        // Update is called once per frame
        private void Update()
        {
            // Lining Control 
            if (Input.GetKeyDown(KeyCode.Joystick1Button4))
            {
                m_Camera.transform.rotation = Quaternion.AngleAxis(30f, this.transform.forward);
                Debug.Log("sx Bumper");
            }
            if (Input.GetKeyDown(KeyCode.Joystick1Button5))
            {
                m_Camera.transform.rotation = Quaternion.AngleAxis(-30f, this.transform.forward);
                Debug.Log("dx Bumper");
            }

            // Crouched Control value 
            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                if (isCrouched)
                {
                    if (CrouchingCoroutine != null)
                    {
                    StopCoroutine(CrouchingCoroutine);
                    }
                    DeCrouchingCoroutine = StartCoroutine_Auto(CO_DeCrouching());
                    m_CharacterController.center = Vector3.zero;
                    m_WalkSpeed = 5;
                    m_RunSpeed = 10;
                    m_HeadBob.VerticaltoHorizontalRatio = 2;
                    isCrouched = false;
                }
                else {
                    isCrouched = true;
                    m_WalkSpeed = 1;
                    m_RunSpeed = m_WalkSpeed;
                    m_HeadBob.VerticaltoHorizontalRatio = 8;
                    if (DeCrouchingCoroutine != null)
                    {
                        StopCoroutine(DeCrouchingCoroutine);
                    }
                    CrouchingCoroutine = StartCoroutine_Auto(CO_Crouching());                 
                    m_CharacterController.center = new Vector3(0f, 0.5f, 0);
                }
            }
            //RotateView();
            // the jump state needs to read here to make sure it is not missed

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
            }
            if (!m_CharacterController.isGrounded  && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward*m_Input.y + transform.right*m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;
            }
            else
            {
                m_MoveDir += Physics.gravity*m_GravityMultiplier*Time.fixedDeltaTime;
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir*Time.fixedDeltaTime);

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
            float vertical = CrossPlatformInputManager.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

            float angH = Input.GetAxis("RightH");
            float angV = Input.GetAxis("RightV");
            float cameraAngle = Camera.main.transform.eulerAngles.x;
            if (Input.GetAxis("RightH") > 0.15f || Input.GetAxis("RightH") < -0.15f)
            {
                this.transform.localEulerAngles += new Vector3(0, angH * rotationSpeed, 0);
            }
            if (Input.GetAxis("RightV") > 0.15f || Input.GetAxis("RightV") < -0.15f)
            {
                float angle = 0;
                Camera.main.transform.localEulerAngles += new Vector3(angV * rotationSpeed, 0, 0);
                if (Camera.main.transform.localEulerAngles.x > 270)
                {
                    angle = Camera.main.transform.localEulerAngles.x - 360;
                }
                else
                    angle = Camera.main.transform.localEulerAngles.x;

                if (angle < -70)
                {
                    Camera.main.transform.localEulerAngles = new Vector3(290, 0, 0);
                }
                else if (angle > 70)
                {
                    Camera.main.transform.localEulerAngles = new Vector3(70, 0, 0);
                }
                        
            }

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }

}
