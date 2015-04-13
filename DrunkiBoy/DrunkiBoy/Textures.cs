﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Textures
    {
        public static Texture2D player, player_lower_body, player_upper_body, player_burger, player_shooting, player_pizza, player_bottle, player_bottle_molotov;

        public static Texture2D platform, background1, background2, deleteCursor, torchTex, Key, heart, bottle, bottle_molotov, hamburgare, pizza, painkiller, jagerbomb, teleport, AktivTeleport, smokeTexture, explosionTexture;
        public static Texture2D healthBarGreen, healthBarRed, healthBarRedBlink, powerUpTimer, testPowerUp, money, pant;

        public static List<Texture2D> heartParticles, smokeParticles, starParticles, pantParticles, painkillerParticles, burgerParticles, pizzaParticles, bottleparticles;

        public static void LoadContent(ContentManager content)
        {
            player = content.Load<Texture2D>("Player/player-animation");
            player_upper_body = content.Load<Texture2D>("Player/player-animation_upper_body");
            player_lower_body = content.Load<Texture2D>("Player/player-animation_lower_body");
            player_burger = content.Load<Texture2D>("Player/player-animation_burger_ub");
            player_shooting = content.Load<Texture2D>("Player/player-animation_burger_shooting");
            player_pizza = content.Load<Texture2D>("Player/player-animation_pizza_ub");
            player_bottle = content.Load<Texture2D>("Player/player-animation_bottle_ub");
            player_bottle_molotov = content.Load<Texture2D>("Player/player-animation_bottle_molotov_ub");
            platform = content.Load<Texture2D>("platform");
            deleteCursor = content.Load<Texture2D>("deleteCursor");
            background1 = content.Load<Texture2D>("space");
            background2 = content.Load<Texture2D>("bg2");
            torchTex = content.Load<Texture2D>("Torch4");
            Key = content.Load<Texture2D>("Key");
            heart = content.Load<Texture2D>("Extralif2");
            bottle = content.Load<Texture2D>("flaska");
            bottle_molotov = content.Load<Texture2D>("bottle_molotov");
            hamburgare = content.Load<Texture2D>("burgare");
            pizza = content.Load<Texture2D>("Pizza");
            painkiller = content.Load<Texture2D>("painkiller");
            jagerbomb = content.Load<Texture2D>("jagerbomb");
            teleport = content.Load<Texture2D>("teleport2");
            AktivTeleport = content.Load<Texture2D>("TeleportAktiv");
            healthBarGreen = content.Load<Texture2D>("GUI/healthBarGreen");
            healthBarRed = content.Load<Texture2D>("GUI/healthBarRed");
            healthBarRedBlink = content.Load<Texture2D>("GUI/healthBarRed_blink");
            powerUpTimer = content.Load<Texture2D>("GUI/powerUpTimerAnimation");
            testPowerUp = content.Load<Texture2D>("GUI/testPowerUp");
            money = content.Load<Texture2D>("Money");
            pant = content.Load<Texture2D>("Pant");
            smokeTexture = content.Load<Texture2D>("Particles/smoke_texture");
            explosionTexture = content.Load<Texture2D>("Particles/explosion_texture");
            heartParticles = new List<Texture2D>();
            heartParticles.Add(content.Load<Texture2D>("Particles/heart_particle"));
            heartParticles.Add(content.Load<Texture2D>("Particles/heart_particle2"));
            starParticles = new List<Texture2D>();
            starParticles.Add(content.Load<Texture2D>("star"));
            pantParticles = new List<Texture2D>();
            pantParticles.Add(content.Load<Texture2D>("pant_Particle"));
            smokeParticles = new List<Texture2D>();
            smokeParticles.Add(content.Load<Texture2D>("Particles/smoke_particle"));
            painkillerParticles = new List<Texture2D>();
            painkillerParticles.Add(content.Load<Texture2D>("Particles/Painkiller_Particle"));
            burgerParticles = new List<Texture2D>();
            burgerParticles.Add(content.Load<Texture2D>("Particles/burger_particles"));
            pizzaParticles = new List<Texture2D>();
            pizzaParticles.Add(content.Load<Texture2D>("Particles/pizza_particles"));
            bottleparticles = new List<Texture2D>();
            bottleparticles.Add(content.Load<Texture2D>("Particles/bottle_particles"));

            
            //smokeParticles.Add(content.Load<Texture2D>("Particles/smoke_particle2"));
        }
    }
}
