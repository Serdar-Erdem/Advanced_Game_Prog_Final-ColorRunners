using Rich.Base.Runtime.Concrete.Context;

namespace Rich.Base.Runtime.Concrete.Root
{
    public class DefaultUICanvasRoot : RichMVCContextRoot
    {
        protected override void InitializeContext()
        {
            context = new BaseUIContext(gameObject);
            context.Start();
            context.Launch();
        }
    }
}