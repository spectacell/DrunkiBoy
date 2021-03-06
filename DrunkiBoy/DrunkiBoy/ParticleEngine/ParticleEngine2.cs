﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    public class ParticleEngine2
    {
        public bool isActive;
        public float width, height;
        Texture2D colorTexture; //Textur att slumpa färger ur
        Color[] colors;
        private Random random; // random number generator
        public Vector2 EmitterLocation { get; set; } // changing the location there the particles will be generating
        public List<Particle> particles;
        private List<Texture2D> textures;
        public ParticleEngine2(List<Texture2D> textures, Vector2 location, float width, float height, Texture2D colorTexture, bool isActive)
        {
            this.width = width;
            this.height = height;
            EmitterLocation = location;
            this.isActive = isActive;            
            this.colorTexture = colorTexture;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
            // Get color data from colors texture
            colors = new Color[colorTexture.Width * colorTexture.Height];
            colorTexture.GetData(colors);
        }
        public ParticleEngine2(Texture2D texture, Vector2 location, float width, float height, Texture2D colorTexture, bool isActive)
        {
            this.width = width;
            this.height = height;
            EmitterLocation = location;
            this.isActive = isActive;
            this.colorTexture = colorTexture;
            textures = new List<Texture2D>();
            textures.Add(texture);
            this.particles = new List<Particle>();
            random = new Random();
            // Get color data from colors texture
            colors = new Color[colorTexture.Width * colorTexture.Height];
            colorTexture.GetData(colors);
        }
        private Particle GenerateNewParticle(bool down)
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = new Vector2(EmitterLocation.X + width * (float)(random.NextDouble() * 2 - 1), EmitterLocation.Y);
            Vector2 velocity;
            int ttl;
            if (down) 
            {
                velocity = new Vector2((float)(random.NextDouble() * 2 - 1)/3, (float)(random.NextDouble() * 10));
                ttl = random.Next(10);
            }
            else
            {
                velocity = new Vector2((float)(random.NextDouble() * 2 - 1) / 3, - Math.Abs((float)(random.NextDouble() * 2 - 1) * height));
                velocity.Y += -0.3f;
                ttl = 5+random.Next(40);
            }
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = colors[(random.Next(0, colorTexture.Height) * colorTexture.Width) +
                                  random.Next(0, colorTexture.Width)]; //Slumpar fram en färg från colorTexture
            float size = (float)random.NextDouble()/5;
            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
       
        public void Update()
        {
            if (isActive)
            {
                int total = 20;

                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle(false));                  
                }
            }
                for (int particle = 0; particle < particles.Count; particle++)
                {
                    particles[particle].Update();
                    if (particles[particle].TTL <= 0)
                    {
                        particles.RemoveAt(particle);
                        particle--;
                    }
                }
        }
        public void Update(Vector2 pos, bool down)
        {
            if (isActive)
            {
                EmitterLocation = pos;
                int total = 30;

                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle(down));
                }
            }
            for (int particle = 0; particle < particles.Count; particle++)
            {
                particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    particles.RemoveAt(particle);
                    particle--;
                }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (isActive) 
            { 
                for (int index = 0; index < particles.Count; index++)
			    {
			        particles[index].Draw(spriteBatch);
			    }
            }
        }

    }
}
