using MinimalAF.Logic;
using MinimalAF.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace VisualTests
{
    public class ECSTest : EntryPoint
    {
        public override void Start()
        {
            Window.Size = (800, 600);
            Window.Title = "ECS Test";
            CTX.SetClearColor(0, 0, 0, 0);

            Window.RenderFrequency = 60;
        }

        public override void Render(double deltaTime)
        {

        }


        public override void Update(double deltaTime)
        {

        }
    }
}
