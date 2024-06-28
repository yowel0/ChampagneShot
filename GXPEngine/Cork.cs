using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Cork : Sprite
    {
        float velocityYOffset =0;

        float speed = 5;
        public float charge;

        Bottle bottle;
        float bottleHeight;

        Vector2 releasePosition;
        float distance;
        float maxDistance;
        float distanceProgression;

        GameObject[] collisions;
        Particle hole;

        public Cork() : base("Cork.png")
        {
            SetOrigin(width / 2, height / 2);
            SetScaleXY(2);
            releasePosition = new Vector2(x,y - 250);
            alpha = 0;
        }

        void Update()
        {
            bottle = game.FindObjectOfType<Bottle>();
            if(bottle != null && bottleHeight == 0)
            {
                bottleHeight = bottle.height;
            }
            if(parent != null)
            {
                if (parent.GetType().Name != "Bottle")
                {
                    alpha = 1;
                    SetScaleXY(2 + velocityYOffset / 1.5f);
                    Move(0, -speed);
                }
                if (parent.GetType().Name == "Bottle")
                {
                    alpha = 0;
                }
            }
            CheckDistance();
        }

        public void release()
        {
            maxDistance = 100 + 5 * charge;
            //speed = (0.4f * charge + 2)/2;
            speed = charge/100*5 + 5;
            scale = this.parent.scale;
            SetOrigin(parent.x,parent.y);
            rotation = this.parent.rotation;
            SetOrigin(width/2, height/2);
            x += this.parent.x + (Mathf.Cos((rotation - 90) * Mathf.PI / 180) * bottleHeight / 2);
            y += this.parent.y + (Mathf.Sin((rotation - 90) * Mathf.PI/180) * bottleHeight /2) + bottleHeight / 2;
            releasePosition = new Vector2 (x, y);
            //Console.WriteLine((Mathf.Sin((rotation - 90) * Mathf.PI / 180) * bottleHeight));
            //Console.WriteLine(x + "  " + y);
            parent = game.FindObjectOfType<CorkParent>();
        }

        void CheckDistance()
        {
            distance = Mathf.Sqrt((releasePosition.x - x) * (releasePosition.x - x) + (releasePosition.y - y) * (releasePosition.y - y));
            distanceProgression = distance / maxDistance;
            velocityYOffset = -(distanceProgression - 0.5f) * (distanceProgression - 0.5f) * 4 + 1;
            //Console.WriteLine("VelocityOffset:  " + velocityYOffset + "Distance:  " + distance);
            if (distance > maxDistance)
            {
                CollisionCheck();
            }
        }

        void CollisionCheck()
        {
            collisions = GetCollisions();
            for (int i = 0; i < collisions.Length; i++)
            {
                if (collisions[i] is Plate)
                {
                    //Console.WriteLine("PlateHit");
                    Destroy();
                    Plate plate = (Plate)collisions[i];
                    plate.Hit();
                    //collisions[i].Destroy();
                    collisions[i].Destroy();
                    break;
                }
                if (collisions[i].name == "Background.png")
                {
                    //Console.WriteLine("backgroundhid");
                    hole = new Particle("hole.png",0,0,0,1);
                    hole.SetXY(x, y);
                    hole.SetScaleXY(1f);
                    game.AddChild(hole);
                    break;
                }
            }
            //Destroy if it hits nothing
            Destroy();
        }
    }
}
