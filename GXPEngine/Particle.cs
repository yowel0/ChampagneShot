using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Particle : Sprite
    {
        float xSpeed;
        float ySpeed;
        float timer;
        float maxTimer;
        float rotationOffset;


        public Particle(String imageFileName, float nRotation, float vXSpeed, float vYSpeed, float lifeTime,float size = 1, float nRotationOffset = 0) : base(imageFileName)
        {
            SetOrigin(width/2, height/2);
            SetScaleXY(size);
            rotation = nRotation + rotationOffset;
            xSpeed = vXSpeed;
            ySpeed = vYSpeed;
            timer = lifeTime * 1000;
            maxTimer = timer;
            rotationOffset = nRotationOffset;
        }

        void Update()
        {
            Move(xSpeed ,ySpeed);
            timer -= Time.deltaTime;
            alpha = timer/ maxTimer;
            if (alpha <= 0)
            {
                Destroy();
            }
        }
    }
}
