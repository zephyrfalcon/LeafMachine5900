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
            _graphics = new GraphicsDeviceManager(this);

            // NOTE: MachineState, AphidInterpreter etc are loaded in Initialize()

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
            base.Initialize();
            _graphics.PreferredBackBufferWidth = 320;
            _graphics.PreferredBackBufferHeight = 200;
            _graphics.ApplyChanges();

            intp = new AphidInterpreter();
            state = new MachineState(_graphics);
            LoadBuiltinLeafWords(intp, state);

            // test test...
            state.SetChar(0, 0, 'A');
            state.SetChar(1, 0, 'B');
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
            GraphicsDevice.Clear(state.palette[state.bgColor]);

            _spriteBatch.Begin();
            // plot characters
            for (int x = 0; x < MachineState.WIDTH; x++)
                for (int y = 0; y < MachineState.HEIGHT; y++) {
                    char c = state.chars[x, y];
                    GraphicChar gc = state.graphicChars[c];
                    int colornum = state.fgcolors[x, y];
                    Color color = state.palette[colornum];
                    _spriteBatch.Draw(gc.GetImage(), new Vector2(x*8, y*8), color);
                }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
