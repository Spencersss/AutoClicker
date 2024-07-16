using Ninject;

namespace SpencerAutoClicker.Source.Model.Helpers
{
    public static class NinjectHelper
    {
        public static IKernel Kernel { get; private set; }

        static NinjectHelper()
        {
            Kernel = new StandardKernel();
        }
    }
}
