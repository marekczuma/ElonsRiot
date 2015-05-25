﻿using Microsoft.Xna.Framework;
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
            ActualCorners = null;
            this.referencePlayer = null;
            this.corners = null;
            this.min = new Vector3(0, 0, 0);
            this.max = new Vector3(0, 0, 0);
            this.center = new Vector3(0, 0, 0);
            this.radiuses = new float[3];
            this.actualRadiuses = null;
            this.planes = new Plane[2];
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
        public Plane[] planes;
            
        public Box(Player gameObj)
        {
            this.planes = new Plane[2];
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
            GetRadius();
            CreateRadiuses();


        }
        public Box(GameObject gameObj, Player pla)
        {
            this.planes = new Plane[2];
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
            GetRadius();
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
            if ((referenceObject.Rotation.Y != 0 && referenceObject.ObjectPath != "3D/ludzik/dude"))
            {
                     tmp = referenceObject.MatrixWorld;
            }
           
            else
            {
                tmp = Matrix.CreateScale(referenceObject.Scale) * Matrix.CreateTranslation(referenceObject.Position);
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
            if ((referenceObject.Rotation.Y != 0 && referenceObject.ObjectPath != "3D/ludzik/dude"))
            {
                tmp = referenceObject.MatrixWorld;
            }

            else
            {
                tmp = Matrix.CreateScale(referenceObject.Scale) * Matrix.CreateTranslation(referenceObject.Position);
            }
            Matrix meshTransform = Matrix.Identity;
            foreach (ModelMesh mesh in referenceObject.GameObjectModel.Meshes)
            {
                meshTransform = referenceObject.boneTransformations[mesh.ParentBone.Index] * tmp;

                // transform by mesh bone matrix
                referenceMin = Vector3.Transform(referenceMin, meshTransform);
                referenceMax = Vector3.Transform(referenceMax, meshTransform);
            }
            if (referenceObject.ObjectPath == "3D/ludzik/dude")
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
                meshMax.X -= 2;
                meshMin.X += 2;
                referenceObject.boxes.Clear();
                referenceObject.boxes.Add(new BoundingBox(meshMin, meshMax));
            }
           
        }
        //potrzebne do kolizji z podłoga 
        public void GetRadius()
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
            Vector3[] vecAB = new Vector3[2];
            Vector3[] vecAC = new Vector3[2];
            Vector3[] vecA = new Vector3[2];
            if (Math.Abs(corners[4].X - corners[5].X) > Math.Abs(corners[5].Z - corners[1].Z))
            {
                vecAB[0] = corners[1] - corners[0];
                vecAC[0] = corners[3] - corners[0];
                vecAB[1] = corners[4] - corners[5];
                vecAC[1] = corners[6] - corners[5];
                vecA[0] = corners[0];
                vecA[1] = corners[5];

            }
            else
            {
                vecA[0] = corners[5];
                vecA[1] = corners[0];
                vecAB[0] = corners[1] - corners[5];
                vecAC[0] = corners[6] - corners[5];
                vecAB[1] = corners[4] - corners[0];
                vecAC[1] = corners[3] - corners[0];
            }

            float dotProduct = 0;
            Vector3 normal = new Vector3(0, 0, 0);
            for(int i = 0; i < 2;i++)
            {

                    normal = Vector3.Cross(vecAB[i], vecAC[i]);
                    normal.Normalize();

                    dotProduct = Vector3.Dot(normal, vecA[i]);

                    planes[i] = new Plane();
                    planes[i].Normal = normal;
                    planes[i].D = dotProduct;
            }
         
        }

    }
}


