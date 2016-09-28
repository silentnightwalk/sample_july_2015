﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MetrologyAdmin
{
    /// <summary>
    /// Interaction logic for AddEditUserControl.xaml
    /// </summary>
    public partial class AddEditUserView : UserControl
    {
        public AddEditUserView(AddUserViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

        public AddEditUserView(EditUserViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }
    }
}