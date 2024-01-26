using System.Collections.Generic;
using strange.extensions.signal.impl;
using Extentions;
using Enums;

namespace Signals
{
    [Inject]
    public class ScoreSignals : MonoSingleton<ScoreSignals>
    {
        private Signal<ScoreTypes, ScoreVariableType> onChangeScoreSignal = new Signal<ScoreTypes, ScoreVariableType>();
        private Signal<List<int>> onUpdateScoreSignal = new Signal<List<int>>();
        private Signal onAddLevelToTotalScoreSignal = new Signal();
        private Signal<ScoreVariableType, int> onGetScoreSignal = new Signal<ScoreVariableType, int>();

        public Signal<ScoreTypes, ScoreVariableType> onChangeScore
        {
            get { return onChangeScoreSignal; }
        }

        public Signal<List<int>> onUpdateScore
        {
            get { return onUpdateScoreSignal; }
        }

        public Signal onAddLevelToTotalScore
        {
            get { return onAddLevelToTotalScoreSignal; }
        }

        public Signal<ScoreVariableType, int> onGetScore
        {
            get { return onGetScoreSignal; }
        }

        public void InvokeOnChangeScore(ScoreTypes scoreType, ScoreVariableType variableType)
        {
            onChangeScoreSignal.Dispatch(scoreType, variableType);
        }

        public int InvokeOnGetScore(ScoreVariableType variableType)
        {
            return onGetScoreSignal.Dispatch(variableType);
        }

        public void InvokeOnUpdateScore(List<int> scores)
        {
            onUpdateScoreSignal.Dispatch(scores);
        }

        public void InvokeOnAddLevelToTotalScore()
        {
            onAddLevelToTotalScoreSignal.Dispatch();
        }
    }
}
```