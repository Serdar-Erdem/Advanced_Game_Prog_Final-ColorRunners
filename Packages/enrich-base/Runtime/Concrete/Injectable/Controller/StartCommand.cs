using Rich.Base.Runtime.Concrete.Data.ValueObject;
using Rich.Base.Runtime.Constant;
using Rich.Base.Runtime.Enums;
using strange.extensions.command.impl;

namespace Rich.Base.Runtime.Concrete.Injectable.Controller
{
    /// <summary>
    /// Open home screen command
    /// </summary>
    public class StartCommand : EventCommand
    {
        public override void Execute()
        {
            // When retain is called command will not be finished until release function 
            //Retain();

            dispatcher.Dispatch(ScreenEvent.Initialize);
            
            dispatcher.Dispatch(ScreenEvent.OpenPanel, new PanelVo
            {
                Name = AppScreens.Home
            });

            // Complete the command with Release function
            //Release();
        }
    }
}
