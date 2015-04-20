using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace DrunkiBoy
{
    public class ParticleEngine
    {
        private Random random;       
        private List<Particle> particles;
        private List<Texture2D> textures;
        public bool isActive;

        public ParticleEngine(List<Texture2D> textures, Vector2 location, bool isActive)
        {
            EmitterLocation = location;
            this.isActive = isActive;
            this.textures = textures;
            this.particles = new List<Particle>();
            random = new Random();
        }

        public ParticleEngine():this(new List<Texture2D>(),Vector2.Zero, false)
        {
            
        }

        public void Update(Vector2 pos) // byt namn på metoden / generar partiklarna här
        {
            if (isActive) 
            {
                EmitterLocation = pos;

                int total = 10;

                for (int i = 0; i < total; i++)
                {
                    particles.Add(GenerateNewParticle());

                }
            }
        }
        public void Update() // uppdaterar particlarna
        {

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

        private Particle GenerateNewParticle()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    1f * (float)(random.NextDouble() * 2.5 - 1.5),
                                    1f * (float)(random.NextDouble() * 2.5 - 1.5));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                        (float)random.NextDouble(),
                        (float)random.NextDouble(),
                        (float)random.NextDouble());
            float size = (float)random.NextDouble();
            int ttl = 1 + random.Next(20);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(spriteBatch);
            }
        }
        private Particle GenerateParticleCircleRange()
        {
            Texture2D texture = textures[random.Next(textures.Count)];
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                    1f * (float)(random.NextDouble() * 2 - 1),
                    1f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            Color color = new Color(
                    (float)random.NextDouble(),
                    (float)random.NextDouble(),
                    (float)random.NextDouble());
            float size = (float)(random.NextDouble() * 0.5);
            int ttl = 40 + random.Next(40);

            return new Particle(texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }
        internal void CreateParticlesInCircleRange(Vector2 position)
        {
            int total = 100;
            EmitterLocation = position;

            for (int i = 0; i < total; i++)
            {
                particles.Add(GenerateParticleCircleRange());
            }
        }
        public Vector2 EmitterLocation { get; set; } //propertys
        public List<Texture2D> Textures { get { return textures; } set { this.textures = value; } }
    }
}
