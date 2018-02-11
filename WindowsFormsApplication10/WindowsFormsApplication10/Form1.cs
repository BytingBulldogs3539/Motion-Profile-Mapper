using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;

namespace MotionProfileMapper
{
    
    public partial class Form1 : Form
    {
        private List<int[]> fieldpts = new List<int[]>();

        private int fieldWidth = 8229;
        int padding = 1;
        private float scale = 1;
        int dr = 1;
        private Path robotPath;

        public Form1()
        {
            InitializeComponent();
        }

        #region Form Mehtods

        private void Form1_Load(object sender, EventArgs e)
        {
            //read in field point objects
            using (var reader = new System.IO.StreamReader("FieldPoints.txt"))
            {
                while (!reader.EndOfStream)
                {
                    List<string> line = reader.ReadLine().Split('\'').ToList<string>();
                    if (!line[0].Equals ("")) {
                        line = line[0].Split(',').ToList<string>();
                        List<int> lineout = new List<int>();
                        foreach (string item in line)
                        {
                            lineout.Add(int.Parse(item));
                        }
                    fieldpts.Add(lineout.ToArray());
                    }
                }
            }

            //draw the grid and fieldobjects
            renderPicture();
        }
        private void Form1_ResizeEnd(object sender, EventArgs e)
        {
            //redraw the bitmap since the picturebox likely changed size
            renderPicture();
        }
        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            //redraw the bitmap since the picturebox likely changed size
            renderPicture();
        }
        private void Form1_AutoSizeChanged(object sender, EventArgs e)
        {
            //redraw the bitmap since the picturebox likely changed size
            renderPicture();
        }
        #endregion

        #region bitmap drawing
        //function to help orientation of drawing rectangles
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

        //draw the grid for the screen
        private Bitmap drawGrid(Bitmap b, int gridsize)
        {
            scale = Math.Max(b.Width*1.0f  / pictureBox1.Width , b.Height * 1.0f / pictureBox1.Height);

            gridsize = Math.Max(b.Width, b.Height);
            gridsize = gridsize / 10;

            if (gridsize > 500) gridsize = 1000;
            else if(gridsize > 50) gridsize = 100;
            else if(gridsize > 5) gridsize = 10;

            //grab handles
            Graphics g = Graphics.FromImage(b);
            Pen p = new Pen(Color.Gray , scale);
            Font f = new Font("Arial", 8*scale);

            //draw both X and Y since we are square
            for(int x = gridsize; x< b.Width ; x=x+ gridsize)
            {
                g.DrawLine(p, x, 200, x , b.Height);
                TextRenderer.DrawText(g, x.ToString(), f, new Point(x-200, 10), Color.Gray);

                g.DrawLine(p, 400, x, b.Height , x);
                TextRenderer.DrawText(g, (x).ToString(), f, new Point(10, x-75), Color.Gray);
            }

            //ensure we flush the graphics handle then clean up all handles
            g.Flush();

            p.Dispose();
            g.Dispose();
            f.Dispose();
            return b;

        }

