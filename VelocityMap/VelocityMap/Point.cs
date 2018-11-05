using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VelocityMap
{
    public class Point
    {    
        public float x;
        public float y;
        public Boolean direction;
        public int pointNumber;
        public Point(float x, float y, Boolean direction, int pointNumber)
        {
            this.x = x;
            this.y = y;
            this.direction = direction;
            this.pointNumber = pointNumber;
        }

    }
}
