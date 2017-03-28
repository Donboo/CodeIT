using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using ScintillaNET;

namespace CodeIT
{
    public partial class Form1 : Form
    {
        // VARS
            // HTML Regex
            public Regex htmlRegex = new Regex(@"<\s*\w*\s*\.*?>|<\s*\/\s*\w\s*.*?>|<\s*br\s*>/g");

            // INTEGER VARS
            int startLines = 0;

            // STACK
            Stack<string> undoList = new Stack<string>();

        public Form1()
        {
            InitializeComponent();

            Width = SystemInformation.VirtualScreen.Width;
            Height = SystemInformation.VirtualScreen.Height;

            richTextBox1.Height = SystemInformation.VirtualScreen.Height - 10;
            richTextBox1.Width = SystemInformation.VirtualScreen.Width-30;

            if (Properties.Settings.Default["Language"].ToString() == "")
            {
                string welcomeSkeleton = "Welcome to CodeIT Editor. Choose your language & theme and start editing.";
                richTextBox1.Text = welcomeSkeleton;
            }
            if (Properties.Settings.Default["Language"].ToString() == "HTML")
            {
                string htmlSkeleton = "<html>\n" + Indent(4) + "<head>\n" + Indent(8) + "<title>Basic Skeleton</title>\n" + Indent(4) + "</head>\n" + Indent(4) + "<body>\n" + Indent(8) + "<h1>Hello world!</h1>\n" + Indent(4) + "</body>\n" + "</html>";
                richTextBox1.Text = htmlSkeleton;
            }
            else if (Properties.Settings.Default["Language"].ToString() == "PHP")
            {
                string phpSkeleton = "<?php\n" + Indent(4) + "echo 'Hello world!';\n" + "?>";
                richTextBox1.Text = phpSkeleton;
            }
            else if (Properties.Settings.Default["Language"].ToString() == "PAWN")
            {
                string pawnSkeleton = "// This is a comment// uncomment the line below if you want to write a filterscript//#define FILTERSCRIPT#include <a_samp>#if defined FILTERSCRIPTpublic OnFilterScriptInit(){print('\n--------------------------------------');print(' Blank Filterscript by your name here');print('--------------------------------------\n');return 1;}public OnFilterScriptExit(){return 1;}#elsemain(){print('\n----------------------------------');print(' Blank Gamemode by your name here');print('----------------------------------\n');}#endifpublic OnGameModeInit(){// Dont use these lines if its a filterscriptSetGameModeText('Blank Script');AddPlayerClass(0, 1958.3783, 1343.1572, 15.3746, 269.1425, 0, 0, 0, 0, 0, 0);return 1;}public OnGameModeExit(){return 1;}public OnPlayerRequestClass(playerid, classid){SetPlayerPos(playerid, 1958.3783, 1343.1572, 15.3746);SetPlayerCameraPos(playerid, 1958.3783, 1343.1572, 15.3746);SetPlayerCameraLookAt(playerid, 1958.3783, 1343.1572, 15.3746);return 1;}public OnPlayerConnect(playerid){return 1;}public OnPlayerDisconnect(playerid, reason){return 1;}public OnPlayerSpawn(playerid){return 1;}public OnPlayerDeath(playerid, killerid, reason){return 1;}public OnVehicleSpawn(vehicleid){return 1;}public OnVehicleDeath(vehicleid, killerid){return 1;}public OnPlayerText(playerid, text[]){return 1;}public OnPlayerCommandText(playerid, cmdtext[]){if (strcmp('/mycommand', cmdtext, true, 10) == 0){// Do something herereturn 1;}return 0;}public OnPlayerEnterVehicle(playerid, vehicleid, ispassenger){return 1;}public OnPlayerExitVehicle(playerid, vehicleid){return 1;}public OnPlayerStateChange(playerid, newstate, oldstate){return 1;}public OnPlayerEnterCheckpoint(playerid){return 1;}public OnPlayerLeaveCheckpoint(playerid){return 1;}public OnPlayerEnterRaceCheckpoint(playerid){return 1;}public OnPlayerLeaveRaceCheckpoint(playerid){return 1;}public OnRconCommand(cmd[]){return 1;}public OnPlayerRequestSpawn(playerid){return 1;}public OnObjectMoved(objectid){return 1;}public OnPlayerObjectMoved(playerid, objectid){return 1;}public OnPlayerPickUpPickup(playerid, pickupid){return 1;}public OnVehicleMod(playerid, vehicleid, componentid){return 1;}public OnVehiclePaintjob(playerid, vehicleid, paintjobid){return 1;}public OnVehicleRespray(playerid, vehicleid, color1, color2){return 1;}public OnPlayerSelectedMenuRow(playerid, row){return 1;}public OnPlayerExitedMenu(playerid){return 1;}public OnPlayerInteriorChange(playerid, newinteriorid, oldinteriorid){return 1;}public OnPlayerKeyStateChange(playerid, newkeys, oldkeys){return 1;}public OnRconLoginAttempt(ip[], password[], success){return 1;}public OnPlayerUpdate(playerid){return 1;}public OnPlayerStreamIn(playerid, forplayerid){return 1;}public OnPlayerStreamOut(playerid, forplayerid){return 1;}public OnVehicleStreamIn(vehicleid, forplayerid){return 1;}public OnVehicleStreamOut(vehicleid, forplayerid){return 1;}public OnDialogResponse(playerid, dialogid, response, listitem, inputtext[]){return 1;}public OnPlayerClickPlayer(playerid, clickedplayerid, source){return 1;}";
                richTextBox1.Text = pawnSkeleton;
            }

            richTextBox1.WordWrap = false;
            richTextBox1.ShortcutsEnabled = true;

            startLines = richTextBox1.Lines.Count(); 
            var lineCounter = new StringBuilder();
            for (int i = 1; i <= startLines;  i++)
            {
                lineCounter.Append(richTextBox2.Text);
                lineCounter.Append(i + "\n");
            }
            richTextBox2.Text = lineCounter.ToString();

            ChooseTemplate(Properties.Settings.Default["Theme"].ToString());
            richTextBox1.KeyDown += new KeyEventHandler(richTextBox1_PreviewKeyDown);
        }

