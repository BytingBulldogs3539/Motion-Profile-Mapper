using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MotionProfile
{
    class Path
    {
        public List<PointF> controlPoints = new List<PointF>();
        public VelocityMap velocityMap = new VelocityMap();

        public bool direction = true;

        public int resolution = 100;
        public float tolerence = 100;
        public float mindifference = 50;
        public float speedLimit = 2;
        public float dx;
        public float dy;
        public bool calibration = false;

        private List<float> controlPointsX = new List<float>();
        private List<float> controlPointsY = new List<float>();

        private float[] distance;
        private float[] velocity;
        private float[] time;
        public float[] heading;

        public float[] xs, ys;

        public Spline.ParametricSpline path;

        private void CreateSpline()
        {
            controlPointsX.Clear();
            controlPointsY.Clear();
            foreach (PointF p in controlPoints)
            {
                controlPointsX.Add(p.X);
                controlPointsY.Add(p.Y);
            }
            path = new Spline.ParametricSpline(controlPointsX.ToArray(), controlPointsY.ToArray(), resolution, out xs, out ys);
            velocityMap.setLength(path.distance.Last());

        }
        public void testCreate(float dx, float dy)
        {
            controlPointsX.Clear();
            controlPointsY.Clear();
            foreach (PointF p in controlPoints)
            {
                controlPointsX.Add(p.X);
                controlPointsY.Add(p.Y);
            }

            if (dx == 0 && dy == 0)
                path = new Spline.ParametricSpline(controlPointsX.ToArray(), controlPointsY.ToArray(), resolution, out xs, out ys);
            else
                path = new Spline.ParametricSpline(controlPointsX.ToArray(), controlPointsY.ToArray(), resolution, out xs, out ys, dx, dy);


            velocityMap.setLength(path.distance.Last());
            buildMaps();

            PointF p1 = path.Eval(distance[distance.Length - 1]);
            PointF p2 = path.Eval(distance[distance.Length - 2]);

            this.dx = (p2.X - p1.X) / (float)velocityMap.time;
            this.dy = (p2.Y - p1.Y) / (float)velocityMap.time;
        }
        public void Create()
        {
            CreateSpline();
            buildMaps();

        }
        public void CreateThrottled(float offset)
        {
            CreateSpline();
            buildMapsThrottled(offset);

        }
        public void addControlPoint(float x, float y)
        {
            controlPoints.Add(new PointF(x, y));
        }
        public void addControlPoints(float[] x, float[] y)
        {
            for (int i = 0; i < x.Length; i++)
            {
                addControlPoint(x[i], y[i]);
            }
        }
        public void createHeadingMap()
        {
            // I created and then deleted this for a really good reason.  -Devon
        }
        public List<float> getOffsetVelocityProfile2(float offset)
        {
            if (!direction)
                offset = -offset;

            PointF[] array = buildOffsetPoints(offset).ToArray();
            List<float> ret = new List<float>();

            for (int i = 1; i < array.Length; i++)
            {
                double vel  = new Segment(array[i - 1], array[i]).length / velocityMap.time;
                if (Math.Abs(velocity[i] - vel) > mindifference && Math.Abs(velocity[i] - vel) < tolerence)
                {
                    if ((velocity[i] - vel) < 0)
                     vel = velocity[i] + tolerence; 
                    else
                    { vel = velocity[i] - tolerence; }
                }
                ret.Add((float)vel);
            }
            return ret;
        }
        public List<float> getOffsetVelocityProfile(float offset)
        {
            if (!direction)
                offset = -offset;

            PointF[] array = buildOffsetPoints(offset).ToArray();
            List<float> ret = new List<float>();
            
            for (int i = 1; i < array.Length; i++)
            {
                float vel = (float)(new Segment(array[i - 1], array[i]).length / velocityMap.time);
                if (Math.Abs(this.velocity[i]) * speedLimit < Math.Abs(vel))
                    vel = this.velocity[i];
                ret.Add(vel);
            }
            return ret;
        }
        public List<float> getOffsetDistanceProfile(float offset)
        {
            if (!direction)
                offset = -offset;
            PointF[] array = buildOffsetPoints(offset).ToArray();
            List<float> ret = new List<float>();

            for (int i = 1; i < array.Length; i++)
            {
                float dist = (float)(new Segment(array[i - 1], array[i]).length);
                if (Math.Abs(this.distance[i-1] - this.distance[i]) * speedLimit < Math.Abs(dist))
                    dist = Math.Abs(this.distance[i - 1] - this.distance[i]) * speedLimit;
                ret.Add(dist);
            }
            return ret;
        }

        public List<PointF> buildOffsetPoints(float offset)
        {
            List<PointF> ret = new List<PointF>();
            PointF p1 = new PointF(0, 0);
            Segment prevSeg = new Segment(p1, p1);

            foreach (float d in distance)
            {
                PointF p2 = path.Eval(d);

                if (p1.X != 0 && p1.Y != 0)
                {
                    ret.Add(new Segment(p1, p2).perp(offset));
                }
                p1 = p2;
            }
            return ret;

        }
        public void buildMapsThrottled(float offset)
        {
            List<float> vel = new List<float>();
            List<float> dist = new List<float>();
            List<float> t = new List<float>();
            vel.Add(0);
            t.Add(0);
            dist.Add(0);

            PointF p1 = new PointF(0,0);
            Segment prevSeg = new Segment(p1,p1);

            while (dist.Last() < path.distance.Last())
            {
                float velocity = velocityMap.getVelocity(dist.Last());
                if (velocity == 0) { velocity = velocityMap.getMinVelocity(); }

                float distance = (float)(dist.Last() + (velocity + vel.Last()) / 2 * velocityMap.time);

                PointF p2 =  path.Eval(dist.Last() + distance);

                if (p1.X == 0 && p1.Y == 0) { p1 = p2; }
                Segment seg = new Segment(p1, p2);

                Segment segOff = new Segment(seg.perp(offset), prevSeg.perp(offset));
                double v1 = Math.Max(new Segment(seg.perp(-offset), prevSeg.perp(-offset)).length , new Segment(seg.perp(offset), prevSeg.perp(offset)).length) / velocityMap.time ;
                if (velocity > velocityMap.vMax/10 && v1 > velocityMap.vMax)
                {
                    velocity = (float)(velocityMap.vMax * velocityMap.vMax / v1);
                }

                distance = (float)(dist.Last() + (velocity + vel.Last()) / 2 * velocityMap.time);
                dist.Add((float)(dist.Last() + (velocity + vel.Last()) / 2 * velocityMap.time));
                vel.Add(velocity);
                t.Add((float)(t.Last() + velocityMap.time));

                prevSeg = seg;
            }
            this.velocity = vel.ToArray();
            this.distance = dist.ToArray();
            this.time = t.ToArray();

        }

        public List<PointF> buildPath()
        {
            List<PointF> pts = new List<PointF>();
            foreach (float p in distance)
            {
                pts.Add(path.Eval(p));
            }
            return pts;
        }
        public void buildMaps()
        {
            List<float> vel = new List<float>();
            List<float> dist = new List<float>();
            List<float> t = new List<float>();
            List<float> head = new List<float>();
            vel.Add(0);
            t.Add(0);
            dist.Add(0);
            head.Add(0);

            while (dist.Last()  < path.distance.Last())
            {
                float velocity = velocityMap.getVelocity(dist.Last());
                if (velocity == 0) { velocity = velocityMap.getMinVelocity(); }

                dist.Add((float)(dist.Last() + (velocity + vel.Last()) / 2 * velocityMap.time));
                vel.Add(velocity);
                t.Add((float)(t.Last() + velocityMap.time));
            }
            this.velocity = vel.ToArray();
            this.distance = dist.ToArray();
            this.time = t.ToArray();
            this.heading = head.ToArray();

        }
        public float[] getTimeProfile()
        {
            return this.time;
        }
        public float[] getDistanceProfile()
        {
            return this.distance;
        }
        public float[] getHeadingProfile()
        {
            return this.heading;
        }
        public float[] getVelocityProfile()
        {
            return this.velocity;
        }

        public float findAngle(PointF point1, PointF point2)
        {
            float ang = 0;
            float chx = point2.X - point1.X;
            float chy = point2.Y - point1.Y;
            if(chx > 0)
            {
                if(chy > 0)
                {
                    // positive x, positive y, 90 - ang
                    ang = (float) (90 - (Math.Atan(chy / chx)));
                }
                else
                {
                    // positive x, negative y, 90 + ang
                    ang = (float)(90 + (Math.Atan(chy / chx)));
                }
            }
            else
            {
                if(chy > 0)
                {
                    // negative x, positive y, 270 + ang
                    ang = (float)(270 + (Math.Atan(chy / chx)));
                }
                else
                {
                    // negative x, negative y, 270 - ang
                    ang = (float)(270 - (Math.Atan(chy / chx)));
                }
            }
            return ang;
        }
    }
}
