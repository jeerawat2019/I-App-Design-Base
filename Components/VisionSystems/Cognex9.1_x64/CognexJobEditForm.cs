using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Cognex.VisionPro.ToolGroup;
using Cognex.VisionPro.ToolBlock;
using Cognex.VisionPro;


namespace MCore.Comp.VisionSystem
{
    /// <summary>
    /// Form for editing cognex vision job
    /// </summary>
    public partial class CognexJobEditForm : Form
    {
        //private CogToolGroupEditV2 _toolGroupEdit; // Use in Vision Pro 6
        private CogToolGroupEditV2 _toolGroupEdit;
        private CognexJob9 _cognexJob;
        private MethodInvoker _delPrevHelp = null;
        public MethodInvoker DelPrevHelp
        {
            get { return _delPrevHelp; }
            set
            {
                _delPrevHelp = value;
                btnPrevHelp.Show();
            }
        }
        private MethodInvoker _delNextHelp = null;
        public MethodInvoker DelNextHelp
        {
            get { return _delNextHelp; }
            set
            {
                _delNextHelp = value;
                btnNextHelp.Show();
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="cognexVisionJob"></param>
        public CognexJobEditForm(CognexJob9 cognexJob)
        {
            _cognexJob = cognexJob;
            InitializeComponent();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_toolGroupEdit != null)
                {
                    _toolGroupEdit.Subject = null;
                    _toolGroupEdit = null;
                } 
                _cognexJob = null;

                if (components != null)
                {
                    components.Dispose(); 
                }
            }
            base.Dispose(disposing);
        }

        private void CognexJobEditForm_Load(object sender, EventArgs e)
        {
            // Create the edit control
            //_toolGroupEdit = new CogToolGroupEditV2(); // Use in Vision Pro 6
            _toolGroupEdit = new CogToolGroupEditV2();
            _toolGroupEdit.Dock = DockStyle.Fill;

            panelToolGroupEdit.Controls.Add(_toolGroupEdit);

            // Assign the tool to the edit control
            _toolGroupEdit.Subject = _cognexJob.CogToolGroup;
            if (_cognexJob.CogToolGroup.Tools == null || _cognexJob.CogToolGroup.Tools.Count == 0)
            {
                _toolGroupEdit.ResetText();

                CogToolBlock grabberBufferToolBlock = new CogToolBlock();
                grabberBufferToolBlock.Name = "Grabber Buffer";
                CogToolBlockTerminal outputTerminal = new CogToolBlockTerminal("OutputBuffer",typeof(ICogImage));
                grabberBufferToolBlock.Outputs.Add(outputTerminal);
                _cognexJob.CogToolGroup.Tools.Add(grabberBufferToolBlock);
            }
        }

        private void CognexJobEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show(
                "Save your changes?",
                "Save File", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (dr == DialogResult.Yes)
            {
                _cognexJob.SaveVisionFile();
            }

        }

        private void btnImportFromFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.DefaultExt = ".vpp";
            of.Filter = "VPP File|*.vpp";
           
            of.CheckFileExists = true;
            if (of.ShowDialog() == DialogResult.OK)
            {
                ImportToolsFromVppFile(of.FileName);
            }
        }

        private void ImportToolsFromVppFile(string filePath)
        {
            CogToolGroup newToolGroup = CogSerializer.LoadObjectFromFile(filePath) as CogToolGroup;

            if (newToolGroup != null)
            {
                // It is a tool group
                foreach (ICogTool cogTool in newToolGroup.Tools)
                {
                    _cognexJob.CogToolGroup.Tools.Add(cogTool);
                }
            }
            else
            {
                ICogTool cogTool = CogSerializer.LoadObjectFromFile(filePath) as ICogTool;
                if (cogTool != null)
                {
                    // It is a tool
                    _cognexJob.CogToolGroup.Tools.Add(cogTool);
                }
                else
                {
                    // It is even not a tool group, probably a job
                    //throw new VisionSystemException("Unknown file format. Currently we accept only toolgroup and tool file");
                }
            }
            _toolGroupEdit.Refresh();
        }

        private void btnPrevHelp_Click(object sender, EventArgs e)
        {
            DelPrevHelp();
        }

        private void btnNextHelp_Click(object sender, EventArgs e)
        {
            DelNextHelp();
        }

    }
}