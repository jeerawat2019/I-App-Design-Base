using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCore.Comp.ScanSystem
{
    public partial class SLStatusCtrl : UserControl
    {
        private SLLaserDesk _laserDesk = null;

        public SLStatusCtrl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Bind 
        /// </summary>
        /// <param name="axis"></param>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public SLLaserDesk Bind
        {
            get { return _laserDesk; }
            set
            {
                _laserDesk = value;


                mcb_WND_OPEN.BindTwoWay(() => _laserDesk.RM_STATE_WND_OPEN);
                mcb_RTC_INIT.BindTwoWay(() => _laserDesk.RM_STATE_RTC_INIT);
                mcb_LAS_INIT.BindTwoWay(() => _laserDesk.RM_STATE_LAS_INIT);
                mcb_MOT_INIT.BindTwoWay(() => _laserDesk.RM_STATE_MOT_INIT);
                mcb_ALL_INIT.BindTwoWay(() => _laserDesk.RM_STATE_ALL_INIT);
                mcb_READY.BindTwoWay(() => _laserDesk.RM_STATE_READY);
                mcb_AUTOMODE.BindTwoWay(() => _laserDesk.RM_STATE_AUTOMODE);
                mcb_LST_EXEC.BindTwoWay(() => _laserDesk.RM_STATE_LST_EXEC);
                mcb_LST_EXE_ERR.BindTwoWay(() => _laserDesk.RM_STATE_LST_EXE_ERR);
                mcb_RM_MODE.BindTwoWay(() => _laserDesk.RM_STATE_RM_MODE);
                mcb_JOB_LOAD.BindTwoWay(() => _laserDesk.RM_STATE_JOB_LOAD);
                mcb_LST_CALC.BindTwoWay(() => _laserDesk.RM_STATE_LST_CALC);
                mcb_CMD_ERR.BindTwoWay(() => _laserDesk.RM_STATE_CMD_ERR);
                mcb_LAS_ERR.BindTwoWay(() => _laserDesk.RM_STATE_LAS_ERR);
                mcb_LAS_ON.BindTwoWay(() => _laserDesk.RM_STATE_LAS_ON);
                mcb_DEV_ERR.BindTwoWay(() => _laserDesk.RM_STATE_DEV_ERR);
                mcb_HEAD_OK.BindTwoWay(() => _laserDesk.RM_STATE_HEAD_OK);
                mcb_EXEC_DONE.BindTwoWay(() => _laserDesk.RM_STATE_EXEC_DONE);
                mcb_PILOT_MODE.BindTwoWay(() => _laserDesk.RM_STATE_PILOT_MODE);
                mcb_JOB_ABORTED.BindTwoWay(() => _laserDesk.RM_STATE_JOB_ABORTED);
                mcb_SWITCH_AUTOMODE.BindTwoWay(() => _laserDesk.RM_STATE_SWITCH_AUTOMODE);
            }
        }

    }
}
