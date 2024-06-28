using System;

namespace GXPEngine
{
    internal class Bottle : Sprite
    {
        float controllerYaw;
        public bool ammoFull;
        public float charge;
        public float maxCharge = 100;
        float chargeSpeed = 1;

        Cork cork;

        Sprite impactPosition;

        AnimationSprite visualBottle;

        SoundManager soundManager;

        public Bottle() : base("Bottle240.png")
        {
            alpha = 0;
            SetOrigin(width / 2, height / 2);
            SetScaleXY(1f);
            SetXY(game.width / 2, game.height - height / 5f);
            impactPosition = new Sprite("Crosshair.png");
            impactPosition.SetOrigin(impactPosition.width / 2, impactPosition.height / 2);
            impactPosition.SetScaleXY(1.5f);
            impactPosition.SetXY(0, -height / 2);
            AddChild(impactPosition);
            visualBottle = new AnimationSprite("BottleSpritesheet.png", 3, 1);
            visualBottle.SetOrigin(visualBottle.width / 2, visualBottle.height / 2);
            visualBottle.SetScaleXY(2);
            visualBottle.SetXY(-45, -100);
            AddChild(visualBottle);
            visualBottle.SetCycle(2);
            soundManager = game.FindObjectOfType<SoundManager>();
        }

        void Update()
        {
            
            //rotation = Mathf.Clamp(Controller.main.roll, -75, 75); //  controllerYaw;
            rotation = controllerYaw = Mathf.Clamp(controllerYaw, -75f, 75f);
            //crosshair
            impactPosition.SetXY(0, -height / 2 - (charge * 5 + 100));

            if (Input.GetKey(Key.RIGHT))
            {
                controllerYaw++;
            }
            if (Input.GetKey(Key.LEFT))
            {
                controllerYaw--;
            }
            if (Input.GetKeyDown(Key.R))
            {
                Reload();
            }
            if (Input.GetKey(Key.SPACE))
            {
                Charge(1);
            }
            if (Input.GetKeyUp(Key.SPACE))
            {
                Shoot();
                charge = 0;
            }
            if (Controller.main.pitch != 3000)
            {
                Reload();
            }
            if (Controller.main.buttonHELD)
            {
                Charge(Controller.main.acceleration);
            }
            if (Controller.main.buttonUP)
            {
                Shoot();
                charge = 0;
            }

            //--------------------------
            if (ammoFull)
            {
                visualBottle.SetCycle(0);
            }
            if (!ammoFull)
            {
                visualBottle.SetCycle(1, 2);
                if (visualBottle.currentFrame == 1)
                {
                    visualBottle.Animate(.03f);
                }
            }

        }

        void Shoot()
        {
            if (ammoFull)
            {
                soundManager.PlaySound("cork");
                cork.charge = charge;
                cork.release();
                ammoFull = false;
            }
        }

        void Reload()
        {
            if (ammoFull == false && visualBottle.currentFrame == 2)
            {
                cork = new Cork();
                //cork.SetXY(cork.x, cork.y - height/2);
                cork.Move(0, -height / 2);
                AddChild(cork);
                ammoFull = true;
            }
        }

        void Charge(float acceleration)
        {
            if (ammoFull)
            {
                if (charge < maxCharge)
                {
                    charge += acceleration * chargeSpeed;
                }
                else
                {
                    charge = maxCharge;
                }
            }
        }
    }
}
