using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing ;

namespace MotionProfileMapper
{
    class Points
    {
        public List<Point> pts = new List<Point>();
        public double[] defVelMap; // default
        public double[] velMap; // not used yet
        public int[] defTimeMap; // default (matched with defVelMap)
        public int[] timeMap; // not used yet (matched with velMap)

        public double[] distancesBtPts; // from [index-1] to [index], so first is always zero

        public Points()
        {
            //fillVel(); // dont do this
            fillDistances(); // should make a zero length array, to be filled later by write() in Path class
        }

        public float x(int idx)
        {
            return pts[idx].X;
        }
        public List<float> x()
        {
            List<float> r = new List<float>();
            foreach (Point p in pts)
            {
                r.Add(p.X);
            }
            return r;
        }
        public float y(int idx)
        {
            return pts[idx].Y;
        }
        public List<float> y()
        {
            List<float> r = new List<float>();
            foreach (Point p in pts)
            {
                r.Add(p.Y);
            }
            return r;
        }
        public List<Point> points()
        {
            return pts;
        }

        public void add(Point pt)
        {
            pts.Add(pt);
        }

        public void add(float[] x, float[] y)
        {
            for (int i = 0; i <= x.Length - 1; i++)
            {
                pts.Add(new Point((int)x[i], (int)y[i]));
            }
        }

        public void clear()
        {
            pts.Clear();
        }

        public float length()
        {
            return (float)length(0, pts.Count() - 1);
        }

        public float length(int startPt, int endPt)
        {
            float eSplineLength = 0;
            if (endPt > pts.Count() - 1)
            {
                endPt = pts.Count() - 1;
            }
            if (startPt < 0)
            {
                startPt = 0;
            }
            for (int i = startPt; i < endPt; i++)
            {
                eSplineLength = eSplineLength + (float)Math.Sqrt(Math.Pow((pts[i + 1].X - pts[i].X), 2) +
                    Math.Pow((pts[i + 1].Y - pts[i].Y), 2));
            }
            return eSplineLength;
        }

        public double seqAngle(int fX, int fY, int sX, int sY, int tX, int tY)
        {
            // a^2 = b^2 + c^2 - 2(a)(b)cos(A)
            // A = arccos( (a^2-b^2-c^2) )
            //             -------------
            //               -2(b)(c)
            double a2 = Math.Pow(tX - sX, 2) + Math.Pow(tY - sY, 2);  // these aren't squared because
            double b2 = Math.Pow(tX - fX, 2) + Math.Pow(tY - fY, 2);  // they cancel with the distance formule
            double c2 = Math.Pow(sX - fX, 2) + Math.Pow(sY - fY, 2);  // square roots
            double tempA = (a2 - b2 - c2) / (-2 * Math.Sqrt(b2) * Math.Sqrt(c2));
            double angle = Math.Acos(tempA);
            return angle;
        }

        public void fillDefVel()
        {  //5400mms
            defVelMap = new double[pts.Count()];
            int size10 = pts.Count() / 10;
            int middle = pts.Count() - (2 * size10);
            double jumpSize = 5400 / size10;
            for(int i = 1; i <= size10; i++)
            {
                defVelMap[i - 1] = i * jumpSize;
            }
            for(int i = 0; i < middle; i++)
            {
                defVelMap[size10 + i] = 5400;
            }
            for(int i = 1; i <= size10; i++)
            {
                defVelMap[size10 + middle + i - 1] = (5400 - i * jumpSize);
            }
        }

        public void fillDistances()
        {
            distancesBtPts = new double[pts.Count()];
            for(int i = 0; i < pts.Count(); i++)
            {
                if(i == 0)
                {
                    distancesBtPts[i] = 0;
                }
                else
                {
                    distancesBtPts[i] = length(i-1, i);
                }
            }
        }

        public void fillDefTime() // always run after fillDefVel() and distance bt points, mm / mm/s
        {
            defTimeMap = new int[pts.Count()];
            for(int i = 0; i < pts.Count(); i++)
            {
                if(i == 0)
                {
                    defTimeMap[i] = 0;
                }
                else
                {
                    if (defVelMap[i] == 0)
                    {
                        defTimeMap[i] = 0;
                    }
                    else
                    {
                        defTimeMap[i] = (int) ( (distancesBtPts[i] / defVelMap[i]) * 1000 );
                    }
                }
            }
        }
    }


