using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Renci.SshNet;
using Renci.SshNet.Sftp;

namespace VelocityMap
{
    public partial class Form1 : Form
    {
        private int fieldHeight = 3500;
        private int fieldWidth = 3048;
        int padding = 1;
        private Bitmap baseFieldImage;
        private MotionProfile.Trajectory paths;

        public List<Point> pointList = new List<Point>();

        private double CONVERT = 180.0 / Math.PI;

        #region mainForm
        public Form1()
        {
            //Create the window with all the fancy buttons.
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Put all of the points from the fieldpoints.txt and put them on the field
            loadFieldPoints();
            SetupMainField();
            SetupPlots();

            DistancePlot.Dock = DockStyle.Fill;
            VelocityPlot.Dock = DockStyle.Fill;

            splitContainer1.SplitterDistance = splitContainer1.Height / 2;


        }

        private void SetupMainField()
        {

            baseFieldImage = buildField();
            //convert the bitmap to an image that we can put on the field.
            Image b = new Bitmap(baseFieldImage, 1000, 1000);
            //Flip the image because someone created the field upside down....
            b.RotateFlip(RotateFlipType.Rotate180FlipNone);
            //Give the background image a name...
            NamedImage backImage = new NamedImage("Background", b);
            //the test series is really the background series.
            mainField.Series.Add("test");
            mainField.Series["test"].ChartType = SeriesChartType.Point;
            mainField.Series["test"].Points.AddXY(0, 0);
            mainField.Series["test"].Points.AddXY(fieldWidth, fieldHeight);
            //add different lines to the main field chart.
            mainField.Series.Add("path");
            mainField.Series.Add("left");
            mainField.Series.Add("right");
            mainField.Series.Add("cp");
            //setup the point size(the dot size on the graph)
            mainField.Series["cp"].MarkerSize = 10;
            mainField.Series["path"].MarkerSize = 2;
            mainField.Series["left"].MarkerSize = 2;
            mainField.Series["right"].MarkerSize = 2;
            
            //set what the points/dots look like
            mainField.Series["cp"].MarkerStyle = MarkerStyle.Diamond;
            //set what the different lines on the graph look like.
            mainField.Series["cp"].ChartType = SeriesChartType.Point;
            mainField.Series["path"].ChartType = SeriesChartType.Point;
            mainField.Series["left"].ChartType = SeriesChartType.Point;
            mainField.Series["right"].ChartType = SeriesChartType.Point;
            //set what the seperate lines color.
            mainField.Series["cp"].Color = Color.ForestGreen;
            mainField.Series["path"].Color = Color.Gray;
            mainField.Series["left"].Color = Color.Blue;
            mainField.Series["right"].Color = Color.Red;

            //this sets up what the graph domain and range is and what our increments are.
            mainField.ChartAreas[0].Axes[0].Maximum = fieldWidth;
            mainField.ChartAreas[0].Axes[0].Interval = 1000;
            mainField.ChartAreas[0].Axes[0].Minimum = 0;

            mainField.ChartAreas[0].Axes[1].Maximum = fieldHeight;
            mainField.ChartAreas[0].Axes[1].Interval = 1000;
            mainField.ChartAreas[0].Axes[1].Minimum = 0;


            //set the background to the background.
            mainField.Images.Add(backImage);
            mainField.ChartAreas[0].BackImageWrapMode = ChartImageWrapMode.Scaled;
            mainField.ChartAreas[0].BackImage = "Background";
        }
        //configure what the velocity chart and the distance chart look like
        private void SetupPlots()
        {
            //set the minimum for the domaine
            VelocityPlot.ChartAreas[0].Axes[0].Minimum = 0;
            //set the lables on the graph
            VelocityPlot.ChartAreas[0].Axes[0].Title = "Distance (mm)";

            VelocityPlot.ChartAreas[0].Axes[1].Title = "Velocity (mm/s)";

            //add different point types for our main path and our right and left paths.
            VelocityPlot.Series.Add("path");
            VelocityPlot.Series.Add("left");
            VelocityPlot.Series.Add("right");

            //Set what the velocity chart should look like.
            VelocityPlot.Series["path"].ChartType = SeriesChartType.FastLine;
            VelocityPlot.Series["left"].ChartType = SeriesChartType.FastLine;
            VelocityPlot.Series["right"].ChartType = SeriesChartType.FastLine;

            //Sets what the point color should look like on the velocity map.
            VelocityPlot.Series["path"].Color = Color.Gray;
            VelocityPlot.Series["left"].Color = Color.Blue;
            VelocityPlot.Series["right"].Color = Color.Red;

            //set the minimium x axis value on the distance graph
            DistancePlot.ChartAreas[0].Axes[0].Minimum = 0;
            //set the amount the x axis increases distance graph
            DistancePlot.ChartAreas[0].Axes[0].Interval = .5;
            //set the title of the x axis distance graph
            DistancePlot.ChartAreas[0].Axes[0].Title = "Time (s)";
            //set the interval of the y axis
            DistancePlot.ChartAreas[0].Axes[1].Interval = 500;
            //set the title of the y axis
            DistancePlot.ChartAreas[0].Axes[1].Title = "Distance (mm)";

            //add the seperate lines to the distance plot.
            DistancePlot.Series.Add("path");
            DistancePlot.Series.Add("left");
            DistancePlot.Series.Add("right");
            //set the type of lines
            DistancePlot.Series["path"].ChartType = SeriesChartType.FastLine;
            DistancePlot.Series["left"].ChartType = SeriesChartType.FastLine;
            DistancePlot.Series["right"].ChartType = SeriesChartType.FastLine;
            //set the color of the lines.
            DistancePlot.Series["path"].Color = Color.LightGray;
            DistancePlot.Series["left"].Color = Color.Blue;
            DistancePlot.Series["right"].Color = Color.Red;
        }
        //remove all of the points from the specified chart.
        private void ClearChart(Chart chart)
        {
            foreach (Series s in chart.Series)
            {
                s.Points.Clear();
            }
            
            mainField.Series["test"].Points.AddXY(0, 0);
            mainField.Series["test"].Points.AddXY(fieldWidth, fieldHeight);
        }

