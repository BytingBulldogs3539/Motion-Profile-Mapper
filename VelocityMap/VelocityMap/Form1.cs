using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace VelocityMap
{
    public partial class Form1 : Form
    {
        private List<int[]> fieldpts = new List<int[]>();
        private int fieldWidth = 8230;
        int padding = 1;
        private Bitmap baseFieldImage;
        private MotionProfile.Trajectory paths;
        public List<float> TEMPXLIST = new List<float>();
        public List<float> TEMPYLIST = new List<float>(); // if these work then it's a christmas miracle

        #region mainForm
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            loadFieldPoints();
            SetupMainField();
            SetupPlots();

            DistancePlot.Dock = DockStyle.Fill;
            VelocityPlot.Dock = DockStyle.Fill;

            splitContainer1.SplitterDistance = splitContainer1.Height / 2;

            this.WindowState = FormWindowState.Maximized;
        }
 
        private void SetupMainField()
        {
            baseFieldImage = buildField();

            Image b = new Bitmap(baseFieldImage,1000, 1000);
            b.RotateFlip(RotateFlipType.Rotate180FlipNone);
            NamedImage backImage = new NamedImage("Background", b);

            mainField.Series.Add("test");
            mainField.Series["test"].ChartType = SeriesChartType.Point;
            mainField.Series["test"].Points.AddXY(0, 0);
            mainField.Series["test"].Points.AddXY(fieldWidth, fieldWidth);

            mainField.Series.Add("path");
            mainField.Series.Add("left");
            mainField.Series.Add("right");
            mainField.Series.Add("cp");

            mainField.Series["cp"].MarkerSize = 8;
            mainField.Series["path"].MarkerSize = 2;
            mainField.Series["left"].MarkerSize = 2;
            mainField.Series["right"].MarkerSize = 2;

            mainField.Series["cp"].MarkerStyle = MarkerStyle.Circle;
            mainField.Series["cp"].ChartType = SeriesChartType.Point;
            mainField.Series["path"].ChartType = SeriesChartType.Point;
            mainField.Series["left"].ChartType = SeriesChartType.Point;
            mainField.Series["right"].ChartType = SeriesChartType.Point;

            mainField.Series["cp"].Color = Color.ForestGreen;
            mainField.Series["path"].Color = Color.Gray;
            mainField.Series["left"].Color = Color.Blue;
            mainField.Series["right"].Color = Color.Red;


            mainField.ChartAreas[0].Axes[0].Maximum = fieldWidth;
            mainField.ChartAreas[0].Axes[0].Interval = 1000;
            mainField.ChartAreas[0].Axes[0].Minimum = 0;

            mainField.ChartAreas[0].Axes[1].Maximum = fieldWidth;
            mainField.ChartAreas[0].Axes[1].Interval = 1000;
            mainField.ChartAreas[0].Axes[1].Minimum = 0;

            mainField.Images.Add(backImage);
            mainField.ChartAreas[0].BackImageWrapMode = ChartImageWrapMode.Scaled;
            mainField.ChartAreas[0].BackImage = "Background";
        }

        private void SetupPlots()
        {
            VelocityPlot.ChartAreas[0].Axes[0].Minimum = 0;
            VelocityPlot.ChartAreas[0].Axes[0].Title = "Distance (mm)";

            VelocityPlot.ChartAreas[0].Axes[1].Title = "Velocity (mm/s)";

            VelocityPlot.Series.Add("path");
            VelocityPlot.Series.Add("left");
            VelocityPlot.Series.Add("right");

            VelocityPlot.Series["path"].ChartType = SeriesChartType.FastLine;
            VelocityPlot.Series["left"].ChartType = SeriesChartType.FastLine;
            VelocityPlot.Series["right"].ChartType = SeriesChartType.FastLine;

            VelocityPlot.Series["path"].Color = Color.Gray;
            VelocityPlot.Series["left"].Color = Color.Blue;
            VelocityPlot.Series["right"].Color = Color.Red;


            DistancePlot.ChartAreas[0].Axes[0].Minimum = 0;
            DistancePlot.ChartAreas[0].Axes[0].Interval = .5;
            DistancePlot.ChartAreas[0].Axes[0].Title = "Time (s)";

            DistancePlot.ChartAreas[0].Axes[1].Interval = 500;
            DistancePlot.ChartAreas[0].Axes[1].Title = "Distance (mm)";

            DistancePlot.Series.Add("path");
            DistancePlot.Series.Add("left");
            DistancePlot.Series.Add("right");

            DistancePlot.Series["path"].ChartType = SeriesChartType.FastLine;
            DistancePlot.Series["left"].ChartType = SeriesChartType.FastLine;
            DistancePlot.Series["right"].ChartType = SeriesChartType.FastLine;

            DistancePlot.Series["path"].Color = Color.LightGray;
            DistancePlot.Series["left"].Color = Color.Blue;
            DistancePlot.Series["right"].Color = Color.Red;
        }

        private void ClearChart(Chart chart)
        {
            foreach(Series s in chart.Series)
            {
                s.Points.Clear();
            }

            mainField.Series["test"].Points.AddXY(0, 0);
            mainField.Series["test"].Points.AddXY(fieldWidth, fieldWidth);
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
          //  this.Width = this.Height+200-30;
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            int h = this.Height;
            int w = this.Width;

            if (this.WindowState == FormWindowState.Maximized)
            {
                this.Top = Screen.PrimaryScreen.WorkingArea.Top;
                this.WindowState = FormWindowState.Normal;
                this.Height = Screen.PrimaryScreen.WorkingArea.Height;
                this.Width = h + 550 - 30;
                this.Left = (Screen.PrimaryScreen.WorkingArea.Width - this.Width) /2;

            }
        }

        #endregion


        #region mainField

        private void loadFieldPoints()
        {
            using (var reader = new System.IO.StreamReader("FieldPoints.txt"))
            {
                while (!reader.EndOfStream)
                {
                    List<string> line = reader.ReadLine().Split('\'').ToList<string>();
                    if (!line[0].Equals(""))
                    {
                        line = line[0].Split(',').ToList<string>();
                        List<int> lineout = new List<int>();
                        foreach (string item in line)
                        {
                            lineout.Add(int.Parse(item));
                        }
                        fieldpts.Add(lineout.ToArray());
                    }
                }
                return;
            }
        }

        private Bitmap buildField()
        {
            Pen bluePen = new Pen(Color.Red, 10);

            //create the drawing bitmap
            Bitmap b = new Bitmap(fieldWidth + padding * 2, fieldWidth + padding * 2);

            //draw the grid on the bitmap
         //   b = drawGrid(pictureBox1.Width, pictureBox1.Height, b, 50, true);


            //draw the field size on the bitmap
            Graphics g = Graphics.FromImage(b);
            g.DrawRectangle(bluePen, new Rectangle(0, 0, b.Width - padding, b.Height - padding));

            //draw the fieldObjects on the bitmap
            foreach (int[] obj in fieldpts)
            {
                if (obj.Length >= 4)
                {
                    int[] pts = obj.Take(4).ToArray<int>();
                    Brush brush = Brushes.ForestGreen;
                    Pen pen = new Pen(Color.Black, 5);

                    if (obj.Length > 4)
                    {
                        switch (obj[4])
                        {
                            case 0:
                                brush = Brushes.Red;
                                break;
                            case 1:
                                brush = Brushes.Yellow;
                                break;
                            case 2:
                                brush = Brushes.LightGray;
                                break;
                        }
                    }
                    g.FillRectangle(brush, makeRectangle(pts));
                    g.DrawRectangle(pen, makeRectangle(pts));
                }
            }
            //clear up remaining handles
            bluePen.Dispose();
            g.Dispose();
            return b;
        }

        private void mainField_MouseClick(object sender, MouseEventArgs e)
        {
            if (dp != null)
            {
                dp = null;
                Apply_Click(null, null);
                return;
            }
            Chart c = (Chart)sender;

            double x = c.ChartAreas[0].AxisX.PixelPositionToValue(e.X);
            double y = c.ChartAreas[0].AxisY.PixelPositionToValue(e.Y);


            c.Series["cp"].Points.AddXY(x, y);

            controlPoints.Rows.Add((int)x, (int)y, "+");
        }

        private void mainField_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                Chart c = (Chart)sender;

                ChartArea ca = c.ChartAreas[0];
                Axis ax = ca.AxisX;
                Axis ay = ca.AxisY;
                if (dp != null)
                {
                    return;
                }
                else
                {
                    HitTestResult hit = mainField.HitTest(e.X, e.Y);

                    if (hit.PointIndex >= 0)
                    {
                        dp = hit.Series.Points[hit.PointIndex];
                        foreach (DataGridViewRow row in controlPoints.Rows)
                        {
                            if (row.Cells[0].Value != null)
                            {
                                // Debug.Print(row.Cells[0].Value.ToString() + ":" + ((int)dp.XValue).ToString() + ":" + row.Cells[1].Value.ToString() + ":" + ((int)dp.YValues[0]).ToString());
                                if (row.Cells[0].Value.ToString() == ((int)dp.XValue).ToString() && row.Cells[1].Value.ToString() == ((int)dp.YValues[0]).ToString())
                                {
                                    Debug.Print(row.Index.ToString());
                                    //move the point
                                    double dx = (int)ax.PixelPositionToValue(e.X);
                                    double dy = (int)ay.PixelPositionToValue(e.Y);

                                    dp.XValue = dx;
                                    dp.YValues[0] = dy;
                                    row.Cells[0].Value = dx;
                                    row.Cells[1].Value = dy;

                                    rowIndex = row.Index;

                                }
                            }
                        }
                    }
                }
            }
        }

        private void mainField_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button.HasFlag(MouseButtons.Left))
            {
                Chart c = (Chart)sender;

                ChartArea ca = c.ChartAreas[0];
                Axis ax = ca.AxisX;
                Axis ay = ca.AxisY;
                if (dp != null)
                {
                    double dx = (int)ax.PixelPositionToValue(e.X);
                    double dy = (int)ay.PixelPositionToValue(e.Y);

                    dp.XValue = dx;
                    dp.YValues[0] = dy;
                    controlPoints.Rows[rowIndex].Cells[0].Value = dx;
                    controlPoints.Rows[rowIndex].Cells[1].Value = dy;
                    c.Invalidate();
                }

            }
        }

        #endregion



        #region ControlPoints
        private int rowIndex;
        DataPoint dp; 
        
        private void Invert_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in controlPoints.Rows)
            {
                if (row.Cells[0].Value != null)
                    row.Cells[0].Value = this.fieldWidth - float.Parse(row.Cells[0].Value.ToString());
            }
            Apply_Click(sender, e);
        }
       
        private void controlPoints_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private void controlPoints_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (controlPoints.CurrentCell.Value.ToString() == "+" || controlPoints.CurrentCell.Value.ToString() == "-")
                {
                }
                else { 
                    controlPoints.CurrentCell.Value = "+";
                   // controlPoints.BeginEdit(true);
                }
            }
        }

        private void controlPoints_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
     
            if (e.Button == MouseButtons.Right)
            {
                this.controlPoints.Rows[e.RowIndex].Selected = true;
                this.rowIndex = e.RowIndex;
                this.controlPoints.CurrentCell = this.controlPoints.Rows[e.RowIndex].Cells[1];
                this.contextMenuStrip1.Show(this.controlPoints, e.Location);
                contextMenuStrip1.Show(controlPoints, e.Location);
            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if(controlPoints.Rows[rowIndex].Cells[0].Value != null)
                controlPoints.Rows.RemoveAt(rowIndex);
            ReloadControlPoints();

        }

        private void ClearCP_Click(object sender, EventArgs e)
        {
            controlPoints.Rows.Clear();

            mainField.Series["cp"].Points.Clear();
            mainField.Series["path"].Points.Clear();
            mainField.Series["left"].Points.Clear();
            mainField.Series["right"].Points.Clear();

            VelocityPlot.Series["path"].Points.Clear();
            VelocityPlot.Series["right"].Points.Clear();
            VelocityPlot.Series["left"].Points.Clear();

            DistancePlot.Series["path"].Points.Clear();
            DistancePlot.Series["right"].Points.Clear();
            DistancePlot.Series["left"].Points.Clear();
        }

        private void insertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            controlPoints.Rows.Insert(rowIndex);
        }
        
        #endregion

        #region Utility Functions
        private Rectangle makeRectangle(int[] array, bool adjustToScreen = false)
        {
            Rectangle rec = new Rectangle();
            rec.X = array[0] + padding - 1;
            if (rec.X < 0) rec.X = padding - 1;

            rec.Width = array[2];
            if (array[0] < 0) rec.Width = rec.Width + array[0];

            rec.Y = array[1] - padding - 1;
            if (rec.Y < 0) rec.Y = 0;

            rec.Height = array[3];
            if (array[1] < 0) rec.Height = rec.Height + array[1];

            if (adjustToScreen)
                rec.Y = fieldWidth - rec.Y - rec.Height;

            return rec;
        }

        private void ReloadControlPoints()
        {
            mainField.Series["cp"].Points.Clear();
            foreach (DataGridViewRow row in controlPoints.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    mainField.Series["cp"].Points.AddXY(float.Parse(row.Cells[0].Value.ToString()), float.Parse(row.Cells[1].Value.ToString()));
                }
            }
        }
        
        private MotionProfile.Path CreateNewPath()
        {
            MotionProfile.Path path = new MotionProfile.Path();
            path.velocityMap = new MotionProfile.VelocityMap();
            path.velocityMap.vMax = int.Parse(maxVelocity.Text);
            path.velocityMap.FL1 = int.Parse(AccelRate.Text);
            path.velocityMap.time = float.Parse(timeSample.Text) / 1000;
            path.tolerence = float.Parse(tolerence.Text);
          //  path.mindifference = float.Parse(Calibration.Text);
            path.velocityMap.instVelocity = isntaVel.Checked;
            path.speedLimit = float.Parse(SpeedLimit.Text);
            path.calibration = CalCheck.Checked;

            return path;
        }

        #endregion
        private void Apply_Click (object sender, EventArgs e)
        {
            MotionProfile.Path path = CreateNewPath();

            int trackwidth = (int)((int.Parse(trackWidth.Text)) / 2);
            
            ClearChart(mainField);

            paths = new MotionProfile.Trajectory();

            string last = "";
            DataGridViewRow lastrow = controlPoints.Rows[0];
            foreach (DataGridViewRow row in controlPoints.Rows)
            {
                if (row.Cells[0].Value != null)
                {
                    lastrow = row;
                    mainField.Series["cp"].Points.AddXY(float.Parse(row.Cells[0].Value.ToString()), float.Parse(row.Cells[1].Value.ToString()));
                    if(path.controlPoints.Count == 0)
                        if (row.Cells[2].Value.ToString() == "-")
                            path.direction = false;

                    if (row.Cells[2].Value.ToString() == "-")
                        mainField.Series["cp"].Points.Last().Color = Color.Red;

                    path.addControlPoint(float.Parse(row.Cells[1].Value.ToString()), float.Parse(row.Cells[0].Value.ToString()));

                    if (last != "" && last != row.Cells[2].Value.ToString())
                    {
                        if (row.Cells[2].Value.ToString() == "+")
                            path.direction = false;

                        if(path.controlPoints.Count >=2)
                            paths.Add(path);

                        path = CreateNewPath();
                        path.velocityMap.instVelocity = isntaVel.Checked;
                        path.addControlPoint(float.Parse(row.Cells[1].Value.ToString()), float.Parse(row.Cells[0].Value.ToString()));

                    }
                    last = row.Cells[2].Value.ToString();
                }
            }
            if (path.controlPoints.Count() == 0)
                return;

            if (lastrow != null && lastrow.Cells[2].Value.ToString() != "+")
                path.direction = false;

            if (path.controlPoints.Count >= 2)
                paths.Add(path);

            if (!checkBox1.Checked)
                paths.test();
            else
                paths.Create(0);

            ClearChart(VelocityPlot);
            ClearChart(DistancePlot);

            float[] t,d,v,l,r ,ld ,rd, c, cd;

            t = paths.getTimeProfile();
            d = paths.getDistanceProfile();
            v = paths.getVelocityProfile();
            l = paths.getOffsetVelocityProfile(trackwidth).ToArray();
            ld = paths.getOffsetDistanceProfile(trackwidth).ToArray();
            c = paths.getOffsetVelocityProfile(0).ToArray();
            cd = paths.getOffsetDistanceProfile(0).ToArray();

            l.NoiseReduction(int.Parse(smoothness.Text));

            if (CalCheck.Checked)
            {
                foreach (MotionProfile.Path p in paths)
                    p.direction =  !p.direction;
                r = paths.getOffsetVelocityProfile(-trackwidth).ToArray();
                rd = paths.getOffsetDistanceProfile(-trackwidth).ToArray();

                foreach (MotionProfile.Path p in paths)
                    p.direction = !p.direction;
            }
            else
            {
                r = paths.getOffsetVelocityProfile(-trackwidth).ToArray();
                rd = paths.getOffsetDistanceProfile(-trackwidth).ToArray();
            }
           
            r.NoiseReduction(int.Parse(smoothness.Text));
            rd.NoiseReduction(int.Parse(smoothness.Text));
            l.NoiseReduction(int.Parse(smoothness.Text));
            rd.NoiseReduction(int.Parse(smoothness.Text));
            c.NoiseReduction(int.Parse(smoothness.Text));
            cd.NoiseReduction(int.Parse(smoothness.Text));

            double ldv = 0;
            double rdv = 0;

            for (int i = 0; i < ld.Length; i++)
            {
                ldv += ld[i];
                rdv += rd[i];

                DistancePlot.Series["left"].Points.AddXY(t[i], ldv);
                DistancePlot.Series["right"].Points.AddXY(t[i], rdv);
            }

            for (int i = 0; i < Math.Min(d.Length, r.Length); i++)
            {
                VelocityPlot.Series["path"].Points.AddXY(d[i], v[i + 2]);
                VelocityPlot.Series["left"].Points.AddXY(d[i], l[i]);
                VelocityPlot.Series["right"].Points.AddXY(d[i], r[i]);

            }
           
            mainField.Series["path"].Points.Clear();
            mainField.Series["left"].Points.Clear();
            mainField.Series["right"].Points.Clear();


            foreach (PointF p in paths.BuildPath())
            {
                mainField.Series["path"].Points.AddXY(p.Y, p.X);
                TEMPXLIST.Add(p.X);
                TEMPYLIST.Add(p.Y);
            }

            foreach (PointF p in paths.BuildPath(trackwidth))
            {
                mainField.Series["left"].Points.AddXY(p.Y, p.X);
            }

            foreach (PointF p in paths.BuildPath(-trackwidth))
            {
                mainField.Series["right"].Points.AddXY(p.Y, p.X);
            }
        }

        private void Save_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Application.StartupPath;
            DialogResult results = folderBrowserDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                String[] p = folderBrowserDialog1.SelectedPath.Split ('\\');
                string path = folderBrowserDialog1.SelectedPath + "\\" + p[p.Length - 1];

                using (var writer = new System.IO.StreamWriter(path + ".java"))
                {
                    string file = path.Split('\\').Last();
                    writer.WriteLine("package org.usfirst.frc.team3539.robot.profiles;");
                    writer.WriteLine("public class " + file + " implements IProfile");
                    writer.WriteLine("{");

                    List<string> left = new List<string>();
                    List<string> right =  new List<string>();
                    List<string> center = new List<string>();
           
                    List<string>  line = new List<string>();
                    int trackwidth = (int)((int.Parse(trackWidth.Text)) / 2);

                    float[] l = paths.getOffsetVelocityProfile(trackwidth).ToArray();
                    List<float> ld = paths.getOffsetDistanceProfile(trackwidth);

                    float[] r;
                    List<float> rd = new List<float>(); ;

                    float[] c = paths.getOffsetVelocityProfile(0).ToArray();
                    List<float> cd = paths.getOffsetDistanceProfile(0);

                    if (CalCheck.Checked)
                    {
                        foreach (MotionProfile.Path ph in paths)
                            ph.direction = !ph.direction;

                        r = paths.getOffsetVelocityProfile(-trackwidth).ToArray();
                        rd = paths.getOffsetDistanceProfile(-trackwidth);

                        foreach (MotionProfile.Path ph in paths)
                            ph.direction = !ph.direction;
                    }
                    else
                    {
                        r = paths.getOffsetVelocityProfile(-trackwidth).ToArray();
                        rd = paths.getOffsetDistanceProfile(-trackwidth);
                    }

                    r.NoiseReduction(int.Parse(smoothness.Text));
                    rd.NoiseReduction(int.Parse(smoothness.Text));
                    l.NoiseReduction(int.Parse(smoothness.Text));
                    ld.NoiseReduction(int.Parse(smoothness.Text));
                    c.NoiseReduction(int.Parse(smoothness.Text));
                    cd.NoiseReduction(int.Parse(smoothness.Text));

                    float[] angles = new float[TEMPXLIST.Count - 2]; // -2 because of the odd thing where its always 2 over the kpoints
                    for(int i = 0; i < (TEMPXLIST.Count - 2); i++)
                    {
                        angles[i] = findAngle(TEMPXLIST[i + 1], TEMPXLIST[i], TEMPYLIST[i + 1], TEMPYLIST[i]);
                    }
                    for (int i =0; i < l.Length ;i++)
                    {
                        line.Clear();
                        String abc = "";
                        if (CTRE.Checked)
                        {
                            double dConvert = Math.PI * double.Parse(wheel.Text)  *25.4; // ", " + paths[0].heading[i] + "},");

                            line.Add("      {" + cd.Take(i).Sum() / dConvert );
                            line.Add((c[i] / dConvert * 60).ToString());
                            line.Add(paths[0].velocityMap.time * 1000 + ", " + angles[i] + "},");
                            left.Add(string.Join(",", line));

                        }else
                        {
                            line.Add("      {" + cd.Take(i).Sum().ToString());
                            line.Add(c[i].ToString());
                            line.Add(paths[0].velocityMap.time * 1000 + ", " + angles[i] + "},");
                            left.Add(string.Join(",", line));

                        }
                        line.Clear();
                        if (CTRE.Checked)
                        {
                            double dConvert = Math.PI * double.Parse(wheel.Text) * 25.4;

                            line.Add("      {" + cd.Take(i).Sum() / dConvert);
                            line.Add((c[i] / dConvert * 60).ToString());
                            line.Add(paths[0].velocityMap.time * 1000 + ", " + angles[i] + "},");
                            right.Add(string.Join(",", line));

                        }
                        else
                        {
                            line.Add("      {" + cd.Take(i).Sum().ToString());
                            line.Add(c[i].ToString());
                            line.Add(paths[0].velocityMap.time * 1000 + ", " + angles[i] + "},");
                            right.Add(string.Join(",", line));
                        }
                    }

                    writer.WriteLine("  public static final int kNumPoints = " + c.Length.ToString ()  + ";");
                    writer.WriteLine("  public static double PointsR[][] = new double[][] {");
                    foreach (string ret in right)
                    {
                        writer.WriteLine(ret);
                    }
                    writer.WriteLine("  }; ");
                    writer.WriteLine("  public static double PointsL[][] = new double[][] {");
                    foreach (string ret in left)
                    {
                        writer.WriteLine(ret);
                    }
                   writer.WriteLine("   }; ");
                   writer.WriteLine("}; ");
                }
                WriteSetupFile(path);
            }
        }

        public float findAngle(double x2, double x1, double y2, double y1)
        {
            float ang = 0;
            float chx = (float) (x2 - x1);
            float chy = (float) (y2 - y1);
            double CONVERT = 180.0 / Math.PI;
            if (chy > 0)
            {                         // X AND Y ARE REVERSED BECAUSE OF MOTION PROFILER STUFF
                if (chx > 0)
                {
                    // positive x, positive y, 90 - ang, quad 1
                    ang = (float)(90 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = 1;
                }
                else
                {
                    // positive x, negative y, 90 + ang, quad 2
                    ang = (float)(90 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = 2;
                }
            }
            else
            {
                if (chx > 0)
                {
                    // negative x, positive y, 270 + ang, quad 4
                    ang = (float)(270 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = 4;
                }
                else
                {
                    // negative x, negative y, 270 - ang, quad 3
                    ang = (float)(270 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = 3;
                }
            }
            return ang;
        }

        private void WriteSetupFile(string path)
        {

            using (var writer = new System.IO.StreamWriter(path + ".mp"))
            {
                List<string> sb = new List<string>();

                sb.Add("settings");
                sb.Add(maxVelocity.Text);
                sb.Add(trackWidth.Text);
                sb.Add(AccelRate.Text);
                sb.Add(timeSample.Text);
                sb.Add(wheel.Text);
                sb.Add(SpeedLimit.Text);
                sb.Add(smoothness.Text);

                sb.Add(CTRE.Checked.ToString());
                sb.Add(isntaVel.Checked.ToString());

                writer.WriteLine(string.Join(",", sb.ToArray()));
                foreach (DataGridViewRow row in controlPoints.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        writer.WriteLine(string.Concat(row.Cells[0].Value.ToString(), ",", row.Cells[1].Value.ToString(), ",", row.Cells[2].Value.ToString()));
                    }
                }
            }
            
        }

        private void Load_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
            openFileDialog1.Filter = "All Files (*.*)|*.*|MotionProfile Data (*.mp)|*.mp";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.DefaultExt = ".mp";

            DialogResult results = openFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                using (var reader = new System.IO.StreamReader(openFileDialog1.FileName))
                {
                    controlPoints.Rows.Clear();
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        if (!line.StartsWith("'"))
                        {
                            List<string> l = line.Split(',').ToList<string>();
                            if (l[0] != "settings")
                            {
                                if (l.Count() == 3)
                                    if (tempflip.Checked)
                                        controlPoints.Rows.Add(float.Parse(l[1]), float.Parse(l[0]), l[2]);
                                    else
                                        controlPoints.Rows.Add(float.Parse(l[0]), float.Parse(l[1]), l[2]);
                            }
                            else
                            {
                                if (l.Count() == 10)
                                {
                                    maxVelocity.Text = l[1];
                                    trackWidth.Text = l[2];
                                    AccelRate.Text = l[3];
                                    timeSample.Text = l[4];
                                    wheel.Text = l[5];
                                    SpeedLimit.Text = l[6];
                                    smoothness.Text = l[7];

                                    CTRE.Checked = Boolean.Parse(l[8]);
                                    isntaVel.Checked = Boolean.Parse(l[9]);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void CalCheck_CheckedChanged(object sender, EventArgs e)
        {
            offset.Text = "0";
            offset.Enabled = false;
            degrees.Enabled = true;
            ClearCP_Click(null, null);
            if (CalCheck.Checked)
            {
                degrees.Enabled = false;
                offset.Enabled = true;
                this.maxVelocity.Text = "1500";
                controlPoints.Rows.Add(1000, 0, "+");
                controlPoints.Rows.Add(1000, 0 + int.Parse(trackWidth.Text) * Math.PI * int.Parse(degrees.Text)/360, "+");
                Apply_Click(null, null);
            }
        }



        private void Rotations_TextChanged(object sender, EventArgs e)
        {
            if (CalCheck.Checked && degrees.Text != "")
            {
                ClearCP_Click(null, null);
                controlPoints.Rows.Add(1000, 0, "+");
                controlPoints.Rows.Add(1000, 0 + int.Parse(trackWidth.Text) * Math.PI * int.Parse(degrees.Text)/360, "+");
                Apply_Click(null, null);
            }
        }

        private void CTRE_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