    class Path
    {
        public Points controlPoints = new Points();
        public Points spline = new Points();
        public Points leftTrack = new Points();
        public Points rightTrack = new Points();

        public int trackwidth = 600;
        public int resolution = 30;

        public void Create()
        {
            float[] xs, ys;

            int eSplineLength = (int)controlPoints.length();

            if(eSplineLength > 0)
            {
                //clear out the previous point
                spline.clear();
                rightTrack.clear();
                leftTrack.clear();

                //Create spline for center of the bot
                TestMySpline.CubicSpline.FitParametric(controlPoints.x().ToArray(), controlPoints.y().ToArray(), eSplineLength / resolution, out xs, out ys);
                spline.add(xs, ys);

                //calculate the tangent to the line for the left and right track
                for (int i = 1; i < spline.points().Count; i++)
                {
                    float x = spline.x(i) - spline.x(i - 1);
                    float y = spline.y(i) - spline.y(i - 1);

                    double z = -Math.Sqrt(x * x + y * y);
                    int my = -(int)(trackwidth / 2 / z * x);
                    int mx = (int)(trackwidth / 2 / z * y);

                    //Create track points 
                    rightTrack.add(new Point((int)(spline.x(i) + mx), (int)(spline.y(i) + my)));
                    leftTrack.add(new Point((int)(spline.x(i) - mx), (int)(spline.y(i) - my)));
                }
            }
            //write();

        }

        public void writeGen(String fileName)
        {
            leftTrack.fillDistances();
            rightTrack.fillDistances();
            leftTrack.fillDefVel();
            rightTrack.fillDefVel();
            leftTrack.fillDefTime();
            rightTrack.fillDefTime();

            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(fileName))
            {
                writetext.WriteLine("package org.usfirst.frc.team3539.robot.profiles;");
                writetext.WriteLine("public class test {");
                writetext.WriteLine("   public static final int kNumPoints = " + leftTrack.pts.Count() + ";");
                writetext.WriteLine("   public static double PointsL[][] = new double[][] {");

                for (int i = 0; i < leftTrack.pts.Count(); i++) //CONVERT LENGTH FINDING TO PREMADE ARRAY, SUBSTITUTE TIME VALUES TO REAL TIMES (todo)
                {
                    if (i == 0)
                    {
                        writetext.WriteLine("       {0, 0, 10},");
                    }
                    else if (i == leftTrack.pts.Count() - 1)
                    {
                        writetext.WriteLine("       {" + leftTrack.distancesBtPts[i] + ", " + leftTrack.defVelMap[i] + ", " + leftTrack.defTimeMap[i] + "}};");
                    }
                    else
                    {
                        writetext.WriteLine("       {" + leftTrack.distancesBtPts[i] + ", " + leftTrack.defVelMap[i] + ", " + leftTrack.defTimeMap[i] + "},");
                    }
                }
                writetext.WriteLine("");
                writetext.WriteLine("   public static double PointsR[][] = new double[][] {");
                for (int i = 0; i < rightTrack.pts.Count(); i++)
                {
                    if (i == 0)
                    {
                        writetext.WriteLine("       {0, 0, 10},");
                    }
                    else if (i == rightTrack.pts.Count() - 1)
                    {
                        writetext.WriteLine("       {" + rightTrack.distancesBtPts[i] + ", " + rightTrack.defVelMap[i] + ", " + rightTrack.defTimeMap[i] + "}};");
                    }
                    else
                    {
                        writetext.WriteLine("       {" + rightTrack.distancesBtPts[i] + ", " + rightTrack.defVelMap[i] + ", " + rightTrack.defTimeMap[i] + "},");
                    }
                }

                writetext.WriteLine("}");
            }
        }

        public void writeCon(String fileName)
        {
            using (System.IO.StreamWriter writetext = new System.IO.StreamWriter(fileName))
            {
                writetext.WriteLine(fileName);

                for (int i = 0; i < controlPoints.pts.Count(); i++)
                {
                    writetext.WriteLine(controlPoints.x(i));
                    writetext.WriteLine(controlPoints.y(i));
                }

            }
        }
    }
}