        #endregion


        #region mainField
        //Used to load the field points from the fieldpoint.txt
        private List<int[]> loadFieldPoints()
        {
            List<int[]> fieldpts = new List<int[]>();

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "VelocityMap.FieldPoints.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
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
                    return fieldpts;
                }
            }
        }
        //Used to draw the points from the fieldpoints.txt on the field
        private Bitmap buildField()
        {
            Pen bluePen = new Pen(Color.Red, 10);

            //create the drawing bitmap
            Bitmap b = new Bitmap(fieldWidth + padding * 2, fieldHeight + padding * 2);

            //draw the field size on the bitmap
            Graphics g = Graphics.FromImage(b);
            g.DrawRectangle(bluePen, new Rectangle(0, 0, b.Width - padding, b.Height - padding));

            //draw the fieldObjects on the bitmap
            foreach (int[] obj in loadFieldPoints())
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
            //if the button click is a left mouse click then add a positive point to the field chart.
            if (e.Button == MouseButtons.Left)
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

                if (x > 0 && y > 0 && x <= fieldWidth && y <= fieldHeight)
                {
                    c.Series["cp"].Points.AddXY(x, y);

                    controlPoints.Rows[controlPoints.Rows.Add((int)x, (int)y, "+", Int32.Parse(maxVelocity.Text))].Selected = true;
                }
            }
            //if the button click is a right mouse click then add a negative point to the field chart.

            if (e.Button == MouseButtons.Right)
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


                if (x > 0 && y > 0 && x <= fieldWidth && y <= fieldHeight)
                {
                    c.Series["cp"].Points.AddXY(x, y);
                    mainField.Series["cp"].Points.Last().Color = Color.Red;

                    controlPoints.Rows[controlPoints.Rows.Add((int)x, (int)y, "-", Int32.Parse(maxVelocity.Text))].Selected = true;
                }

            }

        }

        private void mainField_MouseDown(object sender, MouseEventArgs e)
        {
            //if the user is holding their left mouse button
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
                    //get the point the user is clicking on.
                    if (hit.PointIndex >= 0)
                    {
                        //check to see if the point is part of the controlpoints because we have more than just controlpoints on the field chart
                        if (hit.Series == null)
                            return;
                        if (hit.Series.ToString() != "Series-cp")
                            return;

                        if (hit.Series.Points[hit.PointIndex] == null)
                            return;
                        //if the point is real and exists then set dp to the point.
                        dp = hit.Series.Points[hit.PointIndex];
                        foreach (DataGridViewRow row in controlPoints.Rows)
                        {
                            if (row.Cells[0].Value != null)
                            {
                                // Debug.Print(row.Cells[0].Value.ToString() + ":" + ((int)dp.XValue).ToString() + ":" + row.Cells[1].Value.ToString() + ":" + ((int)dp.YValues[0]).ToString());
                                if (row.Cells[0].Value.ToString() == ((int)dp.XValue).ToString() && row.Cells[1].Value.ToString() == ((int)dp.YValues[0]).ToString())
                                {
                                    //move the point
                                    double dx = (int)ax.PixelPositionToValue(e.X);
                                    double dy = (int)ay.PixelPositionToValue(e.Y);

                                    dp.XValue = dx;
                                    dp.YValues[0] = dy;
                                    row.Cells[0].Value = dx;
                                    row.Cells[1].Value = dy;


                                    rowIndex = row.Index;
                                    row.Selected = true;


                                }
                            }
                        }
                    }
                }
            }
        }

        private void mainField_MouseMove(object sender, MouseEventArgs e)
        {
            //if the user is holding the left button while moving the mouse allow them to move the point.
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
        //ToDo: Do this
        private void controlPoints_CellSelect(object sender, DataGridViewRowStateChangedEventArgs e)
        {

        }
        private void controlPoints_RowStateChange(object sender, DataGridViewRowStateChangedEventArgs e)
        {
        
            if (e.StateChanged != DataGridViewElementStates.Selected)
            {
                return;
            }

            if (e.Row.Selected == true)
            {
                foreach (DataGridViewRow row in controlPoints.Rows)
                {
                    if (controlPoints.Rows.Count - 2 != 0)
                    {
                        if (row.Index >= 0 && row.Index <= controlPoints.Rows.Count - 2)
                        {
                            if (row.Cells[2].Value != null)
                            {
                                if (row.Cells[2].Value.ToString() == "-")
                                {
                                    mainField.Series["cp"].Points[row.Index].Color = Color.Red;
                                }

                                if (row.Cells[2].Value.ToString() == "+")
                                {
                                    mainField.Series["cp"].Points[row.Index].Color = Color.Green;

                                }
                            }
                        }
                    }
                }
                if (controlPoints.Rows.Count - 2 != 0)
                {
                    if (e.Row.Index >= 0 && e.Row.Index <= controlPoints.Rows.Count - 2)
                    {
                        mainField.Series["cp"].Points[e.Row.Index].Color = Color.Yellow;
                    }
                }
            }
        }

    private void controlPoints_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                if (controlPoints.CurrentCell.Value.ToString() == "+" || controlPoints.CurrentCell.Value.ToString() == "-")
                {
                }
                else
                {
                    controlPoints.CurrentCell.Value = "+";
                    // controlPoints.BeginEdit(true);
                }
            }
        }

        private void controlPoints_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    this.controlPoints.Rows[e.RowIndex].Selected = true;
                    this.rowIndex = e.RowIndex;
                    this.controlPoints.CurrentCell = this.controlPoints.Rows[e.RowIndex].Cells[1];

                    var relativeMousePosition = this.controlPoints.PointToClient(System.Windows.Forms.Cursor.Position);
                    this.contextMenuStrip2.Show(this.controlPoints, relativeMousePosition);
                }


            }
        }

        private void Delete_Click(object sender, EventArgs e)
        {
            if (rowIndex != controlPoints.RowCount-1)
            { 
                controlPoints.Rows.RemoveAt(rowIndex);
            }
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

            pointList.Clear();

        }

        private void insertAbove_Click(object sender, EventArgs e)
        {
            controlPoints.Rows.Insert(rowIndex);
        }
        private void insertBelow_Click(object sender, EventArgs e)
        {
            controlPoints.Rows.Insert(rowIndex+1);
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            DataGridView dgv = controlPoints;
            try
            {
                int totalRows = dgv.Rows.Count;
                // get index of the row for the selected cell
                int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
                if (rowIndex == 0)
                    return;
                // get index of the column for the selected cell
                int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                dgv.Rows.Remove(selectedRow);
                dgv.Rows.Insert(rowIndex - 1, selectedRow);
                dgv.ClearSelection();
                dgv.Rows[rowIndex - 1].Cells[colIndex].Selected = true;
            }
            catch { }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            DataGridView dgv = controlPoints;
            try
            {
                int totalRows = dgv.Rows.Count;
                // get index of the row for the selected cell
                int rowIndex = dgv.SelectedCells[0].OwningRow.Index;
                if (rowIndex == totalRows - 1)
                    return;
                // get index of the column for the selected cell
                int colIndex = dgv.SelectedCells[0].OwningColumn.Index;
                DataGridViewRow selectedRow = dgv.Rows[rowIndex];
                dgv.Rows.Remove(selectedRow);
                dgv.Rows.Insert(rowIndex + 1, selectedRow);
                dgv.ClearSelection();
                dgv.Rows[rowIndex + 1].Cells[colIndex].Selected = true;
            }
            catch { }
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
            path.calibration = TurnCheck.Checked;

            return path;
        }

        #endregion
        //used to create the information that is showed in all of the charts.
        private void Apply_Click(object sender, EventArgs e)
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
                    if (path.controlPoints.Count == 0)
                    {
                        if(row.Cells[2].Value==null)
                        {
                            row.Cells[2].Value = "+";
                        }
                        if (row.Cells[2].Value.ToString() == "-")
                            path.direction = true;
                        if (row.Cells[2].Value.ToString() == "+")
                            path.direction = false;
                    }

                    if (row.Cells[2].Value.ToString() == "-")
                    {
                        mainField.Series["cp"].Points.Last().Color = Color.Red;
                        path.direction = true;
                    }

                    if (row.Cells[2].Value.ToString() == "+")
                    {
                        path.direction = false;
                    }


                    path.addControlPoint(float.Parse(row.Cells[1].Value.ToString()), float.Parse(row.Cells[0].Value.ToString()));

                    if (last != "" && last != row.Cells[2].Value.ToString())
                    {
                        if (row.Cells[2].Value.ToString() == "+")
                            path.direction = false;

                        if (row.Cells[2].Value.ToString() == "-")
                            path.direction = true;

                        if (path.controlPoints.Count >= 2)
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

            if (lastrow != null && lastrow.Cells[2].Value.ToString() != "-")
                path.direction = true;


            if (path.controlPoints.Count >= 2)
                paths.Add(path);

            if (!checkBox1.Checked)
                paths.test();
            else
                paths.Create(0);


            ClearChart(VelocityPlot);
            ClearChart(DistancePlot);

            float[] t, d, v, l, r, ld, rd, c, cd;



            if (TurnCheck.Checked)
            {
                if (int.Parse(degrees.Text) > 0)
                {
                    foreach (MotionProfile.Path p in paths)
                        p.direction = !p.direction;
                }

                t = paths.getTimeProfile();
                d = paths.getDistanceProfile();
                v = paths.getVelocityProfile();
                l = paths.getOffsetVelocityProfile(trackwidth).ToArray();
                ld = paths.getOffsetDistanceProfile(trackwidth).ToArray();
                c = paths.getOffsetVelocityProfile(0).ToArray();
                cd = paths.getOffsetDistanceProfile(0).ToArray();

                l.NoiseReduction(int.Parse(smoothness.Text));

                if (int.Parse(degrees.Text) > 0)
                {
                    foreach (MotionProfile.Path p in paths)
                        p.direction = !p.direction;
                }

                if (int.Parse(degrees.Text) < 0)
                {
                    foreach (MotionProfile.Path p in paths)
                        p.direction = !p.direction;
                }

                r = paths.getOffsetVelocityProfile(-trackwidth).ToArray();
                rd = paths.getOffsetDistanceProfile(-trackwidth).ToArray();
                if (int.Parse(degrees.Text) < 0)
                {
                    foreach (MotionProfile.Path p in paths)
                        p.direction = !p.direction;
                }

            }
            else
            {
                t = paths.getTimeProfile();
                d = paths.getDistanceProfile();
                v = paths.getVelocityProfile();
                l = paths.getOffsetVelocityProfile(trackwidth).ToArray();
                ld = paths.getOffsetDistanceProfile(trackwidth).ToArray();
                c = paths.getOffsetVelocityProfile(0).ToArray();
                cd = paths.getOffsetDistanceProfile(0).ToArray();

                l.NoiseReduction(int.Parse(smoothness.Text));
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

            pointList.Clear();


            foreach (ControlPoint p in paths.BuildPath())
            {
                foreach (PointF p1 in p.point)
                {
                    mainField.Series["path"].Points.AddXY(p1.Y, p1.X);

                    pointList.Add(new Point(p1.X, p1.Y, p.direction, p.pointNumber));
                    Debug.Print("Point : "+ p1.X+" , "+p1.Y);
                }
            }

            foreach (ControlPoint p in paths.BuildPath(trackwidth))
            {
                foreach (PointF p1 in p.point)
                {
                    mainField.Series["left"].Points.AddXY(p1.Y, p1.X);
                }
            }


            foreach (ControlPoint p in paths.BuildPath(-trackwidth))
            {
                foreach (PointF p1 in p.point)
                {
                    mainField.Series["right"].Points.AddXY(p1.Y, p1.X);
                }
            }
        }





        private void Save_Click(object sender, EventArgs e)
        {
            Apply_Click(null, null);
            if (pointList.Count > 2)
            {

                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                saveFileDialog1.Filter = "Motion Profile|*.mp;*.json";
                saveFileDialog1.Title = "Save an MP File";
                saveFileDialog1.ShowDialog();



                if (saveFileDialog1.FileName != "")
                {
                    String DirPath = System.IO.Path.GetDirectoryName(saveFileDialog1.FileName);    // Used for storing the directory path of the saved file.
                    String JSONPath = Path.Combine(DirPath, Path.GetFileNameWithoutExtension(saveFileDialog1.FileName) + ".json");     // Used for storing the json saved file directory path.
                    String MPPath = Path.Combine(DirPath, Path.GetFileNameWithoutExtension(saveFileDialog1.FileName) + ".mp");      // Used for storing the mp saved file directory path.



                    using (var writer = new System.IO.StreamWriter(JSONPath))
                    {
                        writer.WriteLine("{");
                        writer.WriteLine("  \"Data\":[ ");

                        List<string> left = new List<string>();
                        List<string> right = new List<string>();
                        List<string> center = new List<string>();

                        List<string> line = new List<string>();

                        int trackwidth = (int)((int.Parse(trackWidth.Text)) / 2);

                        float[] l = paths.getOffsetVelocityProfile(trackwidth).ToArray();
                        List<float> ld = paths.getOffsetDistanceProfile(trackwidth);

                        float[] r;
                        List<float> rd = new List<float>(); ;

                        float[] c = paths.getOffsetVelocityProfile(0).ToArray();
                        List<float> cd = paths.getOffsetDistanceProfile(0);

                        float[] angles = new float[pointList.Count - 2];

                        if (TurnCheck.Checked)
                        {
                            foreach (MotionProfile.Path ph in paths)
                                ph.direction = !ph.direction;

                            r = paths.getOffsetVelocityProfile(-trackwidth).ToArray();
                            rd = paths.getOffsetDistanceProfile(-trackwidth);

                            foreach (MotionProfile.Path ph in paths)
                                ph.direction = !ph.direction;


                            float targetangle = int.Parse(degrees.Text); //Change


                            float angle = 0;

                            for (int i = 0; i < (pointList.Count - 2); i++)
                            {
                                if (i == 0 || i == 1)
                                    angles[i] = 0;
                                else
                                {
                                    angle += fpstodps(c[i - 1]);
                                    angles[i] = angle;

                                }
                            }
                        }
                        else
                        {
                            r = paths.getOffsetVelocityProfile(-trackwidth).ToArray();
                            rd = paths.getOffsetDistanceProfile(-trackwidth);

                            float startAngle = findStartAngle(pointList[1].x, pointList[0].x, pointList[1].y, pointList[0].y);
                            for (int i = 0; i < (pointList.Count - 2); i++) //for not zeroing the angle after each path.
                            {

                                if (i == 0)
                                {
                                    angles[i] = findStartAngle(pointList[i + 1].x, pointList[i].x, pointList[i + 1].y, pointList[i].y);
                                }
                                else
                                {
                                    angles[i] = findAngleChange(pointList[i + 1].x, pointList[i].x, pointList[i + 1].y, pointList[i].y, angles[i - 1]);
                                }
                            }
                            for (int i = 0; i < (pointList.Count - 2); i++) // part of the last for. kinda. you know what i mean.
                            {
                                angles[i] = (angles[i] - startAngle);
                                Boolean forward;
                                if (i == pointList.Count - 1) forward = pointList[i].direction;
                                else forward = pointList[i + 1].direction;
                                if (!forward)
                                {
                                    angles[i] = -angles[i];
                                    int add = 0;
                                    if (angles[i] > 0)
                                        add = -180;
                                    if (angles[i] < 0)
                                        add = 180;
                                    angles[i] = angles[i] + add;
                                }


                            }
                        }

                        r.NoiseReduction(int.Parse(smoothness.Text));
                        rd.NoiseReduction(int.Parse(smoothness.Text));
                        l.NoiseReduction(int.Parse(smoothness.Text));
                        ld.NoiseReduction(int.Parse(smoothness.Text));
                        c.NoiseReduction(int.Parse(smoothness.Text));
                        cd.NoiseReduction(int.Parse(smoothness.Text));

                        for (int i = 0; i < l.Length; i++)
                        {
                            if (CTRE.Checked)
                            {
                                double dConvert = Math.PI * double.Parse(wheel.Text) * 25.4;

                                line.Add("  {   \"Rotation\":" + cd.Take(i).Sum() / dConvert + " , " + "\"Velocity\":" + (c[i] / dConvert * 60).ToString() + " , " + "\"Time\":" + paths[0].velocityMap.time * 1000 + " , " + "\"Angle\":" + angles[i] + "}");

                            }
                            else
                            {
                                line.Add("  {   \"Rotation\":" + cd.Take(i).Sum().ToString() + " , " + "\"Velocity\":" + c[i].ToString() + " , " + "\"Time\":" + paths[0].velocityMap.time * 1000 + " , " + "\"Angle\":" + angles[i] + "}");
                            }
                        }
                        right.Add(string.Join(",\n", line));

                        foreach (string ret in right)
                        {
                            writer.WriteLine(ret);
                        }
                        writer.WriteLine("  ] ");
                        writer.WriteLine("} ");
                    }
                    WriteSetupFile(MPPath);

                }

            }
            else
            {
                MessageBox.Show("You can't save a file with no points!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public float findAngleChange(double x2, double x1, double y2, double y1, float prevAngle)
        {
            float ang = 0;
            float chx = (float)(x2 - x1);
            float chy = (float)(y2 - y1);
            if (chy == 0)
            {
                if (chx >= 0) ang = 0;
                else ang = 180;
            }
            else if (chy > 0)
            {                         // X AND Y ARE REVERSED BECAUSE OF MOTION PROFILER STUFF
                if (chx > 0)
                {
                    // positive x, positive y, 90 - ang, quad 1
                    ang = (float)(90 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = (float)(CONVERT * Math.Atan(chx / chy));
                    //ang = 1; // represents quadrants.
                }
                else
                {
                    // positive x, negative y, 90 + ang, quad 2
                    ang = (float)(90 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = (float)(CONVERT * Math.Atan(chx / chy));
                    //ang = 2;
                }
            }
            else
            {
                if (chx > 0)
                {
                    // negative x, positive y, 270 + ang, quad 4
                    ang = (float)(270 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = (float)(CONVERT * Math.Atan(chx / chy));
                    //ang = 4;
                }
                else
                {
                    // negative x, negative y, 270 - ang, quad 3
                    ang = (float)(270 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = (float)(CONVERT * Math.Atan(chx / chy));
                    //ang = 3;
                }
            }

            float angleChange = ang - prevAngle;
            if (angleChange > 300) angleChange -= 360;
            if (angleChange < -300) angleChange += 360;
            return (prevAngle + angleChange);
        }

        public float findStartAngle(double x2, double x1, double y2, double y1)
        {
            float ang = 0;
            float chx = (float)(x2 - x1);
            float chy = (float)(y2 - y1);
            if (chy == 0)
            {
                if (chx >= 0) ang = 0;
                else ang = 180;
            }
            else if (chy > 0)
            {                         // X AND Y ARE REVERSED BECAUSE OF MOTION PROFILER STUFF
                if (chx > 0)
                {
                    // positive x, positive y, 90 - ang, quad 1
                    ang = (float)(90 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = (float)(CONVERT * Math.Atan(chx / chy));
                    //ang = 1; // represents quadrants.
                }
                else
                {
                    // positive x, negative y, 90 + ang, quad 2
                    ang = (float)(90 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = (float)(CONVERT * Math.Atan(chx / chy));
                    //ang = 2;
                }
            }
            else
            {
                if (chx > 0)
                {
                    // negative x, positive y, 270 + ang, quad 4
                    ang = (float)(270 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = (float)(CONVERT * Math.Atan(chx / chy));
                    //ang = 4;
                }
                else
                {
                    // negative x, negative y, 270 - ang, quad 3
                    ang = (float)(270 - CONVERT * (Math.Atan(chx / chy)));
                    //ang = (float)(CONVERT * Math.Atan(chx / chy));
                    //ang = 3;
                }
            }
            return ang;
        }

        private void WriteSetupFile(string path)
        {
            var writer1 = new System.IO.StreamWriter(path);

            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);

            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;

                writer.WriteStartObject();

                writer.WritePropertyName("Max Velocity");
                writer.WriteValue(maxVelocity.Text);

                writer.WritePropertyName("Track Width");
                writer.WriteValue(trackWidth.Text);

                writer.WritePropertyName("Accel Rate");
                writer.WriteValue(AccelRate.Text);

                writer.WritePropertyName("Time Sample");
                writer.WriteValue(timeSample.Text);

                writer.WritePropertyName("Wheel Diameter");
                writer.WriteValue(wheel.Text);

                writer.WritePropertyName("Speed Limit");
                writer.WriteValue(SpeedLimit.Text);

                writer.WritePropertyName("Smoothness");
                writer.WriteValue(smoothness.Text);

                writer.WritePropertyName("CTRE");
                writer.WriteValue(CTRE.Checked.ToString());

                writer.WritePropertyName("isntaVel");
                writer.WriteValue(isntaVel.Checked.ToString());

                writer.WritePropertyName("Profile Name");
                writer.WriteValue(profilename.Text);

                writer.WritePropertyName("Username");
                writer.WriteValue(user.Text);

                writer.WritePropertyName("Ip-Address");
                writer.WriteValue(ipadd.Text);

                writer.WritePropertyName("Points");
                writer.WriteStartArray();

                foreach (DataGridViewRow row in controlPoints.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {

                        writer.WriteStartArray();
                        writer.WriteValue(string.Concat(row.Cells[0].Value.ToString()));
                        writer.WriteValue(row.Cells[1].Value.ToString());
                        writer.WriteValue(row.Cells[2].Value.ToString());
                        writer.WriteEndArray();
                    }
                }
                writer.WriteEndArray();
                writer.WriteEndObject();
            }
            writer1.WriteLine(sb.ToString());
            writer1.Close();

        }

        private void Load_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            openFileDialog1.FileName = "";
            openFileDialog1.Filter = "MotionProfile Data (*.mp)|*.mp";

            DialogResult results = openFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                using (var reader1 = new System.IO.StreamReader(openFileDialog1.FileName))
                {
                    controlPoints.Rows.Clear();

                    string json = reader1.ReadToEnd();

                    JObject o = JObject.Parse(json);

                    maxVelocity.Text=(string)o["Max Velocity"];
                    trackWidth.Text = (string)o["Track Width"];
                    AccelRate.Text = (string)o["Accel Rate"];
                    timeSample.Text = (string)o["Time Sample"];
                    SpeedLimit.Text = (string)o["Speed Limit"];
                    wheel.Text = (string)o["Wheel Diameter"];
                    smoothness.Text = (string)o["Smoothness"];
                    CTRE.Checked = Boolean.Parse((string)o["CTRE"]);
                    isntaVel.Checked = Boolean.Parse((string)o["isntaVel"]);

                    profilename.Text = (string)o["Profile Name"];
                    user.Text = (string)o["Username"];
                    ipadd.Text = (string)o["Ip-Address"];

                    JArray a = (JArray)o["Points"];

                    for(int x=0; x<=a.Count-1; x++)
                    {
                        controlPoints.Rows.Add(float.Parse((string)a[x][0]), float.Parse((string)a[x][1]), (string)a[x][2]);
                    }
                }
            }
            Apply_Click(null, null);
        }

        private void CalCheck_CheckedChanged(object sender, EventArgs e)
        {
            offset.Text = "0";
            offset.Enabled = false;
            degrees.Enabled = true;
            ClearCP_Click(null, null);
            if (TurnCheck.Checked)
            {
                degrees.Enabled = false;
                offset.Enabled = true;
                this.maxVelocity.Text = "1500";
                controlPoints.Rows.Add(1000, 0, "+");
                controlPoints.Rows.Add(1000, Math.Abs(0 + int.Parse(trackWidth.Text) * Math.PI * int.Parse(degrees.Text) / 360), "+");
                Apply_Click(null, null);
            }
        }



        private void Rotations_TextChanged(object sender, EventArgs e)
        {
            if (TurnCheck.Checked && degrees.Text != "")
            {
                ClearCP_Click(null, null);
                controlPoints.Rows.Add(1000, 0, "+");
                controlPoints.Rows.Add(1000, 0 + int.Parse(trackWidth.Text) * Math.PI * int.Parse(degrees.Text) / 360, "+");
                Apply_Click(null, null);
            }
        }

        private void CTRE_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
        public float fpstodps(float Vel)
        {
            if (int.Parse(degrees.Text) > 0)
            {
                float dgps = (float)((87.92 / 360.0) * (int.Parse(wheel.Text) * Math.PI * Vel / 60));

                return -(float)(dgps * .02199);
            }
            else
            {
                float dgps = (float)((87.92 / 360.0) * (int.Parse(wheel.Text) * Math.PI * Vel / 60));

                return (float)(dgps * .02199);
            }


        }

        private void NODATA_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (profilename.Text=="")
            {
                MessageBox.Show("You must give this profile a name!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ValidateIPv4(ipadd.Text))
            {
                MessageBox.Show("This ip address is invalid!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            String DirPath = Path.GetTempPath();    // Used for storing the directory path of the saved file.
            String JSONPath = Path.Combine(DirPath, profilename.Text + ".json");     // Used for storing the json saved file directory path.
            //This is almost the same as saving the file however this one will be a temp file which will be deleted after deploying.
            Apply_Click(null, null);
            if (pointList.Count > 2)
            {
                    using (var writer = new System.IO.StreamWriter(JSONPath))
                    {
                        writer.WriteLine("{");
                        writer.WriteLine("  \"Data\":[ ");

                        List<string> left = new List<string>();
                        List<string> right = new List<string>();
                        List<string> center = new List<string>();

                        List<string> line = new List<string>();

                        int trackwidth = (int)((int.Parse(trackWidth.Text)) / 2);

                        float[] l = paths.getOffsetVelocityProfile(trackwidth).ToArray();
                        List<float> ld = paths.getOffsetDistanceProfile(trackwidth);

                        float[] r;
                        List<float> rd = new List<float>(); ;

                        float[] c = paths.getOffsetVelocityProfile(0).ToArray();
                        List<float> cd = paths.getOffsetDistanceProfile(0);

                        float[] angles = new float[pointList.Count - 2];

                        if (TurnCheck.Checked)
                        {
                            foreach (MotionProfile.Path ph in paths)
                                ph.direction = !ph.direction;

                            r = paths.getOffsetVelocityProfile(-trackwidth).ToArray();
                            rd = paths.getOffsetDistanceProfile(-trackwidth);

                            foreach (MotionProfile.Path ph in paths)
                                ph.direction = !ph.direction;


                            float targetangle = int.Parse(degrees.Text); //Change


                            float angle = 0;

                            for (int i = 0; i < (pointList.Count - 2); i++)
                            {
                                if (i == 0 || i == 1)
                                    angles[i] = 0;
                                else
                                {
                                    angle += fpstodps(c[i - 1]);
                                    angles[i] = angle;

                                }
                            }
                        }
                        else
                        {
                            r = paths.getOffsetVelocityProfile(-trackwidth).ToArray();
                            rd = paths.getOffsetDistanceProfile(-trackwidth);

                            float startAngle = findStartAngle(pointList[1].x, pointList[0].x, pointList[1].y, pointList[0].y);
                            for (int i = 0; i < (pointList.Count - 2); i++) //for not zeroing the angle after each path.
                            {
                                if (i == 0)
                                {
                                    angles[i] = findStartAngle(pointList[i + 1].x, pointList[i].x, pointList[i + 1].y, pointList[i].y);
                                }
                                else
                                {
                                    angles[i] = findAngleChange(pointList[i + 1].x, pointList[i].x, pointList[i + 1].y, pointList[i].y, angles[i - 1]);
                                    
                                }
                            }
                            for (int i = 0; i < (pointList.Count - 2); i++) // part of the last for. kinda. you know what i mean.
                            {
                                angles[i] = (angles[i] - startAngle);
                                angles[i] = -angles[i];
                                int add = 0;
                                if (angles[i] > 0)
                                    add = -180;
                                if (angles[i] < 0)
                                    add = 180;
                                angles[i] = angles[i] + add;
                        }
                        }

                        r.NoiseReduction(int.Parse(smoothness.Text));
                        rd.NoiseReduction(int.Parse(smoothness.Text));
                        l.NoiseReduction(int.Parse(smoothness.Text));
                        ld.NoiseReduction(int.Parse(smoothness.Text));
                        c.NoiseReduction(int.Parse(smoothness.Text));
                        cd.NoiseReduction(int.Parse(smoothness.Text));





                        for (int i = 0; i < l.Length; i++)
                        {
                            if (CTRE.Checked)
                            {
                                double dConvert = Math.PI * double.Parse(wheel.Text) * 25.4;

                                line.Add("  {   \"Rotation\":" + cd.Take(i).Sum() / dConvert + " , " + "\"Velocity\":" + (c[i] / dConvert * 60).ToString() + " , " + "\"Time\":" + paths[0].velocityMap.time * 1000 + " , " + "\"Angle\":" + angles[i] + "}");

                            }
                            else
                            {
                                line.Add("  {   \"Rotation\":" + cd.Take(i).Sum().ToString() + " , " + "\"Velocity\":" + c[i].ToString() + " , " + "\"Time\":" + paths[0].velocityMap.time * 1000 + " , " + "\"Angle\":" + angles[i] + "}");
                            }
                        }
                        right.Add(string.Join(",\n", line));

                        foreach (string ret in right)
                        {
                            writer.WriteLine(ret);
                        }
                        writer.WriteLine("  ] ");
                        writer.WriteLine("} ");
                    }
                }
            else
            {
                MessageBox.Show("You can't deploy a file with no points!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SftpClient sftp = new SftpClient(ipadd.Text, user.Text, pass.Text);
            
            try
            {
                this.Cursor = Cursors.WaitCursor;
                sftp.Connect();
                try
                {
                    sftp.CreateDirectory("/home/lvuser/Motion_Profiles");

                }
                catch(Exception e2)
                {

                }
                using (FileStream fileStream = File.OpenRead(JSONPath))
                {
                    MemoryStream memStream = new MemoryStream();
                    memStream.SetLength(fileStream.Length);
                    fileStream.Read(memStream.GetBuffer(), 0, (int)fileStream.Length);
                    sftp.UploadFile(memStream, Path.Combine("/home/lvuser/Motion_Profiles/", profilename.Text + ".json"));
                }
            }
            catch(Renci.SshNet.Common.SftpPermissionDeniedException e1)
            {
                Console.WriteLine("IOException source: {0}", e1.StackTrace);
                this.Cursor = Cursors.Default;
                MessageBox.Show("Permission Denied By Host!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Renci.SshNet.Common.SftpPathNotFoundException e1)
            {
                Console.WriteLine("IOException source: {0}", e1.StackTrace);
                this.Cursor = Cursors.Default;
                MessageBox.Show("Path Not Found By Host!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception e1)
            {
                Console.WriteLine("IOException source: {0}", e1.StackTrace);
                this.Cursor = Cursors.Default;
                MessageBox.Show("Unable to connect to host!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            this.Cursor = Cursors.Default;
            MessageBox.Show("Success", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            sftp.Disconnect();
            System.Threading.Thread.Sleep(100);
            File.Delete(JSONPath);
        }


        //This method is used to verifiy if the numbers that are put into the textbox are in the format of a ip address like 127.0.0.1 and not like 0.1.54.6.65655
        public bool ValidateIPv4(string ipString)
        {
            // if the text contains a whitespace/space or a null value then it is clearly not a ip address.
            if (String.IsNullOrWhiteSpace(ipString))
            {
                return false;
            }
            //Split the ip address into different parts
            string[] splitValues = ipString.Split('.');
            if (splitValues.Length != 4)
            {
                return false;
            }

            byte tempForParsing;
            //check to see if all of the values are bytes.
            return splitValues.All(r => byte.TryParse(r, out tempForParsing));
        }
    }
}