        public static string Indent(int count)
        {
            return "".PadLeft(count);
        }

        private string TagHighlight(string theme)
        {
            string returner = "";
            if (Properties.Settings.Default["Theme"].ToString() == "Light")
            {
                returner = "Blue";
            }
            if (Properties.Settings.Default["Theme"].ToString() == "Dark")
            {
                returner = "Red";
            }
            if (Properties.Settings.Default["Theme"].ToString() == "Orange")
            {
                returner = "Orange";
            }
            return returner;
        }

        private string TextHighlight(string theme)
        {
            string returner = "";
            if (Properties.Settings.Default["Theme"].ToString() == "Light")
            {
                returner = "Black";
            }
            if (Properties.Settings.Default["Theme"].ToString() == "Dark")
            {
                returner = "White";
            }
            if (Properties.Settings.Default["Theme"].ToString() == "Orange")
            {
                returner = "White";
            }
            return returner;
        }

        private string ChooseTemplate(string template)
        {
            if (template == "Light")
            {
                // BackColor
                Color backgroundColor = ColorTranslator.FromHtml("#fff");
                richTextBox1.BackColor = backgroundColor;

                Color menuColor = ColorTranslator.FromHtml("#777");
                menuStrip1.BackColor = menuColor;

                // ForeColor
                Color editorColor = ColorTranslator.FromHtml("#242729");
                richTextBox1.ForeColor = editorColor;

                Color menuFont = ColorTranslator.FromHtml("#242729");
                menuStrip1.ForeColor = menuFont;

                Refresh();
            }
            if (template == "Dark")
            {
                // BackColor
                Color backgroundColor = ColorTranslator.FromHtml("#202020");
                richTextBox1.BackColor = backgroundColor;

                Color menuColor = ColorTranslator.FromHtml("#777");
                menuStrip1.BackColor = menuColor;

                // ForeColor
                Color editorColor = ColorTranslator.FromHtml("#fff");
                richTextBox1.ForeColor = editorColor;

                Color menuFont = ColorTranslator.FromHtml("#242729");
                menuStrip1.ForeColor = menuFont;

                Refresh();
            }
            if (template == "Orange") {
                // BackColor
                Color backgroundColor = ColorTranslator.FromHtml("#202020");
                richTextBox1.BackColor = backgroundColor;

                Color menuColor = ColorTranslator.FromHtml("#252525");
                menuStrip1.BackColor = menuColor;

                // ForeColor
                Color editorColor = ColorTranslator.FromHtml("#F48024");
                richTextBox1.ForeColor = editorColor;

                Color menuFont = ColorTranslator.FromHtml("#fff");
                menuStrip1.ForeColor = menuFont;

                Refresh();
            }
            return "";
        }

