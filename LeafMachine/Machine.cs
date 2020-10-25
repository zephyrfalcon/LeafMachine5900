using LeafMachine.Aphid;
using LeafMachine.Aphid.Types;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace LeafMachine
{
    public class Machine : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private MachineState state;
        private AphidInterpreter intp;

        public Machine()
        {
            intp = new AphidInterpreter();
            _graphics = new GraphicsDeviceManager(this);
            state = new MachineState(_graphics);
            LoadBuiltinLeafWords(intp, state);

            // not sure about these
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        void LoadBuiltinLeafWords(AphidInterpreter aip, MachineState state)
        {
            LeafBuiltinWords bw = new LeafBuiltinWords();
            foreach(KeyValuePair<string, DelAphidLeafBuiltinWord> entry in bw.GetBuiltinWords()) {
                aip.LoadBuiltin(entry.Key, new AphidLeafBuiltinWord(state, entry.Key, entry.Value));
            }
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
