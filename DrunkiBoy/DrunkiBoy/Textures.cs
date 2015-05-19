using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DrunkiBoy
{
    class Textures
    {
        public static Texture2D player, player_lower_body, player_upper_body, player_upper_body_hurt, player_burger, player_shooting, player_pizza, player_bottle, DoorOpened, MovingplatfForeditor, wall, ActiveButton,
            player_bottle_molotov, player_kebab, player_head, player_jetpack, player_jetpack_morph, player_invincible, player_invincible_morph, player_invincible_shooting, player_invincible_collision, Button,
            DoorClosed;

        public static Texture2D platform, deleteCursor, torchTex, Key, Wallet, Cellphone, heart, bottle, bottle_molotov, hamburgare, pizza, kebab, painkiller, jagerbomb, redbullVodka, 
            bubble_particle, water_texture, teleport, AktivTeleport, smokeTexture, explosionTexture, toilet_open, toilet_closed, angry_neighbour,
            angry_neighbour_HB_red, angry_neighbour_HB_green, angry_neighbour_HB_blink, fire, bar, barWithoutBurgers, barWithoutKebab, barWithoutBottles;
        public static Texture2D healthBarGreen, healthBarRed, healthBarRedBlink, powerUpTimer, vodkaPowerup, redbullVodkaPowerup, money, pant, vodka,
                                notesOne, notesTwo, notesThree, notesFour;
        public static Texture2D flashlight, radio, redbullVodka_gui, vodka_gui, GUIPizza;
        public static Texture2D gameOverScreen, startButton, exitButton, menuBackground, instructionsButton, menuInstructionsPage, highscoreButton, levelEditorButton;
        public static List<Texture2D> heartParticles, smokeParticles, starParticles, pantParticles, painkillerParticles, burgerParticles, pizzaParticles, bottleparticles, kebabParticles;

        private static List<Texture2D> backgroundsLevel1 = new List<Texture2D>();
        private static List<Texture2D> backgroundsLevel2 = new List<Texture2D>();

        public static List<List<Texture2D>> levelBackgrounds = new List<List<Texture2D>>();
        
        public static void LoadContent(ContentManager content)
        {
            player = content.Load<Texture2D>("Player/player-animation");
            player_upper_body = content.Load<Texture2D>("Player/player-animation_upper_body");
            player_upper_body_hurt = content.Load<Texture2D>("Player/player-animation_upper_body_hurt");
            player_lower_body = content.Load<Texture2D>("Player/player-animation_lower_body");
            player_burger = content.Load<Texture2D>("Player/player-animation_burger_ub");
            player_shooting = content.Load<Texture2D>("Player/player-animation_burger_shooting");
            player_pizza = content.Load<Texture2D>("Player/player-animation_pizza_ub");
            player_bottle = content.Load<Texture2D>("Player/player-animation_bottle_ub");
            player_bottle_molotov = content.Load<Texture2D>("Player/player-animation_bottle_molotov_ub");
            player_kebab = content.Load<Texture2D>("Player/player-animation_kebab_ub");
            player_head = content.Load<Texture2D>("Player/player-animation_head");
            player_jetpack = content.Load<Texture2D>("Player/player-animation_jetpack");
            player_jetpack_morph = content.Load<Texture2D>("Player/player-animation_jetpack_morph");
            player_invincible = content.Load<Texture2D>("Player/player-animation_invincible");
            player_invincible_morph = content.Load<Texture2D>("Player/player-animation_invincible_morph");
            player_invincible_shooting = content.Load<Texture2D>("Player/player-animation_invincible_shooting");
            player_invincible_collision = content.Load<Texture2D>("Player/player-animation_invincible_collision");
            angry_neighbour = content.Load<Texture2D>("Enemies/angry_neighbour-animation");
            angry_neighbour_HB_red = content.Load<Texture2D>("Enemies/angry_neighbour-HB_red");
            angry_neighbour_HB_green = content.Load<Texture2D>("Enemies/angry_neighbour-HB_green");
            angry_neighbour_HB_blink = content.Load<Texture2D>("Enemies/angry_neighbour-HB_blink");

            platform = content.Load<Texture2D>("Level/platform");
            MovingplatfForeditor = content.Load<Texture2D>("Level/movingPlatfLeditor");
            flashlight = content.Load<Texture2D>("Enemies/Flashlight(Animation)");
            radio = content.Load<Texture2D>("Enemies/Radio");
            notesOne = content.Load<Texture2D>("Enemies/NotesOne");
            notesTwo = content.Load<Texture2D>("Enemies/NotesTwo");
            notesThree = content.Load<Texture2D>("Enemies/NotesThree");
            notesFour = content.Load<Texture2D>("Enemies/NotesFour");
            deleteCursor = content.Load<Texture2D>("deleteCursor");
            //menu texture
            startButton = content.Load<Texture2D>("Menu/Start");
            exitButton = content.Load<Texture2D>("Menu/Exit");
            instructionsButton = content.Load<Texture2D>("Menu/instructionsButton");
            menuBackground = content.Load<Texture2D>("Menu/menuBackground");
            menuInstructionsPage = content.Load<Texture2D>("Menu/instructions");
            highscoreButton = content.Load<Texture2D>("Menu/highscoreButton");
            levelEditorButton = content.Load<Texture2D>("Menu/levelEditorButton");
            //Bakgrunder för level 1
            backgroundsLevel1.Add(content.Load<Texture2D>("Level/space"));
            backgroundsLevel1.Add(content.Load<Texture2D>("Level/bg2"));
            levelBackgrounds.Add(backgroundsLevel1);

            //Lägg in bakgrunder för Level 2 här
            backgroundsLevel2.Add(content.Load<Texture2D>("Level/space"));
            backgroundsLevel2.Add(content.Load<Texture2D>("Level/bg2"));
            levelBackgrounds.Add(backgroundsLevel2);

            torchTex = content.Load<Texture2D>("Items/Torch4");
            Key = content.Load<Texture2D>("Items/Key");
            Wallet = content.Load<Texture2D>("Items/wallet");
            Cellphone = content.Load<Texture2D>("Items/cellphone");
            heart = content.Load<Texture2D>("Items/Extralif2");
            bottle = content.Load<Texture2D>("Items/flaska");
            bottle_molotov = content.Load<Texture2D>("Items/bottle_molotov");
            fire = content.Load<Texture2D>("Items/fire");
            hamburgare = content.Load<Texture2D>("Items/burgare");
            pizza = content.Load<Texture2D>("Items/Pizza");
            wall = content.Load<Texture2D>("Items/wall");
            Button = content.Load<Texture2D>("Items/doorButton");
            ActiveButton = content.Load<Texture2D>("Items/ActiveButton");
            DoorClosed = content.Load<Texture2D>("Items/SpacedoorClosed");
            DoorOpened = content.Load<Texture2D>("Items/SpaceDoorOpen");
            kebab = content.Load<Texture2D>("Items/kebab");
            vodka = content.Load<Texture2D>("Items/Vodka");
            vodka_gui = content.Load<Texture2D>("GUI/Vodka");
            bar = content.Load<Texture2D>("Items/Bar");
            barWithoutBurgers = content.Load<Texture2D>("Items/Bar_without_burgers");
            barWithoutBottles = content.Load<Texture2D>("Items/Bar_without_bottles");
            barWithoutKebab = content.Load<Texture2D>("Items/Bar_without_kebab");
            redbullVodka = content.Load<Texture2D>("Items/RedbullVodka");
            redbullVodka_gui = content.Load<Texture2D>("GUI/RedbullVodka");
            painkiller = content.Load<Texture2D>("Items/painkiller");
            jagerbomb = content.Load<Texture2D>("Items/jagerbomb");
            teleport = content.Load<Texture2D>("Items/garbagecan");
            AktivTeleport = content.Load<Texture2D>("Items/TeleportAktiv");
            healthBarGreen = content.Load<Texture2D>("GUI/healthBarGreen");
            healthBarRed = content.Load<Texture2D>("GUI/healthBarRed");
            healthBarRedBlink = content.Load<Texture2D>("GUI/healthBarRed_blink");
            powerUpTimer = content.Load<Texture2D>("GUI/powerUpTimerAnimation");
            vodkaPowerup = content.Load<Texture2D>("Items/Vodka");
            redbullVodkaPowerup = content.Load<Texture2D>("Items/RedbullVodka");
            money = content.Load<Texture2D>("Items/Money");
            pant = content.Load<Texture2D>("Items/Pant");
            toilet_open = content.Load<Texture2D>("Items/toilet_open");
            toilet_closed = content.Load<Texture2D>("Items/toilet_closed");
            gameOverScreen = content.Load<Texture2D>("Menu/GameOver");
            bubble_particle = content.Load<Texture2D>("Particles/bubble_particle");
            smokeTexture = content.Load<Texture2D>("Particles/smoke_texture");
            explosionTexture = content.Load<Texture2D>("Particles/explosion_texture");
            water_texture = content.Load<Texture2D>("Particles/water_texture");
            heartParticles = new List<Texture2D>();
            heartParticles.Add(content.Load<Texture2D>("Particles/heart_particle"));
            heartParticles.Add(content.Load<Texture2D>("Particles/heart_particle2"));
            starParticles = new List<Texture2D>();
            starParticles.Add(content.Load<Texture2D>("Particles/star"));
            pantParticles = new List<Texture2D>();
            pantParticles.Add(content.Load<Texture2D>("Particles/pant_Particle"));
            smokeParticles = new List<Texture2D>();
            smokeParticles.Add(content.Load<Texture2D>("Particles/smoke_particle"));
            //smokeParticles.Add(content.Load<Texture2D>("Particles/smoke_particle2"));
            painkillerParticles = new List<Texture2D>();
            painkillerParticles.Add(content.Load<Texture2D>("Particles/Painkiller_Particle"));
            burgerParticles = new List<Texture2D>();
            burgerParticles.Add(content.Load<Texture2D>("Particles/burger_particles"));
            pizzaParticles = new List<Texture2D>();
            pizzaParticles.Add(content.Load<Texture2D>("Particles/pizza_particles"));
            bottleparticles = new List<Texture2D>();
            bottleparticles.Add(content.Load<Texture2D>("Particles/bottle_particles"));
            kebabParticles = new List<Texture2D>();
            kebabParticles.Add(content.Load<Texture2D>("Particles/Kebab_particles"));
            GUIPizza = content.Load<Texture2D>("Particles/pizza_particles");
        }
    }
}
