using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;

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

      const float scale = 50f / (256f * 256f);

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

         GL.Enable(EnableCap.ColorMaterial);
      }

      private void drawCube(Color cubeColor)
      {
         GL.Begin(PrimitiveType.Quads);
         GL.Color3(cubeColor);
         // front
         GL.Normal3(0.0f, 0.0f, 1.0f);
         GL.Vertex3(0.0f, 0.0f, 0.0f);
         GL.Vertex3(1.0f, 0.0f, 0.0f);
         GL.Vertex3(1.0f, 1.0f, 0.0f);
         GL.Vertex3(0.0f, 1.0f, 0.0f);
         // back
         GL.Normal3(0.0f, 0.0f, -1.0f);
         GL.Vertex3(0.0f, 0.0f, -1.0f);
         GL.Vertex3(1.0f, 0.0f, -1.0f);
         GL.Vertex3(1.0f, 1.0f, -1.0f);
         GL.Vertex3(0.0f, 1.0f, -1.0f);
         // right
         GL.Normal3(1.0f, 0.0f, 0.0f);
         GL.Vertex3(1.0f, 0.0f, 0.0f);
         GL.Vertex3(1.0f, 0.0f, -1.0f);
         GL.Vertex3(1.0f, 1.0f, -1.0f);
         GL.Vertex3(1.0f, 1.0f, 0.0f);
         // left
         GL.Normal3(-1.0f, 0.0f, 0.0f);
         GL.Vertex3(0.0f, 0.0f, 0.0f);
         GL.Vertex3(0.0f, 0.0f, -1.0f);
         GL.Vertex3(0.0f, 1.0f, -1.0f);
         GL.Vertex3(0.0f, 1.0f, 0.0f);
         // top
         GL.Normal3(0.0f, 1.0f, 0.0f);
         GL.Vertex3(0.0f, 1.0f, 0.0f);
         GL.Vertex3(1.0f, 1.0f, 0.0f);
         GL.Vertex3(1.0f, 1.0f, -1.0f);
         GL.Vertex3(0.0f, 1.0f, -1.0f);
         // bottom
         GL.Normal3(0.0f, -1.0f, 0.0f);
         GL.Vertex3(0.0f, 0.0f, 0.0f);
         GL.Vertex3(1.0f, 0.0f, 0.0f);
         GL.Vertex3(1.0f, 0.0f, -1.0f);
         GL.Vertex3(0.0f, 0.0f, -1.0f);
         GL.End();
      }

      private void drawRdu()
      {
         GL.Begin(PrimitiveType.Triangles);
         GL.Color3(Color.Yellow);
         GL.Vertex3( 0.0f, 1.0f, 0.0f);  // Top Of Triangle (Front)
         GL.Color3(Color.Orange);
         GL.Vertex3(-1.0f, 0.0f, 1.0f);  // Left Of Triangle (Front)
         GL.Color3(Color.Brown);
         GL.Vertex3( 1.0f, 0.0f, 1.0f);  // Right Of Triangle (Front)
         GL.Color3(Color.Yellow);
         GL.Vertex3( 0.0f, 1.0f, 0.0f);  // Top Of Triangle (Right)
         GL.Color3(Color.Brown);
         GL.Vertex3( 1.0f, 0.0f, 1.0f);  // Left Of Triangle (Right)
         GL.Color3(Color.Orange);
         GL.Vertex3( 1.0f, 0.0f, -1.0f); // Right Of Triangle (Right)
         GL.Color3(Color.Yellow);
         GL.Vertex3( 0.0f, 1.0f, 0.0f);  // Top Of Triangle (Back)
         GL.Color3(Color.Orange);
         GL.Vertex3( 1.0f, 0.0f, -1.0f); // Left Of Triangle (Back)
         GL.Color3(Color.Brown);
         GL.Vertex3(-1.0f, 0.0f, -1.0f); // Right Of Triangle (Back)
         GL.Color3(Color.Yellow);
         GL.Vertex3( 0.0f, 1.0f, 0.0f);  // Top Of Triangle (Left)
         GL.Color3(Color.Brown);
         GL.Vertex3(-1.0f, 0.0f,-1.0f);  // Left Of Triangle (Left)
         GL.Color3(Color.Orange);
         GL.Vertex3(-1.0f, 0.0f, 1.0f);  // Right Of Triangle (Left)
         GL.End();
      }

      private void glControlViewer_Paint(object sender, PaintEventArgs e)
      {
         if (!loaded) return;

         GL.ClearColor(Color.CornflowerBlue);
         GL.MatrixMode(MatrixMode.Modelview);
         GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
         GL.Enable(EnableCap.DepthTest);
         GL.LoadMatrix(ref cameraMatrix);

         if (levelModel != null && checkBoxDisplay.Checked)
         {
            GL.PushMatrix();
            GL.Scale(scale, scale, scale);
            
            // fill
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Color3(Color.Gray);
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

            // line
            GL.Enable(EnableCap.LineSmooth);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
            GL.Enable(EnableCap.PolygonOffsetLine);
            GL.PolygonOffset(-1.0f, -1.0f);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.Color3(Color.Black);
            GL.LineWidth(1.0f);
            GL.Begin(PrimitiveType.Triangles);
            foreach (Triangle tri in levelModel.triangles)
            {
               foreach (Vertex v in tri.vertices)
               {
                  GL.Vertex3(v.x, v.y, v.z);
               }
            }
            GL.End();
            GL.PopMatrix();
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.Disable(EnableCap.PolygonOffsetLine);
         }

         if (level != null)
         {
            foreach (RDU rdu in level.rdus)
            {
               GL.PushMatrix();
               GL.Scale(scale, scale, scale);
               GL.Translate((float)rdu.x, (float)rdu.y, (float)rdu.z);
               GL.Scale(5f, 5f, 5f);
               drawRdu();
               GL.PopMatrix();
            }
            foreach (TNTCrate tnt in level.tntCrates)
            {
               GL.PushMatrix();
               GL.Scale(scale, scale, scale);
               GL.Translate((float)tnt.x, (float)tnt.y, (float)tnt.z);
               GL.Scale(20f, 20f, 20f);
               drawCube(Color.Black);
               GL.PopMatrix();
            }

            if (checkBoxCollision6C.Checked)
            {
               GL.PushMatrix();
               GL.Scale(scale, scale, scale);
               GL.Color3(Color.DarkOrange);
               GL.Begin(PrimitiveType.Triangles);
               foreach (CollisionGroup cg in level.collision6C)
               {
                  foreach (CollisionTri tri in cg.triangles)
                  {
                     GL.Vertex3(tri.x1, tri.y1, tri.z1);
                     GL.Vertex3(tri.x2, tri.y2, tri.z2);
                     GL.Vertex3(tri.x3, tri.y3, tri.z3);
                  }
               }
               GL.End();
               GL.PopMatrix();
            }

            if (checkBoxTerrain.Checked)
            {
               GL.PushMatrix();
               GL.Scale(scale, scale, scale);
               GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
               GL.Begin(PrimitiveType.Triangles);
               foreach (TerrainGroup tg in level.terrainGroups)
               {
                  foreach (TerrainTri tri in tg.triangles)
                  {
                     Color triColor = Color.Honeydew;
                     switch (tri.b12)
                     {
                        case 0x01: triColor = Color.Brown; break; // low traction / dirt
                        case 0x02: triColor = Color.DarkGray; break; // high traction, high speed / roads, rails
                        case 0x03: triColor = Color.ForestGreen; break; // high traction, medium speed / grass
                        case 0x05: triColor = Color.Yellow; break; // slow speed / ponds
                        case 0x67: triColor = Color.LightGray; break; // high traction, high speed / gravel lots (similar to roads?)
                     }
                     GL.Color3(triColor);
                     GL.Vertex3(tri.x1, tri.y1, tri.z1);
                     GL.Vertex3(tri.x2, tri.y2, tri.z2);
                     GL.Vertex3(tri.x3, tri.y3, tri.z3);
                  }
               }
               GL.End();

               // line
               GL.Enable(EnableCap.LineSmooth);
               GL.Enable(EnableCap.Blend);
               GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
               GL.Hint(HintTarget.LineSmoothHint, HintMode.DontCare);
               GL.Enable(EnableCap.PolygonOffsetLine);
               GL.PolygonOffset(-1.0f, -1.0f);
               GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
               GL.Color3(Color.Black);
               GL.LineWidth(1.0f);
               GL.Begin(PrimitiveType.Triangles);
               foreach (TerrainGroup tg in level.terrainGroups)
               {
                  foreach (TerrainTri tri in tg.triangles)
                  {
                     GL.Vertex3(tri.x1, tri.y1, tri.z1);
                     GL.Vertex3(tri.x2, tri.y2, tri.z2);
                     GL.Vertex3(tri.x3, tri.y3, tri.z3);
                  }
               }
               GL.End();
               GL.PopMatrix();
               GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
               GL.Disable(EnableCap.PolygonOffsetLine);
            }
         }

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
         if (e.Button == System.Windows.Forms.MouseButtons.Left)
         {
            lastX = e.X;
            lastY = e.Y;
         }
      }

      private void glControlViewer_MouseMove(object sender, MouseEventArgs e)
      {
         if (e.Button == System.Windows.Forms.MouseButtons.Left)
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
