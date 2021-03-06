﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

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
        private int previousScrollValue;

        public Vector3 position;
        public Vector3 target;
        public Matrix viewMatrix, projectionMatrix;
        public Matrix bigView;

        private float yaw, pitch, roll;
        public Matrix cameraRotation;

        private Vector3 desiredPosition;
        public Vector3 desiredTarget;
        public Vector3 offsetDistance;
        public Vector3 oldOffset;
        public BoundingFrustum frustum;

        public Camera(Player player)
        {
            ResetCamera(player.elonState);
        }

        public void ResetCamera(CharacterState elonState)
        {
            position = new Vector3(110, 4, -120);
            target = new Vector3();

            viewMatrix = Matrix.Identity;
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45.0f), 16 / 9, 0.5f, 800f);

            bigView = Matrix.Multiply(viewMatrix, 1f);

            frustum = new BoundingFrustum(bigView * projectionMatrix);

            yaw = 0.0f;
            pitch = 0.0f;
            roll = 0.0f;

            cameraRotation = Matrix.Identity;

            desiredPosition = position;
            desiredTarget = target;
            if (elonState.State == State.idleShoot)
            {
                offsetDistance = new Vector3(0, 0, 25);
            }else
            {
                offsetDistance = new Vector3(0, 0, -450);
            }
        }
        public void SetOffsetDistance()
        {
            oldOffset = offsetDistance;
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

        public void Update(Matrix chasedObjectsWorld, CharacterState elonState, Vector3 rotation, Player player)
        {
            MouseState mouseState = Mouse.GetState();

            HandleInput(elonState, rotation);
            UpdateViewMatrix(chasedObjectsWorld, elonState, player);
            UpdateFrustum();
            if (elonState.State == State.idleShoot)
            {
                offsetDistance = new Vector3(0, 0, 25);
            }else
            {
                offsetDistance = oldOffset;
            }
        }

        public void UpdateFrustum()
        {
            bigView = Matrix.Multiply(viewMatrix, 1f);
            Matrix viewProjection = bigView * projectionMatrix;
            frustum.Matrix = viewProjection;
        }

        public void UpdateCamera(GameTime gameTime)
        {
            float elapsedTimeSec = (float)gameTime.ElapsedGameTime.TotalSeconds;

            RotateSmoothly(smoothedMouseMovement.X, smoothedMouseMovement.Y);
        }

        private void UpdateViewMatrix(Matrix chasedObjectsWorld, CharacterState elonState, Player player)
        {
            cameraRotation.Forward.Normalize();
            chasedObjectsWorld.Right.Normalize();
            chasedObjectsWorld.Up.Normalize();

            cameraRotation = Matrix.CreateRotationX(pitch) * Matrix.CreateRotationY(yaw) * Matrix.CreateFromAxisAngle(cameraRotation.Forward, roll);
            
            if (elonState.State == State.idleShoot)
            {
                GameObject tmpDesire = new GameObject { Position = player.Position };
                tmpDesire.Position += 10.0f * Vector3.Transform(Vector3.Forward, player.RotationQ) + Vector3.Up * 6.8f;
                target = tmpDesire.Position;
            }else
            {
                desiredTarget = chasedObjectsWorld.Translation;
                
                desiredTarget.Y += 7;
                target = desiredTarget;
            }
            desiredPosition = Vector3.Transform(offsetDistance, chasedObjectsWorld);
            position = Vector3.SmoothStep(position, desiredPosition, 0.15f);

            yaw = MathHelper.SmoothStep(yaw, 0.0f, 0.1f);
            pitch = MathHelper.SmoothStep(pitch, 0.0f, 0.1f);
            roll = MathHelper.SmoothStep(roll, 0.0f, 0.2f);
            viewMatrix = Matrix.CreateLookAt(position, target, Vector3.Up);
        }

        private void HandleInput(CharacterState elonState, Vector3 rotation)
        {
            previousScrollValue = previousMouseState.ScrollWheelValue;
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            int centerX = 800 / 2;
            int centerY = 600 / 2;
            int deltaX = centerX + currentMouseState.X;
            int deltaY = centerY + currentMouseState.Y;

            smoothedMouseMovement.X = (float)deltaX;
            smoothedMouseMovement.Y = (float)deltaY;
            //if (elonState.State == State.run || elonState.State == State.walk || elonState.State == State.jump)
            //{
                //position.Y += smoothedMouseMovement.Y / 60 - 5;
            if (elonState.State == State.idleShoot)
            {
                position.Y += smoothedMouseMovement.Y / 110 - 5;
                if (position.Y > 10)
                    position.Y = 10;
                else if (position.Y < 7)
                    position.Y = 7;
                
            }else
            {
                position.Y += smoothedMouseMovement.Y / 60 - 5;
                if (position.Y > 25)
                    position.Y = 25;
                else if (position.Y < 5)
                    position.Y = 5;
            }

            //}
                if (elonState.State != State.idleShoot)
                {
                    if (currentMouseState.ScrollWheelValue < previousScrollValue)
                    {
                        offsetDistance.Z -= 15.0f;
                    }
                    else if (currentMouseState.ScrollWheelValue > previousScrollValue)
                    {
                        offsetDistance.Z += 15.0f;
                    }

                    if (currentMouseState.MiddleButton == ButtonState.Pressed)
                        offsetDistance.Z = -550.0f;

                    if (offsetDistance.Z < -750)
                        offsetDistance.Z = -750;
                    else if (offsetDistance.Z > -150)
                        offsetDistance.Z = -150;
                    oldOffset = offsetDistance;
                }

            previousScrollValue = currentMouseState.ScrollWheelValue;
            
        }

        public bool IsVisible(GameObject obj,BoundingFrustum Playerfrustum)
        {
            bool isVisible = false;
            Vector3[] corners = obj.boundingBox.GetCorners();
            for (int i = 0; i < corners.Count(); i++)
            {
                if (Playerfrustum.Contains(corners[i]) != ContainmentType.Disjoint)
                {
                    isVisible = true;
                }
            }
            if (obj.Name.Contains("terrain") || obj.Name == "ceil" || obj.Name == "ramp" || Playerfrustum.Contains(obj.boundingBox) != ContainmentType.Disjoint || obj.Name == "gun" || obj.Name == "gunPalo" || obj.Name =="gunMarian")
            {
                isVisible = true;
            }
            return isVisible;
        }
    } /// Przydałoby się trochę podnieść target przy celowaniu, bo celujemy w podłogę jeśli jesteśmy za daleko od celu
      /// ew. dodać możliwość niewielkiej zmiany Y
}
