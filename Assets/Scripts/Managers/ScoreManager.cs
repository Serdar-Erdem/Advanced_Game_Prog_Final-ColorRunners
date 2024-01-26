using System.Collections.Generic;
using Data.ValueObjects;
using Enums;
using Signals;
using UnityEngine;

namespace Managers
{
    [Inject]
    public class ScoreManager : MonoBehaviour
    {
        private int _totalScore;
        private List<int> _scoreVariables = new List<int>(System.Enum.GetNames(typeof(ScoreVariableType)).Length);

        private void Awake()
        {
            InitScoreValues();
            SubscribeEvents();
        }

        private void InitScoreValues()
        {
            _scoreVariables.Clear();
            _scoreVariables.AddRange(new int[System.Enum.GetNames(typeof(ScoreVariableType)).Length]);
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onChangeGameState += OnSendScoreToManagers;
            ScoreSignals.Instance.onChangeScore += OnChangeScore;
            SaveSignals.Instance.onApplicationPause += OnSendScoreToSave;
            ScoreSignals.Instance.onAddLevelTototalScore += OnAddLevelToTotalScore;
            SaveSignals.Instance.onSendDataToManagers += InitTotalScoreData;
            ScoreSignals.Instance.onGetScore += OnGetScore;
            StackSignals.Instance.onStackInit += OnReset;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void UnSubscribeEvents()
        {
            SaveSignals.Instance.onApplicationPause -= OnSendScoreToSave;
            CoreGameSignals.Instance.onChangeGameState -= OnSendScoreToManagers;
            ScoreSignals.Instance.onChangeScore -= OnChangeScore;
            SaveSignals.Instance.onSendDataToManagers -= InitTotalScoreData;
            ScoreSignals.Instance.onAddLevelTototalScore -= OnAddLevelToTotalScore;
            ScoreSignals.Instance.onGetScore -= OnGetScore;
            StackSignals.Instance.onStackInit -= OnReset;
        }

        private void OnSendScoreToSave()
        {
            SaveSignals.Instance.onChangeSaveData(SaveTypes.TotalColorman, _totalScore);
        }

        private void InitTotalScoreData(SaveData saveData)
        {
            _scoreVariables[(int)ScoreVariableType.TotalScore] = saveData.TotalColorman;
            ScoreSignals.Instance.onUpdateScore?.Invoke(_scoreVariables);
        }

        private int OnGetScore(ScoreVariableType scoreVarType)
        {
            return _scoreVariables[(int)scoreVarType];
        }

        private void OnReset()
        {
            _scoreVariables[(int)ScoreVariableType.LevelScore] = 0;
            ScoreSignals.Instance.onUpdateScore?.Invoke(_scoreVariables);
        }

        private void OnAddLevelToTotalScore()
        {
            _scoreVariables[(int)ScoreVariableType.TotalScore] += _scoreVariables[(int)ScoreVariableType.LevelScore];
        }

        private void OnChangeScore(ScoreTypes scoreType, ScoreVariableType scoreVarType)
        {
            int changedScoreValue = 0;

            switch (scoreType)
            {
                case ScoreTypes.DecScore:
                    changedScoreValue = -1;
                    break;
                case ScoreTypes.IncScore:
                    changedScoreValue = 1;
                    break;
                case ScoreTypes.DoubleScore:
                    changedScoreValue = _scoreVariables[(int)scoreVarType];
                    break;
            }

            _scoreVariables[(int)scoreVarType] += changedScoreValue;
            ScoreSignals.Instance.onUpdateScore?.Invoke(_scoreVariables);
        }

        private void OnSendScoreToManagers(GameStates gameState)
        {
            ScoreSignals.Instance.onUpdateScore?.Invoke(_scoreVariables);
        }
    }
}
```