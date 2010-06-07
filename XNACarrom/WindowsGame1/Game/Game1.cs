using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Camera/View info
        Vector3 cameraPosition = new Vector3(0.0f, 0.0f, GameConstants.CameraHeight);
        Matrix ProjectionMatrix;
        Matrix ViewMatrix;

        //Audio components (eventually)
        /*
        SoundEffect soundEngine;
        SoundEffectInstance soundEngineInstance;
        SoundEffect soundHyperspaceActivation;
        */

        //Visual components
        Striker striker = new Striker();

        Model carromManModel;
        Matrix[] carromManTransforms;
        CarromMan[] carromManList = new CarromMan[GameConstants.NumCarromMen];

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45.0f),
                GraphicsDevice.DisplayMode.AspectRatio,
                GameConstants.CameraHeight - 1000.0f,
                GameConstants.CameraHeight + 1000.0f);
            ViewMatrix = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);

            resetCarromMen();

            base.Initialize();
        }

        private Matrix[] SetupEffectsDefaults(Model myModel)
        {
            Matrix[] absoluteTransforms = new Matrix[myModel.Bones.Count];
            myModel.CopyAbsoluteBoneTransformsTo(absoluteTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.Projection = ProjectionMatrix;
                    effect.View = ViewMatrix;
                }
            }
            return absoluteTransforms;
        }

        private CarromMan createCarromMan(Vector3 loc, Random random)
        {
            CarromMan cm = new CarromMan();

            cm.position = loc;
            double angle = random.NextDouble() * 2 * Math.PI;
            cm.direction.X = -(float)Math.Sin(angle);
            cm.direction.Y = (float)Math.Cos(angle);
            cm.speed = GameConstants.CarromMenMinSpeed +
                0.5f * GameConstants.CarromMenMaxSpeed;
            return cm;
        }

        /// <summary>
        /// Place carromMen in their proper locations, based on location in array
        /// carromManList[0] = queen
        /// [1] to [9] = black pieces
        /// [10] to [18] = white pieces
        /// </summary>
        private void resetCarromMen()
        {
            int i = 0;
            Random random = new Random();
            foreach (Vector3 loc in GameConstants.pieceLocations)
            {
                carromManList[i] = createCarromMan(loc, random);
                i++;
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            striker.Model = this.Content.Load<Model>("Models/p1_wedge");
            striker.Transforms = SetupEffectsDefaults(striker.Model);

            //add the sound code later
            /*
            soundEngine = Content.Load<SoundEffect>("Audio/Waves/engine_2");
            soundEngineInstance = soundEngine.CreateInstance();
            soundHyperspaceActivation =
                Content.Load<SoundEffect>("Audio/Waves/hyperspace_activate");
             */

            carromManModel = Content.Load<Model>("Models/asteroid1");
            carromManTransforms = SetupEffectsDefaults(carromManModel);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            float timeDelta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            //get input
            UpdateInput();

            //add velocity to striker
            striker.Position += striker.Velocity;

            //the striker should slow down over time for various reasons
            striker.Velocity *= 0.95f;

            //update carrom men locations
            for (int i = 0; i < GameConstants.NumCarromMen; i++)
            {
                carromManList[i].Update(timeDelta);
            }

            //check for collisions between striker and carrom-men
            BoundingSphere strikerSphere = new BoundingSphere(
                striker.Position, striker.Model.Meshes[0].BoundingSphere.Radius * GameConstants.strikerBoundingSphereScale);
            for (int i = 0; i < carromManList.Length; i++)
            {
                BoundingSphere cman = new BoundingSphere(carromManList[i].position,
                    carromManModel.Meshes[0].BoundingSphere.Radius *
                    GameConstants.carromManBoundingSphereScale);
                if (cman.Intersects(strikerSphere))
                {
                    //bounce back
                    carromManList[i].direction.X *= -1;
                    carromManList[i].direction.Y *= -1;

                    carromManList[i].position += 10*carromManList[i].direction *
                                                 carromManList[i].speed * GameConstants.CarromMenSpeedAdjustment * timeDelta;
                }
            }

            //check for collisions amongst the carrom-men
            for (int a = 0; a < carromManList.Length; a++)
            {
                BoundingSphere piece1 = new BoundingSphere(
                    carromManList[a].position, carromManModel.Meshes[0].BoundingSphere.Radius *
                    GameConstants.carromManBoundingSphereScale);
                for (int b = a+1; b < carromManList.Length; b++)
                {
                    BoundingSphere piece2 = new BoundingSphere(
                    carromManList[a].position, carromManModel.Meshes[0].BoundingSphere.Radius *
                    GameConstants.carromManBoundingSphereScale);
                    if (piece1.Intersects(piece2))
                    {
                        //bounce back
                        carromManList[a].direction.X *= -1;
                        carromManList[a].direction.Y *= -1;

                        carromManList[a].position += carromManList[a].direction *
                                                 carromManList[a].speed * GameConstants.CarromMenSpeedAdjustment * timeDelta;

                        carromManList[b].direction.X *= -1;
                        carromManList[b].direction.Y *= -1;

                        carromManList[b].position += carromManList[b].direction *
                                                 carromManList[b].speed * GameConstants.CarromMenSpeedAdjustment * timeDelta;
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// Use this method to get input while a shot has been (might not need this?)
        /// </summary>
        protected void UpdateInput()
        {
            //get the controller state
            KeyboardState currentState = Keyboard.GetState();

            //TODO got the state...go do cool stuff.
            striker.Update(currentState);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            Matrix strikerTransformMatrix = striker.RotationMatrix * Matrix.CreateTranslation(striker.Position);
            DrawModel(striker.Model, strikerTransformMatrix, striker.Transforms);

            for (int i = 0; i < GameConstants.NumCarromMen; i++)
            {
                Matrix carromManTransform = Matrix.CreateTranslation(carromManList[i].position);
                DrawModel(carromManModel, carromManTransform, carromManTransforms);
            }

            base.Draw(gameTime);
        }

        public static void DrawModel(Model model, Matrix modelTransform, Matrix[] absoluteBoneTransforms)
        {
            //draw the model; it can have multiple meshes, so iterate
            foreach (ModelMesh mesh in model.Meshes)
            {
                //set the mesh orientation
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.World = absoluteBoneTransforms[mesh.ParentBone.Index] * modelTransform;
                }
                //draw the mesh, it will use the effects set above.
                mesh.Draw();
            }
        }
    }
}
