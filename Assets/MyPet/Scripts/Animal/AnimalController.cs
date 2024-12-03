using UnityEngine;
//using UnityEngine.AI;

namespace MyPet.AI
{
    public class AnimalController : MonoBehaviour
    {
        /// <summary>
        /// ������ �����ϴ� Ŭ���� (�������� �θ� Ŭ����)
        /// </summary>
        #region Variables
        protected StateMachine<AnimalController> stateMachine;

        //����
        protected Animator animator;
        //protected CharacterController characterController;
        //protected NavMeshAgent agent;
        #endregion

        protected virtual void Start()
        {
            //StateMachine ����
            stateMachine = new StateMachine<AnimalController>(this, new IdleState());

            //����
            animator = GetComponent<Animator>();
            //characterController = GetComponent<CharacterController>();
            //agent = GetComponent<NavMeshAgent>();
        }

        protected virtual void Update()
        {
            //���� ������ ������Ʈ�� StateMachine�� ������Ʈ�� ���ؼ� ����
            stateMachine.Update(Time.deltaTime);
        }

        public R ChangeState<R>() where R : State<AnimalController>
        {
            return stateMachine.ChangeState<R>();
        }
    }
}