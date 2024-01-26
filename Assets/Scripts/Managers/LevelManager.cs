using System.Threading.Tasks;
using Signals;
using UnityEngine;
using Commands;
using Enums;

namespace Managers
{
    [Inject]
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject levelHolder;
        [SerializeField] private GameObject idleLevelHolder;

        private IdleLevelLoaderCommand idleLevelLoader;
        private ClearActiveLevelCommand levelClearer;
        private int _levelID;
        private int _idleLevelID;
        private LevelLoaderCommand levelLoader;

        private void Awake()
        {
            _idleLevelID = GetActiveIdleLevel();
            GetCommandComponents();
        }

        private void GetCommandComponents()
        {
            levelLoader = GetComponent<LevelLoaderCommand>();
            idleLevelLoader = GetComponent<IdleLevelLoaderCommand>();
            levelClearer = GetComponent<ClearActiveLevelCommand>();
        }

        private int GetActiveLevel() => SaveSignals.Instance.onGetIntSaveData.Invoke(SaveTypes.Level);

        private int GetActiveIdleLevel() => SaveSignals.Instance.onGetIntSaveData.Invoke(SaveTypes.IdleLevel);

        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += OnInitializeLevel;
            CoreGameSignals.Instance.onLevelIdleInitialize += OnInitializeIdleLevel;
            CoreGameSignals.Instance.onClearActiveLevel += OnClearActiveLevel;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onReset += OnReset;
            CoreGameSignals.Instance.onGetLevelID += OnGetLevelID;
            CoreGameSignals.Instance.onGetIdleLevelID += OnGetIdleLevelID;
        }

        private void UnsubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= OnInitializeLevel;
            CoreGameSignals.Instance.onLevelIdleInitialize -= OnInitializeIdleLevel;
            CoreGameSignals.Instance.onClearActiveLevel -= OnClearActiveLevel;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.onReset -= OnReset;
            CoreGameSignals.Instance.onGetLevelID -= OnGetLevelID;
            CoreGameSignals.Instance.onGetIdleLevelID -= OnGetIdleLevelID;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }

        #endregion

        private void Start()
        {
            _levelID = GetActiveLevel();
            OnInitializeLevel();
            OnInitializeIdleLevel();
        }

        private void OnNextLevel()
        {
            _levelID++;
            SaveSignals.Instance.onChangeSaveData?.Invoke(SaveTypes.Level, _levelID);
            CoreGameSignals.Instance.onReset?.Invoke();
        }

        private void OnNextIdleLevel()
        {
            _idleLevelID++;
            CoreGameSignals.Instance.onClearActiveIdleLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            SaveSignals.Instance.onChangeSaveData?.Invoke(SaveTypes.IdleLevel, _idleLevelID);
            CoreGameSignals.Instance.onLevelIdleInitialize?.Invoke();
        }

        private async void OnReset()
        {
            await Task.Delay(50);
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            SaveSignals.Instance.onChangeSaveData?.Invoke(SaveTypes.Level, _levelID);
            CoreGameSignals.Instance.onLevelInitialize?.Invoke();
            CoreGameSignals.Instance.onLevelIdleInitialize?.Invoke();
        }

        private int OnGetLevelID() => _levelID;

        private int OnGetIdleLevelID() => _idleLevelID;

        private void OnInitializeLevel()
        {
            var newLevelData = _levelID % Resources.Load<CD_Level>("Data/CD_Level").LevelData.LevelAmount;
            levelLoader.InitializeLevel(newLevelData, levelHolder.transform);
        }

        private void OnInitializeIdleLevel()
        {
            var newLevelData = _idleLevelID % Resources.Load<CD_IdleLevel>("Data/CD_IdleLevel").IdleLevelListData.IdleLevelData.Count;
            idleLevelLoader.InitializeIdleLevel(newLevelData, idleLevelHolder.transform);
        }

        private void OnClearActiveLevel()
        {
            OnClearActiveIdleLevel();
            levelClearer.ClearActiveLevel(levelHolder.transform);
        }

        private void OnClearActiveIdleLevel()
        {
            levelClearer.ClearActiveLevel(idleLevelHolder.transform);
        }
    }
}
