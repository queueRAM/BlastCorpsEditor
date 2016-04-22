using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BlastCorpsEditor
{
   public partial class BlastCorps3DForm : Form
   {
      bool loaded = false;

      private Matrix4 cameraMatrix;
      private float[] mouseSpeed = new float[2];
      private Vector3 location = new Vector3(0f, 0.3f, 0f);
      private Vector3 up = new Vector3(0f, 1f, 0f);
      private float pitch = 0.0f;
      private float facing = -MathHelper.PiOver4;
      private int lastX = -1;
      private int lastY = -1;
      private BlastCorpsLevel level = null;
      private Model3D levelModel = null;
      private BlastCorpsItem selectedItem = null;

      const float scale = 50f / (256f * 256f);

      public BlastCorpsItem SelectedItem
      {
         get { return selectedItem; }
         set { selectedItem = value; glControlViewer.Invalidate(); }
      }

      public BlastCorps3DForm()
      {
         InitializeComponent();
      }

      public void SetLevel(BlastCorpsLevel level)
      {
         this.level = level;
         levelModel = new Model3D(level);

         foreach (Vehicle veh in level.vehicles)
         {
            if (veh.type == 0) // player
            {
               location = new Vector3((veh.x + 20) * scale, (veh.y + 400) * scale, (veh.z - 400) * scale);
               pitch = -MathHelper.PiOver6;
               facing = MathHelper.PiOver2 + MathHelper.PiOver6;
               SetupViewport();
               glControlViewer.Invalidate();
               break;
            }
         }

         if (loaded)
         {
            glControlViewer.Invalidate();
         }
      }

      private void glControlViewer_Resize(object sender, EventArgs e)
      {
         GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

         Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView((float)Math.PI / 4, Width / (float)Height, 0.1f, 64.0f);
         GL.MatrixMode(MatrixMode.Projection);
         GL.LoadMatrix(ref projection);
         glControlViewer.Invalidate();
         SetupViewport();
      }

      private void glControlViewer_Load(object sender, EventArgs e)
      {
         loaded = true;
         cameraMatrix = Matrix4.Identity;
         SetupViewport();

         float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
         float[] mat_shininess = { 50.0f };
         float[] light_position = { 1.0f, 1.0f, 1.0f, 0.0f };
         float[] light_ambient = { 0.5f, 0.5f, 0.5f, 1.0f };

         //GL.ShadeModel(ShadingModel.Flat);

         GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Specular, mat_specular);
         GL.Material(MaterialFace.FrontAndBack, MaterialParameter.Shininess, mat_shininess);
         GL.Light(LightName.Light0, LightParameter.Position, light_position);
         GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
         GL.Light(LightName.Light0, LightParameter.Diffuse, mat_specular);

         GL.Enable(EnableCap.Lighting);
         GL.Enable(EnableCap.Light0);
         GL.Enable(EnableCap.DepthTest);
         GL.Enable(EnableCap.ColorMaterial);
         GL.Enable(EnableCap.Normalize);
      }

      private Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c)
      {
         var dir = Vector3.Cross(b - a, c - a);
         var norm = Vector3.Normalize(dir);
         return norm;
      }

      private Vector3 GetNormal(Collision24 tri)
      {
         Vector3 a = new Vector3(tri.x1, tri.y1, tri.z1);
         Vector3 b = new Vector3(tri.x2, tri.y2, tri.z2);
         Vector3 c = new Vector3(tri.x3, tri.y3, tri.z3);
         return GetNormal(a, b, c);
      }

      private Vector3 GetNormal(CollisionTri tri)
      {
         Vector3 a = new Vector3(tri.x1, tri.y1, tri.z1);
         Vector3 b = new Vector3(tri.x2, tri.y2, tri.z2);
         Vector3 c = new Vector3(tri.x3, tri.y3, tri.z3);
         return GetNormal(a, b, c);
      }

      private Vector3 GetNormal(TerrainTri tri)
      {
         Vector3 a = new Vector3(tri.x1, tri.y1, tri.z1);
         Vector3 b = new Vector3(tri.x2, tri.y2, tri.z2);
         Vector3 c = new Vector3(tri.x3, tri.y3, tri.z3);
         return GetNormal(a, b, c);
      }
 
      private Vector3 GetNormal(Wall tri)
      {
         Vector3 a = new Vector3(tri.x1, tri.y1, tri.z1);
         Vector3 b = new Vector3(tri.x2, tri.y2, tri.z2);
         Vector3 c = new Vector3(tri.x3, tri.y3, tri.z3);
         return GetNormal(a, b, c);
      }

      private void drawCube(bool wireframe, Color cubeColor)
      {
         if (wireframe)
         {
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-1.0f, -1.0f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(0.5f);
         }
         else
         {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
         }

         GL.Begin(PrimitiveType.Triangles);
         GL.Color3(cubeColor);
         // bottom
         GL.Normal3( 0.0f, -1.0f,  0.0f);
         GL.Vertex3( 0.5f,  0.0f, -0.5f);
         GL.Vertex3( 0.5f,  0.0f,  0.5f);
         GL.Vertex3(-0.5f,  0.0f,  0.5f);
         GL.Vertex3( 0.5f,  0.0f, -0.5f);
         GL.Vertex3(-0.5f,  0.0f,  0.5f);
         GL.Vertex3(-0.5f,  0.0f, -0.5f);
         // top
         GL.Normal3( 0.0f, 1.0f,  0.0f);
         GL.Vertex3( 0.5f, 1.0f, -0.5f);
         GL.Vertex3(-0.5f, 1.0f, -0.5f);
         GL.Vertex3(-0.5f, 1.0f,  0.5f);
         GL.Vertex3( 0.5f, 1.0f, -0.5f);
         GL.Vertex3(-0.5f, 1.0f,  0.5f);
         GL.Vertex3( 0.5f, 1.0f,  0.5f);
         // right
         GL.Normal3(1.0f,  0.0f,  0.0f);
         GL.Vertex3(0.5f,  0.0f, -0.5f);
         GL.Vertex3(0.5f,  1.0f, -0.5f);
         GL.Vertex3(0.5f,  1.0f,  0.5f);
         GL.Vertex3(0.5f,  0.0f, -0.5f);
         GL.Vertex3(0.5f,  1.0f,  0.5f);
         GL.Vertex3(0.5f,  0.0f,  0.5f);
         // front
         GL.Normal3( 0.0f,  0.0f, 1.0f);
         GL.Vertex3( 0.5f,  0.0f, 0.5f);
         GL.Vertex3( 0.5f,  1.0f, 0.5f);
         GL.Vertex3(-0.5f,  1.0f, 0.5f);
         GL.Vertex3( 0.5f,  0.0f, 0.5f);
         GL.Vertex3(-0.5f,  1.0f, 0.5f);
         GL.Vertex3(-0.5f,  0.0f, 0.5f);
         // left
         GL.Normal3(-1.0f,  0.0f,  0.0f);
         GL.Vertex3(-0.5f,  0.0f,  0.5f);
         GL.Vertex3(-0.5f,  1.0f,  0.5f);
         GL.Vertex3(-0.5f,  1.0f, -0.5f);
         GL.Vertex3(-0.5f,  0.0f,  0.5f);
         GL.Vertex3(-0.5f,  1.0f, -0.5f);
         GL.Vertex3(-0.5f,  0.0f, -0.5f);
         // back
         GL.Normal3( 0.0f,  0.0f, -1.0f);
         GL.Vertex3( 0.5f,  1.0f, -0.5f);
         GL.Vertex3( 0.5f,  0.0f, -0.5f);
         GL.Vertex3(-0.5f,  0.0f, -0.5f);
         GL.Vertex3( 0.5f,  1.0f, -0.5f);
         GL.Vertex3(-0.5f,  0.0f, -0.5f);
         GL.Vertex3(-0.5f,  1.0f, -0.5f);
         GL.End();

         if (wireframe)
         {
            GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.PolygonOffsetLine);
         }
      }

      private void drawArrow(Color color)
      {
         //  +--+
         //  |  |
         //  |  |
         // -+  +-
         // \    /
         //  \  /
         //   \/
         GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
         GL.Begin(PrimitiveType.Triangles);
         GL.Color3(color);
         GL.Normal3(0.0f, 0.0f, 1.0f);

         GL.Vertex3(-0.5f,  1.0f, 0.0f);
         GL.Vertex3( 0.5f,  1.0f, 0.0f);
         GL.Vertex3(-0.5f, -0.5f, 0.0f);

         GL.Vertex3( 0.5f,  1.0f, 0.0f);
         GL.Vertex3( 0.5f, -0.5f, 0.0f);
         GL.Vertex3(-0.5f, -0.5f, 0.0f);

         GL.Vertex3(-1.0f, -0.5f, 0.0f);
         GL.Vertex3( 1.0f, -0.5f, 0.0f);
         GL.Vertex3( 0.0f, -1.0f, 0.0f);
         GL.End();
      }

      // TODO: improve comm point rendering
      private void drawComm(Color color)
      {
         Color c1, c2;
         if (color == Color.Empty)
         {
            c1 = Color.Yellow;
            c2 = Color.Goldenrod;
         }
         else
         {
            c1 = c2 = color;
         }
         GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
         GL.Begin(PrimitiveType.Triangles);
         GL.Color3(c1);
         GL.Vertex3(0.0f, 1.0f, 0.0f);
         GL.Color3(c2);
         GL.Vertex3(-1.0f, 0.0f, 1.0f);
         GL.Color3(c2);
         GL.Vertex3(1.0f, 0.0f, 1.0f);
         GL.Color3(c1);
         GL.Vertex3(0.0f, 1.0f, 0.0f);
         GL.Color3(c2);
         GL.Vertex3(1.0f, 0.0f, 1.0f);
         GL.Color3(c2);
         GL.Vertex3(1.0f, 0.0f, -1.0f);
         GL.Color3(c1);
         GL.Vertex3(0.0f, 1.0f, 0.0f);
         GL.Color3(c2);
         GL.Vertex3(1.0f, 0.0f, -1.0f);
         GL.Color3(c2);
         GL.Vertex3(-1.0f, 0.0f, -1.0f);
         GL.Color3(c1);
         GL.Vertex3(0.0f, 1.0f, 0.0f);
         GL.Color3(c2);
         GL.Vertex3(-1.0f, 0.0f, -1.0f);
         GL.Color3(c2);
         GL.Vertex3(-1.0f, 0.0f, 1.0f);
         GL.End();
      }

      private void drawRdu(Color color)
      {
         Color c1, c2, c3;
         if (color == Color.Empty)
         {
            c1 = Color.Yellow;
            c2 = Color.Orange;
            c3 = Color.Brown;
         }
         else
         {
            c1 = c2 = c3 = color;
         }
         GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
         GL.Begin(PrimitiveType.Triangles);
         GL.Color3(c1);
         GL.Normal3(0.0f, 1.0f, 0.0f);   // normal 'up' - good enough
         GL.Vertex3( 0.0f, 1.0f, 0.0f);  // Top Of Triangle (Front)
         GL.Color3(c2);
         GL.Vertex3(-1.0f, 0.0f, 1.0f);  // Left Of Triangle (Front)
         GL.Color3(c3);
         GL.Vertex3( 1.0f, 0.0f, 1.0f);  // Right Of Triangle (Front)
         GL.Color3(c1);
         GL.Vertex3( 0.0f, 1.0f, 0.0f);  // Top Of Triangle (Right)
         GL.Color3(c3);
         GL.Vertex3( 1.0f, 0.0f, 1.0f);  // Left Of Triangle (Right)
         GL.Color3(c2);
         GL.Vertex3( 1.0f, 0.0f, -1.0f); // Right Of Triangle (Right)
         GL.Color3(c1);
         GL.Vertex3( 0.0f, 1.0f, 0.0f);  // Top Of Triangle (Back)
         GL.Color3(c2);
         GL.Vertex3( 1.0f, 0.0f, -1.0f); // Left Of Triangle (Back)
         GL.Color3(c3);
         GL.Vertex3(-1.0f, 0.0f, -1.0f); // Right Of Triangle (Back)
         GL.Color3(c1);
         GL.Vertex3( 0.0f, 1.0f, 0.0f);  // Top Of Triangle (Left)
         GL.Color3(c3);
         GL.Vertex3(-1.0f, 0.0f,-1.0f);  // Left Of Triangle (Left)
         GL.Color3(c2);
         GL.Vertex3(-1.0f, 0.0f, 1.0f);  // Right Of Triangle (Left)
         GL.End();
      }

      void drawHole(SquareBlock block, Color color)
      {
         GL.Enable(EnableCap.LineSmooth);
         GL.Enable(EnableCap.Blend);
         GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
         GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
         GL.Enable(EnableCap.PolygonOffsetLine);
         GL.PolygonOffset(-1.0f, -1.0f);
         GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
         GL.LineWidth(1.0f);
         GL.Color3(color);

         GL.Begin(PrimitiveType.Triangles);
         foreach (SquareBlock.Node v in block.nodes)
         {
            GL.Vertex3((float)v.x[0], (float)v.y[0], (float)v.z[0]);
            GL.Vertex3((float)v.x[1], (float)v.y[1], (float)v.z[1]);
            GL.Vertex3((float)v.x[2], (float)v.y[2], (float)v.z[2]);
         }
         GL.End();

         GL.Disable(EnableCap.LineSmooth);
         GL.Disable(EnableCap.Blend);
         GL.Disable(EnableCap.PolygonOffsetLine);
      }

      void drawPlatform(TrainPlatform platform, Color color)
      {
         GL.Enable(EnableCap.LineSmooth);
         GL.Enable(EnableCap.Blend);
         GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
         GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
         GL.Enable(EnableCap.PolygonOffsetLine);
         GL.PolygonOffset(-1.0f, -1.0f);
         GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
         GL.LineWidth(1.0f);
         GL.Color3(color);

         GL.Begin(PrimitiveType.Triangles);
         foreach (TrainPlatform.PlatformCollision tri in platform.collision)
         {
            GL.Vertex3(tri.x1, tri.y1, tri.z1);
            GL.Vertex3(tri.x2, tri.y2, tri.z2);
            GL.Vertex3(tri.x3, tri.y3, tri.z3);
         }
         GL.End();

         GL.Disable(EnableCap.LineSmooth);
         GL.Disable(EnableCap.Blend);
         GL.Disable(EnableCap.PolygonOffsetLine);
      }

      void drawDisplayList(bool wireframe)
      {
         if (wireframe)
         {
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-1.0f, -1.0f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Color3(Color.Black);
            GL.LineWidth(0.5f);
         }
         else
         {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Color3(Color.Gray);
         }

         GL.Begin(PrimitiveType.Triangles);
         foreach (Triangle tri in levelModel.triangles)
         {
            foreach (Vertex v in tri.vertices)
            {
               GL.Normal3(v.normals);
               GL.Vertex3(v.x, v.y, v.z);
            }
         }
         GL.End();

         if (wireframe)
         {
            GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.PolygonOffsetLine);
         }
      }

      void drawCollision(List<Collision24> collisions, bool wireframe, Color color)
      {
         if (wireframe)
         {
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-1.0f, -1.0f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(0.5f);
         }
         else
         {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
         }

         GL.Color3(color);
         GL.Begin(PrimitiveType.Triangles);
         foreach (Collision24 tri in collisions)
         {
            GL.Normal3(GetNormal(tri));
            GL.Vertex3(tri.x1, tri.y1, tri.z1);
            GL.Vertex3(tri.x2, tri.y2, tri.z2);
            GL.Vertex3(tri.x3, tri.y3, tri.z3);
         }
         GL.End();

         if (wireframe)
         {
            GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.PolygonOffsetLine);
         }
      }

      void drawBounds(List<Bounds> bounds, Color color)
      {
         GL.Enable(EnableCap.Blend);
         GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
         GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
         GL.Color4(color.R, color.G, color.B, (byte)64);

         GL.Begin(PrimitiveType.Quads);
         foreach (Bounds b in bounds)
         {
            GL.Vertex3(b.x1, b.todo, b.z1);
            GL.Vertex3(b.x1, b.todo, b.z2);
            GL.Vertex3(b.x2, b.todo, b.z2);
            GL.Vertex3(b.x2, b.todo, b.z1);
         }
         GL.End();

         GL.Disable(EnableCap.Blend);
      }

      void drawMissileCarrier(Carrier carrier, int y, Color color)
      {
         GL.Enable(EnableCap.Blend);
         GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
         GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
         GL.Color4(color.R, color.G, color.B, (byte)64);

         double angle = Math.PI * level.carrier.heading / 2048.0;
         // sin/cos swapped so N = 0, W = 1024
         int dx = (int)(level.carrier.distance * Math.Sin(angle));
         int dz = (int)(level.carrier.distance * Math.Cos(angle));

         // front vertices
         Vector3 lbf = new Vector3(carrier.x - 30, y, carrier.z);
         Vector3 rbf = new Vector3(carrier.x + 30, y, carrier.z);
         Vector3 rtf = new Vector3(carrier.x + 30, y + 60, carrier.z);
         Vector3 ltf = new Vector3(carrier.x - 30, y + 60, carrier.z);
         // back vertices
         Vector3 lbb = new Vector3(carrier.x - 30 + dx, y, carrier.z + dz);
         Vector3 rbb = new Vector3(carrier.x + 30 + dx, y, carrier.z + dz);
         Vector3 rtb = new Vector3(carrier.x + 30 + dx, y + 60, carrier.z + dz);
         Vector3 ltb = new Vector3(carrier.x - 30 + dx, y + 60, carrier.z + dz);

         GL.Begin(PrimitiveType.Quads);
         // bottom
         GL.Normal3(0.0f, -1.0f, 0.0f);
         GL.Vertex3(lbf);
         GL.Vertex3(rbf);
         GL.Vertex3(rbb);
         GL.Vertex3(lbb);
         // left
         GL.Normal3(-1.0f, 0.0f, 0.0f);
         GL.Vertex3(lbf);
         GL.Vertex3(ltf);
         GL.Vertex3(ltb);
         GL.Vertex3(lbb);
         // right
         GL.Normal3(1.0f, 0.0f, 0.0f);
         GL.Vertex3(rbf);
         GL.Vertex3(rtf);
         GL.Vertex3(rtb);
         GL.Vertex3(rbb);
         // front
         GL.Normal3(0.0f, 0.0f, 1.0f);
         GL.Vertex3(lbf);
         GL.Vertex3(rbf);
         GL.Vertex3(rtf);
         GL.Vertex3(ltf);
         // back
         GL.Normal3(0.0f, 0.0f, -1.0f);
         GL.Vertex3(lbb);
         GL.Vertex3(rbb);
         GL.Vertex3(rtb);
         GL.Vertex3(ltb);
         // top
         GL.Normal3(0.0f, 1.0f, 0.0f);
         GL.Vertex3(ltf);
         GL.Vertex3(rtf);
         GL.Vertex3(rtb);
         GL.Vertex3(ltb);
         GL.End();

         GL.Disable(EnableCap.Blend);
      }

      void drawCollision(List<CollisionGroup> collisions, bool wireframe, Color color)
      {
         if (wireframe)
         {
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-1.0f, -1.0f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.LineWidth(0.5f);
         }
         else
         {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
         }

         GL.Color3(color);
         GL.Begin(PrimitiveType.Triangles);
         foreach (CollisionGroup cg in collisions)
         {
            foreach (CollisionTri tri in cg.triangles)
            {
               GL.Normal3(GetNormal(tri));
               GL.Vertex3(tri.x1, tri.y1, tri.z1);
               GL.Vertex3(tri.x2, tri.y2, tri.z2);
               GL.Vertex3(tri.x3, tri.y3, tri.z3);
            }
         }
         GL.End();

         if (wireframe)
         {
            GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.PolygonOffsetLine);
         }
      }

      void drawTerrain(List<TerrainGroup> terrainGroups, bool wireframe)
      {
         if (wireframe)
         {
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-1.0f, -1.0f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Color3(Color.Black);
            GL.LineWidth(0.5f);
         }
         else
         {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
         }

         GL.Begin(PrimitiveType.Triangles);
         foreach (TerrainGroup tg in level.terrainGroups)
         {
            foreach (TerrainTri tri in tg.triangles)
            {
               Color triColor = Color.Black;
               if (!wireframe)
               {
                  switch (tri.b12)
                  {
                     case 0x01: triColor = Color.Yellow; break; // low traction / dirt
                     case 0x02: triColor = Color.DarkGray; break; // high traction, high speed / roads, rails
                     case 0x03: triColor = Color.ForestGreen; break; // high traction, medium speed / grass
                     case 0x05: triColor = Color.Red; break; // slow speed / ponds
                     case 0x67: triColor = Color.LightGray; break; // high traction, high speed / gravel lots (similar to roads?)
                  }
               }
               GL.Color3(triColor);
               GL.Normal3(GetNormal(tri));
               GL.Vertex3(tri.x1, tri.y1, tri.z1);
               GL.Vertex3(tri.x2, tri.y2, tri.z2);
               GL.Vertex3(tri.x3, tri.y3, tri.z3);
            }
         }
         GL.End();

         if (wireframe)
         {
            GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.PolygonOffsetLine);
         }
      }

      private void drawWalls(List<WallGroup> wallGroups, bool wireframe)
      {
         if (wireframe)
         {
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-1.0f, -1.0f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Color3(Color.Black);
            GL.LineWidth(0.5f);
         }
         else
         {
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Color3(Color.Beige);
         }

         GL.Begin(PrimitiveType.Triangles);
         foreach (WallGroup wg in level.wallGroups)
         {
            foreach (Wall wall in wg.walls)
            {
               GL.Normal3(GetNormal(wall));
               GL.Vertex3(wall.x1, wall.y1, wall.z1);
               GL.Vertex3(wall.x2, wall.y2, wall.z2);
               GL.Vertex3(wall.x3, wall.y3, wall.z3);
            }
         }
         GL.End();

         if (wireframe)
         {
            GL.Disable(EnableCap.LineSmooth);
            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.PolygonOffsetLine);
         }
      }

      private Color indexToColor(int type, int index)
      {
         int red = (index >> 8) & 0xFF;
         int green = (index) & 0xFF;
         int blue = type & 0xFF;
         return Color.FromArgb(255, red, green, blue);
      }

      private void drawAllObjects(bool paintByNumber)
      {
         Color color = Color.MistyRose;
         int index;
         index = 0;
         foreach (AmmoBox ammo in level.ammoBoxes)
         {
            GL.PushMatrix();
            GL.Translate((float)ammo.x, (float)ammo.y, (float)ammo.z);
            GL.Scale(15f, 15f, 15f);
            if (paintByNumber)
            {
               color = indexToColor(0x20, index);
               index++;
            }
            else
            {
               switch (ammo.type)
               {
                  case 0: color = Color.Black; break;
                  case 1: color = Color.CadetBlue; break;
               }
            }
            drawCube(false, color);
            GL.PopMatrix();
         }
         index = 0;
         foreach (CommPoint comm in level.commPoints)
         {
            GL.PushMatrix();
            GL.Translate((float)comm.x, (float)comm.y, (float)comm.z);
            GL.Scale(15f, 30f, 15f);
            if (paintByNumber)
            {
               color = indexToColor(0x28, index);
               index++;
            }
            else
            {
               color = Color.Empty;
            }
            drawComm(color);
            GL.PopMatrix();
         }
         index = 0;
         foreach (RDU rdu in level.rdus)
         {
            GL.PushMatrix();
            GL.Translate((float)rdu.x, (float)rdu.y, (float)rdu.z);
            GL.Scale(5f, 5f, 5f);
            if (paintByNumber)
            {
               color = indexToColor(0x34, index);
               index++;
            }
            else
            {
               color = Color.Empty;
            }
            drawRdu(color);
            GL.PopMatrix();
         }
         index = 0;
         foreach (TNTCrate tnt in level.tntCrates)
         {
            GL.PushMatrix();
            GL.Translate((float)tnt.x, (float)tnt.y, (float)tnt.z);
            GL.Scale(20f, 20f, 20f);
            if (paintByNumber)
            {
               color = indexToColor(0x38, index);
               index++;
            }
            else
            {
               color = Color.Black;
            }
            drawCube(false, color);
            GL.PopMatrix();
         }
         index = 0;
         foreach (SquareBlock block in level.squareBlocks)
         {
            if (block.type == SquareBlock.Type.Hole)
            {
               drawHole(block, Color.Magenta);
            }
            else
            {
               if (paintByNumber)
               {
                  color = indexToColor(0x3C, index);
                  index++;
               }
               else
               {
                  color = Color.DarkSlateGray;
               }
               switch (block.shape)
               {
                  case SquareBlock.Shape.Square:
                     GL.PushMatrix();
                     GL.Translate((float)block.x, (float)block.y, (float)block.z);
                     GL.Scale(50f, 20f, 50f);
                     drawCube(false, color);
                     GL.PopMatrix();
                     break;
                  case SquareBlock.Shape.Diamond1:
                  case SquareBlock.Shape.Diamond2:
                     GL.PushMatrix();
                     GL.Rotate(MathHelper.PiOver4, 0.0f, 1.0f, 0.0f);
                     GL.Translate((float)block.x, (float)block.y, (float)block.z);
                     GL.Scale(50f, 20f, 50f);
                     drawCube(false, color);
                     GL.PopMatrix();
                     break;
               }
            }
         }
         index = 0;
         // TODO: vehicles 0x50
         index = 0;
         foreach (Building b in level.buildings)
         {
            if (paintByNumber)
            {
               color = indexToColor(0x5C, index);
               index++;
            }
            else
            {
               color = Color.SandyBrown;
            }
            GL.PushMatrix();
            GL.Translate((float)b.x, (float)b.y, (float)b.z);
            GL.Scale(50f, 50f, 50f);
            drawCube(false, color);
            GL.PopMatrix();
         }
         index = 0;
         foreach (TrainPlatform platform in level.trainPlatforms)
         {
            drawPlatform(platform, Color.SaddleBrown);
         }
         index = 0;
         foreach (Object60 obj in level.object60s)
         {
            GL.PushMatrix();
            GL.Translate((float)obj.x, (float)obj.y, (float)obj.z);
            GL.Scale(4.0f, obj.h6, 4.0f);
            drawCube(false, Color.Green);
            GL.PopMatrix();
         }
         index = 0;
         foreach (Object58 obj in level.object58s)
         {
            GL.PushMatrix();
            GL.Translate((float)obj.x, (float)obj.y, (float)obj.z);
            GL.Scale(10f, 10f, 10f);
            drawCube(false, Color.Magenta);
            GL.PopMatrix();
         }
      }

      private void paintByNumber()
      {
         // disable some caps (just want flat objects)
         GL.PushAttrib(AttribMask.AllAttribBits);
         GL.Disable(EnableCap.Fog);
         GL.Disable(EnableCap.Texture2D);
         GL.Disable(EnableCap.Dither);
         GL.Disable(EnableCap.Lighting);
         GL.Disable(EnableCap.LineStipple);
         GL.Disable(EnableCap.PolygonStipple);
         GL.Disable(EnableCap.CullFace);
         GL.Disable(EnableCap.Blend);
         GL.Disable(EnableCap.AlphaTest);

         // clear buffer
         GL.ClearColor(Color.White);
         GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

         GL.MatrixMode(MatrixMode.Modelview);
         GL.LoadMatrix(ref cameraMatrix);
         GL.PushMatrix();
         GL.Scale(scale, scale, scale);

         // draw objects
         drawAllObjects(true);

         GL.PopMatrix();

         // restore caps
         GL.PopAttrib();
      }

      private void doPicking(int mX, int mY)
      {
         // draw to back buffer
         paintByNumber();

         // read pixel color at mouse position mX,mY
         byte[] pixel = new byte[3];
         int[] viewport = new int[4];
         // Y-axis flipped between OpenGL and window
         GL.GetInteger(GetPName.Viewport, viewport);
         GL.ReadPixels(mX, viewport[3] - mY - glControlViewer.Location.Y, 1, 1, PixelFormat.Rgb, PixelType.UnsignedByte, pixel);
         // reverse color mapping from RGB to type/list index
         int type = (int)pixel[2];
         int index = (int)pixel[1] + (((int)pixel[0]) << 8);
         BlastCorpsItem curItem = selectedItem;
         if (index > -1)
         {
            labelType.Text = String.Format("Type: {0:X02}", type);
            labelIndex.Text = String.Format("Index: {0:X04}", index);
            switch (type)
            {
               case 0x20: // Ammo
                  if (index < level.ammoBoxes.Count)
                  {
                     selectedItem = level.ammoBoxes[index];
                  }
                  break;
               case 0x28: // Comm Pt.
                  if (index < level.commPoints.Count)
                  {
                     selectedItem = level.commPoints[index];
                  }
                  break;
               case 0x34: // RDU
                  if (index < level.rdus.Count)
                  {
                     selectedItem = level.rdus[index];
                  }
                  break;
               case 0x38: // TNT
                  if (index < level.tntCrates.Count)
                  {
                     selectedItem = level.tntCrates[index];
                  }
                  break;
               case 0x3C: // square blocks
                  if (index < level.squareBlocks.Count)
                  {
                     selectedItem = level.squareBlocks[index];
                  }
                  break;
               case 0x50: // vehicles
                  if (index < level.vehicles.Count)
                  {
                     selectedItem = level.vehicles[index];
                  }
                  break;
               case 0x54: // missile carrier
                  selectedItem = level.carrier;
                  break;
               case 0x5C: // buildings
                  if (index < level.buildings.Count)
                  {
                     selectedItem = level.buildings[index];
                  }
                  break;
               case 0xFF: // nothing
                  selectedItem = null;
                  break;
            }
            if (curItem != selectedItem)
            {
               glControlViewer.Invalidate();
            }
         }
      }

      private void glControlViewer_Paint(object sender, PaintEventArgs e)
      {
         if (!loaded) return;

         GL.ClearColor(Color.CornflowerBlue);
         GL.MatrixMode(MatrixMode.Modelview);
         GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         GL.LoadMatrix(ref cameraMatrix);

         GL.PushMatrix();
         GL.Scale(scale, scale, scale);

         if (levelModel != null && checkBoxDisplay.Checked)
         {
            drawDisplayList(false);
            if (checkBoxWireframe.Checked)
            {
               drawDisplayList(true);
            }
         }

         if (level != null)
         {
            // draw terrain and collision first so transparent objects can blend
            if (checkBoxTerrain.Checked)
            {
               drawTerrain(level.terrainGroups, false);
               if (checkBoxWireframe.Checked)
               {
                  drawTerrain(level.terrainGroups, true);
               }
            }

            if (checkBoxCollision24.Checked)
            {
               drawCollision(level.collision24, false, Color.Magenta);
               if (checkBoxWireframe.Checked)
               {
                  drawCollision(level.collision24, true, Color.Black);
               }
            }

            if (checkBoxCollision6C.Checked)
            {
               drawCollision(level.collision6C, false, Color.DarkOrange);
               if (checkBoxWireframe.Checked)
               {
                  drawCollision(level.collision6C, true, Color.Black);
               }
            }

            if (checkBoxWalls.Checked)
            {
               drawWalls(level.wallGroups, false);
               if (checkBoxWireframe.Checked)
               {
                  drawWalls(level.wallGroups, true);
               }
            }

            drawAllObjects(false);

            if (selectedItem != null)
            {
               float arrowSize = 20f;
               float arrowOffset = 2 * arrowSize;
               if (selectedItem is RDU)
               {
                  arrowOffset -= 10.0f;
               }
               else if (selectedItem is Building)
               {
                  arrowOffset += 40.0f;
               }
               GL.PushMatrix();
               GL.Translate((float)selectedItem.x, (float)selectedItem.y + arrowOffset, (float)selectedItem.z);
               // billboard the arrow
               GL.Rotate(270.0f - MathHelper.RadiansToDegrees(facing), 0.0f, 1.0f, 0.0f);
               GL.Scale(arrowSize/2, arrowSize, arrowSize);
               drawArrow(Color.Magenta);
               GL.PopMatrix();
            }

            if (level.carrier.distance != 0)
            {
               int y = Int16.MinValue;
               int buildingCount = level.buildings.Count;
               foreach (Building b in level.buildings)
               {
                  if (b.type == 56) // semi
                  {
                     y = b.y;
                     break;
                  }
               }
               // if no semi found in level (such as end sequence), average of first and last building Y
               if (y == Int16.MinValue && buildingCount > 0)
               {
                  y = (level.buildings[0].y + level.buildings[buildingCount - 1].y) / 2;
               }
               drawMissileCarrier(level.carrier, y, Color.Red);
            }

            // bounds 0x40 and 0x44 last for alpha blending
            if (checkBox40.Checked)
            {
               drawBounds(level.bounds40, Color.Green);
            }

            if (checkBox44.Checked)
            {
               drawBounds(level.bounds44, Color.Aqua);
            }
         }

         GL.PopMatrix();

         glControlViewer.SwapBuffers();
      }

      private void glControlViewer_KeyDown(object sender, KeyEventArgs e)
      {
         if (!loaded) return;

         switch (e.KeyCode)
         {
            case Keys.W:
               location.X += (float)Math.Cos(facing) * 0.1f;
               location.Z += (float)Math.Sin(facing) * 0.1f;
               break;
            case Keys.S:
               location.X -= (float)Math.Cos(facing) * 0.1f;
               location.Z -= (float)Math.Sin(facing) * 0.1f;
               break;
            case Keys.A:
               location.X -= (float)Math.Cos(facing + MathHelper.PiOver2) * 0.1f;
               location.Z -= (float)Math.Sin(facing + MathHelper.PiOver2) * 0.1f;
               break;
            case Keys.D:
               location.X += (float)Math.Cos(facing + MathHelper.PiOver2) * 0.1f;
               location.Z += (float)Math.Sin(facing + MathHelper.PiOver2) * 0.1f;
               break;
            case Keys.Q:
               location.Y += 0.1f;
               break;
            case Keys.E:
               location.Y -= 0.1f;
               break;
         }

         SetupViewport();

         glControlViewer.Invalidate();
      }

      private void glControlViewer_MouseDown(object sender, MouseEventArgs e)
      {
         switch (e.Button)
         {
            case System.Windows.Forms.MouseButtons.Left:
               doPicking(e.X, e.Y);
               break;
            case System.Windows.Forms.MouseButtons.Right:
               lastX = e.X;
               lastY = e.Y;
               break;
         }
      }

      private void glControlViewer_MouseMove(object sender, MouseEventArgs e)
      {
         if (e.Button == System.Windows.Forms.MouseButtons.Right)
         {
            int diffX = (e.X - lastX);
            int diffY = (e.Y - lastY);

            lastX = e.X;
            lastY = e.Y;

            mouseSpeed[0] *= 0.9f;
            mouseSpeed[1] *= 0.9f;
            mouseSpeed[0] += diffX / 10f;
            mouseSpeed[1] += diffY / 10f;

            facing += diffX * 0.001f;
            pitch += diffY * 0.001f;

            SetupViewport();

            glControlViewer.Invalidate();
         }
      }

      private void SetupViewport()
      {
         Vector3 lookatPoint = new Vector3((float)Math.Cos(facing), pitch, (float)Math.Sin(facing));
         cameraMatrix = Matrix4.LookAt(location, location + lookatPoint, up);
         labelX.Text = String.Format("X: {0:F3}", location.X);
         labelY.Text = String.Format("Y: {0:F3}", location.Y);
         labelZ.Text = String.Format("Z: {0:F3}", location.Z);
         labelHeading.Text = String.Format("H: {0:F3}", facing);
         labelPitch.Text = String.Format("P: {0:F3}", pitch);
      }

      private void settingCheckedChanged(object sender, EventArgs e)
      {
         glControlViewer.Invalidate();
      }
   }
}
