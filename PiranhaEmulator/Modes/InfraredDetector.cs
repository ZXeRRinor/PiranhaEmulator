using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiranhaEmulator.Modes
{
    public class InfraredDetector : PiranhaMode
    {
        public InfraredDetector(MainWindow piranhaWindow) : base(piranhaWindow)
        {
        }

        public override void Init()
        {
            piranhaWindow.Clear();
            piranhaWindow.DrawString(0, 7, "IR-DETECTOR");
            piranhaWindow.DrawCustomCharacter(72, 0, "sound");
            piranhaWindow.DrawCustomCharacter(84, 1, "battery");
            piranhaWindow.DrawString(102, 7, "TONE");
            piranhaWindow.DrawCustomCharacter(0, 10, "ir_semifilled");
            piranhaWindow.DrawCustomCharacter(6, 10, "ir_semifilled");
            piranhaWindow.DrawCustomCharacter(12, 10, "ir_semifilled");
            piranhaWindow.DrawCustomCharacter(18, 12, "ir_delimiter");
            UpdateScreen();
        }

        public override void OnButtonClicked(PiranhaButton button)
        {
        }

        protected override void OnEffectIntensityChanged()
        {
            UpdateScreen();
        }

        private void UpdateScreen()
        {
            for (int i = 0; i < 17; i++)
                piranhaWindow.DrawCustomCharacter(24 + i * 6, 10, i >= effectIntensity * 17 ? "ir_blank" : "ir_filled");
        }
    }
}