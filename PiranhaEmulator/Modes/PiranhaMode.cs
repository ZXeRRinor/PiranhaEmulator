using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiranhaEmulator.Modes
{
    public abstract class PiranhaMode
    {
        protected MainWindow piranhaWindow;
        protected double effectIntensity;

        protected PiranhaMode(MainWindow piranhaWindow)
        {
            this.piranhaWindow = piranhaWindow;
        }

        public abstract void Init();
        
        public abstract void OnButtonClicked(PiranhaButton button);

        public void OnEffectIntensityChangedHandler(double newIntensity)
        {
            this.effectIntensity = newIntensity;
            OnEffectIntensityChanged();
        }

        protected abstract void OnEffectIntensityChanged();
    }
}
