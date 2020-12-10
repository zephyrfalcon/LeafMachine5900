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

        // do these need to go into MachineState?
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
                aip.LoadWord(entry.Key, new AphidLeafBuiltinWord(state, entry.Key, entry.Value));
            }
        }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.PreferredBackBufferWidth = 320 * scale;   // scaling factor here, we should be able to set
            _graphics.PreferredBackBufferHeight = 200 * scale;  // this manually later
            _graphics.ApplyChanges();
            this.Window.Title = "LEAF-5900";

            intp = new AphidInterpreter();
            state = new MachineState(_graphics);
            LoadBuiltinLeafWords(intp, state);

            if (this.mainfile == "") {
                // test test...
                state.SetChar(0, 0, "A");
                state.SetChar(1, 0, "B");
                state.SetChar(39, 24, "!");
            } else {
                this.Window.Title += $" :: {System.IO.Path.GetFileName(this.mainfile)}";
                intp.RunFile(this.mainfile);
            }

        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            target = new RenderTarget2D(GraphicsDevice, MachineState.WIDTH*8, MachineState.HEIGHT*8);
            GraphicsDevice.SetRenderTarget(target);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            state.tix++;
            state.kbhandler.Update(); 
            // KeyboardHandler is not being used by the built-in words yet... it's not part of MachineState
            // ...should it be?

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // if an updater is defined, execute it
            if (state.updater != null) {
                state.updater.Run(this.intp);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // draw to the target
            GraphicsDevice.SetRenderTarget(target);

            GraphicsDevice.Clear(state.palette[state.bgColor]);

            _spriteBatch.Begin();
            // plot characters
            for (int x = 0; x < MachineState.WIDTH; x++)
                for (int y = 0; y < MachineState.HEIGHT; y++) {
                    GraphicChar gc = state.gcsmanager.Get(state.chars[x,y].charset, state.chars[x,y].charname);
                    int colornum = state.chars[x,y].fgcolor;
                    Color color = state.palette[colornum];
                    _spriteBatch.Draw(gc.GetImage(), new Vector2(x*8, y*8), color);
                }
            _spriteBatch.End();

            // now draw the target to a scaled rectangle
            GraphicsDevice.SetRenderTarget(null);

            _spriteBatch.Begin();
            _spriteBatch.Draw(target, new Rectangle(0, 0, MachineState.WIDTH*8*scale, MachineState.HEIGHT*8*scale), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
