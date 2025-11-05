using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Net.Sockets;
using System.Net;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;





namespace EndmillHMI
{


    public class RobotFunctions
    {
                
       
       
        
        delegate void SetButtonCallback(Boolean en, Button btn, Form frm);
        //public int FunctionCode;
        public string status = "";
        public ListBox lstSend;

        //public TextBox txtstate, textwait;
        //public TextBox text11, text12,text13,text14,text15,text16;
        
        public Form frm;
        
        //public ProgressBar progressbar2;
        public Boolean bExitcycle=false;
        public string RobotCmd = "";
        public string RobotReport = "";
        public Single[] ParmGet = new Single[30];
        Stopwatch stopwatch = new Stopwatch();
        
       
        public string RobotName="" ;
        public string RobotProgram = "";
        public Boolean bPortReading = false;
        public DataIniFile dFile = new DataIniFile();
       
        public struct SendRobotParms //====2013
        {
            public string comment;
            public Single[] SendParm;
            public bool NotSendMess;
            public string cmd;
            public int FunctionCode;
            public Single timeout;
            public int DebugTime;
            //public int FunctionCode;
        }
        public struct SendHostParms //====2013
        {
            public string comment;
            public string[] cmd;
            public float timeout;
            
        }
        public struct SendPlcParms //====2013
        {
            public string comment;
            public Single[] SendParm;
            public int FunctionCode;
        }
        public struct CommReply
        {
            public bool result;
            public float[] data;
            public string status;
            public string comment;//====2013
            public int FunctionCode;
            public string Error;
        }
        public struct HostReply
        {
            public bool result;
            public string reply;
            public string cmd;
            public string[] data;
            public string status;
            public string comment;//====2013
            public string error;
            public HttpListenerContext contex;

        }
        //private  Socket m_socClient; //= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

        //public string ClientStatus;
        public byte[] mb_message = new byte[8];
        public byte[] mb_response = new byte[8];
        public string sRobotSnd = "";
        public string sRobotGt = "";
        public delegate void lst();
        public Boolean bReadPort = false;
        //public string sTempPort;
        public Boolean WaitResult = false;
        public Stopwatch StopWatch1 = new Stopwatch();
        public Stopwatch StopWatch2 = new Stopwatch();
        public struct position
        {
            public Double x;
            public Double y;
            public Double z;
            public Double r;
            public Double corrX;
            public Double corrY;
            public Double corrZ;

            public int Rotate;
            public int EndOfTray;
            public string Error;
        }
        //static rs232 RobotPort = new rs232();
       
       // CancellationTokenSource tokenSource = new CancellationTokenSource();
       // public BackgroundWorker bw1 = new BackgroundWorker();
       
        
       
        public TextBox txtstate;
        
        //Plc PLC = new Plc();
        
       
        

        #region ---------event to Form--------------

        public  string Robot_buffer="";
        SendRobotParms Parms0 = new SendRobotParms();
        
             
        
        
        #endregion

        public void SetControls(ListBox LstSend,  Form Frm)
        {
            lstSend = LstSend;
            //txtstate = Txtstate;
            frm = Frm;
           
        }

        #region Robot CMD
        //FanucWeb FW = new FanucWeb();
        
        //public async Task<RobotFunctions.CommReply> RunCmdFanuc(SendRobotParms Parms,bool debug=false)
        ////send cmd to robot
        //{

        //    //SendRobotParms Parms = new SendRobotParms();
        //    //RobotFunctions.CommReply CommReply = new RobotFunctions.CommReply();
        //    //RobotFunctions.position outpos = new RobotFunctions.position();
        //    //string Error = "";
        //    //int parm = 0;
        //    int cmd = int.Parse(Parms.cmd);
        //    frmMain.newFrmMain.ControlsEnable(false);
        //    switch (cmd)
        //    {
                
