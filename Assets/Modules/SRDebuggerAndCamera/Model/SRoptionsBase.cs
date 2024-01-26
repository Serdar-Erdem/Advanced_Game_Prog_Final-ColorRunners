using System.ComponentModel;
using Modules.SRDebuggerAndCamera.Signals;
using UnityEngine;

namespace Modules.SRDebuggerAndCamera.Model
{
    public class SRoptionsBase:INotifyPropertyChanged
    {
        [Inject] public BaseSrSignals BaseSrSignals { private get; set; }
        private float _x_axis;
        private float _y_axis;
        private float _fov;
        private float _zoom;
        [Category("CameraSettings")]
        [SROptions.NumberRange(-360, 360)]
        public float CameraV
        {
            get { return _x_axis; }
            set
            {
                _x_axis = value;
                BaseSrSignals.UpdateCam.Dispatch(new Vector3(_x_axis, _y_axis));
                OnPropertyChanged("CameraV");
            }
        }

        [Category("CameraSettings")]
        [SROptions.NumberRange(-360, 360)]
        public float CameraH
        {
            get { return _y_axis; }
            set
            {
                _y_axis = value;
                BaseSrSignals.UpdateCam.Dispatch(new Vector2(_x_axis, _y_axis));
                OnPropertyChanged("CameraH");
            }
        }
        

        [Category("CameraSettings")]
        [SROptions.NumberRange(3, 170)]
        public float Fov
        {
            get { return _fov  ; }
            set
            {
                _fov = value;
                BaseSrSignals.UpdateFov.Dispatch(_fov);
                OnPropertyChanged("Fov");
            }
        }
        [Category("CameraSettings")]
        [SROptions.NumberRange(-300, 100)]
        public float Zoom
        {
            get { return _zoom  ; }
            set
            {
                _zoom = value;
                BaseSrSignals.UpdateZoom.Dispatch(_zoom);
                OnPropertyChanged("Zoom");
            }
        }
        [Category("CameraSettings")]
        public void Reset()
        {
            BaseSrSignals.Reset.Dispatch();
        }
        [Category("CameraSettings")]
        public void Main()
        {
            BaseSrSignals.MainCam.Dispatch();
        }
        [Category("CameraSettings")]
        public void FreeCam()
        {
            BaseSrSignals.FreeCam.Dispatch();
        }
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add { PropertyChanged += value; }
            remove { PropertyChanged -= value; }
        }
    }
}