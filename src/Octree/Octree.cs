using Godot;
using Array = Godot.Collections;
using System.Collections.Generic;


namespace OctreeLib
{
    public class Cube
    {
        public ImmediateMesh DebugMesh { get; private set; }
        public MeshInstance3D DebugMeshInstance { get; private set; }
        public OctreeRoot Parent { get; set; }

        public Vector3 Center { get; private set; }
        public Vector3 Dimensions { get; private set; }

        public Cube(Vector3 center, Vector3 dimensions)
        {
            Center = center;
            Dimensions = dimensions;
        }

        public Cube(float x, float y, float z, Vector3 dimensions)
        {
            Center = new Vector3(x, y, z);
            Dimensions = dimensions;
        }

        public Cube(Vector3 center, float w, float h, float d)
        {
            Center = center;
            Dimensions = new Vector3(w, h, d);
        }

        public Cube(float x, float y, float z, float w, float h, float d)
        {
            Center = new Vector3(x, y, z);
            Dimensions = new Vector3(w, h, d);
        }

        public bool Contains(Vector3 point)
        {
            return (
                point.X > Center.X - Dimensions.X &&
                point.X < Center.X + Dimensions.X &&
                point.Y > Center.Y - Dimensions.Y &&
                point.Y < Center.Y + Dimensions.Y &&
                point.Z > Center.Z - Dimensions.Z &&
                point.Z < Center.Z + Dimensions.Z
            );
        }

        public void DebugDraw()
        {
            if (Parent.Debug) {
                if (DebugMesh == null) {
                    DebugMesh = new ImmediateMesh();
                    DebugMeshInstance = new MeshInstance3D();
                    DebugMeshInstance.Mesh = DebugMesh;
                    Parent.AddChild(DebugMeshInstance);
                }

                Vector3 P1 = new Vector3(
                    Center.X - Dimensions.X,
                    Center.Y - Dimensions.Y,
                    Center.Z + Dimensions.Z
                );

                Vector3 P2 = new Vector3(
                    Center.X - Dimensions.X,
                    Center.Y - Dimensions.Y,
                    Center.Z - Dimensions.Z
                );

                Vector3 P3 = new Vector3(
                    Center.X + Dimensions.X,
                    Center.Y - Dimensions.Y,
                    Center.Z + Dimensions.Z
                );

                Vector3 P4 = new Vector3(
                    Center.X + Dimensions.X,
                    Center.Y - Dimensions.Y,
                    Center.Z - Dimensions.Z
                );

                Vector3 P5 = new Vector3(
                    Center.X - Dimensions.X,
                    Center.Y + Dimensions.Y,
                    Center.Z + Dimensions.Z
                );

                Vector3 P6 = new Vector3(
                    Center.X - Dimensions.X,
                    Center.Y + Dimensions.Y,
                    Center.Z - Dimensions.Z
                );

                Vector3 P7 = new Vector3(
                    Center.X + Dimensions.X,
                    Center.Y + Dimensions.Y,
                    Center.Z + Dimensions.Z
                );

                Vector3 P8 = new Vector3(
                    Center.X + Dimensions.X,
                    Center.Y + Dimensions.Y,
                    Center.Z - Dimensions.Z
                );

                DebugMesh.ClearSurfaces();
                DebugMesh.SurfaceBegin(Mesh.PrimitiveType.Lines);

                DebugMesh.SurfaceAddVertex(P1);
                DebugMesh.SurfaceAddVertex(P2);

                DebugMesh.SurfaceAddVertex(P1);
                DebugMesh.SurfaceAddVertex(P3);

                DebugMesh.SurfaceAddVertex(P2);
                DebugMesh.SurfaceAddVertex(P4);

                DebugMesh.SurfaceAddVertex(P3);
                DebugMesh.SurfaceAddVertex(P4);

                DebugMesh.SurfaceAddVertex(P5);
                DebugMesh.SurfaceAddVertex(P6);

                DebugMesh.SurfaceAddVertex(P5);
                DebugMesh.SurfaceAddVertex(P7);

                DebugMesh.SurfaceAddVertex(P6);
                DebugMesh.SurfaceAddVertex(P8);

                DebugMesh.SurfaceAddVertex(P7);
                DebugMesh.SurfaceAddVertex(P8);

                DebugMesh.SurfaceAddVertex(P1);
                DebugMesh.SurfaceAddVertex(P5);

                DebugMesh.SurfaceAddVertex(P2);
                DebugMesh.SurfaceAddVertex(P6);

                DebugMesh.SurfaceAddVertex(P3);
                DebugMesh.SurfaceAddVertex(P7);

                DebugMesh.SurfaceAddVertex(P4);
                DebugMesh.SurfaceAddVertex(P8);

                DebugMesh.SurfaceEnd();
            } else {
                if (DebugMesh != null) {
                    Parent.RemoveChild(DebugMeshInstance);
                    DebugMeshInstance.QueueFree();
                }
            }
        }
    }

