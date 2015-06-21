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
            this.radiuses = new float[3];
            this.actualRadiuses = null;
            this.planes = new List<Plane>();
            this.centersOfWalls = new Vector3[4];
            this.pointOfChangeWall = new List<Vector3>();
            this.maxValueBoundingBoxes = new List<Vector3>();
            this.minValueBoundingBoxes = new List<Vector3>();
            this.centerOfRamp = new Vector3(0, 0, 0);
        }

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
        public List<Vector3> minValueBoundingBoxes;
        public List<Vector3> maxValueBoundingBoxes;
        public Vector3 centerOfRamp;
        public Box(Player gameObj)
        {
            this.centerOfRamp = new Vector3(0, 0, 0);
            this.maxValueBoundingBoxes = new List<Vector3>();
            this.minValueBoundingBoxes = new List<Vector3>();
            this.pointOfChangeWall = new List<Vector3>();
            this.actualCollidingPoints = new List<Vector3>();
            this.centersOfWalls = new Vector3[4];
            this.planes = new List<Plane>();
            ActualCorners = new Vector3[6];
            this.referencePlayer = gameObj;
            this.referenceObject = gameObj;
            this.corners = new Vector3[8];
            this.corners = gameObj.boundingBox.GetCorners();
            this.min = gameObj.boundingBox.Min;
            this.max = gameObj.boundingBox.Max;
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
            this.actualRadiuses = new List<Vector3>();
            this.radiuses = new float[3];
            this.maxValueBoundingBoxes = new List<Vector3>();
            this.minValueBoundingBoxes = new List<Vector3>();
            this.centerOfRamp = new Vector3(0, 0, 0);
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
            if (referenceObject.Name.Contains("Palo"))
            {
               // meshMax.Z += 5f;
               // meshMin.Z -= 5f;
                meshMax.X += 8f;
                meshMin.X -= 8f;
                meshMax.Y += 8f;
                meshMin.Y -= 8f;

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
            if (referenceObject.Name.Contains("character") && referenceObject.Name != "enemyMarian" && !referenceObject.Name.Contains("Palo"))
            {
                referenceMax.Z += 1.5f;
                referenceMin.Z -= 1.5f;
            }
            else if(referenceObject.Name.Contains("stuff"))
            {
               // referenceMax.X += 1f;
               // referenceMin.X -= 1f;
           //     referenceMax.Y += 1f;
           //     referenceMin.Y -= 1f;
              //  referenceMax.Z += 1f;
             //   referenceMin.Z -= 1f;
            }
            else if (referenceObject.Name == "enemyMarian")
            {
                referenceMax.Z -= 1.5f;
                referenceMin.Z += 1.5f;
            }
            else if (referenceObject.Name.Contains("Palo"))
            {
                referenceMax.Z += 5f;
                referenceMin.Z -= 5f;
                referenceMax.X += 3f;
                referenceMin.X -= 3f;
                referenceMax.Y -= 7f;
                referenceMin.Y += 7f;

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
            referenceObject.boxes.Clear();
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
                    minValueBoundingBoxes.Add(meshMin);
                    maxValueBoundingBoxes.Add(meshMax);
                    // transform by mesh bone matrix
                    meshMin = Vector3.Transform(meshMin, meshTransform);
                    meshMax = Vector3.Transform(meshMax, meshTransform);
                    if (referenceObject.Name != "enemyMarian" && !referenceObject.Name.Contains("Palo"))
                    {
                        meshMax.X -= 1;
                        meshMin.X += 1;
                    }
                    if (referenceObject.Name == "enemyMarian")
                    {
                        meshMax.X += 1;
                        meshMin.X -= 1;
                    }
                    else if (referenceObject.Name.Contains("Palo"))
                    {
                        meshMax.Z += 5f;
                        meshMin.Z -= 5f;
                        meshMax.X += 3f;
                        meshMin.X -= 3f;
                        meshMax.Y -= 7f;
                        meshMin.Y += 7f;
                    }
                    
                    referenceObject.boxes.Add(new BoundingBox(meshMin, meshMax));
                }
               
            }
           
        }
        public void UpdateBoundingBoxes()
        {
            referenceObject.boxes.Clear();
            for (int i = 0; i < minValueBoundingBoxes.Count; i++)
            {
                Vector3 referenceMin = minValueBoundingBoxes[i];
                Vector3 referenceMax = maxValueBoundingBoxes[i];
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
                if (!referenceObject.Name.Contains("Palo") && !referenceObject.Name.Contains("stuff"))
                {
                    referenceMax.X -= 1;
                    referenceMin.X += 1;
                  }
                else if (referenceObject.Name.Contains("Palo"))
                {
                    referenceMax.Z += 3f;
                    referenceMin.Z -= 3f;
                    referenceMax.X += 2f;
                    referenceMin.X -= 2f;
                    referenceMax.Y -= 7f;
                    referenceMin.Y += 7f;
                 }
                else if (referenceObject.Name.Contains("stuff"))
                {
                //  referenceMax.X += 1f;
                  //  referenceMin.X -= 1f;
                    referenceMax.Y += 1f;
                    referenceMin.Y -= 1f;
                   // referenceMax.Z += 1f;
                  //  referenceMin.Z -= 1f;
                } 
                referenceObject.boxes.Add(new BoundingBox(referenceMin, referenceMax));

            }
        }
        //potrzebne do kolizji z podłoga 
        public void GetCenter()
        {
            center2.X = (corners[1].X + corners[7].X) / 2;
            center2.Y = (corners[1].Y + corners[7].Y) / 2;
            center2.Z = (corners[1].Z + corners[7].Z) / 2;
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
            if (referenceObject.Name.Contains("6") ||(referenceObject.Rotation.Y !=0 && referenceObject.Name.Contains("2") )
                || (referenceObject.Rotation.Y ==0 && referenceObject.Name.Contains("4")) || (referenceObject.Rotation.Y !=90 && referenceObject.Name.Contains("7")) ||
                (referenceObject.Name.Contains("door") && referenceObject.Rotation.Y == 0))
            {

                vecAB.Add(corners[4] - corners[0]);  //odwracam normalne w physic manager
                vecAC.Add(corners[4] - corners[7]);
                vecAB.Add(corners[5] - corners[1]);  
                vecAC.Add(corners[5] - corners[6]);
                vecA.Add(corners[4]);
                vecA.Add(corners[5]);

                vecA.Add(corners[5]);    //odwracam normalne w physic manager
                vecA.Add(corners[1]);
                vecAB.Add(corners[5] - corners[4]);
                vecAC.Add(corners[5] - corners[6]);
                vecAB.Add(corners[1] - corners[0]);
                vecAC.Add(corners[1] - corners[2]);
            }
            else {
               
                vecAB.Add(corners[5] - corners[1]);   //odwracam normalne w physic manager
                vecAC.Add(corners[5] - corners[6]);
                vecAB.Add(corners[4] - corners[0]);
                vecAC.Add(corners[4] - corners[7]);
                vecA.Add(corners[5]);
                vecA.Add(corners[4]);
            
                vecA.Add(corners[1]);    //odwracam normalne w physic manager
                vecA.Add(corners[5]);  
                vecAB.Add(corners[1] - corners[0]);
                vecAC.Add(corners[1] - corners[2]);
                vecAB.Add(corners[5] - corners[4]);
                vecAC.Add(corners[5] - corners[6]);
               }
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
            if ((referenceObject.Name.Contains("door") && referenceObject.Rotation.Y == 0) || (referenceObject.Rotation.Y != 0 && referenceObject.Name.Contains("2")) || referenceObject.Name.Contains("6") ||
                (referenceObject.Rotation.Y == 0 && referenceObject.Name.Contains("4")) || (referenceObject.Rotation.Y !=90 && referenceObject.Name.Contains("7")))
            {
                centersOfWalls[2].X = (corners[4].X + corners[6].X) / 2; //front
                centersOfWalls[2].Y = (corners[4].Y + corners[6].Y) / 2;
                centersOfWalls[2].Z = (corners[4].Z + corners[6].Z) / 2;
                centersOfWalls[3].X = (corners[0].X + corners[2].X) / 2; //back
                centersOfWalls[3].Y = (corners[0].Y + corners[2].Y) / 2;
                centersOfWalls[3].Z = (corners[0].Z + corners[2].Z) / 2;
                centersOfWalls[0].X = (corners[4].X + corners[3].X) / 2; //left
                centersOfWalls[0].Y = (corners[4].Y + corners[3].Y) / 2;
                centersOfWalls[0].Z = (corners[4].Z + corners[3].Z) / 2;
                centersOfWalls[1].X = (corners[5].X + corners[2].X) / 2; //right
                centersOfWalls[1].Y = (corners[5].Y + corners[2].Y) / 2;
                centersOfWalls[1].Z = (corners[5].Z + corners[2].Z) / 2;
            }
            else{
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
        }

        public void setpointOfChangeWall()
        {

            float distanceX =(float) Math.Sqrt(Math.Pow(referenceObject.boundingBox.Max.X - referenceObject.boundingBox.Min.X,2));
            float distanceZ = (float) Math.Sqrt(Math.Pow(referenceObject.boundingBox.Max.Z - referenceObject.boundingBox.Min.Z,2));
            if (distanceX < distanceZ)
            {
                if (referenceObject.boundingBox.Min.Z < referenceObject.boundingBox.Max.Z) { 
                referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(0, 0, referenceObject.boundingBox.Min.Z));
                referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(0, 0, referenceObject.boundingBox.Max.Z));
                }
                else
                {
                    referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(0, 0, referenceObject.boundingBox.Max.Z));
                    referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(0, 0, referenceObject.boundingBox.Min.Z));
                }
                referenceObject.message = "z";
            }
            else
            {
                if (referenceObject.boundingBox.Min.X < referenceObject.boundingBox.Max.X)
                {
                    referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(referenceObject.boundingBox.Min.X,0, 0));
                    referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(referenceObject.boundingBox.Max.X,0, 0));
                }
                else
                {
                    referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(referenceObject.boundingBox.Max.X,0, 0));
                    referenceObject.AAbox.pointOfChangeWall.Add(new Vector3(referenceObject.boundingBox.Min.X,0, 0));
                }
                referenceObject.message = "x";
            }
        }

        public void rampa()
        {
            GetCorners();
            List<Vector3> vecAB = new List<Vector3>();
            List<Vector3> vecAC = new List<Vector3>();
            List<Vector3> vecA = new List<Vector3>();
           
                vecAB.Add(this.corners[1] - this.corners[0]);  //odwracam normalne w physic manager
                vecAC.Add(this.corners[1] - this.corners[6]);
                vecA.Add(this.corners[1]);
            
           
            float dotProduct = 0;
            Vector3 normal = new Vector3(0, 0, 0);

            for (int i = 0; i < vecA.Count(); i++)
            {

                normal = Vector3.Cross(vecAB[i], vecAC[i]);
                normal.Normalize();

                dotProduct = Vector3.Dot(normal, vecA[i]);

                Plane tmpPlane = new Plane();

                tmpPlane.D = dotProduct;
                tmpPlane.Normal = normal;
                planes.Add(tmpPlane);
            }

            GetCorners();
            Vector3[] corners = referenceObject.boundingBox.GetCorners();

            centerOfRamp.X = (corners[0].X + corners[6].X) / 2; 
            centerOfRamp.Y = (corners[0].Y + corners[6].Y) / 2;
            centerOfRamp.Z = (corners[0].Z + corners[6].Z) / 2;
         
        }
        

    }
}


