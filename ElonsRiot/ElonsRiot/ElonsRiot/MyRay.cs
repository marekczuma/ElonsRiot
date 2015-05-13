using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public static class MyRay
    {
        static GraphicsDevice device;
        public static Scene thisScene;

        public  static void setReferences(GraphicsDevice graphicDevice, Scene myScene)
        {
            device = graphicDevice;
            thisScene = myScene;
        }
        public static Ray GetPickRay()
        {
            Matrix world = Matrix.CreateTranslation(10, 0, 0);
            Vector3 nearPoint = device.Viewport.Unproject(thisScene.PlayerObject.camera.position, thisScene.PlayerObject.camera.projectionMatrix, thisScene.PlayerObject.camera.viewMatrix, world);
            Vector3 farPoint = device.Viewport.Unproject(thisScene.PlayerObject.camera.target, thisScene.PlayerObject.camera.projectionMatrix, thisScene.PlayerObject.camera.viewMatrix, world);
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            return pickRay;
        }

        public static Ray GetPickRayCamera()
        {
            Matrix world = Matrix.CreateTranslation(0, 0, 0);
            Vector3 nearPoint = device.Viewport.Unproject(thisScene.PlayerObject.camera.position, thisScene.PlayerObject.camera.projectionMatrix, thisScene.PlayerObject.camera.viewMatrix, world);
            Vector3 farPoint = device.Viewport.Unproject(thisScene.PlayerObject.Position, thisScene.PlayerObject.camera.projectionMatrix, thisScene.PlayerObject.camera.viewMatrix, world);
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();
            Ray pickRay = new Ray(nearPoint, direction);
            return pickRay;
        }
    }
}
