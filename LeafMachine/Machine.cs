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
        private RenderTarget2D target;
        private MachineState state;
        private AphidInterpreter intp;
        private Effect fullScreenShader;
        int scale = 2;
        string mainfile = "";

        public Machine(string mainfile)
        {
            _graphics = new GraphicsDeviceManager(this);
            this.mainfile = mainfile;

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
            _graphics.PreferredBackBufferWidth = 320 * scale;   // scaling factor here, we should be able to set
            _graphics.PreferredBackBufferHeight = 200 * scale;  // this manually later
            _graphics.ApplyChanges();

            intp = new AphidInterpreter();
            state = new MachineState(_graphics);
            LoadBuiltinLeafWords(intp, state);

            if (this.mainfile == "") {
                // test test...
                state.SetChar(0, 0, 'A');
                state.SetChar(1, 0, 'B');
                state.SetChar(39, 24, '!');
            } else {
                intp.RunFile(this.mainfile);
            }

        }

        protected override void LoadContent()
        {
            fullScreenShader = Content.Load<Effect>("grayscale");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            target = new RenderTarget2D(GraphicsDevice, MachineState.WIDTH*8, MachineState.HEIGHT*8);
            GraphicsDevice.SetRenderTarget(target);

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
            // draw to the target
            GraphicsDevice.SetRenderTarget(target);

            GraphicsDevice.Clear(state.palette[state.bgColor]);

            _spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
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

            // now draw the target to a scaled rectangle
            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin(effect: fullScreenShader);
            _spriteBatch.Draw(target, new Rectangle(0, 0, MachineState.WIDTH*8*scale, MachineState.HEIGHT*8*scale), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
