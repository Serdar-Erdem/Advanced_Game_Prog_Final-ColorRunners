using Modules.SRDebuggerAndCamera.Signals;

namespace Modules.SRDebuggerAndCamera.Model
{
    public class SRoptionsProject : SRoptionsBase
    {
        private float _z_axis;
        [Inject]
        public ProjectSRSignals SrSignals { private get; set; }

        
        public void TurnOffBackground()
        {
            SrSignals.CloseBackground.Dispatch();
        }
        public void TurnOnBackground()
        {
            SrSignals.OpenBackground.Dispatch();
        }
        public void TurnOffUiEffects()
        {
            SrSignals.TurnOffEffects.Dispatch();
        }
        public void TurnOnUiEffects()
        {
            SrSignals.TurnOnEffects.Dispatch();
        }
    }
}