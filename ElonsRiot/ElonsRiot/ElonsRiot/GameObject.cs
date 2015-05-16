using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SkinnedModel;
namespace ElonsRiot
{
    [Serializable]
    public class GameObject// :Microsoft.Xna.Framework.Game
    {
        //Wrzucane z XMLa
        [XmlAttribute("Name")]
        public string Name { get; set; }
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
        [XmlIgnore]
        public Model GameObjectModel { get; set; }
         [XmlIgnore]
        public BoundingBox boundingBox;
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
        public Vector3 center;
        [XmlIgnore]
        public Plane plane;
        [XmlIgnore]
        public Quaternion RotationQ;
        public float gravity;
        [XmlIgnore]
        public InterationTypes interactionType;
        [XmlIgnore]
        public float mass;
        [XmlIgnore]
        public float velocity;
        [XmlIgnore]
        public string collisionCommunicat;
        [XmlIgnore]
        public Vector3 oldPosition;
        public GameObject()
        {
            //Rotation = new Vector3(-90, 0, 0);
            //Position = new Vector3(0, 0, 0);
            RotationQ = Quaternion.Identity;
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
            //MatrixView = Matrix.CreateLookAt(new Vector3(10, 10, 10), new Vector3(0, 0, 0), Vector3.UnitY);
            //MatrixProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 600f, 0.1f, 100f);
            GameObjects = new List<GameObject>();
            AAbox = new Box();
            
        }
        public GameObject(Vector3 _position)
        {
            RotationQ = Quaternion.Identity;
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.ToRadians(Rotation.Y)) * Matrix.CreateRotationX(MathHelper.ToRadians(Rotation.X)) * Matrix.CreateRotationZ(MathHelper.ToRadians(Rotation.Z)) * Matrix.CreateTranslation(Position);
            GameObjects = new List<GameObject>();
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
        }
        public void DrawModels(ContentManager _contentManager, Player _playerObject)
        {

            DrawSimpleModel(GameObjectModel, MatrixWorld, _playerObject.camera.viewMatrix, _playerObject.camera.projectionMatrix);
            if( GameObjects != null)
            {
                foreach(var elem in GameObjects)
                {
                    elem.DrawModels(_contentManager, _playerObject);
                }
            }
        }

        public void DrawAnimatedModels(ContentManager _contentManager, Player _playerObject, AnimationPlayer animationPlayer)
        {
            DrawAnimatedModel(GameObjectModel, animationPlayer, MatrixWorld, _playerObject.camera.viewMatrix, _playerObject.camera.projectionMatrix);
            if (GameObjects != null)
            {
                foreach (var elem in GameObjects)
                {
                    elem.DrawAnimatedModels(_contentManager, _playerObject, animationPlayer);
                }
            }
        }
        public void RefreshMatrix()
        {
            MatrixWorld = Matrix.CreateScale(Scale) * Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateFromQuaternion(RotationQ) * Matrix.CreateTranslation(Position);    // Matrix.CreateRotationY(MathHelper.Pi) * Matrix.CreateTranslation(Position) * Matrix.CreateFromQuaternion(RotationQ);
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
                MoveWithDirectionRotate(toTarget / distanceEP);
            }
        }
        public void ChangePosition(Vector3 _position)
        {
            Position += Vector3.Transform(_position, RotationQ);
            RefreshMatrix();
        }
        public void ChangeRelativePosition(Vector3 _position)
        {
            oldPosition = Position;
            Position += _position;
        }
        public void SetPosition(Vector3 _position)
        {
            oldPosition = Position;
            Position = _position;
            RefreshMatrix();
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
       
        public void GetCentre()
        {
            BoundingSphere sphere = new BoundingSphere();

            Matrix tmp = Matrix.Identity;
            tmp = Matrix.CreateScale(Scale) * Matrix.CreateTranslation(Position);
            Matrix meshTransform = new Matrix();

            foreach (ModelMesh mesh in GameObjectModel.Meshes)
            {
                meshTransform = boneTransformations[mesh.ParentBone.Index] * tmp;


                sphere = BoundingSphere.CreateMerged(sphere, mesh.BoundingSphere);
                sphere = sphere.Transform(meshTransform);

            }
            this.center = sphere.Center;
        }
       
        private void DrawSimpleModel(Model model, Matrix world, Matrix view, Matrix projection, Texture2D newTexture = null)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
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

        private void DrawAnimatedModel(Model currentModel, AnimationPlayer animationPlayer, Matrix world, Matrix view, Matrix projection, Texture2D newTexture = null)
        {
            Matrix[] bones = animationPlayer.GetSkinTransforms();

            foreach (ModelMesh mesh in currentModel.Meshes)
            {
                foreach (SkinnedEffect effect in mesh.Effects)
                {
                    effect.SetBoneTransforms(bones);

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
        
        //aktualizacja danych fizycznych
        public void update()
       {
         
       }
    }
}


