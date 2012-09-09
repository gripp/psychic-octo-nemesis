using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework;

using CS8803AGAGameLibrary;
using CS8803AGAGameLibrary.actions;

namespace XMLTester
{
    class Program
    {
        static void XnaSerialize(object data)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create("test.xml", settings))
            {
                IntermediateSerializer.Serialize(writer, data, null);
            }
        }

        static void Main(string[] args)
        {
            try
            {
                ActionInfoAttack ai = new ActionInfoAttack();
                ai.location = new Rectangle(0, 0, 20, 20);

                Sprite sprite = new Sprite();
                sprite.action = new AActionInfo[1];
                sprite.action[0] = ai;

                object testData = sprite;

                /*
                DecorationSetInfo testData = new DecorationSetInfo();
                testData.assetPath = "Sprites/trees";
                testData.decorations = new Dictionary<string, DecorationInfo>();

                DecorationInfo di = new DecorationInfo();
                testData.decorations.Add("forest", di);
                 * */

                /*
                CharacterInfo testData = new CharacterInfo();
                testData.animationDataPath = @"Animation/Data/mask";
                testData.animationTexturePath = @"Animation/Sprites/mask";
                testData.collisionBox = new Rectangle(-14, 22, 28, 10);
                testData.speed = 5;
                 */

                /*
                TileSet testData = new TileSet(15);
                testData.assetPath = "Sprites/grassRock";
                testData.tileWidth = 40;
                testData.tileHeight = 40;
                testData.tileInfos[0].passable = true;
                testData.tileInfos[1].passable = true;
                testData.tileInfos[2].passable = true;
                testData.tileInfos[12].passable = true;
                testData.columnsOnSpritesheet = 5;
                 */

                /*
                PropAnimSet pas = new PropAnimSet();
                pas.props.Add("frameHeight", "32");
                pas.props.Add("frameWidth", "23");
                pas.props.Add("duration", "5");
                pas.anims.Add(new PropAnim("right", 2));
                pas.anims.Add(new PropAnim("left", 2));
                pas.anims.Add(new PropAnim("up", 2));
                pas.anims.Add(new PropAnim("down", 2));
                pas.anims.Add(new PropAnim("upright", 2));
                pas.anims.Add(new PropAnim("downleft", 2));
                pas.anims.Add(new PropAnim("upleft", 2));
                pas.anims.Add(new PropAnim("downright", 2));

                Object testData = pas;
                 */

                /*
                AnimationSet testData = new AnimationSet();
                testData.anims = new List<Animation>();
                Animation walk = new Animation("walk", 2, 1, true, 0, "idle");
                walk.frames[0] = new Frame();
                walk.frames[0].box = new Rectangle(0, 0, 23, 32);
                walk.frames[0].duration = 2;
                walk.frames[0].trigger = "";
                walk.frames[1] = new Frame();
                walk.frames[1].box = new Rectangle(23, 0, 23, 32);
                walk.frames[1].duration = 2;
                walk.frames[1].trigger = "";
                Animation run = new Animation("run", 2, 1, true, 0, "idle");
                run.frames[0] = new Frame();
                run.frames[0].box = new Rectangle(0, 33, 23, 32);
                run.frames[0].duration = 2;
                run.frames[0].trigger = "";
                run.frames[1] = new Frame();
                run.frames[1].box = new Rectangle(23, 33, 23, 32);
                run.frames[1].duration = 2;
                run.frames[1].trigger = "";
                testData.anims.Add(walk);
                testData.anims.Add(run);
                 * */

                XnaSerialize(sprite);
                
            }
            catch (System.IO.FileNotFoundException fex)
            {
                //fex.Data;
                int a = 7;
            }
        }
    }
}
