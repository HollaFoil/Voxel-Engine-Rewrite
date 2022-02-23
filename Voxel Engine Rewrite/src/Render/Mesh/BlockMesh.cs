using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Voxel_Engine_Rewrite.src.Render.Mesh
{
    internal static class BlockMesh
    {
        public struct Data
        {
            public byte faces;
            public byte tex;
            public byte x;
            public byte y;
            public byte z;
            public int chunkx;
            public int chunky;
            public Data(byte Faces, byte Tex, byte X, byte Y, byte Z, int Chunkx, int Chunky)
            {
                faces = Faces;
                tex = Tex;
                x = X;
                y = Y;
                z = Z;
                chunkx = Chunkx;
                chunky = Chunky;
            }
        }
        public static World.World world = null;
        static BlockMesh()
        {
            for (int i = 0; i < 64; i++)
            {
                faceCounts[i] = Convert.ToString(i, 2).ToCharArray().Count(c => c == '1');
            }
            world = Game.GetWorld();
        }
        public static byte[] GetVertices(Data data, out int length)
        {
            //block byte xyz, vertex id byte, tex id byte, byte AO, chunk int x, y
            int xworld = data.x + data.chunkx * 16, 
                zworld = data.z + data.chunky * 16;

            byte[] vertices = ArrayPool<byte>.Shared.Rent(faceCounts[data.faces] * Constants.FaceSize);
            length = faceCounts[data.faces] * Constants.FaceSize;
            int currentByte = 0;

            for (int i = 0, face = 1; i < 6; i++, face *= 2)
            {
                if ((data.faces & face) == 0) continue;
                for (int j = 0; j < 6; j++)
                {
                    vertices[currentByte] = data.x;
                    vertices[currentByte + 1] = data.y;
                    vertices[currentByte + 2] = data.z;
                    vertices[currentByte + 3] = indices[i * 6 + j];
                    vertices[currentByte + 4] = faceTexIds[data.tex - 1, i];
                    vertices[currentByte + 5] = GetAOOfVertex(xworld, data.y, zworld, indices[i * 6 + j]);

                    int[] chunkcoords = { data.chunkx, data.chunky };
                    CopyToByteArray(currentByte + 6, ref vertices, ref chunkcoords);
                    currentByte += Constants.VertexSize;
                }
            }
            return vertices;
        }
        private static void CopyToByteArray(int offset, ref byte[] destination, ref int[] ToCopy)
        {
            Buffer.BlockCopy(ToCopy, 0, destination, offset, 2 * sizeof(int));
        }
        private static byte GetAOOfVertex(int x, int y, int z, int vertId)
        {

            byte? block;
            int side1, side2, corner;
            int val = 0;
            switch (vertId)
            {
                case 0:
                    block = world.GetBlockType(x - 1, y, z + 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y - 1, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y - 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 1:
                    block = world.GetBlockType(x + 1, y, z + 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y - 1, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y - 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 2:
                    block = world.GetBlockType(x - 1, y, z + 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y + 1, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y + 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 3:
                    block = world.GetBlockType(x + 1, y, z + 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y + 1, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y + 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 4:
                    block = world.GetBlockType(x + 1, y, z + 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y - 1, z);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y - 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 5:
                    block = world.GetBlockType(x + 1, y, z - 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y - 1, z);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y - 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 6:
                    block = world.GetBlockType(x + 1, y, z + 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y + 1, z);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y + 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 7:
                    block = world.GetBlockType(x + 1, y, z - 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y + 1, z);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y + 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 8:
                    block = world.GetBlockType(x + 1, y, z - 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y - 1, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y - 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 9:
                    block = world.GetBlockType(x - 1, y, z - 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y - 1, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y - 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 10:
                    block = world.GetBlockType(x + 1, y, z - 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y + 1, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y + 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 11:
                    block = world.GetBlockType(x - 1, y, z - 1);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y + 1, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y + 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 12:
                    block = world.GetBlockType(x - 1, y - 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y - 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 13:
                    block = world.GetBlockType(x - 1, y - 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y - 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 14:
                    block = world.GetBlockType(x - 1, y + 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y + 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 15:
                    block = world.GetBlockType(x - 1, y + 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y + 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 16:
                    block = world.GetBlockType(x - 1, y - 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y - 1, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y - 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 17:
                    block = world.GetBlockType(x + 1, y - 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y - 1, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y - 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 18:
                    block = world.GetBlockType(x - 1, y - 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y - 1, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y - 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 19:
                    block = world.GetBlockType(x + 1, y - 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y - 1, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y - 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 20:
                    block = world.GetBlockType(x - 1, y + 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y + 1, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y + 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 21:
                    block = world.GetBlockType(x + 1, y + 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y + 1, z + 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y + 1, z + 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 22:
                    block = world.GetBlockType(x - 1, y + 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y + 1, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x - 1, y + 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                case 23:
                    block = world.GetBlockType(x + 1, y + 1, z);
                    side1 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x, y + 1, z - 1);
                    side2 = (block == null || block == 0) ? 0 : 1;
                    block = world.GetBlockType(x + 1, y + 1, z - 1);
                    corner = (block == null || block == 0) ? 0 : 1;
                    return CalcAO(side1, side2, corner);
                default:
                    return 0;
            }
        }
        private static byte CalcAO(int side1, int side2, int corner)
        {
            if (side1 == 1 && side2 == 1) return 0;
            return (byte)(3 - (side1 + side2 + corner));
        }

        static byte[,] faceTexIds = new byte[,]
        {
            {1,1,1,1,1,1 },
            {2,2,2,2,2,2 },
            {5,5,5,5,5,5 },
            {3,3,3,3,2,4 },
            {6,6,6,6,6,6 },
            {7,7,7,7,7,7 },
            {8,8,8,8,9,9 },
            {10,10,10,10,10,10 },
        };
        static byte[] indices = {
                //Faces definition
            0,1,3, 0,3,2,           //Face front
            4,5,7, 4,7,6,           //Face right
            8,9,11, 8,11,10,        //...
            12,13,15, 12,15,14,
            16,17,19, 16,19,18,
            20,21,23, 20,23,22,
        };

        static int[] faceCounts = new int[64];
        // SIDE SIDE SIDE SIDE BOTTOM TOP
    }
}
