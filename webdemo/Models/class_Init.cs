using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Data;
using System.Collections;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace webdemo.Models {
    public class class_Init {
        public static string HostType = "";
        public static string DateFormat;
        public void AppInitialization(string iHostType) {
            DateFormat = "yyyy-MM-dd";
            HostType = iHostType;
        }

    }
}


