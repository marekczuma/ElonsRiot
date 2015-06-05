using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElonsRiot
{
    public class Box
    {
        public Box()
        {
            this.actualCollidingPoints = new List<Vector3>();
            ActualCorners = null;
            this.referencePlayer = null;
            this.corners = null;
            this.min = new Vector3(0, 0, 0);
            this.max = new Vector3(0, 0, 0);
            this.center = new Vector3(0, 0, 0);
            this.radiuses = new float[3];
            this.actualRadiuses = null;
            this.planes = new List<Plane>();
            this.centersOfWalls = new Vector3[4];
            this.pointOfChangeWall = new List<Vector3>();
        }
        public Vector3 center;
        public float[] radiuses;
        public Vector3 center2;
        Player referencePlayer;
        GameObject referenceObject;
        public Vector3[] ActualCorners;
        public List<Vector3> actualRadiuses;
        public Vector3[] corners;
        public Vector3 min, max;
        public int length;
        public float height;
        public List<Plane> planes;
        public Vector3[] centersOfWalls;
        public List<Vector3> actualCollidingPoints;
        public List<Vector3> pointOfChangeWall;
        public Box(Player gameObj)
        {
            this.pointOfChangeWall = new List<Vector3>();
            this.actualCollidingPoints = new List<Vector3>();
            this.centersOfWalls = new Vector3[4];
            this.planes = new List<Plane>();
            ActualCorners = new Vector3[6];
            this.referencePlayer = gameObj;
            this.referenceObject = gameObj;
      //      createBoudingBox();
            this.corners = new Vector3[8];
            this.corners = gameObj.boundingBox.GetCorners();
            this.min = gameObj.boundingBox.Min;
            this.max = gameObj.boundingBox.Max;
            this.center = gameObj.center;
            this.actualRadiuses = new List<Vector3>();
            this.radiuses = new float[3];
            GetCenter();
            CreateRadiuses();


        }
        public Box(GameObject gameObj, Player pla)
        {
            this.pointOfChangeWall = new List<Vector3>();
            this.actualCollidingPoints = new List<Vector3>();
            this.centersOfWalls = new Vector3[4];
            this.planes = new List<Plane>();
            ActualCorners = new Vector3[6];
            this.referenceObject = gameObj;
            createBoudingBox();
            this.corners = new Vector3[8];
            this.corners = gameObj.boundingBox.GetCorners();
            this.min = gameObj.boundingBox.Min;
            this.max = gameObj.boundingBox.Max;
            this.referencePlayer = pla;
            this.center = gameObj.center;
            this.actualRadiuses = new List<Vector3>();
            this.radiuses = new float[3];
            GetCenter();
            CreateRadiuses();
        }
     
        public void GetCorners()
        {
            this.corners = referenceObject.boundingBox.GetCorners();
        }
        public void GetRefrecneObjectAndPlayer(GameObject gobj, Player play)
        {
            referenceObject = gobj;
            referencePlayer = play;
        }
        public void createBoudingBox()
        {
          
            Matrix tmp = Matrix.Identity;
           
            if (referenceObject.Name.Contains("character") || referenceObject.Name.Contains("enemy"))
            {
                tmp = Matrix.CreateScale(referenceObject.Scale) * Matrix.CreateTranslation(referenceObject.Position);
            }
            else
            {
                tmp = referenceObject.MatrixWorld;
            }

            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);
            Matrix meshTransform = new Matrix();
            foreach (ModelMesh mesh in referenceObject.GameObjectModel.Meshes)
            {
                meshTransform = referenceObject.boneTransformations[mesh.ParentBone.Index] * tmp;

                foreach (ModelMeshPart part in mesh.MeshParts)
                {

                    // The stride is how big, in bytes, one vertex is in the vertex buffer
                    // We have to use this as we do not know the make up of the vertex
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                    VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[part.NumVertices];
                    part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, stride);

                    // Find minimum and maximum xyz values for this mesh part
                    Vector3 vertPosition = new Vector3();

                    for (int i = 0; i < vertexData.Length; i++)
                    {
                        vertPosition = vertexData[i].Position;

                        // update our values from this vertex
                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }
                this.max = meshMax;
                this.min = meshMin;
                // transform by mesh bone matrix
                meshMin = Vector3.Transform(meshMin, meshTransform);
                meshMax = Vector3.Transform(meshMax, meshTransform);
            }
            if (referenceObject.ObjectPath == "3D/ludzik/dude")
            {
                meshMax.Z += 1.5f;
                meshMin.Z -= 1.5f;
            }
            
            referenceObject.boundingBox = new BoundingBox(meshMin, meshMax);
            
        }
        public void UpdateBoundingBox()
        {
            Vector3 referenceMin = min;
            Vector3 referenceMax = max;
            Matrix tmp = Matrix.Identity;
            if (referenceObject.Name.Contains("character") || referenceObject.Name.Contains("enemy"))
            {
                tmp = Matrix.CreateScale(referenceObject.Scale) * Matrix.CreateTranslation(referenceObject.Position);
            }
            else
            {
                tmp = referenceObject.MatrixWorld;
            }
            Matrix meshTransform = Matrix.Identity;
            foreach (ModelMesh mesh in referenceObject.GameObjectModel.Meshes)
            {
                meshTransform = referenceObject.boneTransformations[mesh.ParentBone.Index] * tmp;

                // transform by mesh bone matrix
                referenceMin = Vector3.Transform(referenceMin, meshTransform);
                referenceMax = Vector3.Transform(referenceMax, meshTransform);
            }
            if (referenceObject.Name.Contains("character") || referenceObject.Name.Contains("enemy"))
            {
                referenceMax.Z += 1.5f;
                referenceMin.Z -= 1.5f;
            }
            referenceObject.boundingBox = new BoundingBox(referenceMin, referenceMax); 
        }
        public void createBoudingBoxes()
        {
            referenceObject.Initialize();
            Matrix tmp = Matrix.Identity;
            tmp = Matrix.CreateScale(referenceObject.Scale) * Matrix.CreateTranslation(referenceObject.Position);
            Vector3 meshMax = new Vector3(float.MinValue);
            Vector3 meshMin = new Vector3(float.MaxValue);
            Matrix meshTransform = new Matrix();
            foreach (ModelMesh mesh in referenceObject.GameObjectModel.Meshes)
            {
                meshTransform = referenceObject.boneTransformations[mesh.ParentBone.Index] * tmp;
                foreach(ModelBone bones in referenceObject.GameObjectModel.Bones)
                {
                    Matrix stride = bones.Transform;
                }
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    
                    // The stride is how big, in bytes, one vertex is in the vertex buffer
                    // We have to use this as we do not know the make up of the vertex
                    int stride = part.VertexBuffer.VertexDeclaration.VertexStride;

                    VertexPositionNormalTexture[] vertexData = new VertexPositionNormalTexture[part.NumVertices];
                    part.VertexBuffer.GetData(part.VertexOffset * stride, vertexData, 0, part.NumVertices, stride);

                    // Find minimum and maximum xyz values for this mesh part
                    Vector3 vertPosition = new Vector3();

                    for (int i = 0; i < vertexData.Length; i++)
                    {
                        vertPosition = vertexData[i].Position;

                        // update our values from this vertex
                        meshMin = Vector3.Min(meshMin, vertPosition);
                        meshMax = Vector3.Max(meshMax, vertPosition);
                    }
                }

                // transform by mesh bone matrix
                meshMin = Vector3.Transform(meshMin, meshTransform);
                meshMax = Vector3.Transform(meshMax, meshTransform);
                meshMax.X -= 6;
                meshMin.X += 6;
                meshMax.Z -= 2;
                meshMin.Z += 2;
                referenceObject.boxes.Clear();
                referenceObject.boxes.Add(new BoundingBox(meshMin, meshMax));
            }
           
        }
        //potrzebne do kolizji z podłoga 
        public void GetCenter()
        {
            center2.X = (corners[1].X + corners[7].X) / 2;
            center2.Y = (corners[1].Y + corners[7].Y) / 2;
            center2.Z = (corners[1].Z + corners[7].Z) / 2;
        //   max = referenceObject.boundingBox.Max;
           // min = referenceObject.boundingBox.Min;
        }
      

        //potrzebne do wykrywania kolizji
        public void CreateRadiuses()
        {
            planes.Clear();
            radiuses[0] = (float)Math.Sqrt(Math.Pow(Convert.ToDouble(corners[7].X - corners[6].X), 2) + Math.Pow(Convert.ToDouble(corners[7].Y - corners[6].Y), 2)
             +   Math.Pow(Convert.ToDouble(corners[7].Z - corners[6].Z),2));//x
            radiuses[1] = (float)Math.Sqrt(Math.Pow(Convert.ToDouble(corners[7].X - corners[4].X), 2) + Math.Pow(Convert.ToDouble(corners[7].Y - corners[4].Y), 2)
             + Math.Pow(Convert.ToDouble(corners[7].Z - corners[4].Z), 2));//y
            radiuses[2] = (float)Math.Sqrt(Math.Pow(Convert.ToDouble(corners[7].X - corners[3].X), 2) + Math.Pow(Convert.ToDouble(corners[7].Y - corners[3].Y), 2)
             + Math.Pow(Convert.ToDouble(corners[7].Z - corners[3].Z), 2));//z
            radiuses[0] = radiuses[0] / 2;
            radiuses[1] = radiuses[1] / 2;
            radiuses[2] = radiuses[2] / 2;
        }
        
        public void createPlanes()
        {
            GetCorners();
            List<Vector3> vecAB = new List<Vector3>();
            List<Vector3> vecAC = new List<Vector3>();
            List<Vector3> vecA = new List<Vector3>();
          
               //z
             
                vecAB.Add(corners[5] - corners[1]);   //odwracam normalne w physic manager
                vecAC.Add(corners[5] - corners[6]);
                vecAB.Add(corners[4] - corners[0]);
                vecAC.Add(corners[4] - corners[7]);
                vecA.Add(corners[5]);
                vecA.Add(corners[4]);
            //x
                vecA.Add(corners[1]);    //odwracam normalne w physic manager
                vecA.Add(corners[5]);  
                vecAB.Add(corners[1] - corners[0]);
                vecAC.Add(corners[1] - corners[2]);
                vecAB.Add(corners[5] - corners[4]);
                vecAC.Add(corners[5] - corners[6]);
           
            float dotProduct = 0;
            Vector3 normal = new Vector3(0, 0, 0);

            for(int i = 0; i < vecA.Count();i++)
            {

                    normal = Vector3.Cross(vecAB[i], vecAC[i]);
                    normal.Normalize();
                  
                    dotProduct = Vector3.Dot(normal, vecA[i]);

                    Plane tmpPlane = new Plane();
                 
                       tmpPlane.D = dotProduct;
                       tmpPlane.Normal = normal;
                      
                  
                    planes.Add(tmpPlane);
            }
         
        }
        public void createPointsOfCollision()
        {
            GetCorners();
            Vector3[] corners = referenceObject.boundingBox.GetCorners();

            centersOfWalls[3].X = (corners[4].X + corners[6].X)/2; //front
            centersOfWalls[3].Y = (corners[4].Y + corners[6].Y)/2;
            centersOfWalls[3].Z = (corners[4].Z + corners[6].Z)/2;
            centersOfWalls[2].X = (corners[0].X + corners[2].X) / 2; //back
            centersOfWalls[2].Y = (corners[0].Y + corners[2].Y) / 2;
            centersOfWalls[2].Z = (corners[0].Z + corners[2].Z) / 2;
            centersOfWalls[1].X = (corners[4].X + corners[3].X) / 2; //left
            centersOfWalls[1].Y = (corners[4].Y + corners[3].Y) / 2;
            centersOfWalls[1].Z = (corners[4].Z + corners[3].Z) / 2;
            centersOfWalls[0].X = (corners[5].X + corners[2].X) / 2; //right
            centersOfWalls[0].Y = (corners[5].Y + corners[2].Y) / 2;
            centersOfWalls[0].Z = (corners[5].Z + corners[2].Z) / 2;
        }

        public void setpointOfChangeWall()
        {

            float distanceX = referenceObject.boundingBox.Max.X - referenceObject.boundingBox.Min.X;
            float distanceZ = referenceObject.boundingBox.Max.Z - referenceObject.boundingBox.Min.Z;
            if (distanceX < distanceZ)
            {
                referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(0, 0, referenceObject.boundingBox.Min.Z));
                referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(0, 0, referenceObject.boundingBox.Max.Z));
                referenceObject.message = "z";
            }
            else
            {
                referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(referenceObject.boundingBox.Min.X, 0, 0));
                referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(referenceObject.boundingBox.Max.X, 0, 0));
                referenceObject.message = "x";
            }
        }



    }
}


