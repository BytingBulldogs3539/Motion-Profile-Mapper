﻿
namespace VelocityMap.Forms
{
    partial class ConfigurationView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.newProfileButton = new FontAwesome.Sharp.IconButton();
            this.deleteProfileButton = new FontAwesome.Sharp.IconButton();
            this.saveToRioButton = new System.Windows.Forms.Button();
            this.refresh_button = new System.Windows.Forms.Button();
            this.configurationGrid = new System.Windows.Forms.DataGridView();
            this.Variable_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Variable_Type = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.configurationGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 64);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(240, 395);
            this.dataGridView1.TabIndex = 1;
            // 
            // newProfileButton
            // 
            this.newProfileButton.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.newProfileButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.newProfileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newProfileButton.ForeColor = System.Drawing.Color.DarkGray;
            this.newProfileButton.IconChar = FontAwesome.Sharp.IconChar.Plus;
            this.newProfileButton.IconColor = System.Drawing.Color.Green;
            this.newProfileButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.newProfileButton.IconSize = 24;
            this.newProfileButton.Location = new System.Drawing.Point(12, 462);
            this.newProfileButton.Margin = new System.Windows.Forms.Padding(0);
            this.newProfileButton.Name = "newProfileButton";
            this.newProfileButton.Size = new System.Drawing.Size(108, 30);
            this.newProfileButton.TabIndex = 49;
            this.newProfileButton.UseVisualStyleBackColor = false;
            // 
            // deleteProfileButton
            // 
            this.deleteProfileButton.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.deleteProfileButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deleteProfileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteProfileButton.ForeColor = System.Drawing.Color.DarkGray;
            this.deleteProfileButton.IconChar = FontAwesome.Sharp.IconChar.Trash;
            this.deleteProfileButton.IconColor = System.Drawing.Color.Firebrick;
            this.deleteProfileButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.deleteProfileButton.IconSize = 24;
            this.deleteProfileButton.Location = new System.Drawing.Point(144, 462);
            this.deleteProfileButton.Margin = new System.Windows.Forms.Padding(0);
            this.deleteProfileButton.Name = "deleteProfileButton";
            this.deleteProfileButton.Size = new System.Drawing.Size(108, 30);
            this.deleteProfileButton.TabIndex = 50;
            this.deleteProfileButton.UseVisualStyleBackColor = false;
            // 
            // saveToRioButton
            // 
            this.saveToRioButton.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.saveToRioButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveToRioButton.FlatAppearance.BorderSize = 0;
            this.saveToRioButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.saveToRioButton.Font = new System.Drawing.Font("Verdana", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveToRioButton.ForeColor = System.Drawing.Color.DarkCyan;
            this.saveToRioButton.Location = new System.Drawing.Point(12, 495);
            this.saveToRioButton.Name = "saveToRioButton";
            this.saveToRioButton.Size = new System.Drawing.Size(240, 50);
            this.saveToRioButton.TabIndex = 62;
            this.saveToRioButton.Text = "Save to RIO";
            this.saveToRioButton.UseVisualStyleBackColor = false;
            // 
            // refresh_button
            // 
            this.refresh_button.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.refresh_button.Cursor = System.Windows.Forms.Cursors.Hand;
            this.refresh_button.FlatAppearance.BorderSize = 0;
            this.refresh_button.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.refresh_button.Font = new System.Drawing.Font("Verdana", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refresh_button.ForeColor = System.Drawing.Color.DarkCyan;
            this.refresh_button.Location = new System.Drawing.Point(12, 12);
            this.refresh_button.Name = "refresh_button";
            this.refresh_button.Size = new System.Drawing.Size(240, 46);
            this.refresh_button.TabIndex = 63;
            this.refresh_button.Text = "Load from RIO";
            this.refresh_button.UseVisualStyleBackColor = false;
            // 
            // configurationGrid
            // 
            this.configurationGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.configurationGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Variable_Name,
            this.Variable_Type,
            this.Value});
            this.configurationGrid.Location = new System.Drawing.Point(258, 12);
            this.configurationGrid.Name = "configurationGrid";
            this.configurationGrid.RowHeadersWidth = 51;
            this.configurationGrid.RowTemplate.Height = 24;
            this.configurationGrid.Size = new System.Drawing.Size(697, 480);
            this.configurationGrid.TabIndex = 65;
            this.configurationGrid.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView3_CellValidating);
            this.configurationGrid.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dataGridView1_EditingControlShowing);
            // 
            // Variable_Name
            // 
            this.Variable_Name.HeaderText = "Variable Name";
            this.Variable_Name.MinimumWidth = 6;
            this.Variable_Name.Name = "Variable_Name";
            this.Variable_Name.Width = 300;
            // 
            // Variable_Type
            // 
            this.Variable_Type.HeaderText = "Variable Type";
            this.Variable_Type.Items.AddRange(new object[] {
            "Int",
            "Double",
            "Boolean",
            "String"});
            this.Variable_Type.MinimumWidth = 6;
            this.Variable_Type.Name = "Variable_Type";
            this.Variable_Type.Width = 125;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.MinimumWidth = 6;
            this.Value.Name = "Value";
            this.Value.Width = 225;
            // 
            // ConfigurationView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 554);
            this.Controls.Add(this.configurationGrid);
            this.Controls.Add(this.refresh_button);
            this.Controls.Add(this.saveToRioButton);
            this.Controls.Add(this.deleteProfileButton);
            this.Controls.Add(this.newProfileButton);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ConfigurationView";
            this.Text = "ConfigurationView";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.configurationGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private FontAwesome.Sharp.IconButton newProfileButton;
        private FontAwesome.Sharp.IconButton deleteProfileButton;
        private System.Windows.Forms.Button saveToRioButton;
        private System.Windows.Forms.Button refresh_button;
        private System.Windows.Forms.DataGridView configurationGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Variable_Name;
        private System.Windows.Forms.DataGridViewComboBoxColumn Variable_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
    }
}