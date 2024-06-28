using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class ParticleSystem : GameObject
    {
        Particle particle;
        Random rand = new Random();

        public ParticleSystem(String imageFileName,float particleAmount,float lifeTime,float size = 1,float rotationOffset = 0) : base()
        {
            for (int i = 0; i < particleAmount; i++)
            {
                particle = new Particle(imageFileName, 360/particleAmount * i, 0, -1 * (float)rand.NextDouble() - 1, lifeTime, size, rotationOffset);
                AddChild(particle);
            }
        }

        void Update()
        {
            
        }
    }
}
