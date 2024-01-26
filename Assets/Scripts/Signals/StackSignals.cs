using Enums;
using UnityEngine;
using strange.extensions.signal.impl;
using Extentions;

namespace Signals
{
    [Inject]
    public class StackSignals : MonoSingleton<StackSignals>
    {
        private Signal<GameObject> onIncreaseStackSignal = new Signal<GameObject>();
        private Signal<int> onDecreaseStackSignal = new Signal<int>();
        private Signal onStackInitSignal = new Signal();
        private Signal<int> onDecreaseStackRoulletteSignal = new Signal<int>();
        private Signal<int> onDroneAreaSignal = new Signal<int>();
        private Signal<int> onDecreaseStackOnDroneAreaSignal = new Signal<int>();
        private Signal onDoubleStackSignal = new Signal();
        private Signal<ColorTypes> onColorChangeSignal = new Signal<ColorTypes>();
        private Signal<GameObject> onRebuildStackSignal = new Signal<GameObject>();
        private Signal<CollectableAnimationTypes> onAnimationChangeSignal = new Signal<CollectableAnimationTypes>();

        public Signal<GameObject> onIncreaseStack
        {
            get { return onIncreaseStackSignal; }
        }

        public Signal<int> onDecreaseStack
        {
            get { return onDecreaseStackSignal; }
        }

        public Signal onStackInit
        {
            get { return onStackInitSignal; }
        }

        public Signal<int> onDecreaseStackRoullette
        {
            get { return onDecreaseStackRoulletteSignal; }
        }

        public Signal<int> onDroneArea
        {
            get { return onDroneAreaSignal; }
        }

        public Signal<int> onDecreaseStackOnDroneArea
        {
            get { return onDecreaseStackOnDroneAreaSignal; }
        }

        public Signal onDoubleStack
        {
            get { return onDoubleStackSignal; }
        }

        public Signal<ColorTypes> onColorChange
        {
            get { return onColorChangeSignal; }
        }

        public Signal<GameObject> onRebuildStack
        {
            get { return onRebuildStackSignal; }
        }

        public Signal<CollectableAnimationTypes> onAnimationChange
        {
            get { return onAnimationChangeSignal; }
        }

        public void InvokeOnIncreaseStack(GameObject obj)
        {
            onIncreaseStackSignal.Dispatch(obj);
        }

        public void InvokeOnDecreaseStack(int value)
        {
            onDecreaseStackSignal.Dispatch(value);
        }

        public void InvokeOnStackInit()
        {
            onStackInitSignal.Dispatch();
        }

        public void InvokeOnDecreaseStackRoullette(int value)
        {
            onDecreaseStackRoulletteSignal.Dispatch(value);
        }

        public void InvokeOnDroneArea(int value)
        {
            onDroneAreaSignal.Dispatch(value);
        }

        public void InvokeOnDecreaseStackOnDroneArea(int value)
        {
            onDecreaseStackOnDroneAreaSignal.Dispatch(value);
        }

        public void InvokeOnDoubleStack()
        {
            onDoubleStackSignal.Dispatch();
        }

        public void InvokeOnColorChange(ColorTypes color)
        {
            onColorChangeSignal.Dispatch(color);
        }

        public void InvokeOnRebuildStack(GameObject obj)
        {
            onRebuildStackSignal.Dispatch(obj);
        }

        public void InvokeOnAnimationChange(CollectableAnimationTypes animationType)
        {
            onAnimationChangeSignal.Dispatch(animationType);
        }
    }
}
```