        public void SaveMyFile()
        {
            SaveFileDialog saveFile1 = new SaveFileDialog();

            if (Properties.Settings.Default["Language"].ToString() == "HTML")
                saveFile1.DefaultExt = "*.html";

            if (Properties.Settings.Default["Language"].ToString() == "PHP")
                saveFile1.DefaultExt = "*.pawn";

            if (Properties.Settings.Default["Language"].ToString() == "PAWN")
                saveFile1.DefaultExt = "*.pwn";

            if (Properties.Settings.Default["Language"].ToString() == "JAVASCRIPT")
                saveFile1.DefaultExt = "*.js";

            saveFile1.Filter = "HTML Files|*.html|PHP Files|*.php|JS Files|*.js|PAWN Files|*.pwn|Text Files|*.txt|All Files|*.*";

            if (saveFile1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFile1.FileName.Length > 0)
            {
                richTextBox1.SaveFile(saveFile1.FileName, RichTextBoxStreamType.PlainText);
                Text = "CodeIT - " + saveFile1.FileName;
            }
        }

        private void richTextBox1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Z && (e.Control))
            {
                richTextBox1.Text = undoList.Pop();
            }
            if (e.KeyCode == Keys.Enter)
            {
                var strIP = new StringBuilder();
                strIP.Append(richTextBox2.Text);
                strIP.Append((startLines + 1) + "\n");

                richTextBox2.Text = strIP.ToString();
                startLines = startLines + 1;
            }
            if (e.KeyCode == Keys.Back)
            {
                richTextBox2.Text = "";
                startLines = richTextBox1.Lines.Count();
                var lineCounter = new StringBuilder();
                for (int i = 1; i <= startLines; i++)
                {
                    lineCounter.Append(richTextBox2.Text);
                    lineCounter.Append(i + "\n");
                }
                richTextBox2.Text = lineCounter.ToString();
            }
            if (e.KeyData == Keys.Tab)
            {
                var strIP = new StringBuilder();
                strIP.Append(richTextBox1.Text);
                strIP.Append(Indent(4));
                richTextBox1.Text = strIP.ToString();
                richTextBox1.Refresh();
            }
            if (e.KeyData == Keys.Shift && e.KeyData == Keys.Tab)
            {
                var strIP = new StringBuilder();
                strIP.Append(richTextBox1.Text);
                strIP.Append(Indent(-4));
                richTextBox1.Text = strIP.ToString();
                richTextBox1.Refresh();
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            undoList.Push(richTextBox1.Text);

            Focus();
            int selPos = richTextBox1.SelectionStart;
            foreach (Match keyWordMatch in htmlRegex.Matches(richTextBox1.Text))
            {
                richTextBox1.Select(keyWordMatch.Index, keyWordMatch.Length);
                richTextBox1.SelectionColor = Color.FromName(TagHighlight(Properties.Settings.Default["Theme"].ToString()));
                richTextBox1.SelectionStart = selPos;
                richTextBox1.SelectionColor = Color.FromName(TextHighlight(Properties.Settings.Default["Theme"].ToString()));
            }
            richTextBox1.DeselectAll();
            richTextBox1.Focus();
        }

        private void whiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Theme"] = "Light";
            Properties.Settings.Default.Save();

            ChooseTemplate("Light");
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Theme"] = "Dark";
            Properties.Settings.Default.Save();

            ChooseTemplate("Dark");
        }

        private void orangeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Theme"] = "Orange";
            Properties.Settings.Default.Save();

            ChooseTemplate("Orange");
        }

        private void hTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Language"] = "HTML";
            Properties.Settings.Default.Save();
        }

        private void pHPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Language"] = "PHP";
            Properties.Settings.Default.Save();
        }

        private void pAWNToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Language"] = "PAWN";
            Properties.Settings.Default.Save();
        }

        private void jAVASCRIPTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["Language"] = "JAVASCRIPT";
            Properties.Settings.Default.Save();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMyFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile1 = new OpenFileDialog();

            openFile1.Filter = "HTML Files|*.html|PHP Files|*.php|JS Files|*.js|PAWN Files|*.pwn|Text Files|*.txt|All Files|*.*";

            if (openFile1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.LoadFile(openFile1.FileName,
                RichTextBoxStreamType.PlainText);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BeginInvoke(new EventHandler(delegate
            {
                richTextBox1.AutoWordSelection = false;
            }));
        }
    }
}
