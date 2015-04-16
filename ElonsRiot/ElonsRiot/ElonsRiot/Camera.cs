using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ElonsRiot
{
    public class Camera
    {
        private static Vector3 WORLD_X_AXIS = new Vector3(1.0f, 0.0f, 0.0f);
        private static Vector3 WORLD_Y_AXIS = new Vector3(0.0f, 1.0f, 0.0f);
        private static Vector3 WORLD_Z_AXIS = new Vector3(0.0f, 0.0f, 1.0f);

        private Quaternion orientation = Quaternion.Identity;

        private float rotationSpeed = 0.01f;

        private Vector2 smoothedMouseMovement;
        private MouseState currentMouseState;
        private MouseState previousMouseState;

        public Vector3 position;
        private Vector3 target;
        public Matrix viewMatrix, projectionMatrix;

        private float yaw, pitch, roll;
        private Matrix cameraRotation;

        private Vector3 desiredPosition;
        private Vector3 desiredTarget;
        public Vector3 offsetDistance;

        public Camera()
        {
            ResetCamera();
        }

        public void ResetCamera()
        {
            position = new Vector3(0, 10, 50);
            target = new Vector3();

            viewMatrix = Matrix.Identity;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 16 / 9, 0.5f, 500f);

            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;

            cameraRotation = Matrix.Identity;

            desiredPosition = position;
            desiredTarget = target;
            offsetDistance = new Vector3(0, -20, 10);
        }

        public void Rotate()
        {
            float yawRadians = MathHelper.ToRadians(yaw);
            float pitchRadians = MathHelper.ToRadians(pitch);
            Quaternion rotation = Quaternion.Identity;

            if (yawRadians != 0.0f)
            {
                Quaternion.CreateFromAxisAngle(ref WORLD_Y_AXIS, yaw, out rotation);
                Quaternion.Concatenate(ref rotation, ref orientation, out orientation);
            }

            if (pitchRadians != 0.0f)
            {
                Quaternion.CreateFromAxisAngle(ref WORLD_X_AXIS, pitch, out rotation);
                Quaternion.Concatenate(ref orientation, ref rotation, out orientation);
            }
        }

        private void RotateSmoothly(float currentYaw, float currentPitch)
        {
            currentYaw *= rotationSpeed;
            currentPitch *= rotationSpeed;

            yaw = currentYaw;
            pitch = currentPitch;

            Rotate();
        }

        public void Update(Matrix chasedObjectsWorld, CharacterState elonState, Vector3 rotation)
        {
            MouseState mouseState = Mouse.GetState();

            HandleInput(elonState, rotation);
            UpdateViewMatrix(chasedObjectsWorld, elonState);
        }

        public void UpdateCamera(GameTime gameTime)
        {
            float elapsedTimeSec = (float)gameTime.ElapsedGameTime.TotalSeconds;

            RotateSmoothly(smoothedMouseMovement.X, smoothedMouseMovement.Y);
        }

        private void UpdateViewMatrix(Matrix chasedObjectsWorld, CharacterState elonState)
        {
            if (elonState.State == State.run || elonState.State == State.walk || elonState.State == State.jump)
            {
                cameraRotation.Forward.Normalize();
                chasedObjectsWorld.Right.Normalize();
                chasedObjectsWorld.Up.Normalize();

                cameraRotation = Matrix.CreateRotationX(pitch) * Matrix.CreateRotationY(yaw) * Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);

                desiredTarget = chasedObjectsWorld.Translation;
                desiredTarget.Y += 7;
                target = desiredTarget;


                desiredPosition = Vector3.Transform(offsetDistance, chasedObjectsWorld);
                position = Vector3.SmoothStep(position, desiredPosition, 0.15f);

                yaw = MathHelper.SmoothStep(yaw, 0.0f, 0.1f);
                pitch = MathHelper.SmoothStep(pitch, 0.0f, 0.1f);
                roll = MathHelper.SmoothStep(roll, 0.0f, 0.2f);
            }

            else
            {
                cameraRotation.Forward.Normalize();

                

                desiredPosition = Vector3.Transform(offsetDistance, cameraRotation);
                desiredPosition += chasedObjectsWorld.Translation;
                //position = desiredPosition;
                cameraRotation = Matrix.CreateRotationX(pitch) * Matrix.CreateRotationY(yaw) * Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);
                target = chasedObjectsWorld.Translation;
                target.Y += 7;
                roll = MathHelper.SmoothStep(roll, 0f, 0.2f);
            }

            viewMatrix = Matrix.CreateLookAt(position, target, Vector3.Up);
        }

        private void HandleInput(CharacterState elonState, Vector3 rotation)
        {

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

           // Rectangle clientBounds = Game1.Instance.Window.ClientBounds;

            int centerX = 800 / 2;
            int centerY = 600 / 2;
            int deltaX = centerX + currentMouseState.X;
            int deltaY = centerY + currentMouseState.Y;

            smoothedMouseMovement.X = (float)deltaX;
            smoothedMouseMovement.Y = (float)deltaY;

            if (elonState.State == State.run || elonState.State == State.walk || elonState.State == State.jump)
                rotation.Y = -smoothedMouseMovement.X / 100;
        }
    }
}
