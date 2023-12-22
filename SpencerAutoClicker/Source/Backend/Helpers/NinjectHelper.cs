using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace SpencerAutoClicker.Source.Backend.Helpers
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
