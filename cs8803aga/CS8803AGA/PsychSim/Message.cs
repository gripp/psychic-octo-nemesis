using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace CS8803AGA.PsychSim
{
    public enum Message
    {
        askStatus,
        askFunding,
        askNoFunding,
        askSignature,
        askProject,
        tellNoFunding,
        tellFailure,
        giveSignature,
        giveFunding,
        fundProject,
        proposeproject,
        submitApplication,
        acceptApplication
    }
}
