using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SkinnedModel;
using System.Diagnostics;
namespace ElonsRiot
{
    [Serializable]
    public class GameObject// :Microsoft.Xna.Framework.Game
    {
        //Wrzucane z XMLa
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("Id")]
        public string id { get; set; }
        [XmlElement("Position")]
        public Vector3 Position { get; set; }
        [XmlElement("Rotation")]
        public Vector3 Rotation { get; set; }
        [XmlAttribute("Path")]
        public string ObjectPath { get; set; }
        [XmlElement("GameObject")]
        public List<GameObject> GameObjects { get; set; }
        [XmlElement("Scale")]
        public Vector3 Scale { get; set; }
        [XmlElement("Tag")]
        public string Tag { get; set; }
        public Matrix MatrixWorld { get; set; }
        //public Matrix MatrixView { get; set; }
        //public Matrix MatrixProjection { get; set; }
        [XmlElement("Interactive")]
        public bool Interactive { get; set; }
        [XmlElement("Mass")]
        public float mass;
        [XmlIgnore]
        public Model GameObjectModel { get; set; }
         [XmlIgnore]
        public BoundingBox boundingBox;
         [XmlIgnore]
         public List<BoundingBox> boxes;
        [XmlIgnore]
        public Matrix[] boneTransformations;
        [XmlIgnore]
        public short[] bBoxIndices ={
                0, 1, 1, 2, 2, 3, 3, 0, // Front edges
                4, 5, 5, 6, 6, 7, 7, 4, // Back edges
                0, 4, 1, 5, 2, 6, 3, 7 // Side edges connecting front and back
            };
        [XmlIgnore]
        public Box AAbox;
        [XmlIgnore]
        public Plane plane; //korzysta z tego tylko ziemia, wiec trzeba przerobic
        [XmlIgnore]
        public Quaternion RotationQ;
        public float gravity;
        [XmlIgnore]
        public InterationTypes interactionType;
        [XmlIgnore]
        public string collisionCommunicat;
        [XmlIgnore]
        public Vector3 oldPosition, newPosition;
        [XmlIgnore]
        public String message;
        [XmlIgnore]
        public List<GameObject> neighbors;
        [XmlIgnore]
        public Texture2D[] textures;

