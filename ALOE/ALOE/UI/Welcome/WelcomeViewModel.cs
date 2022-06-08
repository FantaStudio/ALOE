using ALOE.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALOE.UI.Welcome
{
    public class WelcomeViewModel
    {

        public WelcomeViewModel()
        {
            Onboardings = WelcomeHelper.boardings;
        }

        public List<Onboarding> Onboardings { get; set; }
    }
}
