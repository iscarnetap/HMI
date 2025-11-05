using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
//using System.IO;
using System.Runtime.InteropServices; // for the P/Invoke

namespace Inspection
{
    internal static class TsaiDll
    {
        #region TsaiDll.dll

        static System.IntPtr hTsaiDll; // handles of a loaded DLL and functions  //static  //(lTsai As Long 'handle of TsaiDll.dll)
        
        static IntPtr hTsaiCalibrate;
        internal delegate int DelegateTsaiCalibrate(string fname, int sensor_width_pix, int sensor_hight_pix, Single sensor_microns_pix,
            Single x1, Single y1, Single xim1, Single yim1, Single x2, Single y2, Single xim2, Single yim2,
            Single x3, Single y3, Single xim3, Single yim3, Single x4, Single y4, Single xim4, Single yim4);
        internal static DelegateTsaiCalibrate fTsaiCalibrate;

        static IntPtr hTsaiLoadCalibration;
        internal delegate int DelegateTsaiLoadCalibration(string fname, int sensor_width_pix, int sensor_hight_pix, Single sensor_microns_pix);
        internal static DelegateTsaiLoadCalibration fTsaiLoadCalibration; 

        static IntPtr hTsaiImageToWorld;
        internal delegate int DelegateTsaiImageToWorld(Single xim, Single yim, ref Single xworld, ref Single yworld);
        internal static DelegateTsaiImageToWorld fTsaiImageToWorld;

        static IntPtr hTsaiWorldToImage;
        internal delegate int DelegateTsaiWorldToImage(Single xworld, Single yworld, ref Single xim, ref Single yim);
        internal static DelegateTsaiWorldToImage fTsaiWorldToImage; 

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)] static extern IntPtr LoadLibrary(string dllPath);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)] static extern bool FreeLibrary(IntPtr hDll);
        [DllImport("kernel32.dll")] static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        // Data Access
        //[DllImport("LabelDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "_LabelImage@16")]
        //extern static public
        //    void LabelImage(int width, int height, int[] input, int[] output);
            //unsafe extern static public
            //    void LabelImage(int width, int height, [MarshalAs(UnmanagedType.LPArray)] int[] input, int[] output); 
        
        [DllImport("TsaiDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "TsaiDll_calibrate")]
        extern static public 
            int TsaiDll_calibrate(string fname, int sensor_width_pix, int sensor_hight_pix, float sensor_microns_pix,
            float x1, float y1, float xim1, float yim1, float x2, float y2, float xim2, float yim2,
            float x3, float y3, float xim3, float yim3, float x4, float y4, float xim4, float yim4);
        
        [DllImport("TsaiDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "TsaiDll_load_calibration")]
        extern static public
            int TsaiDll_load_calibration(string fname, int sensor_width_pix, int sensor_hight_pix, Single sensor_microns_pix);
        

        [DllImport("TsaiDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "TsaiDll_image_to_world")]
        extern static public
            int TsaiDll_image_to_world(Single xim, Single yim, ref Single xworld, ref Single yworld);

        [DllImport("TsaiDll.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, EntryPoint = "TsaiDll_world_to_image")]
        extern static public
            int TsaiDll_world_to_image(Single xworld, Single yworld, ref Single xim, ref Single yim);
        #endregion

        internal static int TsaiDll_open(string aPath) //static
        {
            hTsaiDll = LoadLibrary(aPath + "\\TsaiDll.dll"); // explicitly link to TsaiDll.dll 
            if (hTsaiDll == IntPtr.Zero) {
                MessageBox.Show("TsaiDll.dll not loaded", "TsaiDll_open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }

            hTsaiCalibrate = GetProcAddress(hTsaiDll, "TsaiDll_calibrate");
            if (hTsaiCalibrate == IntPtr.Zero) {
                MessageBox.Show("hTsaiCalibrate (from TsaiDll.dll) not loaded", "TsaiDll_open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            fTsaiCalibrate = (DelegateTsaiCalibrate)Marshal.GetDelegateForFunctionPointer(hTsaiCalibrate, typeof(DelegateTsaiCalibrate));

            hTsaiLoadCalibration = GetProcAddress(hTsaiDll, "TsaiDll_load_calibration");
            if (hTsaiLoadCalibration == IntPtr.Zero) {
                MessageBox.Show("hTsaiLoadCalibration (from TsaiDll.dll) not loaded", "TsaiDll_open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            fTsaiLoadCalibration = (DelegateTsaiLoadCalibration)Marshal.GetDelegateForFunctionPointer(hTsaiLoadCalibration, typeof(DelegateTsaiLoadCalibration));

            hTsaiImageToWorld = GetProcAddress(hTsaiDll, "TsaiDll_image_to_world");
            if (hTsaiImageToWorld == IntPtr.Zero) {
                MessageBox.Show("hTsaiImageToWorld (from TsaiDll.dll) not loaded", "TsaiDll_open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            fTsaiImageToWorld = (DelegateTsaiImageToWorld)Marshal.GetDelegateForFunctionPointer(hTsaiImageToWorld, typeof(DelegateTsaiImageToWorld));

            hTsaiWorldToImage = GetProcAddress(hTsaiDll, "TsaiDll_world_to_image");
            if (hTsaiWorldToImage == IntPtr.Zero) {
                MessageBox.Show("hTsaiWorldToImage (from TsaiDll.dll) not loaded", "TsaiDll_open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
            fTsaiWorldToImage = (DelegateTsaiWorldToImage)Marshal.GetDelegateForFunctionPointer(hTsaiWorldToImage, typeof(DelegateTsaiWorldToImage));


            //TsaiDll_calibrate @1
	        //TsaiDll_load_calibration @2
	        //TsaiDll_image_to_world @3
	        //TsaiDll_world_to_image @4

            //hLabelImage1 = GetProcAddress(hLabelDll, "_LabelImage1@12"); //"LabelImage1"
            //if (hLabelImage1 == IntPtr.Zero)
            //{
            //    MessageBox.Show("LabelImage1 (from LabelDll.dll) not loaded", "LabelDll_open", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return -1;
            //}
            //fLabelImage1 = (DelegateLabelImage1)Marshal.GetDelegateForFunctionPointer(hLabelImage1, typeof(DelegateLabelImage1));
            return 0;
        }

        internal static void TsaiDll_close() 
        {
            FreeLibrary(hTsaiDll);
        }

    }
}
