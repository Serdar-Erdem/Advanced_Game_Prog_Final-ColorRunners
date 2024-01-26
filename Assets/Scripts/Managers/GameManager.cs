using UnityEngine;
using Signals;
using Enums;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Inject] public GameStates States { get; private set; }

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnChangeGameState;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState -= OnChangeGameState;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        private void OnChangeGameState(GameStates newState)
        {
            States = newState;
            CoreGameSignals.Instance.onGetGameState.Invoke(newState);
        }
    }
}
