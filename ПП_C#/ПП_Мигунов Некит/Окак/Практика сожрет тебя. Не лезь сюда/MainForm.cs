﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Практика_сожрет_тебя.Не_лезь_сюда.images
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
