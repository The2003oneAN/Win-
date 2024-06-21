using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Text;
using System.ServiceProcess;
using System.ServiceProcess.Design;
using Microsoft.Win32;
using System.Web.UI.Design.WebControls;

namespace tsk_7
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// we will use this in back ground so we use timer so that it doesnt get freezed
        /// </summary>

        //we can use system timer or form timer but form might cause some problems or freeze
        //using system.Threading.timers using thread pools and doesnt use a ui whereas timers.timer is suitable for ui
        private System.Timers.Timer _timer;
        private System.Timers.Timer t;

        private DataTable data = new DataTable();
        private DataTable data2 = new DataTable();
        public Form1()
        {
            InitializeComponent();
            _timer = new System.Timers.Timer();
            _timer.Interval = 1000;// will be ellapsed per 1 sec
            _timer.Elapsed += OntimedEvent;
            _timer.AutoReset = true;

            t = new System.Timers.Timer();
            t.Interval = 1000;
            t.Elapsed += ter;
            t.Start();
            t.AutoReset = true;

            //guna2DataGridView2.DataSource = data2;
            guna2DataGridView1.DataSource = data;//we bind the datasource object as teh data source but first we create tye data source object
            //t1 = new System.Timers.Timer();
            //t1.Interval = 2000;
            //t1.Elapsed += ter;
            //t1.Start();

        }

        private void TerminateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        Process[] pro;
        private void Form1_Load(object sender, EventArgs e)
        {
            listall();
            //history();
            totalram();
            //eth();
            eh();
            services();
            start();
            //t1.Enabled = true;
            _timer.Enabled = true;
        }
        private void totalram()
        {
            //available
            var rcounter = new PerformanceCounter("Memory", "Available Bytes").RawValue;
            rcounter = rcounter / (1024 * 1024 * 1024);

            //use
            int ur = Process.GetCurrentProcess().PeakWorkingSet;
            ur = ur / (1024 * 1024) / 10;

            long tot = rcounter + ur;
            label4.Text = tot.ToString();
        }

        //private void history()
        //{
            //dh();
           //backgroundWorker2.RunWorkerAsync();
        //}
        private void services()
        {
            ServiceController[] ser = ServiceController.GetServices(); ;
            foreach (ServiceController service in ser)
            {
                if (service.Status == ServiceControllerStatus.Running)
                {
                    guna2DataGridView3.Rows.Add(service.ServiceName, service.DisplayName, service.Status.ToString());
                }
            }
        }
        private void listall()
        {
            dt();
            //eth();
            backgroundWorker1.RunWorkerAsync();

            string name = Environment.UserName;
            label41.Text = name;

            string machine = Environment.MachineName;
            label39.Text = machine;

            
        }
        

        private void Data(object sender, System.Timers.ElapsedEventArgs e)
        {
            //listall();    
        }
        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void tabPage3_Click(object sender, EventArgs e)
        {

        }
        //counters
        private void OntimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                int mvalue = getmemory();
                int cvalue = getcpu();
                int dsvalue = getdisk();
                //int dgvalue = dgpu();
                //int da = Data();
                //though it is not roper but the ivoking caused some real run time problem 
                CheckForIllegalCrossThreadCalls = false;//we use this since the threads are appended mutiple times and they will not update in the background so we cannot acces from mian thread to another thread it will cause thread acces error
                                                        //memory
                                                        //if (guna2ProgressBar1.InvokeRequired)
                {
                    //guna2ProgressBar1.Invoke(new Action(() => guna2ProgressBar1.Value = getmemory()));
                }
                //else
                {
                    //guna2ProgressBar1.Value = mvalue;
                }
                //m label
                //to show the percentage usuage in lable we implement invoke
                if (label3.InvokeRequired)
                {
                    label3.Invoke(new Action(() => label3.Text = mvalue.ToString() + "%"));
                }
                else
                {
                    label3.Text = mvalue.ToString() + " %";
                }


                //cpu
                //if (guna2ProgressBar2.InvokeRequired)
                {
                    //guna2ProgressBar2.Invoke(new Action(() => guna2ProgressBar2.Value = getcpu()));
                }
                // else
                {
                    //guna2ProgressBar2.Value = cvalue;
                }
                //c label
                //to show the percentage usuage in lable we implement invoke
                if (label9.InvokeRequired)
                {
                    label9.Invoke(new Action(() => label9.Text = cvalue.ToString() + "%"));
                }
                else
                {
                    label9.Text = cvalue.ToString() + " %";
                }
                guna2ProgressBar1.Value = mvalue;//it will show an error since its a non static and a object is required since this event is decalred as static
                                                 //so remove static modifier from evry method

                guna2ProgressBar2.Value = cvalue;

                guna2ProgressBar4.Value = dsvalue;

                ///Ram
                //graph
                var ch = new PerformanceCounter("Memory", "% Committed Bytes in Use");
                double usuage = ch.NextValue();
                chart1.Series["MEMORY"].Points.AddY(usuage);
                //remove old data points
                while (chart1.Series["MEMORY"].Points.Count > 50)
                {
                    chart1.Series["MEMORY"].Points.RemoveAt(0);
                }
                /////

                //progressBar1.Value = dgvalue;

                //dlabel
                if (label12.InvokeRequired)
                {
                    label12.Invoke(new Action(() => label12.Text = dsvalue.ToString() + "%"));
                }
                else
                {
                    label12.Text = dsvalue.ToString() + "%";
                }
                //////////////////////////////////

                //cpu info
                //logical core
                int c = Environment.ProcessorCount;
                label14.Text = c.ToString() + " ";

                //number of cores
                int cn = 0;
                if (c > 0)
                {
                    cn = c / 2;
                }
                label13.Text = cn.ToString() + " ";
                //try
                //{
                //foreach (var i in new System.Management.ManagementObjectSearcher("Select * from Win32_Processor").Get())
                //{
                //cnt += int.Parse(i["Cores"].ToString());
                //  }

                //}
                //catch (Exception ex)
                //{

                //}
                //finally
                //{
                // label13.Text = cn.ToString() + " ";
                //}
                //speed
                var mov = cvalue;
                var mo = new ManagementObject("Win32_Processor.DeviceID='CPU0'");
                var speed = (uint)(mo["CurrentClockSpeed"]);
                mo.Dispose();
                label11.Text = speed.ToString() + " ";

                //base
                //var mov = new ManagementObject("Win32_Processor.DeviceID='CPU0'");
                //var spee = (uint)(mov["MaxClockSpeed"]);
                //mov.Dispose();
                //label12.Text = spee.ToString() + " ";


                //cpu name
                var cp = new ManagementObject("Win32_Processor.DeviceID='CPU0'");
                var n = (cp["Name"]);
                label10.Text = n.ToString() + " ";

                //graph
                var cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                double use = cpu.NextValue();
                chart2.Series["CPU"].Points.AddY(use);
                while (chart2.Series["CPU"].Points.Count > 50)
                {
                    chart2.Series["CPU"].Points.RemoveAt(0);
                }
                //////////////////////////////////////////

                //memory info
                //var me = new ManagementObject("Win32_TotalPhysicalMemory.DeviceID='RAM0'");
                //var m = Environment.WorkingSet; //usage
                //var m = new ManagementObject("Win32_OperatingSystem");
                //var n1 = (m["TotalVisibleMemorySize"]);
                //label4.Text = n1.ToString();

                /////////////////////////////////

                //disc
                //local
                DriveInfo[] d = DriveInfo.GetDrives();
                var d1 = " ";
                for (int i = 0; i < d.Count(); i++)
                {
                    d1 = d[i].Name;
                }
                label21.Text = d1.ToString();

                ///graph
                ///
                var disk = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
                double age = disk.NextValue();
                chart3.Series["Disk"].Points.AddY(age);
                while (chart3.Series["Disk"].Points.Count > 50)
                {
                    chart3.Series["Disk"].Points.RemoveAt(0);
                }

                //storage size
                ulong totalsize = 0;
                ManagementObjectSearcher s = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject g in s.Get())
                {
                    ulong size = (ulong)g["Size"];
                    totalsize += size / (1024 * 1024 * 1024);
                }
                label23.Text = totalsize.ToString() + " ";

                //storage type
                ManagementObjectSearcher type = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                string t = " ";
                foreach (ManagementObject g in type.Get())
                {
                    string search = (string)g["MediaType"];
                    if (search == "Solid State Disk")
                    {
                        search = "SSD";
                    }
                    else if (search == "Fixed hard disk media")
                    {
                        search = "HDD";
                    }
                    else
                    {
                        search = "Cannot be determined";
                    }
                    t += search;
                }
                label24.Text = t.ToString();

                //storage name
                ManagementObjectSearcher name = new ManagementObjectSearcher("SELECt * FROM Win32_DiskDrive");
                var na = " ";
                foreach (ManagementObject g in name.Get())
                {
                    var search = (string)g["Manufacturer"];
                    na += search;
                }
                label22.Text = na.ToString() + " ";

                //write speeed
                int wr = wrspeed();
                double wd = wr / 1000;
                label28.Text = wd.ToString() + " ";
                int wg = write();
                guna2CircleProgressBar1.Value = wg;

                //read spped
                int re = rspeed();
                double re2 = re / 1000;
                label29.Text = re2.ToString();
                int r = read();
                guna2CircleProgressBar2.Value = r;


                //////////////////////////////////////////////////////
                //gpu name
                ManagementObjectSearcher gn = new ManagementObjectSearcher("Select * from Win32_VideoController");
                var gn2 = " ";
                foreach (ManagementObject gn1 in gn.Get())
                {
                    var search = (string)gn1["VideoProcessor"];
                    gn2 += search;
                }
                label42.Text = gn2.ToString();

                //vram
                ManagementObjectSearcher vr = new ManagementObjectSearcher("Select * from Win32_VideoController");
                string v2 = " ";
                foreach (ManagementObject v in vr.Get())
                {
                    var search = v["AdapterRam"];
                    v2 += search; /// (1024 * 1024 * 1024);
                }
                double v3 = double.Parse(v2);
                double v4 = v3 / 104876;//number of kb in gb
                double v5 = v4 / 100;
                label33.Text = v5.ToString();

                //driver version
                ManagementObjectSearcher ver = new ManagementObjectSearcher("Select * from Win32_VideoController");
                string ver1 = " ";
                foreach (ManagementObject v in ver.Get())
                {
                    var search = v["DriverVersion"];
                    ver1 += search;
                }
                label35.Text = ver1.ToString();

                /////////////////

                /////ram
                //speed
                ManagementObjectSearcher rs = new ManagementObjectSearcher("SELECT Speed FROM Win32_PhysicalMemory");
                string rs1 = " ";
                foreach (ManagementObject ob in rs.Get())
                {
                    int rm = Convert.ToInt32(ob["Speed"]);
                    rm = rm / 100;
                    rs1 += rm;
                }
                label57.Text = rs1.ToString();

                //available
                var rcounter = new PerformanceCounter("Memory", "Available Bytes").RawValue;
                rcounter = rcounter / (1024 * 1024 * 1024);
                label59.Text = rcounter.ToString();
                //use
                int ur = Process.GetCurrentProcess().PeakWorkingSet;
                ur = ur / (1024 * 1024) / 10;
                label61.Text = ur.ToString();

                //
            }
            catch (Exception ex)
            {
                
            }
        }
        //ethernet throughput
       private static NetworkInterface eth()
       {
            // var br = new PerformanceCounter("Network Interface",
            //"Bytes Received/sec",);
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties cp = IPGlobalProperties.GetIPGlobalProperties();
            ///received
            foreach (NetworkInterface adapter in interfaces)
            {
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    return adapter;
                }
            }
            return null;
        }
        private void ter(object sender, System.Timers.ElapsedEventArgs e)
        {
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties cp = IPGlobalProperties.GetIPGlobalProperties();
            //ethernet
            //sent received
            foreach (NetworkInterface intf in interfaces)
            {
                if (intf.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    try
                    {
                        if (intf.OperationalStatus == OperationalStatus.Up)
                        {
                            IPv4InterfaceStatistics sat = intf.GetIPv4Statistics();

                            guna2ProgressBar6.Minimum = 0;
                            guna2ProgressBar6.Maximum = 100;

                            long oldb = 0;
                            long bytesr = sat.BytesReceived;
                            long bytesrpersec = intf.GetIPStatistics().BytesReceived;
                            long currbytespersec = bytesrpersec - oldb;
                            currbytespersec = currbytespersec / 1000000;
                            label43.Text = currbytespersec.ToString();
                            guna2CircleProgressBar7.Value = (int)currbytespersec / 1000;
                            oldb = currbytespersec;

                            long db = 0;
                            long bytese = sat.BytesSent;
                            long bytessepersec = intf.GetIPStatistics().BytesSent;
                            long curbytessepersec = bytesrpersec - db;
                            db = curbytessepersec;
                            curbytessepersec = curbytessepersec / 1000000;
                            label44.Text = bytessepersec.ToString();
                            guna2CircleProgressBar8.Value = (int)bytessepersec / 1000;


                            long sum = curbytessepersec + currbytespersec;

                            //int pro = (int)sum / 100000;
                            guna2ProgressBar6.Value = (int)sum;
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }

            //wifi
            //sent received
            foreach (NetworkInterface intf in interfaces)
            {
                if(intf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    if (intf.OperationalStatus == OperationalStatus.Up)
                    {
                        IPv4InterfaceStatistics sat = intf.GetIPv4Statistics();

                        long bytesr = sat.BytesReceived;
                        long bytesrpersec = intf.GetIPStatistics().BytesReceived;

                        label50.Text = bytesrpersec.ToString();
                        guna2CircleProgressBar9.Value = (int)bytesrpersec / 10000;

                        long bytese = sat.BytesSent;
                        long bytessepersec = intf.GetIPStatistics().BytesSent;
                        label63.Text = bytessepersec.ToString();
                        guna2CircleProgressBar10.Value = (int)bytessepersec / 10000;

                        long sum = bytesrpersec + bytessepersec;
                        guna2ProgressBar7.Value = (int)sum;

                    }
                }
            }

        }
        private void eh()
        {
            NetworkInterface[] inter   = NetworkInterface.GetAllNetworkInterfaces();
            //ipv4
            foreach (NetworkInterface intf in inter) 
            {
                if(intf.NetworkInterfaceType == NetworkInterfaceType.Ethernet)  
                {
                    if (intf.OperationalStatus == OperationalStatus.Up)
                    {
                        IPInterfaceProperties prop = intf.GetIPProperties();
                        foreach (UnicastIPAddressInformation add in prop.UnicastAddresses)
                        {
                            if (add.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                label47.Text = add.Address.ToString();
                            }
                        }
                    }
                }
            }
            //ipv6
            foreach (NetworkInterface intf in inter)
            {
                if (intf.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    if (intf.OperationalStatus == OperationalStatus.Up)
                    {
                        IPInterfaceProperties prop = intf.GetIPProperties();
                        foreach (UnicastIPAddressInformation add in prop.UnicastAddresses)
                        {
                            if (add.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                            {
                                label48.Text = add.Address.ToString();
                            }
                        }
                    }
                }
            }

            //wifi
            //ipv4
            foreach (NetworkInterface intf in inter)
            {
                if (intf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    if (intf.OperationalStatus == OperationalStatus.Up)
                    {
                        IPInterfaceProperties prop = intf.GetIPProperties();
                        foreach (UnicastIPAddressInformation add in prop.UnicastAddresses)
                        {
                            if (add.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                label65.Text = add.Address.ToString();
                            }
                        }
                    }
                }
            }
            //ipv6
            foreach (NetworkInterface intf in inter)
            {
                if (intf.NetworkInterfaceType == NetworkInterfaceType.Wireless80211)
                {
                    if (intf.OperationalStatus == OperationalStatus.Up)
                    {
                        IPInterfaceProperties prop = intf.GetIPProperties();
                        foreach (UnicastIPAddressInformation add in prop.UnicastAddresses)
                        {
                            if (add.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                            {
                                label67.Text = add.Address.ToString();
                            }
                        }
                    }
                }
            }
        }

        //memory    
        private int getmemory()
        {
            // no need for timer here or no ned to use instance variable since memory is a whole object
            var mcounter = new PerformanceCounter("Memory", "% Committed Bytes in Use");//we caste to int since next value will return float value we can make it float but int is easier to understand
            int returnvalue = (int)mcounter.NextValue();
            return returnvalue;
        }
        //ram available
     
        private int getcpu()
        {
            //the three parameters first is the class i.e. processor 2nd is counter and the last is the instance or count so total counts all the cores in the pc not a single individual or specific
            var Cpcounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");//the performancecounter is in built object so the .net frame work can get the counts of performance of a pc
            Cpcounter.NextValue();//the next value will read the value so after a sleep of 1 sec it will again read the other value and return it in return object
            System.Threading.Thread.Sleep(1000);
            int returnvalue = (int)Cpcounter.NextValue();//we use next value once more since counter,one will calculate it within on sec
            return returnvalue;
        }
        private int getdisk()
        {
            var dscounter = new PerformanceCounter("PhysicalDisk", "% Disk Time", "_Total");
            dscounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            int returnvalue = (int)dscounter.NextValue();
            return returnvalue;
        }

        //write spped
        private int write()
        {
            var wr = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            wr.NextValue();
            System.Threading.Thread.Sleep(1000);    
            int re = (int)wr.NextValue();
            return re;
        }
        private int wrspeed()
        {
            var wrcounter = new PerformanceCounter("PhysicalDisk", "Disk Write Bytes/sec", "_Total");
            wrcounter.NextValue();
            System.Threading.Thread.Sleep(1000);
            int returnvalue = (int)wrcounter.NextValue();
            return returnvalue;
        }

        //read speed
        private int read()
        {
            var re = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            re.NextValue();
            System.Threading.Thread.Sleep(1000);
            int rs = (int)re.NextValue();
            return rs;  
        }
        private int rspeed()
        {
            var wr = new PerformanceCounter("PhysicalDisk", "Disk Read Bytes/sec", "_Total");
            wr.NextValue();
            System.Threading.Thread.Sleep(1000);
            int r = (int)wr.NextValue();
            return r;
        }

        //gpu
       // private int dgpu()
       // {
            //var gp = new PerformanceCounter("GPU Engine", "Utilization Percentage", "_Total");
            //System.Threading.Thread.Sleep(1000);
            //int r = (int)gp.RawValue;
            //return r;
        //}

       // private int Data()
        //{
            //var rc = new PerformanceCounter("MyApp", "DataGridViewRowCount", true);
            //rc.NextValue();
            //System.Threading.Thread.Sleep(1000);
            //int re = (int)rc.NextValue();
            //return re;
        //}
        private void tabPage4_Click(object sender, EventArgs e)
        {

        }


        //menu strips
        //run process
        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (Run_process frm = new Run_process())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    listall();
                }
            }
        }

      
        private void terminateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if(guna2DataGridView1.SelectedRows.Count > 0)
            {
                string name = guna2DataGridView1.SelectedRows[0].Cells["Process"].Value.ToString();
                pro = Process.GetProcessesByName(name);
                foreach (Process pr in pro)
                {
                    pr.Kill();
                }
                MessageBox.Show("Process Killed");
            }
        }
        private void priorityToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    string name = guna2DataGridView1.SelectedRows[0].Cells["Process"].Value.ToString();
                    pro = Process.GetProcessesByName(name);
                    foreach (Process pr in pro)
                    {
                        pr.PriorityClass = ProcessPriorityClass.AboveNormal;
                    }
                }
                catch (Exception ex)
                {
                    
                }
                MessageBox.Show("Increased");
            }
        }
        private void datagridview1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
               int currow = guna2DataGridView1.HitTest(e.X, e.Y).RowIndex;
                if (currow >= 0)
                {
                    guna2DataGridView1.ClearSelection();
                    guna2DataGridView1.Rows[currow].Selected = true;
                    contextMenuStrip1.Show(guna2DataGridView1, new Point(e.X, e.Y));
                }
            }
        }
        private void label9_Click(object sender, EventArgs e)
        {

        }
        private void label14_Click(object sender, EventArgs e)
        {
            
        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
         
        }
        
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
            if (guna2DataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow r = guna2DataGridView1.SelectedRows[0];
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                data = dt();
                backgroundWorker1.ReportProgress(0);
                Thread.Sleep(5000);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                guna2DataGridView1.DataSource = data;
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
             
        }
        private void backgroundWorker2_DoWork(object Sender, DoWorkEventArgs e)
        {
           // while (true)
            //{
               //data2 = dh();
               //backgroundWorker2.ReportProgress(0);
               //Thread.Sleep(5000);
            //}
        }
        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //try
            //{
                //guna2DataGridView2.DataSource = data2;
            //}
            //catch (Exception ex)
            //{
                //MessageBox.Show(ex.Message);
            //}
        }
        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
        private void start()
        {
            try
            {
                RegistryKey runKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run");

                foreach (var ap in runKey.GetValueNames())
                {
                    var path = runKey.GetValue(ap).ToString();
                    guna2DataGridView4.Rows.Add(ap, path);
                }
            }
            catch (Exception ex)
            {

            }
        }
        //private DataTable dh()
        //{
            //pro = Process.GetProcesses();
            //data2 = new DataTable();
            //data2.Columns.Add("Process");
            //data2.Columns.Add("CPU Time");
            //data2.Columns.Add("Time of Use");
            //foreach (Process p1 in pro)
            //{
            //try
            //{
                //DataRow dr = data2.NewRow();
                //dr["Process"] = (string)p1.ProcessName;
                //dr["CPU Time"] = (string)p1.StartTime.ToShortTimeString();
                //dr["Time of Use"] = (string)p1.StartTime.ToShortDateString();
               //data2.Rows.Add(dr);
            //}
            //catch(Exception ex)
            //{
                
            //}
        //}
            //return data2;
        //}
        private DataTable dt()//datasource object
        {
            pro = Process.GetProcesses();
            data = new DataTable();
            data.Columns.Add("Process");
            data.Columns.Add("Process ID");
            data.Columns.Add("Process Priority");
            
            data.Columns.Add("Memory(MB)");
            data.Columns.Add("CPU Time");
            data.Columns.Add("Time of Use");

            foreach (Process p1 in pro)
            {
                try
                {
                    DataRow dr = data.NewRow();
                    dr["Process"] = (string)p1.ProcessName;
                    dr["Process ID"] = (int)p1.Id;
                    dr["Process Priority"] = (int)p1.PriorityClass;
                    
                    dr["Memory(MB)"] = (long)p1.PrivateMemorySize64 / 1024 / 1024;
                    dr["CPU Time"] = (string)p1.StartTime.ToShortTimeString();
                    dr["Time of Use"] = (string)p1.StartTime.ToShortDateString();
                    data.Rows.Add(dr);
                }
                catch (Exception ex)
                {

                }
            }
            return data;
        }

        private void guna2CircleProgressBar10_ValueChanged(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabPage10_Click(object sender, EventArgs e)
        {

        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