        //        case (int)MyStatic.RobotCmd.MoveMaint://-------robot move maint
        //            Parms.FunctionCode = (int)MyStatic.RobotCmd.MoveMaint;
        //            Parms.comment = "Move maintenance";
        //            Parms.timeout = 60;
        //            Array.Resize<Single>(ref Parms.SendParm, 3);
        //            Parms.SendParm[0] = cmd;//16
        //            Parms.SendParm[1] = (Single)frmMain.newFrmMain.FanucSpeed;// general speed
        //            Parms.SendParm[2] = 60;// 0.5f;//timeout
        //            break;
        //        case (int)MyStatic.RobotCmd.PickTray://-------robot move maint
        //            Parms.FunctionCode = (int)MyStatic.RobotCmd.PickTray;
        //            Parms.comment = "Pick Tray";
        //            Parms.timeout = 60;
        //            Array.Resize<Single>(ref Parms.SendParm, 3);
        //            Parms.SendParm[0] = cmd;//16
        //            Parms.SendParm[1] = (Single)frmMain.newFrmMain.FanucSpeed;// general speed
        //            Parms.SendParm[2] = 60;// 0.5f;//timeout
        //            break;
        //        case (int)MyStatic.RobotCmd.PlacePartSumo://-------robot move maint
        //            Parms.FunctionCode = (int)MyStatic.RobotCmd.PlacePartSumo;
        //            Parms.comment = "Place Part Sumo";
        //            Parms.timeout = 60;
        //            Array.Resize<Single>(ref Parms.SendParm, 3);
        //            Parms.SendParm[0] = cmd;//16
        //            Parms.SendParm[1] = (Single)frmMain.newFrmMain.FanucSpeed;// general speed
        //            Parms.SendParm[2] = 60;// 0.5f;//timeout
        //            break;
        //        case (int)MyStatic.RobotCmd.PickPartSumo://-------robot move maint
        //            Parms.FunctionCode = (int)MyStatic.RobotCmd.PickPartSumo;
        //            Parms.comment = "Pick Part Sumo";
        //            Parms.timeout = 60;
        //            Array.Resize<Single>(ref Parms.SendParm, 3);
        //            Parms.SendParm[0] = cmd;//16
        //            Parms.SendParm[1] = (Single)frmMain.newFrmMain.FanucSpeed;// general speed
        //            Parms.SendParm[2] = 60;// 0.5f;//timeout
        //            break;
        //        case (int)MyStatic.RobotCmd.PlaceTray://-------robot move maint
        //            Parms.FunctionCode = (int)MyStatic.RobotCmd.PlaceTray;
        //            Parms.comment = "Place Tray";
        //            Parms.timeout = 60;
        //            Array.Resize<Single>(ref Parms.SendParm, 3);
        //            Parms.SendParm[0] = cmd;//16
        //            Parms.SendParm[1] = (Single)frmMain.newFrmMain.FanucSpeed;// general speed
        //            Parms.SendParm[2] = 60;// 0.5f;//timeout
        //            break;
        //        //all commands
        //        default:
        //            dFile.WriteLogFile("ERROR:  undefined message from robot ");
        //            MessageBox.Show("ERROR:  undefined message from robot ", "ERROR", MessageBoxButtons.OK,
        //            MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        //            frmMain.newFrmMain.ControlsEnable(true);
        //            break;
        //    }

        //    //send message
        //    var ctss = new CancellationTokenSource();
        //    int timeout = (int)(Parms.timeout * 1000);
        //    ctss.CancelAfter(timeout);
        //    string ErrMessage = "";
        //    RobotFunctions.CommReply reply = new RobotFunctions.CommReply();

        //    MyStatic.TaskExecute = true;
        //    if (debug)
        //    {
        //        Thread.Sleep(1000);
        //        reply.result = true;
        //        return reply;
                
        //    }
        //    else
        //    {
        //        try
        //        {
        //            reply.result = false;
        //            string reg = "strreg";
        //            string num = "1";
        //            string val = "";
        //            for (int i = 0; i < Parms.SendParm.Length; i++)
        //            {
        //                val = val + Parms.SendParm[i].ToString() + ",";
        //            }
        //            val = "cmd" + val + "end";
        //            // string val = "cmd10,5,6,7,end";//robot command
        //            var task = FW.WriteFanucRegAsync(reg, num, val, 1000);
        //            await task;
        //            reply = task.Result;
        //            if (!reply.result) { return reply; }

        //            //wait result
        //            var task1 = Task.Run(() => FW.ReadFanucS2RegAsync(10000));
        //            await task1;
        //            reply = task1.Result;
        //            if (reply.data[0] != cmd) reply.result = false;
        //            return reply;
        //        }
        //        catch (Exception ex)
        //        {
        //            reply.result = false;
        //            return reply;

        //        }
        //    }
        //    return reply;
        //    //return (CommReply);
           


        //}
        
            #endregion
          

       
        

      
        
        #region -----------------USER PORT Procedures----------------
        
       
        
        public Boolean InArray(int[] arr, int cmd)
        {            
            int first=  Array.IndexOf<int>(arr, cmd);
            if (first >= 0) return true; else return false;
        }
       
      
        #endregion

        /// <summary>
        /// ////////////////////////////////////////////////////////////////

        /// ////////////////////////////////////////////////////////////////


        #region ---------Delegate Procedures--------------
        delegate void SetTextBox(string text, TextBox txt, Form frm);
        public void SetTxtText(string text, TextBox txt, Form frm)
        {
            if ((txt == null) || (frm == null)) { return; }
            try
            {
                if (txt.InvokeRequired)
                {
                    SetTextBox d = new SetTextBox(SetTxtText);
                    frm.Invoke(d, new object[] { text, txt, frm });
                }
                else
                {
                    txt.Text = text;
                }
            }
            catch { }

        }
        delegate void SetListText(string text, ListBox lst, Form frm);
        public void SetTextLst(string text, ListBox lst, Form frm)
        {
            //if (!Enable) { return; }
            if ((lst == null) || (frm == null)) { return; }

            try
            {
                if (lst.InvokeRequired)
                {
                    SetListText d = new SetListText(SetTextLst);
                    frm.Invoke(d, new object[] { text, lst, frm });
                }
                else
                {
                    lst.Items.Add(text);
                    if (lst.Items.Count > 100) { lst.Items.Clear(); }
                }
            }
            catch { }

        }

        delegate void SetProgressBar(int val, ProgressBar progressbar, Form frm);
        private void SetProgress(int val, ProgressBar progressbar, Form frm)
        {
            if ((progressbar == null) || (frm == null)) { return; }
            try
            {
                if (progressbar.InvokeRequired)
                {
                    SetProgressBar d = new SetProgressBar(SetProgress);
                    frm.Invoke(d, new object[] { val, progressbar, frm });
                }
                else
                {
                    progressbar.Value = val;
                }
            }
            catch {   Debug.Print("63"); }


        }
        #endregion


           

       

        
       
        
        private  String response = String.Empty;

        
       
        /////////////////////////////////////////////

    
     }  //
}
