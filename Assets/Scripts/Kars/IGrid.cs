using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kars.Object
{
    public interface IGrid<TGridObject>
    {
        public int Height { get; protected set; }
        public int Width { get; }
        public TGridObject[,] gridArray { get; protected set; }

        public Vector3Int GetXY(Vector3 worldPosition, out int x, out int y);
        public void SetValue(Vector3 worldPosition, TGridObject value);
        public void SetValue(int x, int y, TGridObject value);
        public TGridObject GetValue(Vector3 worldPosition);
        public TGridObject GetValue(int x, int y);

    }
}

