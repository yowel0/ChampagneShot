using GXPEngine.Core;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    public class Plate : Sprite
    {
        MyGame myGame;

        Random random = new Random();

        ParticleSystem psShards;
        ParticleSystem psPoints;
        String plateShardFileName = "PlateShard.png";
        String pointsFileName;

        int platePoints = 0;
        int plateTime = 0;
        int plateMultiplier = 1;
        float plateShardSize = 1;
        float plateRotationOffset = 0;

        float CooldownMax = 20 * 1000;

        public String plateType;
        String[] plateTypes = {"-5", "-10", "-20", "5", "10", "20", "-10t", "10t", "gold"};
        String behavior;
        String[] behaviors = { "static", "horizontal", "vertical", "hyperbolic" };

      //String[] typeRandom0 = { "-5", "-10", "-20", "5", "10", "20", "-10t", "10t", "gold" };
        String[] typeRandom0 = { "-5", "-10",        "5", "10",               "10t"};
        String[] behaviorsRandom0 = { "static"};

        String[] typeRandom1 = { "-5", "-10",        "5", "10", "20",         "10t" };
        String[] behaviorsRandom1 = { "static", "horizontal", "vertical" };

        String[] typeRandom2 = { "-5", "-10", "-20", "5", "10", "20", "-10t", "10t" };
        String[] behaviorsRandom2 = { "static", "horizontal", "vertical", "hyperbolic" };

        String[] typeRandom3 = { "-5", "-10", "-20", "5", "10", "20", "10t", "-10t", "gold" };
        String[] behaviorsRandom3 = { "static", "horizontal", "vertical", "hyperbolic" };

        String direction;
        int moveSpeed = 5;
        int desiredY;
        float lifeTime;

        float timeAlife;

        Hud hud;
        SoundManager soundManager;

        public Plate(String nPlateType = "", String nBehavior = "", bool dontspawn10t = false) : base("Plate.png")
        {
            myGame = (MyGame)game;
            soundManager = game.FindObjectOfType<SoundManager>();

            plateType = nPlateType;
            behavior = nBehavior;
            SetOrigin(width/2, height/2);
            if (plateType == "random" || plateType == ""){
                plateType = plateTypes[random.Next(plateTypes.Length)];
            }
            if (behavior == "random")
            {
                behavior = behaviors[random.Next(behaviors.Length)];
            }
            switch (plateType)
            {
                case "random0":
                    plateType = typeRandom0[random.Next(typeRandom0.Length)];
                    Console.WriteLine(plateType);
                    break;
                case "random1":
                    plateType = typeRandom1[random.Next(typeRandom1.Length)];
                    break;
                case "random2":
                    plateType = typeRandom2[random.Next(typeRandom2.Length)];
                    break;
                case "random3":
                    plateType = typeRandom3[random.Next(typeRandom3.Length)];
                    break;
            }
            switch (plateType)
            {
                case "1":
                    _texture = new Texture2D("Plate.png");
                    break;
                case "-5":
                    _texture = new Texture2D("Plates/-5Plate.png");
                    pointsFileName = "Plates/-500.png";
                    plateShardFileName = "Plates/redShard.png";
                    plateShardSize = 3.125f;
                    platePoints = -500;
                    break;
                case "-10":
                    _texture = new Texture2D("Plates/-10Plate.png");
                    pointsFileName = "Plates/-1000.png";
                    plateShardFileName = "Plates/redShard.png";
                    plateShardSize = 3.125f;
                    platePoints = -1000;
                    break;
                case "-20":
                    _texture = new Texture2D("Plates/-20Plate.png");
                    pointsFileName = "Plates/-2000.png";
                    plateShardFileName = "Plates/redShard.png";
                    plateShardSize = 3.125f;
                    platePoints = -2000;
                    break;
                case "5":
                    _texture = new Texture2D("Plates/5Plate.png");
                    pointsFileName = "Plates/500.png";
                    plateShardFileName = "Plates/greenShard.png";
                    plateShardSize = 3.125f;
                    platePoints = 500;
                    break;
                case "10":
                    _texture = new Texture2D("Plates/10Plate.png");
                    pointsFileName = "Plates/1000.png";
                    plateShardFileName = "Plates/greenShard.png";
                    plateShardSize = 3.125f;
                    platePoints = 1000;
                    break;
                case "20":
                    _texture = new Texture2D("Plates/20Plate.png");
                    pointsFileName = "Plates/2000.png";
                    plateShardFileName = "Plates/greenShard.png";
                    plateShardSize = 3.125f;
                    platePoints = 2000;
                    break;
                case "-10t":
                    _texture = new Texture2D("Plates/-10tPlate.png");
                    pointsFileName = "Plates/-10t.png";
                    plateShardFileName = "Plates/redShard.png";
                    plateShardSize = 3.125f;
                    plateTime = -10;
                    break;
                case "10t":
                    if (dontspawn10t)
                    {
                        _texture = new Texture2D("Plates/10Plate.png");
                        pointsFileName = "Plates/1000.png";
                        plateShardFileName = "Plates/greenShard.png";
                        plateShardSize = 3.125f;
                        platePoints = 1000;
                    }
                    else
                    {
                        _texture = new Texture2D("Plates/10tPlate.png");
                        pointsFileName = "Plates/10t.png";
                        plateShardFileName = "Plates/blueShard.png";
                        plateShardSize = 3.125f;
                        plateTime = 10;
                    }
                    break;
                case "gold":
                    _texture = new Texture2D("Plates/GoldPlate.png");
                    pointsFileName = "Plates/x2.png";
                    plateShardFileName = "Plates/goldShard.png";
                    plateShardSize = 3.125f;
                    plateMultiplier = 2;
                    break;
                case "Menu":
                    _texture = new Texture2D("Plates/MenuPlate.png");
                    plateShardFileName = "Plates/redShard.png";
                    SetXY(game.width / 2, game.height / 2);
                    SetScaleXY(4.5f);
                    plateShardSize = 4.5f * 3.125f;
                    break;
            }
            switch (behavior)
            {
                case "random0":
                    behavior = behaviorsRandom0[random.Next(behaviorsRandom0.Length)];
                    Console.WriteLine(behavior);
                    break;
                case "random1":
                    behavior = behaviorsRandom1[random.Next(behaviorsRandom1.Length)];
                    break;
                case "random2":
                    behavior = behaviorsRandom2[random.Next(behaviorsRandom2.Length)];
                    break;
                case "random3":
                    behavior = behaviorsRandom3[random.Next(behaviorsRandom3.Length)];
                    break;
            }
            switch (behavior)
            {
                case "static":
                    SetXY(random.Next(300,game.width-300), game.height);
                    desiredY = random.Next(300,game.height - 450);
                    lifeTime = 5 * 1000;
                    direction = "up";
                    moveSpeed = 9;
                    break;
                case "horizontal":
                    if (random.Next(2) == 0) {
                        x = 0 - 100;
                        direction = "right";
                    }
                    else {
                         x = game.width + 100;
                        direction = "left";
                    }
                    y = random.Next( 300, game.height - 450);
                    break;
                case "vertical":
                    y = 0 - 100;
                    direction = "down";
                    x = random.Next(100, game.width - 100);
                    break;
                case "hyperbolic":
                    x = random.Next(100, game.width - 100);
                    if (x < game.width / 2)
                        direction = "right";
                    else
                        direction = "left";
                    y = game.height;
                    break;
            }
        }

        void Update()
        {
            timeAlife += Time.deltaTime;
            if (behavior == "horizontal" || behavior == "vertical" || behavior == "hyperbolic")
            {
                if (direction == "right")
                    x += moveSpeed;
                if (direction == "left")
                    x -= moveSpeed;
                if (direction == "up")
                    y -= moveSpeed;
                if (direction == "down")
                    y += moveSpeed;
            }
            if (behavior == "static"){
                if (direction == "up"){
                    if (y > desiredY)
                    {
                        y -= moveSpeed;
                    }
                    else
                        direction = "down";
                }
                if (direction == "down")
                {
                    lifeTime -= Time.deltaTime;
                    if (lifeTime < 0)
                    {
                        lifeTime = 0;
                        y += moveSpeed;
                    }
                }
            }
            if (behavior == "horizontal")
            {
                if (x < - 100 || x > game.width + 100){
                    Destroy();
                }
            }
            if (behavior == "vertical")
            {
                if (y < - 100 || y > game.height + 100){
                    Destroy();
                }
            }
            if (behavior == "hyperbolic"){
                y = -1000 * Mathf.Sin((float)timeAlife / 1000) + game.height;
                if (y > game.height + 50) {
                    Destroy();
                }
            }
        }

        public void Hit()
        {
            soundManager.PlaySound("plate");
            if (plateType == "Menu")
            {
                //Console.WriteLine("MENUPLATEHIT");
                myGame.LoadLevel("Game");
            }

            hud = game.FindObjectOfType<Hud>();

            psShards = new ParticleSystem(plateShardFileName, 8, 0.5f, plateShardSize, plateRotationOffset);
            psShards.SetXY(x, y);
            game.AddChild(psShards);
            if (pointsFileName != null)
            {
                psPoints = new ParticleSystem(pointsFileName, 1, 1f,5);
                psPoints.SetXY(x, y);
                game.AddChild(psPoints);
            }
            if (platePoints != 0 && hud != null){
                hud.AddScore(platePoints);
            }
            if (plateTime != 0 && hud != null)
            {
                hud.AddTime(plateTime);
            }
            if (plateMultiplier == 2 && hud != null)
            {
                hud.SetMultiplier(2);
            }
            Destroy();
        }
    }
}