        public GameObject()
        {
            //Rotation = new Vector3(-90, 0, 0);
            //Position = new Vector3(0, 0, 0);
            RotationQ = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(Rotation.Y));
            RefreshMatrix();
            //MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
            //MatrixView = Matrix.CreateLookAt(new Vector3(10, 10, 10), new Vector3(0, 0, 0), Vector3.UnitY);
            //MatrixProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
            GameObjects = new List<GameObject>();
            AAbox = new Box();
            boxes = new List<BoundingBox>();
            neighbors = new List<GameObject>();
            gravity = -0.5f;
            
        }
        public GameObject(Vector3 _position)
        {
            Position = _position;
            RotationQ = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(Rotation.Y));
            RefreshMatrix();
            //MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
            GameObjects = new List<GameObject>();
            gravity = -0.5f;
        }
        public GameObject(Vector3 _position, Vector3 _rotation)
        {
            Position = _position;
            Rotation = _rotation;
            RotationQ = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(Rotation.Y));
            RefreshMatrix();
            //MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
            GameObjects = new List<GameObject>();
            gravity = -0.5f;
        }
        public void LoadModels(ContentManager _contentManager)
        {
            GameObjectModel = _contentManager.Load<Model>(ObjectPath);
            RefreshMatrix();
            if( GameObjects != null)
            {
                foreach(var elem in GameObjects)
                {
                    elem.LoadModels(_contentManager);
                }
            }
            if (Name != "characterElon" && Name != "characterPalo" && Tag != "guard")
            {
                textures = new Texture2D[7];
                int i = 0;
                foreach (ModelMesh mesh in GameObjectModel.Meshes)
                    foreach (BasicEffect currentEffect in mesh.Effects)
                        textures[i++] = currentEffect.Texture;
            }

        }
        public void DrawModels(ContentManager _contentManager, Player _playerObject, Vector3 lightPos, float lightPower, float ambientPower, Matrix lightsViewProjectionMatrix, string technique, Texture2D shadowMap, Matrix reflect, bool isMirror)
        {

            DrawSimpleModel(GameObjectModel, MatrixWorld, _playerObject.camera.viewMatrix, _playerObject.camera.projectionMatrix, lightPos, lightPower, ambientPower, lightsViewProjectionMatrix, technique, shadowMap, reflect, isMirror);
            if (GameObjects != null)
            {
                foreach(var elem in GameObjects)
                {
                    elem.DrawModels(_contentManager, _playerObject, lightPos, lightPower, ambientPower, lightsViewProjectionMatrix, technique, shadowMap, reflect, isMirror);
                }
            }
        }

        public void DrawAnimatedModels(ContentManager _contentManager, Player _playerObject, AnimationPlayer animationPlayer, Matrix reflect, bool isMirror)
        {
            DrawAnimatedModel(GameObjectModel, animationPlayer, MatrixWorld, _playerObject.camera.viewMatrix, _playerObject.camera.projectionMatrix, reflect, isMirror);
            if (GameObjects != null)
            {
                foreach (var elem in GameObjects)
                {
                    elem.DrawAnimatedModels(_contentManager, _playerObject, animationPlayer, reflect, isMirror);
                }
            }
        }
        public void RefreshMatrix()
        {
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(RotationQ) * Matrix.CreateTranslation(Position);    // Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(Position) * Matrix.CreateFromQuaternion(RotationQ);
        }
        public void RotationVectorToQuaternion()
        {
            RotationQ = Quaternion.CreateFromAxisAngle(Vector3.Up, MathHelper.ToRadians(Rotation.Y));
        }
        public void MoveWithDirectionRotate(Vector3 deltaVector)
        {
            Vector3 deltaVectorCopy = new Vector3(-deltaVector.X, 0, -deltaVector.Z);
            Position += deltaVector;
            deltaVectorCopy.Normalize();
            Matrix mat = Matrix.CreateLookAt(Position,
                                                Position + deltaVectorCopy,
                                                Vector3.Up);
            mat = Matrix.Transpose(mat);
            Quaternion q = Quaternion.Slerp(RotationQ,
                                            Quaternion.CreateFromRotationMatrix(mat),
                                            0.1f);
            RotationQ = q;
        }
        public void WalkToTarget(GameObject _target, float velocity, float stopDistance)
        {
            GameObject rightTarget = _target;
            Vector3 tmpPos = rightTarget.Position;
            tmpPos.Y = this.Position.Y;
            rightTarget.Position = tmpPos;
            Vector3 toTarget = (rightTarget.Position - Position);
            Vector3 currentDirection = Vector3.Normalize(MatrixWorld.Forward);
            float distanceEP = getDistance(rightTarget) / velocity;
            float angle = (float)Math.Atan2(toTarget.X, toTarget.Z);

            if (getDistance(rightTarget) > stopDistance)
            {
                this.oldPosition = this.Position;
                MoveWithDirectionRotate(toTarget / distanceEP);
            }
        }
        public void ChangePosition(Vector3 _position)
        {
            oldPosition = Position;
            Position += Vector3.Transform(_position, RotationQ);
            RefreshMatrix();
            newPosition = Position;
        }
        public void ChangeRelativePosition(Vector3 _position)
        {
            oldPosition = Position;
            Position += _position;
            newPosition = Position;
        }
        public void SetPositionY(float y)
        {
            Position = new Vector3(Position.X, y, Position.Z);
        }
        public void SetPosition(Vector3 _position)
        {
            oldPosition = Position;
            Position = _position;
            RefreshMatrix();
            newPosition = Position;
            //MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
        public void ChangeRotation(Vector3 _rotation)
        {
            Rotation += Vector3.Transform(_rotation, Matrix.Identity);
            RefreshMatrix();
            //MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
        public void RotateQuaternions(float angle)
        {
            Vector3 y = new Vector3(0, 1, 0);
            Quaternion addRot = Quaternion.CreateFromAxisAngle(y, angle);
            RotationQ = RotationQ * addRot;
            RefreshMatrix();
            //MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateFromQuaternion(RotationQ) * Matrix.CreateTranslation(Position) ;
        }

        public void SetRotation(Vector3 _rotation)
        {
            Rotation = _rotation;
            RefreshMatrix();
            //MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
        }
        public void setScale(Vector3 _scale)
        {
            Scale = _scale;
            RefreshMatrix();
            //MatrixWorld = Matrix.CreateScale(Scale);
        }
        public void Initialize()
        {
            boneTransformations = new Matrix[GameObjectModel.Bones.Count];
            this.GameObjectModel.CopyAbsoluteBoneTransformsTo(boneTransformations);
        }
        public float getDistance(GameObject _target)
        {
            return Vector3.Distance(_target.Position, Position);
        }


        private void DrawSimpleModel(Model model, Matrix world, Matrix view, Matrix projection, Vector3 lightPos, float lightPower, float ambientPower, Matrix lightsViewProjectionMatrix, string technique, Texture2D shadowMap, Matrix reflect, bool isMirror)
        {
            Matrix[] modelTransforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(modelTransforms);
            Matrix worldMatrix;
            int i = 0;
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {               
                    if (isMirror)
                        worldMatrix = modelTransforms[mesh.ParentBone.Index] * world * reflect;
                    else
                        worldMatrix = modelTransforms[mesh.ParentBone.Index] * world;

                    effect.CurrentTechnique = effect.Techniques[technique];
                    effect.Parameters["xCamerasViewProjection"].SetValue(view * projection);
                    effect.Parameters["xLightsViewProjection"].SetValue(lightsViewProjectionMatrix);
                    effect.Parameters["xWorld"].SetValue(worldMatrix);
                    effect.Parameters["xTexture"].SetValue(textures[i++]);
                    effect.Parameters["xLightPos"].SetValue(lightPos);
                    effect.Parameters["xLightPower"].SetValue(lightPower);
                    effect.Parameters["xAmbient"].SetValue(ambientPower);
                    effect.Parameters["xShadowMap"].SetValue(shadowMap);
                }
                mesh.Draw();
            }
        }

        private void DrawAnimatedModel(Model currentModel, AnimationPlayer animationPlayer, Matrix world, Matrix view, Matrix projection, Matrix reflect, bool isMirror, Texture2D newTexture = null)
        {
            Matrix[] bones = animationPlayer.GetSkinTransforms();
            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);
                    if (isMirror)
                        effect.World = mesh.ParentBone.Transform * world * reflect;
                    else
                        effect.World = mesh.ParentBone.Transform * world;

                    effect.View = view;
                    effect.Projection = projection;

                    if (newTexture != null)
                    {
                        effect.Texture = newTexture;
                    }
                }

                mesh.Draw();
            }
        }
        
     //ustawianie rodzaju interakcji
       public void setInteractionType()
        {
           if(this.Name.Contains("door"))
           {
               interactionType = InterationTypes.door;
           }
           else if(this.Name.Contains("box"))
           {
               interactionType = InterationTypes.box;
               
           }
           else if (this.Name.Contains("stairs"))
           {
               interactionType = InterationTypes.stairs;

           }
        }

       public void LoadEffects(Effect effect)
       {
           foreach (ModelMesh mesh in GameObjectModel.Meshes)
               foreach (ModelMeshPart meshPart in mesh.MeshParts)
                   meshPart.Effect = effect.Clone();
       }

        //aktualizacja danych fizycznych
        public void update()
       {
        // if(this.Name.Contains("box"))
        // {
        //     mass = 50;
        // }
        //if(this.Name.Contains("Elon"))
        //{
        //    mass = 70;
        //}
        //if (this.Name.Contains("Palo"))
        //{
        //    mass = 100;
        //}
       }

    }


}


