using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{   
    public class SoundManager : GameObject
    {
        Random random = new Random();

        Sound corkPop1 = new Sound("Sounds/CorkPop1.wav");
        Sound corkPop2 = new Sound("Sounds/CorkPop2.wav");
        Sound corkPop3 = new Sound("Sounds/CorkPop3.wav");
        Sound[] corkSounds = new Sound[3];

        Sound plateCrash1 = new Sound("Sounds/PlateSmash1.wav");
        Sound plateCrash2 = new Sound("Sounds/PlateSmash2.wav");
        Sound plateCrash3 = new Sound("Sounds/PlateSmash3.wav");
        Sound plateCrash4 = new Sound("Sounds/PlateSmash4.wav");
        Sound plateCrash5 = new Sound("Sounds/PlateSmash5.wav");
        Sound[] plateCrashes = new Sound[5];

        Sound theme = new Sound("Sounds/Clownassbeat4.wav");


        public SoundManager() : base()
        {
            corkSounds = new Sound[3] { corkPop1, corkPop2, corkPop3 };
            plateCrashes = new Sound[5] { plateCrash1, plateCrash2, plateCrash3, plateCrash4, plateCrash5 };
        }

        public void PlaySound(string soundString)
        {
            switch (soundString)
            {
                case "cork":
                    corkSounds[random.Next(corkSounds.Length)].Play();
                    break;
                case "plate":
                    plateCrashes[random.Next(plateCrashes.Length)].Play();
                    break;
                case "theme":
                    theme.Play();
                    break;
            }
        }
    }
}
