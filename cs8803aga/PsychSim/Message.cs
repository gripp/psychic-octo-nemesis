using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGAI
{
    enum Message
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
