using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

using MCore;
using MCore.Comp;
using MCore.Comp.IOSystem;
using MCore.Comp.IOSystem.Input;
using MCore.Comp.IOSystem.Output;

namespace AppMachine.Comp.IO
{
    public class AppIOCollection:CompBase
    {
        #region Standard Pattern
        public static AppIOCollection This;
        private Inputs _inputs = null;
        private Outputs _outputs = null;

        [XmlIgnore]
        public Dictionary<string, BoolInput> BoolInputCollection = new Dictionary<string, BoolInput>();

        [XmlIgnore]
        public Dictionary<string, BoolOutput> BoolOutputCollection = new Dictionary<string, BoolOutput>();
        #endregion

        //Add More Application Requirement in Below

        #region Constructor
        public AppIOCollection()
        {

        }

        public AppIOCollection(string name):
            base(name)
        {
            //Add More Application Requirement in Below
        }
        #endregion

        public override void Initialize()
        {
            #region Standard Pattern
            base.Initialize();
            This = this;

            Inputs bInputs = U.GetComponent(AppConstStaticName.INPUTS) as Inputs;
            if (bInputs.ChildArray != null)
            {
                bInputs.SortChildren((x, y) => ((x as IOBoolChannel).Channel.CompareTo((y as IOBoolChannel).Channel)));
                foreach (BoolInput bInput in bInputs.ChildArray)
                {
                    BoolInputCollection.Add(bInput.Name, bInput);
                }
            }

           
            Outputs bOutputs = U.GetComponent(AppConstStaticName.OUTPUTS) as Outputs;
            if (bOutputs.ChildArray != null)
            {
                bOutputs.SortChildren((x, y) => ((x as IOBoolChannel).Channel.CompareTo((y as IOBoolChannel).Channel)));
                foreach (BoolOutput bOutput in bOutputs.ChildArray)
                {
                    BoolOutputCollection.Add(bOutput.Name, bOutput);
                }
            }
            #endregion

            //Add More Application Requirement in Below
        }

        //Add More Application Requirement in Below
    }
}