    public class Octree
    {
        public Cube Boundary { get; private set; }
        public bool Debug = false;

        private int capacity;
        private List<Vector3> points;

        public bool IsDivided { get; private set; }
        public Godot.Collections.Array<ImmediateMesh> DebugMeshes;
        public OctreeRoot Parent { get; set; }

        private Octree NEB;
        private Octree NWB;
        private Octree SEB;
        private Octree SWB;
        private Octree NEF;
        private Octree NWF;
        private Octree SEF;
        private Octree SWF;

        public Octree(Cube boundary, int capacity)
        {
            Boundary = boundary;
            this.capacity = capacity;

            points = new List<Vector3>();
            IsDivided = false;

            DebugMeshes = new Godot.Collections.Array<ImmediateMesh>();
        }

        public bool Insert(Vector3 point)
        {
            if (!Boundary.Contains(point)) return false;

            // We can add it without subdividing
            if (points.Count < capacity) {
                points.Add(point);
                return true;
            }
            // Time to subdivide babyee
            else {
                if (!IsDivided) Subdivide();

                if (NEB.Insert(point)) return true;
                else if (NWB.Insert(point)) return true;
                else if (SEB.Insert(point)) return true;
                else if (SWB.Insert(point)) return true;
                else if (NEF.Insert(point)) return true;
                else if (NWF.Insert(point)) return true;
                else if (SEF.Insert(point)) return true;
                else if (SWF.Insert(point)) return true;
            }

            return false;
        }

        public void DebugDraw()
        {
            if (!IsDivided) return;

            Boundary.DebugDraw();

            NEB.DebugDraw();
            NWB.DebugDraw();
            SEB.DebugDraw();
            SWB.DebugDraw();
            NEF.DebugDraw();
            NWF.DebugDraw();
            SEF.DebugDraw();
            SWF.DebugDraw();
        }

        private void Subdivide()
        {
            Boundary.Parent = Parent;

            float x = Boundary.Center.X;
            float y = Boundary.Center.Y;
            float z = Boundary.Center.Z;

            float w = Boundary.Dimensions.X;
            float h = Boundary.Dimensions.Y;
            float d = Boundary.Dimensions.Z;

            Cube neb = new Cube(x + w / 2, y + h / 2, z + d / 2, w / 2, h / 2, d / 2);
            Cube nwb = new Cube(x - w / 2, y + h / 2, z + d / 2, w / 2, h / 2, d / 2);
            Cube seb = new Cube(x + w / 2, y - h / 2, z + d / 2, w / 2, h / 2, d / 2);
            Cube swb = new Cube(x - w / 2, y - h / 2, z + d / 2, w / 2, h / 2, d / 2);

            Cube nef = new Cube(x + w / 2, y + h / 2, z - d / 2, w / 2, h / 2, d / 2);
            Cube nwf = new Cube(x - w / 2, y + h / 2, z - d / 2, w / 2, h / 2, d / 2);
            Cube sef = new Cube(x + w / 2, y - h / 2, z - d / 2, w / 2, h / 2, d / 2);
            Cube swf = new Cube(x - w / 2, y - h / 2, z - d / 2, w / 2, h / 2, d / 2);

            NEB = new Octree(neb, capacity);
            NWB = new Octree(nwb, capacity);
            SEB = new Octree(seb, capacity);
            SWB = new Octree(swb, capacity);
            NEF = new Octree(nef, capacity);
            NWF = new Octree(nwf, capacity);
            SEF = new Octree(sef, capacity);
            SWF = new Octree(sef, capacity);

            NEB.Parent = Parent;
            NWB.Parent = Parent;
            SEB.Parent = Parent;
            SWB.Parent = Parent;
            NEF.Parent = Parent;
            NWF.Parent = Parent;
            SEF.Parent = Parent;
            SWF.Parent = Parent;

            IsDivided = true;
        }
    }
}
