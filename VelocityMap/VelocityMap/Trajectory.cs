using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionProfile
{
    class Trajectory : List<Path>
    {
        public double getMaxVelocity()
        {
            double max = 0;
            foreach (Path p in this)
            {
                if (p.velocityMap.vMax > max)
                    max = p.velocityMap.vMax;
            }
            return max;
        }
        public float[] getTimeProfile()
        {
            float offset = 0;
            List<float> values = new List<float>();
            foreach (Path p in this)
            {
                foreach (float f in p.getTimeProfile())
                {
                    values.Add(f + offset);
                }
                offset = values.Last();
            }
            return values.ToArray<float>();
        }

        public float[] getDistanceProfile()
        {
            float offset = 0;
            List<float> values = new List<float>();
            foreach (Path p in this)
            {
                foreach (float f in p.getDistanceProfile())
                {
                    values.Add(f + offset);
                }
                offset = values.Last();
            }
            return values.ToArray<float>();
        }
 
        public float[] getVelocityProfile()
        {
            List<float> values = new List<float>();
            foreach (Path p in this)
            {
                if (p.direction)
                {
                    values.AddRange(p.getVelocityProfile());
                }
                else
                {
                    foreach (float f in p.getVelocityProfile())
                        values.Add(-f);
                }
            }

            return values.ToArray<float>();
        }

        public List<float> getOffsetVelocityProfile(int offset)
        {
            List<float> values = new List<float>();
            foreach (Path p in this)
                if (p.direction)
                {
                    values.AddRange(p.getOffsetVelocityProfile(offset));
                }
                else
                {
                    foreach (float f in p.getOffsetVelocityProfile(offset))
                        values.Add(-f);
                }

            return values;
        }
        public List<float> getOffsetDistanceProfile(int offset)
        {
            List<float> values = new List<float>();
            foreach (Path p in this)
                if (p.direction)
                {
                    values.AddRange(p.getOffsetDistanceProfile(offset));
                }
                else
                {
                    foreach (float f in p.getOffsetDistanceProfile(offset))
                        values.Add(-f);
                }

            return values;
        }
        public void test()
        {
            float dx = 0;
            float dy = 0;
            foreach (Path p in this)
            {
                p.testCreate(dx,dy);
                dx = p.dx;
                dy = p.dy;
            }
        }

        public void Create(int offset = 0)
        {
            foreach (Path p in this)
            {
                if (offset > 0)
                    p.CreateThrottled(offset);
                else
                    p.Create();
            }
        }
   
        public List<System.Drawing.PointF> BuildPath(int offset = 0)
        {

            List<System.Drawing.PointF> values = new List<System.Drawing.PointF>();
            foreach (Path p in this)
            {
                if (offset != 0)
                {
                    if (!p.direction)
                        offset = -offset;
                    values.AddRange(p.buildOffsetPoints(offset).ToArray<System.Drawing.PointF>());
                    if (!p.direction)
                        offset = -offset;
                }
                else
                    values.AddRange(p.buildPath().ToArray<System.Drawing.PointF>());
               
            }
            return values;
        }
    }
}
