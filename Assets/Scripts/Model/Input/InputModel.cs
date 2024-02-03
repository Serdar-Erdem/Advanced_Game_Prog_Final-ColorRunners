using Runtime.Data.UnityObject;
using UnityEngine;

namespace Runtime.Model.Input
{
    public class InputModel : IInputModel
    {
        private CD_Input _inputData { get; set; }

        private const string InputDataPath = "Data/CD_Input";

        [PostConstruct]
        public void OnLoadInputData()
        {
            _inputData = Resources.Load<CD_Input>(InputDataPath);
        }

        public CD_Input InputData
        {
            get
            {
                if (_inputData == null)
                {
                    OnLoadInputData();
                }

                return _inputData;
            }
            set => _inputData = value;
        }
    }
}