        //here is where we draw everyting to the bitmap
        private void renderPicture()
        {
            Pen bluePen = new Pen(Color.Red , 10);

            //create the drawing bitmap
            Bitmap b = new Bitmap(fieldWidth + padding*2, fieldWidth + padding*2);
            
            //draw the grid on the bitmap
            drawGrid(b, 50);

            //draw the field size on the bitmap
            Graphics g = Graphics.FromImage(b);
            g.DrawRectangle(bluePen, new Rectangle(0, 0 , b.Width-padding  , b.Height-padding));

            //draw the fieldObjects on the bitmap
            foreach (int[] obj in fieldpts)
            {
                if (obj.Length >= 4)
                {
                    int[] pts = obj.Take(4).ToArray<int>();
                    Brush brush = Brushes.ForestGreen;
                    Pen pen = new Pen(Color.Black , 5);

                    if (obj.Length > 4 )
                    {
                        switch (obj[4])
                        {
                            case 0:
                                brush = Brushes.Red ;
                                break;
                            case 1:
                                brush = Brushes.Yellow  ;
                                break;
                            case 2:
                                brush = Brushes.LightGray  ;
                                break;
                        }
                    }
                    g.FillRectangle(brush, makeRectangle(pts));
                    g.DrawRectangle(pen , makeRectangle(pts));
                }
            }

            //setup the radius of the dots for the robot path
            dr = (int)(1.5 * scale);

            //create a new path class to calculate the robot path
            robotPath = new Path();

            //gather up the control points from the list
            if (controlPoints.Rows.Count > 1)
            {
                foreach (DataGridViewRow row in controlPoints.Rows)
                {
                    if (row.Cells[0].Value != null)
                    {
                        //add the control points to the path and draw them on the bitmap
                        robotPath.controlPoints.add(new Point(int.Parse(row.Cells[0].Value.ToString()),int.Parse(row.Cells[1].Value.ToString())));
                        g.FillEllipse(Brushes.Red, new Rectangle(int.Parse(row.Cells[1].Value.ToString()) - dr * 2, int.Parse(row.Cells[0].Value.ToString()) - dr * 2, dr * 4, dr * 4));
                    }
                }

                //create the path. Add other path parameters if needed before calling create.
                robotPath.Create();

                //clear out any old sline calculations and add in the new ones while drawing them on the bitmap. 
                SplineOutput.Rows.Clear();
                for (int i = 0; i < robotPath.spline.points().Count-1; i++)
                {
                    SplineOutput.Rows.Add(robotPath.spline.x(i), robotPath.spline.y(i));
                    g.FillEllipse(Brushes.Aqua, new Rectangle((int)robotPath.spline.y(i) - dr, (int)robotPath.spline.x(i) - dr, dr * 2, dr * 2));
                    g.FillEllipse(Brushes.Blue, new Rectangle((int)robotPath.rightTrack.y(i) - dr, (int)robotPath.rightTrack.x(i) - dr, dr * 2, dr * 2));
                    g.FillEllipse(Brushes.Blue, new Rectangle((int)robotPath.leftTrack.y(i) - dr, (int)robotPath.leftTrack.x(i) - dr, dr * 2, dr * 2));
                }
            }

            //scale the bitmap to the size of the picturebox to maximize screen use
            int bw = Math.Min(b.Width, pictureBox1.Width);
            int bh = Math.Min(b.Height, pictureBox1.Height);
            int by = b.Height - pictureBox1.Height;
            if (pictureBox1.Height >= b.Height)
                by = b.Height;
            pictureBox1.Image = b;

            //clear up remaining handles
            bluePen.Dispose();
            g.Dispose();
        }
        #endregion

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            renderPicture();
        }

        private void ControlPointsLoad_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Application.StartupPath;
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
                            controlPoints.Rows.Add(int.Parse(l[0]), int.Parse(l[1]));
                        }
                    }
                }
                
                renderPicture();
            }
        }

        private void SplinePointsSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK )
            {
                if (robotPath.controlPoints.length() > 0)
                {
                    String fileName = saveFileDialog1.FileName;
                    robotPath.write(fileName);
                }

                /*  VanCode
                using (var writer = new System.IO.StreamWriter(saveFileDialog1.FileName))
                {
                    foreach(DataGridViewRow row in SplineOutput.Rows )
                    {
                        if (row.Cells[0].Value != null)
                        {
                            writer.WriteLine(string.Concat(row.Cells[0].Value.ToString(), ",", row.Cells[1].Value.ToString()));
                        }
                    }
                }
                */
            }
        }

        private void ControlPointsSave_Click(object sender, EventArgs e)
        {
            saveFileDialog1.InitialDirectory = Application.StartupPath;
            DialogResult results = saveFileDialog1.ShowDialog();
            if (results == DialogResult.OK)
            {
                using (var writer = new System.IO.StreamWriter(saveFileDialog1.FileName))
                {
                    foreach (DataGridViewRow row in controlPoints.Rows)
                    {
                        if (row.Cells[0].Value != null)
                        {
                            writer.WriteLine(string.Concat(row.Cells[0].Value.ToString(), ",", row.Cells[1].Value.ToString()));
                        }
                    }
                }
            }
        }
 

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            var d = Math.Abs(pictureBox1.Height - pictureBox1.Width)/2 * scale;
            var x = (int)(e.X * scale);
            var y = (int)(e.Y * scale);

            if (pictureBox1.Width > pictureBox1.Height)
                x = (int)(x - d);
            else
                y = (int)(y - d);

            //grab graphics handle for the picturebox and draw point
            Graphics g = Graphics.FromImage(pictureBox1.Image);
            g.FillEllipse(Brushes.Red, new Rectangle(x- dr * 2, y - dr * 2, dr * 4, dr * 4));

            //push and clean up handle
            g.Flush();
            g.Dispose();

            pictureBox1.Refresh();
            controlPoints.Rows.Add(y, x);
        }

        #region ControlPointMenu
        private int cpRow = 0;

        private void controlPoints_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DataGridView d = (DataGridView)sender;

                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("Insert", new EventHandler(InsertPoint)));
                m.MenuItems.Add(new MenuItem("Delete", new EventHandler(DeletePoint)));
                m.MenuItems.Add(new MenuItem("Move Up", new EventHandler(MoveUpPoint)));
                m.MenuItems.Add(new MenuItem("Move Down", new EventHandler(MoveDownPoint)));
                m.MenuItems.Add(new MenuItem("Clear All", new EventHandler(ClearPoint)));
                // m.MenuItems.Add(new MenuItem("Paste"));

                cpRow = d.HitTest(e.X, e.Y).RowIndex;

                m.Show(d, new Point(e.X, e.Y));

                if (d.CurrentCell != null)
                {
                    // d.Rows.Insert(d.CurrentCell.RowIndex, new DataGridViewRow());
                }
            }
        }

        private void controlPoints_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (controlPoints.Rows.Count > 1)
                ApplyButton.Enabled = true;
        }

        public void InsertPoint(object sender, EventArgs e)
        {
            if (cpRow > 0)
            {
                controlPoints.Rows.Insert(cpRow, new DataGridViewRow());
                cpRow = 0;
            }
        }
        public void ClearPoint(object sender, EventArgs e)
        {
            if (cpRow > 0)
            {
                controlPoints.Rows.Clear();
                SplineOutput.Rows.Clear();
                ApplyButton.Enabled = false;
                cpRow = 0;
                renderPicture();
            }
        }
        public void DeletePoint(object sender, EventArgs e)
        {
            if (cpRow > 0 && cpRow < (controlPoints.Rows.Count - 1))
            {
                controlPoints.Rows.RemoveAt(cpRow);
                cpRow = 0;
                renderPicture();
            }
        }
        public void MoveUpPoint(object sender, EventArgs e)
        {
            if (cpRow > 1 && cpRow < (controlPoints.Rows.Count - 1))
            {
                DataGridViewRow r = controlPoints.Rows[cpRow];
                controlPoints.Rows.RemoveAt(cpRow);
                controlPoints.Rows.Insert(cpRow - 1, r);
                cpRow = 0;

            }
        }
        public void MoveDownPoint(object sender, EventArgs e)
        {
            if (cpRow > 0 && cpRow < (controlPoints.Rows.Count - 2))
            {
                DataGridViewRow r = controlPoints.Rows[cpRow];
                controlPoints.Rows.RemoveAt(cpRow);
                controlPoints.Rows.Insert(cpRow + 1, r);
                cpRow = 0;

            }
        }
        #endregion


    
    }
}
