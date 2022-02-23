using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Voxel_Engine_Rewrite.src.Util;
using Voxel_Engine_Rewrite.src.World;

namespace Voxel_Engine_Rewrite.src.Render.Mesh
{
    internal class ChunkMesh
    {
        byte[] vertices;
        Chunk chunk;
        int size = 0;
        byte[,,] exposedFaces;
        int arrayPoolId;
        public void Free()
        {
            if (exposedFaces != null)
            {
                ChunkDataArrayPool.Return3DArray(arrayPoolId);
                exposedFaces = null;
                arrayPoolId = 0;
            }
            if (vertices == null) return;
            ArrayPool<byte>.Shared.Return(vertices);
            vertices = null;
        }
        public ChunkMesh(Chunk c)
        {
            chunk = c;
            arrayPoolId = ChunkDataArrayPool.Rent3DArray();
            exposedFaces = ChunkDataArrayPool.GetArray(arrayPoolId);
        }
        public byte[] GetMesh()
        {
            int faces = CheckExposedFaces();
            vertices = GenerateMesh(faces);
            return vertices;
        }
        private byte[] GenerateMesh(int faces)
        {
            byte[] data = ArrayPool<byte>.Shared.Rent(faces * Constants.VertexSize);

            int index = 0;
            for (byte x = 0; x < 16; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    for (byte z = 0; z < 16; z++)
                    {
                        if (chunk.GetBlock(x, y, z) == 0) continue;
                        BlockMesh.Data blockdata = new BlockMesh.Data(exposedFaces[x, y, z], chunk.GetBlock(x, y, z), x, (byte)y, z, chunk.location.x, chunk.location.y);
                        byte[] blockmesh = BlockMesh.GetVertices(blockdata, out int size);
                        Buffer.BlockCopy(blockmesh, 0, data, index, size);
                        index += size;
                        ArrayPool<byte>.Shared.Return(data, true);
                    }
                }
            }

            return data;
        }
        private int CheckExposedFaces()
        {
            Chunk? right = Game.GetWorld().GetChunk(-1, 0);
            Chunk? left = Game.GetWorld().GetChunk(1, 0);
            Chunk? down = Game.GetWorld().GetChunk(0, 1);
            Chunk? up = Game.GetWorld().GetChunk(0, -1);
            int faces = 0;
            int index = 0;
            for (int x = 0; x < 16; x++)
            {
                for (int y = 0; y < 256; y++)
                {
                    for (int z = 0; z < 16; z++)
                    {
                        if (chunk.GetBlock(x, y, z) == 0)
                        {
                            exposedFaces[x, y, z] = 0;
                        }
                        byte directions = 0;
                        if (x == 15 && left == null) directions += Constants.LEFT;
                        else if (x == 15 && (left.GetBlock(0, y, z) == 0 || left.GetBlock(0, y, z) == 8)) directions += Constants.LEFT;
                        else if (x < 15 && (chunk.GetBlock(x+1, y, z) == 0 || chunk.GetBlock(x+1, y, z) == 8)) directions += Constants.LEFT;

                        if (z == 15 && down == null) directions += Constants.BACK;
                        else if (z == 15 && (down.GetBlock(x, y, 0) == 0 || down.GetBlock(x, y, 0) == 8)) directions += Constants.BACK;
                        else if (z < 15 && (chunk.GetBlock(x, y, z + 1) == 0 || chunk.GetBlock(x, y, z + 1) == 8)) directions += Constants.BACK;

                        if (x == 0 && right == null) directions += Constants.RIGHT;
                        else if (x == 0 && (right.GetBlock(15, y, z) == 0 || right.GetBlock(15, y, z) == 8)) directions += Constants.RIGHT;
                        else if (x != 0 && (chunk.GetBlock(x - 1, y, z) == 0 || chunk.GetBlock(x - 1, y, z) == 8)) directions += Constants.RIGHT;

                        if (z == 0 && up == null) directions += Constants.FRONT;
                        else if (z == 0 && (up.GetBlock(x, y, 15) == 0 || up.GetBlock(x, y, 15) == 8)) directions += Constants.FRONT;
                        else if (z != 0 && (chunk.GetBlock(x, y, z - 1) == 0 || chunk.GetBlock(x, y, z - 1) == 8)) directions += Constants.FRONT;

                        if (y == 0 || (chunk.GetBlock(x, y - 1, z) == 0 || chunk.GetBlock(x, y - 1, z) == 8)) directions += Constants.BOTTOM;
                        if (y == 255 || (chunk.GetBlock(x, y + 1, z) == 0 || chunk.GetBlock(x, y + 1, z) == 8)) directions += Constants.TOP;
                        exposedFaces[x, y, z] = directions;

                        if ((exposedFaces[x, y, z] & Constants.FRONT) > 0) faces++;
                        if ((exposedFaces[x, y, z] & Constants.RIGHT) > 0) faces++;
                        if ((exposedFaces[x, y, z] & Constants.LEFT) > 0) faces++;
                        if ((exposedFaces[x, y, z] & Constants.BACK) > 0) faces++;
                        if ((exposedFaces[x, y, z] & Constants.TOP) > 0) faces++;
                        if ((exposedFaces[x, y, z] & Constants.BOTTOM) > 0) faces++;
                    }
                }
            }
            return faces;
        }

        public int GetVertexCount()
        {
            return size / Constants.VertexSize;
        }
        public bool Empty()
        {
            return vertices == null;
        }
        public int GetSize()
        {
            return size;
        }
    }
}
