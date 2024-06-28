using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class PlateSpawner : GameObject
    {
        Hud hud;
        int difficulty;
        float spawnTimer;
        float spawnTimeMax = 2.5f * 1000;

        float _10sCooldown;
        float _10sCooldownMax = 20 * 1000;
        bool dontspawn10t = false;

        Plate plate;

        public PlateSpawner() : base()
        { 
            hud = game.FindObjectOfType<Hud>();
            spawnTimer = spawnTimeMax;
        }

        void Update()
        {
            if (hud == null)
            {
                hud = game.FindObjectOfType<Hud>();
            }
            difficulty = (int)((hud.gameTimerMax - hud.gameTimer / 1000) / 25);
            spawnTimeMax = (-difficulty * 0.5f + 2.5f) * 1000 ;

            if (_10sCooldown <= 0)
            {
                _10sCooldown = 0;
                dontspawn10t = false;
            }
            else
            {
                dontspawn10t = true;
                _10sCooldown -= Time.deltaTime;
            }

            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0 && hud.gameTimer > 0)
            {
                spawnTimer = spawnTimeMax;
                Console.WriteLine("Spawn");
                plate = new Plate("random" + difficulty, "random" + difficulty, dontspawn10t);
                if (plate.plateType == "10t")
                    _10sCooldown = _10sCooldownMax;

                AddChild(plate);
            }
        }
    }
}
