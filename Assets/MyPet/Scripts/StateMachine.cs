using System.Collections.Generic;
using UnityEditor.Compilation;
using UnityEngine;

namespace MyPet.AI
{
    /// <summary>
    /// <T>의 상태를 관리하는 클래스
    /// </summary>
    [System.Serializable]
    public abstract class State<T>
    {
        protected StateMachine<T> stateMachine; //현 state가 등록되어 있는 Machine
        protected T context;                    //stateMachine을 가지고 있는 주체

        public State() { }                      //생성자

        public void SetMachineAndContext(StateMachine<T> stateMachine, T context)
        {
            this.stateMachine = stateMachine;
            this.context = context;

            OnInitialized();
        }

        public virtual void OnInitialized() { } //생성 후 1회 실행, 초기값 설정

        public virtual void OnEnter() { }       //상태 전환시 상태로 들어올때 1회 실행

        public abstract void Update(float deltaTime);   //상태 실행 중

        public virtual void OnExit() { }        //상태 전환시 상태를 나갈때 1회 실행
    }


    /// <summary>
    /// <T>의 State들을 관리하는 클래스
    /// </summary>
    public class StateMachine<T>
    {
        private T context;                      //stateMachine을 가지고 있는 주체

        private State<T> currentState;          //현재 상태
        public State<T> CurrentState => currentState;

        private State<T> previousState;         //이전 상태
        public State<T> PreviousState => previousState;

        private float elapsedTimeInState = 0.0f;    //현재 상태 지속 시간
        public float ElapsedTimeInState => elapsedTimeInState;

        //등록된 상태의 타입을 키값으로 저장
        private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>();

        //생성자
        public StateMachine(T context, State<T> initialState)
        {
            this.context = context;

            AddState(initialState);
            currentState = initialState;
            currentState.OnEnter();
        }

        //StateMachine에 State 등록
        public void AddState(State<T> state)
        {
            state.SetMachineAndContext(this, context);
            states[state.GetType()] = state;
        }

        //StateMachine에서 State의 업데이트 실행
        public void Update(float deltaTime)
        {
            elapsedTimeInState += deltaTime;    //지나간 시간 저장(시간의 흐름)

            currentState.Update(deltaTime);
        }

        //currentState의 상태 바꾸기
        public R ChangeState<R>() where R : State<T> //State<T>를 상속받는 R만 상태를 바꿀 수 있도록 제한, 바꾼 상태로 반환
        {
            //현상태와 새로운 상태 비교
            var newType = typeof(R);
            if (currentState.GetType() == newType)  //타입이 동일하면 R로 리턴
            {
                return CurrentState as R;
            }

            //상태 변경이전
            if (currentState != null)
            {
                currentState.OnExit();
            }
            previousState = currentState;

            //상태 변경(OnExit 먼저 실행)
            currentState = states[newType]; //상태 저장
            currentState.OnEnter();         //실행
            elapsedTimeInState = 0.0f;      //시간 초기화

            return CurrentState as R;
        }
    }
}